// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading;

namespace Rx.WorkloadHarness
{
    /// <summary>
    /// A long-running host that runs a sustained Rx pipeline so live runtime telemetry can be collected
    /// with <c>dotnet-counters</c> / <c>dotnet-trace</c> / <c>dotnet-monitor</c> — the system-level view
    /// (steady-state GC, memory growth, thread-pool saturation, contention) that the microbenchmarks
    /// cannot show. Prints its PID and runs until Ctrl+C.
    /// </summary>
    /// <remarks>
    /// Usage: <c>dotnet run -c Release -f net10.0 -- [buffer|select|merge|groupby] [--rate-ms N]</c>.
    /// Then, in another shell: <c>dotnet-counters monitor -p &lt;pid&gt; System.Runtime</c>.
    /// </remarks>
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var workload = args.Length > 0 && !args[0].StartsWith("--", StringComparison.Ordinal)
                ? args[0].ToLowerInvariant()
                : "buffer";
            var rateMs = GetIntOption(args, "--rate-ms", 1);
            var period = TimeSpan.FromMilliseconds(rateMs);
            var seconds = GetIntOption(args, "--seconds", 0); // 0 = run until Ctrl+C; > 0 = auto-stop (handy for CI/smoke runs)

            var pid = Environment.ProcessId;
            Console.WriteLine($"Rx.WorkloadHarness  PID={pid}  workload='{workload}'  rate={rateMs}ms");
            Console.WriteLine($"  dotnet-counters monitor -p {pid} System.Runtime");
            Console.WriteLine($"  dotnet-trace collect -p {pid}");
            Console.WriteLine(seconds > 0 ? $"Auto-stops after {seconds}s (or press Ctrl+C)." : "Press Ctrl+C to stop.");

            var pipeline = BuildPipeline(workload, Observable.Interval(period));

            using var stop = new ManualResetEventSlim(false);
            Console.CancelKeyPress += (_, e) => { e.Cancel = true; stop.Set(); };
            using var autoStop = seconds > 0
                ? new Timer(_ => stop.Set(), null, TimeSpan.FromSeconds(seconds), Timeout.InfiniteTimeSpan)
                : null;

            var observed = 0L;
            using (pipeline.Subscribe(_ => Interlocked.Increment(ref observed)))
            {
                stop.Wait();
            }

            Console.WriteLine($"Stopped. Observed {Volatile.Read(ref observed)} notifications.");
        }

        // A handful of representative sustained pipelines. "buffer" allocates a list per batch, which is a
        // deliberately visible GC signal; the others exercise transform / multicast / grouping paths.
        private static IObservable<object> BuildPipeline(string workload, IObservable<long> source) => workload switch
        {
            "select" => source.Select(x => x + 1).Where(x => (x & 1L) == 0L).Select(x => (object)x),
            "merge" => Observable.Merge(source, source.Select(x => x * 2), source.Select(x => x * 3)).Select(x => (object)x),
            "groupby" => source.GroupBy(x => x % 8).SelectMany(g => g.Select(x => (object)x)),
            _ => source.Buffer(64).Select(batch => (object)batch),
        };

        private static int GetIntOption(IReadOnlyList<string> args, string name, int fallback)
        {
            for (var i = 0; i < args.Count - 1; i++)
            {
                if (string.Equals(args[i], name, StringComparison.OrdinalIgnoreCase) && int.TryParse(args[i + 1], out var value))
                {
                    return value;
                }
            }

            return fallback;
        }
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Usage: <c>dotnet run -c Release -f net10.0 -- [buffer|select|merge|groupby] [--rate-ms N] [--seconds N]</c>.
    /// <c>--seconds 0</c> (the default) runs until Ctrl+C.
    /// Then, in another shell: <c>dotnet-counters monitor -p &lt;pid&gt; System.Runtime</c>.
    /// </remarks>
    internal static class Program
    {
        private static int Main(string[] args)
        {
            var workload = args.Length > 0 && !args[0].StartsWith("--", StringComparison.Ordinal)
                ? args[0].ToLowerInvariant()
                : "buffer";
            if (!TryGetIntOption(args, "--rate-ms", 1, out var rateMs) || !TryGetIntOption(args, "--seconds", 0, out var seconds))
            {
                return 1;
            }

            if (rateMs < 1)
            {
                Console.Error.WriteLine("--rate-ms must be >= 1.");
                return 1;
            }

            if (seconds < 0)
            {
                Console.Error.WriteLine("--seconds must be >= 0 (0 = run until Ctrl+C).");
                return 1;
            }

            if (workload is not ("buffer" or "select" or "merge" or "groupby"))
            {
                Console.Error.WriteLine($"Unknown workload '{workload}'. Use buffer, select, merge or groupby.");
                return 1;
            }

            var period = TimeSpan.FromMilliseconds(rateMs);
            var pipeline = BuildPipeline(workload, Observable.Interval(period));

            var pid = Environment.ProcessId;
            Console.WriteLine($"Rx.WorkloadHarness  PID={pid}  workload='{workload}'  rate={rateMs}ms");
            Console.WriteLine($"  dotnet-counters monitor -p {pid} System.Runtime");
            Console.WriteLine($"  dotnet-trace collect -p {pid}");
            Console.WriteLine(seconds > 0 ? $"Auto-stops after {seconds}s (or press Ctrl+C)." : "Press Ctrl+C to stop.");

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
            return 0;
        }

        // A handful of representative sustained pipelines. "buffer" allocates a list per batch, which is a
        // deliberately visible GC signal; the others exercise transform / multicast / grouping paths.
        private static IObservable<object> BuildPipeline(string workload, IObservable<long> source) => workload switch
        {
            "select" => source.Select(x => x + 1).Where(x => (x & 1L) == 0L).Select(x => (object)x),
            "merge" => Observable.Merge(source, source.Select(x => x * 2), source.Select(x => x * 3)).Select(x => (object)x),
            "groupby" => source.GroupBy(x => x % 8).SelectMany(g => g.Select(x => (object)x)),
            "buffer" => source.Buffer(64).Select(batch => (object)batch),
            _ => throw new ArgumentOutOfRangeException(nameof(workload), workload, "Unknown workload."),
        };

        /// <summary>
        /// Reads an integer option. Returns false (and reports the problem) when the option is given without a value
        /// or with a value that is not an integer, rather than quietly using <paramref name="fallback"/>.
        /// </summary>
        private static bool TryGetIntOption(IReadOnlyList<string> args, string name, int fallback, out int value)
        {
            value = fallback;
            for (var i = 0; i < args.Count; i++)
            {
                if (!string.Equals(args[i], name, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (i + 1 < args.Count && int.TryParse(args[i + 1], NumberStyles.Integer, CultureInfo.InvariantCulture, out value))
                {
                    return true;
                }

                Console.Error.WriteLine($"{name} needs an integer value; got '{(i + 1 < args.Count ? args[i + 1] : "")}'.");
                return false;
            }

            return true;
        }
    }
}

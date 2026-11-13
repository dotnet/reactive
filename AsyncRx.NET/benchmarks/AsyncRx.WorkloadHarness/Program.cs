// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncRx.WorkloadHarness
{
    /// <summary>
    /// A long-running host that runs a sustained AsyncRx pipeline so live runtime telemetry can be collected with
    /// <c>dotnet-counters</c> / <c>dotnet-trace</c> / <c>dotnet-monitor</c>: steady-state GC, memory growth and
    /// thread-pool load (from <c>Interval</c> with <c>--rate-ms</c>, and from operators that hop schedulers).
    /// Microbenchmarks under-report that system-level cost. Prints its PID and runs until Ctrl+C.
    /// </summary>
    /// <remarks>
    /// Usage: <c>dotnet run -c Release -f net10.0 -- [buffer|select|merge|groupby] [--rate-ms N] [--seconds N]</c>.
    /// <c>--seconds 0</c> (the default) runs until Ctrl+C.
    /// <c>--rate-ms 0</c> (the default) runs a free-running producer that awaits each <c>OnNextAsync</c> into a
    /// subject, so the pipeline runs as fast as its backpressure allows. <c>--rate-ms N</c> (N &gt;= 1) uses
    /// <c>AsyncObservable.Interval</c> instead; on Windows, timer resolution makes intervals under ~15 ms
    /// inaccurate. Then, in another shell: <c>dotnet-counters monitor -p &lt;pid&gt; System.Runtime</c>.
    /// </remarks>
    internal static class Program
    {
        private static async Task<int> Main(string[] args)
        {
            var workload = args.Length > 0 && !args[0].StartsWith("--", StringComparison.Ordinal)
                ? args[0].ToLowerInvariant()
                : "buffer";
            if (!TryGetIntOption(args, "--rate-ms", 0, out var rateMs) || !TryGetIntOption(args, "--seconds", 0, out var seconds))
            {
                return 1;
            }

            if (rateMs < 0)
            {
                Console.Error.WriteLine("--rate-ms must be >= 0 (0 = free-running producer).");
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

            var pid = Environment.ProcessId;
            Console.WriteLine($"AsyncRx.WorkloadHarness  PID={pid}  workload='{workload}'  rate={(rateMs == 0 ? "free-running" : rateMs + "ms")}");
            Console.WriteLine($"  dotnet-counters monitor -p {pid} System.Runtime");
            Console.WriteLine($"  dotnet-trace collect -p {pid}");
            Console.WriteLine(seconds > 0 ? $"Auto-stops after {seconds}s (or press Ctrl+C)." : "Press Ctrl+C to stop.");

            using var stop = new CancellationTokenSource();
            Console.CancelKeyPress += (_, e) =>
            {
                e.Cancel = true;
                stop.Cancel();
            };
            if (seconds > 0)
            {
                stop.CancelAfter(TimeSpan.FromSeconds(seconds));
            }

            var observed = 0L;
            var subject = rateMs == 0 ? new SequentialSimpleAsyncSubject<long>() : null;
            IAsyncObservable<long> source = subject ?? AsyncObservable.Interval(TimeSpan.FromMilliseconds(rateMs));
            var subscription = await BuildPipeline(workload, source)
                .SubscribeAsync(_ => Interlocked.Increment(ref observed))
                .ConfigureAwait(false);

            if (subject != null)
            {
                // Free-running producer: each push is awaited, which is the AsyncRx backpressure contract.
                for (var i = 0L; !stop.IsCancellationRequested; i++)
                {
                    await subject.OnNextAsync(i).ConfigureAwait(false);
                }
            }
            else
            {
                try
                {
                    await Task.Delay(Timeout.Infinite, stop.Token).ConfigureAwait(false);
                }
                catch (OperationCanceledException)
                {
                }
            }

            await subscription.DisposeAsync().ConfigureAwait(false);
            Console.WriteLine($"Stopped. Observed {Interlocked.Read(ref observed)} notifications.");
            return 0;
        }

        // A handful of representative sustained pipelines. "buffer" allocates a list per batch, a deliberately
        // visible GC signal; "merge" and "groupby" take an AsyncGate per element (an uncontended
        // gate completes synchronously); "select" stays synchronous.
        private static IAsyncObservable<object> BuildPipeline(string workload, IAsyncObservable<long> source) => workload switch
        {
            "select" => source.Select(x => x + 1).Where(x => (x & 1L) == 0L).Select(x => (object)x),
            "merge" => AsyncObservable.Range(0, 3, ImmediateAsyncScheduler.Instance)
                .Select(k => source.Select(x => x * (k + 1)))
                .Merge()
                .Select(x => (object)x),
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

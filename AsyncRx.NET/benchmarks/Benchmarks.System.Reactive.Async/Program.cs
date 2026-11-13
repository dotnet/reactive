// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Running;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async
{
    internal class Program
    {
        private static async Task<int> Main(string[] args)
        {
            Console.WriteLine("Effective AsyncRx-version: " + typeof(AsyncObservable).Assembly.GetName().Version);

            if (args.Contains("--verify"))
            {
                return await SelfCheck.RunAsync().ConfigureAwait(false);
            }

            // Auto-discover every public, non-abstract class with [Benchmark] methods in this assembly.
            // Profiling / runtime selection is driven by CLI args (e.g. --filter, -f/--runtimes,
            // --profiler ETW|EP, --disasm, --threading), layered on top of the shared AsyncRxBenchmarkConfig.
            var summaries = BenchmarkSwitcher
                .FromAssembly(typeof(Program).Assembly)
                .Run(args, AsyncRxBenchmarkConfig.Create());

            // A non-zero exit code lets tools/capture-baseline.ps1 reject a run in which a benchmark failed
            // validation, did not build, or threw, rather than keep a baseline with missing rows.
            return summaries.Any(s => s.HasCriticalValidationErrors || s.Reports.Any(r => !r.Success)) ? 1 : 0;
        }
    }
}

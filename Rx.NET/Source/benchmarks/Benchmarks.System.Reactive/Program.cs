// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Linq;

using BenchmarkDotNet.Running;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Effective Rx-version: " + typeof(Observable).Assembly.GetName().Version);

            // Auto-discover every public, non-abstract class with [Benchmark] methods in this assembly.
            // Profiling / runtime selection is driven by CLI args (e.g. --filter, -f/--runtimes,
            // --profiler ETW|EP, --disasm), layered on top of the shared RxBenchmarkConfig.
            BenchmarkSwitcher
                .FromAssembly(typeof(Program).Assembly)
                .Run(args, RxBenchmarkConfig.Create());
        }
    }
}

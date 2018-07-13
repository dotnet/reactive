// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Linq;
using BenchmarkDotNet.Running;

namespace Benchmarks.System.Reactive
{
    internal class Program
    {
        private static void Main()
        {
            Console.WriteLine("Effective Rx-version: " + typeof(Observable).Assembly.GetName().Version);

            var switcher = new BenchmarkSwitcher(new[] {
                typeof(ZipBenchmark),
                typeof(CombineLatestBenchmark),
                typeof(SwitchBenchmark),
                typeof(BufferCountBenchmark),
                typeof(RangeBenchmark),
                typeof(ToObservableBenchmark),
                typeof(RepeatBenchmark),
                typeof(ComparisonBenchmark),
                typeof(ComparisonAsyncBenchmark),
                typeof(ScalarScheduleBenchmark),
                typeof(StableCompositeDisposableBenchmark),
                typeof(SubjectBenchmark),
                typeof(ComparisonAsyncBenchmark)
#if (CURRENT)
                ,typeof(AppendPrependBenchmark)
                ,typeof(PrependVsStartWtihBenchmark)
#endif
            });

            switcher.Run();
            Console.ReadLine();
        }
    }
}

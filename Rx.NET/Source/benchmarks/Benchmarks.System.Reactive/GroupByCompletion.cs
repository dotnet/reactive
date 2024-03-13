// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

namespace Benchmarks.System.Reactive
{
    /// <summary>
    /// Completion of a wide fan-out/in scenario.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This was added to address https://github.com/dotnet/reactive/issues/2005 in which completion
    /// takes longer and longer to handle as the number of groups increases.
    /// </para>
    /// <para>
    /// The queries in this benchmark represent the common 'fan out/in' pattern in Rx. It is often
    /// useful to split a stream into groups to enable per-group processing, and then to recombine
    /// the data back into a single stream. These benchmarks don't do any per-group processing, so
    /// they might look pointless, but we're trying to measure the minimum unavoidable overhead
    /// that any code using this technique will encounter.
    /// </para>
    /// </remarks>
    [MemoryDiagnoser]
    public class GroupByCompletion
    {
        private IObservable<int> observable;

        [Params(200_000, 1_000_000)]
        public int NumberOfSamples { get; set; }

        [Params(10, 100, 1_000, 10_000, 100_000, 150_000, 200_000)]
        public int NumberOfGroups { get; set; }

        [GlobalSetup]
        public void GlobalSetup()
        {
            var data = new int[NumberOfSamples];
            for (var i = 0; i < data.Length; ++i)
            {
                data[i] = i;
            }

            observable = data.ToObservable();
        }

        [Benchmark]
        public void GroupBySelectMany()
        {
            var numberOfGroups = NumberOfGroups;

            observable!.GroupBy(value => value % numberOfGroups)
                .SelectMany(groupOfInts => groupOfInts)
                .Subscribe(intValue => { });
        }

        [Benchmark]
        public void GroupByMerge()
        {
            var numberOfGroups = NumberOfGroups;

            observable!.GroupBy(value => value % numberOfGroups)
                .Merge()
                .Subscribe(intValue => { });
        }
    }
}

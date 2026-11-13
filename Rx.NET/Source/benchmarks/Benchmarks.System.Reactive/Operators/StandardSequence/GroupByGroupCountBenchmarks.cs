// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.StandardSequence
{
    /// <summary>
    /// S1: the fan-out/fan-in shape (<c>GroupBy</c>, then <c>SelectMany</c> or <c>Merge</c>) with a fixed element count
    /// and a sweep over the number of groups. This is the regression guard for
    /// <see href="https://github.com/dotnet/reactive/issues/2005">issue #2005</see>, where completion took longer and
    /// longer as the number of groups grew. <see cref="GroupByBenchmarks"/> uses 8 groups, so it cannot show that.
    /// </summary>
    /// <remarks>
    /// <para>
    /// It replaces the earlier <c>GroupByCompletion</c> benchmark. N is fixed at 200,000 (the smaller of that
    /// benchmark's two sample counts) so that <c>ns/N</c> compares directly across the <c>Groups</c> values: if the
    /// cost per element grows with <c>Groups</c>, completion is no longer linear in the number of groups.
    /// </para>
    /// <para>
    /// The source is a pre-built <see cref="ArraySource{T}"/>, not <c>Observable.Range</c>, so the per-element cost of
    /// the default scheduler does not hide the cost of the groups.
    /// </para>
    /// </remarks>
    [BenchmarkCategory("StandardSequence")]
    public class GroupByGroupCountBenchmarks : BenchmarkBase
    {
        private IObservable<int> _source = null!;
        private Func<int, int> _keySelector = null!;

        [Params(200_000)]
        public int N;

        [Params(10, 1_000, 100_000, 200_000)]
        public int Groups;

        [GlobalSetup]
        public void Setup()
        {
            var data = new int[N];
            for (var i = 0; i < data.Length; i++)
            {
                data[i] = i;
            }

            _source = new ArraySource<int>(data);

            // Built once, so the measured region allocates no closure or delegate for the key selector.
            var groups = Groups;
            _keySelector = v => v % groups;
        }

        [Benchmark]
        public void GroupBy_SelectMany()
        {
            _source.GroupBy(_keySelector).SelectMany(static g => g).SubscribeConsume(Consumer);
        }

        [Benchmark]
        public void GroupBy_Merge()
        {
            _source.GroupBy(_keySelector).Merge().SubscribeConsume(Consumer);
        }
    }
}

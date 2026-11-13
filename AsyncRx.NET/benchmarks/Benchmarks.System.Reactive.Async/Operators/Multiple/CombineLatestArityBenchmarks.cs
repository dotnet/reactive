// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.Multiple
{
    /// <summary>
    /// S1 (gated sweep): the generated higher-arity <c>CombineLatest</c> overloads (3, 4, 8 and 15 sources, tuple
    /// results), each source a cold source of N. Shows how per-element cost scales with the number of sources.
    /// </summary>
    [BenchmarkCategory("Multiple")]
    public class CombineLatestArityBenchmarks : GatedOperatorBenchmarkBase
    {
        [Benchmark]
        public ValueTask CombineLatest_3()
        {
            var s = BenchmarkSources.Range(N);
            return s.CombineLatest(s, s).SubscribeConsumeAsync(Consumer);
        }

        [Benchmark]
        public ValueTask CombineLatest_4()
        {
            var s = BenchmarkSources.Range(N);
            return s.CombineLatest(s, s, s).SubscribeConsumeAsync(Consumer);
        }

        [Benchmark]
        public ValueTask CombineLatest_8()
        {
            var s = BenchmarkSources.Range(N);
            return s.CombineLatest(s, s, s, s, s, s, s).SubscribeConsumeAsync(Consumer);
        }

        [Benchmark]
        public ValueTask CombineLatest_15()
        {
            var s = BenchmarkSources.Range(N);
            return s.CombineLatest(s, s, s, s, s, s, s, s, s, s, s, s, s, s).SubscribeConsumeAsync(Consumer);
        }
    }
}

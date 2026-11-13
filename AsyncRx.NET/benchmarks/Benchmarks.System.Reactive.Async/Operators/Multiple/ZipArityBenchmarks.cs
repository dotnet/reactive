// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.Multiple
{
    /// <summary>
    /// S1 (gated sweep): the generated higher-arity <c>Zip</c> overloads (3, 4, 8 and 15 sources, tuple results),
    /// each source a cold source of N. Shows how the per-element cost scales with the number of sources. Each
    /// source but the last pushes all N into its queue before the next is subscribed; the last one drains them
    /// and the sequence completes (see <see cref="ZipBenchmarks"/>).
    /// </summary>
    [BenchmarkCategory("Multiple")]
    public class ZipArityBenchmarks : GatedOperatorBenchmarkBase
    {
        [Benchmark]
        public ValueTask Zip_3()
        {
            var s = BenchmarkSources.Range(N);
            return s.Zip(s, s).SubscribeConsumeAsync(Consumer);
        }

        [Benchmark]
        public ValueTask Zip_4()
        {
            var s = BenchmarkSources.Range(N);
            return s.Zip(s, s, s).SubscribeConsumeAsync(Consumer);
        }

        [Benchmark]
        public ValueTask Zip_8()
        {
            var s = BenchmarkSources.Range(N);
            return s.Zip(s, s, s, s, s, s, s).SubscribeConsumeAsync(Consumer);
        }

        [Benchmark]
        public ValueTask Zip_15()
        {
            var s = BenchmarkSources.Range(N);
            return s.Zip(s, s, s, s, s, s, s, s, s, s, s, s, s, s).SubscribeConsumeAsync(Consumer);
        }
    }
}

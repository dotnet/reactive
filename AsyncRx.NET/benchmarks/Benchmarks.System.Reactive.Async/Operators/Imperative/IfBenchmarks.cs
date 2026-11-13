// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.Imperative
{
    /// <summary>
    /// S1 (variant sweep): <c>If</c> choosing between a source of N and an empty source, with synchronous and
    /// <c>ValueTask</c> conditions. The branches are built in <c>[GlobalSetup]</c>.
    /// </summary>
    [BenchmarkCategory("Imperative")]
    public class IfBenchmarks : OperatorVariantBenchmarkBase
    {
        private IAsyncObservable<int> _then = default!;
        private IAsyncObservable<int> _else = default!;

        [GlobalSetup]
        public void Setup()
        {
            _then = BenchmarkSources.Range(N);
            _else = AsyncObservable.Empty<int>();
        }

        [Benchmark]
        public ValueTask If_Then() =>
            AsyncObservable.If(static () => true, _then, _else).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask If_Else() =>
            AsyncObservable.If(static () => false, _else, _then).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask If_Then_Async() =>
            AsyncObservable.If(static () => new ValueTask<bool>(true), _then, _else).SubscribeConsumeAsync(Consumer);
    }
}

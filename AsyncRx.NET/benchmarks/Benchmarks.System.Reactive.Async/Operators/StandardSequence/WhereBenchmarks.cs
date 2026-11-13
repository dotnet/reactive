// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.StandardSequence
{
    /// <summary>
    /// S1: <c>Where</c> keeping every other element, with the synchronous predicate and the
    /// <c>ValueTask</c>-predicate overload.
    /// </summary>
    [BenchmarkCategory("StandardSequence")]
    public class WhereBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public ValueTask Where() =>
            BenchmarkSources.Range(N).Where(static v => (v & 1) == 0).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Where_Async() =>
            BenchmarkSources.Range(N).Where(static v => new ValueTask<bool>((v & 1) == 0)).SubscribeConsumeAsync(Consumer);
    }
}

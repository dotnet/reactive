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
    /// S1 (gated sweep): <c>Amb</c> where the first source wins (it emits N elements while the others never emit),
    /// for the binary form and the n-ary <c>Amb(params)</c> form over three sources. Amb takes an
    /// <c>AsyncGate</c> per element. <c>Amb(IEnumerable)</c> forwards to the n-ary form and is not measured
    /// separately.
    /// </summary>
    [BenchmarkCategory("Multiple")]
    public class AmbBenchmarks : GatedOperatorBenchmarkBase
    {
        [Benchmark]
        public ValueTask Amb_Binary() =>
            BenchmarkSources.Range(N).Amb(AsyncObservable.Never<int>()).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Amb_Params() =>
            AsyncObservable.Amb(BenchmarkSources.Range(N), AsyncObservable.Never<int>(), AsyncObservable.Never<int>()).SubscribeConsumeAsync(Consumer);
    }
}

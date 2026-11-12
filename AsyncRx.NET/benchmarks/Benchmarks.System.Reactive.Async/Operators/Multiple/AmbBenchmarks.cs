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
    /// for the binary form. Amb takes an <c>AsyncGate</c> per element.
    /// </summary>
    /// <remarks>
    /// KNOWN GAP: <c>Amb(params)</c> and <c>Amb(IEnumerable)</c> throw <c>IndexOutOfRangeException</c>: the
    /// <c>ContinueWith</c> that assigns each subscription captures the loop variable, which equals the source
    /// count by the time the continuation runs. Restore the commented-out row when the library is fixed.
    /// </remarks>
    [BenchmarkCategory("Multiple")]
    public class AmbBenchmarks : GatedOperatorBenchmarkBase
    {
        [Benchmark]
        public ValueTask Amb_Binary() =>
            BenchmarkSources.Range(N).Amb(AsyncObservable.Never<int>()).SubscribeConsumeAsync(Consumer);

        // KNOWN GAP (see remarks):
        // [Benchmark]
        // public ValueTask Amb_Params() =>
        //     AsyncObservable.Amb(BenchmarkSources.Range(N), AsyncObservable.Never<int>(), AsyncObservable.Never<int>()).SubscribeConsumeAsync(Consumer);
    }
}

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
    /// Hot (gated sweep): the n-ary <c>Zip(params sources)</c>, which emits an <c>IList&lt;T&gt;</c> per step, over
    /// 2 and 8 <see cref="PushSource{T}"/>s pushed round-robin, N values each, then disposed.
    /// </summary>
    /// <remarks>
    /// KNOWN GAP: over cold sources <c>Zip(params)</c> completes as soon as one source has completed and drained,
    /// then the remaining sources keep calling the terminated observer, which throws "The observer has already
    /// terminated.". The sources are therefore never completed; these rows measure the per-element path.
    /// </remarks>
    [BenchmarkCategory("Multiple")]
    public class ZipManyBenchmarks : GatedOperatorBenchmarkBase
    {
        [Benchmark]
        public ValueTask Zip_Params_2()
        {
            var s = Pump.Sources(2);
            return AsyncObservable.Zip<int>(s[0], s[1]).PushAllAndDisposeAsync(s, N, interleaved: true, Consumer);
        }

        [Benchmark]
        public ValueTask Zip_Params_8()
        {
            var s = Pump.Sources(8);
            return AsyncObservable.Zip<int>(s[0], s[1], s[2], s[3], s[4], s[5], s[6], s[7]).PushAllAndDisposeAsync(s, N, interleaved: true, Consumer);
        }
    }
}

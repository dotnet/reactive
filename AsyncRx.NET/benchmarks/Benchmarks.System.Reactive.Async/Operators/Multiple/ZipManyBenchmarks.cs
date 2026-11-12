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
    /// S1 (gated sweep): the n-ary <c>Zip(params sources)</c>, which emits an <c>IList&lt;T&gt;</c> per step, over
    /// 2 and 8 cold sources of N.
    /// </summary>
    /// <remarks>
    /// <c>Zip(params)</c> completes as soon as one source has completed and drained, which happens on the last
    /// source's last element. Later notifications reach a terminated observer and are dropped.
    /// </remarks>
    [BenchmarkCategory("Multiple")]
    public class ZipManyBenchmarks : GatedOperatorBenchmarkBase
    {
        [Benchmark]
        public ValueTask Zip_Params_2()
        {
            var s = BenchmarkSources.Range(N);
            return AsyncObservable.Zip<int>(s, s).SubscribeConsumeAsync(Consumer);
        }

        [Benchmark]
        public ValueTask Zip_Params_8()
        {
            var s = BenchmarkSources.Range(N);
            return AsyncObservable.Zip<int>(s, s, s, s, s, s, s, s).SubscribeConsumeAsync(Consumer);
        }
    }
}

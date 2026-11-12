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
    /// Hot (gated sweep): binary <c>Zip</c> (tuple result, synchronous selector and <c>ValueTask</c> selector). The
    /// first <see cref="PushSource{T}"/> receives all N before the second starts, so the pairing queue grows to N
    /// (the shape two cold sources would produce); the subscription is then disposed. Zip takes an
    /// <c>AsyncGate</c> per element.
    /// </summary>
    /// <remarks>
    /// KNOWN GAP: the generated <c>Zip</c> overloads create their observers with 1-based indices against a
    /// 0-based <c>isDone</c> array, so completing the last source throws <c>IndexOutOfRangeException</c> (and over
    /// cold sources the first failure surfaces as "The observer has already terminated."). The sources are
    /// therefore never completed; these rows measure the per-element pairing path, not completion.
    /// </remarks>
    [BenchmarkCategory("Multiple")]
    public class ZipBenchmarks : GatedOperatorBenchmarkBase
    {
        [Benchmark]
        public ValueTask Zip_Tuple()
        {
            var s = Pump.Sources(2);
            return s[0].Zip(s[1]).PushAllAndDisposeAsync(s, N, interleaved: false, Consumer);
        }

        [Benchmark]
        public ValueTask Zip_Selector()
        {
            var s = Pump.Sources(2);
            return s[0].Zip(s[1], static (a, b) => a + b).PushAllAndDisposeAsync(s, N, interleaved: false, Consumer);
        }

        [Benchmark]
        public ValueTask Zip_Selector_Async()
        {
            var s = Pump.Sources(2);
            return s[0].Zip(s[1], static (a, b) => new ValueTask<int>(a + b)).PushAllAndDisposeAsync(s, N, interleaved: false, Consumer);
        }
    }
}

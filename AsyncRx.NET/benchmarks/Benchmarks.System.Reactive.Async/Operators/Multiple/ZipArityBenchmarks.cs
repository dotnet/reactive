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
    /// Hot (gated sweep): the generated higher-arity <c>Zip</c> overloads (3, 4, 8 and 15 sources, tuple results)
    /// over <see cref="PushSource{T}"/>s pushed round-robin, N values each, then disposed. Shows how the
    /// per-element cost scales with the number of sources. The sources are never completed: see the
    /// <see cref="ZipBenchmarks"/> known gap.
    /// </summary>
    [BenchmarkCategory("Multiple")]
    public class ZipArityBenchmarks : GatedOperatorBenchmarkBase
    {
        [Benchmark]
        public ValueTask Zip_3()
        {
            var s = Pump.Sources(3);
            return s[0].Zip(s[1], s[2]).PushAllAndDisposeAsync(s, N, interleaved: true, Consumer);
        }

        [Benchmark]
        public ValueTask Zip_4()
        {
            var s = Pump.Sources(4);
            return s[0].Zip(s[1], s[2], s[3]).PushAllAndDisposeAsync(s, N, interleaved: true, Consumer);
        }

        [Benchmark]
        public ValueTask Zip_8()
        {
            var s = Pump.Sources(8);
            return s[0].Zip(s[1], s[2], s[3], s[4], s[5], s[6], s[7]).PushAllAndDisposeAsync(s, N, interleaved: true, Consumer);
        }

        [Benchmark]
        public ValueTask Zip_15()
        {
            var s = Pump.Sources(15);
            return s[0].Zip(s[1], s[2], s[3], s[4], s[5], s[6], s[7], s[8], s[9], s[10], s[11], s[12], s[13], s[14])
                .PushAllAndDisposeAsync(s, N, interleaved: true, Consumer);
        }
    }
}

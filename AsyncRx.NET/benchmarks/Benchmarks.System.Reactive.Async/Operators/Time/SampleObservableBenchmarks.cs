// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.Time
{
    /// <summary>
    /// Hot: <c>Sample(sampler)</c> where both the source and the sampler are <see cref="PushSource{T}"/>s; the
    /// sampler fires after every fourth element. No virtual time is needed. AsyncRx completes the output at the
    /// first sample after the source completes, so the benchmark fires one final sample.
    /// </summary>
    [BenchmarkCategory("Time")]
    public class SampleObservableBenchmarks
    {
        [Params(1_000, 10_000, 100_000)]
        public int N;

        private readonly Consumer _consumer = new();

        [Benchmark]
        public async ValueTask Sample_Observable()
        {
            var source = new PushSource<int>();
            var sampler = new PushSource<int>();
            var sink = Sinks.For<int>(_consumer);
            await source.Sample(sampler).SubscribeAsync(sink).ConfigureAwait(false);

            for (var i = 0; i < N; i++)
            {
                await source.OnNextAsync(i).ConfigureAwait(false);
                if ((i & 3) == 3)
                {
                    await sampler.OnNextAsync(i).ConfigureAwait(false);
                }
            }

            // AsyncRx Sample(sampler) completes at the next sample after the source completes (unlike Rx, where
            // source completion ends the output), so one final tick is needed.
            await source.OnCompletedAsync().ConfigureAwait(false);
            if (!sink.Completion.IsSet)
            {
                await sampler.OnNextAsync(-1).ConfigureAwait(false);
            }

            await sink.Completion.WaitAsync().ConfigureAwait(false);
        }
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.Time
{
    /// <summary>
    /// Hot: <c>Buffer</c> and <c>Window</c> with an openings observable and a closing selector. Three
    /// <see cref="PushSource{T}"/> instances drive them: the source, the openings (pushed before every 16th
    /// element, from element 0) and the closing (pushed after the 16th element of each window). The windows do not
    /// overlap, so one closing source serves every window: each opening subscribes the closing selector's result
    /// again. Windows are flattened through <c>Merge</c>; <c>Buffer</c> is built by the library as
    /// <c>Window(...).SelectMany(w =&gt; w.ToList())</c>.
    /// </summary>
    /// <remarks>
    /// The library defines both operators as a <c>GroupJoin</c> of the openings (left) with the source (right),
    /// which completes when the <i>openings</i> complete and does not complete the open windows. So the benchmark
    /// closes the last window, if it is still open, then completes the source and the openings.
    /// </remarks>
    [BenchmarkCategory("Time")]
    public class BufferWindowOpeningsBenchmarks
    {
        [Params(1_000, 10_000, 100_000)]
        public int N;

        private readonly Consumer _consumer = new();

        [Benchmark]
        public ValueTask Buffer_Openings_ClosingSelector()
        {
            var source = new PushSource<int>();
            var openings = new PushSource<int>();
            var closing = new PushSource<int>();
            return RunAsync(source.Buffer(openings, _ => closing), source, openings, closing);
        }

        [Benchmark]
        public ValueTask Window_Openings_ClosingSelector()
        {
            var source = new PushSource<int>();
            var openings = new PushSource<int>();
            var closing = new PushSource<int>();
            return RunAsync(source.Window(openings, _ => closing).Merge(), source, openings, closing);
        }

        private async ValueTask RunAsync<TResult>(IAsyncObservable<TResult> pipeline, PushSource<int> source, PushSource<int> openings, PushSource<int> closing)
        {
            var sink = Sinks.For<TResult>(_consumer);
            await pipeline.SubscribeAsync(sink).ConfigureAwait(false);

            for (var i = 0; i < N; i++)
            {
                if ((i & 15) == 0)
                {
                    await openings.OnNextAsync(i).ConfigureAwait(false);
                }

                await source.OnNextAsync(i).ConfigureAwait(false);

                if ((i & 15) == 15)
                {
                    await closing.OnNextAsync(i).ConfigureAwait(false);
                }
            }

            if (((N - 1) & 15) != 15)
            {
                // The last window is still open.
                await closing.OnNextAsync(-1).ConfigureAwait(false);
            }

            await source.OnCompletedAsync().ConfigureAwait(false);
            await openings.OnCompletedAsync().ConfigureAwait(false);
            await sink.Completion.WaitAsync().ConfigureAwait(false);
        }
    }
}

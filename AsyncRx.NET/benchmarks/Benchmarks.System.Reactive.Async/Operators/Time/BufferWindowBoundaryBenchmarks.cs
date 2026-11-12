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
    /// Hot: <c>Buffer</c> and <c>Window</c> closed by a second observable rather than by time or count. The boundary
    /// <see cref="PushSource{T}"/> fires after every 16th element. The closing-selector forms resubscribe to the
    /// boundary source for every buffer or window. Windows are flattened through <c>Merge</c>.
    /// </summary>
    [BenchmarkCategory("Time")]
    public class BufferWindowBoundaryBenchmarks
    {
        [Params(1_000, 10_000, 100_000)]
        public int N;

        private readonly Consumer _consumer = new();

        [Benchmark]
        public ValueTask Buffer_Boundaries()
        {
            var source = new PushSource<int>();
            var boundary = new PushSource<int>();
            return RunAsync(source.Buffer(boundary), source, boundary);
        }

        [Benchmark]
        public ValueTask Buffer_ClosingSelector()
        {
            var source = new PushSource<int>();
            var boundary = new PushSource<int>();
            return RunAsync(source.Buffer(() => boundary), source, boundary);
        }

        [Benchmark]
        public ValueTask Window_Boundaries()
        {
            var source = new PushSource<int>();
            var boundary = new PushSource<int>();
            return RunAsync(source.Window(boundary).Merge(), source, boundary);
        }

        [Benchmark]
        public ValueTask Window_ClosingSelector()
        {
            var source = new PushSource<int>();
            var boundary = new PushSource<int>();
            return RunAsync(source.Window(() => boundary).Merge(), source, boundary);
        }

        private async ValueTask RunAsync<TResult>(IAsyncObservable<TResult> pipeline, PushSource<int> source, PushSource<int> boundary)
        {
            var sink = Sinks.For<TResult>(_consumer);
            await pipeline.SubscribeAsync(sink).ConfigureAwait(false);

            for (var i = 0; i < N; i++)
            {
                await source.OnNextAsync(i).ConfigureAwait(false);
                if ((i & 15) == 15)
                {
                    await boundary.OnNextAsync(i).ConfigureAwait(false);
                }
            }

            await source.OnCompletedAsync().ConfigureAwait(false);
            if (!sink.Completion.IsSet)
            {
                // In case the operator only closes on a boundary after the source completes.
                await boundary.OnNextAsync(-1).ConfigureAwait(false);
            }

            await sink.Completion.WaitAsync().ConfigureAwait(false);
        }
    }
}

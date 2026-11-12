// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.Multiple
{
    /// <summary>
    /// Hot: the combining operators with two <see cref="PushSource{T}"/>s pushed alternately (left, right, left,
    /// ...), N/2 pushes each. Cold sources run one after the other, so they never exercise the interleaved path
    /// (Zip pairing immediately, CombineLatest emitting on both sides). Each push is awaited, so the interleaving
    /// is deterministic without virtual time. <c>Zip</c> is disposed instead of completed (see the
    /// <see cref="ZipBenchmarks"/> known gap).
    /// </summary>
    [BenchmarkCategory("Multiple")]
    public class InterleavedCombiningBenchmarks
    {
        [Params(1_000, 10_000, 100_000)]
        public int N;

        private readonly Consumer _consumer = new();

        [Benchmark]
        public ValueTask Zip()
        {
            var s = Pump.Sources(2);
            return s[0].Zip(s[1], static (x, y) => x + y).PushAllAndDisposeAsync(s, N / 2, interleaved: true, _consumer);
        }

        [Benchmark]
        public ValueTask CombineLatest()
        {
            var a = new PushSource<int>();
            var b = new PushSource<int>();
            return RunAsync(a.CombineLatest(b, static (x, y) => x + y), a, b);
        }

        [Benchmark]
        public ValueTask WithLatestFrom()
        {
            var a = new PushSource<int>();
            var b = new PushSource<int>();
            return RunAsync(a.WithLatestFrom(b, static (x, y) => x + y), a, b);
        }

        [Benchmark]
        public ValueTask Merge()
        {
            var a = new PushSource<int>();
            var b = new PushSource<int>();
            return RunAsync(new ArrayAsyncSource<IAsyncObservable<int>>(new IAsyncObservable<int>[] { a, b }).Merge(), a, b);
        }

        [Benchmark]
        public ValueTask Amb()
        {
            var a = new PushSource<int>();
            var b = new PushSource<int>();
            return RunAsync(a.Amb(b), a, b);
        }

        private async ValueTask RunAsync(IAsyncObservable<int> pipeline, PushSource<int> a, PushSource<int> b)
        {
            var sink = Sinks.For<int>(_consumer);
            await pipeline.SubscribeAsync(sink).ConfigureAwait(false);

            var half = N / 2;
            for (var i = 0; i < half; i++)
            {
                await a.OnNextAsync(i).ConfigureAwait(false);
                await b.OnNextAsync(i).ConfigureAwait(false);
            }

            await a.OnCompletedAsync().ConfigureAwait(false);
            await b.OnCompletedAsync().ConfigureAwait(false);
            await sink.Completion.WaitAsync().ConfigureAwait(false);
        }
    }
}

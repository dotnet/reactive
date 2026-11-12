// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.Hot
{
    /// <summary>
    /// S2 (hot push): the benchmark awaits <c>OnNextAsync</c> for each element into a <see cref="PushSource{T}"/>
    /// that feeds the operator, the way a producer drives a hot AsyncRx pipeline. The single-observer source keeps
    /// subject cost out of the measurement (see <see cref="SubjectBenchmarks"/>). Select, Where and Scan stay
    /// synchronous; SelectMany, GroupBy and Merge take an <c>AsyncGate</c> per element.
    /// </summary>
    [BenchmarkCategory("Hot")]
    public class HotSourceBenchmarks
    {
        [Params(1_000, 10_000, 100_000)]
        public int N;

        private readonly Consumer _consumer = new();

        [Benchmark(Baseline = true)]
        public ValueTask Source_Baseline()
        {
            var source = new PushSource<int>();
            return source.RunHotAsync(source, N, _consumer);
        }

        [Benchmark]
        public ValueTask Select()
        {
            var source = new PushSource<int>();
            return source.Select(static v => v + 1).RunHotAsync(source, N, _consumer);
        }

        [Benchmark]
        public ValueTask Where()
        {
            var source = new PushSource<int>();
            return source.Where(static v => (v & 1) == 0).RunHotAsync(source, N, _consumer);
        }

        [Benchmark]
        public ValueTask Scan()
        {
            var source = new PushSource<int>();
            return source.Scan(0, static (acc, v) => acc + v).RunHotAsync(source, N, _consumer);
        }

        [Benchmark]
        public ValueTask SelectMany()
        {
            var source = new PushSource<int>();
            return source.SelectMany(static v => BenchmarkSources.Return(v)).RunHotAsync(source, N, _consumer);
        }

        [Benchmark]
        public ValueTask GroupBy_SelectMany()
        {
            var source = new PushSource<int>();
            return source.GroupBy(static v => v & 7).SelectMany(static g => g).RunHotAsync(source, N, _consumer);
        }

        [Benchmark]
        public ValueTask Merge_Two()
        {
            var a = new PushSource<int>();
            var b = new PushSource<int>();
            var merged = new ArrayAsyncSource<IAsyncObservable<int>>(new IAsyncObservable<int>[] { a, b }).Merge();
            return RunMergeAsync(merged, a, b);
        }

        private async ValueTask RunMergeAsync(IAsyncObservable<int> merged, PushSource<int> a, PushSource<int> b)
        {
            var sink = Sinks.For<int>(_consumer);
            await merged.SubscribeAsync(sink).ConfigureAwait(false);

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

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
    /// Hot (gated sweep): <c>SkipUntil(other)</c> and <c>TakeUntil(other)</c> driven by two
    /// <see cref="PushSource{T}"/>s: N/2 elements, then the trigger fires, then N/2 more. Hot sources make the
    /// trigger point deterministic. Both operators take an <c>AsyncGate</c> per element.
    /// </summary>
    [BenchmarkCategory("Multiple")]
    public class SkipUntilTakeUntilObservableBenchmarks : GatedOperatorBenchmarkBase
    {
        [Benchmark]
        public ValueTask SkipUntil()
        {
            var source = new PushSource<int>();
            var trigger = new PushSource<int>();
            return RunAsync(source.SkipUntil(trigger), source, trigger);
        }

        [Benchmark]
        public ValueTask TakeUntil()
        {
            var source = new PushSource<int>();
            var trigger = new PushSource<int>();
            return RunAsync(source.TakeUntil(trigger), source, trigger);
        }

        private async ValueTask RunAsync(IAsyncObservable<int> pipeline, PushSource<int> source, PushSource<int> trigger)
        {
            var sink = Sinks.For<int>(Consumer);
            await pipeline.SubscribeAsync(sink).ConfigureAwait(false);

            var half = N / 2;
            await source.PushAsync(0, half).ConfigureAwait(false);
            await trigger.OnNextAsync(0).ConfigureAwait(false);
            await source.PushAsync(half, N - half).ConfigureAwait(false);
            await source.OnCompletedAsync().ConfigureAwait(false);
            await sink.Completion.WaitAsync().ConfigureAwait(false);
        }
    }
}

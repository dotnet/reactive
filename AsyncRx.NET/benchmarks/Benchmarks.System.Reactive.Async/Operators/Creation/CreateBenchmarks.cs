// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.Creation
{
    /// <summary>
    /// S1: a hand-written source through <c>AsyncObservable.Create</c> with an inline awaited push loop, against
    /// <see cref="ArrayAsyncSource{T}"/> (a raw <c>IAsyncObservable</c> implementation with no AsyncRx wrapper) as
    /// the baseline. The difference is the <c>Create</c> wrapper: the auto-detach observer, which checks state on
    /// every call.
    /// </summary>
    [BenchmarkCategory("Creation")]
    public class CreateBenchmarks : OperatorBenchmarkBase
    {
        private ArrayAsyncSource<int> _raw = default!;
        private IAsyncObservable<int> _created = default!;

        [GlobalSetup]
        public void Setup()
        {
            var items = new int[N];
            for (var i = 0; i < items.Length; i++)
            {
                items[i] = i;
            }

            _raw = new ArrayAsyncSource<int>(items);

            var n = N;
            _created = AsyncObservable.Create<int>(async (observer, _) =>
            {
                for (var i = 0; i < n; i++)
                {
                    await observer.OnNextAsync(i).ConfigureAwait(false);
                }

                await observer.OnCompletedAsync().ConfigureAwait(false);
                return AsyncDisposable.Nop;
            });
        }

        [Benchmark(Baseline = true)]
        public ValueTask RawSource() => _raw.SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Create() => _created.SubscribeConsumeAsync(Consumer);
    }
}

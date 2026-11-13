// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;

namespace Benchmarks.System.Reactive.Async.Infrastructure
{
    /// <summary>
    /// A hot, single-observer source that the benchmark pushes into directly (S2 hot push, S3 temporal, S4
    /// lifecycle). It forwards each call to the one subscribed observer and allocates nothing: the source is its
    /// own subscription handle.
    /// </summary>
    /// <remarks>
    /// The library subjects copy their observer list on every <c>OnNextAsync</c> (<c>SimpleAsyncSubject</c>) or
    /// take an <c>AsyncGate</c> (Behavior/Replay). Pushing through this type keeps that cost out of operator
    /// benchmarks; <c>SubjectBenchmarks</c> measures the subjects themselves. A second subscription replaces the
    /// first, so use one instance per subscriber.
    /// </remarks>
    public sealed class PushSource<T> : IAsyncObservable<T>, IAsyncObserver<T>, IAsyncDisposable
    {
        private IAsyncObserver<T>? _observer;

        public ValueTask<IAsyncDisposable> SubscribeAsync(IAsyncObserver<T> observer)
        {
            _observer = observer ?? throw new ArgumentNullException(nameof(observer));
            return new ValueTask<IAsyncDisposable>(this);
        }

        public ValueTask OnNextAsync(T value) => _observer?.OnNextAsync(value) ?? default;

        public ValueTask OnErrorAsync(Exception error) => _observer?.OnErrorAsync(error) ?? default;

        public ValueTask OnCompletedAsync() => _observer?.OnCompletedAsync() ?? default;

        public ValueTask DisposeAsync()
        {
            _observer = null;
            return default;
        }
    }
}

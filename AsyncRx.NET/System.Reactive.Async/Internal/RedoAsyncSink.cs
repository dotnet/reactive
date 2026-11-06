// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Disposables;
using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    // The sink behind RepeatWhen and RetryWhen, a port of Rx.NET's MainObserver in RepeatWhen.cs
    // and RetryWhen.cs. Three parties take part:
    //
    //  * the source, which the sink subscribes to with itself as the observer, forwarding its
    //    elements to the downstream Observer;
    //  * the signals, which the derived sink raises from the source's terminal notification:
    //    RepeatWhen's when the source completes, RetryWhen's when it fails;
    //  * the handler's sequence, which the operator builds from a subject of those signals and
    //    subscribes with HandlerConsumer, keeping the subscription in HandlerUpstream.
    //
    // To raise a signal, the derived sink first calls ReleaseUpstreamAsync, which drops the
    // current subscription to the source (and returns false if the sink was disposed meanwhile,
    // in which case nothing further happens), and then pushes the signal into the subject. Each
    // element the handler's sequence produces in reply reaches HandlerNextAsync, which subscribes
    // to the source again. The operator makes the first subscription the same way, by calling
    // HandlerNextAsync once after subscribing the handler's sequence. The handler's own
    // completion or error ends the result, through ForwardCompletedAsync and ForwardErrorAsync;
    // the derived sink uses the same two for the source's terminal notification that is not its
    // signal (RepeatWhen's error, RetryWhen's completion).
    //
    // HandlerNextAsync runs on a trampoline, as Rx.NET's does. A source that completes or fails
    // synchronously raises its signal from inside the subscribe call, and the handler may answer
    // with another element at once; the counter turns that nested arrival into one more turn of
    // the loop already running rather than a nested subscribe, so the stack does not grow with
    // the number of repetitions. A signal that finds the sink disposed, or a subscription still
    // in place, returns with the counter raised, so no later signal subscribes either.
    //
    // Rx.NET's MainObserver also carries a half-serializer, which orders a failure of the
    // handler against elements the source is delivering concurrently. It is not needed here:
    // delivery is sequential, as everywhere in this library, so the two cannot overlap.
    internal abstract class RedoAsyncSink<TSource, TSignal> : IAsyncObserver<TSource>, IAsyncDisposable
    {
        private static readonly IAsyncDisposable Disposed = AsyncDisposable.Create(() => default);

        private readonly IAsyncObservable<TSource> _source;
        private IAsyncDisposable _upstream;
        private int _trampoline;
        private int _terminated;

        protected RedoAsyncSink(IAsyncObserver<TSource> observer, IAsyncObservable<TSource> source)
        {
            Observer = observer;
            _source = source;
            HandlerConsumer = new HandlerObserver(this);
        }

        protected IAsyncObserver<TSource> Observer { get; }

        internal IAsyncObserver<TSignal> HandlerConsumer { get; }

        internal SingleAssignmentAsyncDisposable HandlerUpstream { get; } = new();

        public virtual ValueTask OnNextAsync(TSource value) => Observer.OnNextAsync(value);

        public abstract ValueTask OnErrorAsync(Exception error);

        public abstract ValueTask OnCompletedAsync();

        public async ValueTask DisposeAsync()
        {
            var old = Interlocked.Exchange(ref _upstream, Disposed);

            if (old != null && old != Disposed)
            {
                await old.DisposeAsync().ConfigureAwait(false);
            }

            await HandlerUpstream.DisposeAsync().ConfigureAwait(false);
        }

        // Releases the current subscription to the source ahead of the signal to the handler;
        // false once the sink has been disposed (Rx.NET's Disposable.TrySetSerial to null).
        protected async ValueTask<bool> ReleaseUpstreamAsync()
        {
            IAsyncDisposable old;

            while (true)
            {
                old = Volatile.Read(ref _upstream);

                if (old == Disposed)
                {
                    return false;
                }

                if (Interlocked.CompareExchange(ref _upstream, null, old) == old)
                {
                    break;
                }
            }

            if (old != null)
            {
                await old.DisposeAsync().ConfigureAwait(false);
            }

            return true;
        }

        protected ValueTask ForwardErrorAsync(Exception error) =>
            Interlocked.Exchange(ref _terminated, 1) == 0 ? Observer.OnErrorAsync(error) : default;

        protected ValueTask ForwardCompletedAsync() =>
            Interlocked.Exchange(ref _terminated, 1) == 0 ? Observer.OnCompletedAsync() : default;

        // Subscribes to the source for each signal, on the trampoline described above. As in
        // Rx.NET, a signal that finds the sink disposed, or a subscription still in place,
        // returns with the counter raised, so no later signal subscribes either.
        internal async ValueTask HandlerNextAsync()
        {
            if (Interlocked.Increment(ref _trampoline) == 1)
            {
                do
                {
                    var subscription = new SingleAssignmentAsyncDisposable();

                    if (Interlocked.CompareExchange(ref _upstream, subscription, null) != null)
                    {
                        return;
                    }

                    await subscription.AssignAsync(await _source.SubscribeSafeAsync(this).ConfigureAwait(false)).ConfigureAwait(false);
                }
                while (Interlocked.Decrement(ref _trampoline) != 0);
            }
        }

        private sealed class HandlerObserver : IAsyncObserver<TSignal>
        {
            private readonly RedoAsyncSink<TSource, TSignal> _sink;

            public HandlerObserver(RedoAsyncSink<TSource, TSignal> sink)
            {
                _sink = sink;
            }

            public ValueTask OnNextAsync(TSignal value) => _sink.HandlerNextAsync();

            public ValueTask OnErrorAsync(Exception error) => _sink.ForwardErrorAsync(error);

            public ValueTask OnCompletedAsync() => _sink.ForwardCompletedAsync();
        }
    }
}

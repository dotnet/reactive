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
    // The source and the handler's sequence are independent, so the handler may terminate while
    // an element of the source is still being delivered downstream, and the observer grammar
    // forbids delivering the terminal notification until that delivery has finished. Rx.NET's
    // MainObserver orders the two with a half-serializer, and so does this sink: _wip counts
    // the delivery in flight, and a terminal notification that arrives during one is kept in
    // _terminal and forwarded by that delivery once it has finished, while one that arrives
    // between deliveries is forwarded at once. Forwarding a terminal notification then disposes
    // the sink's subscriptions, as Rx.NET's does, so a handler that terminates during the
    // subscribe call (an empty one, say) leaves the source unsubscribed, and an element arriving
    // after a terminal notification is dropped.
    internal abstract class RedoAsyncSink<TSource, TSignal> : IAsyncObserver<TSource>, IAsyncDisposable
    {
        private static readonly IAsyncDisposable Disposed = AsyncDisposable.Create(() => default);

        // Stands in _terminal for a completion, so that one field records whichever terminal
        // notification was accepted.
        private static readonly Exception Completed = new();

        private readonly IAsyncObservable<TSource> _source;
        private IAsyncDisposable _upstream;
        private int _trampoline;
        private int _wip;
        private Exception _terminal;

        protected RedoAsyncSink(IAsyncObserver<TSource> observer, IAsyncObservable<TSource> source)
        {
            Observer = observer;
            _source = source;
            HandlerConsumer = new HandlerObserver(this);
        }

        protected IAsyncObserver<TSource> Observer { get; }

        internal IAsyncObserver<TSignal> HandlerConsumer { get; }

        internal SingleAssignmentAsyncDisposable HandlerUpstream { get; } = new();

        public ValueTask OnNextAsync(TSource value)
        {
            if (Interlocked.CompareExchange(ref _wip, 1, 0) != 0)
            {
                // A terminal notification has been accepted (or a delivery is somehow still in
                // flight, which the source's own grammar rules out); the element is dropped.
                return default;
            }

            return DeliverAsync(value);
        }

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

        protected ValueTask ForwardErrorAsync(Exception error) => ForwardTerminalAsync(error);

        protected ValueTask ForwardCompletedAsync() => ForwardTerminalAsync(Completed);

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

        private async ValueTask DeliverAsync(TSource value)
        {
            await Observer.OnNextAsync(value).ConfigureAwait(false);

            if (Interlocked.Decrement(ref _wip) != 0)
            {
                // A terminal notification arrived during the delivery; it goes next.
                await DeliverTerminalAsync().ConfigureAwait(false);
            }
        }

        // Accepts the first terminal notification and forwards it unless a delivery is in
        // flight, in which case the delivery forwards it. _wip stays raised either way, so
        // nothing further is delivered.
        private ValueTask ForwardTerminalAsync(Exception terminal)
        {
            if (Interlocked.CompareExchange(ref _terminal, terminal, null) != null)
            {
                return default;
            }

            if (Interlocked.Increment(ref _wip) != 1)
            {
                return default;
            }

            return DeliverTerminalAsync();
        }

        private async ValueTask DeliverTerminalAsync()
        {
            var terminal = Volatile.Read(ref _terminal);

            if (terminal == Completed)
            {
                await Observer.OnCompletedAsync().ConfigureAwait(false);
            }
            else
            {
                await Observer.OnErrorAsync(terminal).ConfigureAwait(false);
            }

            await DisposeAsync().ConfigureAwait(false);
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

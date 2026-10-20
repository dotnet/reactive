// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Disposables;
using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    // The sink RepeatWhen and RetryWhen share, a port of Rx.NET's MainObserver in RepeatWhen.cs
    // and RetryWhen.cs: it subscribes to the source, and each element the handler's sequence
    // produces subscribes to it again (once the previous subscription has been released by the
    // completion or error that signalled the handler, which the subclass does); the handler's
    // completion or error terminates the sequence. The resubscription runs on a trampoline, as
    // Rx.NET's does: a signal arriving while a subscription is in progress is counted and taken
    // up by the loop already running, so a source that completes synchronously does not recurse.
    // Rx.NET's half-serializer, which orders a handler failure against the source's concurrent
    // elements, is not ported: delivery here is sequential, as elsewhere in the library.
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

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Disposables;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    // RetryWhen's observable: the source's error is the signal to the handler, its completion terminates.
    internal sealed class RetryWhenAsyncObservable<TSource, TSignal> : AsyncObservableBase<TSource>
    {
        private readonly IAsyncObservable<TSource> _source;
        private readonly Func<IAsyncObservable<Exception>, IAsyncObservable<TSignal>> _handler;

        public RetryWhenAsyncObservable(IAsyncObservable<TSource> source, Func<IAsyncObservable<Exception>, IAsyncObservable<TSignal>> handler)
        {
            _source = source;
            _handler = handler;
        }

        protected override async ValueTask<IAsyncDisposable> SubscribeAsyncCore(IAsyncObserver<TSource> observer)
        {
            var signals = new SequentialSimpleAsyncSubject<Exception>();

            IAsyncObservable<TSignal> redo;

            try
            {
                redo = _handler(signals);

                if (redo == null)
                {
                    throw new NullReferenceException("The handler returned a null IAsyncObservable");
                }
            }
            catch (Exception ex)
            {
                await observer.OnErrorAsync(ex).ConfigureAwait(false);
                return AsyncDisposable.Nop;
            }

            // The signals reach the subject through a queue lock: a signal raised while the
            // handler is still delivering the previous one (a source that completes
            // synchronously on resubscription) is queued, as Rx.NET's RedoSerializedObserver
            // queues it, rather than re-entering the subject.
            var sink = new Sink(observer, _source, new AsyncQueueLockAsyncObserver<Exception>(signals));

            await sink.HandlerUpstream.AssignAsync(await redo.SubscribeSafeAsync(sink.HandlerConsumer).ConfigureAwait(false)).ConfigureAwait(false);
            await sink.HandlerNextAsync().ConfigureAwait(false);

            return sink;
        }

        private sealed class Sink : RedoAsyncSink<TSource, TSignal>
        {
            private readonly IAsyncObserver<Exception> _signals;

            public Sink(IAsyncObserver<TSource> observer, IAsyncObservable<TSource> source, IAsyncObserver<Exception> signals)
                : base(observer, source)
            {
                _signals = signals;
            }

            public override async ValueTask OnErrorAsync(Exception error)
            {
                if (await ReleaseUpstreamAsync().ConfigureAwait(false))
                {
                    await _signals.OnNextAsync(error).ConfigureAwait(false);
                }
            }

            public override async ValueTask OnCompletedAsync()
            {
                await ForwardCompletedAsync().ConfigureAwait(false);
            }
        }
    }
}

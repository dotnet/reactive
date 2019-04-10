// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.ComponentModel;
using System.Reactive.Disposables;
using System.Threading;

namespace System.Reactive.Concurrency
{
    /// <summary>
    /// Provides basic synchronization and scheduling services for observable sequences.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static class Synchronization
    {
        #region SubscribeOn

        /// <summary>
        /// Wraps the source sequence in order to run its subscription and unsubscription logic on the specified scheduler.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="scheduler">Scheduler to perform subscription and unsubscription actions on.</param>
        /// <returns>The source sequence whose subscriptions and unsubscriptions happen on the specified scheduler.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="scheduler"/> is <c>null</c>.</exception>
        /// <remarks>
        /// Only the side-effects of subscribing to the source sequence and disposing subscriptions to the source sequence are run on the specified scheduler.
        /// In order to invoke observer callbacks on the specified scheduler, e.g. to offload callback processing to a dedicated thread, use <see cref="Synchronization.ObserveOn{TSource}(IObservable{TSource}, IScheduler)"/>.
        /// </remarks>
        public static IObservable<TSource> SubscribeOn<TSource>(IObservable<TSource> source, IScheduler scheduler)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (scheduler == null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            return new SubscribeOnObservable<TSource>(source, scheduler);
        }

        private sealed class SubscribeOnObservable<TSource> : ObservableBase<TSource>
        {
            private sealed class Subscription : IDisposable
            {
                private IDisposable _cancel;

                public Subscription(IObservable<TSource> source, IScheduler scheduler, IObserver<TSource> observer)
                {
                    Disposable.TrySetSingle(
                        ref _cancel,
                        scheduler.Schedule(
                            (@this: this, source, observer),
                            (closureScheduler, state) =>
                            {
                                Disposable.TrySetSerial(ref state.@this._cancel, new ScheduledDisposable(closureScheduler, state.source.SubscribeSafe(state.observer)));
                                return Disposable.Empty;
                            }));
                }

                public void Dispose()
                {
                    Disposable.TryDispose(ref _cancel);
                }
            }

            private readonly IObservable<TSource> _source;
            private readonly IScheduler _scheduler;

            public SubscribeOnObservable(IObservable<TSource> source, IScheduler scheduler)
            {
                _source = source;
                _scheduler = scheduler;
            }

            protected override IDisposable SubscribeCore(IObserver<TSource> observer)
            {
                return new Subscription(_source, _scheduler, observer);
            }
        }

        /// <summary>
        /// Wraps the source sequence in order to run its subscription and unsubscription logic on the specified synchronization context.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="context">Synchronization context to perform subscription and unsubscription actions on.</param>
        /// <returns>The source sequence whose subscriptions and unsubscriptions happen on the specified synchronization context.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="context"/> is <c>null</c>.</exception>
        /// <remarks>
        /// Only the side-effects of subscribing to the source sequence and disposing subscriptions to the source sequence are run on the specified synchronization context.
        /// In order to invoke observer callbacks on the specified synchronization context, e.g. to post callbacks to a UI thread represented by the synchronization context, use <see cref="Synchronization.ObserveOn{TSource}(IObservable{TSource}, SynchronizationContext)"/>.
        /// </remarks>
        public static IObservable<TSource> SubscribeOn<TSource>(IObservable<TSource> source, SynchronizationContext context)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            return new SubscribeOnCtxObservable<TSource>(source, context);
        }

        private sealed class SubscribeOnCtxObservable<TSource> : ObservableBase<TSource>
        {
            private sealed class Subscription : IDisposable
            {
                private readonly IObservable<TSource> _source;
                private readonly IObserver<TSource> _observer;
                private readonly SynchronizationContext _context;
                private IDisposable _cancel;

                public Subscription(IObservable<TSource> source, SynchronizationContext context, IObserver<TSource> observer)
                {
                    _source = source;
                    _context = context;
                    _observer = observer;

                    context.PostWithStartComplete(
                        @this =>
                        {
                            if (!Disposable.GetIsDisposed(ref @this._cancel))
                            {
                                Disposable.SetSingle(ref @this._cancel, new ContextDisposable(@this._context, @this._source.SubscribeSafe(@this._observer)));
                            }
                        },
                        this);
                }

                public void Dispose()
                {
                    Disposable.TryDispose(ref _cancel);
                }
            }

            private readonly IObservable<TSource> _source;
            private readonly SynchronizationContext _context;

            public SubscribeOnCtxObservable(IObservable<TSource> source, SynchronizationContext context)
            {
                _source = source;
                _context = context;
            }

            protected override IDisposable SubscribeCore(IObserver<TSource> observer)
            {
                return new Subscription(_source, _context, observer);
            }
        }

        #endregion

        #region ObserveOn

        /// <summary>
        /// Wraps the source sequence in order to run its observer callbacks on the specified scheduler.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="scheduler">Scheduler to notify observers on.</param>
        /// <returns>The source sequence whose observations happen on the specified scheduler.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="scheduler"/> is <c>null</c>.</exception>
        public static IObservable<TSource> ObserveOn<TSource>(IObservable<TSource> source, IScheduler scheduler)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (scheduler == null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            var longRunning = scheduler.AsLongRunning();
            if (longRunning != null)
            {
                return new ObserveOn<TSource>.SchedulerLongRunning(source, longRunning);
            }
            return new ObserveOn<TSource>.Scheduler(source, scheduler);
        }

        /// <summary>
        /// Wraps the source sequence in order to run its observer callbacks on the specified synchronization context.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="context">Synchronization context to notify observers on.</param>
        /// <returns>The source sequence whose observations happen on the specified synchronization context.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="context"/> is <c>null</c>.</exception>
        public static IObservable<TSource> ObserveOn<TSource>(IObservable<TSource> source, SynchronizationContext context)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            return new ObserveOn<TSource>.Context(source, context);
        }

        #endregion

        #region Synchronize

        /// <summary>
        /// Wraps the source sequence in order to ensure observer callbacks are properly serialized.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <returns>The source sequence whose outgoing calls to observers are synchronized.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is <c>null</c>.</exception>
        public static IObservable<TSource> Synchronize<TSource>(IObservable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return new Synchronize<TSource>(source);
        }

        /// <summary>
        /// Wraps the source sequence in order to ensure observer callbacks are synchronized using the specified gate object.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="gate">Gate object to synchronize each observer call on.</param>
        /// <returns>The source sequence whose outgoing calls to observers are synchronized on the given gate object.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="gate"/> is <c>null</c>.</exception>
        public static IObservable<TSource> Synchronize<TSource>(IObservable<TSource> source, object gate)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (gate == null)
            {
                throw new ArgumentNullException(nameof(gate));
            }

            return new Synchronize<TSource>(source, gate);
        }

        #endregion
    }
}

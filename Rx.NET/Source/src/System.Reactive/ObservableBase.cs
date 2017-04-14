// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Disposables;

namespace System.Reactive
{
    /// <summary>
    /// Abstract base class for implementations of the <see cref="IObservable{T}"/> interface.
    /// </summary>
    /// <remarks>
    /// If you don't need a named type to create an observable sequence (i.e. you rather need
    /// an instance rather than a reusable type), use the Observable.Create method to create
    /// an observable sequence with specified subscription behavior.
    /// </remarks>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    public abstract class ObservableBase<T> : IObservable<T>
    {
        /// <summary>
        /// Subscribes the given observer to the observable sequence.
        /// </summary>
        /// <param name="observer">Observer that will receive notifications from the observable sequence.</param>
        /// <returns>Disposable object representing an observer's subscription to the observable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="observer"/> is <c>null</c>.</exception>
        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var autoDetachObserver = new AutoDetachObserver<T>(observer);

            if (CurrentThreadScheduler.IsScheduleRequired)
            {
                //
                // Notice we don't protect this piece of code using an exception handler to
                // redirect errors to the OnError channel. This call to Schedule will run the
                // trampoline, so we'd be catching all exceptions, including those from user
                // callbacks that happen to run there. For example, consider:
                //
                //    Observable.Return(42, Scheduler.CurrentThread)
                //              .Subscribe(x => { throw new Exception(); });
                //
                // Here, the OnNext(42) call would be scheduled on the trampoline, so when we
                // return from the scheduled Subscribe call, the CurrentThreadScheduler moves
                // on to invoking this work item. Too much of protection here would cause the
                // exception thrown in OnNext to circle back to OnError, which looks like the
                // sequence can't make up its mind.
                //
                CurrentThreadScheduler.Instance.Schedule(autoDetachObserver, ScheduledSubscribe);
            }
            else
            {
                try
                {
                    autoDetachObserver.Disposable = SubscribeCore(autoDetachObserver);
                }
                catch (Exception exception)
                {
                    //
                    // This can happen when there's a synchronous callback to OnError in the
                    // implementation of SubscribeCore, which also throws. So, we're seeing
                    // an exception being thrown from a handler.
                    //
                    // For compat with v1.x, we rethrow the exception in this case, keeping
                    // in mind this should be rare but if it happens, something's totally
                    // screwed up.
                    //
                    if (!autoDetachObserver.Fail(exception))
                        throw;
                }
            }

            return autoDetachObserver;
        }

        private IDisposable ScheduledSubscribe(IScheduler _, AutoDetachObserver<T> autoDetachObserver)
        {
            try
            {
                autoDetachObserver.Disposable = SubscribeCore(autoDetachObserver);
            }
            catch (Exception exception)
            {
                //
                // This can happen when there's a synchronous callback to OnError in the
                // implementation of SubscribeCore, which also throws. So, we're seeing
                // an exception being thrown from a handler.
                //
                // For compat with v1.x, we rethrow the exception in this case, keeping
                // in mind this should be rare but if it happens, something's totally
                // screwed up.
                //
                if (!autoDetachObserver.Fail(exception))
                    throw;
            }

            return Disposable.Empty;
        }

        /// <summary>
        /// Implement this method with the core subscription logic for the observable sequence.
        /// </summary>
        /// <param name="observer">Observer to send notifications to.</param>
        /// <returns>Disposable object representing an observer's subscription to the observable sequence.</returns>
        protected abstract IDisposable SubscribeCore(IObserver<T> observer);
    }
}

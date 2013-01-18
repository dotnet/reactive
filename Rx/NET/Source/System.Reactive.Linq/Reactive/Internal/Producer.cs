// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_PERF
using System.Reactive.Concurrency;
using System.Reactive.Disposables;

namespace System.Reactive
{
    /// <summary>
    /// Base class for implementation of query operators, providing performance benefits over the use of <see cref="System.Reactive.Linq.Observable.Create">Observable.Create</see>.
    /// </summary>
    /// <typeparam name="TSource">Type of the resulting sequence's elements.</typeparam>
    abstract class Producer<TSource> : IObservable<TSource>
    {
        /// <summary>
        /// Publicly visible Subscribe method.
        /// </summary>
        /// <param name="observer">Observer to send notifications on. The implementation of a producer must ensure the correct message grammar on the observer.</param>
        /// <returns>IDisposable to cancel the subscription. This causes the underlying sink to be notified of unsubscription, causing it to prevent further messages from being sent to the observer.</returns>
        public IDisposable Subscribe(IObserver<TSource> observer)
        {
            if (observer == null)
                throw new ArgumentNullException("observer");

            var sink = new SingleAssignmentDisposable();
            var subscription = new SingleAssignmentDisposable();

            if (CurrentThreadScheduler.Instance.ScheduleRequired)
            {
                CurrentThreadScheduler.Instance.Schedule(this, (_, me) => subscription.Disposable = me.Run(observer, subscription, s => sink.Disposable = s));
            }
            else
            {
                subscription.Disposable = this.Run(observer, subscription, s => sink.Disposable = s);
            }

            return new CompositeDisposable(2) { sink, subscription };
        }

        /// <summary>
        /// Core implementation of the query operator, called upon a new subscription to the producer object.
        /// </summary>
        /// <param name="observer">Observer to send notifications on. The implementation of a producer must ensure the correct message grammar on the observer.</param>
        /// <param name="cancel">The subscription disposable object returned from the Run call, passed in such that it can be forwarded to the sink, allowing it to dispose the subscription upon sending a final message (or prematurely for other reasons).</param>
        /// <param name="setSink">Callback to communicate the sink object to the subscriber, allowing consumers to tunnel a Dispose call into the sink, which can stop the processing.</param>
        /// <returns>Disposable representing all the resources and/or subscriptions the operator uses to process events.</returns>
        /// <remarks>The <paramref name="observer">observer</paramref> passed in to this method is not protected using auto-detach behavior upon an OnError or OnCompleted call. The implementation must ensure proper resource disposal and enforce the message grammar.</remarks>
        protected abstract IDisposable Run(IObserver<TSource> observer, IDisposable cancel, Action<IDisposable> setSink);
    }
}
#endif
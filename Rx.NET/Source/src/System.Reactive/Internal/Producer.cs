// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Disposables;

namespace System.Reactive
{
    /// <summary>
    /// Interface with variance annotation; allows for better type checking when detecting capabilities in SubscribeSafe.
    /// </summary>
    /// <typeparam name="TSource">Type of the resulting sequence's elements.</typeparam>
    internal interface IProducer<out TSource> : IObservable<TSource>
    {
        IDisposable SubscribeRaw(IObserver<TSource> observer, bool enableSafeguard);
    }

    /// <summary>
    /// Base class for implementation of query operators, providing performance benefits over the use of Observable.Create.
    /// </summary>
    /// <typeparam name="TSource">Type of the resulting sequence's elements.</typeparam>
    internal abstract class BasicProducer<TSource> : IProducer<TSource>
    {
        /// <summary>
        /// Publicly visible Subscribe method.
        /// </summary>
        /// <param name="observer">Observer to send notifications on. The implementation of a producer must ensure the correct message grammar on the observer.</param>
        /// <returns>IDisposable to cancel the subscription. This causes the underlying sink to be notified of unsubscription, causing it to prevent further messages from being sent to the observer.</returns>
        public IDisposable Subscribe(IObserver<TSource> observer)
        {
            if (observer == null)
            {
                throw new ArgumentNullException(nameof(observer));
            }

            return SubscribeRaw(observer, enableSafeguard: true);
        }

        public IDisposable SubscribeRaw(IObserver<TSource> observer, bool enableSafeguard)
        {
            IDisposable run;
            ISafeObserver<TSource> safeObserver = null;

            //
            // See AutoDetachObserver.cs for more information on the safeguarding requirement and
            // its implementation aspects.
            //
            if (enableSafeguard)
            {
                observer = safeObserver = SafeObserver<TSource>.Wrap(observer);
            }

            if (CurrentThreadScheduler.IsScheduleRequired)
            {
                var runAssignable = new SingleAssignmentDisposable();

                CurrentThreadScheduler.Instance.ScheduleAction(
                    (@this: this, runAssignable, observer),
                    tuple => tuple.runAssignable.Disposable = tuple.@this.Run(tuple.observer));

                run = runAssignable;
            }
            else
            {
                run = Run(observer);
            }

            safeObserver?.SetResource(run);
            return run;
        }

        /// <summary>
        /// Core implementation of the query operator, called upon a new subscription to the producer object.
        /// </summary>
        /// <param name="observer">Observer to send notifications on. The implementation of a producer must ensure the correct message grammar on the observer.</param>
        /// <returns>Disposable representing all the resources and/or subscriptions the operator uses to process events.</returns>
        /// <remarks>The <paramref name="observer">observer</paramref> passed in to this method is not protected using auto-detach behavior upon an OnError or OnCompleted call. The implementation must ensure proper resource disposal and enforce the message grammar.</remarks>
        protected abstract IDisposable Run(IObserver<TSource> observer);
    }

    internal abstract class Producer<TTarget, TSink> : IProducer<TTarget>
        where TSink : IDisposable
    {
        /// <summary>
        /// Publicly visible Subscribe method.
        /// </summary>
        /// <param name="observer">Observer to send notifications on. The implementation of a producer must ensure the correct message grammar on the observer.</param>
        /// <returns>IDisposable to cancel the subscription. This causes the underlying sink to be notified of unsubscription, causing it to prevent further messages from being sent to the observer.</returns>
        public IDisposable Subscribe(IObserver<TTarget> observer)
        {
            if (observer == null)
            {
                throw new ArgumentNullException(nameof(observer));
            }

            return SubscribeRaw(observer, enableSafeguard: true);
        }

        public IDisposable SubscribeRaw(IObserver<TTarget> observer, bool enableSafeguard)
        {
            ISafeObserver<TTarget> safeObserver = null;

            //
            // See AutoDetachObserver.cs for more information on the safeguarding requirement and
            // its implementation aspects.
            //
            if (enableSafeguard)
            {
                observer = safeObserver = SafeObserver<TTarget>.Wrap(observer);
            }

            var sink = CreateSink(observer);

            safeObserver?.SetResource(sink);

            if (CurrentThreadScheduler.IsScheduleRequired)
            {
                CurrentThreadScheduler.Instance.ScheduleAction(
                    (@this: this, sink),
                    tuple => tuple.@this.Run(tuple.sink));
            }
            else
            {
                Run(sink);
            }

            return sink;
        }

        /// <summary>
        /// Core implementation of the query operator, called upon a new subscription to the producer object.
        /// </summary>
        /// <param name="sink">The sink object.</param>
        protected abstract void Run(TSink sink);

        protected abstract TSink CreateSink(IObserver<TTarget> observer);
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Threading;

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
                throw new ArgumentNullException(nameof(observer));

            return SubscribeRaw(observer, enableSafeguard: true);
        }

        public IDisposable SubscribeRaw(IObserver<TSource> observer, bool enableSafeguard)
        {
            var subscription = new SingleAssignmentDisposable();

            //
            // See AutoDetachObserver.cs for more information on the safeguarding requirement and
            // its implementation aspects.
            //
            if (enableSafeguard)
            {
                observer = SafeObserver<TSource>.Create(observer, subscription);
            }

            if (CurrentThreadScheduler.IsScheduleRequired)
            {
                var state = new State { subscription = subscription, observer = observer };
                CurrentThreadScheduler.Instance.Schedule(state, Run);
            }
            else
            {
                subscription.Disposable = Run(observer);
            }

            return subscription;
        }

        private struct State
        {
            public SingleAssignmentDisposable subscription;
            public IObserver<TSource> observer;
        }

        private IDisposable Run(IScheduler _, State x)
        {
            x.subscription.Disposable = Run(x.observer);
            return Disposable.Empty;
        }

        /// <summary>
        /// Core implementation of the query operator, called upon a new subscription to the producer object.
        /// </summary>
        /// <param name="observer">Observer to send notifications on. The implementation of a producer must ensure the correct message grammar on the observer.</param>
        /// <returns>Disposable representing all the resources and/or subscriptions the operator uses to process events.</returns>
        /// <remarks>The <paramref name="observer">observer</paramref> passed in to this method is not protected using auto-detach behavior upon an OnError or OnCompleted call. The implementation must ensure proper resource disposal and enforce the message grammar.</remarks>
        protected abstract IDisposable Run(IObserver<TSource> observer);
    }

    internal abstract class Producer<TSource, TSink> : IProducer<TSource>
        where TSink : IDisposable
    {
        /// <summary>
        /// Publicly visible Subscribe method.
        /// </summary>
        /// <param name="observer">Observer to send notifications on. The implementation of a producer must ensure the correct message grammar on the observer.</param>
        /// <returns>IDisposable to cancel the subscription. This causes the underlying sink to be notified of unsubscription, causing it to prevent further messages from being sent to the observer.</returns>
        public IDisposable Subscribe(IObserver<TSource> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            return SubscribeRaw(observer, enableSafeguard: true);
        }

        public IDisposable SubscribeRaw(IObserver<TSource> observer, bool enableSafeguard)
        {
            var subscription = new SubscriptionDisposable();

            //
            // See AutoDetachObserver.cs for more information on the safeguarding requirement and
            // its implementation aspects.
            //
            if (enableSafeguard)
            {
                observer = SafeObserver<TSource>.Create(observer, subscription);
            }

            var sink = CreateSink(observer, subscription.Inner);

            subscription.Sink = sink;

            if (CurrentThreadScheduler.IsScheduleRequired)
            {
                var state = new State { sink = sink, inner = subscription.Inner };

                CurrentThreadScheduler.Instance.Schedule(state, Run);
            }
            else
            {
                subscription.Inner.Disposable = Run(sink);
            }

            return subscription;
        }

        private struct State
        {
            public TSink sink;
            public SingleAssignmentDisposable inner;
        }

        private IDisposable Run(IScheduler _, State x)
        {
            x.inner.Disposable = Run(x.sink);
            return Disposable.Empty;
        }

        /// <summary>
        /// Core implementation of the query operator, called upon a new subscription to the producer object.
        /// </summary>
        /// <param name="sink">The sink object.</param>
        protected abstract IDisposable Run(TSink sink);

        protected abstract TSink CreateSink(IObserver<TSource> observer, IDisposable cancel);
    }

    internal sealed class SubscriptionDisposable : ICancelable
    {
        public volatile IDisposable Sink;
        public readonly SingleAssignmentDisposable Inner = new SingleAssignmentDisposable();

        public bool IsDisposed => Sink == null;

        public void Dispose()
        {
            Interlocked.Exchange(ref Sink, null)?.Dispose();
            Inner.Dispose();
        }
    }
}

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
    internal abstract class Producer<TSource> : IProducer<TSource>
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

            return SubscribeRaw(observer, true);
        }

        public IDisposable SubscribeRaw(IObserver<TSource> observer, bool enableSafeguard)
        {
            var state = new State();
            state.observer = observer;
            state.sink = new SingleAssignmentDisposable();
            state.subscription = new SingleAssignmentDisposable();

            var d = StableCompositeDisposable.Create(state.sink, state.subscription);

            //
            // See AutoDetachObserver.cs for more information on the safeguarding requirement and
            // its implementation aspects.
            //
            if (enableSafeguard)
            {
                state.observer = SafeObserver<TSource>.Create(state.observer, d);
            }

            if (CurrentThreadScheduler.IsScheduleRequired)
            {
                CurrentThreadScheduler.Instance.Schedule(state, Run);
            }
            else
            {
                state.subscription.Disposable = Run(state.observer, state.subscription, state.Assign);
            }

            return d;
        }

        private struct State
        {
            public SingleAssignmentDisposable sink;
            public SingleAssignmentDisposable subscription;
            public IObserver<TSource> observer;

            public void Assign(IDisposable s)
            {
                sink.Disposable = s;
            }
        }

        private IDisposable Run(IScheduler _, State x)
        {
            x.subscription.Disposable = this.Run(x.observer, x.subscription, x.Assign);
            return Disposable.Empty;
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

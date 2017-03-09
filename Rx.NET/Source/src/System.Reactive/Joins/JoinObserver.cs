// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace System.Reactive.Joins
{
    internal interface IJoinObserver : IDisposable
    {
        void Subscribe(object gate);
        void Dequeue();
    }

    internal sealed class JoinObserver<T> : ObserverBase<Notification<T>>, IJoinObserver
    {
        private object gate;
        private readonly IObservable<T> source;
        private readonly Action<Exception> onError;
        private List<ActivePlan> activePlans;
        public Queue<Notification<T>> Queue { get; private set; }
        private readonly SingleAssignmentDisposable subscription;
        private bool isDisposed;

        public JoinObserver(IObservable<T> source, Action<Exception> onError)
        {
            this.source = source;
            this.onError = onError;
            Queue = new Queue<Notification<T>>();
            subscription = new SingleAssignmentDisposable();
            activePlans = new List<ActivePlan>();
        }

        public void AddActivePlan(ActivePlan activePlan)
        {
            activePlans.Add(activePlan);
        }

        public void Subscribe(object gate)
        {
            this.gate = gate;
            subscription.Disposable = source.Materialize().SubscribeSafe(this);
        }

        public void Dequeue()
        {
            Queue.Dequeue();
        }

        protected override void OnNextCore(Notification<T> notification)
        {
            lock (gate)
            {
                if (!isDisposed)
                {
                    if (notification.Kind == NotificationKind.OnError)
                    {
                        onError(notification.Exception);
                        return;
                    }

                    Queue.Enqueue(notification);
                    foreach (var activePlan in activePlans.ToArray())
                        activePlan.Match();
                }
            }
        }

        protected override void OnErrorCore(Exception exception)
        {
        }

        protected override void OnCompletedCore()
        {
        }

        internal void RemoveActivePlan(ActivePlan activePlan)
        {
            activePlans.Remove(activePlan);
            if (activePlans.Count == 0)
                Dispose();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (!isDisposed)
            {
                if (disposing)
                    subscription.Dispose();

                isDisposed = true;
            }
        }
    }
}
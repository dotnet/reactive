﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
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
        private object? _gate;
        private readonly IObservable<T> _source;
        private readonly Action<Exception> _onError;
        private readonly List<ActivePlan> _activePlans = [];
        private SingleAssignmentDisposableValue _subscription;
        private bool _isDisposed;

        public JoinObserver(IObservable<T> source, Action<Exception> onError)
        {
            _source = source;
            _onError = onError;
            Queue = new Queue<Notification<T>>();
        }

        public Queue<Notification<T>> Queue { get; }

        public void AddActivePlan(ActivePlan activePlan)
        {
            _activePlans.Add(activePlan);
        }

        public void Subscribe(object gate)
        {
            _gate = gate;
            _subscription.Disposable = _source.Materialize().SubscribeSafe(this);
        }

        public void Dequeue()
        {
            Queue.Dequeue();
        }

        protected override void OnNextCore(Notification<T> notification)
        {
            lock (_gate!) // NB: Called after Subscribe(object) is called.
            {
                if (!_isDisposed)
                {
                    if (notification.Kind == NotificationKind.OnError)
                    {
                        _onError(notification.Exception!);
                        return;
                    }

                    Queue.Enqueue(notification);
                    foreach (var activePlan in _activePlans.ToArray()) // Working on a copy since _activePlans might change while iterating.
                    {
                        activePlan.Match();
                    }
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
            _activePlans.Remove(activePlan);
            if (_activePlans.Count == 0)
            {
                Dispose();
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (!_isDisposed)
            {
                if (disposing)
                {
                    _subscription.Dispose();
                }

                _isDisposed = true;
            }
        }
    }
}

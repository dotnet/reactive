// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Disposables;

namespace Microsoft.Reactive.Testing
{
    internal class HotObservable<T> : ITestableObservable<T>
    {
        private readonly TestScheduler _scheduler;
        private readonly List<IObserver<T>> _observers = new List<IObserver<T>>();
        private readonly List<Subscription> _subscriptions = new List<Subscription>();
        private readonly Recorded<Notification<T>>[] _messages;

        public HotObservable(TestScheduler scheduler, params Recorded<Notification<T>>[] messages)
        {
            _scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
            _messages = messages ?? throw new ArgumentNullException(nameof(messages));

            for (var i = 0; i < messages.Length; ++i)
            {
                var notification = messages[i].Value;
                scheduler.ScheduleAbsolute(default(object), messages[i].Time, (scheduler1, state1) =>
                {
                    var _observers = this._observers.ToArray();
                    for (var j = 0; j < _observers.Length; ++j)
                    {
                        notification.Accept(_observers[j]);
                    }
                    return Disposable.Empty;
                });
            }
        }

        public virtual IDisposable Subscribe(IObserver<T> observer)
        {
            if (observer == null)
            {
                throw new ArgumentNullException(nameof(observer));
            }

            _observers.Add(observer);
            _subscriptions.Add(new Subscription(_scheduler.Clock));
            var index = _subscriptions.Count - 1;

            return Disposable.Create(() =>
            {
                _observers.Remove(observer);
                _subscriptions[index] = new Subscription(_subscriptions[index].Subscribe, _scheduler.Clock);
            });
        }

        public IList<Subscription> Subscriptions
        {
            get { return _subscriptions; }
        }

        public IList<Recorded<Notification<T>>> Messages
        {
            get { return _messages; }
        }
    }
}

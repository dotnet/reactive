// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Disposables;

namespace Microsoft.Reactive.Testing
{
    internal class ColdObservable<T> : ITestableObservable<T>
    {
        private readonly TestScheduler _scheduler;
        private readonly Recorded<Notification<T>>[] _messages;
        private readonly List<Subscription> _subscriptions = new List<Subscription>();

        public ColdObservable(TestScheduler scheduler, params Recorded<Notification<T>>[] messages)
        {
            _scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
            _messages = messages ?? throw new ArgumentNullException(nameof(messages));
        }

        public virtual IDisposable Subscribe(IObserver<T> observer)
        {
            if (observer == null)
            {
                throw new ArgumentNullException(nameof(observer));
            }

            _subscriptions.Add(new Subscription(_scheduler.Clock));
            var index = _subscriptions.Count - 1;

            var d = new CompositeDisposable();

            for (var i = 0; i < _messages.Length; ++i)
            {
                var notification = _messages[i].Value;
                d.Add(_scheduler.ScheduleRelative(default(object), _messages[i].Time, (scheduler1, state1) => { notification.Accept(observer); return Disposable.Empty; }));
            }

            return Disposable.Create(() =>
            {
                _subscriptions[index] = new Subscription(_subscriptions[index].Subscribe, _scheduler.Clock);
                d.Dispose();
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

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Reactive;
using System;
using System.Reactive.Disposables;

namespace Microsoft.Reactive.Testing
{
    class ColdObservable<T> : ITestableObservable<T>
    {
        readonly TestScheduler scheduler;
        readonly Recorded<Notification<T>>[] messages;
        readonly List<Subscription> subscriptions = new List<Subscription>();

        public ColdObservable(TestScheduler scheduler, params Recorded<Notification<T>>[] messages)
        {
            if (scheduler == null)
                throw new ArgumentNullException("scheduler");
            if (messages == null)
                throw new ArgumentNullException("messages");

            this.scheduler = scheduler;
            this.messages = messages;
        }

        public virtual IDisposable Subscribe(IObserver<T> observer)
        {
            if (observer == null)
                throw new ArgumentNullException("observer");

            subscriptions.Add(new Subscription(scheduler.Clock));
            var index = subscriptions.Count - 1;

            var d = new CompositeDisposable();

            for (var i = 0; i < messages.Length; ++i)
            {
                var notification = messages[i].Value;
                d.Add(scheduler.ScheduleRelative(default(object), messages[i].Time, (scheduler1, state1) => { notification.Accept(observer); return Disposable.Empty; }));
            }

            return Disposable.Create(() =>
            {
                subscriptions[index] = new Subscription(subscriptions[index].Subscribe, scheduler.Clock);
                d.Dispose();
            });
        }

        public IList<Subscription> Subscriptions
        {
            get { return subscriptions; }
        }

        public IList<Recorded<Notification<T>>> Messages
        {
            get { return messages; }
        }
    }
}

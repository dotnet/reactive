// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Reactive;

namespace Microsoft.Reactive.Testing
{
    internal class MockObserver<T> : ITestableObserver<T>
    {
        private TestScheduler _scheduler;
        private List<Recorded<Notification<T>>> _messages;

        public MockObserver(TestScheduler scheduler)
        {
            this._scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
            _messages = new List<Recorded<Notification<T>>>();
        }

        public void OnNext(T value)
        {
            _messages.Add(new Recorded<Notification<T>>(_scheduler.Clock, Notification.CreateOnNext(value)));
        }

        public void OnError(Exception exception)
        {
            _messages.Add(new Recorded<Notification<T>>(_scheduler.Clock, Notification.CreateOnError<T>(exception)));
        }

        public void OnCompleted()
        {
            _messages.Add(new Recorded<Notification<T>>(_scheduler.Clock, Notification.CreateOnCompleted<T>()));
        }

        public IList<Recorded<Notification<T>>> Messages
        {
            get { return _messages; }
        }
    }
}

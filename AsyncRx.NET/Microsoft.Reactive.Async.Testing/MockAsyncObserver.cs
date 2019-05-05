// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Reactive;
using System.Threading.Tasks;

namespace Microsoft.Reactive.Testing
{
    internal class MockAsyncObserver<T> : ITestableAsyncObserver<T>
    {
        private readonly TestAsyncScheduler _scheduler;
        private readonly List<Recorded<Notification<T>>> _messages;

        public MockAsyncObserver(TestAsyncScheduler scheduler)
        {
            _scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
            _messages = new List<Recorded<Notification<T>>>();
        }

        public IReadOnlyList<Recorded<Notification<T>>> Messages { get => _messages; }

        public Task OnNextAsync(T value)
        {
            _messages.Add(new Recorded<Notification<T>>(_scheduler.Clock, Notification.CreateOnNext(value)));
            return Task.CompletedTask;
        }

        public Task OnErrorAsync(Exception error)
        {
            _messages.Add(new Recorded<Notification<T>>(_scheduler.Clock, Notification.CreateOnError<T>(error)));
            return Task.CompletedTask;
        }

        public Task OnCompletedAsync()
        {
            _messages.Add(new Recorded<Notification<T>>(_scheduler.Clock, Notification.CreateOnCompleted<T>()));
            return Task.CompletedTask;
        }

    }
}

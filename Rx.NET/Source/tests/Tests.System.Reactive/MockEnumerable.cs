// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using Microsoft.Reactive.Testing;

namespace ReactiveTests
{
    public class MockEnumerable<T> : IEnumerable<T>
    {
        public readonly TestScheduler Scheduler;
        public readonly List<Subscription> Subscriptions = new List<Subscription>();
        private IEnumerable<T> underlyingEnumerable;

        public MockEnumerable(TestScheduler scheduler, IEnumerable<T> underlyingEnumerable)
        {
            Scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
            this.underlyingEnumerable = underlyingEnumerable ?? throw new ArgumentNullException(nameof(underlyingEnumerable));
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new MockEnumerator(Scheduler, Subscriptions, underlyingEnumerable.GetEnumerator());
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private class MockEnumerator : IEnumerator<T>
        {
            private readonly List<Subscription> subscriptions;
            private IEnumerator<T> enumerator;
            private TestScheduler scheduler;
            private readonly int index;
            private bool disposed = false;

            public MockEnumerator(TestScheduler scheduler, List<Subscription> subscriptions, IEnumerator<T> enumerator)
            {
                this.subscriptions = subscriptions;
                this.enumerator = enumerator;
                this.scheduler = scheduler;

                index = subscriptions.Count;
                subscriptions.Add(new Subscription(scheduler.Clock));
            }

            public T Current
            {
                get
                {
                    if (disposed)
                    {
                        throw new ObjectDisposedException("this");
                    }

                    return enumerator.Current;
                }
            }

            public void Dispose()
            {
                if (!disposed)
                {
                    disposed = true;
                    enumerator.Dispose();
                    subscriptions[index] = new Subscription(subscriptions[index].Subscribe, scheduler.Clock);
                }
            }

            object System.Collections.IEnumerator.Current
            {
                get { return Current; }
            }

            public bool MoveNext()
            {
                if (disposed)
                {
                    throw new ObjectDisposedException("this");
                }

                return enumerator.MoveNext();
            }

            public void Reset()
            {
                if (disposed)
                {
                    throw new ObjectDisposedException("this");
                }

                enumerator.Reset();
            }
        }

    }
}

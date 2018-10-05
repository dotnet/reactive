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
        private IEnumerable<T> _underlyingEnumerable;

        public MockEnumerable(TestScheduler scheduler, IEnumerable<T> underlyingEnumerable)
        {
            Scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
            this._underlyingEnumerable = underlyingEnumerable ?? throw new ArgumentNullException(nameof(underlyingEnumerable));
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new MockEnumerator(Scheduler, Subscriptions, _underlyingEnumerable.GetEnumerator());
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private class MockEnumerator : IEnumerator<T>
        {
            private readonly List<Subscription> _subscriptions;
            private IEnumerator<T> _enumerator;
            private TestScheduler _scheduler;
            private readonly int _index;
            private bool _disposed;

            public MockEnumerator(TestScheduler scheduler, List<Subscription> subscriptions, IEnumerator<T> enumerator)
            {
                this._subscriptions = subscriptions;
                this._enumerator = enumerator;
                this._scheduler = scheduler;

                _index = subscriptions.Count;
                subscriptions.Add(new Subscription(scheduler.Clock));
            }

            public T Current
            {
                get
                {
                    if (_disposed)
                    {
                        throw new ObjectDisposedException("this");
                    }

                    return _enumerator.Current;
                }
            }

            public void Dispose()
            {
                if (!_disposed)
                {
                    _disposed = true;
                    _enumerator.Dispose();
                    _subscriptions[_index] = new Subscription(_subscriptions[_index].Subscribe, _scheduler.Clock);
                }
            }

            object System.Collections.IEnumerator.Current
            {
                get { return Current; }
            }

            public bool MoveNext()
            {
                if (_disposed)
                {
                    throw new ObjectDisposedException("this");
                }

                return _enumerator.MoveNext();
            }

            public void Reset()
            {
                if (_disposed)
                {
                    throw new ObjectDisposedException("this");
                }

                _enumerator.Reset();
            }
        }

    }
}

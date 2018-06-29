// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Subjects;

namespace ReactiveTests.Tests
{
    internal class MySubject : ISubject<int>
    {
        private Dictionary<int, IDisposable> _disposeOn = new Dictionary<int, IDisposable>();

        public void DisposeOn(int value, IDisposable disposable)
        {
            _disposeOn[value] = disposable;
        }

        private IObserver<int> _observer;

        public void OnNext(int value)
        {
            _observer.OnNext(value);

            if (_disposeOn.TryGetValue(value, out var disconnect))
            {
                disconnect.Dispose();
            }
        }

        public void OnError(Exception exception)
        {
            _observer.OnError(exception);
        }

        public void OnCompleted()
        {
            _observer.OnCompleted();
        }

        public IDisposable Subscribe(IObserver<int> observer)
        {
            _subscribeCount++;
            _observer = observer;
            return Disposable.Create(() => { _disposed = true; });
        }

        private int _subscribeCount;
        private bool _disposed;

        public int SubscribeCount { get { return _subscribeCount; } }
        public bool Disposed { get { return _disposed; } }
    }
}

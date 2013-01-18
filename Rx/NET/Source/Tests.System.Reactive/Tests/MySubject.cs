// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Subjects;

namespace ReactiveTests.Tests
{
    class MySubject : ISubject<int>
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

            IDisposable disconnect;
            if (_disposeOn.TryGetValue(value, out disconnect))
                disconnect.Dispose();
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

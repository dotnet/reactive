// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Subjects;

namespace ReactiveTests.Tests
{
    internal class MySubject : ISubject<int>
    {
        private readonly Dictionary<int, IDisposable> _disposeOn = [];

        public void DisposeOn(int value, IDisposable disposable)
        {
            _disposeOn[value] = disposable;
        }

        private readonly List<IObserver<int>> _observers = new();

        public void OnNext(int value)
        {
            foreach (var observer in _observers.ToArray())
            {
                observer.OnNext(value); 
            }

            if (_disposeOn.TryGetValue(value, out var disconnect))
            {
                disconnect.Dispose();
            }
        }

        public void OnError(Exception exception)
        {
            foreach (var observer in _observers.ToArray())
            {
                observer.OnError(exception);
            }
        }

        public void OnCompleted()
        {
            foreach (var observer in _observers.ToArray())
            {
                observer.OnCompleted();
            }
        }

        public IDisposable Subscribe(IObserver<int> observer)
        {
            _subscribeCount++;
            _observers.Add(observer);
            return Disposable.Create(() =>
            {
                _observers.Remove(observer);
                _disposed = true;
            });
        }

        private int _subscribeCount;
        private bool _disposed;

        public int SubscribeCount { get { return _subscribeCount; } }
        public bool Disposed { get { return _disposed; } }
    }
}

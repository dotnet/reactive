// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace ReactiveTests.Tests
{
    class ConnectableObservable<T> : IConnectableObservable<T>
    {
        private IConnectableObservable<T> _o;

        public ConnectableObservable(IObservable<T> o, ISubject<T, T> s)
        {
            _o = o.Multicast(s);
        }

        public IDisposable Connect()
        {
            return _o.Connect();
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            return _o.Subscribe(observer);
        }
    }
}

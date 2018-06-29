// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace ReactiveTests.Tests
{
    internal class ConnectableObservable<T> : IConnectableObservable<T>
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

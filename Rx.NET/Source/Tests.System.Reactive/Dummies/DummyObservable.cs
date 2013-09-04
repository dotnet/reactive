// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;

namespace ReactiveTests.Dummies
{
    class DummyObservable<T> : IObservable<T>
    {
        public static readonly DummyObservable<T> Instance = new DummyObservable<T>();

        DummyObservable()
        {
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            throw new NotImplementedException();
        }
    }
}

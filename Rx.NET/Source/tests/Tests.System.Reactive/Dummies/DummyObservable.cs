// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System;

namespace ReactiveTests.Dummies
{
    internal class DummyObservable<T> : IObservable<T>
    {
        public static readonly DummyObservable<T> Instance = new();

        private DummyObservable()
        {
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            throw new NotImplementedException();
        }
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;

namespace ReactiveTests.Dummies
{
    internal class DummyObserver<T> : IObserver<T>
    {
        public static readonly DummyObserver<T> Instance = new DummyObserver<T>();

        private DummyObserver()
        {
        }

        public void OnNext(T value)
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception exception)
        {
            throw new NotImplementedException();
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }
    }
}

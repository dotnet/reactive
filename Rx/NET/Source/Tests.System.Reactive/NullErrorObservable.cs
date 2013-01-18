// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System.Reactive.Disposables;
using System;

namespace ReactiveTests
{
    public class NullErrorObservable<T> : IObservable<T>
    {
        public static NullErrorObservable<T> Instance = new NullErrorObservable<T>();

        private NullErrorObservable()
        {
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (observer == null)
                throw new ArgumentNullException("observer");

            observer.OnError(null);
            return Disposable.Empty;
        }
    }
}

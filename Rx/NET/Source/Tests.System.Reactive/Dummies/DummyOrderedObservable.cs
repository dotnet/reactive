// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Reactive.Linq;

namespace ReactiveTests.Dummies
{
    class DummyOrderedObservable<T> : IOrderedObservable<T>
    {
        public static readonly DummyOrderedObservable<T> Instance = new DummyOrderedObservable<T>();

        DummyOrderedObservable()
        {
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            throw new NotImplementedException();
        }

        public IOrderedObservable<T> CreateOrderedObservable<TKey>(Func<T, TKey> keySelector, IComparer<TKey> comparer, bool descending)
        {
            return descending ? this.OrderByDescending(keySelector, comparer) : this.OrderBy(keySelector, comparer);
        }

        public IOrderedObservable<T> CreateOrderedObservable<TOther>(Func<T, IObservable<TOther>> timeSelector, bool descending)
        {
            return descending ? this.OrderByDescending(timeSelector) : this.OrderBy(timeSelector);
        }
    }
}

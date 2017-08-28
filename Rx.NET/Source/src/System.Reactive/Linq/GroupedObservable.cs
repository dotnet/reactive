// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Disposables;
using System.Reactive.Subjects;

namespace System.Reactive.Linq
{
    internal sealed class GroupedObservable<TKey, TElement> : ObservableBase<TElement>, IGroupedObservable<TKey, TElement>
    {
        private readonly IObservable<TElement> _subject;
        private readonly RefCountDisposable _refCount;

        public GroupedObservable(TKey key, ISubject<TElement> subject, RefCountDisposable refCount)
        {
            Key = key;
            _subject = subject;
            _refCount = refCount;
        }

        public GroupedObservable(TKey key, ISubject<TElement> subject)
        {
            Key = key;
            _subject = subject;
        }

        public TKey Key { get; }

        protected override IDisposable SubscribeCore(IObserver<TElement> observer)
        {
            if (_refCount != null)
            {
                //
                // [OK] Use of unsafe Subscribe: called on a known subject implementation.
                //
                var release = _refCount.GetDisposable();
                var subscription = _subject.Subscribe/*Unsafe*/(observer);
                return StableCompositeDisposable.Create(release, subscription);
            }
            else
            {
                //
                // [OK] Use of unsafe Subscribe: called on a known subject implementation.
                //
                return _subject.Subscribe/*Unsafe*/(observer);
            }
        }
    }
}

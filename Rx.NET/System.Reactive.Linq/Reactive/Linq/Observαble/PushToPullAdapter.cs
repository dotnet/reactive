// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reactive.Disposables;

namespace System.Reactive.Linq.Observαble
{
    abstract class PushToPullAdapter<TSource, TResult> : IEnumerable<TResult>
    {
        private readonly IObservable<TSource> _source;

        public PushToPullAdapter(IObservable<TSource> source)
        {
            _source = source;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<TResult> GetEnumerator()
        {
            var d = new SingleAssignmentDisposable();
            var res = Run(d);
            d.Disposable = _source.SubscribeSafe(res);
            return res;
        }

        protected abstract PushToPullSink<TSource, TResult> Run(IDisposable subscription);
    }

    abstract class PushToPullSink<TSource, TResult> : IObserver<TSource>, IEnumerator<TResult>, IDisposable
    {
        private readonly IDisposable _subscription;

        public PushToPullSink(IDisposable subscription)
        {
            _subscription = subscription;
        }

        public abstract void OnNext(TSource value);
        public abstract void OnError(Exception error);
        public abstract void OnCompleted();

        public abstract bool TryMoveNext(out TResult current);

        private bool _done;

        public bool MoveNext()
        {
            if (!_done)
            {
                var current = default(TResult);
                if (TryMoveNext(out current))
                {
                    Current = current;
                    return true;
                }
                else
                {
                    _done = true;
                    _subscription.Dispose();
                }
            }

            return false;
        }

        public TResult Current
        {
            get;
            private set;
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }

        public void Reset()
        {
            throw new NotSupportedException();
        }

        public void Dispose()
        {
            _subscription.Dispose();
        }
    }
}

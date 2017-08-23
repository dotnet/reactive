// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections;
using System.Collections.Generic;
using System.Reactive.Disposables;

namespace System.Reactive.Linq.ObservableImpl
{
    internal abstract class PushToPullAdapter<TSource, TResult> : IEnumerable<TResult>
    {
        private readonly IObservable<TSource> _source;

        public PushToPullAdapter(IObservable<TSource> source)
        {
            _source = source;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<TResult> GetEnumerator()
        {
            var d = new SingleAssignmentDisposable();
            var res = Run(d);
            d.Disposable = _source.SubscribeSafe(res);
            return res;
        }

        protected abstract PushToPullSink<TSource, TResult> Run(IDisposable subscription);
    }

    internal abstract class PushToPullSink<TSource, TResult> : IObserver<TSource>, IEnumerator<TResult>, IDisposable
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
                if (TryMoveNext(out var current))
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

        object IEnumerator.Current => Current;

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

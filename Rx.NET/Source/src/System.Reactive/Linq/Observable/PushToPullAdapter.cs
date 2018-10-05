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

        protected PushToPullAdapter(IObservable<TSource> source)
        {
            _source = source;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<TResult> GetEnumerator()
        {
            var res = Run();
            res.SetUpstream(_source.SubscribeSafe(res));
            return res;
        }

        protected abstract PushToPullSink<TSource, TResult> Run();
    }

    internal abstract class PushToPullSink<TSource, TResult> : IObserver<TSource>, IEnumerator<TResult>
    {
        private IDisposable _upstream;

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

                _done = true;
                Dispose();
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
            Disposable.TryDispose(ref _upstream);
        }

        public void SetUpstream(IDisposable d)
        {
            Disposable.SetSingle(ref _upstream, d);
        }
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

#if !NO_PERF
using System;
using System.Collections.Generic;

namespace System.Reactive.Linq.ObservableImpl
{
    class ToArray<TSource> : Producer<TSource[]>
    {
        private readonly IObservable<TSource> _source;

        public ToArray(IObservable<TSource> source)
        {
            _source = source;
        }

        protected override IDisposable Run(IObserver<TSource[]> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            var sink = new _(observer, cancel);
            setSink(sink);
            return _source.SubscribeSafe(sink);
        }

        class _ : Sink<TSource[]>, IObserver<TSource>
        {
            private List<TSource> _list;

            public _(IObserver<TSource[]> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _list = new List<TSource>();
            }

            public void OnNext(TSource value)
            {
                _list.Add(value);
            }

            public void OnError(Exception error)
            {
                base._observer.OnError(error);
                base.Dispose();
            }

            public void OnCompleted()
            {
                base._observer.OnNext(_list.ToArray());
                base._observer.OnCompleted();
                base.Dispose();
            }
        }
    }
}
#endif
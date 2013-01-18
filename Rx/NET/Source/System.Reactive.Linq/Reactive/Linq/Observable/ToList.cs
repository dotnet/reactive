// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_PERF
using System;
using System.Collections.Generic;

namespace System.Reactive.Linq.Observαble
{
    class ToList<TSource> : Producer<IList<TSource>>
    {
        private readonly IObservable<TSource> _source;

        public ToList(IObservable<TSource> source)
        {
            _source = source;
        }

        protected override IDisposable Run(IObserver<IList<TSource>> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            var sink = new _(observer, cancel);
            setSink(sink);
            return _source.SubscribeSafe(sink);
        }

        class _ : Sink<IList<TSource>>, IObserver<TSource>
        {
            private List<TSource> _list;

            public _(IObserver<IList<TSource>> observer, IDisposable cancel)
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
                base._observer.OnNext(_list);
                base._observer.OnCompleted();
                base.Dispose();
            }
        }
    }
}
#endif
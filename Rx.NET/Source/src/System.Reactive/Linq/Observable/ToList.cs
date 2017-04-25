// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class ToList<TSource> : Producer<IList<TSource>, ToList<TSource>._>
    {
        private readonly IObservable<TSource> _source;

        public ToList(IObservable<TSource> source)
        {
            _source = source;
        }

        protected override _ CreateSink(IObserver<IList<TSource>> observer, IDisposable cancel) => new _(observer, cancel);

        protected override IDisposable Run(_ sink) => _source.SubscribeSafe(sink);

        internal sealed class _ : Sink<IList<TSource>>, IObserver<TSource>
        {
            private readonly List<TSource> _list;

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

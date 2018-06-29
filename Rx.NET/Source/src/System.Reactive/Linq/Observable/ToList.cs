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

        protected override _ CreateSink(IObserver<IList<TSource>> observer) => new _(observer);

        protected override void Run(_ sink) => sink.Run(_source);

        internal sealed class _ : Sink<TSource, IList<TSource>>
        {
            private List<TSource> _list;

            public _(IObserver<IList<TSource>> observer)
                : base(observer)
            {
                _list = new List<TSource>();
            }

            public override void OnNext(TSource value)
            {
                _list.Add(value);
            }

            public override void OnError(Exception error)
            {
                _list = null;
                ForwardOnError(error);
            }

            public override void OnCompleted()
            {
                var list = _list;
                _list = null;
                ForwardOnNext(list);
                ForwardOnCompleted();
            }
        }
    }
}

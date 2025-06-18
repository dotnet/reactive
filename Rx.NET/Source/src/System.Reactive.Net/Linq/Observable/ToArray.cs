// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class ToArray<TSource> : Producer<TSource[], ToArray<TSource>._>
    {
        private readonly IObservable<TSource> _source;

        public ToArray(IObservable<TSource> source)
        {
            _source = source;
        }

        protected override _ CreateSink(IObserver<TSource[]> observer) => new(observer);

        protected override void Run(_ sink) => sink.Run(_source);

        internal sealed class _ : Sink<TSource, TSource[]>
        {
            private List<TSource> _list = [];

            public _(IObserver<TSource[]> observer)
                : base(observer)
            {
            }

            public override void OnNext(TSource value)
            {
                _list.Add(value);
            }

            public override void OnError(Exception error)
            {
                Cleanup();
                base.OnError(error);
            }

            public override void OnCompleted()
            {
                var list = _list;
                Cleanup();
                ForwardOnNext(list.ToArray());
                ForwardOnCompleted();
            }

            private void Cleanup()
            {
                _list = null!;
            }
        }
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class ElementAtOrDefault<TSource> : Producer<TSource, ElementAtOrDefault<TSource>._>
    {
        private readonly IObservable<TSource> _source;
        private readonly int _index;

        public ElementAtOrDefault(IObservable<TSource> source, int index)
        {
            _source = source;
            _index = index;
        }

        protected override _ CreateSink(IObserver<TSource> observer, IDisposable cancel) => new _(_index, observer, cancel);

        protected override IDisposable Run(_ sink) => _source.SubscribeSafe(sink);

        internal sealed class _ : Sink<TSource>, IObserver<TSource>
        {
            private int _i;

            public _(int index, IObserver<TSource> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _i = index;
            }

            public void OnNext(TSource value)
            {
                if (_i == 0)
                {
                    base._observer.OnNext(value);
                    base._observer.OnCompleted();
                    base.Dispose();
                }

                _i--;
            }

            public void OnError(Exception error)
            {
                base._observer.OnError(error);
                base.Dispose();
            }

            public void OnCompleted()
            {
                base._observer.OnNext(default(TSource));
                base._observer.OnCompleted();
                base.Dispose();
            }
        }
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class ElementAt<TSource> : Producer<TSource, ElementAt<TSource>._>
    {
        private readonly IObservable<TSource> _source;
        private readonly int _index;

        public ElementAt(IObservable<TSource> source, int index)
        {
            _source = source;
            _index = index;
        }

        protected override _ CreateSink(IObserver<TSource> observer, IDisposable cancel) => new _(_index, observer, cancel);

        protected override IDisposable Run(_ sink) => _source.SubscribeSafe(sink);

        internal sealed class _ : IdentitySink<TSource>
        {
            private int _i;

            public _(int index, IObserver<TSource> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _i = index;
            }

            public override void OnNext(TSource value)
            {
                if (_i == 0)
                {
                    ForwardOnNext(value);
                    ForwardOnCompleted();
                }

                _i--;
            }

            public override void OnCompleted()
            {
                ForwardOnError(new ArgumentOutOfRangeException("index"));
            }
        }
    }
}

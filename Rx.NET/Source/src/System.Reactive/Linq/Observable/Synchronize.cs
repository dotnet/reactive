// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class Synchronize<TSource> : Producer<TSource, Synchronize<TSource>._>
    {
        private readonly IObservable<TSource> _source;
        private readonly object _gate;

        public Synchronize(IObservable<TSource> source, object gate)
        {
            _source = source;
            _gate = gate;
        }

        public Synchronize(IObservable<TSource> source)
        {
            _source = source;
        }

        protected override _ CreateSink(IObserver<TSource> observer, IDisposable cancel) => new _(_gate, observer, cancel);

        protected override IDisposable Run(_ sink) => _source.Subscribe(sink);

        internal sealed class _ : Sink<TSource>, IObserver<TSource>
        {
            private readonly object _gate;

            public _(object gate, IObserver<TSource> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _gate = gate ?? new object();
            }

            public void OnNext(TSource value)
            {
                lock (_gate)
                {
                    base._observer.OnNext(value);
                }
            }

            public void OnError(Exception error)
            {
                lock (_gate)
                {
                    base._observer.OnError(error);
                    base.Dispose();
                }
            }

            public void OnCompleted()
            {
                lock (_gate)
                {
                    base._observer.OnCompleted();
                    base.Dispose();
                }
            }
        }
    }
}

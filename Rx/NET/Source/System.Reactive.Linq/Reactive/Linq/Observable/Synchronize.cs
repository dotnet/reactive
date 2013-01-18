// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_PERF
using System;

namespace System.Reactive.Linq.Observαble
{
    class Synchronize<TSource> : Producer<TSource>
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

        protected override IDisposable Run(IObserver<TSource> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            var sink = new _(this, observer, cancel);
            setSink(sink);
            return _source.Subscribe(sink);
        }

        class _ : Sink<TSource>, IObserver<TSource>
        {
            private readonly Synchronize<TSource> _parent;
            private object _gate;

            public _(Synchronize<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
                _gate = _parent._gate ?? new object();
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
#endif
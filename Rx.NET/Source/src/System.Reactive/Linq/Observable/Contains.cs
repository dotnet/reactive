// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class Contains<TSource> : Producer<bool, Contains<TSource>._>
    {
        private readonly IObservable<TSource> _source;
        private readonly TSource _value;
        private readonly IEqualityComparer<TSource> _comparer;

        public Contains(IObservable<TSource> source, TSource value, IEqualityComparer<TSource> comparer)
        {
            _source = source;
            _value = value;
            _comparer = comparer;
        }

        protected override _ CreateSink(IObserver<bool> observer, IDisposable cancel) => new _(this, observer, cancel);

        protected override IDisposable Run(_ sink) => _source.SubscribeSafe(sink);

        internal sealed class _ : Sink<bool>, IObserver<TSource>
        {
            private readonly TSource _value;
            private readonly IEqualityComparer<TSource> _comparer;

            public _(Contains<TSource> parent, IObserver<bool> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _value = parent._value;
                _comparer = parent._comparer;
            }

            public void OnNext(TSource value)
            {
                var res = false;
                try
                {
                    res = _comparer.Equals(value, _value);
                }
                catch (Exception ex)
                {
                    base._observer.OnError(ex);
                    base.Dispose();
                    return;
                }

                if (res)
                {
                    base._observer.OnNext(true);
                    base._observer.OnCompleted();
                    base.Dispose();
                }
            }

            public void OnError(Exception error)
            {
                base._observer.OnError(error);
                base.Dispose();
            }

            public void OnCompleted()
            {
                base._observer.OnNext(false);
                base._observer.OnCompleted();
                base.Dispose();
            }
        }
    }
}

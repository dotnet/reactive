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

        protected override _ CreateSink(IObserver<bool> observer) => new _(this, observer);

        protected override void Run(_ sink) => sink.Run(_source);

        internal sealed class _ : Sink<TSource, bool>
        {
            private readonly TSource _value;
            private readonly IEqualityComparer<TSource> _comparer;

            public _(Contains<TSource> parent, IObserver<bool> observer)
                : base(observer)
            {
                _value = parent._value;
                _comparer = parent._comparer;
            }

            public override void OnNext(TSource value)
            {
                bool res;
                try
                {
                    res = _comparer.Equals(value, _value);
                }
                catch (Exception ex)
                {
                    ForwardOnError(ex);
                    return;
                }

                if (res)
                {
                    ForwardOnNext(true);
                    ForwardOnCompleted();
                }
            }

            public override void OnCompleted()
            {
                ForwardOnNext(false);
                ForwardOnCompleted();
            }
        }
    }
}

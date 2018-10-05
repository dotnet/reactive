// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class All<TSource> : Producer<bool, All<TSource>._>
    {
        private readonly IObservable<TSource> _source;
        private readonly Func<TSource, bool> _predicate;

        public All(IObservable<TSource> source, Func<TSource, bool> predicate)
        {
            _source = source;
            _predicate = predicate;
        }

        protected override _ CreateSink(IObserver<bool> observer) => new _(_predicate, observer);

        protected override void Run(_ sink) => sink.Run(_source);

        internal sealed class _ : Sink<TSource, bool>
        {
            private readonly Func<TSource, bool> _predicate;

            public _(Func<TSource, bool> predicate, IObserver<bool> observer)
                : base(observer)
            {
                _predicate = predicate;
            }

            public override void OnNext(TSource value)
            {
                var res = false;
                try
                {
                    res = _predicate(value);
                }
                catch (Exception ex)
                {
                    ForwardOnError(ex);
                    return;
                }

                if (!res)
                {
                    ForwardOnNext(false);
                    ForwardOnCompleted();
                }
            }

            public override void OnCompleted()
            {
                ForwardOnNext(true);
                ForwardOnCompleted();
            }
        }
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class Scan<TSource, TAccumulate> : Producer<TAccumulate, Scan<TSource, TAccumulate>._>
    {
        private readonly IObservable<TSource> _source;
        private readonly TAccumulate _seed;
        private readonly Func<TAccumulate, TSource, TAccumulate> _accumulator;

        public Scan(IObservable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> accumulator)
        {
            _source = source;
            _seed = seed;
            _accumulator = accumulator;
        }

        protected override _ CreateSink(IObserver<TAccumulate> observer) => new _(this, observer);

        protected override void Run(_ sink) => sink.Run(_source);

        internal sealed class _ : Sink<TSource, TAccumulate>
        {
            private readonly Func<TAccumulate, TSource, TAccumulate> _accumulator;
            private TAccumulate _accumulation;

            public _(Scan<TSource, TAccumulate> parent, IObserver<TAccumulate> observer)
                : base(observer)
            {
                _accumulator = parent._accumulator;
                _accumulation = parent._seed;
            }

            public override void OnNext(TSource value)
            {
                try
                {
                    _accumulation = _accumulator(_accumulation, value);
                }
                catch (Exception exception)
                {
                    ForwardOnError(exception);
                    return;
                }

                ForwardOnNext(_accumulation);
            }
        }
    }

    internal sealed class Scan<TSource> : Producer<TSource, Scan<TSource>._>
    {
        private readonly IObservable<TSource> _source;
        private readonly Func<TSource, TSource, TSource> _accumulator;

        public Scan(IObservable<TSource> source, Func<TSource, TSource, TSource> accumulator)
        {
            _source = source;
            _accumulator = accumulator;
        }

        protected override _ CreateSink(IObserver<TSource> observer) => new _(_accumulator, observer);

        protected override void Run(_ sink) => sink.Run(_source);

        internal sealed class _ : IdentitySink<TSource>
        {
            private readonly Func<TSource, TSource, TSource> _accumulator;
            private TSource _accumulation;
            private bool _hasAccumulation;

            public _(Func<TSource, TSource, TSource> accumulator, IObserver<TSource> observer)
                : base(observer)
            {
                _accumulator = accumulator;
            }

            public override void OnNext(TSource value)
            {
                try
                {
                    if (_hasAccumulation)
                    {
                        _accumulation = _accumulator(_accumulation, value);
                    }
                    else
                    {
                        _accumulation = value;
                        _hasAccumulation = true;
                    }
                }
                catch (Exception exception)
                {
                    ForwardOnError(exception);

                    return;
                }

                ForwardOnNext(_accumulation);
            }
        }
    }
}

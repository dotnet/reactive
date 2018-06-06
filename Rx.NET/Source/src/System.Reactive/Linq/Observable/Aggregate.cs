// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class Aggregate<TSource> : Producer<TSource, Aggregate<TSource>._>
    {
        private readonly IObservable<TSource> _source;
        private readonly Func<TSource, TSource, TSource> _accumulator;

        public Aggregate(IObservable<TSource> source, Func<TSource, TSource, TSource> accumulator)
        {
            _source = source;
            _accumulator = accumulator;
        }

        protected override _ CreateSink(IObserver<TSource> observer, IDisposable cancel) => new _(_accumulator, observer, cancel);

        protected override IDisposable Run(_ sink) => _source.SubscribeSafe(sink);

        internal sealed class _ : IdentitySink<TSource>
        {
            private readonly Func<TSource, TSource, TSource> _accumulator;
            private TSource _accumulation;
            private bool _hasAccumulation;

            public _(Func<TSource, TSource, TSource> accumulator, IObserver<TSource> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _accumulator = accumulator;
                _accumulation = default(TSource);
                _hasAccumulation = false;
            }

            public override void OnNext(TSource value)
            {
                if (!_hasAccumulation)
                {
                    _accumulation = value;
                    _hasAccumulation = true;
                }
                else
                {
                    try
                    {
                        _accumulation = _accumulator(_accumulation, value);
                    }
                    catch (Exception exception)
                    {
                        ForwardOnError(exception);
                    }
                }
            }

            public override void OnCompleted()
            {
                if (!_hasAccumulation)
                {
                    ForwardOnError(new InvalidOperationException(Strings_Linq.NO_ELEMENTS));
                }
                else
                {
                    ForwardOnNext(_accumulation);
                    ForwardOnCompleted();
                }
            }
        }
    }

    internal sealed class Aggregate<TSource, TAccumulate> : Producer<TAccumulate, Aggregate<TSource, TAccumulate>._>
    {
        private readonly IObservable<TSource> _source;
        private readonly TAccumulate _seed;
        private readonly Func<TAccumulate, TSource, TAccumulate> _accumulator;

        public Aggregate(IObservable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> accumulator)
        {
            _source = source;
            _seed = seed;
            _accumulator = accumulator;
        }

        protected override _ CreateSink(IObserver<TAccumulate> observer, IDisposable cancel) => new _(_seed, _accumulator, observer, cancel);

        protected override IDisposable Run(_ sink) => _source.SubscribeSafe(sink);

        internal sealed class _ : Sink<TSource, TAccumulate> 
        {
            private readonly Func<TAccumulate, TSource, TAccumulate> _accumulator;
            private TAccumulate _accumulation;

            public _(TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> accumulator, IObserver<TAccumulate> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _accumulator = accumulator;
                _accumulation = seed;
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
                }
            }

            public override void OnError(Exception error)
            {
                ForwardOnError(error);
            }

            public override void OnCompleted()
            {
                ForwardOnNext(_accumulation);
                ForwardOnCompleted();
            }
        }
    }

    internal sealed class Aggregate<TSource, TAccumulate, TResult> : Producer<TResult, Aggregate<TSource, TAccumulate, TResult>._>
    {
        private readonly IObservable<TSource> _source;
        private readonly TAccumulate _seed;
        private readonly Func<TAccumulate, TSource, TAccumulate> _accumulator;
        private readonly Func<TAccumulate, TResult> _resultSelector;

        public Aggregate(IObservable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> accumulator, Func<TAccumulate, TResult> resultSelector)
        {
            _source = source;
            _seed = seed;
            _accumulator = accumulator;
            _resultSelector = resultSelector;
        }

        protected override _ CreateSink(IObserver<TResult> observer, IDisposable cancel) => new _(this, observer, cancel);

        protected override IDisposable Run(_ sink) => _source.SubscribeSafe(sink);

        internal sealed class _ : Sink<TSource, TResult> 
        {
            private readonly Func<TAccumulate, TSource, TAccumulate> _accumulator;
            private readonly Func<TAccumulate, TResult> _resultSelector;

            private TAccumulate _accumulation;

            public _(Aggregate<TSource, TAccumulate, TResult> parent, IObserver<TResult> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _accumulator = parent._accumulator;
                _resultSelector = parent._resultSelector;

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
                }
            }

            public override void OnError(Exception error)
            {
                ForwardOnError(error);
            }

            public override void OnCompleted()
            {
                var result = default(TResult);
                try
                {
                    result = _resultSelector(_accumulation);
                }
                catch (Exception exception)
                {
                    ForwardOnError(exception);
                    return;
                }

                ForwardOnNext(result);
                ForwardOnCompleted();
            }
        }
    }
}

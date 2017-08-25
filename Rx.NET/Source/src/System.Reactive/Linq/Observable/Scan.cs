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

        protected override _ CreateSink(IObserver<TAccumulate> observer, IDisposable cancel) => new _(this, observer, cancel);

        protected override IDisposable Run(_ sink) => _source.SubscribeSafe(sink);

        internal sealed class _ : Sink<TAccumulate>, IObserver<TSource>
        {
            private readonly Func<TAccumulate, TSource, TAccumulate> _accumulator;
            private TAccumulate _accumulation;

            public _(Scan<TSource, TAccumulate> parent, IObserver<TAccumulate> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _accumulator = parent._accumulator;
                _accumulation = parent._seed;
            }

            public void OnNext(TSource value)
            {
                try
                {
                    _accumulation = _accumulator(_accumulation, value);
                }
                catch (Exception exception)
                {
                    base._observer.OnError(exception);
                    base.Dispose();
                    return;
                }

                base._observer.OnNext(_accumulation);
            }

            public void OnError(Exception error)
            {
                base._observer.OnError(error);
                base.Dispose();
            }

            public void OnCompleted()
            {
                base._observer.OnCompleted();
                base.Dispose();
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

        protected override _ CreateSink(IObserver<TSource> observer, IDisposable cancel) => new _(_accumulator, observer, cancel);

        protected override IDisposable Run(_ sink) => _source.SubscribeSafe(sink);

        internal sealed class _ : Sink<TSource>, IObserver<TSource>
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

            public void OnNext(TSource value)
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
                    base._observer.OnError(exception);
                    base.Dispose();
                    return;
                }

                base._observer.OnNext(_accumulation);
            }

            public void OnError(Exception error)
            {
                base._observer.OnError(error);
                base.Dispose();
            }

            public void OnCompleted()
            {
                base._observer.OnCompleted();
                base.Dispose();
            }
        }
    }
}

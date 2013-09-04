// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_PERF
using System;

namespace System.Reactive.Linq.Observαble
{
    class Aggregate<TSource, TAccumulate, TResult> : Producer<TResult>
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

        protected override IDisposable Run(IObserver<TResult> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            var sink = new _(this, observer, cancel);
            setSink(sink);
            return _source.SubscribeSafe(sink);
        }

        class _ : Sink<TResult>, IObserver<TSource>
        {
            private readonly Aggregate<TSource, TAccumulate, TResult> _parent;
            private TAccumulate _accumulation;

            public _(Aggregate<TSource, TAccumulate, TResult> parent, IObserver<TResult> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
                _accumulation = _parent._seed;
            }

            public void OnNext(TSource value)
            {
                try
                {
                    _accumulation = _parent._accumulator(_accumulation, value);
                }
                catch (Exception exception)
                {
                    base._observer.OnError(exception);
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
                var result = default(TResult);
                try
                {
                    result = _parent._resultSelector(_accumulation);
                }
                catch (Exception exception)
                {
                    base._observer.OnError(exception);
                    base.Dispose();
                    return;
                }

                base._observer.OnNext(result);
                base._observer.OnCompleted();
                base.Dispose();
            }
        }
    }

    class Aggregate<TSource> : Producer<TSource>
    {
        private readonly IObservable<TSource> _source;
        private readonly Func<TSource, TSource, TSource> _accumulator;

        public Aggregate(IObservable<TSource> source, Func<TSource, TSource, TSource> accumulator)
        {
            _source = source;
            _accumulator = accumulator;
        }

        protected override IDisposable Run(IObserver<TSource> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            var sink = new _(this, observer, cancel);
            setSink(sink);
            return _source.SubscribeSafe(sink);
        }

        class _ : Sink<TSource>, IObserver<TSource>
        {
            private readonly Aggregate<TSource> _parent;
            private TSource _accumulation;
            private bool _hasAccumulation;

            public _(Aggregate<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
                _accumulation = default(TSource);
                _hasAccumulation = false;
            }

            public void OnNext(TSource value)
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
                        _accumulation = _parent._accumulator(_accumulation, value);
                    }
                    catch (Exception exception)
                    {
                        base._observer.OnError(exception);
                        base.Dispose();
                    }
                }
            }

            public void OnError(Exception error)
            {
                base._observer.OnError(error);
                base.Dispose();
            }

            public void OnCompleted()
            {
                if (!_hasAccumulation)
                {
                    base._observer.OnError(new InvalidOperationException(Strings_Linq.NO_ELEMENTS));
                    base.Dispose();
                }
                else
                {
                    base._observer.OnNext(_accumulation);
                    base._observer.OnCompleted();
                    base.Dispose();
                }
            }
        }
    }
}
#endif
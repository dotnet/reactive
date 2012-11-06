// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_PERF
using System;

namespace System.Reactive.Linq.Observαble
{
    class Scan<TSource, TAccumulate> : Producer<TAccumulate>
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

        protected override IDisposable Run(IObserver<TAccumulate> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            var sink = new _(this, observer, cancel);
            setSink(sink);
            return _source.SubscribeSafe(sink);
        }

        class _ : Sink<TAccumulate>, IObserver<TSource>
        {
            private readonly Scan<TSource, TAccumulate> _parent;
            private TAccumulate _accumulation;
            private bool _hasAccumulation;

            public _(Scan<TSource, TAccumulate> parent, IObserver<TAccumulate> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
                _accumulation = default(TAccumulate);
                _hasAccumulation = false;
            }

            public void OnNext(TSource value)
            {
                try
                {
                    if (_hasAccumulation)
                        _accumulation = _parent._accumulator(_accumulation, value);
                    else
                    {
                        _accumulation = _parent._accumulator(_parent._seed, value);
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

    class Scan<TSource> : Producer<TSource>
    {
        private readonly IObservable<TSource> _source;
        private readonly Func<TSource, TSource, TSource> _accumulator;

        public Scan(IObservable<TSource> source, Func<TSource, TSource, TSource> accumulator)
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
            private readonly Scan<TSource> _parent;
            private TSource _accumulation;
            private bool _hasAccumulation;

            public _(Scan<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
                _accumulation = default(TSource);
                _hasAccumulation = false;
            }

            public void OnNext(TSource value)
            {
                try
                {
                    if (_hasAccumulation)
                        _accumulation = _parent._accumulator(_accumulation, value);
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
#endif
// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_PERF
using System;

namespace System.Reactive.Linq.Observαble
{
    class LongCount<TSource> : Producer<long>
    {
        private readonly IObservable<TSource> _source;
        private readonly Func<TSource, bool> _predicate;

        public LongCount(IObservable<TSource> source)
        {
            _source = source;
        }

        public LongCount(IObservable<TSource> source, Func<TSource, bool> predicate)
        {
            _source = source;
            _predicate = predicate;
        }

        protected override IDisposable Run(IObserver<long> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            if (_predicate == null)
            {
                var sink = new _(observer, cancel);
                setSink(sink);
                return _source.SubscribeSafe(sink);
            }
            else
            {
                var sink = new π(this, observer, cancel);
                setSink(sink);
                return _source.SubscribeSafe(sink);
            }
        }

        class _ : Sink<long>, IObserver<TSource>
        {
            private long _count;

            public _(IObserver<long> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _count = 0L;
            }

            public void OnNext(TSource value)
            {
                try
                {
                    checked
                    {
                        _count++;
                    }
                }
                catch (Exception ex)
                {
                    base._observer.OnError(ex);
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
                base._observer.OnNext(_count);
                base._observer.OnCompleted();
                base.Dispose();
            }
        }

        class π : Sink<long>, IObserver<TSource>
        {
            private readonly LongCount<TSource> _parent;
            private long _count;

            public π(LongCount<TSource> parent, IObserver<long> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
                _count = 0L;
            }

            public void OnNext(TSource value)
            {
                try
                {
                    checked
                    {
                        if (_parent._predicate(value))
                            _count++;
                    }
                }
                catch (Exception ex)
                {
                    base._observer.OnError(ex);
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
                base._observer.OnNext(_count);
                base._observer.OnCompleted();
                base.Dispose();
            }
        }
    }
}
#endif
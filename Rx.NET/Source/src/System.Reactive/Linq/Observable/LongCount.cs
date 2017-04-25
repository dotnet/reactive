// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive.Linq.ObservableImpl
{
    internal static class LongCount<TSource>
    {
        internal sealed class All : Producer<long, All._>
        {
            private readonly IObservable<TSource> _source;

            public All(IObservable<TSource> source)
            {
                _source = source;
            }

            protected override _ CreateSink(IObserver<long> observer, IDisposable cancel) => new _(observer, cancel);

            protected override IDisposable Run(_ sink) => _source.SubscribeSafe(sink);

            internal sealed class _ : Sink<long>, IObserver<TSource>
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
        }

        internal sealed class Predicate : Producer<long, Predicate._>
        {
            private readonly IObservable<TSource> _source;
            private readonly Func<TSource, bool> _predicate;

            public Predicate(IObservable<TSource> source, Func<TSource, bool> predicate)
            {
                _source = source;
                _predicate = predicate;
            }

            protected override _ CreateSink(IObserver<long> observer, IDisposable cancel) => new _(_predicate, observer, cancel);

            protected override IDisposable Run(_ sink) => _source.SubscribeSafe(sink);

            internal sealed class _ : Sink<long>, IObserver<TSource>
            {
                private readonly Func<TSource, bool> _predicate;
                private long _count;

                public _(Func<TSource, bool> predicate, IObserver<long> observer, IDisposable cancel)
                    : base(observer, cancel)
                {
                    _predicate = predicate;
                    _count = 0L;
                }

                public void OnNext(TSource value)
                {
                    try
                    {
                        checked
                        {
                            if (_predicate(value))
                            {
                                _count++;
                            }
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
}

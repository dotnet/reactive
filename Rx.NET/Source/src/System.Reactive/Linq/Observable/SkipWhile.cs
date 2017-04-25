// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive.Linq.ObservableImpl
{
    internal static class SkipWhile<TSource>
    {
        internal sealed class Predicate : Producer<TSource, Predicate._>
        {
            private readonly IObservable<TSource> _source;
            private readonly Func<TSource, bool> _predicate;

            public Predicate(IObservable<TSource> source, Func<TSource, bool> predicate)
            {
                _source = source;
                _predicate = predicate;
            }

            protected override _ CreateSink(IObserver<TSource> observer, IDisposable cancel) => new _(_predicate, observer, cancel);

            protected override IDisposable Run(_ sink) => _source.SubscribeSafe(sink);

            internal sealed class _ : Sink<TSource>, IObserver<TSource>
            {
                private Func<TSource, bool> _predicate;

                public _(Func<TSource, bool> predicate, IObserver<TSource> observer, IDisposable cancel)
                    : base(observer, cancel)
                {
                    _predicate = predicate;
                }

                public void OnNext(TSource value)
                {
                    if (_predicate != null)
                    {
                        var shouldStart = default(bool);
                        try
                        {
                            shouldStart = !_predicate(value);
                        }
                        catch (Exception exception)
                        {
                            base._observer.OnError(exception);
                            base.Dispose();
                            return;
                        }

                        if (shouldStart)
                        {
                            _predicate = null;

                            base._observer.OnNext(value);
                        }
                    }
                    else
                    {
                        base._observer.OnNext(value);
                    }
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

        internal sealed class PredicateIndexed : Producer<TSource, PredicateIndexed._>
        {
            private readonly IObservable<TSource> _source;
            private readonly Func<TSource, int, bool> _predicate;

            public PredicateIndexed(IObservable<TSource> source, Func<TSource, int, bool> predicate)
            {
                _source = source;
                _predicate = predicate;
            }

            protected override _ CreateSink(IObserver<TSource> observer, IDisposable cancel) => new _(_predicate, observer, cancel);

            protected override IDisposable Run(_ sink) => _source.SubscribeSafe(sink);

            internal sealed class _ : Sink<TSource>, IObserver<TSource>
            {
                private Func<TSource, int, bool> _predicate;
                private int _index;

                public _(Func<TSource, int, bool> predicate, IObserver<TSource> observer, IDisposable cancel)
                    : base(observer, cancel)
                {
                    _predicate = predicate;
                    _index = 0;
                }

                public void OnNext(TSource value)
                {
                    if (_predicate != null)
                    {
                        var shouldStart = default(bool);
                        try
                        {
                            shouldStart = !_predicate(value, checked(_index++));
                        }
                        catch (Exception exception)
                        {
                            base._observer.OnError(exception);
                            base.Dispose();
                            return;
                        }

                        if (shouldStart)
                        {
                            _predicate = null;

                            base._observer.OnNext(value);
                        }
                    }
                    else
                    {
                        base._observer.OnNext(value);
                    }
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
}

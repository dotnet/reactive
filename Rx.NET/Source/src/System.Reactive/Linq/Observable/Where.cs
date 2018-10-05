// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive.Linq.ObservableImpl
{
    internal static class Where<TSource>
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

            public IObservable<TSource> Combine(Func<TSource, bool> predicate)
            {
                return new Predicate(_source, x => _predicate(x) && predicate(x));
            }

            protected override _ CreateSink(IObserver<TSource> observer) => new _(_predicate, observer);

            protected override void Run(_ sink) => sink.Run(_source);

            internal sealed class _ : IdentitySink<TSource>
            {
                private readonly Func<TSource, bool> _predicate;

                public _(Func<TSource, bool> predicate, IObserver<TSource> observer)
                    : base(observer)
                {
                    _predicate = predicate;
                }

                public override void OnNext(TSource value)
                {
                    var shouldRun = false;
                    try
                    {
                        shouldRun = _predicate(value);
                    }
                    catch (Exception exception)
                    {
                        ForwardOnError(exception);
                        return;
                    }

                    if (shouldRun)
                    {
                        ForwardOnNext(value);
                    }
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

            protected override _ CreateSink(IObserver<TSource> observer) => new _(_predicate, observer);

            protected override void Run(_ sink) => sink.Run(_source);

            internal sealed class _ : IdentitySink<TSource>
            {
                private readonly Func<TSource, int, bool> _predicate;
                private int _index;

                public _(Func<TSource, int, bool> predicate, IObserver<TSource> observer)
                    : base(observer)
                {
                    _predicate = predicate;
                }

                public override void OnNext(TSource value)
                {
                    var shouldRun = false;
                    try
                    {
                        shouldRun = _predicate(value, checked(_index++));
                    }
                    catch (Exception exception)
                    {
                        ForwardOnError(exception);
                    }

                    if (shouldRun)
                    {
                        ForwardOnNext(value);
                    }
                }
            }
        }
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive.Linq.ObservableImpl
{
    internal static class SingleAsync<TSource>
    {
        internal sealed class Sequence : Producer<TSource, Sequence._>
        {
            private readonly IObservable<TSource> _source;

            public Sequence(IObservable<TSource> source)
            {
                _source = source;
            }

            protected override _ CreateSink(IObserver<TSource> observer) => new _(observer);

            protected override void Run(_ sink) => sink.Run(_source);

            internal sealed class _ : IdentitySink<TSource>
            {
                private TSource _value;
                private bool _seenValue;

                public _(IObserver<TSource> observer)
                    : base(observer)
                {
                }

                public override void OnNext(TSource value)
                {
                    if (_seenValue)
                    {
                        ForwardOnError(new InvalidOperationException(Strings_Linq.MORE_THAN_ONE_ELEMENT));
                        return;
                    }

                    _value = value;
                    _seenValue = true;
                }

                public override void OnCompleted()
                {
                    if (!_seenValue)
                    {
                        ForwardOnError(new InvalidOperationException(Strings_Linq.NO_ELEMENTS));
                    }
                    else
                    {
                        ForwardOnNext(_value);
                        ForwardOnCompleted();
                    }
                }
            }
        }

        internal sealed class Predicate : Producer<TSource, Predicate._>
        {
            private readonly IObservable<TSource> _source;
            private readonly Func<TSource, bool> _predicate;

            public Predicate(IObservable<TSource> source, Func<TSource, bool> predicate)
            {
                _source = source;
                _predicate = predicate;
            }

            protected override _ CreateSink(IObserver<TSource> observer) => new _(_predicate, observer);

            protected override void Run(_ sink) => sink.Run(_source);

            internal sealed class _ : IdentitySink<TSource>
            {
                private readonly Func<TSource, bool> _predicate;
                private TSource _value;
                private bool _seenValue;

                public _(Func<TSource, bool> predicate, IObserver<TSource> observer)
                    : base(observer)
                {
                    _predicate = predicate;
                }

                public override void OnNext(TSource value)
                {
                    var b = false;

                    try
                    {
                        b = _predicate(value);
                    }
                    catch (Exception ex)
                    {
                        ForwardOnError(ex);
                        return;
                    }

                    if (b)
                    {
                        if (_seenValue)
                        {
                            ForwardOnError(new InvalidOperationException(Strings_Linq.MORE_THAN_ONE_MATCHING_ELEMENT));
                            return;
                        }

                        _value = value;
                        _seenValue = true;
                    }
                }

                public override void OnCompleted()
                {
                    if (!_seenValue)
                    {
                        ForwardOnError(new InvalidOperationException(Strings_Linq.NO_MATCHING_ELEMENTS));
                    }
                    else
                    {
                        ForwardOnNext(_value);
                        ForwardOnCompleted();
                    }
                }
            }
        }
    }
}

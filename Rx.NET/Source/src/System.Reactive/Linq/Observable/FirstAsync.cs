// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive.Linq.ObservableImpl
{
    internal static class FirstAsync<TSource>
    {
        internal sealed class Sequence : Producer<TSource, Sequence._>
        {
            private readonly IObservable<TSource> _source;

            public Sequence(IObservable<TSource> source)
            {
                _source = source;
            }

            protected override _ CreateSink(IObserver<TSource> observer) => new(observer);

            protected override void Run(_ sink) => sink.Run(_source);

            internal sealed class _ : IdentitySink<TSource>
            {
                private bool _found;

                public _(IObserver<TSource> observer)
                    : base(observer)
                {
                }

                public override void OnNext(TSource value)
                {
                    _found = true;
                    ForwardOnNext(value);
                    ForwardOnCompleted();
                }

                public override void OnCompleted()
                {
                    if (!_found)
                    {
                        try
                        {
                            throw new InvalidOperationException(Strings_Linq.NO_ELEMENTS);
                        }
                        catch (Exception e)
                        {
                            ForwardOnError(e);
                        }
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

            protected override _ CreateSink(IObserver<TSource> observer) => new(_predicate, observer);

            protected override void Run(_ sink) => sink.Run(_source);

            internal sealed class _ : IdentitySink<TSource>
            {
                private readonly Func<TSource, bool> _predicate;
                private bool _found;

                public _(Func<TSource, bool> predicate, IObserver<TSource> observer)
                    : base(observer)
                {
                    _predicate = predicate;
                }

                public override void OnNext(TSource value)
                {
                    bool b;

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
                        _found = true;
                        ForwardOnNext(value);
                        ForwardOnCompleted();
                    }
                }

                public override void OnCompleted()
                {
                    if (!_found)
                    {
                        try
                        {
                            throw new InvalidOperationException(Strings_Linq.NO_MATCHING_ELEMENTS);
                        }
                        catch (Exception e)
                        {
                            ForwardOnError(e);
                        }
                    }
                }
            }
        }
    }
}

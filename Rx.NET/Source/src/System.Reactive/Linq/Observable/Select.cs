// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive.Linq.ObservableImpl
{
    internal static class Select<TSource, TResult>
    {
        internal sealed class Selector : Producer<TResult, Selector._>
        {
            private readonly IObservable<TSource> _source;
            private readonly Func<TSource, TResult> _selector;

            public Selector(IObservable<TSource> source, Func<TSource, TResult> selector)
            {
                _source = source;
                _selector = selector;
            }

            protected override _ CreateSink(IObserver<TResult> observer) => new _(_selector, observer);

            protected override void Run(_ sink) => sink.Run(_source);

            internal sealed class _ : Sink<TSource, TResult>
            {
                private readonly Func<TSource, TResult> _selector;

                public _(Func<TSource, TResult> selector, IObserver<TResult> observer)
                    : base(observer)
                {
                    _selector = selector;
                }

                public override void OnNext(TSource value)
                {
                    TResult result;
                    try
                    {
                        result = _selector(value);
                    }
                    catch (Exception exception)
                    {
                        ForwardOnError(exception);
                        return;
                    }

                    ForwardOnNext(result);
                }
            }
        }

        internal sealed class SelectorIndexed : Producer<TResult, SelectorIndexed._>
        {
            private readonly IObservable<TSource> _source;
            private readonly Func<TSource, int, TResult> _selector;

            public SelectorIndexed(IObservable<TSource> source, Func<TSource, int, TResult> selector)
            {
                _source = source;
                _selector = selector;
            }

            protected override _ CreateSink(IObserver<TResult> observer) => new _(_selector, observer);

            protected override void Run(_ sink) => sink.Run(_source);

            internal sealed class _ : Sink<TSource, TResult>
            {
                private readonly Func<TSource, int, TResult> _selector;
                private int _index;

                public _(Func<TSource, int, TResult> selector, IObserver<TResult> observer)
                    : base(observer)
                {
                    _selector = selector;
                }

                public override void OnNext(TSource value)
                {
                    TResult result;
                    try
                    {
                        result = _selector(value, checked(_index++));
                    }
                    catch (Exception exception)
                    {
                        ForwardOnError(exception);
                        return;
                    }

                    ForwardOnNext(result);
                }
            }
        }
    }
}

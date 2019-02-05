// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive.Linq.ObservableImpl
{
    internal static class Select<TSource, TResult>
    {
        internal sealed class Selector : Pipe<TSource, TResult>
        {
            private readonly Func<TSource, TResult> _selector;

            public Selector(IObservable<TSource> source, Func<TSource, TResult> selector) : base(source)
            {
                _selector = selector;
            }

            protected override Pipe<TSource, TResult> Clone() => new Selector(_source, _selector);
            
            public override void OnNext(TSource value)
            {
                var result = default(TResult);
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

        internal sealed class SelectorIndexed : Pipe<TSource, TResult>
        {
            private readonly Func<TSource, int, TResult> _selector;
            private int _index;

            public SelectorIndexed(IObservable<TSource> source, Func<TSource, int, TResult> selector) : base(source)
            {
                _selector = selector;
            }

            protected override Pipe<TSource, TResult> Clone() => new SelectorIndexed(_source, _selector);

            public override void OnNext(TSource value)
            {
                var result = default(TResult);
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

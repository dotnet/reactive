// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive.Linq.ObservableImpl
{
    internal static class Where<TSource>
    {
        internal sealed class Predicate : Pipe<TSource, TSource>
        {
            private readonly Func<TSource, bool> _predicate;

            public Predicate(IObservable<TSource> source, Func<TSource, bool> predicate) : base(source)
            {
                _predicate = predicate;
            }

            protected override Pipe<TSource, TSource> Clone() => new Predicate(_source, _predicate);

            public IObservable<TSource> Combine(Func<TSource, bool> predicate)
            {
                return new Predicate(_source, x => _predicate(x) && predicate(x));
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

        internal sealed class PredicateIndexed : Pipe<TSource, TSource>
        {
            private readonly Func<TSource, int, bool> _predicate;
            private int _index;

            public PredicateIndexed(IObservable<TSource> source, Func<TSource, int, bool> predicate) : base(source)
            {
                _predicate = predicate;
            }

            protected override Pipe<TSource, TSource> Clone() => new PredicateIndexed(_source, _predicate);
            
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

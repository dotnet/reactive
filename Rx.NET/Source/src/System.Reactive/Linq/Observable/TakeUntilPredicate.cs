// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 


namespace System.Reactive.Linq.ObservableImpl
{
    /// <summary>
    /// Relays items to the downstream until the predicate returns <code>true</code>.
    /// </summary>
    /// <typeparam name="TSource">The element type of the sequence</typeparam>
    internal sealed class TakeUntilPredicate<TSource> :
        Producer<TSource, TakeUntilPredicate<TSource>.TakeUntilPredicateObserver>
    {
        private readonly IObservable<TSource> _source;
        private readonly Func<TSource, bool> _stopPredicate;

        public TakeUntilPredicate(IObservable<TSource> source, Func<TSource, bool> stopPredicate)
        {
            _source = source;
            _stopPredicate = stopPredicate;
        }

        protected override TakeUntilPredicateObserver CreateSink(IObserver<TSource> observer) => new TakeUntilPredicateObserver(observer, _stopPredicate);

        protected override void Run(TakeUntilPredicateObserver sink) => sink.Run(_source);

        internal sealed class TakeUntilPredicateObserver : IdentitySink<TSource>
        {
            private readonly Func<TSource, bool> _stopPredicate;

            public TakeUntilPredicateObserver(IObserver<TSource> downstream,
                Func<TSource, bool> predicate) : base(downstream)
            {
                _stopPredicate = predicate;
            }

            public override void OnCompleted()
            {
                ForwardOnCompleted();
            }

            public override void OnError(Exception error)
            {
                ForwardOnError(error);
            }

            public override void OnNext(TSource value)
            {
                ForwardOnNext(value);

                var shouldStop = false;
                try
                {
                    shouldStop = _stopPredicate(value);
                }
                catch (Exception ex)
                {
                    ForwardOnError(ex);
                    return;
                }
                if (shouldStop)
                {
                    ForwardOnCompleted();
                }
            }
        }
    }
}

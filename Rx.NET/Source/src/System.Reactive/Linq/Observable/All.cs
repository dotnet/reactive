// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class All<TSource> : Producer<bool, All<TSource>._>
    {
        private readonly IObservable<TSource> _source;
        private readonly Func<TSource, bool> _predicate;

        public All(IObservable<TSource> source, Func<TSource, bool> predicate)
        {
            _source = source;
            _predicate = predicate;
        }

        protected override _ CreateSink(IObserver<bool> observer, IDisposable cancel) => new _(_predicate, observer, cancel);

        protected override IDisposable Run(_ sink) => _source.SubscribeSafe(sink);

        internal sealed class _ : Sink<bool>, IObserver<TSource>
        {
            private readonly Func<TSource, bool> _predicate;

            public _(Func<TSource, bool> predicate, IObserver<bool> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _predicate = predicate;
            }

            public void OnNext(TSource value)
            {
                var res = false;
                try
                {
                    res = _predicate(value);
                }
                catch (Exception ex)
                {
                    base._observer.OnError(ex);
                    base.Dispose();
                    return;
                }

                if (!res)
                {
                    base._observer.OnNext(false);
                    base._observer.OnCompleted();
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
                base._observer.OnNext(true);
                base._observer.OnCompleted();
                base.Dispose();
            }
        }
    }
}

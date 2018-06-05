// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive.Linq.ObservableImpl
{
    internal static class Any<TSource>
    {
        internal sealed class Count : Producer<bool, Count._>
        {
            private readonly IObservable<TSource> _source;

            public Count(IObservable<TSource> source)
            {
                _source = source;
            }

            protected override _ CreateSink(IObserver<bool> observer, IDisposable cancel) => new _(observer, cancel);

            protected override IDisposable Run(_ sink) => _source.SubscribeSafe(sink);

            internal sealed class _ : Sink<TSource, bool> 
            {
                public _(IObserver<bool> observer, IDisposable cancel)
                    : base(observer, cancel)
                {
                }

                public override void OnNext(TSource value)
                {
                    ForwardOnNext(true);
                    ForwardOnCompleted();
                }

                public override void OnCompleted()
                {
                    ForwardOnNext(false);
                    ForwardOnCompleted();
                }
            }
        }

        internal sealed class Predicate : Producer<bool, Predicate._>
        {
            private readonly IObservable<TSource> _source;
            private readonly Func<TSource, bool> _predicate;

            public Predicate(IObservable<TSource> source, Func<TSource, bool> predicate)
            {
                _source = source;
                _predicate = predicate;
            }

            protected override _ CreateSink(IObserver<bool> observer, IDisposable cancel) => new _(_predicate, observer, cancel);

            protected override IDisposable Run(_ sink) => _source.SubscribeSafe(sink);

            internal sealed class _ : Sink<TSource, bool> 
            {
                private readonly Func<TSource, bool> _predicate;

                public _(Func<TSource, bool> predicate, IObserver<bool> observer, IDisposable cancel)
                    : base(observer, cancel)
                {
                    _predicate = predicate;
                }

                public override void OnNext(TSource value)
                {
                    var res = false;
                    try
                    {
                        res = _predicate(value);
                    }
                    catch (Exception ex)
                    {
                        ForwardOnError(ex);
                        return;
                    }

                    if (res)
                    {
                        ForwardOnNext(true);
                        ForwardOnCompleted();
                    }
                }

                public override void OnCompleted()
                {
                    ForwardOnNext(false);
                    ForwardOnCompleted();
                }
            }
        }
    }
}

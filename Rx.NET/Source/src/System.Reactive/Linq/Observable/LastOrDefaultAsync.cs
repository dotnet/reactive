// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive.Linq.ObservableImpl
{
    internal static class LastOrDefaultAsync<TSource>
    {
        internal sealed class Sequence : Producer<TSource, Sequence._>
        {
            private readonly IObservable<TSource> _source;

            public Sequence(IObservable<TSource> source)
            {
                _source = source;
            }

            protected override _ CreateSink(IObserver<TSource> observer, IDisposable cancel) => new _(observer, cancel);

            protected override IDisposable Run(_ sink) => _source.SubscribeSafe(sink);

            internal sealed class _ : IdentitySink<TSource>
            {
                private TSource _value;

                public _(IObserver<TSource> observer, IDisposable cancel)
                    : base(observer, cancel)
                {
                    _value = default(TSource);
                }

                public override void OnNext(TSource value)
                {
                    _value = value;
                }

                public override void OnCompleted()
                {
                    ForwardOnNext(_value);
                    ForwardOnCompleted();
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

            protected override _ CreateSink(IObserver<TSource> observer, IDisposable cancel) => new _(_predicate, observer, cancel);

            protected override IDisposable Run(_ sink) => _source.SubscribeSafe(sink);

            internal sealed class _ : IdentitySink<TSource>
            {
                private readonly Func<TSource, bool> _predicate;
                private TSource _value;

                public _(Func<TSource, bool> predicate, IObserver<TSource> observer, IDisposable cancel)
                    : base(observer, cancel)
                {
                    _predicate = predicate;

                    _value = default(TSource);
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
                        _value = value;
                    }
                }

                public override void OnCompleted()
                {
                    ForwardOnNext(_value);
                    ForwardOnCompleted();
                }
            }
        }
    }
}

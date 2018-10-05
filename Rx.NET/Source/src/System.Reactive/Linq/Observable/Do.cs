// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive.Linq.ObservableImpl
{
    internal static class Do<TSource>
    {
        internal sealed class OnNext : Producer<TSource, OnNext._>
        {
            private readonly IObservable<TSource> _source;
            private readonly Action<TSource> _onNext;

            public OnNext(IObservable<TSource> source, Action<TSource> onNext)
            {
                _source = source;
                _onNext = onNext;
            }

            protected override _ CreateSink(IObserver<TSource> observer) => new _(_onNext, observer);

            protected override void Run(_ sink) => sink.Run(_source);

            internal sealed class _ : IdentitySink<TSource>
            {
                private readonly Action<TSource> _onNext;

                public _(Action<TSource> onNext, IObserver<TSource> observer)
                    : base(observer)
                {
                    _onNext = onNext;
                }

                public override void OnNext(TSource value)
                {
                    try
                    {
                        _onNext(value);
                    }
                    catch (Exception ex)
                    {
                        ForwardOnError(ex);
                        return;
                    }

                    ForwardOnNext(value);
                }
            }
        }

        internal sealed class Observer : Producer<TSource, Observer._>
        {
            private readonly IObservable<TSource> _source;
            private readonly IObserver<TSource> _observer;

            public Observer(IObservable<TSource> source, IObserver<TSource> observer)
            {
                _source = source;
                _observer = observer;
            }

            protected override _ CreateSink(IObserver<TSource> observer) => new _(_observer, observer);

            protected override void Run(_ sink) => sink.Run(_source);

            internal sealed class _ : IdentitySink<TSource>
            {
                private readonly IObserver<TSource> _doObserver;

                public _(IObserver<TSource> doObserver, IObserver<TSource> observer)
                    : base(observer)
                {
                    _doObserver = doObserver;
                }

                public override void OnNext(TSource value)
                {
                    try
                    {
                        _doObserver.OnNext(value);
                    }
                    catch (Exception ex)
                    {
                        ForwardOnError(ex);
                        return;
                    }

                    ForwardOnNext(value);
                }

                public override void OnError(Exception error)
                {
                    try
                    {
                        _doObserver.OnError(error);
                    }
                    catch (Exception ex)
                    {
                        ForwardOnError(ex);
                        return;
                    }

                    ForwardOnError(error);
                }

                public override void OnCompleted()
                {
                    try
                    {
                        _doObserver.OnCompleted();
                    }
                    catch (Exception ex)
                    {
                        ForwardOnError(ex);
                        return;
                    }

                    ForwardOnCompleted();
                }
            }
        }

        internal sealed class Actions : Producer<TSource, Actions._>
        {
            private readonly IObservable<TSource> _source;
            private readonly Action<TSource> _onNext;
            private readonly Action<Exception> _onError;
            private readonly Action _onCompleted;

            public Actions(IObservable<TSource> source, Action<TSource> onNext, Action<Exception> onError, Action onCompleted)
            {
                _source = source;
                _onNext = onNext;
                _onError = onError;
                _onCompleted = onCompleted;
            }

            protected override _ CreateSink(IObserver<TSource> observer) => new _(this, observer);

            protected override void Run(_ sink) => sink.Run(_source);

            internal sealed class _ : IdentitySink<TSource>
            {
                // CONSIDER: This sink has a parent reference that can be considered for removal.

                private readonly Actions _parent;

                public _(Actions parent, IObserver<TSource> observer)
                    : base(observer)
                {
                    _parent = parent;
                }

                public override void OnNext(TSource value)
                {
                    try
                    {
                        _parent._onNext(value);
                    }
                    catch (Exception ex)
                    {
                        ForwardOnError(ex);
                        return;
                    }

                    ForwardOnNext(value);
                }

                public override void OnError(Exception error)
                {
                    try
                    {
                        _parent._onError(error);
                    }
                    catch (Exception ex)
                    {
                        ForwardOnError(ex);
                        return;
                    }

                    ForwardOnError(error);
                }

                public override void OnCompleted()
                {
                    try
                    {
                        _parent._onCompleted();
                    }
                    catch (Exception ex)
                    {
                        ForwardOnError(ex);
                        return;
                    }

                    ForwardOnCompleted();
                }
            }
        }
    }
}

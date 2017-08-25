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

            protected override _ CreateSink(IObserver<TSource> observer, IDisposable cancel) => new _(_onNext, observer, cancel);

            protected override IDisposable Run(_ sink) => _source.SubscribeSafe(sink);

            internal sealed class _ : Sink<TSource>, IObserver<TSource>
            {
                private readonly Action<TSource> _onNext;

                public _(Action<TSource> onNext, IObserver<TSource> observer, IDisposable cancel)
                    : base(observer, cancel)
                {
                    _onNext = onNext;
                }

                public void OnNext(TSource value)
                {
                    try
                    {
                        _onNext(value);
                    }
                    catch (Exception ex)
                    {
                        base._observer.OnError(ex);
                        base.Dispose();
                        return;
                    }

                    base._observer.OnNext(value);
                }

                public void OnError(Exception error)
                {
                    base._observer.OnError(error);
                    base.Dispose();
                }

                public void OnCompleted()
                {
                    base._observer.OnCompleted();
                    base.Dispose();
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

            protected override _ CreateSink(IObserver<TSource> observer, IDisposable cancel) => new _(_observer, observer, cancel);

            protected override IDisposable Run(_ sink) => _source.SubscribeSafe(sink);

            internal sealed class _ : Sink<TSource>, IObserver<TSource>
            {
                private readonly IObserver<TSource> _doObserver;

                public _(IObserver<TSource> doObserver, IObserver<TSource> observer, IDisposable cancel)
                    : base(observer, cancel)
                {
                    _doObserver = doObserver;
                }

                public void OnNext(TSource value)
                {
                    try
                    {
                        _doObserver.OnNext(value);
                    }
                    catch (Exception ex)
                    {
                        base._observer.OnError(ex);
                        base.Dispose();
                        return;
                    }

                    base._observer.OnNext(value);
                }

                public void OnError(Exception error)
                {
                    try
                    {
                        _doObserver.OnError(error);
                    }
                    catch (Exception ex)
                    {
                        base._observer.OnError(ex);
                        base.Dispose();
                        return;
                    }

                    base._observer.OnError(error);
                    base.Dispose();
                }

                public void OnCompleted()
                {
                    try
                    {
                        _doObserver.OnCompleted();
                    }
                    catch (Exception ex)
                    {
                        base._observer.OnError(ex);
                        base.Dispose();
                        return;
                    }

                    base._observer.OnCompleted();
                    base.Dispose();
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

            protected override _ CreateSink(IObserver<TSource> observer, IDisposable cancel) => new _(this, observer, cancel);

            protected override IDisposable Run(_ sink) => _source.SubscribeSafe(sink);

            internal sealed class _ : Sink<TSource>, IObserver<TSource>
            {
                // CONSIDER: This sink has a parent reference that can be considered for removal.

                private readonly Actions _parent;

                public _(Actions parent, IObserver<TSource> observer, IDisposable cancel)
                    : base(observer, cancel)
                {
                    _parent = parent;
                }

                public void OnNext(TSource value)
                {
                    try
                    {
                        _parent._onNext(value);
                    }
                    catch (Exception ex)
                    {
                        base._observer.OnError(ex);
                        base.Dispose();
                        return;
                    }

                    base._observer.OnNext(value);
                }

                public void OnError(Exception error)
                {
                    try
                    {
                        _parent._onError(error);
                    }
                    catch (Exception ex)
                    {
                        base._observer.OnError(ex);
                        base.Dispose();
                        return;
                    }

                    base._observer.OnError(error);
                    base.Dispose();
                }

                public void OnCompleted()
                {
                    try
                    {
                        _parent._onCompleted();
                    }
                    catch (Exception ex)
                    {
                        base._observer.OnError(ex);
                        base.Dispose();
                        return;
                    }

                    base._observer.OnCompleted();
                    base.Dispose();
                }
            }
        }
    }
}

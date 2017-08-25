// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Reactive.Disposables;

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class Catch<TSource> : Producer<TSource, Catch<TSource>._>
    {
        private readonly IEnumerable<IObservable<TSource>> _sources;

        public Catch(IEnumerable<IObservable<TSource>> sources)
        {
            _sources = sources;
        }

        protected override _ CreateSink(IObserver<TSource> observer, IDisposable cancel) => new _(observer, cancel);

        protected override IDisposable Run(_ sink) => sink.Run(_sources);

        internal sealed class _ : TailRecursiveSink<TSource>
        {
            public _(IObserver<TSource> observer, IDisposable cancel)
                : base(observer, cancel)
            {
            }

            protected override IEnumerable<IObservable<TSource>> Extract(IObservable<TSource> source)
            {
                if (source is Catch<TSource> @catch)
                    return @catch._sources;

                return null;
            }

            public override void OnNext(TSource value)
            {
                base._observer.OnNext(value);
            }

            private Exception _lastException;

            public override void OnError(Exception error)
            {
                _lastException = error;
                _recurse();
            }

            public override void OnCompleted()
            {
                base._observer.OnCompleted();
                base.Dispose();
            }

            protected override void Done()
            {
                if (_lastException != null)
                    base._observer.OnError(_lastException);
                else
                    base._observer.OnCompleted();

                base.Dispose();
            }

            protected override bool Fail(Exception error)
            {
                //
                // Note that the invocation of _recurse in OnError will
                // cause the next MoveNext operation to be enqueued, so
                // we will still return to the caller immediately.
                //
                OnError(error);
                return true;
            }
        }
    }

    internal sealed class Catch<TSource, TException> : Producer<TSource, Catch<TSource, TException>._> where TException : Exception
    {
        private readonly IObservable<TSource> _source;
        private readonly Func<TException, IObservable<TSource>> _handler;

        public Catch(IObservable<TSource> source, Func<TException, IObservable<TSource>> handler)
        {
            _source = source;
            _handler = handler;
        }

        protected override _ CreateSink(IObserver<TSource> observer, IDisposable cancel) => new _(_handler, observer, cancel);

        protected override IDisposable Run(_ sink) => sink.Run(_source);

        internal sealed class _ : Sink<TSource>, IObserver<TSource>
        {
            private readonly Func<TException, IObservable<TSource>> _handler;

            public _(Func<TException, IObservable<TSource>> handler, IObserver<TSource> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _handler = handler;
            }

            private SerialDisposable _subscription;

            public IDisposable Run(IObservable<TSource> source)
            {
                _subscription = new SerialDisposable();

                var d1 = new SingleAssignmentDisposable();
                _subscription.Disposable = d1;
                d1.Disposable = source.SubscribeSafe(this);

                return _subscription;
            }

            public void OnNext(TSource value)
            {
                base._observer.OnNext(value);
            }

            public void OnError(Exception error)
            {
                if (error is TException e)
                {
                    var result = default(IObservable<TSource>);
                    try
                    {
                        result = _handler(e);
                    }
                    catch (Exception ex)
                    {
                        base._observer.OnError(ex);
                        base.Dispose();
                        return;
                    }

                    var d = new SingleAssignmentDisposable();
                    _subscription.Disposable = d;
                    d.Disposable = result.SubscribeSafe(new HandlerObserver(this));
                }
                else
                {
                    base._observer.OnError(error);
                    base.Dispose();
                }
            }

            public void OnCompleted()
            {
                base._observer.OnCompleted();
                base.Dispose();
            }

            private sealed class HandlerObserver : IObserver<TSource>
            {
                private readonly _ _parent;

                public HandlerObserver(_ parent)
                {
                    _parent = parent;
                }

                public void OnNext(TSource value)
                {
                    _parent._observer.OnNext(value);
                }

                public void OnError(Exception error)
                {
                    _parent._observer.OnError(error);
                    _parent.Dispose();
                }

                public void OnCompleted()
                {
                    _parent._observer.OnCompleted();
                    _parent.Dispose();
                }
            }
        }
    }
}

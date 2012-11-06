// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_PERF
using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;

namespace System.Reactive.Linq.Observαble
{
    class Catch<TSource> : Producer<TSource>
    {
        private readonly IEnumerable<IObservable<TSource>> _sources;

        public Catch(IEnumerable<IObservable<TSource>> sources)
        {
            _sources = sources;
        }

        protected override IDisposable Run(IObserver<TSource> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            var sink = new _(observer, cancel);
            setSink(sink);
            return sink.Run(_sources);
        }

        class _ : TailRecursiveSink<TSource>
        {
            public _(IObserver<TSource> observer, IDisposable cancel)
                : base(observer, cancel)
            {
            }

            protected override IEnumerable<IObservable<TSource>> Extract(IObservable<TSource> source)
            {
                var @catch = source as Catch<TSource>;
                if (@catch != null)
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
        }
    }

    class Catch<TSource, TException> : Producer<TSource> where TException : Exception
    {
        private readonly IObservable<TSource> _source;
        private readonly Func<TException, IObservable<TSource>> _handler;

        public Catch(IObservable<TSource> source, Func<TException, IObservable<TSource>> handler)
        {
            _source = source;
            _handler = handler;
        }

        protected override IDisposable Run(IObserver<TSource> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            var sink = new _(this, observer, cancel);
            setSink(sink);
            return sink.Run();
        }

        class _ : Sink<TSource>, IObserver<TSource>
        {
            private readonly Catch<TSource, TException> _parent;

            public _(Catch<TSource, TException> parent, IObserver<TSource> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
            }

            private SerialDisposable _subscription;

            public IDisposable Run()
            {
                _subscription = new SerialDisposable();

                var d1 = new SingleAssignmentDisposable();
                _subscription.Disposable = d1;
                d1.Disposable = _parent._source.SubscribeSafe(this);

                return _subscription;
            }

            public void OnNext(TSource value)
            {
                base._observer.OnNext(value);
            }

            public void OnError(Exception error)
            {
                var e = error as TException;
                if (e != null)
                {
                    var result = default(IObservable<TSource>);
                    try
                    {
                        result = _parent._handler(e);
                    }
                    catch (Exception ex)
                    {
                        base._observer.OnError(ex);
                        base.Dispose();
                        return;
                    }

                    var d = new SingleAssignmentDisposable();
                    _subscription.Disposable = d;
                    d.Disposable = result.SubscribeSafe(new ε(this));
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

            class ε : IObserver<TSource>
            {
                private readonly _ _parent;

                public ε(_ parent)
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
#endif
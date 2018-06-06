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

        protected override _ CreateSink(IObserver<TSource> observer) => new _(observer);

        protected override void Run(_ sink) => sink.Run(_sources);

        internal sealed class _ : TailRecursiveSink<TSource>
        {
            public _(IObserver<TSource> observer)
                : base(observer)
            {
            }

            protected override IEnumerable<IObservable<TSource>> Extract(IObservable<TSource> source)
            {
                if (source is Catch<TSource> @catch)
                    return @catch._sources;

                return null;
            }

            private Exception _lastException;

            public override void OnError(Exception error)
            {
                _lastException = error;
                Recurse();
            }

            protected override void Done()
            {
                if (_lastException != null)
                    ForwardOnError(_lastException);
                else
                    ForwardOnCompleted();
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

        protected override _ CreateSink(IObserver<TSource> observer) => new _(_handler, observer);

        protected override void Run(_ sink) => sink.Run(_source);

        internal sealed class _ : IdentitySink<TSource>
        {
            private readonly Func<TException, IObservable<TSource>> _handler;

            public _(Func<TException, IObservable<TSource>> handler, IObserver<TSource> observer)
                : base(observer)
            {
                _handler = handler;
            }

            private SerialDisposable _subscription;

            public void Run(IObservable<TSource> source)
            {
                _subscription = new SerialDisposable();

                var d1 = new SingleAssignmentDisposable();
                _subscription.Disposable = d1;
                d1.Disposable = source.SubscribeSafe(this);

                SetUpstream(_subscription);
            }

            public override void OnError(Exception error)
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
                        ForwardOnError(ex);
                        return;
                    }

                    var d = new SingleAssignmentDisposable();
                    _subscription.Disposable = d;
                    d.Disposable = result.SubscribeSafe(new HandlerObserver(this));
                }
                else
                {
                    ForwardOnError(error);
                }
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
                    _parent.ForwardOnNext(value);
                }

                public void OnError(Exception error)
                {
                    _parent.ForwardOnError(error);
                }

                public void OnCompleted()
                {
                    _parent.ForwardOnCompleted();
                }
            }
        }
    }
}

// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_PERF
using System;
using System.Reactive.Disposables;
using System.Reactive.Subjects;

namespace System.Reactive.Linq.Observαble
{
    class Multicast<TSource, TIntermediate, TResult> : Producer<TResult>
    {
        private readonly IObservable<TSource> _source;
        private readonly Func<ISubject<TSource, TIntermediate>> _subjectSelector;
        private readonly Func<IObservable<TIntermediate>, IObservable<TResult>> _selector;

        public Multicast(IObservable<TSource> source, Func<ISubject<TSource, TIntermediate>> subjectSelector, Func<IObservable<TIntermediate>, IObservable<TResult>> selector)
        {
            _source = source;
            _subjectSelector = subjectSelector;
            _selector = selector;
        }

        protected override IDisposable Run(IObserver<TResult> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            var sink = new _(this, observer, cancel);
            setSink(sink);
            return sink.Run();
        }

        class _ : Sink<TResult>, IObserver<TResult>
        {
            private readonly Multicast<TSource, TIntermediate, TResult> _parent;

            public _(Multicast<TSource, TIntermediate, TResult> parent, IObserver<TResult> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
            }

            public IDisposable Run()
            {
                var observable = default(IObservable<TResult>);
                var connectable = default(IConnectableObservable<TIntermediate>);
                try
                {
                    var subject = _parent._subjectSelector();
                    connectable = new ConnectableObservable<TSource, TIntermediate>(_parent._source, subject);
                    observable = _parent._selector(connectable);
                }
                catch (Exception exception)
                {
                    base._observer.OnError(exception);
                    base.Dispose();
                    return Disposable.Empty;
                }

                var subscription = observable.SubscribeSafe(this);
                var connection = connectable.Connect();

                return new CompositeDisposable(subscription, connection);
            }

            public void OnNext(TResult value)
            {
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
}
#endif

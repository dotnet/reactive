// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Disposables;
using System.Reactive.Subjects;

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class Multicast<TSource, TIntermediate, TResult> : Producer<TResult, Multicast<TSource, TIntermediate, TResult>._>
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

        protected override _ CreateSink(IObserver<TResult> observer) => new(observer);

        protected override void Run(_ sink) => sink.Run(this);

        internal sealed class _ : IdentitySink<TResult>
        {
            private SingleAssignmentDisposableValue _connection;

            public _(IObserver<TResult> observer)
                : base(observer)
            {
            }

            public void Run(Multicast<TSource, TIntermediate, TResult> parent)
            {
                IObservable<TResult> observable;
                IConnectableObservable<TIntermediate> connectable;

                try
                {
                    var subject = parent._subjectSelector();
                    connectable = new ConnectableObservable<TSource, TIntermediate>(parent._source, subject);
                    observable = parent._selector(connectable);
                }
                catch (Exception exception)
                {
                    ForwardOnError(exception);
                    return;
                }

                Run(observable);
                _connection.Disposable = connectable.Connect();
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    _connection.Dispose();
                }
                
                base.Dispose(disposing);
            }
        }
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
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

        protected override _ CreateSink(IObserver<TResult> observer, IDisposable cancel) => new _(observer, cancel);

        protected override IDisposable Run(_ sink) => sink.Run(this);

        internal sealed class _ : IdentitySink<TResult>
        {
            public _(IObserver<TResult> observer, IDisposable cancel)
                : base(observer, cancel)
            {
            }

            public IDisposable Run(Multicast<TSource, TIntermediate, TResult> parent)
            {
                var observable = default(IObservable<TResult>);
                var connectable = default(IConnectableObservable<TIntermediate>);
                try
                {
                    var subject =parent._subjectSelector();
                    connectable = new ConnectableObservable<TSource, TIntermediate>(parent._source, subject);
                    observable = parent._selector(connectable);
                }
                catch (Exception exception)
                {
                    ForwardOnError(exception);
                    return Disposable.Empty;
                }

                var subscription = observable.SubscribeSafe(this);
                var connection = connectable.Connect();

                return StableCompositeDisposable.Create(subscription, connection);
            }
        }
    }
}

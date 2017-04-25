// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class Materialize<TSource> : Producer<Notification<TSource>, Materialize<TSource>._>
    {
        private readonly IObservable<TSource> _source;

        public Materialize(IObservable<TSource> source)
        {
            _source = source;
        }

        public IObservable<TSource> Dematerialize() => _source.AsObservable();

        protected override _ CreateSink(IObserver<Notification<TSource>> observer, IDisposable cancel) => new _(observer, cancel);

        protected override IDisposable Run(_ sink) => _source.SubscribeSafe(sink);

        internal sealed class _ : Sink<Notification<TSource>>, IObserver<TSource>
        {
            public _(IObserver<Notification<TSource>> observer, IDisposable cancel)
                : base(observer, cancel)
            {
            }

            public void OnNext(TSource value)
            {
                base._observer.OnNext(Notification.CreateOnNext<TSource>(value));
            }

            public void OnError(Exception error)
            {
                base._observer.OnNext(Notification.CreateOnError<TSource>(error));
                base._observer.OnCompleted();
                base.Dispose();
            }

            public void OnCompleted()
            {
                base._observer.OnNext(Notification.CreateOnCompleted<TSource>());
                base._observer.OnCompleted();
                base.Dispose();
            }
        }
    }
}

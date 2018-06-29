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

        protected override _ CreateSink(IObserver<Notification<TSource>> observer) => new _(observer);

        protected override void Run(_ sink) => sink.Run(_source);

        internal sealed class _ : Sink<TSource, Notification<TSource>>
        {
            public _(IObserver<Notification<TSource>> observer)
                : base(observer)
            {
            }

            public override void OnNext(TSource value)
            {
                ForwardOnNext(Notification.CreateOnNext(value));
            }

            public override void OnError(Exception error)
            {
                ForwardOnNext(Notification.CreateOnError<TSource>(error));
                ForwardOnCompleted();
            }

            public override void OnCompleted()
            {
                ForwardOnNext(Notification.CreateOnCompleted<TSource>());
                ForwardOnCompleted();
            }
        }
    }
}

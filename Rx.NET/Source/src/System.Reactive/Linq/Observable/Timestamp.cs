// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class Timestamp<TSource> : Producer<Timestamped<TSource>, Timestamp<TSource>._>
    {
        private readonly IObservable<TSource> _source;
        private readonly IScheduler _scheduler;

        public Timestamp(IObservable<TSource> source, IScheduler scheduler)
        {
            _source = source;
            _scheduler = scheduler;
        }

        protected override _ CreateSink(IObserver<Timestamped<TSource>> observer) => new _(_scheduler, observer);

        protected override void Run(_ sink) => sink.Run(_source);

        internal sealed class _ : Sink<TSource, Timestamped<TSource>>
        {
            private readonly IScheduler _scheduler;

            public _(IScheduler scheduler, IObserver<Timestamped<TSource>> observer)
                : base(observer)
            {
                _scheduler = scheduler;
            }

            public override void OnNext(TSource value)
            {
                ForwardOnNext(new Timestamped<TSource>(value, _scheduler.Now));
            }
        }
    }
}

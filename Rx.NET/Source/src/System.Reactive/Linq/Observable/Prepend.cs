// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Disposables;

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class Prepend<TSource> : Producer<TSource, Prepend<TSource>._>
    {
        private readonly IObservable<TSource> _source;
        private readonly TSource _value;
        private readonly IScheduler _scheduler;

        public Prepend(IObservable<TSource> source, TSource value, IScheduler scheduler)
        {
            _source = source;
            _value = value;
            _scheduler = scheduler;
        }

        protected override _ CreateSink(IObserver<TSource> observer) => new _(this, observer);

        protected override void Run(_ sink) => sink.Run(); 

        internal sealed class _ : IdentitySink<TSource>
        {
            private readonly IObservable<TSource> _source;
            private readonly TSource _value;
            private readonly IScheduler _scheduler;

            public _(Prepend<TSource> parent, IObserver<TSource> observer)
                : base(observer)
            {
                _source = parent._source;
                _value = parent._value;
                _scheduler = parent._scheduler;
            }

            public void Run() => SetUpstream(_scheduler.Schedule(this, ForwardValue));

            private static IDisposable ForwardValue(IScheduler scheduler, _ sink)
            {
                sink.ForwardOnNext(sink._value);
                return scheduler.Schedule(sink, ForwardSource);
            }

            private static IDisposable ForwardSource(IScheduler _unused, _ sink) => sink._source.SubscribeSafe(sink);
        }
    }
}

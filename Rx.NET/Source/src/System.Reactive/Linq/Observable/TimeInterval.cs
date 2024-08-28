// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class TimeInterval<TSource> : Producer<Reactive.TimeInterval<TSource>, TimeInterval<TSource>._>
    {
        private readonly IObservable<TSource> _source;
        private readonly IScheduler _scheduler;

        public TimeInterval(IObservable<TSource> source, IScheduler scheduler)
        {
            _source = source;
            _scheduler = scheduler;
        }

        protected override _ CreateSink(IObserver<Reactive.TimeInterval<TSource>> observer) => new(observer);

        protected override void Run(_ sink) => sink.Run(this);

        internal sealed class _ : Sink<TSource, Reactive.TimeInterval<TSource>>
        {
            public _(IObserver<Reactive.TimeInterval<TSource>> observer)
                : base(observer)
            {
            }

            private IStopwatch? _watch;
            private TimeSpan _last;

            public void Run(TimeInterval<TSource> parent)
            {
                _watch = parent._scheduler.StartStopwatch();
                _last = TimeSpan.Zero;

                SetUpstream(parent._source.Subscribe(this));
            }

            public override void OnNext(TSource value)
            {
                var now = _watch!.Elapsed; // NB: Watch is assigned during Run.
                var span = now.Subtract(_last);
                _last = now;
                ForwardOnNext(new Reactive.TimeInterval<TSource>(value, span));
            }
        }
    }
}

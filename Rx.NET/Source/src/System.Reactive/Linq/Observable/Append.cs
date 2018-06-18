// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Disposables;

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class Append<TSource> : Producer<TSource, Append<TSource>._>
    {
        private readonly IObservable<TSource> _source;
        private readonly TSource _value;
        private readonly IScheduler _scheduler;

        public Append(IObservable<TSource> source, TSource value, IScheduler scheduler)
        {
            _source = source;
            _value = value;
            _scheduler = scheduler;
        }

        protected override _ CreateSink(IObserver<TSource> observer) => new _(this, observer);

        protected override void Run(_ sink) => sink.Run(_source);

        internal sealed class _ : IdentitySink<TSource>
        {
            private readonly TSource _value;
            private readonly IScheduler _scheduler;
            private IDisposable _schedulerDisposable;

            public _(Append<TSource> parent, IObserver<TSource> observer)
                : base(observer)
            {
                _value = parent._value;
                _scheduler = parent._scheduler;
            }

            public override void OnCompleted()
            {
                var disposable = _scheduler.Schedule(this, ForwardValue);
                Disposable.TrySetSingle(ref _schedulerDisposable, disposable);
            }

            private static IDisposable ForwardValue(IScheduler scheduler, _ sink)
            {
                sink.ForwardOnNext(sink._value);
                sink.ForwardOnCompleted();
                return Disposable.Empty;
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    Disposable.TryDispose(ref _schedulerDisposable);
                }
                base.Dispose(disposing);
            }
        }
    }
}

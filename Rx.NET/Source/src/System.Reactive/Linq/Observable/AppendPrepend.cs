// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Disposables;

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class AppendPrependSingle<TSource> : Producer<TSource, AppendPrependSingle<TSource>._>
    {
        private readonly IObservable<TSource> _source;
        private readonly TSource _value;
        private readonly IScheduler _scheduler;
        private readonly bool _append;

        public AppendPrependSingle(IObservable<TSource> source, TSource value, IScheduler scheduler, bool append)
        {
            _source = source;
            _value = value;
            _scheduler = scheduler;
            _append = append;
        }

        protected override _ CreateSink(IObserver<TSource> observer) => new _(this, observer);

        protected override void Run(_ sink) => sink.Run(); 

        internal sealed class _ : IdentitySink<TSource>
        {
            private readonly IObservable<TSource> _source;
            private readonly TSource _value;
            private readonly IScheduler _scheduler;
            private readonly bool _append;
            private IDisposable _schedulerDisposable;

            public _(AppendPrependSingle<TSource> parent, IObserver<TSource> observer)
                : base(observer)
            {
                _source = parent._source;
                _value = parent._value;
                _scheduler = parent._scheduler;
                _append = parent._append;
            }

            public void Run()
            {
                var disp = _append 
                    ? _source.SubscribeSafe(this)
                    : _scheduler.Schedule(this, PrependValue);

                SetUpstream(disp);
            }

            private static IDisposable PrependValue(IScheduler scheduler, _ sink)
            {
                sink.ForwardOnNext(sink._value);
                return sink._source.SubscribeSafe(sink);
            }

            public override void OnCompleted()
            {
                if (_append)
                {
                    var disposable = _scheduler.Schedule(this, AppendValue);
                    Disposable.TrySetSingle(ref _schedulerDisposable, disposable);
                }
                else
                {
                    ForwardOnCompleted();
                }
            }

            private static IDisposable AppendValue(IScheduler scheduler, _ sink)
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

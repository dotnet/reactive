// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Disposables;

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class Throw<TResult> : Producer<TResult, Throw<TResult>._>
    {
        private readonly Exception _exception;
        private readonly IScheduler _scheduler;

        public Throw(Exception exception, IScheduler scheduler)
        {
            _exception = exception;
            _scheduler = scheduler;
        }

        protected override _ CreateSink(IObserver<TResult> observer) => new _(_exception, observer);

        protected override void Run(_ sink) => sink.Run(_scheduler);

        internal sealed class _ : IdentitySink<TResult>
        {
            private readonly Exception _exception;

            public _(Exception exception, IObserver<TResult> observer)
                : base(observer)
            {
                _exception = exception;
            }

            public void Run(IScheduler scheduler)
            {
                SetUpstream(scheduler.ScheduleAction(this, @this => @this.ForwardOnError(@this._exception)));
            }
        }
    }

    // There is no need for a full Producer/IdentitySink as there is no
    // way to stop a first task running on the immediate scheduler
    // as it is always synchronous.
    internal sealed class ThrowImmediate<TSource> : BasicProducer<TSource>
    {
        private readonly Exception _exception;

        public ThrowImmediate(Exception exception)
        {
            _exception = exception;
        }

        protected override IDisposable Run(IObserver<TSource> observer)
        {
            observer.OnError(_exception);
            return Disposable.Empty;
        }
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Disposables;

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class Return<TResult> : Producer<TResult, Return<TResult>._>
    {
        private readonly TResult _value;
        private readonly IScheduler _scheduler;

        public Return(TResult value, IScheduler scheduler)
        {
            _value = value;
            _scheduler = scheduler;
        }

        protected override _ CreateSink(IObserver<TResult> observer) => new _(_value, observer);

        protected override void Run(_ sink) => sink.Run(_scheduler);

        internal sealed class _ : IdentitySink<TResult>
        {
            private readonly TResult _value;

            public _(TResult value, IObserver<TResult> observer)
                : base(observer)
            {
                _value = value;
            }

            public void Run(IScheduler scheduler)
            {
                SetUpstream(scheduler.ScheduleAction(this, @this => @this.Invoke()));
            }

            private void Invoke()
            {
                ForwardOnNext(_value);
                ForwardOnCompleted();
            }
        }
    }

    // There is no need for a full Producer/IdentitySink as there is no
    // way to stop a first task running on the immediate scheduler
    // as it is always synchronous.
    internal sealed class ReturnImmediate<TSource> : BasicProducer<TSource>
    {
        private readonly TSource _value;

        public ReturnImmediate(TSource value)
        {
            _value = value;
        }

        protected override IDisposable Run(IObserver<TSource> observer)
        {
            observer.OnNext(_value);
            observer.OnCompleted();
            return Disposable.Empty;
        }
    }
}

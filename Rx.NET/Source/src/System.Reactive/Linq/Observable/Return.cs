// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;

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

        protected override _ CreateSink(IObserver<TResult> observer, IDisposable cancel) => new _(_value, observer, cancel);

        protected override IDisposable Run(_ sink) => sink.Run(_scheduler);

        internal sealed class _ : Sink<TResult>
        {
            private readonly TResult _value;

            public _(TResult value, IObserver<TResult> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _value = value;
            }

            public IDisposable Run(IScheduler scheduler)
            {
                return scheduler.Schedule(Invoke);
            }

            private void Invoke()
            {
                base._observer.OnNext(_value);
                base._observer.OnCompleted();
                base.Dispose();
            }
        }
    }
}

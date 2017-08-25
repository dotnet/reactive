// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class Empty<TResult> : Producer<TResult, Empty<TResult>._>
    {
        private readonly IScheduler _scheduler;

        public Empty(IScheduler scheduler)
        {
            _scheduler = scheduler;
        }

        protected override _ CreateSink(IObserver<TResult> observer, IDisposable cancel) => new _(observer, cancel);

        protected override IDisposable Run(_ sink) => sink.Run(_scheduler);

        internal sealed class _ : Sink<TResult>
        {
            public _(IObserver<TResult> observer, IDisposable cancel)
                : base(observer, cancel)
            {
            }

            public IDisposable Run(IScheduler scheduler)
            {
                return scheduler.Schedule(Invoke);
            }

            private void Invoke()
            {
                base._observer.OnCompleted();
                base.Dispose();
            }
        }
    }
}

// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_PERF
using System;
using System.Reactive.Concurrency;

namespace System.Reactive.Linq.Observαble
{
    class Throw<TResult> : Producer<TResult>
    {
        private readonly Exception _exception;
        private readonly IScheduler _scheduler;

        public Throw(Exception exception, IScheduler scheduler)
        {
            _exception = exception;
            _scheduler = scheduler;
        }

        protected override IDisposable Run(IObserver<TResult> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            var sink = new _(this, observer, cancel);
            setSink(sink);
            return sink.Run();
        }

        class _ : Sink<TResult>
        {
            private readonly Throw<TResult> _parent;

            public _(Throw<TResult> parent, IObserver<TResult> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
            }

            public IDisposable Run()
            {
                return _parent._scheduler.Schedule(Invoke);
            }

            private void Invoke()
            {
                base._observer.OnError(_parent._exception);
                base.Dispose();
            }
        }
    }
}
#endif
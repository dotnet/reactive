// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_PERF
using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;

namespace System.Reactive.Linq.Observαble
{
    class Repeat<TResult> : Producer<TResult>
    {
        private readonly TResult _value;
        private readonly int? _repeatCount;
        private readonly IScheduler _scheduler;

        public Repeat(TResult value, int? repeatCount, IScheduler scheduler)
        {
            _value = value;
            _repeatCount = repeatCount;
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
            private readonly Repeat<TResult> _parent;

            public _(Repeat<TResult> parent, IObserver<TResult> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
            }

            public IDisposable Run()
            {
                var longRunning = _parent._scheduler.AsLongRunning();
                if (longRunning != null)
                {
                    return Run(longRunning);
                }
                else
                {
                    return Run(_parent._scheduler);
                }
            }

            private IDisposable Run(IScheduler scheduler)
            {
                if (_parent._repeatCount == null)
                {
                    return scheduler.Schedule(LoopRecInf);
                }
                else
                {
                    return scheduler.Schedule(_parent._repeatCount.Value, LoopRec);
                }
            }

            private void LoopRecInf(Action recurse)
            {
                base._observer.OnNext(_parent._value);
                recurse();
            }

            private void LoopRec(int n, Action<int> recurse)
            {
                if (n > 0)
                {
                    base._observer.OnNext(_parent._value);
                    n--;
                }

                if (n == 0)
                {
                    base._observer.OnCompleted();
                    base.Dispose();
                    return;
                }

                recurse(n);
            }

            private IDisposable Run(ISchedulerLongRunning scheduler)
            {
                if (_parent._repeatCount == null)
                {
                    return scheduler.ScheduleLongRunning(LoopInf);
                }
                else
                {
                    return scheduler.ScheduleLongRunning(_parent._repeatCount.Value, Loop);
                }
            }

            private void LoopInf(ICancelable cancel)
            {
                var value = _parent._value;
                while (!cancel.IsDisposed)
                    base._observer.OnNext(value);

                base.Dispose();
            }

            private void Loop(int n, ICancelable cancel)
            {
                var value = _parent._value;
                while (n > 0 && !cancel.IsDisposed)
                {
                    base._observer.OnNext(value);
                    n--;
                }

                if (!cancel.IsDisposed)
                    base._observer.OnCompleted();

                base.Dispose();
            }
        }
    }
}
#endif
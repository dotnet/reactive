// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_PERF
using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;

namespace System.Reactive.Linq.Observαble
{
    class Range : Producer<int>
    {
        private readonly int _start;
        private readonly int _count;
        private readonly IScheduler _scheduler;

        public Range(int start, int count, IScheduler scheduler)
        {
            _start = start;
            _count = count;
            _scheduler = scheduler;
        }

        protected override IDisposable Run(IObserver<int> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            var sink = new _(this, observer, cancel);
            setSink(sink);
            return sink.Run();
        }

        class _ : Sink<int>
        {
            private readonly Range _parent;

            public _(Range parent, IObserver<int> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
            }

            public IDisposable Run()
            {
                var longRunning = _parent._scheduler.AsLongRunning();
                if (longRunning != null)
                {
                    return longRunning.ScheduleLongRunning(0, Loop);
                }
                else
                {
                    return _parent._scheduler.Schedule(0, LoopRec);
                }
            }

            private void Loop(int i, ICancelable cancel)
            {
                while (!cancel.IsDisposed && i < _parent._count)
                {
                    base._observer.OnNext(_parent._start + i);
                    i++;
                }

                if (!cancel.IsDisposed)
                    base._observer.OnCompleted();

                base.Dispose();
            }

            private void LoopRec(int i, Action<int> recurse)
            {
                if (i < _parent._count)
                {
                    base._observer.OnNext(_parent._start + i);
                    recurse(i + 1);
                }
                else
                {
                    base._observer.OnCompleted();
                    base.Dispose();
                }
            }
        }
    }
}
#endif
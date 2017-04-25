// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Disposables;

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class Range : Producer<int, Range._>
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

        protected override _ CreateSink(IObserver<int> observer, IDisposable cancel) => new _(this, observer, cancel);

        protected override IDisposable Run(_ sink) => sink.Run(_scheduler);

        internal sealed class _ : Sink<int>
        {
            private readonly int _start;
            private readonly int _count;

            public _(Range parent, IObserver<int> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _start = parent._start;
                _count = parent._count;
            }

            public IDisposable Run(IScheduler scheduler)
            {
                var longRunning = scheduler.AsLongRunning();
                if (longRunning != null)
                {
                    return longRunning.ScheduleLongRunning(0, Loop);
                }
                else
                {
                    return scheduler.Schedule(0, LoopRec);
                }
            }

            private void Loop(int i, ICancelable cancel)
            {
                while (!cancel.IsDisposed && i < _count)
                {
                    base._observer.OnNext(_start + i);
                    i++;
                }

                if (!cancel.IsDisposed)
                    base._observer.OnCompleted();

                base.Dispose();
            }

            private void LoopRec(int i, Action<int> recurse)
            {
                if (i < _count)
                {
                    base._observer.OnNext(_start + i);
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

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

        protected override _ CreateSink(IObserver<int> observer) => new _(this, observer);

        protected override void Run(_ sink) => sink.Run(_scheduler);

        internal sealed class _ : IdentitySink<int>
        {
            private readonly int _start;
            private readonly int _count;

            public _(Range parent, IObserver<int> observer)
                : base(observer)
            {
                _start = parent._start;
                _count = parent._count;
            }

            public void Run(IScheduler scheduler)
            {
                var longRunning = scheduler.AsLongRunning();
                if (longRunning != null)
                {
                    SetUpstream(longRunning.ScheduleLongRunning(0, Loop));
                }
                else
                {
                    SetUpstream(scheduler.Schedule(0, LoopRec));
                }
            }

            private void Loop(int i, ICancelable cancel)
            {
                while (!cancel.IsDisposed && i < _count)
                {
                    ForwardOnNext(_start + i);
                    i++;
                }

                if (!cancel.IsDisposed)
                    ForwardOnCompleted();
            }

            private void LoopRec(int i, Action<int> recurse)
            {
                if (i < _count)
                {
                    ForwardOnNext(_start + i);
                    recurse(i + 1);
                }
                else
                {
                    ForwardOnCompleted();
                }
            }
        }
    }
}

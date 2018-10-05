// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Disposables;

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class RangeRecursive : Producer<int, RangeRecursive.RangeSink>
    {
        private readonly int _start;
        private readonly int _count;
        private readonly IScheduler _scheduler;

        public RangeRecursive(int start, int count, IScheduler scheduler)
        {
            _start = start;
            _count = count;
            _scheduler = scheduler;
        }

        protected override RangeSink CreateSink(IObserver<int> observer) => new RangeSink(_start, _count, observer);

        protected override void Run(RangeSink sink) => sink.Run(_scheduler);

        internal sealed class RangeSink : IdentitySink<int>
        {
            private readonly int _end;
            private int _index;
            private IDisposable _task;

            public RangeSink(int start, int count, IObserver<int> observer)
                : base(observer)
            {
                _index = start;
                _end = start + count;
            }

            public void Run(IScheduler scheduler)
            {
                var first = scheduler.Schedule(this, (innerScheduler, @this) => @this.LoopRec(innerScheduler));
                Disposable.TrySetSingle(ref _task, first);
            }

            protected override void Dispose(bool disposing)
            {
                base.Dispose(disposing);
                if (disposing)
                {
                    Disposable.TryDispose(ref _task);
                }
            }

            private IDisposable LoopRec(IScheduler scheduler)
            {
                var idx = _index;
                if (idx != _end)
                {
                    _index = idx + 1;
                    ForwardOnNext(idx);
                    var next = scheduler.Schedule(this, (innerScheduler, @this) => @this.LoopRec(innerScheduler));
                    Disposable.TrySetMultiple(ref _task, next);
                }
                else
                {
                    ForwardOnCompleted();
                }
                return Disposable.Empty;
            }
        }
    }

    internal sealed class RangeLongRunning : Producer<int, RangeLongRunning.RangeSink>
    {
        private readonly int _start;
        private readonly int _count;
        private readonly ISchedulerLongRunning _scheduler;

        public RangeLongRunning(int start, int count, ISchedulerLongRunning scheduler)
        {
            _start = start;
            _count = count;
            _scheduler = scheduler;
        }

        protected override RangeSink CreateSink(IObserver<int> observer) => new RangeSink(_start, _count, observer);

        protected override void Run(RangeSink sink) => sink.Run(_scheduler);

        internal sealed class RangeSink : IdentitySink<int>
        {
            private readonly int _end;
            private readonly int _index;

            public RangeSink(int start, int count, IObserver<int> observer)
                : base(observer)
            {
                _index = start;
                _end = start + count;
            }

            public void Run(ISchedulerLongRunning scheduler)
            {
                SetUpstream(scheduler.ScheduleLongRunning(this, (@this, cancel) => @this.Loop(cancel)));
            }

            private void Loop(ICancelable cancel)
            {
                var idx = _index;
                var end = _end;
                while (!cancel.IsDisposed && idx != end)
                {
                    ForwardOnNext(idx++);
                }

                if (!cancel.IsDisposed)
                {
                    ForwardOnCompleted();
                }
            }
        }
    }
}

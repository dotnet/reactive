// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Disposables;

namespace System.Reactive.Linq.ObservableImpl
{
    internal static class Repeat<TResult>
    {
        internal sealed class ForeverRecursive : Producer<TResult, ForeverRecursive._>
        {
            private readonly TResult _value;
            private readonly IScheduler _scheduler;

            public ForeverRecursive(TResult value, IScheduler scheduler)
            {
                _value = value;
                _scheduler = scheduler;
            }

            protected override _ CreateSink(IObserver<TResult> observer) => new _(_value, observer);

            protected override void Run(_ sink) => sink.Run(_scheduler);

            internal sealed class _ : IdentitySink<TResult>
            {
                private readonly TResult _value;

                private IDisposable _task;

                public _(TResult value, IObserver<TResult> observer)
                    : base(observer)
                {
                    _value = value;
                }

                public void Run(IScheduler scheduler)
                {
                    var first = scheduler.Schedule(this, (innerScheduler, @this) => @this.LoopRecInf(innerScheduler));
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

                private IDisposable LoopRecInf(IScheduler scheduler)
                {
                    ForwardOnNext(_value);

                    var next = scheduler.Schedule(this, (innerScheduler, @this) => @this.LoopRecInf(innerScheduler));
                    Disposable.TrySetMultiple(ref _task, next);

                    return Disposable.Empty;
                }
            }
        }

        internal sealed class ForeverLongRunning : Producer<TResult, ForeverLongRunning._>
        {
            private readonly TResult _value;
            private readonly ISchedulerLongRunning _scheduler;

            public ForeverLongRunning(TResult value, ISchedulerLongRunning scheduler)
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

                public void Run(ISchedulerLongRunning longRunning)
                {
                    SetUpstream(longRunning.ScheduleLongRunning(this, (@this, c) => @this.LoopInf(c)));
                }

                private void LoopInf(ICancelable cancel)
                {
                    var value = _value;
                    while (!cancel.IsDisposed)
                    {
                        ForwardOnNext(value);
                    }

                    Dispose();
                }
            }
        }

        internal sealed class CountRecursive : Producer<TResult, CountRecursive._>
        {
            private readonly TResult _value;
            private readonly IScheduler _scheduler;
            private readonly int _repeatCount;

            public CountRecursive(TResult value, int repeatCount, IScheduler scheduler)
            {
                _value = value;
                _scheduler = scheduler;
                _repeatCount = repeatCount;
            }

            protected override _ CreateSink(IObserver<TResult> observer) => new _(_value, _repeatCount, observer);

            protected override void Run(_ sink) => sink.Run(_scheduler);

            internal sealed class _ : IdentitySink<TResult>
            {
                private readonly TResult _value;

                private int _remaining;

                private IDisposable _task;

                public _(TResult value, int repeatCount, IObserver<TResult> observer)
                    : base(observer)
                {
                    _value = value;
                    _remaining = repeatCount;
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
                    var remaining = _remaining;
                    if (remaining > 0)
                    {
                        ForwardOnNext(_value);
                        _remaining = --remaining;
                    }

                    if (remaining == 0)
                    {
                        ForwardOnCompleted();
                    }
                    else
                    {
                        var next = scheduler.Schedule(this, (innerScheduler, @this) => @this.LoopRec(innerScheduler));
                        Disposable.TrySetMultiple(ref _task, next);
                    }
                    return Disposable.Empty;
                }
            }
        }

        internal sealed class CountLongRunning : Producer<TResult, CountLongRunning._>
        {
            private readonly TResult _value;
            private readonly ISchedulerLongRunning _scheduler;
            private readonly int _repeatCount;

            public CountLongRunning(TResult value, int repeatCount, ISchedulerLongRunning scheduler)
            {
                _value = value;
                _scheduler = scheduler;
                _repeatCount = repeatCount;
            }

            protected override _ CreateSink(IObserver<TResult> observer) => new _(_value, _repeatCount, observer);

            protected override void Run(_ sink) => sink.Run(_scheduler);

            internal sealed class _ : IdentitySink<TResult>
            {
                private readonly TResult _value;
                private readonly int _remaining;

                public _(TResult value, int remaining, IObserver<TResult> observer)
                    : base(observer)
                {
                    _value = value;
                    _remaining = remaining;
                }

                public void Run(ISchedulerLongRunning longRunning)
                {
                    SetUpstream(longRunning.ScheduleLongRunning(this, (@this, cancel) => @this.Loop(cancel)));
                }

                private void Loop(ICancelable cancel)
                {
                    var value = _value;
                    var n = _remaining;
                    while (n > 0 && !cancel.IsDisposed)
                    {
                        ForwardOnNext(value);
                        n--;
                    }

                    if (!cancel.IsDisposed)
                    {
                        ForwardOnCompleted();
                    }
                }
            }
        }
    }
}

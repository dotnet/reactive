// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Disposables;

namespace System.Reactive.Linq.ObservableImpl
{
    internal static class Repeat<TResult>
    {
        internal sealed class Forever : Producer<TResult, Forever._>
        {
            private readonly TResult _value;
            private readonly IScheduler _scheduler;

            public Forever(TResult value, IScheduler scheduler)
            {
                _value = value;
                _scheduler = scheduler;
            }

            protected override _ CreateSink(IObserver<TResult> observer, IDisposable cancel) => new _(_value, observer, cancel);

            protected override IDisposable Run(_ sink) => sink.Run(this);

            internal sealed class _ : Sink<TResult>
            {
                private readonly TResult _value;

                public _(TResult value, IObserver<TResult> observer, IDisposable cancel)
                    : base(observer, cancel)
                {
                    _value = value;
                }

                public IDisposable Run(Forever parent)
                {
                    var longRunning = parent._scheduler.AsLongRunning();
                    if (longRunning != null)
                    {
                        return longRunning.ScheduleLongRunning(LoopInf);
                    }
                    else
                    {
                        return parent._scheduler.Schedule(LoopRecInf);
                    }
                }

                private void LoopRecInf(Action recurse)
                {
                    base._observer.OnNext(_value);
                    recurse();
                }

                private void LoopInf(ICancelable cancel)
                {
                    var value = _value;
                    while (!cancel.IsDisposed)
                        base._observer.OnNext(value);

                    base.Dispose();
                }
            }
        }

        internal sealed class Count : Producer<TResult, Count._>
        {
            private readonly TResult _value;
            private readonly IScheduler _scheduler;
            private readonly int _repeatCount;

            public Count(TResult value, int repeatCount, IScheduler scheduler)
            {
                _value = value;
                _scheduler = scheduler;
                _repeatCount = repeatCount;
            }

            protected override _ CreateSink(IObserver<TResult> observer, IDisposable cancel) => new _(_value, observer, cancel);

            protected override IDisposable Run(_ sink) => sink.Run(this);

            internal sealed class _ : Sink<TResult>
            {
                private readonly TResult _value;

                public _(TResult value, IObserver<TResult> observer, IDisposable cancel)
                    : base(observer, cancel)
                {
                    _value = value;
                }

                public IDisposable Run(Count parent)
                {
                    var longRunning = parent._scheduler.AsLongRunning();
                    if (longRunning != null)
                    {
                        return longRunning.ScheduleLongRunning(parent._repeatCount, Loop);
                    }
                    else
                    {
                        return parent._scheduler.Schedule(parent._repeatCount, LoopRec);
                    }
                }

                private void LoopRec(int n, Action<int> recurse)
                {
                    if (n > 0)
                    {
                        base._observer.OnNext(_value);
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

                private void Loop(int n, ICancelable cancel)
                {
                    var value = _value;
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
}

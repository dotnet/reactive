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

            protected override _ CreateSink(IObserver<TResult> observer) => new _(_value, observer);

            protected override void Run(_ sink) => sink.Run(this);

            internal sealed class _ : IdentitySink<TResult>
            {
                private readonly TResult _value;

                public _(TResult value, IObserver<TResult> observer)
                    : base(observer)
                {
                    _value = value;
                }

                public void Run(Forever parent)
                {
                    var longRunning = parent._scheduler.AsLongRunning();
                    if (longRunning != null)
                    {
                        SetUpstream(longRunning.ScheduleLongRunning(this, (@this, c) => @this.LoopInf(c)));
                    }
                    else
                    {
                        SetUpstream(parent._scheduler.Schedule(this, (@this, a) => @this.LoopRecInf(a)));
                    }
                }

                private void LoopRecInf(Action<_> recurse)
                {
                    ForwardOnNext(_value);
                    recurse(this);
                }

                private void LoopInf(ICancelable cancel)
                {
                    var value = _value;
                    while (!cancel.IsDisposed)
                        ForwardOnNext(value);

                    Dispose();
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

            protected override _ CreateSink(IObserver<TResult> observer) => new _(_value, observer);

            protected override void Run(_ sink) => sink.Run(this);

            internal sealed class _ : IdentitySink<TResult>
            {
                private readonly TResult _value;

                public _(TResult value, IObserver<TResult> observer)
                    : base(observer)
                {
                    _value = value;
                }

                public void Run(Count parent)
                {
                    var longRunning = parent._scheduler.AsLongRunning();
                    if (longRunning != null)
                    {
                        SetUpstream(longRunning.ScheduleLongRunning(parent._repeatCount, Loop));
                    }
                    else
                    {
                        SetUpstream(parent._scheduler.Schedule(parent._repeatCount, LoopRec));
                    }
                }

                private void LoopRec(int n, Action<int> recurse)
                {
                    if (n > 0)
                    {
                        ForwardOnNext(_value);
                        n--;
                    }

                    if (n == 0)
                    {
                        ForwardOnCompleted();
                        return;
                    }

                    recurse(n);
                }

                private void Loop(int n, ICancelable cancel)
                {
                    var value = _value;
                    while (n > 0 && !cancel.IsDisposed)
                    {
                        ForwardOnNext(value);
                        n--;
                    }

                    if (!cancel.IsDisposed)
                        ForwardOnCompleted();
                }
            }
        }
    }
}

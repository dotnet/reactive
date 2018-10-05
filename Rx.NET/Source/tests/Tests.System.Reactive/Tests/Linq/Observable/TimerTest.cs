// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using Microsoft.Reactive.Testing;
using ReactiveTests.Dummies;
using Xunit;

namespace ReactiveTests.Tests
{
    public class TimerTest : ReactiveTest
    {

        [Fact]
        public void OneShotTimer_TimeSpan_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Timer(TimeSpan.Zero, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Timer(TimeSpan.Zero, DummyScheduler.Instance).Subscribe(null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Timer(DateTimeOffset.Now, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Timer(TimeSpan.Zero, TimeSpan.Zero, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Timer(DateTimeOffset.Now, TimeSpan.Zero, null));

            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(-1)));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(-1), DummyScheduler.Instance));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Timer(DateTimeOffset.Now, TimeSpan.FromSeconds(-1)));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Timer(DateTimeOffset.Now, TimeSpan.FromSeconds(-1), DummyScheduler.Instance));
        }

        [Fact]
        public void OneShotTimer_TimeSpan_Basic()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Observable.Timer(TimeSpan.FromTicks(300), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(500, 0L),
                OnCompleted<long>(500)
            );
        }

        [Fact]
        public void OneShotTimer_TimeSpan_Zero()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Observable.Timer(TimeSpan.FromTicks(0), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(201, 0L),
                OnCompleted<long>(201)
            );
        }

        [Fact]
        public void OneShotTimer_TimeSpan_Zero_DefaultScheduler()
        {
            var scheduler = new TestScheduler();
            var observer = scheduler.CreateObserver<long>();
            var completed = new ManualResetEvent(false);

            Observable.Timer(TimeSpan.Zero).Subscribe(observer.OnNext, () => completed.Set());

            completed.WaitOne();

            Assert.Equal(1, observer.Messages.Count);
        }

        [Fact]
        public void OneShotTimer_TimeSpan_Negative()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Observable.Timer(TimeSpan.FromTicks(-1), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(201, 0L),
                OnCompleted<long>(201)
            );
        }

        [Fact]
        public void OneShotTimer_TimeSpan_Disposed()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Observable.Timer(TimeSpan.FromTicks(1000), scheduler)
            );

            res.Messages.AssertEqual(
            );
        }

        [Fact]
        public void OneShotTimer_TimeSpan_ObserverThrows()
        {
            var scheduler1 = new TestScheduler();

            var xs = Observable.Timer(TimeSpan.FromTicks(1), scheduler1);

            xs.Subscribe(x => { throw new InvalidOperationException(); });

            ReactiveAssert.Throws<InvalidOperationException>(() => scheduler1.Start());

            var scheduler2 = new TestScheduler();

            var ys = Observable.Timer(TimeSpan.FromTicks(1), scheduler2);

            ys.Subscribe(x => { }, ex => { }, () => { throw new InvalidOperationException(); });

            ReactiveAssert.Throws<InvalidOperationException>(() => scheduler2.Start());
        }

        [Fact]
        public void OneShotTimer_TimeSpan_DefaultScheduler()
        {
            Assert.True(Observable.Timer(TimeSpan.FromMilliseconds(1)).ToEnumerable().SequenceEqual(new[] { 0L }));
        }

        [Fact]
        public void OneShotTimer_DateTimeOffset_DefaultScheduler()
        {
            Assert.True(Observable.Timer(DateTimeOffset.UtcNow + TimeSpan.FromSeconds(1)).ToEnumerable().SequenceEqual(new[] { 0L }));
        }

        [Fact]
        public void OneShotTimer_TimeSpan_TimeSpan_DefaultScheduler()
        {
            Assert.True(Observable.Timer(TimeSpan.FromMilliseconds(1), TimeSpan.FromMilliseconds(1)).ToEnumerable().Take(2).SequenceEqual(new[] { 0L, 1L }));
        }

        [Fact]
        public void OneShotTimer_DateTimeOffset_TimeSpan_DefaultScheduler()
        {
            Assert.True(Observable.Timer(DateTimeOffset.UtcNow + TimeSpan.FromSeconds(1), TimeSpan.FromMilliseconds(1)).ToEnumerable().Take(2).SequenceEqual(new[] { 0L, 1L }));
        }

        [Fact]
        public void OneShotTimer_DateTimeOffset_Basic()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Observable.Timer(new DateTimeOffset(500, TimeSpan.Zero), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(500, 0L),
                OnCompleted<long>(500)
            );
        }

        [Fact]
        public void OneShotTimer_DateTimeOffset_Zero()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Observable.Timer(new DateTimeOffset(200, TimeSpan.Zero), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(201, 0L),
                OnCompleted<long>(201)
            );
        }

        [Fact]
        public void OneShotTimer_DateTimeOffset_Past()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Observable.Timer(new DateTimeOffset(0, TimeSpan.Zero), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(201, 0L),
                OnCompleted<long>(201)
            );
        }

        [Fact]
        public void RepeatingTimer_TimeSpan_Zero_DefaultScheduler()
        {
            var scheduler = new TestScheduler();
            var observer = scheduler.CreateObserver<long>();
            var completed = new ManualResetEvent(false);

            Observable.Timer(TimeSpan.Zero, TimeSpan.Zero).TakeWhile(i => i < 10).Subscribe(observer.OnNext, () => completed.Set());

            completed.WaitOne();

            Assert.Equal(10, observer.Messages.Count);
        }

        [Fact]
        public void RepeatingTimer_DateTimeOffset_TimeSpan_Simple()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Observable.Timer(new DateTimeOffset(300, TimeSpan.Zero), TimeSpan.FromTicks(100), scheduler),
                0, 200, 750
            );

            res.Messages.AssertEqual(
                OnNext(300, 0L),
                OnNext(400, 1L),
                OnNext(500, 2L),
                OnNext(600, 3L),
                OnNext(700, 4L)
            );
        }

        [Fact]
        public void RepeatingTimer_TimeSpan_TimeSpan_Simple()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Observable.Timer(TimeSpan.FromTicks(100), TimeSpan.FromTicks(100), scheduler),
                0, 200, 750
            );

            res.Messages.AssertEqual(
                OnNext(300, 0L),
                OnNext(400, 1L),
                OnNext(500, 2L),
                OnNext(600, 3L),
                OnNext(700, 4L)
            );
        }

        [Fact]
        public void RepeatingTimer_Periodic1()
        {
            var scheduler = new PeriodicTestScheduler();

            var res = scheduler.Start(() =>
                Observable.Timer(TimeSpan.FromTicks(50), TimeSpan.FromTicks(100), scheduler),
                0, 200, 700
            );

            res.Messages.AssertEqual(
                OnNext(250, 0L),
                OnNext(350, 1L),
                OnNext(450, 2L),
                OnNext(550, 3L),
                OnNext(650, 4L)
            );

#if !WINDOWS
            scheduler.Timers.AssertEqual(
                new TimerRun(250, 700) { 350, 450, 550, 650 }
            );
#endif
        }

        [Fact]
        public void RepeatingTimer_Periodic2()
        {
            var scheduler = new PeriodicTestScheduler();

            var res = scheduler.Start(() =>
                Observable.Timer(TimeSpan.FromTicks(100), TimeSpan.FromTicks(100), scheduler),
                0, 200, 750
            );

            res.Messages.AssertEqual(
                OnNext(300, 0L),
                OnNext(400, 1L),
                OnNext(500, 2L),
                OnNext(600, 3L),
                OnNext(700, 4L)
            );

#if !WINDOWS
            scheduler.Timers.AssertEqual(
                new TimerRun(200, 750) { 300, 400, 500, 600, 700 }
            );
#endif
        }

        [Fact]
        public void RepeatingTimer_UsingStopwatch_Slippage1()
        {
            var scheduler = new TestScheduler();

            var xs = default(IObservable<long>);
            scheduler.ScheduleAbsolute(100, () => { xs = Observable.Timer(TimeSpan.Zero, TimeSpan.FromTicks(100), scheduler); });

            var times = new List<long>();

            var onNext = new Action<long>(x =>
            {
                times.Add(scheduler.Clock);

                if (x == 0)
                {
                    return;
                }

                if (x < 2)
                {
                    scheduler.Sleep(50);
                    return;
                }

                if (x < 4)
                {
                    scheduler.Sleep(120);
                    return;
                }

                if (x < 6)
                {
                    scheduler.Sleep(50);
                    return;
                }

                if (x < 8)
                {
                    return;
                }
            });

            var d = default(IDisposable);
            scheduler.ScheduleAbsolute(200, () => { d = xs.Subscribe(onNext); });

            scheduler.ScheduleAbsolute(1000, () => { d.Dispose(); });

            scheduler.Start();

            times.AssertEqual(
                201, // 1 off because of initial scheduling jump (InvokeStart)
                301,
                401,
                522, // 2 off because of 401 + 120 + 1 scheduling tick
                643, // 3 off because of 522 + 120 + 1 scheduling tick
                701,
                801,
                901
            );
        }

        [Fact]
        public void RepeatingTimer_UsingStopwatch_Slippage2()
        {
            var scheduler = new TestScheduler();

            var xs = default(IObservable<long>);
            scheduler.ScheduleAbsolute(100, () => { xs = Observable.Timer(TimeSpan.FromTicks(100), TimeSpan.FromTicks(100), scheduler); });

            var times = new List<long>();

            var onNext = new Action<long>(x =>
            {
                times.Add(scheduler.Clock);

                if (x == 0)
                {
                    return;
                }

                if (x < 2)
                {
                    scheduler.Sleep(50);
                    return;
                }

                if (x < 4)
                {
                    scheduler.Sleep(120);
                    return;
                }

                if (x < 6)
                {
                    scheduler.Sleep(50);
                    return;
                }

                if (x < 8)
                {
                    return;
                }
            });

            var d = default(IDisposable);
            scheduler.ScheduleAbsolute(200, () => { d = xs.Subscribe(onNext); });

            scheduler.ScheduleAbsolute(1000, () => { d.Dispose(); });

            scheduler.Start();

            times.AssertEqual(
                300,
                400,
                500,
                621, // 1 off because of recursive scheduling beyond the target time
                742, // 2 off because of 621 + 120 + 1 scheduling tick
                800,
                900
            );
        }

        [Fact]
        public void RepeatingTimer_UsingStopwatch_Slippage3_CatchUpFromLongInvokeStart()
        {
            var scheduler = new TestScheduler();

            var xs = default(IObservable<long>);
            scheduler.ScheduleAbsolute(100, () => { xs = Observable.Timer(TimeSpan.Zero, TimeSpan.FromTicks(100), scheduler); });

            var times = new List<long>();

            var onNext = new Action<long>(x =>
            {
                times.Add(scheduler.Clock);

                if (x == 0)
                {
                    scheduler.Sleep(350);
                    return;
                }
            });

            var d = default(IDisposable);
            scheduler.ScheduleAbsolute(200, () => { d = xs.Subscribe(onNext); });

            scheduler.ScheduleAbsolute(1000, () => { d.Dispose(); });

            scheduler.Start();

            times.AssertEqual(
                201, // 1 off because of initial scheduling jump (InvokeStart)
                551, // catching up after excessive delay of 350 (target was 300)
                552, // catching up after excessive delay of 350 (target was 400)
                553, // catching up after excessive delay of 350 (target was 500)
                601, // back in sync
                701,
                801,
                901
            );
        }

        [Fact]
        public void RepeatingTimer_UsingStopwatch_Slippage3_CatchUpFromLongInvokeStart_ThrowsFirst()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = default(IObservable<long>);
            scheduler.ScheduleAbsolute(100, () => { xs = Observable.Timer(TimeSpan.Zero, TimeSpan.FromTicks(100), scheduler); });

            var onNext = new Action<long>(x =>
            {
                if (x == 0)
                {
                    throw ex;
                }
            });

            var d = default(IDisposable);
            scheduler.ScheduleAbsolute(200, () => { d = xs.Subscribe(onNext); });

            scheduler.ScheduleAbsolute(1000, () => { d.Dispose(); });

            try
            {
                scheduler.Start();
            }
            catch (Exception e)
            {
                Assert.Equal(201, scheduler.Clock);
                Assert.Same(ex, e);
            }
        }

        [Fact]
        public void RepeatingTimer_UsingStopwatch_Slippage3_CatchUpFromLongInvokeStart_ThrowsBeyondFirst()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = default(IObservable<long>);
            scheduler.ScheduleAbsolute(100, () => { xs = Observable.Timer(TimeSpan.Zero, TimeSpan.FromTicks(100), scheduler); });

            var times = new List<long>();

            var onNext = new Action<long>(x =>
            {
                times.Add(scheduler.Clock);

                if (x == 0)
                {
                    scheduler.Sleep(350);
                    return;
                }

                if (x == 5)
                {
                    throw ex;
                }
            });

            var d = default(IDisposable);
            scheduler.ScheduleAbsolute(200, () => { d = xs.Subscribe(onNext); });

            scheduler.ScheduleAbsolute(1000, () => { d.Dispose(); });

            try
            {
                scheduler.Start();
            }
            catch (Exception e)
            {
                Assert.Equal(701, scheduler.Clock);
                Assert.Same(ex, e);
            }

            times.AssertEqual(
                201, // 1 off because of initial scheduling jump (InvokeStart)
                551, // catching up after excessive delay of 350 (target was 300)
                552, // catching up after excessive delay of 350 (target was 400)
                553, // catching up after excessive delay of 350 (target was 500)
                601, // back in sync
                701
            );
        }

        [Fact]
        public void RepeatingTimer_NoStopwatch_Slippage1()
        {
            var scheduler = new TestScheduler();

            var xs = default(IObservable<long>);
            scheduler.ScheduleAbsolute(100, () => { xs = Observable.Timer(TimeSpan.Zero, TimeSpan.FromTicks(100), scheduler.DisableOptimizations(typeof(IStopwatchProvider))); });

            var times = new List<long>();

            var onNext = new Action<long>(x =>
            {
                times.Add(scheduler.Clock);

                if (x == 0)
                {
                    return;
                }

                if (x < 2)
                {
                    scheduler.Sleep(50);
                    return;
                }

                if (x < 4)
                {
                    scheduler.Sleep(120);
                    return;
                }

                if (x < 6)
                {
                    scheduler.Sleep(50);
                    return;
                }

                if (x < 8)
                {
                    return;
                }
            });

            var d = default(IDisposable);
            scheduler.ScheduleAbsolute(200, () => { d = xs.Subscribe(onNext); });

            scheduler.ScheduleAbsolute(1000, () => { d.Dispose(); });

            scheduler.Start();

            times.AssertEqual(
                201, // 1 off because of initial scheduling jump (InvokeStart)
                301,
                401,
                523, // 3 off because of 401 + 120 + 2 scheduling ticks (one due to yield in SchedulePeriodic emulation code)
                645, // 5 off because of 523 + 120 + 2 scheduling ticks (one due to yield in SchedulePeriodic emulation code)
                743, // \
                843, //  +--> 43 off because this situation (no stopwatch or periodic scheduling interface) only gets best effort treatment (see SchedulePeriodic emulation code)
                943  // /
            );
        }

        [Fact]
        public void RepeatingTimer_NoStopwatch_Slippage2()
        {
            var scheduler = new TestScheduler();

            var xs = default(IObservable<long>);
            scheduler.ScheduleAbsolute(100, () => { xs = Observable.Timer(TimeSpan.FromTicks(100), TimeSpan.FromTicks(100), scheduler.DisableOptimizations(typeof(IStopwatchProvider))); });

            var times = new List<long>();

            var onNext = new Action<long>(x =>
            {
                times.Add(scheduler.Clock);

                if (x == 0)
                {
                    return;
                }

                if (x < 2)
                {
                    scheduler.Sleep(50);
                    return;
                }

                if (x < 4)
                {
                    scheduler.Sleep(120);
                    return;
                }

                if (x < 6)
                {
                    scheduler.Sleep(50);
                    return;
                }

                if (x < 8)
                {
                    return;
                }
            });

            var d = default(IDisposable);
            scheduler.ScheduleAbsolute(200, () => { d = xs.Subscribe(onNext); });

            scheduler.ScheduleAbsolute(1000, () => { d.Dispose(); });

            scheduler.Start();

            times.AssertEqual(
                300,
                400,
                500,
                622, // 2 off because of 500 + 120 + 2 scheduling ticks (one due to yield in SchedulePeriodic emulation code)
                744, // 4 off because of 622 + 120 + 2 scheduling ticks (one due to yield in SchedulePeriodic emulation code)
                842, // |
                942  // +--> 42 off because this situation (no stopwatch or periodic scheduling interface) only gets best effort treatment (see SchedulePeriodic emulation code)
            );
        }

#if !NO_THREAD
        [Fact]
        public void RepeatingTimer_Start_CatchUp()
        {
            var e = new ManualResetEvent(false);

            var xs = Observable.Timer(TimeSpan.Zero, TimeSpan.FromMilliseconds(10));

            var d = new SingleAssignmentDisposable();
            d.Disposable = xs.Subscribe(x =>
            {
                if (x == 0)
                {
                    Thread.Sleep(500);
                }

                if (x > 10)
                {
                    e.Set();
                    d.Dispose();
                }
            });

            e.WaitOne();
        }

        [Fact]
        public void RepeatingTimer_Start_CatchUp_Throws()
        {
            var end = new ManualResetEvent(false);

            var err = new Exception();
            var ex = default(Exception);

            var s = ThreadPoolScheduler.Instance.Catch<Exception>(e =>
            {
                Interlocked.Exchange(ref ex, e);
                end.Set();
                return true;
            });

            var xs = Observable.Timer(TimeSpan.Zero, TimeSpan.FromMilliseconds(10), s);

            xs.Subscribe(x =>
            {
                if (x == 0)
                {
                    Thread.Sleep(500);
                }

                if (x == 5)
                {
                    throw err;
                }
            });

            end.WaitOne();

            Assert.Same(err, ex);
        }
#endif

    }

    internal class SchedulerWithCatch : IServiceProvider, IScheduler
    {
        private readonly IScheduler _scheduler;
        private readonly Action<Exception> _setException;

        public SchedulerWithCatch(IScheduler scheduler, Action<Exception> setException)
        {
            _scheduler = scheduler;
            _setException = setException;
        }

        public object GetService(Type serviceType)
        {
            return ((IServiceProvider)_scheduler).GetService(serviceType);
        }

        public DateTimeOffset Now
        {
            get { return _scheduler.Now; }
        }

        public IDisposable Schedule<TState>(TState state, Func<IScheduler, TState, IDisposable> action)
        {
            return _scheduler.Schedule(state, GetCatch(action));
        }

        public IDisposable Schedule<TState>(TState state, TimeSpan dueTime, Func<IScheduler, TState, IDisposable> action)
        {
            return _scheduler.Schedule(state, dueTime, GetCatch(action));
        }

        public IDisposable Schedule<TState>(TState state, DateTimeOffset dueTime, Func<IScheduler, TState, IDisposable> action)
        {
            return _scheduler.Schedule(state, dueTime, GetCatch(action));
        }

        private Func<IScheduler, TState, IDisposable> GetCatch<TState>(Func<IScheduler, TState, IDisposable> action)
        {
            return (self, s) =>
            {
                try
                {
                    return action(new SchedulerWithCatch(self, _setException), s);
                }
                catch (Exception ex)
                {
                    _setException(ex);
                    return Disposable.Empty;
                }
            };
        }
    }

    internal class PeriodicTestScheduler : TestScheduler, ISchedulerPeriodic, IServiceProvider
    {
        private readonly List<TimerRun> _timers;

        public PeriodicTestScheduler()
        {
            _timers = new List<TimerRun>();
        }

        public IDisposable SchedulePeriodic<TState>(TState state, TimeSpan period, Func<TState, TState> action)
        {
            var run = new TimerRun(Clock);
            _timers.Add(run);

            var x = state;

            var d = this.Schedule(period, self =>
            {
                run.Add(Clock);

                x = action(x);
                self(period);
            });

            return new CompositeDisposable(
                Disposable.Create(() => { run.Stop(Clock); }),
                d
            );
        }

        public List<TimerRun> Timers
        {
            get { return _timers; }
        }

        protected override object GetService(Type serviceType)
        {
            if (serviceType == typeof(ISchedulerPeriodic))
            {
                return this as ISchedulerPeriodic;
            }

            return base.GetService(serviceType);
        }
    }

    internal class TimerRun : IEnumerable<long>
    {
        private readonly long _started;
        private long _stopped;
        private bool _hasStopped;
        private readonly List<long> _ticks;

        public TimerRun(long started)
        {
            _started = started;
            _ticks = new List<long>();
        }

        public TimerRun(long started, long stopped)
        {
            _started = started;
            _stopped = stopped;
            _hasStopped = true;
            _ticks = new List<long>();
        }

        public override int GetHashCode()
        {
            return 0;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is TimerRun other))
            {
                return false;
            }

            return _started == other._started && _stopped == other._stopped && _ticks.SequenceEqual(other._ticks);
        }

        public long Started
        {
            get { return _started; }
        }

        public IEnumerable<long> Ticks
        {
            get { return _ticks; }
        }

        public long Stopped
        {
            get { return _stopped; }
        }

        internal void Stop(long clock)
        {
            _stopped = clock;
            _hasStopped = true;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append("Start(" + _started + ") ");
            sb.Append("Ticks(" + string.Join(", ", _ticks.Select(t => t.ToString()).ToArray()) + ") ");
            if (_hasStopped)
            {
                sb.Append("Stop(" + _stopped + ")");
            }

            return sb.ToString();
        }

        public IEnumerator<long> GetEnumerator()
        {
            return _ticks.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _ticks.GetEnumerator();
        }

        public void Add(long clock)
        {
            _ticks.Add(clock);
        }
    }
}

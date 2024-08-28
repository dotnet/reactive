// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Threading;

namespace System.Reactive.Linq.ObservableImpl
{
    internal static class Delay<TSource>
    {
        internal abstract class Base<TParent> : Producer<TSource, Base<TParent>._>
            where TParent : Base<TParent>
        {
            protected readonly IObservable<TSource> _source;
            protected readonly IScheduler _scheduler;

            protected Base(IObservable<TSource> source, IScheduler scheduler)
            {
                _source = source;
                _scheduler = scheduler;
            }

            internal abstract class _ : IdentitySink<TSource>
            {
                protected readonly IScheduler _scheduler;
                private IStopwatch? _watch;

                protected _(TParent parent, IObserver<TSource> observer)
                    : base(observer)
                {
                    _scheduler = parent._scheduler;
                }

                public void Run(TParent parent)
                {
                    _watch = _scheduler.StartStopwatch();

                    RunCore(parent);

                    base.Run(parent._source);
                }

                protected TimeSpan Elapsed => _watch!.Elapsed; // NB: Only used after Run is called.

                protected abstract void RunCore(TParent parent);
            }

            internal abstract class S : _
            {
                protected readonly object _gate = new();
                protected SerialDisposableValue _cancelable;

                protected S(TParent parent, IObserver<TSource> observer)
                    : base(parent, observer)
                {
                }

                protected TimeSpan _delay;
                protected bool _ready;
                protected bool _active;
                protected bool _running;
                protected Queue<Reactive.TimeInterval<TSource>> _queue = new();

                private bool _hasCompleted;
                private TimeSpan _completeAt;
                private bool _hasFailed;
                private Exception? _exception;

                protected override void Dispose(bool disposing)
                {
                    base.Dispose(disposing);

                    if (disposing)
                    {
                        _cancelable.Dispose();
                    }
                }

                public override void OnNext(TSource value)
                {
                    var shouldRun = false;

                    lock (_gate)
                    {
                        var next = Elapsed.Add(_delay);

                        _queue.Enqueue(new Reactive.TimeInterval<TSource>(value, next));

                        shouldRun = _ready && !_active;
                        _active = true;
                    }

                    if (shouldRun)
                    {
                        DrainQueue(_delay);
                    }
                }

                public override void OnError(Exception error)
                {
                    DisposeUpstream();

                    var shouldRun = false;

                    lock (_gate)
                    {
                        _queue.Clear();

                        _exception = error;
                        _hasFailed = true;

                        shouldRun = !_running;
                    }

                    if (shouldRun)
                    {
                        ForwardOnError(error);
                    }
                }

                public override void OnCompleted()
                {
                    DisposeUpstream();

                    var shouldRun = false;

                    lock (_gate)
                    {
                        var next = Elapsed.Add(_delay);

                        _completeAt = next;
                        _hasCompleted = true;

                        shouldRun = _ready && !_active;
                        _active = true;
                    }

                    if (shouldRun)
                    {
                        DrainQueue(_delay);
                    }
                }

                protected void DrainQueue(TimeSpan next)
                {
                    _cancelable.Disposable = _scheduler.Schedule(this, next, static (@this, a) => @this.DrainQueue(a));
                }

                private void DrainQueue(Action<S, TimeSpan> recurse)
                {
                    lock (_gate)
                    {
                        if (_hasFailed)
                        {
                            return;
                        }

                        _running = true;
                    }

                    //
                    // The shouldYield flag was added to address TFS 487881: "Delay can be unfair". In the old
                    // implementation, the loop below kept running while there was work for immediate dispatch,
                    // potentially causing a long running work item on the target scheduler. With the addition
                    // of long-running scheduling in Rx v2.0, we can check whether the scheduler supports this
                    // interface and perform different processing (see LongRunningImpl). To reduce the code 
                    // churn in the old loop code here, we set the shouldYield flag to true after the first 
                    // dispatch iteration, in order to break from the loop and enter the recursive scheduling path.
                    //
                    var shouldYield = false;

                    while (true)
                    {
                        var hasFailed = false;
                        var error = default(Exception);

                        var hasValue = false;
                        var value = default(TSource);
                        var hasCompleted = false;

                        var shouldRecurse = false;
                        var recurseDueTime = default(TimeSpan);

                        lock (_gate)
                        {
                            if (_hasFailed)
                            {
                                error = _exception;
                                hasFailed = true;
                                _running = false;
                            }
                            else
                            {
                                var now = Elapsed;

                                if (_queue.Count > 0)
                                {
                                    var nextDue = _queue.Peek().Interval;

                                    if (nextDue.CompareTo(now) <= 0 && !shouldYield)
                                    {
                                        value = _queue.Dequeue().Value;
                                        hasValue = true;
                                    }
                                    else
                                    {
                                        shouldRecurse = true;
                                        recurseDueTime = Scheduler.Normalize(nextDue.Subtract(now));
                                        _running = false;
                                    }
                                }
                                else if (_hasCompleted)
                                {
                                    if (_completeAt.CompareTo(now) <= 0 && !shouldYield)
                                    {
                                        hasCompleted = true;
                                    }
                                    else
                                    {
                                        shouldRecurse = true;
                                        recurseDueTime = Scheduler.Normalize(_completeAt.Subtract(now));
                                        _running = false;
                                    }
                                }
                                else
                                {
                                    _running = false;
                                    _active = false;
                                }
                            }
                        } /* lock (_gate) */

                        if (hasValue)
                        {
                            ForwardOnNext(value!);
                            shouldYield = true;
                        }
                        else
                        {
                            if (hasCompleted)
                            {
                                ForwardOnCompleted();
                            }
                            else if (hasFailed)
                            {
                                ForwardOnError(error!);
                            }
                            else if (shouldRecurse)
                            {
                                recurse(this, recurseDueTime);
                            }

                            return;
                        }
                    } /* while (true) */
                }
            }

            protected abstract class L : _
            {
                protected readonly object _gate = new();
                private readonly SemaphoreSlim _evt = new(0);

                protected L(TParent parent, IObserver<TSource> observer)
                    : base(parent, observer)
                {
                }

                protected Queue<Reactive.TimeInterval<TSource>> _queue = new();
                protected SerialDisposableValue _cancelable;
                protected TimeSpan _delay;

                private bool _hasCompleted;
                private TimeSpan _completeAt;
                private bool _hasFailed;
                private Exception? _exception;

                protected override void Dispose(bool disposing)
                {
                    base.Dispose(disposing);

                    if (disposing)
                    {
                        _cancelable.Dispose();
                    }
                }

                protected void ScheduleDrain()
                {
                    var cd = new CancellationDisposable();
                    _cancelable.Disposable = cd;

                    _scheduler.AsLongRunning()!.ScheduleLongRunning(cd.Token, DrainQueue); // NB: This class is only used with long-running schedulers.
                }

                public override void OnNext(TSource value)
                {
                    lock (_gate)
                    {
                        var next = Elapsed.Add(_delay);

                        _queue.Enqueue(new Reactive.TimeInterval<TSource>(value, next));

                        _evt.Release();
                    }
                }

                public override void OnError(Exception error)
                {
                    DisposeUpstream();

                    lock (_gate)
                    {
                        _queue.Clear();

                        _exception = error;
                        _hasFailed = true;

                        _evt.Release();
                    }
                }

                public override void OnCompleted()
                {
                    DisposeUpstream();

                    lock (_gate)
                    {
                        var next = Elapsed.Add(_delay);

                        _completeAt = next;
                        _hasCompleted = true;

                        _evt.Release();
                    }
                }

#pragma warning disable CA1068 // (CancellationToken parameters must come last.) Method signature determined by ISchedulerLongRunning, so we can't comply with the analyzer rule here.
                private void DrainQueue(CancellationToken token, ICancelable cancel)
#pragma warning restore CA1068
                {
                    while (true)
                    {
                        try
                        {
                            _evt.Wait(token);
                        }
                        catch (OperationCanceledException)
                        {
                            return;
                        }

                        var hasFailed = false;
                        var error = default(Exception);

                        var hasValue = false;
                        var value = default(TSource);
                        var hasCompleted = false;

                        var shouldWait = false;
                        var waitTime = default(TimeSpan);

                        lock (_gate)
                        {
                            if (_hasFailed)
                            {
                                error = _exception;
                                hasFailed = true;
                            }
                            else
                            {
                                var now = Elapsed;

                                if (_queue.Count > 0)
                                {
                                    var next = _queue.Dequeue();

                                    hasValue = true;
                                    value = next.Value;

                                    var nextDue = next.Interval;
                                    if (nextDue.CompareTo(now) > 0)
                                    {
                                        shouldWait = true;
                                        waitTime = Scheduler.Normalize(nextDue.Subtract(now));
                                    }
                                }
                                else if (_hasCompleted)
                                {
                                    hasCompleted = true;

                                    if (_completeAt.CompareTo(now) > 0)
                                    {
                                        shouldWait = true;
                                        waitTime = Scheduler.Normalize(_completeAt.Subtract(now));
                                    }
                                }
                            }
                        } /* lock (_gate) */

                        if (shouldWait)
                        {
                            var timer = new ManualResetEventSlim();
                            _scheduler.ScheduleAction(timer, waitTime, static slimTimer => { slimTimer.Set(); });

                            try
                            {
                                timer.Wait(token);
                            }
                            catch (OperationCanceledException)
                            {
                                return;
                            }
                        }

                        if (hasValue)
                        {
                            ForwardOnNext(value!);
                        }
                        else
                        {
                            if (hasCompleted)
                            {
                                ForwardOnCompleted();
                            }
                            else if (hasFailed)
                            {
                                ForwardOnError(error!);
                            }

                            return;
                        }
                    }
                }
            }
        }

        internal sealed class Absolute : Base<Absolute>
        {
            private readonly DateTimeOffset _dueTime;

            public Absolute(IObservable<TSource> source, DateTimeOffset dueTime, IScheduler scheduler)
                : base(source, scheduler)
            {
                _dueTime = dueTime;
            }

            protected override _ CreateSink(IObserver<TSource> observer) => _scheduler.AsLongRunning() != null ? (_)new L(this, observer) : new S(this, observer);

            protected override void Run(_ sink) => sink.Run(this);

            private new sealed class S : Base<Absolute>.S
            {
                public S(Absolute parent, IObserver<TSource> observer)
                    : base(parent, observer)
                {
                }

                protected override void RunCore(Absolute parent)
                {
                    _ready = false;

                    _cancelable.TrySetFirst(parent._scheduler.ScheduleAction(this, parent._dueTime, static @this => @this.Start()));
                }

                private void Start()
                {
                    var next = default(TimeSpan);
                    var shouldRun = false;

                    lock (_gate)
                    {
                        _delay = Elapsed;

                        var oldQueue = _queue;
                        _queue = new Queue<Reactive.TimeInterval<TSource>>();

                        if (oldQueue.Count > 0)
                        {
                            next = oldQueue.Peek().Interval;

                            while (oldQueue.Count > 0)
                            {
                                var item = oldQueue.Dequeue();
                                _queue.Enqueue(new Reactive.TimeInterval<TSource>(item.Value, item.Interval.Add(_delay)));
                            }

                            shouldRun = true;
                            _active = true;
                        }

                        _ready = true;
                    }

                    if (shouldRun)
                    {
                        DrainQueue(next);
                    }
                }
            }

            private new sealed class L : Base<Absolute>.L
            {
                public L(Absolute parent, IObserver<TSource> observer)
                    : base(parent, observer)
                {
                }

                protected override void RunCore(Absolute parent)
                {
                    // ScheduleDrain might have already set a newer disposable
                    // using TrySetSerial would cancel it, stopping the emission
                    // and hang the consumer
                    _cancelable.TrySetFirst(parent._scheduler.ScheduleAction(this, parent._dueTime, static @this => @this.Start()));
                }

                private void Start()
                {
                    lock (_gate)
                    {
                        _delay = Elapsed;

                        var oldQueue = _queue;
                        _queue = new Queue<Reactive.TimeInterval<TSource>>();

                        while (oldQueue.Count > 0)
                        {
                            var item = oldQueue.Dequeue();
                            _queue.Enqueue(new Reactive.TimeInterval<TSource>(item.Value, item.Interval.Add(_delay)));
                        }
                    }

                    ScheduleDrain();
                }
            }
        }

        internal sealed class Relative : Base<Relative>
        {
            private readonly TimeSpan _dueTime;

            public Relative(IObservable<TSource> source, TimeSpan dueTime, IScheduler scheduler)
                : base(source, scheduler)
            {
                _dueTime = dueTime;
            }

            protected override _ CreateSink(IObserver<TSource> observer) => _scheduler.AsLongRunning() != null ? (_)new L(this, observer) : new S(this, observer);

            protected override void Run(_ sink) => sink.Run(this);

            private new sealed class S : Base<Relative>.S
            {
                public S(Relative parent, IObserver<TSource> observer)
                    : base(parent, observer)
                {
                }

                protected override void RunCore(Relative parent)
                {
                    _ready = true;

                    _delay = Scheduler.Normalize(parent._dueTime);
                }
            }

            private new sealed class L : Base<Relative>.L
            {
                public L(Relative parent, IObserver<TSource> observer)
                    : base(parent, observer)
                {
                }

                protected override void RunCore(Relative parent)
                {
                    _delay = Scheduler.Normalize(parent._dueTime);
                    ScheduleDrain();
                }
            }
        }
    }

    internal static class Delay<TSource, TDelay>
    {
        internal abstract class Base<TParent> : Producer<TSource, Base<TParent>._>
            where TParent : Base<TParent>
        {
            protected readonly IObservable<TSource> _source;

            protected Base(IObservable<TSource> source)
            {
                _source = source;
            }

            internal abstract class _ : IdentitySink<TSource>
            {
                private readonly CompositeDisposable _delays = [];
                private readonly object _gate = new();

                private readonly Func<TSource, IObservable<TDelay>> _delaySelector;

                protected _(Func<TSource, IObservable<TDelay>> delaySelector, IObserver<TSource> observer)
                    : base(observer)
                {
                    _delaySelector = delaySelector;
                }

                private bool _atEnd;
                private SingleAssignmentDisposableValue _subscription;

                public void Run(TParent parent)
                {
                    _atEnd = false;

                    _subscription.Disposable = RunCore(parent);
                }

                protected override void Dispose(bool disposing)
                {
                    if (disposing)
                    {
                        _subscription.Dispose();
                        _delays.Dispose();
                    }
                    base.Dispose(disposing);
                }

                protected abstract IDisposable RunCore(TParent parent);

                public override void OnNext(TSource value)
                {
                    IObservable<TDelay> delay;
                    try
                    {
                        delay = _delaySelector(value);
                    }
                    catch (Exception error)
                    {
                        lock (_gate)
                        {
                            ForwardOnError(error);
                        }

                        return;
                    }

                    var observer = new DelayObserver(this, value);
                    _delays.Add(observer);
                    observer.SetResource(delay.SubscribeSafe(observer));
                }

                public override void OnError(Exception error)
                {
                    lock (_gate)
                    {
                        ForwardOnError(error);
                    }
                }

                public override void OnCompleted()
                {
                    lock (_gate)
                    {
                        _atEnd = true;
                        _subscription.Dispose();

                        CheckDone();
                    }
                }

                private void CheckDone()
                {
                    if (_atEnd && _delays.Count == 0)
                    {
                        ForwardOnCompleted();
                    }
                }

                private sealed class DelayObserver : SafeObserver<TDelay>
                {
                    private readonly _ _parent;
                    private readonly TSource _value;
                    private bool _once;

                    public DelayObserver(_ parent, TSource value)
                    {
                        _parent = parent;
                        _value = value;
                    }

                    public override void OnNext(TDelay value)
                    {
                        if (!_once)
                        {
                            _once = true;
                            lock (_parent._gate)
                            {
                                _parent.ForwardOnNext(_value);

                                _parent._delays.Remove(this);
                                _parent.CheckDone();
                            }
                        }
                    }

                    public override void OnError(Exception error)
                    {
                        lock (_parent._gate)
                        {
                            _parent.ForwardOnError(error);
                        }
                    }

                    public override void OnCompleted()
                    {
                        if (!_once)
                        {
                            lock (_parent._gate)
                            {
                                _parent.ForwardOnNext(_value);

                                _parent._delays.Remove(this);
                                _parent.CheckDone();
                            }
                        }
                    }
                }
            }
        }

        internal class Selector : Base<Selector>
        {
            private readonly Func<TSource, IObservable<TDelay>> _delaySelector;

            public Selector(IObservable<TSource> source, Func<TSource, IObservable<TDelay>> delaySelector)
                : base(source)
            {
                _delaySelector = delaySelector;
            }

            protected override Base<Selector>._ CreateSink(IObserver<TSource> observer) => new _(_delaySelector, observer);

            protected override void Run(Base<Selector>._ sink) => sink.Run(this);

            private new sealed class _ : Base<Selector>._
            {
                public _(Func<TSource, IObservable<TDelay>> delaySelector, IObserver<TSource> observer)
                    : base(delaySelector, observer)
                {
                }

                protected override IDisposable RunCore(Selector parent) => parent._source.SubscribeSafe(this);
            }
        }

        internal sealed class SelectorWithSubscriptionDelay : Base<SelectorWithSubscriptionDelay>
        {
            private readonly IObservable<TDelay> _subscriptionDelay;
            private readonly Func<TSource, IObservable<TDelay>> _delaySelector;

            public SelectorWithSubscriptionDelay(IObservable<TSource> source, IObservable<TDelay> subscriptionDelay, Func<TSource, IObservable<TDelay>> delaySelector)
                : base(source)
            {
                _subscriptionDelay = subscriptionDelay;
                _delaySelector = delaySelector;
            }

            protected override Base<SelectorWithSubscriptionDelay>._ CreateSink(IObserver<TSource> observer) => new _(_delaySelector, observer);

            protected override void Run(Base<SelectorWithSubscriptionDelay>._ sink) => sink.Run(this);

            private new sealed class _ : Base<SelectorWithSubscriptionDelay>._
            {
                public _(Func<TSource, IObservable<TDelay>> delaySelector, IObserver<TSource> observer)
                    : base(delaySelector, observer)
                {
                }

                protected override IDisposable RunCore(SelectorWithSubscriptionDelay parent)
                {
                    var delayConsumer = new SubscriptionDelayObserver(this, parent._source);

                    delayConsumer.SetFirst(parent._subscriptionDelay.SubscribeSafe(delayConsumer));

                    return delayConsumer;
                }

                private sealed class SubscriptionDelayObserver : IObserver<TDelay>, IDisposable
                {
                    private readonly _ _parent;
                    private readonly IObservable<TSource> _source;
                    private SerialDisposableValue _subscription;

                    public SubscriptionDelayObserver(_ parent, IObservable<TSource> source)
                    {
                        _parent = parent;
                        _source = source;
                    }

                    internal void SetFirst(IDisposable d)
                    {
                        _subscription.TrySetFirst(d);
                    }

                    public void OnNext(TDelay value)
                    {
                        _subscription.Disposable = _source.SubscribeSafe(_parent);
                    }

                    public void OnError(Exception error)
                    {
                        _parent.ForwardOnError(error);
                    }

                    public void OnCompleted()
                    {
                        _subscription.Disposable = _source.SubscribeSafe(_parent);
                    }

                    public void Dispose()
                    {
                        _subscription.Dispose();
                    }
                }
            }
        }
    }
}

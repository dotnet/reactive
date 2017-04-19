// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Threading;

namespace System.Reactive.Linq.ObservableImpl
{
    internal static class Delay<TSource>
    {
        internal abstract class Base : Producer<TSource>
        {
            protected readonly IObservable<TSource> _source;
            protected readonly IScheduler _scheduler;

            public Base(IObservable<TSource> source, IScheduler scheduler)
            {
                _source = source;
                _scheduler = scheduler;
            }

            protected abstract class _<TParent> : Sink<TSource>, IObserver<TSource>
                where TParent : Base
            {
                protected readonly object _gate = new object();
                protected readonly SerialDisposable _cancelable = new SerialDisposable();

                protected readonly IScheduler _scheduler;

                public _(TParent parent, IObserver<TSource> observer, IDisposable cancel)
                    : base(observer, cancel)
                {
                    _scheduler = parent._scheduler;
                }

                private IDisposable _sourceSubscription;

                protected IStopwatch _watch;
                protected TimeSpan _delay;
                protected bool _ready;
                protected bool _active;
                protected bool _running;
                protected Queue<System.Reactive.TimeInterval<TSource>> _queue = new Queue<Reactive.TimeInterval<TSource>>();

                private bool _hasCompleted;
                private TimeSpan _completeAt;
                private bool _hasFailed;
                private Exception _exception;

                public IDisposable Run(TParent parent)
                {
                    _active = false;
                    _running = false;
                    _queue = new Queue<System.Reactive.TimeInterval<TSource>>();
                    _hasCompleted = false;
                    _completeAt = default(TimeSpan);
                    _hasFailed = false;
                    _exception = default(Exception);

                    _watch = _scheduler.StartStopwatch();

                    RunCore(parent);

                    var sourceSubscription = new SingleAssignmentDisposable();
                    _sourceSubscription = sourceSubscription;
                    sourceSubscription.Disposable = parent._source.SubscribeSafe(this);

                    return StableCompositeDisposable.Create(_sourceSubscription, _cancelable);
                }

                protected abstract void RunCore(TParent parent);

                public void OnNext(TSource value)
                {
                    var next = _watch.Elapsed.Add(_delay);
                    var shouldRun = false;

                    lock (_gate)
                    {
                        _queue.Enqueue(new System.Reactive.TimeInterval<TSource>(value, next));

                        shouldRun = _ready && !_active;
                        _active = true;
                    }

                    if (shouldRun)
                    {
                        _cancelable.Disposable = _scheduler.Schedule(_delay, DrainQueue);
                    }
                }

                public void OnError(Exception error)
                {
                    _sourceSubscription.Dispose();

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
                        base._observer.OnError(error);
                        base.Dispose();
                    }
                }

                public void OnCompleted()
                {
                    _sourceSubscription.Dispose();

                    var next = _watch.Elapsed.Add(_delay);
                    var shouldRun = false;

                    lock (_gate)
                    {
                        _completeAt = next;
                        _hasCompleted = true;

                        shouldRun = _ready && !_active;
                        _active = true;
                    }

                    if (shouldRun)
                    {
                        _cancelable.Disposable = _scheduler.Schedule(_delay, DrainQueue);
                    }
                }

                protected void DrainQueue(Action<TimeSpan> recurse)
                {
                    lock (_gate)
                    {
                        if (_hasFailed)
                            return;
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
                                var now = _watch.Elapsed;

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
                            base._observer.OnNext(value);
                            shouldYield = true;
                        }
                        else
                        {
                            if (hasCompleted)
                            {
                                base._observer.OnCompleted();
                                base.Dispose();
                            }
                            else if (hasFailed)
                            {
                                base._observer.OnError(error);
                                base.Dispose();
                            }
                            else if (shouldRecurse)
                            {
                                recurse(recurseDueTime);
                            }

                            return;
                        }
                    } /* while (true) */
                }
            }

            protected abstract class L<TParent> : Sink<TSource>, IObserver<TSource>
                where TParent : Base
            {
                protected readonly object _gate = new object();
                protected readonly SerialDisposable _cancelable = new SerialDisposable();
                private readonly SemaphoreSlim _evt = new SemaphoreSlim(0);

                private readonly IScheduler _scheduler;

                public L(TParent parent, IObserver<TSource> observer, IDisposable cancel)
                    : base(observer, cancel)
                {
                    _scheduler = parent._scheduler;
                }

                private IDisposable _sourceSubscription;

                protected IStopwatch _watch;
                protected TimeSpan _delay;
                protected Queue<System.Reactive.TimeInterval<TSource>> _queue;

                private CancellationTokenSource _stop;
                private bool _hasCompleted;
                private TimeSpan _completeAt;
                private bool _hasFailed;
                private Exception _exception;

                public IDisposable Run(TParent parent)
                {
                    _queue = new Queue<System.Reactive.TimeInterval<TSource>>();
                    _hasCompleted = false;
                    _completeAt = default(TimeSpan);
                    _hasFailed = false;
                    _exception = default(Exception);

                    _watch = _scheduler.StartStopwatch();

                    RunCore(parent);

                    var sourceSubscription = new SingleAssignmentDisposable();
                    _sourceSubscription = sourceSubscription;
                    sourceSubscription.Disposable = parent._source.SubscribeSafe(this);

                    return StableCompositeDisposable.Create(_sourceSubscription, _cancelable);
                }

                protected abstract void RunCore(TParent parent);

                protected void ScheduleDrain()
                {
                    _stop = new CancellationTokenSource();
                    _cancelable.Disposable = Disposable.Create(_stop.Cancel);

                    _scheduler.AsLongRunning().ScheduleLongRunning(DrainQueue);
                }

                public void OnNext(TSource value)
                {
                    var next = _watch.Elapsed.Add(_delay);

                    lock (_gate)
                    {
                        _queue.Enqueue(new System.Reactive.TimeInterval<TSource>(value, next));

                        _evt.Release();
                    }
                }

                public void OnError(Exception error)
                {
                    _sourceSubscription.Dispose();

                    lock (_gate)
                    {
                        _queue.Clear();

                        _exception = error;
                        _hasFailed = true;

                        _evt.Release();
                    }
                }

                public void OnCompleted()
                {
                    _sourceSubscription.Dispose();

                    var next = _watch.Elapsed.Add(_delay);

                    lock (_gate)
                    {
                        _completeAt = next;
                        _hasCompleted = true;

                        _evt.Release();
                    }
                }

                private void DrainQueue(ICancelable cancel)
                {
                    while (true)
                    {
                        try
                        {
                            _evt.Wait(_stop.Token);
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
                                var now = _watch.Elapsed;

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
                            _scheduler.Schedule(waitTime, () => { timer.Set(); });

                            try
                            {
                                timer.Wait(_stop.Token);
                            }
                            catch (OperationCanceledException)
                            {
                                return;
                            }
                        }

                        if (hasValue)
                        {
                            base._observer.OnNext(value);
                        }
                        else
                        {
                            if (hasCompleted)
                            {
                                base._observer.OnCompleted();
                                base.Dispose();
                            }
                            else if (hasFailed)
                            {
                                base._observer.OnError(error);
                                base.Dispose();
                            }

                            return;
                        }
                    }
                }
            }
        }

        internal sealed class Absolute : Base
        {
            private readonly DateTimeOffset _dueTime;

            public Absolute(IObservable<TSource> source, DateTimeOffset dueTime, IScheduler scheduler)
                : base(source, scheduler)
            {
                _dueTime = dueTime;
            }

            protected override IDisposable Run(IObserver<TSource> observer, IDisposable cancel, Action<IDisposable> setSink)
            {
                if (_scheduler.AsLongRunning() != null)
                {
                    var sink = new L(this, observer, cancel);
                    setSink(sink);
                    return sink.Run(this);
                }
                else
                {
                    var sink = new _(this, observer, cancel);
                    setSink(sink);
                    return sink.Run(this);
                }
            }

            private sealed class _ : _<Absolute>
            {
                public _(Absolute parent, IObserver<TSource> observer, IDisposable cancel)
                    : base(parent, observer, cancel)
                {
                }

                protected override void RunCore(Absolute parent)
                {
                    _ready = false;

                    _cancelable.Disposable = parent._scheduler.Schedule(parent._dueTime, Start);
                }

                private void Start()
                {
                    var next = default(TimeSpan);
                    var shouldRun = false;

                    lock (_gate)
                    {
                        _delay = _watch.Elapsed;

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
                        _cancelable.Disposable = _scheduler.Schedule(next, DrainQueue);
                    }
                }
            }

            private sealed class L : L<Absolute>
            {
                public L(Absolute parent, IObserver<TSource> observer, IDisposable cancel)
                    : base(parent, observer, cancel)
                {
                }

                protected override void RunCore(Absolute parent)
                {
                    _cancelable.Disposable = parent._scheduler.Schedule(parent._dueTime, Start);
                }

                private void Start()
                {
                    lock (_gate)
                    {
                        _delay = _watch.Elapsed;

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

        internal sealed class Relative : Base
        {
            private readonly TimeSpan _dueTime;

            public Relative(IObservable<TSource> source, TimeSpan dueTime, IScheduler scheduler)
                : base(source, scheduler)
            {
                _dueTime = dueTime;
            }

            protected override IDisposable Run(IObserver<TSource> observer, IDisposable cancel, Action<IDisposable> setSink)
            {
                if (_scheduler.AsLongRunning() != null)
                {
                    var sink = new L(this, observer, cancel);
                    setSink(sink);
                    return sink.Run(this);
                }
                else
                {
                    var sink = new _(this, observer, cancel);
                    setSink(sink);
                    return sink.Run(this);
                }
            }

            private sealed class _ : _<Relative>
            {
                public _(Relative parent, IObserver<TSource> observer, IDisposable cancel)
                    : base(parent, observer, cancel)
                {
                }

                protected override void RunCore(Relative parent)
                {
                    _ready = true;

                    _delay = Scheduler.Normalize(parent._dueTime);
                }
            }

            private sealed class L : L<Relative>
            {
                public L(Relative parent, IObserver<TSource> observer, IDisposable cancel)
                    : base(parent, observer, cancel)
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
        internal class Selector : Producer<TSource>
        {
            protected readonly IObservable<TSource> _source;
            private readonly Func<TSource, IObservable<TDelay>> _delaySelector;

            public Selector(IObservable<TSource> source, Func<TSource, IObservable<TDelay>> delaySelector)
            {
                _source = source;
                _delaySelector = delaySelector;
            }

            protected override IDisposable Run(IObserver<TSource> observer, IDisposable cancel, Action<IDisposable> setSink)
            {
                var sink = new _<Selector>(this, observer, cancel);
                setSink(sink);
                return sink.Run(this);
            }

            protected class _<TParent> : Sink<TSource>, IObserver<TSource>
                where TParent : Selector
            {
                private readonly CompositeDisposable _delays = new CompositeDisposable();
                private object _gate = new object();

                private readonly Func<TSource, IObservable<TDelay>> _delaySelector;

                public _(Selector parent, IObserver<TSource> observer, IDisposable cancel)
                    : base(observer, cancel)
                {
                    _delaySelector = parent._delaySelector;
                }

                private bool _atEnd;
                private IDisposable _subscription;

                public IDisposable Run(TParent parent)
                {
                    _atEnd = false;

                    _subscription = RunCore(parent);

                    return StableCompositeDisposable.Create(_subscription, _delays);
                }

                protected virtual IDisposable RunCore(TParent parent)
                {
                    return parent._source.SubscribeSafe(this);
                }

                public void OnNext(TSource value)
                {
                    var delay = default(IObservable<TDelay>);
                    try
                    {
                        delay = _delaySelector(value);
                    }
                    catch (Exception error)
                    {
                        lock (_gate)
                        {
                            base._observer.OnError(error);
                            base.Dispose();
                        }

                        return;
                    }

                    var d = new SingleAssignmentDisposable();
                    _delays.Add(d);
                    d.Disposable = delay.SubscribeSafe(new DelayObserver(this, value, d));
                }

                public void OnError(Exception error)
                {
                    lock (_gate)
                    {
                        base._observer.OnError(error);
                        base.Dispose();
                    }
                }

                public void OnCompleted()
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
                        base._observer.OnCompleted();
                        base.Dispose();
                    }
                }

                private sealed class DelayObserver : IObserver<TDelay>
                {
                    private readonly _<TParent> _parent;
                    private readonly TSource _value;
                    private readonly IDisposable _self;

                    public DelayObserver(_<TParent> parent, TSource value, IDisposable self)
                    {
                        _parent = parent;
                        _value = value;
                        _self = self;
                    }

                    public void OnNext(TDelay value)
                    {
                        lock (_parent._gate)
                        {
                            _parent._observer.OnNext(_value);

                            _parent._delays.Remove(_self);
                            _parent.CheckDone();
                        }
                    }

                    public void OnError(Exception error)
                    {
                        lock (_parent._gate)
                        {
                            _parent._observer.OnError(error);
                            _parent.Dispose();
                        }
                    }

                    public void OnCompleted()
                    {
                        lock (_parent._gate)
                        {
                            _parent._observer.OnNext(_value);

                            _parent._delays.Remove(_self);
                            _parent.CheckDone();
                        }
                    }
                }
            }
        }

        internal sealed class SelectorWithSubscriptionDelay : Selector
        {
            private readonly IObservable<TDelay> _subscriptionDelay;

            public SelectorWithSubscriptionDelay(IObservable<TSource> source, IObservable<TDelay> subscriptionDelay, Func<TSource, IObservable<TDelay>> delaySelector)
                : base(source, delaySelector)
            {
                _subscriptionDelay = subscriptionDelay;
            }

            protected override IDisposable Run(IObserver<TSource> observer, IDisposable cancel, Action<IDisposable> setSink)
            {
                var sink = new _(this, observer, cancel);
                setSink(sink);
                return sink.Run(this);
            }

            private sealed class _ : _<SelectorWithSubscriptionDelay>
            {
                public _(SelectorWithSubscriptionDelay parent, IObserver<TSource> observer, IDisposable cancel)
                    : base(parent, observer, cancel)
                {
                }

                protected override IDisposable RunCore(SelectorWithSubscriptionDelay parent)
                {
                    var subscription = new SerialDisposable();

                    subscription.Disposable = parent._subscriptionDelay.SubscribeSafe(new SubscriptionDelayObserver(this, parent._source, subscription));

                    return subscription;
                }

                private sealed class SubscriptionDelayObserver : IObserver<TDelay>
                {
                    private readonly _ _parent;
                    private readonly IObservable<TSource> _source;
                    private readonly SerialDisposable _subscription;

                    public SubscriptionDelayObserver(_ parent, IObservable<TSource> source, SerialDisposable subscription)
                    {
                        _parent = parent;
                        _source = source;
                        _subscription = subscription;
                    }

                    public void OnNext(TDelay value)
                    {
                        _subscription.Disposable = _source.SubscribeSafe(_parent);
                    }

                    public void OnError(Exception error)
                    {
                        _parent._observer.OnError(error);
                        _parent.Dispose();
                    }

                    public void OnCompleted()
                    {
                        _subscription.Disposable = _source.SubscribeSafe(_parent);
                    }
                }
            }
        }
    }
}

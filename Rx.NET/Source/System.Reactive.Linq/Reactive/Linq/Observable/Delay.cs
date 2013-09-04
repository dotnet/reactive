// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_PERF
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Threading;

#if NO_SEMAPHORE
using System.Reactive.Threading;
#endif

namespace System.Reactive.Linq.Observαble
{
    class Delay<TSource> : Producer<TSource>
    {
        private readonly IObservable<TSource> _source;
        private readonly TimeSpan? _dueTimeR;
        private readonly DateTimeOffset? _dueTimeA;
        private readonly IScheduler _scheduler;

        public Delay(IObservable<TSource> source, TimeSpan dueTime, IScheduler scheduler)
        {
            _source = source;
            _dueTimeR = dueTime;
            _scheduler = scheduler;
        }

        public Delay(IObservable<TSource> source, DateTimeOffset dueTime, IScheduler scheduler)
        {
            _source = source;
            _dueTimeA = dueTime;
            _scheduler = scheduler;
        }

        protected override IDisposable Run(IObserver<TSource> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            if (_scheduler.AsLongRunning() != null)
            {
                var sink = new λ(this, observer, cancel);
                setSink(sink);
                return sink.Run();
            }
            else
            {
                var sink = new _(this, observer, cancel);
                setSink(sink);
                return sink.Run();
            }
        }

        class _ : Sink<TSource>, IObserver<TSource>
        {
            private readonly Delay<TSource> _parent;

            public _(Delay<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
            }

            private IScheduler _scheduler;
            private IDisposable _sourceSubscription;
            private SerialDisposable _cancelable;
            private TimeSpan _delay;
            private IStopwatch _watch;

            private object _gate;
            private bool _ready;
            private bool _active;
            private bool _running;
            private Queue<System.Reactive.TimeInterval<TSource>> _queue;
            private bool _hasCompleted;
            private TimeSpan _completeAt;
            private bool _hasFailed;
            private Exception _exception;

            public IDisposable Run()
            {
                _scheduler = _parent._scheduler;

                _cancelable = new SerialDisposable();

                _gate = new object();
                _active = false;
                _running = false;
                _queue = new Queue<System.Reactive.TimeInterval<TSource>>();
                _hasCompleted = false;
                _completeAt = default(TimeSpan);
                _hasFailed = false;
                _exception = default(Exception);

                _watch = _scheduler.StartStopwatch();

                if (_parent._dueTimeA.HasValue)
                {
                    _ready = false;

                    var dueTimeA = _parent._dueTimeA.Value;
                    _cancelable.Disposable = _scheduler.Schedule(dueTimeA, Start);
                }
                else
                {
                    _ready = true;

                    var dueTimeR = _parent._dueTimeR.Value;
                    _delay = Scheduler.Normalize(dueTimeR);
                }

                var sourceSubscription = new SingleAssignmentDisposable();
                _sourceSubscription = sourceSubscription;
                sourceSubscription.Disposable = _parent._source.SubscribeSafe(this);

                return new CompositeDisposable(_sourceSubscription, _cancelable);
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

            private void DrainQueue(Action<TimeSpan> recurse)
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
                // interface and perform different processing (see λ). To reduce the code churn in the old
                // loop code here, we set the shouldYield flag to true after the first dispatch iteration,
                // in order to break from the loop and enter the recursive scheduling path.
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

        class λ : Sink<TSource>, IObserver<TSource>
        {
            private readonly Delay<TSource> _parent;

            public λ(Delay<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
            }

            private IDisposable _sourceSubscription;
            private SerialDisposable _cancelable;
            private TimeSpan _delay;
            private IStopwatch _watch;

            private object _gate;
#if !NO_CDS
            private SemaphoreSlim _evt;
            private CancellationTokenSource _stop;
#else
            private Semaphore _evt;
            private bool _stopped;
            private ManualResetEvent _stop;
#endif
            private Queue<System.Reactive.TimeInterval<TSource>> _queue;
            private bool _hasCompleted;
            private TimeSpan _completeAt;
            private bool _hasFailed;
            private Exception _exception;

            public IDisposable Run()
            {
                _cancelable = new SerialDisposable();

                _gate = new object();
#if !NO_CDS
                _evt = new SemaphoreSlim(0);
#else
                _evt = new Semaphore(0, int.MaxValue);
#endif
                _queue = new Queue<System.Reactive.TimeInterval<TSource>>();
                _hasCompleted = false;
                _completeAt = default(TimeSpan);
                _hasFailed = false;
                _exception = default(Exception);

                _watch = _parent._scheduler.StartStopwatch();

                if (_parent._dueTimeA.HasValue)
                {
                    var dueTimeA = _parent._dueTimeA.Value;
                    _cancelable.Disposable = _parent._scheduler.Schedule(dueTimeA, Start);
                }
                else
                {
                    var dueTimeR = _parent._dueTimeR.Value;
                    _delay = Scheduler.Normalize(dueTimeR);
                    ScheduleDrain();
                }

                var sourceSubscription = new SingleAssignmentDisposable();
                _sourceSubscription = sourceSubscription;
                sourceSubscription.Disposable = _parent._source.SubscribeSafe(this);

                return new CompositeDisposable(_sourceSubscription, _cancelable);
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

            private void ScheduleDrain()
            {
#if !NO_CDS
                _stop = new CancellationTokenSource();
                _cancelable.Disposable = Disposable.Create(() => _stop.Cancel());
#else
                _stop = new ManualResetEvent(false);
                _cancelable.Disposable = Disposable.Create(() =>
                {
                    _stopped = true;
                    _stop.Set();
                    _evt.Release();
                });
#endif

                _parent._scheduler.AsLongRunning().ScheduleLongRunning(DrainQueue);
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
#if !NO_CDS
                    try
                    {
                        _evt.Wait(_stop.Token);
                    }
                    catch (OperationCanceledException)
                    {
                        return;
                    }
#else
                    _evt.WaitOne();
                    if (_stopped)
                        return;
#endif

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
#if !NO_CDS
                        var timer = new ManualResetEventSlim();
                        _parent._scheduler.Schedule(waitTime, () => { timer.Set(); });

                        try
                        {
                            timer.Wait(_stop.Token);
                        }
                        catch (OperationCanceledException)
                        {
                            return;
                        }
#else
                        var timer = new ManualResetEvent(false);
                        _parent._scheduler.Schedule(waitTime, () => { timer.Set(); });
                        if (WaitHandle.WaitAny(new[] { timer, _stop }) == 1)
                            return;
#endif
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

    class Delay<TSource, TDelay> : Producer<TSource>
    {
        private readonly IObservable<TSource> _source;
        private readonly IObservable<TDelay> _subscriptionDelay;
        private readonly Func<TSource, IObservable<TDelay>> _delaySelector;

        public Delay(IObservable<TSource> source, IObservable<TDelay> subscriptionDelay, Func<TSource, IObservable<TDelay>> delaySelector)
        {
            _source = source;
            _subscriptionDelay = subscriptionDelay;
            _delaySelector = delaySelector;
        }

        protected override IDisposable Run(IObserver<TSource> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            var sink = new _(this, observer, cancel);
            setSink(sink);
            return sink.Run();
        }

        class _ : Sink<TSource>, IObserver<TSource>
        {
            private readonly Delay<TSource, TDelay> _parent;

            public _(Delay<TSource, TDelay> parent, IObserver<TSource> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
            }

            private CompositeDisposable _delays;
            private object _gate;
            private bool _atEnd;
            private SerialDisposable _subscription;

            public IDisposable Run()
            {
                _delays = new CompositeDisposable();
                _gate = new object();
                _atEnd = false;
                _subscription = new SerialDisposable();

                if (_parent._subscriptionDelay == null)
                {
                    Start();
                }
                else
                {
                    _subscription.Disposable = _parent._subscriptionDelay.SubscribeSafe(new σ(this));
                }

                return new CompositeDisposable(_subscription, _delays);
            }

            private void Start()
            {
                _subscription.Disposable = _parent._source.SubscribeSafe(this);
            }

            public void OnNext(TSource value)
            {
                var delay = default(IObservable<TDelay>);
                try
                {
                    delay = _parent._delaySelector(value);
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
                d.Disposable = delay.SubscribeSafe(new δ(this, value, d));
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

            class σ : IObserver<TDelay>
            {
                private readonly _ _parent;

                public σ(_ parent)
                {
                    _parent = parent;
                }

                public void OnNext(TDelay value)
                {
                    _parent.Start();
                }

                public void OnError(Exception error)
                {
                    _parent._observer.OnError(error);
                    _parent.Dispose();
                }

                public void OnCompleted()
                {
                    _parent.Start();
                }
            }

            class δ : IObserver<TDelay>
            {
                private readonly _ _parent;
                private readonly TSource _value;
                private readonly IDisposable _self;

                public δ(_ parent, TSource value, IDisposable self)
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
}
#endif
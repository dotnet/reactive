// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Threading;

namespace System.Reactive.Subjects
{
    /// <summary>
    /// Represents an object that is both an observable sequence as well as an observer.
    /// Each notification is broadcasted to all subscribed and future observers, subject to buffer trimming policies.
    /// </summary>
    /// <typeparam name="T">The type of the elements processed by the subject.</typeparam>
    public sealed class ReplaySubject<T> : ISubject<T>, IDisposable
    {
        private readonly IReplaySubjectImplementation _implementation;

        /// <summary>
        /// Initializes a new instance of the <see cref="System.Reactive.Subjects.ReplaySubject&lt;T&gt;" /> class with the specified buffer size, window and scheduler.
        /// </summary>
        /// <param name="bufferSize">Maximum element count of the replay buffer.</param>
        /// <param name="window">Maximum time length of the replay buffer.</param>
        /// <param name="scheduler">Scheduler the observers are invoked on.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="bufferSize"/> is less than zero. -or- <paramref name="window"/> is less than TimeSpan.Zero.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="scheduler"/> is null.</exception>
        public ReplaySubject(int bufferSize, TimeSpan window, IScheduler scheduler)
        {
            _implementation = new ReplayByTime(bufferSize, window, scheduler);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="System.Reactive.Subjects.ReplaySubject&lt;T&gt;" /> class with the specified buffer size and window.
        /// </summary>
        /// <param name="bufferSize">Maximum element count of the replay buffer.</param>
        /// <param name="window">Maximum time length of the replay buffer.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="bufferSize"/> is less than zero. -or- <paramref name="window"/> is less than TimeSpan.Zero.</exception>
        public ReplaySubject(int bufferSize, TimeSpan window)
        {
            _implementation = new ReplayByTime(bufferSize, window);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="System.Reactive.Subjects.ReplaySubject&lt;T&gt;" /> class.
        /// </summary>
        public ReplaySubject()
        {
            _implementation = new ReplayAll();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="System.Reactive.Subjects.ReplaySubject&lt;T&gt;" /> class with the specified scheduler.
        /// </summary>
        /// <param name="scheduler">Scheduler the observers are invoked on.</param>
        /// <exception cref="ArgumentNullException"><paramref name="scheduler"/> is null.</exception>
        public ReplaySubject(IScheduler scheduler)
        {
            _implementation = new ReplayByTime(scheduler);
        }

        //TODO: Does this overload make any sense with the optimisations? Surely this now is just <c>new ReplaySubject<T>(bufferSize).SubscribeOn(scheduler)</c>?
        //Potentially should be marked as obsolete
        /// <summary>
        /// Initializes a new instance of the <see cref="System.Reactive.Subjects.ReplaySubject&lt;T&gt;" /> class with the specified buffer size and scheduler.
        /// </summary>
        /// <param name="bufferSize">Maximum element count of the replay buffer.</param>
        /// <param name="scheduler">Scheduler the observers are invoked on.</param>
        /// <exception cref="ArgumentNullException"><paramref name="scheduler"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="bufferSize"/> is less than zero.</exception>
        public ReplaySubject(int bufferSize, IScheduler scheduler)
        {
            _implementation = new ReplayByTime(bufferSize, scheduler);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="System.Reactive.Subjects.ReplaySubject&lt;T&gt;" /> class with the specified buffer size.
        /// </summary>
        /// <param name="bufferSize">Maximum element count of the replay buffer.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="bufferSize"/> is less than zero.</exception>
        public ReplaySubject(int bufferSize)
        {
            switch (bufferSize)
            {
                case 1:
                    _implementation = new ReplayOne();
                    break;
                case int.MaxValue:
                    _implementation = new ReplayAll();
                    break;
                default:
                    _implementation = new ReplayMany(bufferSize);
                    break;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="System.Reactive.Subjects.ReplaySubject&lt;T&gt;" /> class with the specified window and scheduler.
        /// </summary>
        /// <param name="window">Maximum time length of the replay buffer.</param>
        /// <param name="scheduler">Scheduler the observers are invoked on.</param>
        /// <exception cref="ArgumentNullException"><paramref name="scheduler"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="window"/> is less than TimeSpan.Zero.</exception>
        public ReplaySubject(TimeSpan window, IScheduler scheduler)
        {
            _implementation = new ReplayByTime(window, scheduler);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="System.Reactive.Subjects.ReplaySubject&lt;T&gt;" /> class with the specified window.
        /// </summary>
        /// <param name="window">Maximum time length of the replay buffer.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="window"/> is less than TimeSpan.Zero.</exception>
        public ReplaySubject(TimeSpan window)
        {
            _implementation = new ReplayByTime(window);
        }

        /// <summary>
        /// Indicates whether the subject has observers subscribed to it.
        /// </summary>
        public bool HasObservers
        {
            get { return _implementation.HasObservers; }
        }

        /// <summary>
        /// Notifies all subscribed and future observers about the arrival of the specified element in the sequence.
        /// </summary>
        /// <param name="value">The value to send to all observers.</param>
        public void OnNext(T value)
        {
            _implementation.OnNext(value);
        }

        /// <summary>
        /// Notifies all subscribed and future observers about the specified exception.
        /// </summary>
        /// <param name="error">The exception to send to all observers.</param>
        /// <exception cref="ArgumentNullException"><paramref name="error"/> is null.</exception>
        public void OnError(Exception error)
        {
            _implementation.OnError(error);
        }

        /// <summary>
        /// Notifies all subscribed and future observers about the end of the sequence.
        /// </summary>
        public void OnCompleted()
        {
            _implementation.OnCompleted();
        }

        /// <summary>
        /// Subscribes an observer to the subject.
        /// </summary>
        /// <param name="observer">Observer to subscribe to the subject.</param>
        /// <returns>Disposable object that can be used to unsubscribe the observer from the subject.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="observer"/> is null.</exception>
        public IDisposable Subscribe(IObserver<T> observer)
        {
            return _implementation.Subscribe(observer);
        }

        /// <summary>
        /// Releases all resources used by the current instance of the <see cref="System.Reactive.Subjects.ReplaySubject&lt;T&gt;"/> class and unsubscribe all observers.
        /// </summary>
        public void Dispose()
        {
            _implementation.Dispose();
        }

        private interface IReplaySubjectImplementation : ISubject<T>, IDisposable
        {
            bool HasObservers { get; }
            void Unsubscribe(IObserver<T> observer);
        }

        private class Subscription : IDisposable
        {
            private IReplaySubjectImplementation _subject;
            private IObserver<T> _observer;

            public Subscription(IReplaySubjectImplementation subject, IObserver<T> observer)
            {
                _subject = subject;
                _observer = observer;
            }

            public void Dispose()
            {
                var observer = Interlocked.Exchange(ref _observer, null);
                if (observer == null)
                    return;

                _subject.Unsubscribe(observer);
                _subject = null;
            }
        }

        //Original implementation of the ReplaySubject with time based operations (Scheduling, Stopwatch, buffer-by-time).
        private sealed class ReplayByTime : IReplaySubjectImplementation
        {
            private const int InfiniteBufferSize = int.MaxValue;

            private readonly int _bufferSize;
            private readonly TimeSpan _window;
            private readonly IScheduler _scheduler;
            private readonly IStopwatch _stopwatch;

            private readonly Queue<TimeInterval<T>> _queue;
            private bool _isStopped;
            private Exception _error;

            private ImmutableList<ScheduledObserver<T>> _observers;
            private bool _isDisposed;

            private readonly object _gate = new object();

            public ReplayByTime(int bufferSize, TimeSpan window, IScheduler scheduler)
            {
                if (bufferSize < 0)
                    throw new ArgumentOutOfRangeException("bufferSize");
                if (window < TimeSpan.Zero)
                    throw new ArgumentOutOfRangeException("window");
                if (scheduler == null)
                    throw new ArgumentNullException("scheduler");

                _bufferSize = bufferSize;
                _window = window;
                _scheduler = scheduler;

                _stopwatch = _scheduler.StartStopwatch();
                _queue = new Queue<TimeInterval<T>>();
                _isStopped = false;
                _error = null;

                _observers = new ImmutableList<ScheduledObserver<T>>();
            }

            public ReplayByTime(int bufferSize, TimeSpan window)
                : this(bufferSize, window, SchedulerDefaults.Iteration)
            {
            }

            public ReplayByTime(IScheduler scheduler)
                : this(InfiniteBufferSize, TimeSpan.MaxValue, scheduler)
            {
            }

            public ReplayByTime(int bufferSize, IScheduler scheduler)
                : this(bufferSize, TimeSpan.MaxValue, scheduler)
            {
            }

            public ReplayByTime(TimeSpan window, IScheduler scheduler)
                : this(InfiniteBufferSize, window, scheduler)
            {
            }

            public ReplayByTime(TimeSpan window)
                : this(InfiniteBufferSize, window, SchedulerDefaults.Iteration)
            {
            }

            public bool HasObservers
            {
                get
                {
                    var observers = _observers;
                    return observers != null && observers.Data.Length > 0;
                }
            }

            private void Trim(TimeSpan now)
            {
                while (_queue.Count > _bufferSize)
                    _queue.Dequeue();
                while (_queue.Count > 0 && now.Subtract(_queue.Peek().Interval).CompareTo(_window) > 0)
                    _queue.Dequeue();
            }

            public void OnNext(T value)
            {
                var o = default(ScheduledObserver<T>[]);
                lock (_gate)
                {
                    CheckDisposed();

                    if (!_isStopped)
                    {
                        var now = _stopwatch.Elapsed;
                        _queue.Enqueue(new TimeInterval<T>(value, now));
                        Trim(now);

                        o = _observers.Data;
                        foreach (var observer in o)
                            observer.OnNext(value);
                    }
                }

                if (o != null)
                    foreach (var observer in o)
                        observer.EnsureActive();
            }

            public void OnError(Exception error)
            {
                if (error == null)
                    throw new ArgumentNullException("error");

                var o = default(ScheduledObserver<T>[]);
                lock (_gate)
                {
                    CheckDisposed();

                    if (!_isStopped)
                    {
                        var now = _stopwatch.Elapsed;
                        _isStopped = true;
                        _error = error;
                        Trim(now);

                        o = _observers.Data;
                        foreach (var observer in o)
                            observer.OnError(error);

                        _observers = new ImmutableList<ScheduledObserver<T>>();
                    }
                }

                if (o != null)
                    foreach (var observer in o)
                        observer.EnsureActive();
            }

            public void OnCompleted()
            {
                var o = default(ScheduledObserver<T>[]);
                lock (_gate)
                {
                    CheckDisposed();

                    if (!_isStopped)
                    {
                        var now = _stopwatch.Elapsed;
                        _isStopped = true;
                        Trim(now);

                        o = _observers.Data;
                        foreach (var observer in o)
                            observer.OnCompleted();

                        _observers = new ImmutableList<ScheduledObserver<T>>();
                    }
                }

                if (o != null)
                    foreach (var observer in o)
                        observer.EnsureActive();
            }

            public IDisposable Subscribe(IObserver<T> observer)
            {
                if (observer == null)
                    throw new ArgumentNullException("observer");

                var so = new ScheduledObserver<T>(_scheduler, observer);

                var n = 0;

                var subscription = new RemovableDisposable(this, so);
                lock (_gate)
                {
                    CheckDisposed();

                    //
                    // Notice the v1.x behavior of always calling Trim is preserved here.
                    //
                    // This may be subject (pun intended) of debate: should this policy
                    // only be applied while the sequence is active? With the current
                    // behavior, a sequence will "die out" after it has terminated by
                    // continuing to drop OnNext notifications from the queue.
                    //
                    // In v1.x, this behavior was due to trimming based on the clock value
                    // returned by scheduler.Now, applied to all but the terminal message
                    // in the queue. Using the IStopwatch has the same effect. Either way,
                    // we guarantee the final notification will be observed, but there's
                    // no way to retain the buffer directly. One approach is to use the
                    // time-based TakeLast operator and apply an unbounded ReplaySubject
                    // to it.
                    //
                    // To conclude, we're keeping the behavior as-is for compatibility
                    // reasons with v1.x.
                    //
                    Trim(_stopwatch.Elapsed);
                    _observers = _observers.Add(so);

                    n = _queue.Count;
                    foreach (var item in _queue)
                        so.OnNext(item.Value);

                    if (_error != null)
                    {
                        n++;
                        so.OnError(_error);
                    }
                    else if (_isStopped)
                    {
                        n++;
                        so.OnCompleted();
                    }
                }

                so.EnsureActive(n);

                return subscription;
            }

            private void Unsubscribe(ScheduledObserver<T> observer)
            {
                lock (_gate)
                {
                    observer.Dispose();
                    if (!_isDisposed)
                        _observers = _observers.Remove(observer);
                }
            }

            void IReplaySubjectImplementation.Unsubscribe(IObserver<T> observer)
            {
                var so = (ScheduledObserver<T>)observer;
                Unsubscribe(so);
            }

            sealed class RemovableDisposable : IDisposable
            {
                private readonly ReplayByTime _subject;
                private readonly ScheduledObserver<T> _observer;

                public RemovableDisposable(ReplayByTime subject, ScheduledObserver<T> observer)
                {
                    _subject = subject;
                    _observer = observer;
                }

                public void Dispose()
                {
                    _observer.Dispose();
                    _subject.Unsubscribe(_observer);
                }
            }

            private void CheckDisposed()
            {
                if (_isDisposed)
                    throw new ObjectDisposedException(string.Empty);
            }

            public void Dispose()
            {
                lock (_gate)
                {
                    _isDisposed = true;
                    _observers = null;
                    _queue.Clear();
                }
            }
        }













        //Below are the non-time based implementations. 
        //These removed the need for the scheduler indirection, SchedulerObservers, stopwatch, TimeInterval and ensuring the scheduled observers are active after each action.
        //The ReplayOne implementation also removes the need to even have a queue.
        private sealed class ReplayOne : ReplayBufferBase, IReplaySubjectImplementation
        {
            private bool _hasValue;
            private T _value;

            protected override void Trim()
            {
                //NoOp. No need to trim.
            }

            protected override void AddValueToBuffer(T value)
            {
                _hasValue = true;
                _value = value;
            }

            protected override void ReplayBuffer(IObserver<T> observer)
            {
                if (_hasValue)
                    observer.OnNext(_value);
            }

            protected override void Dispose(bool disposing)
            {
                base.Dispose(disposing);
                _value = default(T);
            }
        }

        private sealed class ReplayMany : ReplayManyBase, IReplaySubjectImplementation
        {
            private readonly int _bufferSize;

            public ReplayMany(int bufferSize)
                : base(bufferSize)
            {
                _bufferSize = bufferSize;
            }

            protected override void Trim()
            {
                while (Queue.Count > _bufferSize)
                    Queue.Dequeue();
            }
        }

        private sealed class ReplayAll : ReplayManyBase, IReplaySubjectImplementation
        {
            public ReplayAll()
                : base(0)
            {
            }

            protected override void Trim()
            {
                //NoOp; i.e. Dont' trim, keep all values.
            }
        }

        private abstract class ReplayBufferBase : IReplaySubjectImplementation
        {
            private readonly object _gate = new object();
            private bool _isDisposed;
            private bool _isStopped;
            private Exception _error;
            private ImmutableList<IObserver<T>> _observers;

            protected ReplayBufferBase()
            {
                _observers = new ImmutableList<IObserver<T>>();
            }

            protected abstract void Trim();
            protected abstract void AddValueToBuffer(T value);
            protected abstract void ReplayBuffer(IObserver<T> observer);

            public bool HasObservers
            {
                get
                {
                    var observers = _observers;
                    return observers != null && observers.Data.Length > 0;
                }
            }

            public void OnNext(T value)
            {
                lock (_gate)
                {
                    CheckDisposed();

                    if (!_isStopped)
                    {
                        AddValueToBuffer(value);
                        Trim();

                        var o = _observers.Data;
                        foreach (var observer in o)
                            observer.OnNext(value);
                    }
                }
            }

            public void OnError(Exception error)
            {
                if (error == null)
                    throw new ArgumentNullException("error");

                lock (_gate)
                {
                    CheckDisposed();

                    if (!_isStopped)
                    {
                        _isStopped = true;
                        _error = error;
                        Trim();

                        var o = _observers.Data;
                        foreach (var observer in o)
                            observer.OnError(error);

                        _observers = new ImmutableList<IObserver<T>>();
                    }
                }
            }

            public void OnCompleted()
            {
                lock (_gate)
                {
                    CheckDisposed();

                    if (!_isStopped)
                    {
                        _isStopped = true;
                        Trim();

                        var o = _observers.Data;
                        foreach (var observer in o)
                            observer.OnCompleted();

                        _observers = new ImmutableList<IObserver<T>>();
                    }
                }
            }

            public IDisposable Subscribe(IObserver<T> observer)
            {
                if (observer == null)
                    throw new ArgumentNullException("observer");


                var subscription = new Subscription(this, observer);
                lock (_gate)
                {
                    CheckDisposed();

                    //
                    // Notice the v1.x behavior of always calling Trim is preserved here.
                    //
                    // This may be subject (pun intended) of debate: should this policy
                    // only be applied while the sequence is active? With the current
                    // behavior, a sequence will "die out" after it has terminated by
                    // continuing to drop OnNext notifications from the queue.
                    //
                    // In v1.x, this behavior was due to trimming based on the clock value
                    // returned by scheduler.Now, applied to all but the terminal message
                    // in the queue. Using the IStopwatch has the same effect. Either way,
                    // we guarantee the final notification will be observed, but there's
                    // no way to retain the buffer directly. One approach is to use the
                    // time-based TakeLast operator and apply an unbounded ReplaySubject
                    // to it.
                    //
                    // To conclude, we're keeping the behavior as-is for compatibility
                    // reasons with v1.x.
                    //
                    _observers = _observers.Add(observer);

                    ReplayBuffer(observer);

                    if (_error != null)
                    {
                        observer.OnError(_error);
                    }
                    else if (_isStopped)
                    {
                        observer.OnCompleted();
                    }
                }

                return subscription;
            }

            public void Unsubscribe(IObserver<T> observer)
            {
                lock (_gate)
                {
                    if (!_isDisposed)
                        _observers = _observers.Remove(observer);
                }
            }

            private void CheckDisposed()
            {
                if (_isDisposed)
                    throw new ObjectDisposedException(string.Empty);
            }

            public void Dispose()
            {
                Dispose(true);
            }
            protected virtual void Dispose(bool disposing)
            {
                lock (_gate)
                {
                    _isDisposed = true;
                    _observers = null;
                }
            }
        }

        private abstract class ReplayManyBase : ReplayBufferBase, IReplaySubjectImplementation
        {
            private readonly Queue<T> _queue;

            protected ReplayManyBase(int queueSize)
                : base()
            {
                _queue = new Queue<T>(queueSize);
            }

            protected Queue<T> Queue { get { return _queue; } }

            protected override void AddValueToBuffer(T value)
            {
                _queue.Enqueue(value);
            }

            protected override void ReplayBuffer(IObserver<T> observer)
            {
                foreach (var item in _queue)
                    observer.OnNext(item);
            }

            protected override void Dispose(bool disposing)
            {
                base.Dispose(disposing);
                _queue.Clear();
            }
        }
    }
}
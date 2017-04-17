// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Threading;

namespace System.Reactive.Subjects
{
    /// <summary>
    /// Represents an object that is both an observable sequence as well as an observer.
    /// Each notification is broadcasted to all subscribed and future observers, subject to buffer trimming policies.
    /// </summary>
    /// <typeparam name="T">The type of the elements processed by the subject.</typeparam>
    public sealed class ReplaySubject<T> : SubjectBase<T>, IDisposable
    {
        #region Fields

        /// <summary>
        /// Underlying optimized implementation of the replay subject.
        /// </summary>
        private readonly SubjectBase<T> _implementation;

        #endregion

        #region Constructors

        #region All

        /// <summary>
        /// Initializes a new instance of the <see cref="ReplaySubject{T}" /> class.
        /// </summary>
        public ReplaySubject()
            : this(int.MaxValue)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReplaySubject{T}" /> class with the specified scheduler.
        /// </summary>
        /// <param name="scheduler">Scheduler the observers are invoked on.</param>
        /// <exception cref="ArgumentNullException"><paramref name="scheduler"/> is <c>null</c>.</exception>
        public ReplaySubject(IScheduler scheduler)
        {
            _implementation = new ReplayByTime(scheduler);
        }

        #endregion

        #region Count

        /// <summary>
        /// Initializes a new instance of the <see cref="ReplaySubject{T}" /> class with the specified buffer size.
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
        /// Initializes a new instance of the <see cref="ReplaySubject{T}" /> class with the specified buffer size and scheduler.
        /// </summary>
        /// <param name="bufferSize">Maximum element count of the replay buffer.</param>
        /// <param name="scheduler">Scheduler the observers are invoked on.</param>
        /// <exception cref="ArgumentNullException"><paramref name="scheduler"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="bufferSize"/> is less than zero.</exception>
        public ReplaySubject(int bufferSize, IScheduler scheduler)
        {
            _implementation = new ReplayByTime(bufferSize, scheduler);
        }

        #endregion

        #region Time

        /// <summary>
        /// Initializes a new instance of the <see cref="ReplaySubject{T}" /> class with the specified window.
        /// </summary>
        /// <param name="window">Maximum time length of the replay buffer.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="window"/> is less than <see cref="TimeSpan.Zero"/>.</exception>
        public ReplaySubject(TimeSpan window)
        {
            _implementation = new ReplayByTime(window);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReplaySubject{T}" /> class with the specified window and scheduler.
        /// </summary>
        /// <param name="window">Maximum time length of the replay buffer.</param>
        /// <param name="scheduler">Scheduler the observers are invoked on.</param>
        /// <exception cref="ArgumentNullException"><paramref name="scheduler"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="window"/> is less than <see cref="TimeSpan.Zero"/>.</exception>
        public ReplaySubject(TimeSpan window, IScheduler scheduler)
        {
            _implementation = new ReplayByTime(window, scheduler);
        }

        #endregion

        #region Count & Time

        /// <summary>
        /// Initializes a new instance of the <see cref="ReplaySubject{T}" /> class with the specified buffer size and window.
        /// </summary>
        /// <param name="bufferSize">Maximum element count of the replay buffer.</param>
        /// <param name="window">Maximum time length of the replay buffer.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="bufferSize"/> is less than zero. -or- <paramref name="window"/> is less than <see cref="TimeSpan.Zero"/>.</exception>
        public ReplaySubject(int bufferSize, TimeSpan window)
        {
            _implementation = new ReplayByTime(bufferSize, window);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReplaySubject{T}" /> class with the specified buffer size, window and scheduler.
        /// </summary>
        /// <param name="bufferSize">Maximum element count of the replay buffer.</param>
        /// <param name="window">Maximum time length of the replay buffer.</param>
        /// <param name="scheduler">Scheduler the observers are invoked on.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="bufferSize"/> is less than zero. -or- <paramref name="window"/> is less than <see cref="TimeSpan.Zero"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="scheduler"/> is <c>null</c>.</exception>
        public ReplaySubject(int bufferSize, TimeSpan window, IScheduler scheduler)
        {
            _implementation = new ReplayByTime(bufferSize, window, scheduler);
        }

        #endregion

        #endregion

        #region Properties

        /// <summary>
        /// Indicates whether the subject has observers subscribed to it.
        /// </summary>
        public override bool HasObservers => _implementation.HasObservers;

        /// <summary>
        /// Indicates whether the subject has been disposed.
        /// </summary>
        public override bool IsDisposed => _implementation.IsDisposed;

        #endregion

        #region Methods

        #region IObserver<T> implementation

        /// <summary>
        /// Notifies all subscribed and future observers about the arrival of the specified element in the sequence.
        /// </summary>
        /// <param name="value">The value to send to all observers.</param>
        public override void OnNext(T value) => _implementation.OnNext(value);

        /// <summary>
        /// Notifies all subscribed and future observers about the specified exception.
        /// </summary>
        /// <param name="error">The exception to send to all observers.</param>
        /// <exception cref="ArgumentNullException"><paramref name="error"/> is <c>null</c>.</exception>
        public override void OnError(Exception error)
        {
            if (error == null)
                throw new ArgumentNullException(nameof(error));

            _implementation.OnError(error);
        }

        /// <summary>
        /// Notifies all subscribed and future observers about the end of the sequence.
        /// </summary>
        public override void OnCompleted() => _implementation.OnCompleted();

        #endregion

        #region IObservable<T> implementation

        /// <summary>
        /// Subscribes an observer to the subject.
        /// </summary>
        /// <param name="observer">Observer to subscribe to the subject.</param>
        /// <returns>Disposable object that can be used to unsubscribe the observer from the subject.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="observer"/> is <c>null</c>.</exception>
        public override IDisposable Subscribe(IObserver<T> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            return _implementation.Subscribe(observer);
        }

        #endregion

        #region IDisposable implementation

        /// <summary>
        /// Releases all resources used by the current instance of the <see cref="ReplaySubject{T}"/> class and unsubscribe all observers.
        /// </summary>
        public override void Dispose() => _implementation.Dispose();

        #endregion

        #endregion

        private abstract class ReplayBase : SubjectBase<T>
        {
            private readonly object _gate = new object();

            private ImmutableList<IScheduledObserver<T>> _observers;

            private bool _isStopped;
            private Exception _error;
            private bool _isDisposed;

            public ReplayBase()
            {
                _observers = ImmutableList<IScheduledObserver<T>>.Empty;

                _isStopped = false;
                _error = null;
            }

            public override bool HasObservers => _observers?.Data.Length > 0;

            public override bool IsDisposed
            {
                get
                {
                    lock (_gate)
                    {
                        return _isDisposed;
                    }
                }
            }

            public override void OnNext(T value)
            {
                var o = default(IScheduledObserver<T>[]);
                lock (_gate)
                {
                    CheckDisposed();

                    if (!_isStopped)
                    {
                        Next(value);
                        Trim();

                        o = _observers.Data;
                        foreach (var observer in o)
                        {
                            observer.OnNext(value);
                        }
                    }
                }

                if (o != null)
                {
                    foreach (var observer in o)
                    {
                        observer.EnsureActive();
                    }
                }
            }

            public override void OnError(Exception error)
            {
                var o = default(IScheduledObserver<T>[]);
                lock (_gate)
                {
                    CheckDisposed();

                    if (!_isStopped)
                    {
                        _isStopped = true;
                        _error = error;
                        Trim();

                        o = _observers.Data;
                        foreach (var observer in o)
                        {
                            observer.OnError(error);
                        }

                        _observers = ImmutableList<IScheduledObserver<T>>.Empty;
                    }
                }

                if (o != null)
                {
                    foreach (var observer in o)
                    {
                        observer.EnsureActive();
                    }
                }
            }

            public override void OnCompleted()
            {
                var o = default(IScheduledObserver<T>[]);
                lock (_gate)
                {
                    CheckDisposed();

                    if (!_isStopped)
                    {
                        _isStopped = true;
                        Trim();

                        o = _observers.Data;
                        foreach (var observer in o)
                        {
                            observer.OnCompleted();
                        }

                        _observers = ImmutableList<IScheduledObserver<T>>.Empty;
                    }
                }

                if (o != null)
                {
                    foreach (var observer in o)
                    {
                        observer.EnsureActive();
                    }
                }
            }

            public override IDisposable Subscribe(IObserver<T> observer)
            {
                var so = CreateScheduledObserver(observer);

                var n = 0;

                var subscription = Disposable.Empty;

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
                    Trim();

                    n = Replay(so);

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

                    if (!_isStopped)
                    {
                        subscription = new Subscription(this, so);
                        _observers = _observers.Add(so);
                    }
                }

                so.EnsureActive(n);

                return subscription;
            }

            public override void Dispose()
            {
                lock (_gate)
                {
                    _isDisposed = true;
                    _observers = null;
                    DisposeCore();
                }
            }

            protected abstract void DisposeCore();

            protected abstract void Next(T value);

            protected abstract int Replay(IObserver<T> observer);

            protected abstract void Trim();

            protected abstract IScheduledObserver<T> CreateScheduledObserver(IObserver<T> observer);

            private void CheckDisposed()
            {
                if (_isDisposed)
                    throw new ObjectDisposedException(string.Empty);
            }

            private void Unsubscribe(IScheduledObserver<T> observer)
            {
                lock (_gate)
                {
                    if (!_isDisposed)
                    {
                        _observers = _observers.Remove(observer);
                    }
                }
            }

            private sealed class Subscription : IDisposable
            {
                private readonly ReplayBase _subject;
                private readonly IScheduledObserver<T> _observer;

                public Subscription(ReplayBase subject, IScheduledObserver<T> observer)
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
        }

        /// <summary>
        /// Original implementation of the ReplaySubject with time based operations (Scheduling, Stopwatch, buffer-by-time).
        /// </summary>
        private sealed class ReplayByTime : ReplayBase
        {
            private const int InfiniteBufferSize = int.MaxValue;

            private readonly int _bufferSize;
            private readonly TimeSpan _window;
            private readonly IScheduler _scheduler;
            private readonly IStopwatch _stopwatch;

            private readonly Queue<TimeInterval<T>> _queue;

            public ReplayByTime(int bufferSize, TimeSpan window, IScheduler scheduler)
            {
                if (bufferSize < 0)
                    throw new ArgumentOutOfRangeException(nameof(bufferSize));
                if (window < TimeSpan.Zero)
                    throw new ArgumentOutOfRangeException(nameof(window));
                if (scheduler == null)
                    throw new ArgumentNullException(nameof(scheduler));

                _bufferSize = bufferSize;
                _window = window;
                _scheduler = scheduler;

                _stopwatch = _scheduler.StartStopwatch();
                _queue = new Queue<TimeInterval<T>>();
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

            protected override IScheduledObserver<T> CreateScheduledObserver(IObserver<T> observer)
            {
                return new ScheduledObserver<T>(_scheduler, observer);
            }

            protected override void DisposeCore()
            {
                _queue.Clear();
            }

            protected override void Next(T value)
            {
                var now = _stopwatch.Elapsed;

                _queue.Enqueue(new TimeInterval<T>(value, now));
            }

            protected override int Replay(IObserver<T> observer)
            {
                var n = _queue.Count;

                foreach (var item in _queue)
                {
                    observer.OnNext(item.Value);
                }

                return n;
            }

            protected override void Trim()
            {
                var now = _stopwatch.Elapsed;

                while (_queue.Count > _bufferSize)
                {
                    _queue.Dequeue();
                }

                while (_queue.Count > 0 && now.Subtract(_queue.Peek().Interval).CompareTo(_window) > 0)
                {
                    _queue.Dequeue();
                }
            }
        }

        //
        // Below are the non-time based implementations.
        // These removed the need for the scheduler indirection, SchedulerObservers, stopwatch, TimeInterval and ensuring the scheduled observers are active after each action.
        // The ReplayOne implementation also removes the need to even have a queue.
        //

        private sealed class ReplayOne : ReplayBufferBase
        {
            private bool _hasValue;
            private T _value;

            protected override void Trim()
            {
                //
                // No need to trim.
                //
            }

            protected override void Next(T value)
            {
                _hasValue = true;
                _value = value;
            }

            protected override int Replay(IObserver<T> observer)
            {
                var n = 0;

                if (_hasValue)
                {
                    n = 1;
                    observer.OnNext(_value);
                }

                return n;
            }

            protected override void DisposeCore()
            {
                _value = default(T);
            }
        }

        private sealed class ReplayMany : ReplayManyBase
        {
            private readonly int _bufferSize;

            public ReplayMany(int bufferSize)
                : base(bufferSize)
            {
                _bufferSize = bufferSize;
            }

            protected override void Trim()
            {
                while (_queue.Count > _bufferSize)
                {
                    _queue.Dequeue();
                }
            }
        }

        private sealed class ReplayAll : ReplayManyBase
        {
            public ReplayAll()
                : base(0)
            {
            }

            protected override void Trim()
            {
                //
                // Don't trim, keep all values.
                //
            }
        }

        private abstract class ReplayBufferBase : ReplayBase
        {
            protected override IScheduledObserver<T> CreateScheduledObserver(IObserver<T> observer)
            {
                return new FastImmediateObserver<T>(observer);
            }

            protected override void DisposeCore()
            {
            }
        }

        private abstract class ReplayManyBase : ReplayBufferBase
        {
            protected readonly Queue<T> _queue;

            protected ReplayManyBase(int queueSize)
                : base()
            {
                _queue = new Queue<T>(Math.Min(queueSize, 64));
            }

            protected override void Next(T value)
            {
                _queue.Enqueue(value);
            }

            protected override int Replay(IObserver<T> observer)
            {
                var n = _queue.Count;

                foreach (var item in _queue)
                {
                    observer.OnNext(item);
                }

                return n;
            }

            protected override void DisposeCore()
            {
                _queue.Clear();
            }
        }
    }

    /// <summary>
    /// Specialized scheduled observer similar to a scheduled observer for the immediate scheduler.
    /// </summary>
    /// <typeparam name="T">Type of the elements processed by the observer.</typeparam>
    internal sealed class FastImmediateObserver<T> : IScheduledObserver<T>
    {
        /// <summary>
        /// Gate to control ownership transfer and protect data structures.
        /// </summary>
        private readonly object _gate = new object();

        /// <summary>
        /// Observer to forward notifications to.
        /// </summary>
        private volatile IObserver<T> _observer;

        /// <summary>
        /// Queue to enqueue OnNext notifications into.
        /// </summary>
        private Queue<T> _queue = new Queue<T>();

        /// <summary>
        /// Standby queue to swap out for _queue when transferring ownership. This allows to reuse
        /// queues in case of busy subjects where the initial replay doesn't suffice to catch up.
        /// </summary>
        private Queue<T> _queue2;

        /// <summary>
        /// Exception passed to an OnError notification, if any.
        /// </summary>
        private Exception _error;

        /// <summary>
        /// Indicates whether an OnCompleted notification was received.
        /// </summary>
        private bool _done;

        /// <summary>
        /// Indicates whether the observer is busy, i.e. some thread is actively draining the
        /// notifications that were queued up.
        /// </summary>
        private bool _busy;

        /// <summary>
        /// Indicates whether a failure occurred when the owner was draining the queue. This will
        /// prevent future work to be processed.
        /// </summary>
        private bool _hasFaulted;

        /// <summary>
        /// Creates a new scheduled observer that proxies to the specified observer.
        /// </summary>
        /// <param name="observer">Observer to forward notifications to.</param>
        public FastImmediateObserver(IObserver<T> observer)
        {
            _observer = observer;
        }

        /// <summary>
        /// Disposes the observer.
        /// </summary>
        public void Dispose()
        {
            Done();
        }

        /// <summary>
        /// Notifies the observer of pending work. This will either cause the current owner to
        /// process the newly enqueued notifications, or it will cause the calling thread to
        /// become the owner and start processing the notification queue.
        /// </summary>
        public void EnsureActive()
        {
            EnsureActive(1);
        }

        /// <summary>
        /// Notifies the observer of pending work. This will either cause the current owner to
        /// process the newly enqueued notifications, or it will cause the calling thread to
        /// become the owner and start processing the notification queue.
        /// </summary>
        /// <param name="count">The number of enqueued notifications to process (ignored).</param>
        public void EnsureActive(int count)
        {
            var isOwner = false;

            lock (_gate)
            {
                //
                // If we failed to process work in the past, we'll simply drop it.
                //
                if (!_hasFaulted)
                {
                    //
                    // If no-one is processing the notification queue, become the owner.
                    //
                    if (!_busy)
                    {
                        isOwner = true;
                        _busy = true;
                    }
                }
            }

            if (isOwner)
            {
                while (true)
                {
                    var queue = default(Queue<T>);
                    var error = default(Exception);
                    var done = false;

                    //
                    // Steal notifications from the producer side to drain them to the observer.
                    //
                    lock (_gate)
                    {
                        //
                        // Do we have any OnNext notifications to process?
                        //
                        if (_queue.Count > 0)
                        {
                            if (_queue2 == null)
                            {
                                _queue2 = new Queue<T>();
                            }

                            //
                            // Swap out the current queue for a fresh or recycled one. The standby
                            // queue is set to null; when notifications are sent out the processed
                            // queue will become the new standby.
                            //
                            queue = _queue;
                            _queue = _queue2;
                            _queue2 = null;
                        }

                        //
                        // Do we have any terminal notifications to process?
                        //
                        if (_error != null)
                        {
                            error = _error;
                        }
                        else if (_done)
                        {
                            done = true;
                        }
                        else if (queue == null)
                        {
                            //
                            // No work left; quit the loop and let another thread become the
                            // owner in the future.
                            //
                            _busy = false;
                            break;
                        }
                    }

                    try
                    {
                        //
                        // Process OnNext notifications, if any.
                        //
                        if (queue != null)
                        {
                            //
                            // Drain the stolen OnNext notification queue.
                            //
                            while (queue.Count > 0)
                            {
                                _observer.OnNext(queue.Dequeue());
                            }

                            //
                            // The queue is now empty, so we can reuse it by making it the standby
                            // queue for a future swap.
                            //
                            lock (_gate)
                            {
                                _queue2 = queue;
                            }
                        }

                        //
                        // Process terminal notifications, if any. Notice we don't release ownership
                        // after processing these notifications; we simply quit from the loop. This
                        // will cause all processing of the scheduler observer to cease.
                        //
                        if (error != null)
                        {
                            var observer = Done();
                            observer.OnError(error);
                            break;
                        }
                        else if (done)
                        {
                            var observer = Done();
                            observer.OnCompleted();
                            break;
                        }
                    }
                    catch
                    {
                        lock (_gate)
                        {
                            _hasFaulted = true;
                            _queue.Clear();
                        }

                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Enqueues an OnCompleted notification.
        /// </summary>
        public void OnCompleted()
        {
            lock (_gate)
            {
                if (!_hasFaulted)
                {
                    _done = true;
                }
            }
        }

        /// <summary>
        /// Enqueues an OnError notification.
        /// </summary>
        /// <param name="error">Error of the notification.</param>
        public void OnError(Exception error)
        {
            lock (_gate)
            {
                if (!_hasFaulted)
                {
                    _error = error;
                }
            }
        }

        /// <summary>
        /// Enqueues an OnNext notification.
        /// </summary>
        /// <param name="value">Value of the notification.</param>
        public void OnNext(T value)
        {
            lock (_gate)
            {
                if (!_hasFaulted)
                {
                    _queue.Enqueue(value);
                }
            }
        }

        /// <summary>
        /// Terminates the observer upon receiving terminal notifications, thus preventing
        /// future notifications to go out.
        /// </summary>
        /// <returns>Observer to send terminal notifications to.</returns>
        private IObserver<T> Done()
        {
            return Interlocked.Exchange(ref _observer, NopObserver<T>.Instance);
        }
    }
}

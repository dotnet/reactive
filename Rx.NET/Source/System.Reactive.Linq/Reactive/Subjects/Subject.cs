// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_PERF
using System.Reactive.Disposables;
using System.Threading;

namespace System.Reactive.Subjects
{
    /// <summary>
    /// Represents an object that is both an observable sequence as well as an observer.
    /// Each notification is broadcasted to all subscribed observers.
    /// </summary>
    /// <typeparam name="T">The type of the elements processed by the subject.</typeparam>
    public sealed class Subject<T> : ISubject<T>, IDisposable
    {
        private volatile IObserver<T> _observer;

        /// <summary>
        /// Creates a subject.
        /// </summary>
        public Subject()
        {
            _observer = NopObserver<T>.Instance;
        }

        /// <summary>
        /// Indicates whether the subject has observers subscribed to it.
        /// </summary>
        public bool HasObservers
        {
            get
            {
                return _observer != NopObserver<T>.Instance && !(_observer is DoneObserver<T>) && _observer != DisposedObserver<T>.Instance;
            }
        }

        /// <summary>
        /// Notifies all subscribed observers about the end of the sequence.
        /// </summary>
        public void OnCompleted()
        {
            var oldObserver = default(IObserver<T>);
            var newObserver = DoneObserver<T>.Completed;

            do
            {
                oldObserver = _observer;

                if (oldObserver == DisposedObserver<T>.Instance || oldObserver is DoneObserver<T>)
                    break;
#pragma warning disable 0420
            } while (Interlocked.CompareExchange(ref _observer, newObserver, oldObserver) != oldObserver);
#pragma warning restore 0420

            oldObserver.OnCompleted();
        }

        /// <summary>
        /// Notifies all subscribed observers about the specified exception.
        /// </summary>
        /// <param name="error">The exception to send to all currently subscribed observers.</param>
        /// <exception cref="ArgumentNullException"><paramref name="error"/> is null.</exception>
        public void OnError(Exception error)
        {
            if (error == null)
                throw new ArgumentNullException("error");

            var oldObserver = default(IObserver<T>);
            var newObserver = new DoneObserver<T> { Exception = error };

            do
            {
                oldObserver = _observer;

                if (oldObserver == DisposedObserver<T>.Instance || oldObserver is DoneObserver<T>)
                    break;
#pragma warning disable 0420
            } while (Interlocked.CompareExchange(ref _observer, newObserver, oldObserver) != oldObserver);
#pragma warning restore 0420

            oldObserver.OnError(error);
        }

        /// <summary>
        /// Notifies all subscribed observers about the arrival of the specified element in the sequence.
        /// </summary>
        /// <param name="value">The value to send to all currently subscribed observers.</param>
        public void OnNext(T value)
        {
            _observer.OnNext(value);
        }

        /// <summary>
        /// Subscribes an observer to the subject.
        /// </summary>
        /// <param name="observer">Observer to subscribe to the subject.</param>
        /// <returns>Disposable object that can be used to unsubscribe the observer from the subject.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="observer"/> is null.</exception>
        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (observer == null)
                throw new ArgumentNullException("observer");

            var oldObserver = default(IObserver<T>);
            var newObserver = default(IObserver<T>);

            do
            {
                oldObserver = _observer;

                if (oldObserver == DisposedObserver<T>.Instance)
                {
                    throw new ObjectDisposedException("");
                }

                if (oldObserver == DoneObserver<T>.Completed)
                {
                    observer.OnCompleted();
                    return Disposable.Empty;
                }

                var done = oldObserver as DoneObserver<T>;
                if (done != null)
                {
                    observer.OnError(done.Exception);
                    return Disposable.Empty;
                }

                if (oldObserver == NopObserver<T>.Instance)
                {
                    newObserver = observer;
                }
                else
                {
                    var obs = oldObserver as Observer<T>;
                    if (obs != null)
                    {
                        newObserver = obs.Add(observer);
                    }
                    else
                    {
                        newObserver = new Observer<T>(new ImmutableList<IObserver<T>>(new[] { oldObserver, observer }));
                    }
                }
#pragma warning disable 0420
            } while (Interlocked.CompareExchange(ref _observer, newObserver, oldObserver) != oldObserver);
#pragma warning restore 0420

            return new Subscription(this, observer);
        }

        class Subscription : IDisposable
        {
            private Subject<T> _subject;
            private IObserver<T> _observer;

            public Subscription(Subject<T> subject, IObserver<T> observer)
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

        private void Unsubscribe(IObserver<T> observer)
        {
            var oldObserver = default(IObserver<T>);
            var newObserver = default(IObserver<T>);

            do
            {
                oldObserver = _observer;

                if (oldObserver == DisposedObserver<T>.Instance || oldObserver is DoneObserver<T>)
                    return;

                var obs = oldObserver as Observer<T>;
                if (obs != null)
                {
                    newObserver = obs.Remove(observer);
                }
                else
                {
                    if (oldObserver != observer)
                        return;

                    newObserver = NopObserver<T>.Instance;
                }
#pragma warning disable 0420
            } while (Interlocked.CompareExchange(ref _observer, newObserver, oldObserver) != oldObserver);
#pragma warning restore 0420
        }

        /// <summary>
        /// Releases all resources used by the current instance of the <see cref="System.Reactive.Subjects.Subject&lt;T&gt;"/> class and unsubscribes all observers.
        /// </summary>
        public void Dispose()
        {
            _observer = DisposedObserver<T>.Instance;
        }
    }
}
#else
using System.Reactive.Disposables;
using System.Threading;

namespace System.Reactive.Subjects
{
    /// <summary>
    /// Represents an object that is both an observable sequence as well as an observer.
    /// Each notification is broadcasted to all subscribed observers.
    /// </summary>
    /// <typeparam name="T">The type of the elements processed by the subject.</typeparam>
    public sealed class Subject<T> : ISubject<T>, IDisposable
    {
        bool isDisposed;
        bool isStopped;
        ImmutableList<IObserver<T>> observers;
        object gate = new object();
        Exception exception;

        /// <summary>
        /// Creates a subject.
        /// </summary>
        public Subject()
        {
            observers = new ImmutableList<IObserver<T>>();
        }

        /// <summary>
        /// Notifies all subscribed observers about the end of the sequence.
        /// </summary>
        public void OnCompleted()
        {
            var os = default(IObserver<T>[]);
            lock (gate)
            {
                CheckDisposed();

                if (!isStopped)
                {
                    os = observers.Data;
                    observers = new ImmutableList<IObserver<T>>();
                    isStopped = true;
                }
            }

            if (os != null)
                foreach (var o in os)
                    o.OnCompleted();
        }

        /// <summary>
        /// Notifies all subscribed observers with the exception.
        /// </summary>
        /// <param name="error">The exception to send to all subscribed observers.</param>
        /// <exception cref="ArgumentNullException"><paramref name="error"/> is null.</exception>
        public void OnError(Exception error)
        {
            if (error == null)
                throw new ArgumentNullException("error");

            var os = default(IObserver<T>[]);
            lock (gate)
            {
                CheckDisposed();

                if (!isStopped)
                {
                    os = observers.Data;
                    observers = new ImmutableList<IObserver<T>>();
                    isStopped = true;
                    exception = error;
                }
            }

            if (os != null)
                foreach (var o in os)
                    o.OnError(error);
        }

        /// <summary>
        /// Notifies all subscribed observers with the value.
        /// </summary>
        /// <param name="value">The value to send to all subscribed observers.</param>
        public void OnNext(T value)
        {
            var os = default(IObserver<T>[]);
            lock (gate)
            {
                CheckDisposed();

                if (!isStopped)
                {
                    os = observers.Data;
                }
            }

            if (os != null)
                foreach (var o in os)
                    o.OnNext(value);
        }

        /// <summary>
        /// Subscribes an observer to the subject.
        /// </summary>
        /// <param name="observer">Observer to subscribe to the subject.</param>
        /// <remarks>IDisposable object that can be used to unsubscribe the observer from the subject.</remarks>
        /// <exception cref="ArgumentNullException"><paramref name="observer"/> is null.</exception>
        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (observer == null)
                throw new ArgumentNullException("observer");

            lock (gate)
            {
                CheckDisposed();

                if (!isStopped)
                {
                    observers = observers.Add(observer);
                    return new Subscription(this, observer);
                }
                else if (exception != null)
                {
                    observer.OnError(exception);
                    return Disposable.Empty;
                }
                else
                {
                    observer.OnCompleted();
                    return Disposable.Empty;
                }
            }
        }

        void Unsubscribe(IObserver<T> observer)
        {
            lock (gate)
            {
                if (observers != null)
                    observers = observers.Remove(observer);
            }
        }

        class Subscription : IDisposable
        {
            Subject<T> subject;
            IObserver<T> observer;

            public Subscription(Subject<T> subject, IObserver<T> observer)
            {
                this.subject = subject;
                this.observer = observer;
            }

            public void Dispose()
            {
                var o = Interlocked.Exchange<IObserver<T>>(ref observer, null);
                if (o != null)
                {
                    subject.Unsubscribe(o);
                    subject = null;
                }
            }
        }

        void CheckDisposed()
        {
            if (isDisposed)
                throw new ObjectDisposedException(string.Empty);
        }

        /// <summary>
        /// Unsubscribe all observers and release resources.
        /// </summary>
        public void Dispose()
        {
            lock (gate)
            {
                isDisposed = true;
                observers = null;
            }
        }
    }
}
#endif
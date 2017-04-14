// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Disposables;

namespace System.Reactive.Subjects
{
    /// <summary>
    /// Represents a value that changes over time.
    /// Observers can subscribe to the subject to receive the last (or initial) value and all subsequent notifications.
    /// </summary>
    /// <typeparam name="T">The type of the elements processed by the subject.</typeparam>
    public sealed class BehaviorSubject<T> : SubjectBase<T>, IDisposable
    {
        #region Fields

        private readonly object _gate = new object();

        private ImmutableList<IObserver<T>> _observers;
        private bool _isStopped;
        private T _value;
        private Exception _exception;
        private bool _isDisposed;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BehaviorSubject{T}"/> class which creates a subject that caches its last value and starts with the specified value.
        /// </summary>
        /// <param name="value">Initial value sent to observers when no other value has been received by the subject yet.</param>
        public BehaviorSubject(T value)
        {
            _value = value;
            _observers = ImmutableList<IObserver<T>>.Empty;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Indicates whether the subject has observers subscribed to it.
        /// </summary>
        public override bool HasObservers => _observers?.Data.Length > 0;

        /// <summary>
        /// Indicates whether the subject has been disposed.
        /// </summary>
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

        /// <summary>
        /// Gets the current value or throws an exception.
        /// </summary>
        /// <value>The initial value passed to the constructor until <see cref="OnNext"/> is called; after which, the last value passed to <see cref="OnNext"/>.</value>
        /// <remarks>
        /// <para><see cref="Value"/> is frozen after <see cref="OnCompleted"/> is called.</para>
        /// <para>After <see cref="OnError"/> is called, <see cref="Value"/> always throws the specified exception.</para>
        /// <para>An exception is always thrown after <see cref="Dispose"/> is called.</para>
        /// <alert type="caller">
        /// Reading <see cref="Value"/> is a thread-safe operation, though there's a potential race condition when <see cref="OnNext"/> or <see cref="OnError"/> are being invoked concurrently.
        /// In some cases, it may be necessary for a caller to use external synchronization to avoid race conditions.
        /// </alert>
        /// </remarks>
        /// <exception cref="ObjectDisposedException">Dispose was called.</exception>
        public T Value
        {
            get
            {
                lock (_gate)
                {
                    CheckDisposed();

                    if (_exception != null)
                    {
                        throw _exception;
                    }

                    return _value;
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Tries to get the current value or throws an exception.
        /// </summary>
        /// <param name="value">The initial value passed to the constructor until <see cref="OnNext"/> is called; after which, the last value passed to <see cref="OnNext"/>.</param>
        /// <returns>true if a value is available; false if the subject was disposed.</returns>
        /// <remarks>
        /// <para>The value returned from <see cref="TryGetValue"/> is frozen after <see cref="OnCompleted"/> is called.</para>
        /// <para>After <see cref="OnError"/> is called, <see cref="TryGetValue"/> always throws the specified exception.</para>
        /// <alert type="caller">
        /// Calling <see cref="TryGetValue"/> is a thread-safe operation, though there's a potential race condition when <see cref="OnNext"/> or <see cref="OnError"/> are being invoked concurrently.
        /// In some cases, it may be necessary for a caller to use external synchronization to avoid race conditions.
        /// </alert>
        /// </remarks>
        public bool TryGetValue(out T value)
        {
            lock (_gate)
            {
                if (_isDisposed)
                {
                    value = default(T);
                    return false;
                }
                else if (_exception != null)
                {
                    throw _exception;
                }
                else
                {
                    value = _value;
                    return true;
                }
            }
        }

        #region IObserver<T> implementation

        /// <summary>
        /// Notifies all subscribed observers about the end of the sequence.
        /// </summary>
        public override void OnCompleted()
        {
            var os = default(IObserver<T>[]);
            lock (_gate)
            {
                CheckDisposed();

                if (!_isStopped)
                {
                    os = _observers.Data;
                    _observers = ImmutableList<IObserver<T>>.Empty;
                    _isStopped = true;
                }
            }

            if (os != null)
            {
                foreach (var o in os)
                {
                    o.OnCompleted();
                }
            }
        }

        /// <summary>
        /// Notifies all subscribed observers about the exception.
        /// </summary>
        /// <param name="error">The exception to send to all observers.</param>
        /// <exception cref="ArgumentNullException"><paramref name="error"/> is <c>null</c>.</exception>
        public override void OnError(Exception error)
        {
            if (error == null)
                throw new ArgumentNullException(nameof(error));

            var os = default(IObserver<T>[]);
            lock (_gate)
            {
                CheckDisposed();

                if (!_isStopped)
                {
                    os = _observers.Data;
                    _observers = ImmutableList<IObserver<T>>.Empty;
                    _isStopped = true;
                    _exception = error;
                }
            }

            if (os != null)
            {
                foreach (var o in os)
                {
                    o.OnError(error);
                }
            }
        }

        /// <summary>
        /// Notifies all subscribed observers about the arrival of the specified element in the sequence.
        /// </summary>
        /// <param name="value">The value to send to all observers.</param>
        public override void OnNext(T value)
        {
            var os = default(IObserver<T>[]);
            lock (_gate)
            {
                CheckDisposed();

                if (!_isStopped)
                {
                    _value = value;
                    os = _observers.Data;
                }
            }

            if (os != null)
            {
                foreach (var o in os)
                {
                    o.OnNext(value);
                }
            }
        }

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

            var ex = default(Exception);

            lock (_gate)
            {
                CheckDisposed();

                if (!_isStopped)
                {
                    _observers = _observers.Add(observer);
                    observer.OnNext(_value);
                    return new Subscription(this, observer);
                }

                ex = _exception;
            }

            if (ex != null)
            {
                observer.OnError(ex);
            }
            else
            {
                observer.OnCompleted();
            }

            return Disposable.Empty;
        }

        #endregion

        #region IDisposable implementation

        /// <summary>
        /// Unsubscribe all observers and release resources.
        /// </summary>
        public override void Dispose()
        {
            lock (_gate)
            {
                _isDisposed = true;
                _observers = null;
                _value = default(T);
                _exception = null;
            }
        }

        private void CheckDisposed()
        {
            if (_isDisposed)
                throw new ObjectDisposedException(string.Empty);
        }

        #endregion

        private sealed class Subscription : IDisposable
        {
            private readonly BehaviorSubject<T> _subject;
            private IObserver<T> _observer;

            public Subscription(BehaviorSubject<T> subject, IObserver<T> observer)
            {
                _subject = subject;
                _observer = observer;
            }

            public void Dispose()
            {
                if (_observer != null)
                {
                    lock (_subject._gate)
                    {
                        if (!_subject._isDisposed && _observer != null)
                        {
                            _subject._observers = _subject._observers.Remove(_observer);
                            _observer = null;
                        }
                    }
                }
            }
        }

        #endregion
    }
}

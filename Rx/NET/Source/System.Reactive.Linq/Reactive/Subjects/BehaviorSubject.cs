// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System.Reactive.Disposables;

namespace System.Reactive.Subjects
{
    /// <summary>
    /// Represents a value that changes over time.
    /// Observers can subscribe to the subject to receive the last (or initial) value and all subsequent notifications.
    /// </summary>
    /// <typeparam name="T">The type of the elements processed by the subject.</typeparam>
    public sealed class BehaviorSubject<T> : ISubject<T>, IDisposable
    {
        private readonly object _gate = new object();

        private ImmutableList<IObserver<T>> _observers;
        private bool _isStopped;
        private T _value;
        private Exception _exception;
        private bool _isDisposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="System.Reactive.Subjects.BehaviorSubject&lt;T&gt;"/> class which creates a subject that caches its last value and starts with the specified value.
        /// </summary>
        /// <param name="value">Initial value sent to observers when no other value has been received by the subject yet.</param>
        public BehaviorSubject(T value)
        {
            _value = value;
            _observers = new ImmutableList<IObserver<T>>();
        }

        /// <summary>
        /// Indicates whether the subject has observers subscribed to it.
        /// </summary>
        public bool HasObservers
        {
            get
            {
                var observers = _observers;
                return observers != null && observers.Data.Length > 0;
            }
        }

        /// <summary>
        /// Notifies all subscribed observers about the end of the sequence.
        /// </summary>
        public void OnCompleted()
        {
            var os = default(IObserver<T>[]);
            lock (_gate)
            {
                CheckDisposed();

                if (!_isStopped)
                {
                    os = _observers.Data;
                    _observers = new ImmutableList<IObserver<T>>();
                    _isStopped = true;
                }
            }

            if (os != null)
            {
                foreach (var o in os)
                    o.OnCompleted();
            }
        }

        /// <summary>
        /// Notifies all subscribed observers about the exception.
        /// </summary>
        /// <param name="error">The exception to send to all observers.</param>
        /// <exception cref="ArgumentNullException"><paramref name="error"/> is null.</exception>
        public void OnError(Exception error)
        {
            if (error == null)
                throw new ArgumentNullException("error");

            var os = default(IObserver<T>[]);
            lock (_gate)
            {
                CheckDisposed();

                if (!_isStopped)
                {
                    os = _observers.Data;
                    _observers = new ImmutableList<IObserver<T>>();
                    _isStopped = true;
                    _exception = error;
                }
            }

            if (os != null)
                foreach (var o in os)
                    o.OnError(error);
        }

        /// <summary>
        /// Notifies all subscribed observers about the arrival of the specified element in the sequence.
        /// </summary>
        /// <param name="value">The value to send to all observers.</param>
        public void OnNext(T value)
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
                    o.OnNext(value);
            }
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
                observer.OnError(ex);
            else
                observer.OnCompleted();

            return Disposable.Empty;
        }

        class Subscription : IDisposable
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

        void CheckDisposed()
        {
            if (_isDisposed)
                throw new ObjectDisposedException(string.Empty);
        }

        /// <summary>
        /// Unsubscribe all observers and release resources.
        /// </summary>
        public void Dispose()
        {
            lock (_gate)
            {
                _isDisposed = true;
                _observers = null;
                _value = default(T);
                _exception = null;
            }
        }
    }
}

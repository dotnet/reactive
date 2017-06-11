// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Disposables;
using System.Threading;
using System.Runtime.CompilerServices;

namespace System.Reactive.Subjects
{
    /// <summary>
    /// Represents the result of an asynchronous operation.
    /// The last value before the OnCompleted notification, or the error received through OnError, is sent to all subscribed observers.
    /// </summary>
    /// <typeparam name="T">The type of the elements processed by the subject.</typeparam>
    public sealed class AsyncSubject<T> : SubjectBase<T>, IDisposable, INotifyCompletion
    {
        #region Fields

        private readonly object _gate = new object();

        private ImmutableList<IObserver<T>> _observers;
        private bool _isDisposed;
        private bool _isStopped;
        private T _value;
        private bool _hasValue;
        private Exception _exception;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a subject that can only receive one value and that value is cached for all future observations.
        /// </summary>
        public AsyncSubject()
        {
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

        #endregion

        #region Methods

        #region IObserver<T> implementation

        /// <summary>
        /// Notifies all subscribed observers about the end of the sequence, also causing the last received value to be sent out (if any).
        /// </summary>
        public override void OnCompleted()
        {
            var os = default(IObserver<T>[]);

            var v = default(T);
            var hv = false;
            lock (_gate)
            {
                CheckDisposed();

                if (!_isStopped)
                {
                    os = _observers.Data;
                    _observers = ImmutableList<IObserver<T>>.Empty;
                    _isStopped = true;
                    v = _value;
                    hv = _hasValue;
                }
            }

            if (os != null)
            {
                if (hv)
                {
                    foreach (var o in os)
                    {
                        o.OnNext(v);
                        o.OnCompleted();
                    }
                }
                else
                {
                    foreach (var o in os)
                    {
                        o.OnCompleted();
                    }
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
        /// Sends a value to the subject. The last value received before successful termination will be sent to all subscribed and future observers.
        /// </summary>
        /// <param name="value">The value to store in the subject.</param>
        public override void OnNext(T value)
        {
            lock (_gate)
            {
                CheckDisposed();

                if (!_isStopped)
                {
                    _value = value;
                    _hasValue = true;
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
            var v = default(T);
            var hv = false;

            lock (_gate)
            {
                CheckDisposed();

                if (!_isStopped)
                {
                    _observers = _observers.Add(observer);
                    return new Subscription(this, observer);
                }

                ex = _exception;
                hv = _hasValue;
                v = _value;
            }

            if (ex != null)
            {
                observer.OnError(ex);
            }
            else if (hv)
            {
                observer.OnNext(v);
                observer.OnCompleted();
            }
            else
            {
                observer.OnCompleted();
            }

            return Disposable.Empty;
        }

        private sealed class Subscription : IDisposable
        {
            private readonly AsyncSubject<T> _subject;
            private IObserver<T> _observer;

            public Subscription(AsyncSubject<T> subject, IObserver<T> observer)
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

        #region IDisposable implementation

        void CheckDisposed()
        {
            if (_isDisposed)
                throw new ObjectDisposedException(string.Empty);
        }

        /// <summary>
        /// Unsubscribe all observers and release resources.
        /// </summary>
        public override void Dispose()
        {
            lock (_gate)
            {
                _isDisposed = true;
                _observers = null;
                _exception = null;
                _value = default(T);
            }
        }

        #endregion

        #region Await support

        /// <summary>
        /// Gets an awaitable object for the current AsyncSubject.
        /// </summary>
        /// <returns>Object that can be awaited.</returns>
        public AsyncSubject<T> GetAwaiter() => this;

        /// <summary>
        /// Specifies a callback action that will be invoked when the subject completes.
        /// </summary>
        /// <param name="continuation">Callback action that will be invoked when the subject completes.</param>
        /// <exception cref="ArgumentNullException"><paramref name="continuation"/> is <c>null</c>.</exception>
        public void OnCompleted(Action continuation)
        {
            if (continuation == null)
                throw new ArgumentNullException(nameof(continuation));

            OnCompleted(continuation, originalContext: true);
        }

        private void OnCompleted(Action continuation, bool originalContext)
        {
            //
            // [OK] Use of unsafe Subscribe: this type's Subscribe implementation is safe.
            //
            Subscribe/*Unsafe*/(new AwaitObserver(continuation, originalContext));
        }

        private sealed class AwaitObserver : IObserver<T>
        {
            private readonly SynchronizationContext _context;
            private readonly Action _callback;

            public AwaitObserver(Action callback, bool originalContext)
            {
                if (originalContext)
                    _context = SynchronizationContext.Current;

                _callback = callback;
            }

            public void OnCompleted() => InvokeOnOriginalContext();

            public void OnError(Exception error) => InvokeOnOriginalContext();

            public void OnNext(T value) { }

            private void InvokeOnOriginalContext()
            {
                if (_context != null)
                {
                    //
                    // No need for OperationStarted and OperationCompleted calls here;
                    // this code is invoked through await support and will have a way
                    // to observe its start/complete behavior, either through returned
                    // Task objects or the async method builder's interaction with the
                    // SynchronizationContext object.
                    //
                    _context.Post(c => ((Action)c)(), _callback);
                }
                else
                {
                    _callback();
                }
            }
        }

        /// <summary>
        /// Gets whether the AsyncSubject has completed.
        /// </summary>
        public bool IsCompleted => _isStopped;

        /// <summary>
        /// Gets the last element of the subject, potentially blocking until the subject completes successfully or exceptionally.
        /// </summary>
        /// <returns>The last element of the subject. Throws an InvalidOperationException if no element was received.</returns>
        /// <exception cref="InvalidOperationException">The source sequence is empty.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Await pattern for C# and VB compilers.")]
        public T GetResult()
        {
            if (!_isStopped)
            {
                var e = new ManualResetEvent(initialState: false);
                OnCompleted(() => e.Set(), originalContext: false);
                e.WaitOne();
            }

            _exception.ThrowIfNotNull();

            if (!_hasValue)
                throw new InvalidOperationException(Strings_Linq.NO_ELEMENTS);

            return _value;
        }

        #endregion

        #endregion
    }
}

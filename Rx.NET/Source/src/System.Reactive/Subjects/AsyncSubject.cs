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

        private AsyncSubjectDisposable[] _observers;

        private T _value;
        private bool _hasValue;
        private Exception _exception;

        /// <summary>
        /// A pre-allocated empty array for the no-observers state.
        /// </summary>
        static readonly AsyncSubjectDisposable[] EMPTY = new AsyncSubjectDisposable[0];

        /// <summary>
        /// A pre-allocated empty array indicating the AsyncSubject has terminated
        /// </summary>
        static readonly AsyncSubjectDisposable[] TERMINATED = new AsyncSubjectDisposable[0];

        /// <summary>
        /// A pre-allocated empty array indicating the AsyncSubject has terminated
        /// </summary>
        static readonly AsyncSubjectDisposable[] DISPOSED = new AsyncSubjectDisposable[0];

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a subject that can only receive one value and that value is cached for all future observations.
        /// </summary>
        public AsyncSubject()
        {
            _observers = EMPTY;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Indicates whether the subject has observers subscribed to it.
        /// </summary>
        public override bool HasObservers => _observers.Length != 0;

        /// <summary>
        /// Indicates whether the subject has been disposed.
        /// </summary>
        public override bool IsDisposed => Volatile.Read(ref _observers) == DISPOSED;

        #endregion

        #region Methods

        #region IObserver<T> implementation

        /// <summary>
        /// Notifies all subscribed observers about the end of the sequence, also causing the last received value to be sent out (if any).
        /// </summary>
        public override void OnCompleted()
        {
            for (; ; )
            {
                var observers = Volatile.Read(ref _observers);
                if (observers == DISPOSED)
                {
                    _exception = null;
                    ThrowDisposed();
                    break;
                }
                if (observers == TERMINATED)
                {
                    break;
                }
                if (Interlocked.CompareExchange(ref _observers, TERMINATED, observers) == observers)
                {
                    var hasValue = _hasValue;
                    if (hasValue)
                    {
                        var value = _value;

                        foreach (var o in observers)
                        {
                            if (!o.IsDisposed())
                            {
                                o.downstream.OnNext(value);
                                o.downstream.OnCompleted();
                            }
                        }
                    }
                    else
                    {
                        foreach (var o in observers)
                        {
                            if (!o.IsDisposed())
                            {
                                o.downstream.OnCompleted();
                            }
                        }
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

            for (; ; )
            {
                var observers = Volatile.Read(ref _observers);
                if (observers == DISPOSED)
                {
                    _exception = null;
                    _value = default(T);
                    ThrowDisposed();
                    break;
                }
                if (observers == TERMINATED)
                {
                    break;
                }
                _exception = error;
                if (Interlocked.CompareExchange(ref _observers, TERMINATED, observers) == observers)
                {
                    var ex = _exception;
                    foreach (var o in observers)
                    {
                        if (!o.IsDisposed())
                        {
                            o.downstream.OnError(error);
                        }
                    }
                }
            }

        }

        /// <summary>
        /// Sends a value to the subject. The last value received before successful termination will be sent to all subscribed and future observers.
        /// </summary>
        /// <param name="value">The value to store in the subject.</param>
        public override void OnNext(T value)
        {
            var observers = Volatile.Read(ref _observers);
            if (observers == DISPOSED)
            {
                _value = default(T);
                _exception = null;
                ThrowDisposed();
                return;
            }
            if (observers == TERMINATED)
            {
                return;
            }

            _value = value;
            _hasValue = true;
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

            var parent = new AsyncSubjectDisposable(this, observer);

            if (!Add(parent))
            {
                var ex = _exception;
                if (ex != null)
                {
                    observer.OnError(ex);
                }
                else
                {
                    if (_hasValue)
                    {
                        observer.OnNext(_value);
                    }
                    observer.OnCompleted();
                }
                return Disposable.Empty;
            }

            return parent;
        }

        bool Add(AsyncSubjectDisposable inner)
        {
            for (; ; )
            {
                var a = Volatile.Read(ref _observers);
                if (a == DISPOSED)
                {
                    _value = default(T);
                    _exception = null;
                    ThrowDisposed();
                    return true;
                }

                if (a == TERMINATED)
                {
                    return false;
                }

                var n = a.Length;
                var b = new AsyncSubjectDisposable[n + 1];
                Array.Copy(a, 0, b, 0, n);
                b[n] = inner;
                if (Interlocked.CompareExchange(ref _observers, b, a) == a)
                {
                    return true;
                }
            }
        }

        void Remove(AsyncSubjectDisposable inner)
        {
            for (; ; )
            {
                var a = Volatile.Read(ref _observers);

                var n = a.Length;

                if (n == 0)
                {
                    break;
                }

                var j = -1;

                for (var i = 0; i < n; i++)
                {
                    if (a[i] == inner)
                    {
                        j = i;
                        break;
                    }
                }

                if (j < 0)
                {
                    break;
                }

                var b = default(AsyncSubjectDisposable[]);
                if (n == 1)
                {
                    b = EMPTY;
                }
                else
                {
                    b = new AsyncSubjectDisposable[n - 1];
                    Array.Copy(a, 0, b, 0, j);
                    Array.Copy(a, j + 1, b, j, n - j - 1);
                }

                if (Interlocked.CompareExchange(ref _observers, b, a) == a)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// A disposable connecting the AsyncSubject and an IObserver.
        /// </summary>
        sealed class AsyncSubjectDisposable : IDisposable
        {
            internal readonly IObserver<T> downstream;

            AsyncSubject<T> parent;

            public AsyncSubjectDisposable(AsyncSubject<T> parent, IObserver<T> downstream)
            {
                this.parent = parent;
                this.downstream = downstream;
            }

            public void Dispose()
            {
                Interlocked.Exchange(ref parent, null)?.Remove(this);
            }

            internal bool IsDisposed()
            {
                return Volatile.Read(ref parent) == null;
            }
        }

        #endregion

        #region IDisposable implementation

        void ThrowDisposed()
        {
            throw new ObjectDisposedException(string.Empty);
        }

        /// <summary>
        /// Unsubscribe all observers and release resources.
        /// </summary>
        public override void Dispose()
        {
            if (Interlocked.Exchange(ref _observers, DISPOSED) != DISPOSED)
            {
                _exception = null;
                _value = default(T);
                _hasValue = false;
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
        public bool IsCompleted => Volatile.Read(ref _observers) == TERMINATED;

        /// <summary>
        /// Gets the last element of the subject, potentially blocking until the subject completes successfully or exceptionally.
        /// </summary>
        /// <returns>The last element of the subject. Throws an InvalidOperationException if no element was received.</returns>
        /// <exception cref="InvalidOperationException">The source sequence is empty.</exception>
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Await pattern for C# and VB compilers.")]
        public T GetResult()
        {
            if (Volatile.Read(ref _observers) != TERMINATED)
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

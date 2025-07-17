// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Disposables;
using System.Runtime.CompilerServices;
using System.Threading;

namespace System.Reactive.Subjects
{
    /// <summary>
    /// Represents the result of an asynchronous operation.
    /// The last value before the OnCompleted notification, or the error received through OnError, is sent to all subscribed observers.
    /// </summary>
    /// <typeparam name="T">The type of the elements processed by the subject.</typeparam>
    public sealed class AsyncSubject<T> : SubjectBase<T>, INotifyCompletion
    {
        #region Fields

        private AsyncSubjectDisposable[] _observers;
        private T? _value;
        private bool _hasValue;
        private Exception? _exception;

#pragma warning disable CA1825,IDE0300 // (Avoid zero-length array allocations. Use collection expressions) The identity of these arrays matters, so we can't use the shared Array.Empty<T>() instance either explicitly, or indirectly via a collection expression
        /// <summary>
        /// A pre-allocated empty array indicating the AsyncSubject has terminated.
        /// </summary>
        private static readonly AsyncSubjectDisposable[] Terminated = new AsyncSubjectDisposable[0];
        /// <summary>
        /// A pre-allocated empty array indicating the AsyncSubject has been disposed.
        /// </summary>
        private static readonly AsyncSubjectDisposable[] Disposed = new AsyncSubjectDisposable[0];
#pragma warning restore CA1825,IDE0300

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a subject that can only receive one value and that value is cached for all future observations.
        /// </summary>
        public AsyncSubject() => _observers = [];

        #endregion

        #region Properties

        /// <summary>
        /// Indicates whether the subject has observers subscribed to it.
        /// </summary>
        public override bool HasObservers => Volatile.Read(ref _observers).Length != 0;

        /// <summary>
        /// Indicates whether the subject has been disposed.
        /// </summary>
        public override bool IsDisposed => Volatile.Read(ref _observers) == Disposed;

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

                if (observers == Disposed)
                {
                    _exception = null;
                    ThrowDisposed();
                    break;
                }

                if (observers == Terminated)
                {
                    break;
                }

                if (Interlocked.CompareExchange(ref _observers, Terminated, observers) == observers)
                {
                    var hasValue = _hasValue;

                    if (hasValue)
                    {
                        var value = _value;

                        foreach (var observer in observers)
                        {
                            var o = observer.Observer;

                            if (o != null)
                            {
                                o.OnNext(value!);
                                o.OnCompleted();
                            }
                        }
                    }
                    else
                    {
                        foreach (var observer in observers)
                        {
                            observer.Observer?.OnCompleted();
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
            {
                throw new ArgumentNullException(nameof(error));
            }

            for (; ; )
            {
                var observers = Volatile.Read(ref _observers);

                if (observers == Disposed)
                {
                    _exception = null;
                    _value = default;
                    ThrowDisposed();
                    break;
                }

                if (observers == Terminated)
                {
                    break;
                }

                _exception = error;

                if (Interlocked.CompareExchange(ref _observers, Terminated, observers) == observers)
                {
                    foreach (var observer in observers)
                    {
                        observer.Observer?.OnError(error);
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

            if (observers == Disposed)
            {
                _value = default;
                _exception = null;
                ThrowDisposed();
                return;
            }

            if (observers == Terminated)
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
            {
                throw new ArgumentNullException(nameof(observer));
            }

            var disposable = default(AsyncSubjectDisposable);
            for (; ; )
            {
                var observers = Volatile.Read(ref _observers);

                if (observers == Disposed)
                {
                    _value = default;
                    _exception = null;
                    ThrowDisposed();

                    break;
                }

                if (observers == Terminated)
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
                            observer.OnNext(_value!);
                        }

                        observer.OnCompleted();
                    }

                    break;
                }

                disposable ??= new AsyncSubjectDisposable(this, observer);

                var n = observers.Length;
                var b = new AsyncSubjectDisposable[n + 1];

                Array.Copy(observers, 0, b, 0, n);

                b[n] = disposable;

                if (Interlocked.CompareExchange(ref _observers, b, observers) == observers)
                {
                    return disposable;
                }
            }

            return Disposable.Empty;
        }

        private void Unsubscribe(AsyncSubjectDisposable observer)
        {
            for (; ; )
            {
                var a = Volatile.Read(ref _observers);
                var n = a.Length;

                if (n == 0)
                {
                    break;
                }

                var j = Array.IndexOf(a, observer);

                if (j < 0)
                {
                    break;
                }

                AsyncSubjectDisposable[] b;

                if (n == 1)
                {
                    b = [];
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
        private sealed class AsyncSubjectDisposable : IDisposable
        {
            private AsyncSubject<T> _subject;
            private volatile IObserver<T>? _observer;

            public AsyncSubjectDisposable(AsyncSubject<T> subject, IObserver<T> observer)
            {
                _subject = subject;
                _observer = observer;
            }

            public IObserver<T>? Observer => _observer;

            public void Dispose()
            {
                var observer = Interlocked.Exchange(ref _observer, null);
                if (observer == null)
                {
                    return;
                }

                _subject.Unsubscribe(this);
                _subject = null!;
            }
        }

        #endregion

        #region IDisposable implementation

        private static void ThrowDisposed() => throw new ObjectDisposedException(string.Empty);

        /// <summary>
        /// Unsubscribe all observers and release resources.
        /// </summary>
        public override void Dispose()
        {
            if (Interlocked.Exchange(ref _observers, Disposed) != Disposed)
            {
                _exception = null;
                _value = default;
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
            {
                throw new ArgumentNullException(nameof(continuation));
            }

            //
            // [OK] Use of unsafe Subscribe: this type's Subscribe implementation is safe.
            //
            Subscribe/*Unsafe*/(new AwaitObserver(continuation));
        }

        private sealed class AwaitObserver : IObserver<T>
        {
            private readonly SynchronizationContext? _context;
            private readonly Action _callback;

            public AwaitObserver(Action callback)
            {
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
                    _context.Post(static c => ((Action)c!)(), _callback);
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
        public bool IsCompleted => Volatile.Read(ref _observers) == Terminated;

        /// <summary>
        /// Gets the last element of the subject, potentially blocking until the subject completes successfully or exceptionally.
        /// </summary>
        /// <returns>The last element of the subject. Throws an InvalidOperationException if no element was received.</returns>
        /// <exception cref="InvalidOperationException">The source sequence is empty.</exception>
        public T GetResult()
        {
            if (Volatile.Read(ref _observers) != Terminated)
            {
                using var e = new ManualResetEventSlim(initialState: false);

                //
                // [OK] Use of unsafe Subscribe: this type's Subscribe implementation is safe.
                //
                Subscribe/*Unsafe*/(new BlockingObserver(e));

                e.Wait();
            }

            _exception?.Throw();

            if (!_hasValue)
            {
                throw new InvalidOperationException(Strings_Linq.NO_ELEMENTS);
            }

            return _value!;
        }

        private sealed class BlockingObserver : IObserver<T>
        {
            private readonly ManualResetEventSlim _e;

            public BlockingObserver(ManualResetEventSlim e) => _e = e;

            public void OnCompleted() => Done();

            public void OnError(Exception error) => Done();

            public void OnNext(T value) { }

            private void Done() => _e.Set();
        }

        #endregion

        #endregion
    }
}

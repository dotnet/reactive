// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reactive.Disposables;
using System.Threading;

namespace System.Reactive.Subjects
{
    /// <summary>
    /// Represents an object that is both an observable sequence as well as an observer.
    /// Each notification is broadcasted to all subscribed observers.
    /// </summary>
    /// <typeparam name="T">The type of the elements processed by the subject.</typeparam>
    public sealed class Subject<T> : SubjectBase<T>
    {
        private sealed class SubjectDisposableComparer : IComparer<SubjectDisposable>
        {
            public int Compare(SubjectDisposable x, SubjectDisposable y)
            {
                return x.GetHashCode().CompareTo(y.GetHashCode());
            }
        }

        #region Fields
        private static readonly ImmutableSortedDictionary<SubjectDisposable, int> Empty = ImmutableSortedDictionary<SubjectDisposable, int>.Empty.WithComparers(new SubjectDisposableComparer());
        private ImmutableSortedDictionary<SubjectDisposable, int> _observers;
        private Exception _exception;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a subject.
        /// </summary>
        public Subject()
        {
            Volatile.Write(ref _observers, Empty);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Indicates whether the subject has observers subscribed to it.
        /// </summary>
        public override bool HasObservers
        {
            get
            {
                return (Volatile.Read(ref _observers)?.Count).GetValueOrDefault() != 0;
            }
        }

        /// <summary>
        /// Indicates whether the subject has been disposed.
        /// </summary>
        public override bool IsDisposed => Volatile.Read(ref _exception) == ExceptionHelper.Disposed;

        #endregion

        #region Methods

        #region IObserver<T> implementation

        private void ThrowDisposed()
        {
            throw new ObjectDisposedException(string.Empty);
        }

        /// <summary>
        /// Notifies all subscribed observers about the end of the sequence.
        /// </summary>
        public override void OnCompleted()
        {
            for (;;)
            {
                var exception = Interlocked.CompareExchange(ref _exception, ExceptionHelper.Terminated, null);

                if (exception == null)
                {
                    var observers = Volatile.Read(ref _observers);

                    if (observers != null && Interlocked.CompareExchange(ref _observers, Empty, observers) == observers)
                    {
                        foreach (var kvp in observers)
                        {
                            for (var i = 0; i < kvp.Value; i++)
                            {
                                kvp.Key.Observer?.OnCompleted();
                            }
                        }

                        break;
                    }
                }
                else if (exception == ExceptionHelper.Disposed)
                    ThrowDisposed();
                else
                    return;
            }
        }

        /// <summary>
        /// Notifies all subscribed observers about the specified exception.
        /// </summary>
        /// <param name="error">The exception to send to all currently subscribed observers.</param>
        /// <exception cref="ArgumentNullException"><paramref name="error"/> is null.</exception>
        public override void OnError(Exception error)
        {
            if (error == null)
            {
                throw new ArgumentNullException(nameof(error));
            }

            for (;;)
            {
                var exception = Interlocked.CompareExchange(ref _exception, error, null);

                if (exception == null)
                {
                    var observers = Volatile.Read(ref _observers);

                    if (observers != null && Interlocked.CompareExchange(ref _observers, Empty, observers) == observers)
                    {
                        foreach (var kvp in observers)
                        {
                            for (var i = 0; i < kvp.Value; i++)
                            {
                                kvp.Key.Observer?.OnError(error);
                            }
                        }

                        break;
                    }
                }
                else if (exception == ExceptionHelper.Disposed)
                    ThrowDisposed();
                else
                    return;
            }
        }

        /// <summary>
        /// Notifies all subscribed observers about the arrival of the specified element in the sequence.
        /// </summary>
        /// <param name="value">The value to send to all currently subscribed observers.</param>
        public override void OnNext(T value)
        {
            var observers = Volatile.Read(ref _observers);

            if (observers == null)
            {
                ThrowDisposed();
                return;
            }

            foreach (var kvp in observers)
            {
                for (var i = 0; i < kvp.Value; i++)
                {
                    kvp.Key.Observer?.OnNext(value);
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
        /// <exception cref="ArgumentNullException"><paramref name="observer"/> is null.</exception>
        public override IDisposable Subscribe(IObserver<T> observer)
        {
            if (observer == null)
            {
                throw new ArgumentNullException(nameof(observer));
            }

            var disposable = default(SubjectDisposable);

            for (; ; )
            {
                var exception = Volatile.Read(ref _exception);

                if (exception != null)
                {
                    if (exception == ExceptionHelper.Disposed)
                    {
                        ThrowDisposed();
                    }
                    else if (exception == ExceptionHelper.Terminated)
                    {
                        observer.OnCompleted();
                    }
                    else
                    {
                        observer.OnError(exception);
                    }

                    break;
                }

                if (disposable == null)
                {
                    disposable = new SubjectDisposable(this, observer);
                }

                var observers = Volatile.Read(ref _observers);

                if (observers != null)
                {
                    var newObservers = observers.TryGetValue(disposable, out var existingCount)
                        ? observers.SetItem(disposable, existingCount + 1)
                        : observers.Add(disposable, 1);

                    if (Interlocked.CompareExchange(ref _observers, newObservers, observers) == observers)
                    {
                        return disposable;
                    }
                }
            }

            return Disposable.Empty;
        }

        private void Unsubscribe(SubjectDisposable observer)
        {
            for (; ; )
            {
                var observers = Volatile.Read(ref _observers);

                if (observers == null)
                    return;

                var newObservers = observers.TryGetValue(observer, out var existingCount) && existingCount > 1
                    ? observers.SetItem(observer, existingCount - 1)
                    : observers.Remove(observer);
                    
                if (Interlocked.CompareExchange(ref _observers, newObservers, observers) == observers)
                {
                    break;
                }
            }
        }

        private sealed class SubjectDisposable : IDisposable
        {
            private Subject<T> _subject;
            private IObserver<T> _observer;

            public SubjectDisposable(Subject<T> subject, IObserver<T> observer)
            {
                _subject = subject;
                Volatile.Write(ref _observer, observer);
            }

            public void Dispose()
            {
                var observer = Interlocked.Exchange(ref _observer, null);
                if (observer == null)
                {
                    return;
                }

                _subject.Unsubscribe(this);
                _subject = null;
            }

            public IObserver<T> Observer { get { return Volatile.Read(ref _observer); } }
        }

        #endregion

        #region IDisposable implementation

        /// <summary>
        /// Releases all resources used by the current instance of the <see cref="Subject{T}"/> class and unsubscribes all observers.
        /// </summary>
        public override void Dispose()
        {
            Interlocked.Exchange(ref _observers, null);
            Interlocked.Exchange(ref _exception, ExceptionHelper.Disposed);
        }

        #endregion

        #endregion
    }
}

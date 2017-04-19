// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Disposables;
using System.Threading;

namespace System.Reactive.Subjects
{
    /// <summary>
    /// Represents an object that is both an observable sequence as well as an observer.
    /// Each notification is broadcasted to all subscribed observers.
    /// </summary>
    /// <typeparam name="T">The type of the elements processed by the subject.</typeparam>
    public sealed class Subject<T> : SubjectBase<T>, IDisposable
    {
        #region Fields

        private volatile IObserver<T> _observer;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a subject.
        /// </summary>
        public Subject()
        {
            _observer = NopObserver<T>.Instance;
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
                return _observer != NopObserver<T>.Instance && !(_observer is DoneObserver<T>) && _observer != DisposedObserver<T>.Instance;
            }
        }

        /// <summary>
        /// Indicates whether the subject has been disposed.
        /// </summary>
        public override bool IsDisposed => _observer is DisposedObserver<T>;

        #endregion

        #region Methods

        #region IObserver<T> implementation

        /// <summary>
        /// Notifies all subscribed observers about the end of the sequence.
        /// </summary>
        public override void OnCompleted()
        {
            var oldObserver = default(IObserver<T>);
            var newObserver = DoneObserver<T>.Completed;

            do
            {
                oldObserver = _observer;

                if (oldObserver == DisposedObserver<T>.Instance || oldObserver is DoneObserver<T>)
                    break;
            } while (Interlocked.CompareExchange(ref _observer, newObserver, oldObserver) != oldObserver);

            oldObserver.OnCompleted();
        }

        /// <summary>
        /// Notifies all subscribed observers about the specified exception.
        /// </summary>
        /// <param name="error">The exception to send to all currently subscribed observers.</param>
        /// <exception cref="ArgumentNullException"><paramref name="error"/> is null.</exception>
        public override void OnError(Exception error)
        {
            if (error == null)
                throw new ArgumentNullException(nameof(error));

            var oldObserver = default(IObserver<T>);
            var newObserver = new DoneObserver<T> { Exception = error };

            do
            {
                oldObserver = _observer;

                if (oldObserver == DisposedObserver<T>.Instance || oldObserver is DoneObserver<T>)
                    break;
            } while (Interlocked.CompareExchange(ref _observer, newObserver, oldObserver) != oldObserver);

            oldObserver.OnError(error);
        }

        /// <summary>
        /// Notifies all subscribed observers about the arrival of the specified element in the sequence.
        /// </summary>
        /// <param name="value">The value to send to all currently subscribed observers.</param>
        public override void OnNext(T value)
        {
            _observer.OnNext(value);
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
                throw new ArgumentNullException(nameof(observer));

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

                if (oldObserver is DoneObserver<T> done)
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
                    if (oldObserver is Observer<T> obs)
                    {
                        newObserver = obs.Add(observer);
                    }
                    else
                    {
                        newObserver = new Observer<T>(new ImmutableList<IObserver<T>>(new[] { oldObserver, observer }));
                    }
                }
            } while (Interlocked.CompareExchange(ref _observer, newObserver, oldObserver) != oldObserver);

            return new Subscription(this, observer);
        }

        private sealed class Subscription : IDisposable
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

                if (oldObserver is Observer<T> obs)
                {
                    newObserver = obs.Remove(observer);
                }
                else
                {
                    if (oldObserver != observer)
                        return;

                    newObserver = NopObserver<T>.Instance;
                }
            } while (Interlocked.CompareExchange(ref _observer, newObserver, oldObserver) != oldObserver);
        }

        #endregion

        #region IDisposable implementation

        /// <summary>
        /// Releases all resources used by the current instance of the <see cref="Subject{T}"/> class and unsubscribes all observers.
        /// </summary>
        public override void Dispose()
        {
            _observer = DisposedObserver<T>.Instance;
        }

        #endregion

        #endregion
    }
}

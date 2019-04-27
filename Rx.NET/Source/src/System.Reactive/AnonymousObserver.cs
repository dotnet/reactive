// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive
{
    /// <summary>
    /// Class to create an <see cref="IObserver{T}"/> instance from delegate-based implementations of the On* methods.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    public sealed class AnonymousObserver<T> : ObserverBase<T>
    {
        private readonly Action<T> _onNext;
        private readonly Action<Exception> _onError;
        private readonly Action _onCompleted;

        /// <summary>
        /// Creates an observer from the specified <see cref="IObserver{T}.OnNext(T)"/>, <see cref="IObserver{T}.OnError(Exception)"/>, and <see cref="IObserver{T}.OnCompleted()"/> actions.
        /// </summary>
        /// <param name="onNext">Observer's <see cref="IObserver{T}.OnNext(T)"/> action implementation.</param>
        /// <param name="onError">Observer's <see cref="IObserver{T}.OnError(Exception)"/> action implementation.</param>
        /// <param name="onCompleted">Observer's <see cref="IObserver{T}.OnCompleted()"/> action implementation.</param>
        /// <exception cref="ArgumentNullException"><paramref name="onNext"/> or <paramref name="onError"/> or <paramref name="onCompleted"/> is <c>null</c>.</exception>
        public AnonymousObserver(Action<T> onNext, Action<Exception> onError, Action onCompleted)
            : this(onNext, onError, onCompleted, true)
        {
            if (onNext == null)
            {
                throw new ArgumentNullException(nameof(onNext));
            }

            if (onError == null)
            {
                throw new ArgumentNullException(nameof(onError));
            }

            if (onCompleted == null)
            {
                throw new ArgumentNullException(nameof(onCompleted));
            }
        }

        internal AnonymousObserver(Action<T> onNext, Action<Exception> onError, Action onCompleted, bool @internal)
        {
            _onNext = onNext;
            _onError = onError;
            _onCompleted = onCompleted;
        }

        /// <summary>
        /// Creates an observer from the specified <see cref="IObserver{T}.OnNext(T)"/> action.
        /// </summary>
        /// <param name="onNext">Observer's <see cref="IObserver{T}.OnNext(T)"/> action implementation.</param>
        /// <exception cref="ArgumentNullException"><paramref name="onNext"/> is <c>null</c>.</exception>
        public AnonymousObserver(Action<T> onNext)
            : this(onNext, Stubs.Throw, Stubs.Nop, true)
        {
            if (onNext == null)
            {
                throw new ArgumentNullException(nameof(onNext));
            }
        }

        /// <summary>
        /// Creates an observer from the specified <see cref="IObserver{T}.OnNext(T)"/> and <see cref="IObserver{T}.OnError(Exception)"/> actions.
        /// </summary>
        /// <param name="onNext">Observer's <see cref="IObserver{T}.OnNext(T)"/> action implementation.</param>
        /// <param name="onError">Observer's <see cref="IObserver{T}.OnError(Exception)"/> action implementation.</param>
        /// <exception cref="ArgumentNullException"><paramref name="onNext"/> or <paramref name="onError"/> is <c>null</c>.</exception>
        public AnonymousObserver(Action<T> onNext, Action<Exception> onError)
            : this(onNext, onError, Stubs.Nop, true)
        {
            if (onNext == null)
            {
                throw new ArgumentNullException(nameof(onNext));
            }

            if (onError == null)
            {
                throw new ArgumentNullException(nameof(onError));
            }
        }

        /// <summary>
        /// Creates an observer from the specified <see cref="IObserver{T}.OnNext(T)"/> and <see cref="IObserver{T}.OnCompleted()"/> actions.
        /// </summary>
        /// <param name="onNext">Observer's <see cref="IObserver{T}.OnNext(T)"/> action implementation.</param>
        /// <param name="onCompleted">Observer's <see cref="IObserver{T}.OnCompleted()"/> action implementation.</param>
        /// <exception cref="ArgumentNullException"><paramref name="onNext"/> or <paramref name="onCompleted"/> is <c>null</c>.</exception>
        public AnonymousObserver(Action<T> onNext, Action onCompleted)
            : this(onNext, Stubs.Throw, onCompleted, true)
        {
            if (onNext == null)
            {
                throw new ArgumentNullException(nameof(onNext));
            }

            if (onCompleted == null)
            {
                throw new ArgumentNullException(nameof(onCompleted));
            }
        }

        /// <summary>
        /// Calls the action implementing <see cref="IObserver{T}.OnNext(T)"/>.
        /// </summary>
        /// <param name="value">Next element in the sequence.</param>
        protected override void OnNextCore(T value) => _onNext(value);

        /// <summary>
        /// Calls the action implementing <see cref="IObserver{T}.OnError(Exception)"/>.
        /// </summary>
        /// <param name="error">The error that has occurred.</param>
        protected override void OnErrorCore(Exception error) => _onError(error);

        /// <summary>
        /// Calls the action implementing <see cref="IObserver{T}.OnCompleted()"/>.
        /// </summary>
        protected override void OnCompletedCore() => _onCompleted();

        internal ISafeObserver<T> MakeSafe() => new AnonymousSafeObserver<T>(_onNext, _onError, _onCompleted);
    }

    internal static class AnonymousObserver
    {
        public static AnonymousObserver<T> Create_<T>()
        {
            return new AnonymousObserver<T>(Stubs<T>.Ignore, Stubs.Throw, Stubs.Nop, true);
        }

        public static AnonymousObserver<T> Create_<T>(Action<T> onNext)
        {
            return new AnonymousObserver<T>(onNext, Stubs.Throw, Stubs.Nop, true);
        }

        public static AnonymousObserver<T> Create_<T>(Action<T> onNext, Action<Exception> onError)
        {
            return new AnonymousObserver<T>(onNext, onError, Stubs.Nop, true);
        }

        public static AnonymousObserver<T> Create_<T>(Action<T> onNext, Action onCompleted)
        {
            return new AnonymousObserver<T>(onNext, Stubs.Throw, onCompleted, true);
        }

        public static AnonymousObserver<T> Create_<T>(Action<T> onNext, Action<Exception> onError, Action onCompleted)
        {
            return new AnonymousObserver<T>(onNext, onError, onCompleted, true);
        }
    }
}

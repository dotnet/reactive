// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Disposables;

namespace System.Reactive
{
    /// <summary>
    /// Class to create an <see cref="IObservable{T}"/> instance from a delegate-based implementation of the <see cref="IObservable{T}.Subscribe(IObserver{T})"/> method.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    public sealed class AnonymousObservable<T> : ObservableBase<T>
    {
        private readonly Func<IObserver<T>, IDisposable> _subscribe;

        /// <summary>
        /// Creates an observable sequence object from the specified subscription function.
        /// </summary>
        /// <param name="subscribe"><see cref="IObservable{T}.Subscribe(IObserver{T})"/> method implementation.</param>
        /// <exception cref="ArgumentNullException"><paramref name="subscribe"/> is <c>null</c>.</exception>
        public AnonymousObservable(Func<IObserver<T>, IDisposable> subscribe)
        {
            if (subscribe == null)
                throw new ArgumentNullException(nameof(subscribe));

            _subscribe = subscribe;
        }

        /// <summary>
        /// Calls the subscription function that was supplied to the constructor.
        /// </summary>
        /// <param name="observer">Observer to send notifications to.</param>
        /// <returns>Disposable object representing an observer's subscription to the observable sequence.</returns>
        protected override IDisposable SubscribeCore(IObserver<T> observer)
        {
            return _subscribe(observer) ?? Disposable.Empty;
        }

        public static StatefulAnonymousObservable<T, TState> CreateStateful<TState>(Func<IObserver<T>, TState, IDisposable> subscribe, TState state)
        {
            return new StatefulAnonymousObservable<T, TState>(subscribe, state);
        }
    }

    /// <summary>
    /// Class to create an <see cref="IObservable{T}"/> instance from a delegate-based implementation of the <see cref="IObservable{T}.Subscribe(IObserver{T})"/> method.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    /// <typeparam name="TState">The type of the state that is passed to the subscription function.</typeparam>
    public sealed class StatefulAnonymousObservable<T, TState> : ObservableBase<T>
    {
        private readonly TState _state;
        private readonly Func<IObserver<T>, TState, IDisposable> _subscribe;

        /// <summary>
        /// Creates an observable sequence object from the specified subscription function.
        /// </summary>
        /// <param name="subscribe"><see cref="IObservable{T}.Subscribe(IObserver{T})"/> method implementation.</param>
        /// <param name="state">The state to pass to the subscription function.</param>
        /// <exception cref="ArgumentNullException"><paramref name="subscribe"/> is <c>null</c>.</exception>
        public StatefulAnonymousObservable(Func<IObserver<T>, TState, IDisposable> subscribe, TState state)
        {
            if (subscribe == null)
                throw new ArgumentNullException(nameof(subscribe));

            _state = state;
            _subscribe = subscribe;
        }

        /// <summary>
        /// Calls the subscription function that was supplied to the constructor.
        /// </summary>
        /// <param name="observer">Observer to send notifications to.</param>
        /// <returns>Disposable object representing an observer's subscription to the observable sequence.</returns>
        protected override IDisposable SubscribeCore(IObserver<T> observer)
        {
            return _subscribe(observer, this._state) ?? Disposable.Empty;
        }
    }
}

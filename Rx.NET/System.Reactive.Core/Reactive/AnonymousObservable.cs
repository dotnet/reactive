// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System.Reactive.Disposables;

namespace System.Reactive
{
    /// <summary>
    /// Class to create an IObservable&lt;T&gt; instance from a delegate-based implementation of the Subscribe method.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    public sealed class AnonymousObservable<T> : ObservableBase<T>
    {
        private readonly Func<IObserver<T>, IDisposable> _subscribe;

        /// <summary>
        /// Creates an observable sequence object from the specified subscription function.
        /// </summary>
        /// <param name="subscribe">Subscribe method implementation.</param>
        /// <exception cref="ArgumentNullException"><paramref name="subscribe"/> is null.</exception>
        public AnonymousObservable(Func<IObserver<T>, IDisposable> subscribe)
        {
            if (subscribe == null)
                throw new ArgumentNullException("subscribe");

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
    }
}

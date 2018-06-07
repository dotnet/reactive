// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System.Reactive.Disposables;

namespace System.Reactive
{

    /// <summary>
    /// Weak reference Observable
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class WeakObservable<T> : IObservable<T>
    {
        private readonly IObservable<T> _source;

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="WeakObservable{T}"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <exception cref="System.ArgumentNullException">source</exception>
        public WeakObservable(IObservable<T> source)
        {
            #region Validation

            if (source == null)
                throw new ArgumentNullException(nameof(source));

            #endregion Validation

            _source = source;
        }

        #endregion Ctor

        #region Subscribe

        /// <summary>
        /// Subscribes the specified observer.
        /// </summary>
        /// <param name="observer">The observer.</param>
        /// <returns></returns>
        public IDisposable Subscribe(IObserver<T> observer)
        {
            IObservable<T> source = _source;
            if (source == null)
                return Disposable.Empty;
            var weakObserver = new WeakObserver<T>(observer);
            IDisposable disp = source.Subscribe(weakObserver);
            return disp;
        }

        #endregion Subscribe
    }
}

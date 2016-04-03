// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System.Reactive.PlatformServices;

namespace System.Reactive
{
    /// <summary>
    /// Weak reference Observer
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class WeakObserver<T> : IObserver<T>
    {
        private readonly WeakReference<IObserver<T>> _target;

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="WeakObserver{T}"/> class.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <exception cref="System.ArgumentNullException">target</exception>
        public WeakObserver(IObserver<T> target)
        {
            #region Validation

            if (target == null)
                throw new ArgumentNullException(nameof(target));

            #endregion Validation

            _target = new WeakReference<IObserver<T>>(target);
        }

        #endregion Ctor

        #region Target

        /// <summary>
        /// Gets the target.
        /// </summary>
        /// <value>
        /// The target.
        /// </value>
        private IObserver<T> Target
        {
            get
            {
                IObserver<T> target;
                if (_target.TryGetTarget(out target))
                    return target;
                return null;
            }
        }

        #endregion Target

        #region IObserver<T> Members

        #region OnCompleted

        /// <summary>
        /// Notifies the observer that the provider has finished sending push-based notifications.
        /// </summary>
        public void OnCompleted()
        {
            IObserver<T> target = Target;
            if (target == null)
                return;

            target.OnCompleted();
        }

        #endregion OnCompleted

        #region OnError

        /// <summary>
        /// Notifies the observer that the provider has experienced an error condition.
        /// </summary>
        /// <param name="error">An object that provides additional information about the error.</param>
        public void OnError(Exception error)
        {
            IObserver<T> target = Target;
            if (target == null)
                return;

            target.OnError(error);
        }

        #endregion OnError

        #region OnNext

        /// <summary>
        /// Provides the observer with new data.
        /// </summary>
        /// <param name="value">The current notification information.</param>
        public void OnNext(T value)
        {
            IObserver<T> target = Target;
            if (target == null)
                return;

            target.OnNext(value);
        }

        #endregion OnNext

        #endregion IObserver<T> Members
    }
}

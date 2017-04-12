// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Runtime.CompilerServices;

namespace System.Reactive
{
    /// <summary>
    /// Implementation of the IObservable&lt;T&gt; interface compatible with async method return types.
    /// </summary>
    /// <remarks>
    /// This class implements a "task-like" type that can be used as the return type of an asynchronous
    /// method in C# 7.0 and beyond. For example:
    /// <code>
    /// async Observable&lt;int&gt; RxAsync()
    /// {
    ///     var res = await Observable.Return(21).Delay(TimeSpan.FromSeconds(1));
    ///     return res * 2;
    /// }
    /// </code>
    /// </remarks>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    [AsyncMethodBuilder(typeof(ObservableMethodBuilder<>))]
    public sealed class Observable<T> : IObservable<T>
    {
        /// <summary>
        /// The underlying observable sequence to subscribe to.
        /// </summary>
        private readonly IObservable<T> _inner;

        /// <summary>
        /// Creates a new task-like observable instance using the specified <paramref name="inner"/> observable sequence.
        /// </summary>
        /// <param name="inner">The underlying observable sequence to subscribe to.</param>
        internal Observable(IObservable<T> inner)
        {
            _inner = inner;
        }

        /// <summary>
        /// Subscribes the given observer to the observable sequence.
        /// </summary>
        /// <param name="observer">Observer that will receive notifications from the observable sequence.</param>
        /// <returns>Disposable object representing an observer's subscription to the observable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="observer"/> is null.</exception>
        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            return _inner.Subscribe(observer);
        }
    }
}

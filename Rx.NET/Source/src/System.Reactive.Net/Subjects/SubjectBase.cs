// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive.Subjects
{
    /// <summary>
    /// Base class for objects that are both an observable sequence as well as an observer.
    /// </summary>
    /// <typeparam name="T">The type of the elements processed by the subject.</typeparam>
#pragma warning disable CA1063 // (Overridable IDisposable.) This analyzer wants us to make breaking changes to its public API, which we can't do.
    public abstract class SubjectBase<T> : ISubject<T>, IDisposable
    {
        /// <summary>
        /// Indicates whether the subject has observers subscribed to it.
        /// </summary>
        public abstract bool HasObservers { get; }

        /// <summary>
        /// Indicates whether the subject has been disposed.
        /// </summary>
        public abstract bool IsDisposed { get; }

        /// <summary>
        /// Releases all resources used by the current instance of the subject and unsubscribes all observers.
        /// </summary>
        public abstract void Dispose();

        /// <summary>
        /// Notifies all subscribed observers about the end of the sequence.
        /// </summary>
        public abstract void OnCompleted();

        /// <summary>
        /// Notifies all subscribed observers about the specified exception.
        /// </summary>
        /// <param name="error">The exception to send to all currently subscribed observers.</param>
        /// <exception cref="ArgumentNullException"><paramref name="error"/> is <c>null</c>.</exception>
#pragma warning disable CA1716 // (Identifiers should not match keywords.) This has been the name for years, so the (admittedly small) risk from changing it doesn't seem to offer a meaningful benefit.
        public abstract void OnError(Exception error);
#pragma warning restore CA1716

        /// <summary>
        /// Notifies all subscribed observers about the arrival of the specified element in the sequence.
        /// </summary>
        /// <param name="value">The value to send to all currently subscribed observers.</param>
        public abstract void OnNext(T value);

        /// <summary>
        /// Subscribes an observer to the subject.
        /// </summary>
        /// <param name="observer">Observer to subscribe to the subject.</param>
        /// <returns>Disposable object that can be used to unsubscribe the observer from the subject.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="observer"/> is <c>null</c>.</exception>
        public abstract IDisposable Subscribe(IObserver<T> observer);
    }
#pragma warning restore CA1063
}

// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System.Threading;

namespace System.Reactive
{
    /// <summary>
    /// Abstract base class for implementations of the IObserver&lt;T&gt; interface.
    /// </summary>
    /// <remarks>This base class enforces the grammar of observers where OnError and OnCompleted are terminal messages.</remarks>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    public abstract class ObserverBase<T> : IObserver<T>, IDisposable
    {
        private int isStopped;

        /// <summary>
        /// Creates a new observer in a non-stopped state.
        /// </summary>
        protected ObserverBase()
        {
            isStopped = 0;
        }

        /// <summary>
        /// Notifies the observer of a new element in the sequence.
        /// </summary>
        /// <param name="value">Next element in the sequence.</param>
        public void OnNext(T value)
        {
            if (isStopped == 0)
                OnNextCore(value);
        }

        /// <summary>
        /// Implement this method to react to the receival of a new element in the sequence.
        /// </summary>
        /// <param name="value">Next element in the sequence.</param>
        /// <remarks>This method only gets called when the observer hasn't stopped yet.</remarks>
        protected abstract void OnNextCore(T value);

        /// <summary>
        /// Notifies the observer that an exception has occurred.
        /// </summary>
        /// <param name="error">The error that has occurred.</param>
        /// <exception cref="ArgumentNullException"><paramref name="error"/> is null.</exception>
        public void OnError(Exception error)
        {
            if (error == null)
                throw new ArgumentNullException("error");

            if (Interlocked.Exchange(ref isStopped, 1) == 0)
            {
                OnErrorCore(error);
            }
        }

        /// <summary>
        /// Implement this method to react to the occurrence of an exception.
        /// </summary>
        /// <param name="error">The error that has occurred.</param>
        /// <remarks>This method only gets called when the observer hasn't stopped yet, and causes the observer to stop.</remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Error", Justification = "Same name as in the IObserver<T> definition of OnError in the BCL.")]
        protected abstract void OnErrorCore(Exception error);

        /// <summary>
        /// Notifies the observer of the end of the sequence.
        /// </summary>
        public void OnCompleted()
        {
            if (Interlocked.Exchange(ref isStopped, 1) == 0)
            {
                OnCompletedCore();
            }
        }

        /// <summary>
        /// Implement this method to react to the end of the sequence.
        /// </summary>
        /// <remarks>This method only gets called when the observer hasn't stopped yet, and causes the observer to stop.</remarks>
        protected abstract void OnCompletedCore();

        /// <summary>
        /// Disposes the observer, causing it to transition to the stopped state.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Core implementation of IDisposable.
        /// </summary>
        /// <param name="disposing">true if the Dispose call was triggered by the IDisposable.Dispose method; false if it was triggered by the finalizer.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                isStopped = 1;
            }
        }

        internal bool Fail(Exception error)
        {
            if (Interlocked.Exchange(ref isStopped, 1) == 0)
            {
                OnErrorCore(error);
                return true;
            }

            return false;
        }
    }
}

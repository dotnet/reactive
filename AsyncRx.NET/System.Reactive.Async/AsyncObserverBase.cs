// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive
{
    /// <summary>
    /// Base class for observers, enforcing the observer grammar on the notifications it receives.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    /// <remarks>
    /// <para>
    /// The grammar requires notifications to be serialized: a notification must not be delivered
    /// until the task returned for the previous one has completed, and nothing may follow
    /// <see cref="OnErrorAsync"/> or <see cref="OnCompletedAsync"/>. This class refuses the first
    /// kind of violation and tolerates the second, as Rx.NET's <c>ObserverBase</c> does. A
    /// notification arriving while a previous <see cref="OnNextAsync"/> is still being delivered,
    /// whether that delivery is in its synchronous part or has yielded, throws
    /// <see cref="InvalidOperationException"/> to its caller. A notification arriving once a
    /// terminal one has been accepted is dropped, because a source cannot always see that its
    /// observer has been shut down from elsewhere, and the terminal notification is accepted as
    /// soon as it arrives rather than when its delivery completes, so that the late notification
    /// is dropped and not refused.
    /// </para>
    /// <para>
    /// The refusals throw synchronously rather than returning a faulted task, so that a caller
    /// which does not await the result still sees them. A delivery that completes synchronously
    /// returns the task <c>OnNextAsyncCore</c> returned, without allocating.
    /// </para>
    /// </remarks>
    public abstract class AsyncObserverBase<T> : IAsyncObserver<T>
    {
        private const int Idle = 0;
        private const int Busy = 1;
        private const int Done = 2;

        private int _status = Idle;

        public ValueTask OnCompletedAsync()
        {
            if (!TryMoveTo(Done))
            {
                return default;
            }

            return OnCompletedAsyncCore();
        }

        protected abstract ValueTask OnCompletedAsyncCore();

        public ValueTask OnErrorAsync(Exception error)
        {
            if (error == null)
                throw new ArgumentNullException(nameof(error));

            if (!TryMoveTo(Done))
            {
                return default;
            }

            return OnErrorAsyncCore(error);
        }

        protected abstract ValueTask OnErrorAsyncCore(Exception error);

        public ValueTask OnNextAsync(T value)
        {
            if (!TryMoveTo(Busy))
            {
                return default;
            }

            ValueTask delivery;

            try
            {
                delivery = OnNextAsyncCore(value);
            }
            catch
            {
                Interlocked.Exchange(ref _status, Idle);
                throw;
            }

            if (delivery.IsCompleted)
            {
                Interlocked.Exchange(ref _status, Idle);
                return delivery;
            }

            return LeaveWhenDelivered(delivery);
        }

        protected abstract ValueTask OnNextAsyncCore(T value);

        private async ValueTask LeaveWhenDelivered(ValueTask delivery)
        {
            try
            {
                await delivery.ConfigureAwait(false);
            }
            finally
            {
                Interlocked.Exchange(ref _status, Idle);
            }
        }

        // Moves from Idle to the given state. Busy means a previous OnNextAsync is still being
        // delivered, which the caller has broken the grammar by overlapping; Done means a
        // terminal notification has been accepted, after which anything further is dropped.
        private bool TryMoveTo(int status)
        {
            var old = Interlocked.CompareExchange(ref _status, status, Idle);

            switch (old)
            {
                case Busy:
                    throw new InvalidOperationException("The observer is currently processing a notification.");
                case Done:
                    return false;
            }

            return true;
        }
    }
}

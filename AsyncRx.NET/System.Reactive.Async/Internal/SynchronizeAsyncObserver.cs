// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive
{
    // Synchronize's observer: every notification is delivered under one gate, so a source that
    // pushes from several threads at once, or re-enters, reaches the downstream observer one
    // notification at a time, as Rx.NET's SynchronizedObserver serializes under a monitor. This
    // is the one observer in the library that must accept a notification arriving while another
    // is in progress, since correcting that is the operator's purpose; so it implements the
    // interface itself rather than deriving from AsyncObserverBase, whose guard rejects such a
    // call before any gate could see it. What happens after a terminal notification is the
    // downstream observer's business, as in Rx.NET: the auto-detaching observer every
    // subscription gets drops anything that follows one.
    internal sealed class SynchronizeAsyncObserver<T> : IAsyncObserver<T>
    {
        private readonly IAsyncObserver<T> _observer;
        private readonly AsyncGate _gate;

        public SynchronizeAsyncObserver(IAsyncObserver<T> observer, AsyncGate gate)
        {
            _observer = observer;
            _gate = gate;
        }

        public async ValueTask OnNextAsync(T value)
        {
            using (await _gate.LockAsync().ConfigureAwait(false))
            {
                await _observer.OnNextAsync(value).ConfigureAwait(false);
            }
        }

        public async ValueTask OnErrorAsync(Exception error)
        {
            if (error == null)
                throw new ArgumentNullException(nameof(error));

            using (await _gate.LockAsync().ConfigureAwait(false))
            {
                await _observer.OnErrorAsync(error).ConfigureAwait(false);
            }
        }

        public async ValueTask OnCompletedAsync()
        {
            using (await _gate.LockAsync().ConfigureAwait(false))
            {
                await _observer.OnCompletedAsync().ConfigureAwait(false);
            }
        }
    }
}

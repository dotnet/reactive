// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive.Disposables
{
    public sealed class SerialAsyncDisposable : IAsyncDisposable
    {
        private readonly AsyncLock _gate = new AsyncLock();

        private IAsyncDisposable _disposable;
        private bool _disposed;

        public async Task AssignAsync(IAsyncDisposable disposable)
        {
            if (disposable == null)
                throw new ArgumentNullException(nameof(disposable));

            var shouldDispose = false;
            var old = default(IAsyncDisposable);

            using (await _gate.LockAsync().ConfigureAwait(false))
            {
                if (_disposed)
                {
                    shouldDispose = true;
                }
                else
                {
                    old = _disposable;
                    _disposable = disposable;
                }
            }

            if (old != null)
            {
                await old.DisposeAsync().ConfigureAwait(false);
            }

            if (shouldDispose && disposable != null)
            {
                await disposable.DisposeAsync().ConfigureAwait(false);
            }
        }

        public async Task DisposeAsync()
        {
            var old = default(IAsyncDisposable);

            using (await _gate.LockAsync().ConfigureAwait(false))
            {
                if (!_disposed)
                {
                    _disposed = true;
                    old = _disposable;
                    _disposable = null;
                }
            }

            if (old != null)
            {
                await old.DisposeAsync().ConfigureAwait(false);
            }
        }
    }
}

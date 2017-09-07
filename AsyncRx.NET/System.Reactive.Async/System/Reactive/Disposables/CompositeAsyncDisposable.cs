// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive.Disposables
{
    public sealed class CompositeAsyncDisposable : IAsyncDisposable
    {
        private readonly AsyncLock gate = new AsyncLock();
        private readonly List<IAsyncDisposable> disposables = new List<IAsyncDisposable>();
        private bool disposed;

        public async Task AddAsync(IAsyncDisposable disposable)
        {
            if (disposable == null)
                throw new ArgumentNullException(nameof(disposable));

            using (await gate.LockAsync().ConfigureAwait(false))
            {
                if (disposed)
                {
                    await disposable.DisposeAsync().ConfigureAwait(false);
                }
                else
                {
                    disposables.Add(disposable);
                }
            }
        }

        public async Task RemoveAsync(IAsyncDisposable disposable)
        {
            if (disposable == null)
                throw new ArgumentNullException(nameof(disposable));

            using (await gate.LockAsync().ConfigureAwait(false))
            {
                if (!disposables.Remove(disposable))
                    throw new InvalidOperationException("Disposable not found.");

                await disposable.DisposeAsync().ConfigureAwait(false);
            }
        }

        public async Task DisposeAsync()
        {
            using (await gate.LockAsync().ConfigureAwait(false))
            {
                if (disposed)
                    return;

                disposed = true;

                var tasks = disposables.Select(disposable => disposable.DisposeAsync());

                await Task.WhenAll(tasks);
            }
        }
    }
}

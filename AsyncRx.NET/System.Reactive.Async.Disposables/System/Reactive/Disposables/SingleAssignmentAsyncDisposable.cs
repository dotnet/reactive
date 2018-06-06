// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive.Disposables
{
    public sealed class SingleAssignmentAsyncDisposable : IAsyncDisposable
    {
        private static readonly IAsyncDisposable Disposed = AsyncDisposable.Create(() => Task.CompletedTask);

        private IAsyncDisposable _disposable;

        public async Task AssignAsync(IAsyncDisposable disposable)
        {
            if (disposable == null)
                throw new ArgumentNullException(nameof(disposable));

            var old = Interlocked.CompareExchange(ref _disposable, disposable, null);

            if (old == null)
                return;

            if (old != Disposed)
                throw new InvalidOperationException("Disposable already assigned.");

            await disposable.DisposeAsync().ConfigureAwait(false);
        }

        public Task DisposeAsync()
        {
            return Interlocked.Exchange(ref _disposable, Disposed)?.DisposeAsync() ?? Task.CompletedTask;
        }
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive.Disposables
{
    public sealed class SingleAssignmentAsyncDisposable : IAsyncDisposable
    {
        private static readonly IAsyncDisposable Disposed = AsyncDisposable.Create(() => default);

        private IAsyncDisposable _disposable;

        public async ValueTask AssignAsync(IAsyncDisposable disposable)
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

        public ValueTask DisposeAsync()
        {
            return Interlocked.Exchange(ref _disposable, Disposed)?.DisposeAsync() ?? default;
        }
    }
}

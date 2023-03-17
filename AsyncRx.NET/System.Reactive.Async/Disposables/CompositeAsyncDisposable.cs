// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive.Disposables
{
    public sealed class CompositeAsyncDisposable : IAsyncDisposable
    {
        private readonly AsyncGate _gate = new();
        private readonly List<IAsyncDisposable> _disposables;
        private bool _disposed;

        public CompositeAsyncDisposable()
        {
            _disposables = new List<IAsyncDisposable>();
        }

        public CompositeAsyncDisposable(params IAsyncDisposable[] disposables)
        {
            if (disposables == null)
                throw new ArgumentNullException(nameof(disposables));

            _disposables = new List<IAsyncDisposable>(disposables);
        }

        public CompositeAsyncDisposable(IEnumerable<IAsyncDisposable> disposables)
        {
            if (disposables == null)
                throw new ArgumentNullException(nameof(disposables));

            _disposables = new List<IAsyncDisposable>(disposables);
        }

        public async ValueTask AddAsync(IAsyncDisposable disposable)
        {
            if (disposable == null)
                throw new ArgumentNullException(nameof(disposable));

            var shouldDispose = false;

            using (await _gate.LockAsync().ConfigureAwait(false))
            {
                if (_disposed)
                {
                    shouldDispose = true;
                }
                else
                {
                    _disposables.Add(disposable);
                }
            }

            if (shouldDispose)
            {
                await disposable.DisposeAsync().ConfigureAwait(false);
            }
        }

        public async ValueTask<bool> RemoveAsync(IAsyncDisposable disposable)
        {
            if (disposable == null)
                throw new ArgumentNullException(nameof(disposable));

            var shouldDispose = false;

            using (await _gate.LockAsync().ConfigureAwait(false))
            {
                if (!_disposed && _disposables.Remove(disposable))
                {
                    shouldDispose = true;
                }
            }

            if (shouldDispose)
            {
                await disposable.DisposeAsync().ConfigureAwait(false);
            }

            return shouldDispose;
        }

        public async ValueTask DisposeAsync()
        {
            var disposables = default(IAsyncDisposable[]);

            using (await _gate.LockAsync().ConfigureAwait(false))
            {
                if (!_disposed)
                {
                    _disposed = true;

                    disposables = _disposables.ToArray();
                    _disposables.Clear();
                }
            }

            if (disposables != null)
            {
                var tasks = disposables.Select(disposable => disposable.DisposeAsync().AsTask());

                await Task.WhenAll(tasks);
            }
        }
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive.Disposables
{
    public sealed class RefCountAsyncDisposable : IAsyncDisposable
    {
        private readonly AsyncGate _gate = new();
        private IAsyncDisposable _disposable;
        private bool _primaryDisposed;
        private int _count;

        public RefCountAsyncDisposable(IAsyncDisposable disposable)
        {
            _disposable = disposable ?? throw new ArgumentNullException(nameof(disposable));
            _primaryDisposed = false;
            _count = 0;
        }

        public async ValueTask<IAsyncDisposable> GetDisposableAsync()
        {
            using (await _gate.LockAsync().ConfigureAwait(false))
            {
                if (_disposable == null)
                {
                    return AsyncDisposable.Nop;
                }
                else
                {
                    _count++;
                    return new Inner(this);
                }
            }
        }

        public async ValueTask DisposeAsync()
        {
            var disposable = default(IAsyncDisposable);

            using (await _gate.LockAsync().ConfigureAwait(false))
            {
                if (_disposable != null && !_primaryDisposed)
                {
                    _primaryDisposed = true;

                    if (_count == 0)
                    {
                        disposable = _disposable;
                        _disposable = null;
                    }
                }
            }

            if (disposable != null)
            {
                await disposable.DisposeAsync().ConfigureAwait(false);
            }
        }

        private async ValueTask ReleaseAsync()
        {
            var disposable = default(IAsyncDisposable);

            using (await _gate.LockAsync().ConfigureAwait(false))
            {
                if (_disposable != null)
                {
                    _count--;

                    if (_primaryDisposed && _count == 0)
                    {
                        disposable = _disposable;
                        _disposable = null;
                    }
                }
            }

            if (disposable != null)
            {
                await disposable.DisposeAsync().ConfigureAwait(false);
            }
        }

        private sealed class Inner : IAsyncDisposable
        {
            private RefCountAsyncDisposable _parent;

            public Inner(RefCountAsyncDisposable parent)
            {
                _parent = parent;
            }

            public ValueTask DisposeAsync() => Interlocked.Exchange(ref _parent, null)?.ReleaseAsync() ?? default;
        }
    }
}

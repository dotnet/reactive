// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Diagnostics;
using System.Threading.Tasks;

namespace System.Threading
{
    public sealed class AsyncGate
    {
        private readonly object _gate = new();
        private readonly SemaphoreSlim _semaphore = new(1, 1);
        private readonly AsyncLocal<int> _recursionCount = new();

        public ValueTask<Releaser> LockAsync()
        {
            var shouldAcquire = false;

            lock (_gate)
            {
                if (_recursionCount.Value == 0)
                {
                    shouldAcquire = true;
                    _recursionCount.Value = 1;
                }
                else
                {
                    _recursionCount.Value++;
                }
            }

            if (shouldAcquire)
            {
                return new ValueTask<Releaser>(_semaphore.WaitAsync().ContinueWith(_ => new Releaser(this)));
            }

            return new ValueTask<Releaser>(new Releaser(this));
        }

        private void Release()
        {
            lock (_gate)
            {
                Debug.Assert(_recursionCount.Value > 0);

                if (--_recursionCount.Value == 0)
                {
                    _semaphore.Release();
                }
            }
        }

        public readonly struct Releaser : IDisposable
        {
            private readonly AsyncGate _parent;

            public Releaser(AsyncGate parent) => _parent = parent;

            public void Dispose() => _parent.Release();
        }
    }
}

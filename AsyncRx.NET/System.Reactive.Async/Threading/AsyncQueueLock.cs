// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Threading.Tasks;

namespace System.Threading
{
    public sealed class AsyncQueueLock : IAsyncDisposable
    {
        private readonly Queue<Func<ValueTask>> _queue = new();
        private readonly AsyncGate _gate = new();

        private bool _isAcquired;
        private bool _hasFaulted;

        public async ValueTask WaitAsync(Func<ValueTask> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            var shouldRun = false;

            using (await _gate.LockAsync().ConfigureAwait(false))
            {
                if (!_hasFaulted)
                {
                    _queue.Enqueue(action);
                    shouldRun = !_isAcquired;
                    _isAcquired = true;
                }
            }

            if (shouldRun)
            {
                while (true)
                {
                    var next = default(Func<ValueTask>);

                    using (await _gate.LockAsync().ConfigureAwait(false))
                    {
                        if (_queue.Count == 0)
                        {
                            _isAcquired = false;
                            break;
                        }

                        next = _queue.Dequeue();
                    }

                    try
                    {
                        await next().ConfigureAwait(false);
                    }
                    catch
                    {
                        using (await _gate.LockAsync().ConfigureAwait(false))
                        {
                            _queue.Clear();
                            _hasFaulted = true;
                        }

                        throw;
                    }
                }
            }
        }

        public async ValueTask DisposeAsync()
        {
            var queue = _queue;

            using (await _gate.LockAsync().ConfigureAwait(false))
            {
                queue.Clear();
                _hasFaulted = true;
            }
        }
    }
}

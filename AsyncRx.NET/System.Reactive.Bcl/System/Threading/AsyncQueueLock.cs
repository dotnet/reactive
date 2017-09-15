// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Threading.Tasks;

namespace System.Threading
{
    public sealed class AsyncQueueLock : IAsyncDisposable
    {
        private readonly Queue<Func<Task>> _queue = new Queue<Func<Task>>();
        private readonly AsyncLock _gate = new AsyncLock();

        private bool _isAcquired;
        private bool _hasFaulted;

        public async Task WaitAsync(Func<Task> action)
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
                    var next = default(Func<Task>);

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

        public async Task DisposeAsync()
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

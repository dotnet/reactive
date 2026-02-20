// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive.Threading
{
    /// <summary>
    /// Provides an implementation of <see cref="IAsyncGate"/>, enabling mutually exclusive locking
    /// in async code.
    /// </summary>
    public sealed class AsyncGate : IAsyncGate, IAsyncGateReleaser
    {
        private readonly object _gate = new();
        private readonly SemaphoreSlim _semaphore = new(1, 1);
        private readonly AsyncLocal<int> _recursionCount = new();

        /// <summary>
        /// Creates an <see cref="AsyncGate"/>.
        /// </summary>
        /// <remarks>
        /// This is private because we hope that one day, the .NET runtime will provide a built-in
        /// asynchronous mutual exclusion primitive, and that we might be able to use that instead of
        /// our own implementation. Although that might be something we could do by modifying this
        /// class, it might prove useful to be able to provide the old implementation for backwards
        /// compatibility, so we don't want AsyncRx.NET consumers to depend on a specific concrete type
        /// as the <see cref="IAsyncGate"/> implementation.
        /// </remarks>
        private AsyncGate()
        {
        }

        /// <summary>
        /// Creates a new instance of an <see cref="IAsyncGate"/> implementation.
        /// </summary>
        /// <returns></returns>
        public static IAsyncGate Create() => new AsyncGate();

        ValueTask<IAsyncGateReleaser> IAsyncGate.AcquireAsync()
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
                Task acquireTask = _semaphore.WaitAsync();
                if (acquireTask.IsCompleted)
                {
                    return new ValueTask<IAsyncGateReleaser>(this);
                }
                return new ValueTask<IAsyncGateReleaser>(acquireTask.ContinueWith<IAsyncGateReleaser>(_ => this));
            }

            return new ValueTask<IAsyncGateReleaser>(this);
        }

        void IAsyncGateReleaser.Release()
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
    }
}

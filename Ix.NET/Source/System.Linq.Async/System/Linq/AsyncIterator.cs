// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    internal abstract partial class AsyncIterator<TSource> : IAsyncEnumerable<TSource>, IAsyncEnumerator<TSource>
    {
        private readonly int _threadId;

        protected TSource current;
        protected AsyncIteratorState state = AsyncIteratorState.New;
        protected CancellationToken cancellationToken;

        protected AsyncIterator()
        {
            _threadId = Environment.CurrentManagedThreadId;
        }

        public IAsyncEnumerator<TSource> GetAsyncEnumerator(CancellationToken cancellationToken)
        {
            var enumerator = state == AsyncIteratorState.New && _threadId == Environment.CurrentManagedThreadId
                ? this
                : Clone();

            enumerator.state = AsyncIteratorState.Allocated;
            enumerator.cancellationToken = cancellationToken;

            return enumerator;
        }

        public virtual ValueTask DisposeAsync()
        {
            current = default;
            state = AsyncIteratorState.Disposed;

            return TaskExt.CompletedTask;
        }

        public TSource Current => current;

        public async ValueTask<bool> MoveNextAsync()
        {
            // Note: MoveNext *must* be implemented as an async method to ensure
            // that any exceptions thrown from the MoveNextCore call are handled 
            // by the try/catch, whether they're sync or async

            if (state == AsyncIteratorState.Disposed)
            {
                return false;
            }

            try
            {
                return await MoveNextCore().ConfigureAwait(false);
            }
            catch
            {
                await DisposeAsync().ConfigureAwait(false);
                throw;
            }
        }

        public abstract AsyncIterator<TSource> Clone();

        protected abstract ValueTask<bool> MoveNextCore();
    }

    internal enum AsyncIteratorState
    {
        New = 0,
        Allocated = 1,
        Iterating = 2,
        Disposed = -1,
    }
}

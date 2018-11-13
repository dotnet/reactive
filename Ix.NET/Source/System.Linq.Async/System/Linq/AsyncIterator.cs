// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    internal abstract class AsyncIteratorBase<TSource> : IAsyncEnumerable<TSource>, IAsyncEnumerator<TSource>
    {
        private readonly int _threadId;

        protected AsyncIteratorState _state = AsyncIteratorState.New;
        protected CancellationToken _cancellationToken;

        protected AsyncIteratorBase()
        {
            _threadId = Environment.CurrentManagedThreadId;
        }

        public IAsyncEnumerator<TSource> GetAsyncEnumerator(CancellationToken cancellationToken)
        {
            var enumerator = _state == AsyncIteratorState.New && _threadId == Environment.CurrentManagedThreadId
                ? this
                : Clone();

            enumerator._state = AsyncIteratorState.Allocated;
            enumerator._cancellationToken = cancellationToken;

            // REVIEW: If the final interface contains a CancellationToken here, should we check for a cancellation request
            //         either here or in the first call to MoveNextAsync?

            return enumerator;
        }

        public virtual ValueTask DisposeAsync()
        {
            _state = AsyncIteratorState.Disposed;

            return default;
        }

        public abstract TSource Current { get; }

        public async ValueTask<bool> MoveNextAsync()
        {
            // Note: MoveNext *must* be implemented as an async method to ensure
            // that any exceptions thrown from the MoveNextCore call are handled 
            // by the try/catch, whether they're sync or async

            if (_state == AsyncIteratorState.Disposed)
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

    internal abstract partial class AsyncIterator<TSource> : AsyncIteratorBase<TSource>
    {
        protected TSource _current;

        public override TSource Current => _current;

        public override ValueTask DisposeAsync()
        {
            _current = default;

            return base.DisposeAsync();
        }
    }

    internal enum AsyncIteratorState
    {
        New = 0,
        Allocated = 1,
        Iterating = 2,
        Disposed = -1,
    }
}

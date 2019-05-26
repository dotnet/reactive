// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    // REVIEW: The base class below was introduced to avoid the overhead of storing a field of type TSource if the
    //         value of the iterator can trivially be inferred from another field (e.g. in Repeat). It is also used
    //         by the Defer operator in System.Interactive.Async. For some operators such as Where, Skip, Take, and
    //         Concat, it could be used to retrieve the value from the underlying enumerator. However, performance
    //         of this approach is a bit worse in some cases, so we don't go ahead with it for now. One decision to
    //         make is whether it's okay for Current to throw an exception when MoveNextAsync returns false, e.g.
    //         by omitting a null check for an enumerator field.

    internal abstract partial class AsyncIteratorBase<TSource> : IAsyncEnumerable<TSource>, IAsyncEnumerator<TSource>
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
            cancellationToken.ThrowIfCancellationRequested(); // NB: [LDM-2018-11-28] Equivalent to async iterator behavior.

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

        public abstract AsyncIteratorBase<TSource> Clone();

        protected abstract ValueTask<bool> MoveNextCore();
    }

    internal abstract class AsyncIterator<TSource> : AsyncIteratorBase<TSource>
    {
        protected TSource _current = default!;

        public override TSource Current => _current;

        public override ValueTask DisposeAsync()
        {
            _current = default!;

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

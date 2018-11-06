// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    internal abstract class AsyncIterator<TSource> : IAsyncEnumerable<TSource>, IAsyncEnumerator<TSource>
    {
        private readonly int threadId;

        private bool currentIsInvalid = true;

        internal TSource current;
        internal AsyncIteratorState state = AsyncIteratorState.New;
        internal CancellationToken cancellationToken;

        protected AsyncIterator()
        {
            threadId = Environment.CurrentManagedThreadId;
        }

        public IAsyncEnumerator<TSource> GetAsyncEnumerator(CancellationToken cancellationToken)
        {
            var enumerator = state == AsyncIteratorState.New && threadId == Environment.CurrentManagedThreadId
                ? this
                : Clone();

            enumerator.state = AsyncIteratorState.Allocated;
            enumerator.cancellationToken = cancellationToken;

            try
            {
                enumerator.OnGetEnumerator(cancellationToken);
            }
            catch
            {
                enumerator.DisposeAsync(); // REVIEW: fire-and-forget?
                throw;
            }

            return enumerator;
        }

        public virtual ValueTask DisposeAsync()
        {
            current = default(TSource);
            state = AsyncIteratorState.Disposed;

            return TaskExt.CompletedTask;
        }

        public TSource Current
        {
            get
            {
                if (currentIsInvalid)
                    throw new InvalidOperationException("Enumerator is in an invalid state");

                return current;
            }
        }

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
                var result = await MoveNextCore(cancellationToken).ConfigureAwait(false);

                currentIsInvalid = !result; // if move next is false, invalid otherwise valid

                return result;
            }
            catch
            {
                currentIsInvalid = true;
                await DisposeAsync().ConfigureAwait(false);
                throw;
            }
        }

        public abstract AsyncIterator<TSource> Clone();

        public virtual IAsyncEnumerable<TResult> Select<TResult>(Func<TSource, TResult> selector)
        {
            return new AsyncEnumerable.SelectEnumerableAsyncIterator<TSource, TResult>(this, selector);
        }

        public virtual IAsyncEnumerable<TResult> Select<TResult>(Func<TSource, Task<TResult>> selector)
        {
            return new AsyncEnumerable.SelectEnumerableAsyncIteratorWithTask<TSource, TResult>(this, selector);
        }

        public virtual IAsyncEnumerable<TSource> Where(Func<TSource, bool> predicate)
        {
            return new AsyncEnumerable.WhereEnumerableAsyncIterator<TSource>(this, predicate);
        }

        public virtual IAsyncEnumerable<TSource> Where(Func<TSource, Task<bool>> predicate)
        {
            return new AsyncEnumerable.WhereEnumerableAsyncIteratorWithTask<TSource>(this, predicate);
        }

        protected abstract ValueTask<bool> MoveNextCore(CancellationToken cancellationToken);

        protected virtual void OnGetEnumerator(CancellationToken cancellationToken)
        {
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

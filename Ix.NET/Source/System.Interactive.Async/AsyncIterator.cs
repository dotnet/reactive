// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    internal abstract class AsyncIterator<TSource> : IAsyncEnumerable<TSource>, IAsyncEnumerator<TSource>
    {
        private readonly int _threadId;

        private bool _currentIsInvalid = true;

        internal TSource current;
        internal AsyncIteratorState state = AsyncIteratorState.New;
        internal CancellationToken token;

        protected AsyncIterator()
        {
            _threadId = Environment.CurrentManagedThreadId;
        }

        public IAsyncEnumerator<TSource> GetAsyncEnumerator(CancellationToken token)
        {
            var enumerator = state == AsyncIteratorState.New && _threadId == Environment.CurrentManagedThreadId
                ? this
                : Clone();

            enumerator.state = AsyncIteratorState.Allocated;
            enumerator.token = token;

            try
            {
                enumerator.OnGetEnumerator(token);
            }
            catch (Exception ex)
            {
                // GetAsyncEnumerator should not throw but rather return a failing enumerator
                return new FailingEnumerator(enumerator, ex);
            }

            return enumerator;
        }

        public virtual ValueTask DisposeAsync()
        {
            current = default;
            state = AsyncIteratorState.Disposed;

            return TaskExt.CompletedTask;
        }

        public TSource Current
        {
            get
            {
                if (_currentIsInvalid)
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
                var result = await MoveNextCore(token).ConfigureAwait(false);

                _currentIsInvalid = !result; // if move next is false, invalid otherwise valid

                return result;
            }
            catch
            {
                _currentIsInvalid = true;
                await DisposeAsync().ConfigureAwait(false);
                throw;
            }
        }

        public abstract AsyncIterator<TSource> Clone();

        protected abstract ValueTask<bool> MoveNextCore(CancellationToken cancellationToken);

        protected virtual void OnGetEnumerator(CancellationToken cancellationToken)
        {
        }

        /// <summary>
        /// Disposes a source enumerator then fails with an exception in DisposeAsync.
        /// </summary>
        private sealed class FailingEnumerator : IAsyncEnumerator<TSource>
        {
            private readonly IAsyncEnumerator<TSource> _source;

            private readonly Exception _error;

            public FailingEnumerator(IAsyncEnumerator<TSource> source, Exception error)
            {
                _source = source;
                _error = error;
            }

            public TSource Current => throw new InvalidOperationException("Enumerator is in an invalid state");

            public async ValueTask DisposeAsync()
            {
                await _source.DisposeAsync().ConfigureAwait(false);
                ExceptionDispatchInfo.Capture(_error).Throw();
            }

            public ValueTask<bool> MoveNextAsync()
            {
                return TaskExt.False;
            }
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

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        internal abstract class AsyncIterator<TSource> : IAsyncEnumerable<TSource>, IAsyncEnumerator<TSource>
        {
            private readonly int threadId;
            private CancellationTokenSource cancellationTokenSource;
            internal TSource current;
            private bool currentIsInvalid = true;
            internal AsyncIteratorState state = AsyncIteratorState.New;

            protected AsyncIterator()
            {
                threadId = Environment.CurrentManagedThreadId;
            }

            public IAsyncEnumerator<TSource> GetEnumerator()
            {
                var enumerator = state == AsyncIteratorState.New && threadId == Environment.CurrentManagedThreadId ? this : Clone();

                enumerator.state = AsyncIteratorState.Allocated;
                enumerator.cancellationTokenSource = new CancellationTokenSource();

                try
                {
                    enumerator.OnGetEnumerator();
                }
                catch
                {
                    enumerator.Dispose();
                    throw;
                }

                return enumerator;
            }

            public virtual void Dispose()
            {
                if (cancellationTokenSource != null)
                {
                    if (!cancellationTokenSource.IsCancellationRequested)
                    {
                        cancellationTokenSource.Cancel();
                    }
                    cancellationTokenSource.Dispose();
                }

                current = default(TSource);
                state = AsyncIteratorState.Disposed;
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

            public async Task<bool> MoveNext(CancellationToken cancellationToken)
            {
                // Note: MoveNext *must* be implemented as an async method to ensure
                // that any exceptions thrown from the MoveNextCore call are handled 
                // by the try/catch, whether they're sync or async

                if (state == AsyncIteratorState.Disposed)
                {
                    return false;
                }

                using (cancellationToken.Register(Dispose))
                using (var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, cancellationTokenSource.Token))
                {
                    try
                    {
                        // Short circuit and don't even call MoveNexCore
                        cancellationToken.ThrowIfCancellationRequested();

                        if (state == AsyncIteratorState.Allocated)
                        {
                            await this.Initialize(cts.Token);

                            if (state == AsyncIteratorState.Allocated)
                                state = AsyncIteratorState.Iterating;
                        }

                        if (state == AsyncIteratorState.Iterating)
                        {
                            var result = await MoveNextCore(cts.Token)
                                             .ConfigureAwait(false);

                            currentIsInvalid = !result; // if move next is false, invalid otherwise valid

                            return result;
                        }

                        return false;
                    }
                    catch
                    {
                        currentIsInvalid = true;
                        Dispose();
                        throw;
                    }
                }
            }

            public abstract AsyncIterator<TSource> Clone();

            public virtual IAsyncEnumerable<TResult> Select<TResult>(Func<TSource, TResult> selector)
            {
                return new SelectEnumerableAsyncIterator<TSource, TResult>(this, selector);
            }

            public virtual IAsyncEnumerable<TSource> Where(Func<TSource, bool> predicate)
            {
                return new WhereEnumerableAsyncIterator<TSource>(this, predicate);
            }

            protected abstract Task<bool> MoveNextCore(CancellationToken cancellationToken);

            protected virtual void OnGetEnumerator()
            {
            }

            protected abstract Task Initialize(CancellationToken cancellationToken);
        }

        internal enum AsyncIteratorState
        {
            New = 0,
            Allocated = 1,
            Iterating = 2,
            Disposed = -1
        }
    }
}
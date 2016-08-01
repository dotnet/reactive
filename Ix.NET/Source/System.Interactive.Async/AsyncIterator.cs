using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        internal abstract class AsyncIterator<TSource> : IAsyncEnumerable<TSource>, IAsyncEnumerator<TSource>
        {
            public enum State
            {
                New = 0,
                Allocated = 1,
                Iterating = 2,
                Disposed = -1,
            }

            private readonly int threadId;
            internal State state = State.New;
            internal TSource current;
            private CancellationTokenSource cancellationTokenSource;
            private List<CancellationTokenRegistration> moveNextRegistrations;
            private bool currentIsInvalid = true;

            protected AsyncIterator()
            {
                threadId = Environment.CurrentManagedThreadId;
            }

            public abstract AsyncIterator<TSource> Clone();

            public IAsyncEnumerator<TSource> GetEnumerator()
            {
                var enumerator = state == State.New && threadId == Environment.CurrentManagedThreadId ? this : Clone();

                enumerator.state = State.Allocated;
                enumerator.cancellationTokenSource = new CancellationTokenSource();
                enumerator.moveNextRegistrations = new List<CancellationTokenRegistration>();
                return enumerator;
            }

            
            public virtual void Dispose()
            {
                if (!cancellationTokenSource.IsCancellationRequested)
                {
                    cancellationTokenSource.Cancel();
                }
                cancellationTokenSource.Dispose();

                current = default(TSource);
                state = State.Disposed;

                var toClean = moveNextRegistrations?.ToList();
                moveNextRegistrations = null;
                if (toClean != null)
                {
                    foreach (var r in toClean)
                    {
                        r.Dispose();
                    }
                    toClean.Clear();
                }
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
                if (state == State.Disposed)
                {
                    return false;
                }
               

                // We keep these because cancelling any of these must trigger dispose of the iterator
                moveNextRegistrations.Add(cancellationToken.Register(Dispose));

                using (var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, cancellationTokenSource.Token))
                {
                    try
                    {
                        // Short circuit and don't even call MoveNexCore
                        cancellationToken.ThrowIfCancellationRequested();

                        var result = await MoveNextCore(cts.Token).ConfigureAwait(false);

                        currentIsInvalid = !result; // if move next is false, invalid otherwise valid

                        return result;
                    }
                    catch
                    {
                        currentIsInvalid = true;
                        Dispose();
                        throw;
                    }
                }
            }

            protected abstract Task<bool> MoveNextCore(CancellationToken cancellationToken);

            public virtual IAsyncEnumerable<TResult> Select<TResult>(Func<TSource, TResult> selector)
            {
                return new SelectEnumerableAsyncIterator<TSource, TResult>(this, selector);
            }

            public virtual IAsyncEnumerable<TSource> Where(Func<TSource, bool> predicate)
            {
                return new WhereEnumerableAsyncIterator<TSource>(this, predicate);
            }
        }
    }
}

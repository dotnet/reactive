// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

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
        public static IAsyncEnumerable<TSource> ToAsyncEnumerable<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new AsyncEnumerableAdapter<TSource>(source);
        }

        public static IAsyncEnumerable<TSource> ToAsyncEnumerable<TSource>(this Task<TSource> task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));

            return CreateEnumerable(
                () =>
                {
                    var called = 0;

                    var value = default(TSource);
                    return CreateEnumerator(
                        async ct =>
                        {
                            if (Interlocked.CompareExchange(ref called, 1, 0) == 0)
                            {
                                value = await task.ConfigureAwait(false);
                                return true;
                            }
                            return false;
                        },
                        () => value,
                        () => { });
                });
        }

        public static IEnumerable<TSource> ToEnumerable<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return ToEnumerable_(source);
        }

        private static IEnumerable<TSource> ToEnumerable_<TSource>(IAsyncEnumerable<TSource> source)
        {
            using (var e = source.GetEnumerator())
            {
                while (true)
                {
                    if (!e.MoveNext(CancellationToken.None)
                          .Result)
                        break;
                    var c = e.Current;
                    yield return c;
                }
            }
        }

        internal sealed class AsyncEnumerableAdapter<T> : AsyncIterator<T>, IIListProvider<T>
        {
            private readonly IEnumerable<T> source;
            private IEnumerator<T> enumerator;
 
            public AsyncEnumerableAdapter(IEnumerable<T> source)
            {
                Debug.Assert(source != null);
                this.source = source;
            }

            public override AsyncIterator<T> Clone()
            {
                return new AsyncEnumerableAdapter<T>(source);
            }

            public override void Dispose()
            {
                if (enumerator != null)
                {
                    enumerator.Dispose();
                    enumerator = null;
                }

                base.Dispose();
            }

            protected override Task<bool> MoveNextCore(CancellationToken cancellationToken)
            {
                switch (state)
                {
                    case State.Allocated:
                        enumerator = source.GetEnumerator();
                        state = State.Iterating;
                        goto case State.Iterating;

                    case State.Iterating:
                        if (enumerator.MoveNext())
                        {
                            current = enumerator.Current;
                            return Task.FromResult(true);
                        }

                        Dispose();
                        break;
                }
                
                return Task.FromResult(false);
            }

            // These optimizations rely on the Sys.Linq impls from IEnumerable to optimize
            // and short circuit as appropriate
            public Task<T[]> ToArrayAsync(CancellationToken cancellationToken)
            {
                return Task.FromResult(source.ToArray());
            }

            public Task<List<T>> ToListAsync(CancellationToken cancellationToken)
            {
                return Task.FromResult(source.ToList());
            }

            public Task<int> GetCountAsync(bool onlyIfCheap, CancellationToken cancellationToken)
            {
                return Task.FromResult(source.Count());
            }
        }
    }
}
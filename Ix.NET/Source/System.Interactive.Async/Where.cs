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
        public static IAsyncEnumerable<TSource> Where<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            var iterator = source as AsyncIterator<TSource>;
            if (iterator != null)
            {
                return iterator.Where(predicate);
            }

            // TODO: Can we add array/list optimizations here, does it make sense?
            return new WhereEnumerableAsyncIterator<TSource>(source, predicate);
        }

        public static IAsyncEnumerable<TSource> Where<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return CreateEnumerable(
                () =>
                {
                    var e = source.GetEnumerator();
                    var index = 0;

                    var cts = new CancellationTokenDisposable();
                    var d = Disposable.Create(cts, e);

                    var f = default(Func<CancellationToken, Task<bool>>);
                    f = async ct =>
                        {
                            if (await e.MoveNext(ct)
                                       .ConfigureAwait(false))
                            {
                                if (predicate(e.Current, checked(index++)))
                                    return true;
                                return await f(ct)
                                           .ConfigureAwait(false);
                            }
                            return false;
                        };

                    return CreateEnumerator(
                        ct => f(cts.Token),
                        () => e.Current,
                        d.Dispose,
                        e
                    );
                });
        }

        private static Func<TSource, bool> CombinePredicates<TSource>(Func<TSource, bool> predicate1, Func<TSource, bool> predicate2)

        {

            return x => predicate1(x) && predicate2(x);

        }

        internal sealed class WhereEnumerableAsyncIterator<TSource> : AsyncIterator<TSource>
        {
            private readonly IAsyncEnumerable<TSource> source;
            private readonly Func<TSource, bool> predicate;
            private IAsyncEnumerator<TSource> enumerator;

            public WhereEnumerableAsyncIterator(IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
            {
                Debug.Assert(source != null);
                Debug.Assert(predicate != null);

                this.source = source;
                this.predicate = predicate;
            }

            public override AsyncIterator<TSource> Clone()
            {
                return new WhereEnumerableAsyncIterator<TSource>(source, predicate);
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

            protected override async Task<bool> MoveNextCore(CancellationToken cancellationToken)
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        enumerator = source.GetEnumerator();
                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        while (await enumerator.MoveNext(cancellationToken)
                                               .ConfigureAwait(false))
                        {
                            var item = enumerator.Current;
                            if (predicate(item))
                            {
                                current = item;
                                return true;
                            }
                        }

                        Dispose();
                        break;
                }

                return false;
            }

            public override IAsyncEnumerable<TSource> Where(Func<TSource, bool> predicate)
            {
                return new WhereEnumerableAsyncIterator<TSource>(source, CombinePredicates(this.predicate, predicate));
            }

            public override IAsyncEnumerable<TResult> Select<TResult>(Func<TSource, TResult> selector)
            {
                return new WhereSelectEnumerableAsyncIterator<TSource, TResult>(source, predicate, selector);
            }
        }

        internal sealed class WhereSelectEnumerableAsyncIterator<TSource, TResult> : AsyncIterator<TResult>
        {
            private readonly IAsyncEnumerable<TSource> source;
            private readonly Func<TSource, bool> predicate;
            private readonly Func<TSource, TResult> selector;
            private IAsyncEnumerator<TSource> enumerator;

            public WhereSelectEnumerableAsyncIterator(IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, Func<TSource, TResult> selector)
            {
                Debug.Assert(source != null);
                Debug.Assert(predicate != null);
                Debug.Assert(selector != null);

                this.source = source;
                this.predicate = predicate;
                this.selector = selector;
            }

            public override AsyncIterator<TResult> Clone()
            {
                return new WhereSelectEnumerableAsyncIterator<TSource, TResult>(source, predicate, selector);
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

            protected override async Task<bool> MoveNextCore(CancellationToken cancellationToken)
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        enumerator = source.GetEnumerator();
                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        while (await enumerator.MoveNext(cancellationToken)
                                               .ConfigureAwait(false))
                        {
                            var item = enumerator.Current;
                            if (predicate(item))
                            {
                                current = selector(item);
                                return true;
                            }
                        }

                        Dispose();
                        break;
                }

                return false;
            }

            public override IAsyncEnumerable<TResult1> Select<TResult1>(Func<TResult, TResult1> selector)
            {
                return new WhereSelectEnumerableAsyncIterator<TSource, TResult1>(source, predicate, CombineSelectors(this.selector, selector));
            }
        }
    }
}
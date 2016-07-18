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
        public static IAsyncEnumerable<TResult> Select<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));


            var iterator = source as AsyncIterator<TSource>;
            if (iterator != null)
            {
                return iterator.Select(selector);
            }

            // TODO: Can we add optimizations for IList or anything else here?

            return new SelectEnumerableAsyncIterator<TSource, TResult>(source, selector);
        }

        public static IAsyncEnumerable<TResult> Select<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, int, TResult> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return CreateEnumerable(
                () =>
                {
                    var e = source.GetEnumerator();
                    var current = default(TResult);
                    var index = 0;

                    var cts = new CancellationTokenDisposable();
                    var d = Disposable.Create(cts, e);

                    return CreateEnumerator(
                        async ct =>
                        {
                            if (await e.MoveNext(cts.Token)
                                       .ConfigureAwait(false))
                            {
                                current = selector(e.Current, checked(index++));
                                return true;
                            }
                            return false;
                        },
                        () => current,
                        d.Dispose,
                        e
                    );
                });
        }

        private static Func<TSource, TResult> CombineSelectors<TSource, TMiddle, TResult>(Func<TSource, TMiddle> selector1, Func<TMiddle, TResult> selector2)

        {

            return x => selector2(selector1(x));

        }

        internal sealed class SelectEnumerableAsyncIterator<TSource, TResult> : AsyncIterator<TResult>
        {
            private readonly IAsyncEnumerable<TSource> source;
            private readonly Func<TSource, TResult> selector;
            private IAsyncEnumerator<TSource> enumerator;

            public SelectEnumerableAsyncIterator(IAsyncEnumerable<TSource> source, Func<TSource, TResult> selector)
            {
                Debug.Assert(source != null);
                Debug.Assert(selector != null);

                this.source = source;
                this.selector = selector;
            }

            public override AsyncIterator<TResult> Clone()
            {
                return new SelectEnumerableAsyncIterator<TSource, TResult>(source, selector);
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

            public override async Task<bool> MoveNextCore(CancellationToken cancellationToken)
            {
                switch (state)
                {
                    case State.Allocated:
                        enumerator = source.GetEnumerator();
                        state = State.Iterating;
                        goto case State.Iterating;

                    case State.Iterating:
                        if (await enumerator.MoveNext(cancellationToken)
                                            .ConfigureAwait(false))
                        {
                            current = selector(enumerator.Current);
                            return true;

                        }

                        Dispose();
                        break;
                }

                return false;
            }

            public override IAsyncEnumerable<TResult1> Select<TResult1>(Func<TResult, TResult1> selector)
            {
                return new SelectEnumerableAsyncIterator<TSource, TResult1>(source, CombineSelectors(this.selector, selector));
            }
        }
    }
}
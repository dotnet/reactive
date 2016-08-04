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
        public static IAsyncEnumerable<TResult> Join<TOuter, TInner, TKey, TResult>(this IAsyncEnumerable<TOuter> outer, IAsyncEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector, IEqualityComparer<TKey> comparer)
        {
            if (outer == null)
                throw new ArgumentNullException(nameof(outer));
            if (inner == null)
                throw new ArgumentNullException(nameof(inner));
            if (outerKeySelector == null)
                throw new ArgumentNullException(nameof(outerKeySelector));
            if (innerKeySelector == null)
                throw new ArgumentNullException(nameof(innerKeySelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return new JoinAsyncIterator<TOuter, TInner, TKey, TResult>(outer, inner, outerKeySelector, innerKeySelector, resultSelector, comparer);
        }

        public static IAsyncEnumerable<TResult> Join<TOuter, TInner, TKey, TResult>(this IAsyncEnumerable<TOuter> outer, IAsyncEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector)
        {
            if (outer == null)
                throw new ArgumentNullException(nameof(outer));
            if (inner == null)
                throw new ArgumentNullException(nameof(inner));
            if (outerKeySelector == null)
                throw new ArgumentNullException(nameof(outerKeySelector));
            if (innerKeySelector == null)
                throw new ArgumentNullException(nameof(innerKeySelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            return new JoinAsyncIterator<TOuter,TInner,TKey,TResult>(outer, inner, outerKeySelector, innerKeySelector, resultSelector, EqualityComparer<TKey>.Default);
        }

        internal sealed class JoinAsyncIterator<TOuter, TInner, TKey, TResult> : AsyncIterator<TResult>
        {
            private readonly IAsyncEnumerable<TOuter> outer;
            private readonly IAsyncEnumerable<TInner> inner;
            private readonly Func<TOuter, TKey> outerKeySelector;
            private readonly Func<TInner, TKey> innerKeySelector;
            private readonly Func<TOuter, TInner, TResult> resultSelector;
            private readonly IEqualityComparer<TKey> comparer;

            private IAsyncEnumerator<TOuter> outerEnumerator;
 


            public JoinAsyncIterator(IAsyncEnumerable<TOuter> outer, IAsyncEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector, IEqualityComparer<TKey> comparer)
            {
                Debug.Assert(outer != null);
                Debug.Assert(inner != null);
                Debug.Assert(outerKeySelector != null);
                Debug.Assert(innerKeySelector != null);
                Debug.Assert(resultSelector != null);

                this.outer = outer;
                this.inner = inner;
                this.outerKeySelector = outerKeySelector;
                this.innerKeySelector = innerKeySelector;
                this.resultSelector = resultSelector;
                this.comparer = comparer;
            }

            public override AsyncIterator<TResult> Clone()
            {
                return new JoinAsyncIterator<TOuter, TInner, TKey, TResult>(outer, inner, outerKeySelector, innerKeySelector, resultSelector, comparer);
            }

            public override void Dispose()
            {
                if (outerEnumerator != null)
                {
                    outerEnumerator.Dispose();
                    outerEnumerator = null;
                }

                base.Dispose();
            }

            // State machine vars
            Internal.Lookup<TKey, TInner> lookup;
            int count;
            TInner[] elements;
            int index;
            TOuter item;
            private int mode;

            const int State_Begin = 1;
            const int State_DoLoop = 2;
            const int State_For = 3;
            const int State_While = 4;

            protected override async Task<bool> MoveNextCore(CancellationToken cancellationToken)
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        outerEnumerator = outer.GetEnumerator();
                        mode = State_Begin;
                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        switch (mode)
                        {
                            case State_Begin:
                                if (await outerEnumerator.MoveNext(cancellationToken)
                                                         .ConfigureAwait(false))
                                {
                                    lookup = await Internal.Lookup<TKey, TInner>.CreateForJoinAsync(inner, innerKeySelector, comparer, cancellationToken).ConfigureAwait(false);
                                    if (lookup.Count != 0)
                                    {
                                        mode = State_DoLoop;
                                        goto case State_DoLoop;   
                                    }
                                }

                                break;
                            case State_DoLoop:
                                item = outerEnumerator.Current;
                                var g = lookup.GetGrouping(outerKeySelector(item), create: false);
                                if (g != null)
                                {
                                    count = g._count;
                                    elements = g._elements;
                                    index = 0;
                                    mode = State_For;
                                    goto case State_For;
                                }

                                break;

                            case State_For:
                                current = resultSelector(item, elements[index]);
                                index++;
                                if (index == count)
                                {
                                    mode = State_While;
                                }
                                return true;

                            case State_While:
                                var hasNext = await outerEnumerator.MoveNext(cancellationToken).ConfigureAwait(false);
                                if (hasNext)
                                {
                                    goto case State_DoLoop;
                                }

                             
                                break;
                        }

                        Dispose();

                        break;
                }

                return false;
            }
        }
    }
}
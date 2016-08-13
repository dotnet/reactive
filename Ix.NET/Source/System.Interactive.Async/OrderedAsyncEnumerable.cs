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
    internal abstract class OrderedAsyncEnumerable<TElement> : AsyncEnumerable.AsyncIterator<TElement>, IOrderedAsyncEnumerable<TElement>
    {
        internal IOrderedEnumerable<TElement> enumerable;
        internal IAsyncEnumerable<TElement> source;

        IOrderedAsyncEnumerable<TElement> IOrderedAsyncEnumerable<TElement>.CreateOrderedEnumerable<TKey>(Func<TElement, TKey> keySelector, IComparer<TKey> comparer, bool descending)
        {
            return new OrderedAsyncEnumerable<TElement, TKey>(source, keySelector, comparer, descending, this);
        }

        internal abstract Task Initialize(CancellationToken cancellationToken);
    }

    internal sealed class OrderedAsyncEnumerable<TElement, TKey> : OrderedAsyncEnumerable<TElement>
    {
        private readonly IComparer<TKey> comparer;
        private readonly bool descending;
        private readonly Func<TElement, TKey> keySelector;


        private IEnumerator<TElement> enumerator;

        private readonly OrderedAsyncEnumerable<TElement> parent;
        private IAsyncEnumerator<TElement> parentEnumerator;


        public OrderedAsyncEnumerable(IAsyncEnumerable<TElement> source, Func<TElement, TKey> keySelector, IComparer<TKey> comparer, bool descending, OrderedAsyncEnumerable<TElement> parent)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));
            this.source = source;
            this.keySelector = keySelector;
            this.comparer = comparer ?? Comparer<TKey>.Default;
            this.descending = descending;
            this.parent = parent;
        }

        public override AsyncEnumerable.AsyncIterator<TElement> Clone()
        {
            return new OrderedAsyncEnumerable<TElement, TKey>(source, keySelector, comparer, descending, parent);
        }


        public override void Dispose()
        {
            if (enumerator != null)
            {
                enumerator.Dispose();
                enumerator = null;
            }

            if (parentEnumerator != null)
            {
                parentEnumerator.Dispose();
                parentEnumerator = null;
            }
            base.Dispose();
        }


        protected override async Task<bool> MoveNextCore(CancellationToken cancellationToken)
        {
            switch (state)
            {
                case AsyncEnumerable.AsyncIteratorState.Allocated:

                    await Initialize(cancellationToken)
                        .ConfigureAwait(false);

                    enumerator = enumerable.GetEnumerator();
                    state = AsyncEnumerable.AsyncIteratorState.Iterating;
                    goto case AsyncEnumerable.AsyncIteratorState.Iterating;

                case AsyncEnumerable.AsyncIteratorState.Iterating:
                    if (enumerator.MoveNext())
                    {
                        current = enumerator.Current;
                        return true;
                    }

                    Dispose();
                    break;
            }

            return false;
        }

        internal override async Task Initialize(CancellationToken cancellationToken)
        {
            if (parent == null)
            {
                var buffer = await source.ToList(cancellationToken)
                                         .ConfigureAwait(false);
                enumerable = (!@descending ? buffer.OrderBy(keySelector, comparer) : buffer.OrderByDescending(keySelector, comparer));
            }
            else
            {
                parentEnumerator = parent.GetEnumerator();
                await parent.Initialize(cancellationToken)
                            .ConfigureAwait(false);
                enumerable = parent.enumerable.CreateOrderedEnumerable(keySelector, comparer, @descending);
            }
        }
    }
}
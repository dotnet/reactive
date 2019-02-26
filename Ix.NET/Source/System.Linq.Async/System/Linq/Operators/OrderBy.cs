// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        public static IOrderedAsyncEnumerable<TSource> OrderBy<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector) =>
            new OrderedAsyncEnumerable<TSource, TKey>(source, keySelector, comparer: null, descending: false, parent: null);

        public static IOrderedAsyncEnumerable<TSource> OrderByAwait<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector) =>
            new OrderedAsyncEnumerableWithTask<TSource, TKey>(source, keySelector, comparer: null, descending: false, parent: null);

#if !NO_DEEP_CANCELLATION
        public static IOrderedAsyncEnumerable<TSource> OrderByAwaitWithCancellation<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector) =>
            new OrderedAsyncEnumerableWithTaskAndCancellation<TSource, TKey>(source, keySelector, comparer: null, descending: false, parent: null);
#endif

        public static IOrderedAsyncEnumerable<TSource> OrderBy<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer) =>
            new OrderedAsyncEnumerable<TSource, TKey>(source, keySelector, comparer, descending: false, parent: null);

        public static IOrderedAsyncEnumerable<TSource> OrderByAwait<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, IComparer<TKey> comparer) =>
            new OrderedAsyncEnumerableWithTask<TSource, TKey>(source, keySelector, comparer, descending: false, parent: null);

#if !NO_DEEP_CANCELLATION
        public static IOrderedAsyncEnumerable<TSource> OrderByAwaitWithCancellation<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector, IComparer<TKey> comparer) =>
            new OrderedAsyncEnumerableWithTaskAndCancellation<TSource, TKey>(source, keySelector, comparer, descending: false, parent: null);
#endif

        public static IOrderedAsyncEnumerable<TSource> OrderByDescending<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector) =>
            new OrderedAsyncEnumerable<TSource, TKey>(source, keySelector, comparer: null, descending: true, parent: null);

        public static IOrderedAsyncEnumerable<TSource> OrderByDescendingAwait<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector) =>
            new OrderedAsyncEnumerableWithTask<TSource, TKey>(source, keySelector, comparer: null, descending: true, parent: null);

#if !NO_DEEP_CANCELLATION
        public static IOrderedAsyncEnumerable<TSource> OrderByDescendingAwaitWithCancellation<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector) =>
            new OrderedAsyncEnumerableWithTaskAndCancellation<TSource, TKey>(source, keySelector, comparer: null, descending: true, parent: null);
#endif

        public static IOrderedAsyncEnumerable<TSource> OrderByDescending<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer) =>
            new OrderedAsyncEnumerable<TSource, TKey>(source, keySelector, comparer, descending: true, parent: null);

        public static IOrderedAsyncEnumerable<TSource> OrderByDescendingAwait<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, IComparer<TKey> comparer) =>
            new OrderedAsyncEnumerableWithTask<TSource, TKey>(source, keySelector, comparer, descending: true, parent: null);

#if !NO_DEEP_CANCELLATION
        public static IOrderedAsyncEnumerable<TSource> OrderByDescendingAwaitWithCancellation<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector, IComparer<TKey> comparer) =>
            new OrderedAsyncEnumerableWithTaskAndCancellation<TSource, TKey>(source, keySelector, comparer, descending: true, parent: null);
#endif

        public static IOrderedAsyncEnumerable<TSource> ThenBy<TSource, TKey>(this IOrderedAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return source.CreateOrderedEnumerable(keySelector, comparer: null, descending: false);
        }

        public static IOrderedAsyncEnumerable<TSource> ThenByAwait<TSource, TKey>(this IOrderedAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return source.CreateOrderedEnumerable(keySelector, comparer: default(IComparer<TKey>), descending: false);
        }

#if !NO_DEEP_CANCELLATION
        public static IOrderedAsyncEnumerable<TSource> ThenByAwaitWithCancellation<TSource, TKey>(this IOrderedAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return source.CreateOrderedEnumerable(keySelector, comparer: null, descending: false);
        }
#endif

        public static IOrderedAsyncEnumerable<TSource> ThenBy<TSource, TKey>(this IOrderedAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return source.CreateOrderedEnumerable(keySelector, comparer, descending: false);
        }

        public static IOrderedAsyncEnumerable<TSource> ThenByAwait<TSource, TKey>(this IOrderedAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, IComparer<TKey> comparer)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return source.CreateOrderedEnumerable(keySelector, comparer, descending: false);
        }

#if !NO_DEEP_CANCELLATION
        public static IOrderedAsyncEnumerable<TSource> ThenByAwaitWithCancellation<TSource, TKey>(this IOrderedAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector, IComparer<TKey> comparer)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return source.CreateOrderedEnumerable(keySelector, comparer, descending: false);
        }
#endif

        public static IOrderedAsyncEnumerable<TSource> ThenByDescending<TSource, TKey>(this IOrderedAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return source.CreateOrderedEnumerable(keySelector, comparer: null, descending: true);
        }

        public static IOrderedAsyncEnumerable<TSource> ThenByDescendingAwait<TSource, TKey>(this IOrderedAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return source.CreateOrderedEnumerable(keySelector, comparer: default(IComparer<TKey>), descending: true);
        }

#if !NO_DEEP_CANCELLATION
        public static IOrderedAsyncEnumerable<TSource> ThenByDescendingAwaitWithCancellation<TSource, TKey>(this IOrderedAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return source.CreateOrderedEnumerable(keySelector, comparer: null, descending: true);
        }
#endif

        public static IOrderedAsyncEnumerable<TSource> ThenByDescending<TSource, TKey>(this IOrderedAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return source.CreateOrderedEnumerable(keySelector, comparer, descending: true);
        }

        public static IOrderedAsyncEnumerable<TSource> ThenByDescendingAwait<TSource, TKey>(this IOrderedAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, IComparer<TKey> comparer)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return source.CreateOrderedEnumerable(keySelector, comparer, descending: true);
        }

#if !NO_DEEP_CANCELLATION
        public static IOrderedAsyncEnumerable<TSource> ThenByDescendingAwaitWithCancellation<TSource, TKey>(this IOrderedAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector, IComparer<TKey> comparer)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return source.CreateOrderedEnumerable(keySelector, comparer, descending: true);
        }
#endif
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerableEx
    {
        public static ValueTask<IList<TSource>> MaxByAsync<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (keySelector == null)
                throw Error.ArgumentNull(nameof(keySelector));

            return MaxByCore(source, keySelector, comparer: null, cancellationToken);
        }

        public static ValueTask<IList<TSource>> MaxByAsync<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey>? comparer, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (keySelector == null)
                throw Error.ArgumentNull(nameof(keySelector));

            return MaxByCore(source, keySelector, comparer, cancellationToken);
        }

        public static ValueTask<IList<TSource>> MaxByAsync<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (keySelector == null)
                throw Error.ArgumentNull(nameof(keySelector));

            return MaxByCore<TSource, TKey>(source, keySelector, comparer: null, cancellationToken);
        }

#if !NO_DEEP_CANCELLATION
        public static ValueTask<IList<TSource>> MaxByAsync<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (keySelector == null)
                throw Error.ArgumentNull(nameof(keySelector));

            return MaxByCore<TSource, TKey>(source, keySelector, comparer: null, cancellationToken);
        }
#endif

        public static ValueTask<IList<TSource>> MaxByAsync<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, IComparer<TKey>? comparer, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (keySelector == null)
                throw Error.ArgumentNull(nameof(keySelector));

            return MaxByCore(source, keySelector, comparer, cancellationToken);
        }

#if !NO_DEEP_CANCELLATION
        public static ValueTask<IList<TSource>> MaxByAsync<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector, IComparer<TKey>? comparer, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (keySelector == null)
                throw Error.ArgumentNull(nameof(keySelector));

            return MaxByCore(source, keySelector, comparer, cancellationToken);
        }
#endif

        private static ValueTask<IList<TSource>> MaxByCore<TSource, TKey>(IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey>? comparer, CancellationToken cancellationToken)
        {
            comparer ??= Comparer<TKey>.Default;

            return ExtremaBy(source, keySelector, (key, minValue) => comparer.Compare(key, minValue), cancellationToken);
        }

        private static ValueTask<IList<TSource>> MaxByCore<TSource, TKey>(IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, IComparer<TKey>? comparer, CancellationToken cancellationToken)
        {
            comparer ??= Comparer<TKey>.Default;

            return ExtremaBy(source, keySelector, (key, minValue) => comparer.Compare(key, minValue), cancellationToken);
        }

#if !NO_DEEP_CANCELLATION
        private static ValueTask<IList<TSource>> MaxByCore<TSource, TKey>(IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector, IComparer<TKey>? comparer, CancellationToken cancellationToken)
        {
            comparer ??= Comparer<TKey>.Default;

            return ExtremaBy(source, keySelector, (key, minValue) => comparer.Compare(key, minValue), cancellationToken);
        }
#endif
    }
}

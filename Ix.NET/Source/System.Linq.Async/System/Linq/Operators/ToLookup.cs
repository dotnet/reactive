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
        public static Task<ILookup<TKey, TSource>> ToLookupAsync<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (keySelector == null)
                throw Error.ArgumentNull(nameof(keySelector));

            return ToLookupCore(source, keySelector, x => x, comparer: null, cancellationToken);
        }

        public static Task<ILookup<TKey, TSource>> ToLookupAsync<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (keySelector == null)
                throw Error.ArgumentNull(nameof(keySelector));

            return ToLookupCore<TSource, TKey, TSource>(source, keySelector, x => new ValueTask<TSource>(x), comparer: null, cancellationToken);
        }

#if !NO_DEEP_CANCELLATION
        public static Task<ILookup<TKey, TSource>> ToLookupAsync<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (keySelector == null)
                throw Error.ArgumentNull(nameof(keySelector));

            return ToLookupCore<TSource, TKey, TSource>(source, x => keySelector(x, cancellationToken), x => new ValueTask<TSource>(x), comparer: null, cancellationToken);
        }
#endif

        public static Task<ILookup<TKey, TSource>> ToLookupAsync<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (keySelector == null)
                throw Error.ArgumentNull(nameof(keySelector));

            return ToLookupCore(source, keySelector, x => x, comparer, cancellationToken);
        }

        public static Task<ILookup<TKey, TSource>> ToLookupAsync<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (keySelector == null)
                throw Error.ArgumentNull(nameof(keySelector));

            return ToLookupCore(source, keySelector, x => new ValueTask<TSource>(x), comparer, cancellationToken);
        }

#if !NO_DEEP_CANCELLATION
        public static Task<ILookup<TKey, TSource>> ToLookupAsync<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (keySelector == null)
                throw Error.ArgumentNull(nameof(keySelector));

            return ToLookupCore(source, x => keySelector(x, cancellationToken), x => new ValueTask<TSource>(x), comparer, cancellationToken);
        }
#endif

        public static Task<ILookup<TKey, TElement>> ToLookupAsync<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (keySelector == null)
                throw Error.ArgumentNull(nameof(keySelector));
            if (elementSelector == null)
                throw Error.ArgumentNull(nameof(elementSelector));

            return ToLookupCore(source, keySelector, elementSelector, comparer: null, cancellationToken);
        }

        public static Task<ILookup<TKey, TElement>> ToLookupAsync<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, Func<TSource, ValueTask<TElement>> elementSelector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (keySelector == null)
                throw Error.ArgumentNull(nameof(keySelector));
            if (elementSelector == null)
                throw Error.ArgumentNull(nameof(elementSelector));

            return ToLookupCore<TSource, TKey, TElement>(source, keySelector, elementSelector, comparer: null, cancellationToken);
        }

#if !NO_DEEP_CANCELLATION
        public static Task<ILookup<TKey, TElement>> ToLookupAsync<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector, Func<TSource, CancellationToken, ValueTask<TElement>> elementSelector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (keySelector == null)
                throw Error.ArgumentNull(nameof(keySelector));
            if (elementSelector == null)
                throw Error.ArgumentNull(nameof(elementSelector));

            return ToLookupCore<TSource, TKey, TElement>(source, keySelector, elementSelector, comparer: null, cancellationToken);
        }
#endif

        public static Task<ILookup<TKey, TElement>> ToLookupAsync<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (keySelector == null)
                throw Error.ArgumentNull(nameof(keySelector));
            if (elementSelector == null)
                throw Error.ArgumentNull(nameof(elementSelector));

            return ToLookupCore(source, keySelector, elementSelector, comparer, cancellationToken);
        }

        public static Task<ILookup<TKey, TElement>> ToLookupAsync<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, Func<TSource, ValueTask<TElement>> elementSelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (keySelector == null)
                throw Error.ArgumentNull(nameof(keySelector));
            if (elementSelector == null)
                throw Error.ArgumentNull(nameof(elementSelector));

            return ToLookupCore(source, keySelector, elementSelector, comparer, cancellationToken);
        }

#if !NO_DEEP_CANCELLATION
        public static Task<ILookup<TKey, TElement>> ToLookupAsync<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector, Func<TSource, CancellationToken, ValueTask<TElement>> elementSelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (keySelector == null)
                throw Error.ArgumentNull(nameof(keySelector));
            if (elementSelector == null)
                throw Error.ArgumentNull(nameof(elementSelector));

            return ToLookupCore(source, keySelector, elementSelector, comparer, cancellationToken);
        }
#endif

        private static async Task<ILookup<TKey, TElement>> ToLookupCore<TSource, TKey, TElement>(IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken)
        {
            return await Internal.Lookup<TKey, TElement>.CreateAsync(source, keySelector, elementSelector, comparer, cancellationToken).ConfigureAwait(false);
        }

        private static async Task<ILookup<TKey, TElement>> ToLookupCore<TSource, TKey, TElement>(IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, Func<TSource, ValueTask<TElement>> elementSelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken)
        {
            return await Internal.LookupWithTask<TKey, TElement>.CreateAsync(source, keySelector, elementSelector, comparer, cancellationToken).ConfigureAwait(false);
        }

#if !NO_DEEP_CANCELLATION
        private static async Task<ILookup<TKey, TElement>> ToLookupCore<TSource, TKey, TElement>(IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector, Func<TSource, CancellationToken, ValueTask<TElement>> elementSelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken)
        {
            return await Internal.LookupWithTask<TKey, TElement>.CreateAsync(source, keySelector, elementSelector, comparer, cancellationToken).ConfigureAwait(false);
        }
#endif
    }
}

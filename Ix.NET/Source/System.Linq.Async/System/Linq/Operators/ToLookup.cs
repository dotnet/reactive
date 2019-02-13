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
        public static ValueTask<ILookup<TKey, TSource>> ToLookupAsync<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, CancellationToken cancellationToken = default) =>
            ToLookupAsync(source, keySelector, comparer: null, cancellationToken);

        public static ValueTask<ILookup<TKey, TSource>> ToLookupAsync<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (keySelector == null)
                throw Error.ArgumentNull(nameof(keySelector));

            return Core(source, keySelector, comparer, cancellationToken);

            static async ValueTask<ILookup<TKey, TSource>> Core(IAsyncEnumerable<TSource> _source, Func<TSource, TKey> _keySelector, IEqualityComparer<TKey> _comparer, CancellationToken _cancellationToken)
            {
                return await Internal.Lookup<TKey, TSource>.CreateAsync(_source, _keySelector, _comparer, _cancellationToken).ConfigureAwait(false);
            }
        }

        public static ValueTask<ILookup<TKey, TSource>> ToLookupAsync<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, CancellationToken cancellationToken = default) =>
            ToLookupAsync<TSource, TKey>(source, keySelector, comparer:null, cancellationToken);

        public static ValueTask<ILookup<TKey, TSource>> ToLookupAsync<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (keySelector == null)
                throw Error.ArgumentNull(nameof(keySelector));

            return Core(source, keySelector, comparer, cancellationToken);

            async ValueTask<ILookup<TKey, TSource>> Core(IAsyncEnumerable<TSource> _source, Func<TSource, ValueTask<TKey>> _keySelector, IEqualityComparer<TKey> _comparer, CancellationToken _cancellationToken)
            {
                return await Internal.LookupWithTask<TKey, TSource>.CreateAsync(_source, _keySelector, _comparer, _cancellationToken).ConfigureAwait(false);
            }
        }

#if !NO_DEEP_CANCELLATION
        public static ValueTask<ILookup<TKey, TSource>> ToLookupAsync<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector, CancellationToken cancellationToken = default) =>
            ToLookupAsync(source, keySelector, comparer: null, cancellationToken);

        public static ValueTask<ILookup<TKey, TSource>> ToLookupAsync<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (keySelector == null)
                throw Error.ArgumentNull(nameof(keySelector));

            return Core(source, keySelector, comparer, cancellationToken);

            static async ValueTask<ILookup<TKey, TSource>> Core(IAsyncEnumerable<TSource> _source, Func<TSource, CancellationToken, ValueTask<TKey>> _keySelector, IEqualityComparer<TKey> _comparer, CancellationToken _cancellationToken)
            {
                return await Internal.LookupWithTask<TKey, TSource>.CreateAsync(_source, _keySelector, _comparer, _cancellationToken).ConfigureAwait(false);
            }
        }
#endif

        public static ValueTask<ILookup<TKey, TElement>> ToLookupAsync<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, CancellationToken cancellationToken = default) =>
            ToLookupAsync(source, keySelector, elementSelector, comparer: null, cancellationToken);

        public static ValueTask<ILookup<TKey, TElement>> ToLookupAsync<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (keySelector == null)
                throw Error.ArgumentNull(nameof(keySelector));
            if (elementSelector == null)
                throw Error.ArgumentNull(nameof(elementSelector));

            return Core(source, keySelector, elementSelector, comparer, cancellationToken);

            static async ValueTask<ILookup<TKey, TElement>> Core(IAsyncEnumerable<TSource> _source, Func<TSource, TKey> _keySelector, Func<TSource, TElement> _elementSelector, IEqualityComparer<TKey> _comparer, CancellationToken _cancellationToken)
            {
                return await Internal.Lookup<TKey, TElement>.CreateAsync(_source, _keySelector, _elementSelector, _comparer, _cancellationToken).ConfigureAwait(false);
            }
        }

        public static ValueTask<ILookup<TKey, TElement>> ToLookupAsync<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, Func<TSource, ValueTask<TElement>> elementSelector, CancellationToken cancellationToken = default) =>
            ToLookupAsync<TSource, TKey, TElement>(source, keySelector, elementSelector, comparer: null, cancellationToken);

        public static ValueTask<ILookup<TKey, TElement>> ToLookupAsync<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, Func<TSource, ValueTask<TElement>> elementSelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (keySelector == null)
                throw Error.ArgumentNull(nameof(keySelector));
            if (elementSelector == null)
                throw Error.ArgumentNull(nameof(elementSelector));

            return Core(source, keySelector, elementSelector, comparer, cancellationToken);

            static async ValueTask<ILookup<TKey, TElement>> Core(IAsyncEnumerable<TSource> _source, Func<TSource, ValueTask<TKey>> _keySelector, Func<TSource, ValueTask<TElement>> _elementSelector, IEqualityComparer<TKey> _comparer, CancellationToken _cancellationToken)
            {
                return await Internal.LookupWithTask<TKey, TElement>.CreateAsync(_source, _keySelector, _elementSelector, _comparer, _cancellationToken).ConfigureAwait(false);
            }
        }

#if !NO_DEEP_CANCELLATION
        public static ValueTask<ILookup<TKey, TElement>> ToLookupAsync<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector, Func<TSource, CancellationToken, ValueTask<TElement>> elementSelector, CancellationToken cancellationToken = default) =>
            ToLookupAsync(source, keySelector, elementSelector, comparer: null, cancellationToken);

        public static ValueTask<ILookup<TKey, TElement>> ToLookupAsync<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector, Func<TSource, CancellationToken, ValueTask<TElement>> elementSelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (keySelector == null)
                throw Error.ArgumentNull(nameof(keySelector));
            if (elementSelector == null)
                throw Error.ArgumentNull(nameof(elementSelector));

            return Core(source, keySelector, elementSelector, comparer, cancellationToken);

            static async ValueTask<ILookup<TKey, TElement>> Core(IAsyncEnumerable<TSource> _source, Func<TSource, CancellationToken, ValueTask<TKey>> _keySelector, Func<TSource, CancellationToken, ValueTask<TElement>> _elementSelector, IEqualityComparer<TKey> _comparer, CancellationToken _cancellationToken)
            {
                return await Internal.LookupWithTask<TKey, TElement>.CreateAsync(_source, _keySelector, _elementSelector, _comparer, _cancellationToken).ConfigureAwait(false);
            }
        }
#endif
    }
}

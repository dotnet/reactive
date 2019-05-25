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
        public static IAsyncEnumerable<TSource> DistinctUntilChanged<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return DistinctUntilChangedCore(source, comparer: null);
        }

        public static IAsyncEnumerable<TSource> DistinctUntilChanged<TSource>(this IAsyncEnumerable<TSource> source, IEqualityComparer<TSource>? comparer)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return DistinctUntilChangedCore(source, comparer);
        }

        public static IAsyncEnumerable<TSource> DistinctUntilChanged<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (keySelector == null)
                throw Error.ArgumentNull(nameof(keySelector));

            return DistinctUntilChangedCore(source, keySelector, comparer: null);
        }

        public static IAsyncEnumerable<TSource> DistinctUntilChanged<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey>? comparer)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (keySelector == null)
                throw Error.ArgumentNull(nameof(keySelector));

            return DistinctUntilChangedCore(source, keySelector, comparer);
        }

        public static IAsyncEnumerable<TSource> DistinctUntilChanged<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (keySelector == null)
                throw Error.ArgumentNull(nameof(keySelector));

            return DistinctUntilChangedCore<TSource, TKey>(source, keySelector, comparer: null);
        }

#if !NO_DEEP_CANCELLATION
        public static IAsyncEnumerable<TSource> DistinctUntilChanged<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (keySelector == null)
                throw Error.ArgumentNull(nameof(keySelector));

            return DistinctUntilChangedCore<TSource, TKey>(source, keySelector, comparer: null);
        }
#endif

        public static IAsyncEnumerable<TSource> DistinctUntilChanged<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, IEqualityComparer<TKey>? comparer)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (keySelector == null)
                throw Error.ArgumentNull(nameof(keySelector));

            return DistinctUntilChangedCore(source, keySelector, comparer);
        }

#if !NO_DEEP_CANCELLATION
        public static IAsyncEnumerable<TSource> DistinctUntilChanged<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (keySelector == null)
                throw Error.ArgumentNull(nameof(keySelector));

            return DistinctUntilChangedCore(source, keySelector, comparer);
        }
#endif

        private static IAsyncEnumerable<TSource> DistinctUntilChangedCore<TSource>(IAsyncEnumerable<TSource> source, IEqualityComparer<TSource>? comparer)
        {
            comparer ??= EqualityComparer<TSource>.Default;

            return AsyncEnumerable.Create(Core);

            async IAsyncEnumerator<TSource> Core(CancellationToken cancellationToken)
            {
                await using (var e = source.GetConfiguredAsyncEnumerator(cancellationToken, false))
                {
                    if (!await e.MoveNextAsync())
                    {
                        yield break;
                    }

                    var latest = e.Current;

                    yield return latest;

                    while (await e.MoveNextAsync())
                    {
                        var item = e.Current;

                        // REVIEW: Need comparer!.Equals to satisfy nullable reference type warnings.

                        if (!comparer!.Equals(latest, item))
                        {
                            latest = item;

                            yield return latest;
                        }
                    }
                }
            }
        }

        private static IAsyncEnumerable<TSource> DistinctUntilChangedCore<TSource, TKey>(IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey>? comparer)
        {
            comparer ??= EqualityComparer<TKey>.Default;

            return AsyncEnumerable.Create(Core);

            async IAsyncEnumerator<TSource> Core(CancellationToken cancellationToken)
            {
                await using (var e = source.GetConfiguredAsyncEnumerator(cancellationToken, false))
                {
                    if (!await e.MoveNextAsync())
                    {
                        yield break;
                    }

                    var item = e.Current;

                    var latestKey = keySelector(item);

                    yield return item;

                    while (await e.MoveNextAsync())
                    {
                        item = e.Current;

                        var currentKey = keySelector(item);

                        // REVIEW: Need comparer!.Equals to satisfy nullable reference type warnings.

                        if (!comparer!.Equals(latestKey, currentKey))
                        {
                            latestKey = currentKey;

                            yield return item;
                        }
                    }
                }
            }
        }

        private static IAsyncEnumerable<TSource> DistinctUntilChangedCore<TSource, TKey>(IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, IEqualityComparer<TKey>? comparer)
        {
            comparer ??= EqualityComparer<TKey>.Default;

            return AsyncEnumerable.Create(Core);

            async IAsyncEnumerator<TSource> Core(CancellationToken cancellationToken)
            {
                await using (var e = source.GetConfiguredAsyncEnumerator(cancellationToken, false))
                {
                    if (!await e.MoveNextAsync())
                    {
                        yield break;
                    }

                    var item = e.Current;

                    var latestKey = await keySelector(item).ConfigureAwait(false);

                    yield return item;

                    while (await e.MoveNextAsync())
                    {
                        item = e.Current;

                        var currentKey = await keySelector(item).ConfigureAwait(false);

                        // REVIEW: Need comparer!.Equals to satisfy nullable reference type warnings.

                        if (!comparer!.Equals(latestKey, currentKey))
                        {
                            latestKey = currentKey;

                            yield return item;
                        }
                    }
                }
            }
        }

#if !NO_DEEP_CANCELLATION
        private static IAsyncEnumerable<TSource> DistinctUntilChangedCore<TSource, TKey>(IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector, IEqualityComparer<TKey>? comparer)
        {
            comparer ??= EqualityComparer<TKey>.Default;

            return AsyncEnumerable.Create(Core);

            async IAsyncEnumerator<TSource> Core(CancellationToken cancellationToken)
            {
                await using (var e = source.GetConfiguredAsyncEnumerator(cancellationToken, false))
                {
                    if (!await e.MoveNextAsync())
                    {
                        yield break;
                    }

                    var item = e.Current;

                    var latestKey = await keySelector(item, cancellationToken).ConfigureAwait(false);

                    yield return item;

                    while (await e.MoveNextAsync())
                    {
                        item = e.Current;

                        var currentKey = await keySelector(item, cancellationToken).ConfigureAwait(false);

                        // REVIEW: Need comparer!.Equals to satisfy nullable reference type warnings.

                        if (!comparer!.Equals(latestKey, currentKey))
                        {
                            latestKey = currentKey;

                            yield return item;
                        }
                    }
                }
            }
        }
#endif
    }
}

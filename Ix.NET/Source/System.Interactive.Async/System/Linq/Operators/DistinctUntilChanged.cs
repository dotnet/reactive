// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerableEx
    {
        public static IAsyncEnumerable<TSource> DistinctUntilChanged<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return DistinctUntilChangedCore(source, EqualityComparer<TSource>.Default);
        }

        public static IAsyncEnumerable<TSource> DistinctUntilChanged<TSource>(this IAsyncEnumerable<TSource> source, IEqualityComparer<TSource> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return DistinctUntilChangedCore(source, comparer);
        }

        public static IAsyncEnumerable<TSource> DistinctUntilChanged<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return DistinctUntilChangedCore(source, keySelector, EqualityComparer<TKey>.Default);
        }

        public static IAsyncEnumerable<TSource> DistinctUntilChanged<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return DistinctUntilChangedCore(source, keySelector, comparer);
        }

        public static IAsyncEnumerable<TSource> DistinctUntilChanged<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<TKey>> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return DistinctUntilChangedCore(source, keySelector, EqualityComparer<TKey>.Default);
        }

        public static IAsyncEnumerable<TSource> DistinctUntilChanged<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<TKey>> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return DistinctUntilChangedCore(source, keySelector, comparer);
        }

        private static IAsyncEnumerable<TSource> DistinctUntilChangedCore<TSource>(IAsyncEnumerable<TSource> source, IEqualityComparer<TSource> comparer)
        {
            return new DistinctUntilChangedAsyncIterator<TSource>(source, comparer);
        }

        private static IAsyncEnumerable<TSource> DistinctUntilChangedCore<TSource, TKey>(IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            return new DistinctUntilChangedAsyncIterator<TSource, TKey>(source, keySelector, comparer);
        }

        private static IAsyncEnumerable<TSource> DistinctUntilChangedCore<TSource, TKey>(IAsyncEnumerable<TSource> source, Func<TSource, Task<TKey>> keySelector, IEqualityComparer<TKey> comparer)
        {
            return new DistinctUntilChangedAsyncIteratorWithTask<TSource, TKey>(source, keySelector, comparer);
        }

        private sealed class DistinctUntilChangedAsyncIterator<TSource> : AsyncIterator<TSource>
        {
            private readonly IEqualityComparer<TSource> comparer;
            private readonly IAsyncEnumerable<TSource> source;

            private TSource currentValue;
            private IAsyncEnumerator<TSource> enumerator;
            private bool hasCurrentValue;

            public DistinctUntilChangedAsyncIterator(IAsyncEnumerable<TSource> source, IEqualityComparer<TSource> comparer)
            {
                Debug.Assert(comparer != null);
                Debug.Assert(source != null);

                this.source = source;
                this.comparer = comparer;
            }

            public override AsyncIterator<TSource> Clone()
            {
                return new DistinctUntilChangedAsyncIterator<TSource>(source, comparer);
            }

            public override async ValueTask DisposeAsync()
            {
                if (enumerator != null)
                {
                    await enumerator.DisposeAsync().ConfigureAwait(false);
                    enumerator = null;
                    currentValue = default;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async ValueTask<bool> MoveNextCore(CancellationToken cancellationToken)
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        enumerator = source.GetAsyncEnumerator(cancellationToken);
                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        while (await enumerator.MoveNextAsync().ConfigureAwait(false))
                        {
                            var item = enumerator.Current;
                            var comparerEquals = false;

                            if (hasCurrentValue)
                            {
                                comparerEquals = comparer.Equals(currentValue, item);
                            }

                            if (!hasCurrentValue || !comparerEquals)
                            {
                                hasCurrentValue = true;
                                currentValue = item;
                                current = item;
                                return true;
                            }
                        }

                        break;
                }

                await DisposeAsync().ConfigureAwait(false);
                return false;
            }
        }

        private sealed class DistinctUntilChangedAsyncIterator<TSource, TKey> : AsyncIterator<TSource>
        {
            private readonly IEqualityComparer<TKey> comparer;
            private readonly Func<TSource, TKey> keySelector;
            private readonly IAsyncEnumerable<TSource> source;
            private TKey currentKeyValue;

            private IAsyncEnumerator<TSource> enumerator;
            private bool hasCurrentKey;

            public DistinctUntilChangedAsyncIterator(IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
            {
                this.source = source;
                this.keySelector = keySelector;
                this.comparer = comparer;
            }

            public override AsyncIterator<TSource> Clone()
            {
                return new DistinctUntilChangedAsyncIterator<TSource, TKey>(source, keySelector, comparer);
            }

            public override async ValueTask DisposeAsync()
            {
                if (enumerator != null)
                {
                    await enumerator.DisposeAsync().ConfigureAwait(false);
                    enumerator = null;
                    currentKeyValue = default;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async ValueTask<bool> MoveNextCore(CancellationToken cancellationToken)
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        enumerator = source.GetAsyncEnumerator(cancellationToken);
                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        while (await enumerator.MoveNextAsync().ConfigureAwait(false))
                        {
                            var item = enumerator.Current;
                            var key = keySelector(item);
                            var comparerEquals = false;

                            if (hasCurrentKey)
                            {
                                comparerEquals = comparer.Equals(currentKeyValue, key);
                            }
                            if (!hasCurrentKey || !comparerEquals)
                            {
                                hasCurrentKey = true;
                                currentKeyValue = key;
                                current = item;
                                return true;
                            }
                        }

                        break; // case
                }

                await DisposeAsync().ConfigureAwait(false);
                return false;
            }
        }

        private sealed class DistinctUntilChangedAsyncIteratorWithTask<TSource, TKey> : AsyncIterator<TSource>
        {
            private readonly IEqualityComparer<TKey> comparer;
            private readonly Func<TSource, Task<TKey>> keySelector;
            private readonly IAsyncEnumerable<TSource> source;
            private TKey currentKeyValue;

            private IAsyncEnumerator<TSource> enumerator;
            private bool hasCurrentKey;

            public DistinctUntilChangedAsyncIteratorWithTask(IAsyncEnumerable<TSource> source, Func<TSource, Task<TKey>> keySelector, IEqualityComparer<TKey> comparer)
            {
                this.source = source;
                this.keySelector = keySelector;
                this.comparer = comparer;
            }

            public override AsyncIterator<TSource> Clone()
            {
                return new DistinctUntilChangedAsyncIteratorWithTask<TSource, TKey>(source, keySelector, comparer);
            }

            public override async ValueTask DisposeAsync()
            {
                if (enumerator != null)
                {
                    await enumerator.DisposeAsync().ConfigureAwait(false);
                    enumerator = null;
                    currentKeyValue = default;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async ValueTask<bool> MoveNextCore(CancellationToken cancellationToken)
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        enumerator = source.GetAsyncEnumerator(cancellationToken);
                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        while (await enumerator.MoveNextAsync().ConfigureAwait(false))
                        {
                            var item = enumerator.Current;
                            var key = await keySelector(item).ConfigureAwait(false);
                            var comparerEquals = false;

                            if (hasCurrentKey)
                            {
                                comparerEquals = comparer.Equals(currentKeyValue, key);
                            }
                            if (!hasCurrentKey || !comparerEquals)
                            {
                                hasCurrentKey = true;
                                currentKeyValue = key;
                                current = item;
                                return true;
                            }
                        }

                        break; // case
                }

                await DisposeAsync().ConfigureAwait(false);
                return false;
            }
        }
    }
}

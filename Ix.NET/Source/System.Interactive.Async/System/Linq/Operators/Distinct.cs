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
        public static IAsyncEnumerable<TSource> Distinct<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return DistinctCore(source, keySelector, EqualityComparer<TKey>.Default);
        }

        public static IAsyncEnumerable<TSource> Distinct<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return DistinctCore(source, keySelector, comparer);
        }

        public static IAsyncEnumerable<TSource> Distinct<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<TKey>> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return DistinctCore(source, keySelector, EqualityComparer<TKey>.Default);
        }

        public static IAsyncEnumerable<TSource> Distinct<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<TKey>> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return DistinctCore(source, keySelector, comparer);
        }

        private static IAsyncEnumerable<TSource> DistinctCore<TSource, TKey>(IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            return new DistinctAsyncIterator<TSource, TKey>(source, keySelector, comparer);
        }

        private static IAsyncEnumerable<TSource> DistinctCore<TSource, TKey>(IAsyncEnumerable<TSource> source, Func<TSource, Task<TKey>> keySelector, IEqualityComparer<TKey> comparer)
        {
            return new DistinctAsyncIteratorWithTask<TSource, TKey>(source, keySelector, comparer);
        }

        private sealed class DistinctAsyncIterator<TSource, TKey> : AsyncIterator<TSource>, IAsyncIListProvider<TSource>
        {
            private readonly IEqualityComparer<TKey> comparer;
            private readonly Func<TSource, TKey> keySelector;
            private readonly IAsyncEnumerable<TSource> source;

            private IAsyncEnumerator<TSource> enumerator;
            private Set<TKey> set;

            public DistinctAsyncIterator(IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
            {
                Debug.Assert(source != null);
                Debug.Assert(keySelector != null);
                Debug.Assert(comparer != null);

                this.source = source;
                this.keySelector = keySelector;
                this.comparer = comparer;
            }

            public async Task<TSource[]> ToArrayAsync(CancellationToken cancellationToken)
            {
                var s = await FillSetAsync(cancellationToken).ConfigureAwait(false);
                return s.ToArray();
            }

            public async Task<List<TSource>> ToListAsync(CancellationToken cancellationToken)
            {
                var s = await FillSetAsync(cancellationToken).ConfigureAwait(false);
                return s;
            }

            public async Task<int> GetCountAsync(bool onlyIfCheap, CancellationToken cancellationToken)
            {
                if (onlyIfCheap)
                {
                    return -1;
                }

                var count = 0;
                var s = new Set<TKey>(comparer);

                var enu = source.GetAsyncEnumerator();

                try
                {
                    while (await enu.MoveNextAsync().ConfigureAwait(false))
                    {
                        var item = enu.Current;
                        if (s.Add(keySelector(item)))
                        {
                            count++;
                        }
                    }
                }
                finally
                {
                    await enu.DisposeAsync().ConfigureAwait(false);
                }

                return count;
            }

            public override AsyncIterator<TSource> Clone()
            {
                return new DistinctAsyncIterator<TSource, TKey>(source, keySelector, comparer);
            }

            public override async Task DisposeAsync()
            {
                if (enumerator != null)
                {
                    await enumerator.DisposeAsync().ConfigureAwait(false);
                    enumerator = null;
                    set = null;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async Task<bool> MoveNextCore()
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        enumerator = source.GetAsyncEnumerator();

                        if (!await enumerator.MoveNextAsync().ConfigureAwait(false))
                        {
                            await DisposeAsync().ConfigureAwait(false);
                            return false;
                        }

                        var element = enumerator.Current;
                        set = new Set<TKey>(comparer);
                        set.Add(keySelector(element));
                        current = element;

                        state = AsyncIteratorState.Iterating;
                        return true;

                    case AsyncIteratorState.Iterating:
                        while (await enumerator.MoveNextAsync().ConfigureAwait(false))
                        {
                            element = enumerator.Current;
                            if (set.Add(keySelector(element)))
                            {
                                current = element;
                                return true;
                            }
                        }

                        break;
                }

                await DisposeAsync().ConfigureAwait(false);
                return false;
            }

            private async Task<List<TSource>> FillSetAsync(CancellationToken cancellationToken)
            {
                var s = new Set<TKey>(comparer);
                var r = new List<TSource>();

                var enu = source.GetAsyncEnumerator();

                try
                {
                    while (await enu.MoveNextAsync(cancellationToken).ConfigureAwait(false))
                    {
                        var item = enu.Current;
                        if (s.Add(keySelector(item)))
                        {
                            r.Add(item);
                        }
                    }
                }
                finally
                {
                    await enu.DisposeAsync().ConfigureAwait(false);
                }

                return r;
            }
        }

        private sealed class DistinctAsyncIteratorWithTask<TSource, TKey> : AsyncIterator<TSource>, IAsyncIListProvider<TSource>
        {
            private readonly IEqualityComparer<TKey> comparer;
            private readonly Func<TSource, Task<TKey>> keySelector;
            private readonly IAsyncEnumerable<TSource> source;

            private IAsyncEnumerator<TSource> enumerator;
            private Set<TKey> set;

            public DistinctAsyncIteratorWithTask(IAsyncEnumerable<TSource> source, Func<TSource, Task<TKey>> keySelector, IEqualityComparer<TKey> comparer)
            {
                Debug.Assert(source != null);
                Debug.Assert(keySelector != null);
                Debug.Assert(comparer != null);

                this.source = source;
                this.keySelector = keySelector;
                this.comparer = comparer;
            }

            public async Task<TSource[]> ToArrayAsync(CancellationToken cancellationToken)
            {
                var s = await FillSetAsync(cancellationToken).ConfigureAwait(false);
                return s.ToArray();
            }

            public async Task<List<TSource>> ToListAsync(CancellationToken cancellationToken)
            {
                var s = await FillSetAsync(cancellationToken).ConfigureAwait(false);
                return s;
            }

            public async Task<int> GetCountAsync(bool onlyIfCheap, CancellationToken cancellationToken)
            {
                if (onlyIfCheap)
                {
                    return -1;
                }

                var count = 0;
                var s = new Set<TKey>(comparer);

                var enu = source.GetAsyncEnumerator();

                try
                {
                    while (await enu.MoveNextAsync().ConfigureAwait(false))
                    {
                        var item = enu.Current;
                        if (s.Add(await keySelector(item).ConfigureAwait(false)))
                        {
                            count++;
                        }
                    }
                }
                finally
                {
                    await enu.DisposeAsync().ConfigureAwait(false);
                }

                return count;
            }

            public override AsyncIterator<TSource> Clone()
            {
                return new DistinctAsyncIteratorWithTask<TSource, TKey>(source, keySelector, comparer);
            }

            public override async Task DisposeAsync()
            {
                if (enumerator != null)
                {
                    await enumerator.DisposeAsync().ConfigureAwait(false);
                    enumerator = null;
                    set = null;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async Task<bool> MoveNextCore()
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        enumerator = source.GetAsyncEnumerator();

                        if (!await enumerator.MoveNextAsync().ConfigureAwait(false))
                        {
                            await DisposeAsync().ConfigureAwait(false);
                            return false;
                        }

                        var element = enumerator.Current;
                        set = new Set<TKey>(comparer);
                        set.Add(await keySelector(element).ConfigureAwait(false));
                        current = element;

                        state = AsyncIteratorState.Iterating;
                        return true;

                    case AsyncIteratorState.Iterating:
                        while (await enumerator.MoveNextAsync().ConfigureAwait(false))
                        {
                            element = enumerator.Current;
                            if (set.Add(await keySelector(element).ConfigureAwait(false)))
                            {
                                current = element;
                                return true;
                            }
                        }

                        break;
                }

                await DisposeAsync().ConfigureAwait(false);
                return false;
            }

            private async Task<List<TSource>> FillSetAsync(CancellationToken cancellationToken)
            {
                var s = new Set<TKey>(comparer);
                var r = new List<TSource>();

                var enu = source.GetAsyncEnumerator();

                try
                {
                    while (await enu.MoveNextAsync(cancellationToken).ConfigureAwait(false))
                    {
                        var item = enu.Current;
                        if (s.Add(await keySelector(item).ConfigureAwait(false)))
                        {
                            r.Add(item);
                        }
                    }
                }
                finally
                {
                    await enu.DisposeAsync().ConfigureAwait(false);
                }

                return r;
            }
        }
    }
}

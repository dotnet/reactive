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
        public static IAsyncEnumerable<TSource> Distinct<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return new DistinctAsyncIterator<TSource, TKey>(source, keySelector, comparer);
        }

        public static IAsyncEnumerable<TSource> Distinct<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return source.Distinct(keySelector, EqualityComparer<TKey>.Default);
        }

        public static IAsyncEnumerable<TSource> Distinct<TSource>(this IAsyncEnumerable<TSource> source, IEqualityComparer<TSource> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return new DistinctAsyncIterator<TSource>(source, comparer);
        }

        public static IAsyncEnumerable<TSource> Distinct<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Distinct(EqualityComparer<TSource>.Default);
        }

        public static IAsyncEnumerable<TSource> DistinctUntilChanged<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.DistinctUntilChanged(EqualityComparer<TSource>.Default);
        }

        public static IAsyncEnumerable<TSource> DistinctUntilChanged<TSource>(this IAsyncEnumerable<TSource> source, IEqualityComparer<TSource> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return new DistinctUntilChangedAsyncIterator<TSource>(source, comparer);
        }

        public static IAsyncEnumerable<TSource> DistinctUntilChanged<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return source.DistinctUntilChanged_(keySelector, EqualityComparer<TKey>.Default);
        }

        public static IAsyncEnumerable<TSource> DistinctUntilChanged<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return source.DistinctUntilChanged_(keySelector, comparer);
        }

        private static IAsyncEnumerable<TSource> DistinctUntilChanged_<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            return new DistinctUntilChangedAsyncIterator<TSource, TKey>(source, keySelector, comparer);
        }

        private sealed class DistinctAsyncIterator<TSource, TKey> : AsyncIterator<TSource>, IIListProvider<TSource>
        {
            private readonly IEqualityComparer<TKey> comparer;
            private readonly Func<TSource, TKey> keySelector;
            private readonly IAsyncEnumerable<TSource> source;

            private IAsyncEnumerator<TSource> enumerator;
            private Set<TKey> set;

            public DistinctAsyncIterator(IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
            {
                Debug.Assert(source != null);

                this.source = source;
                this.keySelector = keySelector;
                this.comparer = comparer;
            }

            public async Task<TSource[]> ToArrayAsync(CancellationToken cancellationToken)
            {
                var s = await FillSet(cancellationToken)
                            .ConfigureAwait(false);
                return s.ToArray();
            }

            public async Task<List<TSource>> ToListAsync(CancellationToken cancellationToken)
            {
                var s = await FillSet(cancellationToken)
                            .ConfigureAwait(false);
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
                using (var enu = source.GetEnumerator())
                {
                    while (await enu.MoveNext(cancellationToken)
                                    .ConfigureAwait(false))
                    {
                        var item = enu.Current;
                        if (s.Add(keySelector(item)))
                        {
                            count++;
                        }
                    }
                }

                return count;
            }

            public override AsyncIterator<TSource> Clone()
            {
                return new DistinctAsyncIterator<TSource, TKey>(source, keySelector, comparer);
            }

            public override void Dispose()
            {
                if (enumerator != null)
                {
                    enumerator.Dispose();
                    enumerator = null;
                    set = null;
                }

                base.Dispose();
            }

            protected override async Task<bool> MoveNextCore(CancellationToken cancellationToken)
            {
                while (await enumerator.MoveNext(cancellationToken)
                                       .ConfigureAwait(false))
                {
                    var element = enumerator.Current;
                    if (set.Add(keySelector(element)))
                    {
                        current = element;
                        return true;
                    }
                }

                Dispose();
                return false;
            }

            protected override Task Initialize(CancellationToken cancellationToken)
            {
                enumerator = source.GetEnumerator();
                set = new Set<TKey>(comparer);

                return TaskExt.True;
            }

            private async Task<List<TSource>> FillSet(CancellationToken cancellationToken)
            {
                var s = new Set<TKey>(comparer);
                var r = new List<TSource>();
                using (var enu = source.GetEnumerator())
                {
                    while (await enu.MoveNext(cancellationToken)
                                    .ConfigureAwait(false))
                    {
                        var item = enu.Current;
                        if (s.Add(keySelector(item)))
                        {
                            r.Add(item);
                        }
                    }
                }

                return r;
            }
        }

        private sealed class DistinctAsyncIterator<TSource> : AsyncIterator<TSource>, IIListProvider<TSource>
        {
            private readonly IEqualityComparer<TSource> comparer;
            private readonly IAsyncEnumerable<TSource> source;

            private IAsyncEnumerator<TSource> enumerator;
            private Set<TSource> set;

            public DistinctAsyncIterator(IAsyncEnumerable<TSource> source, IEqualityComparer<TSource> comparer)
            {
                Debug.Assert(source != null);

                this.source = source;
                this.comparer = comparer;
            }

            public async Task<TSource[]> ToArrayAsync(CancellationToken cancellationToken)
            {
                var s = await FillSet(cancellationToken)
                            .ConfigureAwait(false);
                return s.ToArray();
            }

            public async Task<List<TSource>> ToListAsync(CancellationToken cancellationToken)
            {
                var s = await FillSet(cancellationToken)
                            .ConfigureAwait(false);
                return s.ToList();
            }

            public async Task<int> GetCountAsync(bool onlyIfCheap, CancellationToken cancellationToken)
            {
                return onlyIfCheap ? -1 : (await FillSet(cancellationToken)
                                               .ConfigureAwait(false)).Count;
            }

            public override AsyncIterator<TSource> Clone()
            {
                return new DistinctAsyncIterator<TSource>(source, comparer);
            }

            public override void Dispose()
            {
                if (enumerator != null)
                {
                    enumerator.Dispose();
                    enumerator = null;
                    set = null;
                }

                base.Dispose();
            }

            protected override async Task<bool> MoveNextCore(CancellationToken cancellationToken)
            {
                while (await enumerator.MoveNext(cancellationToken)
                                       .ConfigureAwait(false))
                {
                    var element = enumerator.Current;
                    if (set.Add(element))
                    {
                        current = element;
                        return true;
                    }
                }


                Dispose();
                return false;
            }

            protected override Task Initialize(CancellationToken cancellationToken)
            {
                enumerator = source.GetEnumerator();
                set = new Set<TSource>(comparer);

                return TaskExt.True;
            }

            private async Task<Set<TSource>> FillSet(CancellationToken cancellationToken)
            {
                var s = new Set<TSource>(comparer);
                using (var enu = source.GetEnumerator())
                {
                    while (await enu.MoveNext(cancellationToken)
                                    .ConfigureAwait(false))
                    {
                        s.Add(enu.Current);
                    }
                }

                return s;
            }
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
                this.source = source;
                this.comparer = comparer;
            }

            public override AsyncIterator<TSource> Clone()
            {
                return new DistinctUntilChangedAsyncIterator<TSource>(source, comparer);
            }

            public override void Dispose()
            {
                if (enumerator != null)
                {
                    enumerator.Dispose();
                    enumerator = null;
                    currentValue = default(TSource);
                }

                base.Dispose();
            }

            protected override async Task<bool> MoveNextCore(CancellationToken cancellationToken)
            {
                while (await enumerator.MoveNext(cancellationToken)
                                    .ConfigureAwait(false))
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


                Dispose();
                return false;
            }

            protected override Task Initialize(CancellationToken cancellationToken)
            {
                enumerator = source.GetEnumerator();

                return TaskExt.True;
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

            public override void Dispose()
            {
                if (enumerator != null)
                {
                    enumerator.Dispose();
                    enumerator = null;
                    currentKeyValue = default(TKey);
                }

                base.Dispose();
            }

            protected override async Task<bool> MoveNextCore(CancellationToken cancellationToken)
            {
                while (await enumerator.MoveNext(cancellationToken)
                                       .ConfigureAwait(false))
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

                Dispose();
                return false;
            }

            protected override Task Initialize(CancellationToken cancellationToken)
            {
                enumerator = source.GetEnumerator();

                return TaskExt.True;
            }
        }
    }
}
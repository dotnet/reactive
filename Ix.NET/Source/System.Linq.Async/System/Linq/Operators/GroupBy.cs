// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        public static IAsyncEnumerable<IAsyncGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return new GroupedAsyncEnumerable<TSource, TKey>(source, keySelector, EqualityComparer<TKey>.Default);
        }

        public static IAsyncEnumerable<IAsyncGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return new GroupedAsyncEnumerable<TSource, TKey>(source, keySelector, comparer);
        }

        public static IAsyncEnumerable<IAsyncGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<TKey>> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return new GroupedAsyncEnumerableWithTask<TSource, TKey>(source, keySelector, EqualityComparer<TKey>.Default);
        }

        public static IAsyncEnumerable<IAsyncGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<TKey>> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return new GroupedAsyncEnumerableWithTask<TSource, TKey>(source, keySelector, comparer);
        }

        public static IAsyncEnumerable<IAsyncGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));

            return source.GroupBy(keySelector, elementSelector, EqualityComparer<TKey>.Default);
        }

        public static IAsyncEnumerable<IAsyncGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return new GroupedAsyncEnumerable<TSource, TKey, TElement>(source, keySelector, elementSelector, comparer);
        }

        public static IAsyncEnumerable<IAsyncGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<TKey>> keySelector, Func<TSource, Task<TElement>> elementSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));

            return source.GroupBy(keySelector, elementSelector, EqualityComparer<TKey>.Default);
        }

        public static IAsyncEnumerable<IAsyncGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<TKey>> keySelector, Func<TSource, Task<TElement>> elementSelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return new GroupedAsyncEnumerableWithTask<TSource, TKey, TElement>(source, keySelector, elementSelector, comparer);
        }

        public static IAsyncEnumerable<TResult> GroupBy<TSource, TKey, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TKey, IAsyncEnumerable<TSource>, TResult> resultSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            return GroupBy(source, keySelector, resultSelector, EqualityComparer<TKey>.Default);
        }

        public static IAsyncEnumerable<TResult> GroupBy<TSource, TKey, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TKey, IAsyncEnumerable<TSource>, TResult> resultSelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return new GroupedResultAsyncEnumerable<TSource, TKey, TResult>(source, keySelector, resultSelector, comparer);
        }

        public static IAsyncEnumerable<TResult> GroupBy<TSource, TKey, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<TKey>> keySelector, Func<TKey, IAsyncEnumerable<TSource>, Task<TResult>> resultSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            return GroupBy(source, keySelector, resultSelector, EqualityComparer<TKey>.Default);
        }

        public static IAsyncEnumerable<TResult> GroupBy<TSource, TKey, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<TKey>> keySelector, Func<TKey, IAsyncEnumerable<TSource>, Task<TResult>> resultSelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return new GroupedResultAsyncEnumerableWithTask<TSource, TKey, TResult>(source, keySelector, resultSelector, comparer);
        }

        public static IAsyncEnumerable<TResult> GroupBy<TSource, TKey, TElement, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<TKey, IAsyncEnumerable<TElement>, TResult> resultSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            return source.GroupBy(keySelector, elementSelector, EqualityComparer<TKey>.Default).Select(g => resultSelector(g.Key, g));
        }

        public static IAsyncEnumerable<TResult> GroupBy<TSource, TKey, TElement, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<TKey, IAsyncEnumerable<TElement>, TResult> resultSelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return source.GroupBy(keySelector, elementSelector, comparer).Select(g => resultSelector(g.Key, g));
        }

        public static IAsyncEnumerable<TResult> GroupBy<TSource, TKey, TElement, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<TKey>> keySelector, Func<TSource, Task<TElement>> elementSelector, Func<TKey, IAsyncEnumerable<TElement>, Task<TResult>> resultSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            return source.GroupBy(keySelector, elementSelector, EqualityComparer<TKey>.Default).Select(g => resultSelector(g.Key, g));
        }

        public static IAsyncEnumerable<TResult> GroupBy<TSource, TKey, TElement, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<TKey>> keySelector, Func<TSource, Task<TElement>> elementSelector, Func<TKey, IAsyncEnumerable<TElement>, Task<TResult>> resultSelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return source.GroupBy(keySelector, elementSelector, comparer).Select(g => resultSelector(g.Key, g));
        }

        internal sealed class GroupedResultAsyncEnumerable<TSource, TKey, TResult> : AsyncIterator<TResult>, IIListProvider<TResult>
        {
            private readonly IAsyncEnumerable<TSource> source;
            private readonly Func<TSource, TKey> keySelector;
            private readonly Func<TKey, IAsyncEnumerable<TSource>, TResult> resultSelector;
            private readonly IEqualityComparer<TKey> comparer;

            private Internal.Lookup<TKey, TSource> lookup;
            private IEnumerator<TResult> enumerator;

            public GroupedResultAsyncEnumerable(IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TKey, IAsyncEnumerable<TSource>, TResult> resultSelector, IEqualityComparer<TKey> comparer)
            {
                Debug.Assert(source != null);
                Debug.Assert(keySelector != null);
                Debug.Assert(resultSelector != null);
                Debug.Assert(comparer != null);

                this.source = source;
                this.keySelector = keySelector;
                this.resultSelector = resultSelector;
                this.comparer = comparer;
            }

            public override AsyncIterator<TResult> Clone()
            {
                return new GroupedResultAsyncEnumerable<TSource, TKey, TResult>(source, keySelector, resultSelector, comparer);
            }

            public override async Task DisposeAsync()
            {
                if (enumerator != null)
                {
                    enumerator.Dispose();
                    enumerator = null;
                    lookup = null;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async Task<bool> MoveNextCore()
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        lookup = await Internal.Lookup<TKey, TSource>.CreateAsync(source, keySelector, comparer).ConfigureAwait(false);
                        enumerator = lookup.ApplyResultSelector(resultSelector).GetEnumerator();
                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        if (enumerator.MoveNext())
                        {
                            current = enumerator.Current;
                            return true;
                        }

                        await DisposeAsync().ConfigureAwait(false);
                        break;
                }

                return false;
            }

            public async Task<TResult[]> ToArrayAsync(CancellationToken cancellationToken)
            {
                var l = await Internal.Lookup<TKey, TSource>.CreateAsync(source, keySelector, comparer, cancellationToken).ConfigureAwait(false);
                return l.ToArray(resultSelector);
            }

            public async Task<List<TResult>> ToListAsync(CancellationToken cancellationToken)
            {
                var l = await Internal.Lookup<TKey, TSource>.CreateAsync(source, keySelector, comparer, cancellationToken).ConfigureAwait(false);
                return l.ToList(resultSelector);
            }

            public async Task<int> GetCountAsync(bool onlyIfCheap, CancellationToken cancellationToken)
            {
                if (onlyIfCheap)
                {
                    return -1;
                }

                var l = await Internal.Lookup<TKey, TSource>.CreateAsync(source, keySelector, comparer, cancellationToken).ConfigureAwait(false);

                return l.Count;
            }
        }

        internal sealed class GroupedResultAsyncEnumerableWithTask<TSource, TKey, TResult> : AsyncIterator<TResult>, IIListProvider<TResult>
        {
            private readonly IAsyncEnumerable<TSource> source;
            private readonly Func<TSource, Task<TKey>> keySelector;
            private readonly Func<TKey, IAsyncEnumerable<TSource>, Task<TResult>> resultSelector;
            private readonly IEqualityComparer<TKey> comparer;

            private Internal.LookupWithTask<TKey, TSource> lookup;
            private IAsyncEnumerator<TResult> enumerator;

            public GroupedResultAsyncEnumerableWithTask(IAsyncEnumerable<TSource> source, Func<TSource, Task<TKey>> keySelector, Func<TKey, IAsyncEnumerable<TSource>, Task<TResult>> resultSelector, IEqualityComparer<TKey> comparer)
            {
                Debug.Assert(source != null);
                Debug.Assert(keySelector != null);
                Debug.Assert(resultSelector != null);
                Debug.Assert(comparer != null);

                this.source = source;
                this.keySelector = keySelector;
                this.resultSelector = resultSelector;
                this.comparer = comparer;
            }

            public override AsyncIterator<TResult> Clone()
            {
                return new GroupedResultAsyncEnumerableWithTask<TSource, TKey, TResult>(source, keySelector, resultSelector, comparer);
            }

            public override async Task DisposeAsync()
            {
                if (enumerator != null)
                {
                    await enumerator.DisposeAsync().ConfigureAwait(false);
                    enumerator = null;
                    lookup = null;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async Task<bool> MoveNextCore()
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        lookup = await Internal.LookupWithTask<TKey, TSource>.CreateAsync(source, keySelector, comparer).ConfigureAwait(false);
                        enumerator = lookup.Select(async g => await resultSelector(g.Key, g).ConfigureAwait(false)).GetAsyncEnumerator();
                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        if (await enumerator.MoveNextAsync().ConfigureAwait(false))
                        {
                            current = enumerator.Current;
                            return true;
                        }

                        await DisposeAsync().ConfigureAwait(false);
                        break;
                }

                return false;
            }

            public async Task<TResult[]> ToArrayAsync(CancellationToken cancellationToken)
            {
                var l = await Internal.LookupWithTask<TKey, TSource>.CreateAsync(source, keySelector, comparer, cancellationToken).ConfigureAwait(false);
                return await l.ToArray(resultSelector).ConfigureAwait(false);
            }

            public async Task<List<TResult>> ToListAsync(CancellationToken cancellationToken)
            {
                var l = await Internal.LookupWithTask<TKey, TSource>.CreateAsync(source, keySelector, comparer, cancellationToken).ConfigureAwait(false);
                return await l.ToList(resultSelector).ConfigureAwait(false);
            }

            public async Task<int> GetCountAsync(bool onlyIfCheap, CancellationToken cancellationToken)
            {
                if (onlyIfCheap)
                {
                    return -1;
                }

                var l = await Internal.LookupWithTask<TKey, TSource>.CreateAsync(source, keySelector, comparer, cancellationToken).ConfigureAwait(false);

                return l.Count;
            }
        }

        internal sealed class GroupedAsyncEnumerable<TSource, TKey, TElement> : AsyncIterator<IAsyncGrouping<TKey, TElement>>, IIListProvider<IAsyncGrouping<TKey, TElement>>
        {
            private readonly IAsyncEnumerable<TSource> source;
            private readonly Func<TSource, TKey> keySelector;
            private readonly Func<TSource, TElement> elementSelector;
            private readonly IEqualityComparer<TKey> comparer;

            private Internal.Lookup<TKey, TElement> lookup;
            private IEnumerator<IGrouping<TKey, TElement>> enumerator;

            public GroupedAsyncEnumerable(IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
            {
                Debug.Assert(source != null);
                Debug.Assert(keySelector != null);
                Debug.Assert(elementSelector != null);
                Debug.Assert(comparer != null);

                this.source = source;
                this.keySelector = keySelector;
                this.elementSelector = elementSelector;
                this.comparer = comparer;
            }

            public override AsyncIterator<IAsyncGrouping<TKey, TElement>> Clone()
            {
                return new GroupedAsyncEnumerable<TSource, TKey, TElement>(source, keySelector, elementSelector, comparer);
            }

            public override async Task DisposeAsync()
            {
                if (enumerator != null)
                {
                    enumerator.Dispose();
                    enumerator = null;
                    lookup = null;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async Task<bool> MoveNextCore()
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        lookup = await Internal.Lookup<TKey, TElement>.CreateAsync(source, keySelector, elementSelector, comparer).ConfigureAwait(false);
                        enumerator = lookup.GetEnumerator();
                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        if (enumerator.MoveNext())
                        {
                            current = (IAsyncGrouping<TKey, TElement>)enumerator.Current;
                            return true;
                        }

                        await DisposeAsync().ConfigureAwait(false);
                        break;
                }

                return false;
            }
            
            public async Task<IAsyncGrouping<TKey, TElement>[]> ToArrayAsync(CancellationToken cancellationToken)
            {
                IIListProvider<IAsyncGrouping<TKey, TElement>> l = await Internal.Lookup<TKey, TElement>.CreateAsync(source, keySelector, elementSelector, comparer, cancellationToken).ConfigureAwait(false);
                return await l.ToArrayAsync(cancellationToken).ConfigureAwait(false);
            }

            public async Task<List<IAsyncGrouping<TKey, TElement>>> ToListAsync(CancellationToken cancellationToken)
            {
                IIListProvider<IAsyncGrouping<TKey, TElement>> l = await Internal.Lookup<TKey, TElement>.CreateAsync(source, keySelector, elementSelector, comparer, cancellationToken).ConfigureAwait(false);
                return await l.ToListAsync(cancellationToken).ConfigureAwait(false);
            }

            public async Task<int> GetCountAsync(bool onlyIfCheap, CancellationToken cancellationToken)
            {
                if (onlyIfCheap)
                {
                    return -1;
                }

                var l = await Internal.Lookup<TKey, TElement>.CreateAsync(source, keySelector, elementSelector, comparer, cancellationToken).ConfigureAwait(false);

                return l.Count;
            }
        }

        internal sealed class GroupedAsyncEnumerableWithTask<TSource, TKey, TElement> : AsyncIterator<IAsyncGrouping<TKey, TElement>>, IIListProvider<IAsyncGrouping<TKey, TElement>>
        {
            private readonly IAsyncEnumerable<TSource> source;
            private readonly Func<TSource, Task<TKey>> keySelector;
            private readonly Func<TSource, Task<TElement>> elementSelector;
            private readonly IEqualityComparer<TKey> comparer;

            private Internal.LookupWithTask<TKey, TElement> lookup;
            private IEnumerator<IGrouping<TKey, TElement>> enumerator;

            public GroupedAsyncEnumerableWithTask(IAsyncEnumerable<TSource> source, Func<TSource, Task<TKey>> keySelector, Func<TSource, Task<TElement>> elementSelector, IEqualityComparer<TKey> comparer)
            {
                Debug.Assert(source != null);
                Debug.Assert(keySelector != null);
                Debug.Assert(elementSelector != null);
                Debug.Assert(comparer != null);

                this.source = source;
                this.keySelector = keySelector;
                this.elementSelector = elementSelector;
                this.comparer = comparer;
            }

            public override AsyncIterator<IAsyncGrouping<TKey, TElement>> Clone()
            {
                return new GroupedAsyncEnumerableWithTask<TSource, TKey, TElement>(source, keySelector, elementSelector, comparer);
            }

            public override async Task DisposeAsync()
            {
                if (enumerator != null)
                {
                    enumerator.Dispose();
                    enumerator = null;
                    lookup = null;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async Task<bool> MoveNextCore()
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        lookup = await Internal.LookupWithTask<TKey, TElement>.CreateAsync(source, keySelector, elementSelector, comparer).ConfigureAwait(false);
                        enumerator = lookup.GetEnumerator();
                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        if (enumerator.MoveNext())
                        {
                            current = (IAsyncGrouping<TKey, TElement>)enumerator.Current;
                            return true;
                        }

                        await DisposeAsync().ConfigureAwait(false);
                        break;
                }

                return false;
            }

            public async Task<IAsyncGrouping<TKey, TElement>[]> ToArrayAsync(CancellationToken cancellationToken)
            {
                IIListProvider<IAsyncGrouping<TKey, TElement>> l = await Internal.LookupWithTask<TKey, TElement>.CreateAsync(source, keySelector, elementSelector, comparer, cancellationToken).ConfigureAwait(false);
                return await l.ToArrayAsync(cancellationToken).ConfigureAwait(false);
            }

            public async Task<List<IAsyncGrouping<TKey, TElement>>> ToListAsync(CancellationToken cancellationToken)
            {
                IIListProvider<IAsyncGrouping<TKey, TElement>> l = await Internal.LookupWithTask<TKey, TElement>.CreateAsync(source, keySelector, elementSelector, comparer, cancellationToken).ConfigureAwait(false);
                return await l.ToListAsync(cancellationToken).ConfigureAwait(false);
            }

            public async Task<int> GetCountAsync(bool onlyIfCheap, CancellationToken cancellationToken)
            {
                if (onlyIfCheap)
                {
                    return -1;
                }

                var l = await Internal.LookupWithTask<TKey, TElement>.CreateAsync(source, keySelector, elementSelector, comparer, cancellationToken).ConfigureAwait(false);

                return l.Count;
            }
        }

        internal sealed class GroupedAsyncEnumerable<TSource, TKey> : AsyncIterator<IAsyncGrouping<TKey, TSource>>, IIListProvider<IAsyncGrouping<TKey, TSource>>
        {
            private readonly IAsyncEnumerable<TSource> source;
            private readonly Func<TSource, TKey> keySelector;
            private readonly IEqualityComparer<TKey> comparer;

            private Internal.Lookup<TKey, TSource> lookup;
            private IEnumerator<IGrouping<TKey, TSource>> enumerator;

            public GroupedAsyncEnumerable(IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
            {
                Debug.Assert(source != null);
                Debug.Assert(keySelector != null);
                Debug.Assert(comparer != null);

                this.source = source;
                this.keySelector = keySelector;
                this.comparer = comparer;
            }

            public override AsyncIterator<IAsyncGrouping<TKey, TSource>> Clone()
            {
                return new GroupedAsyncEnumerable<TSource, TKey>(source, keySelector, comparer);
            }
            public override async Task DisposeAsync()
            {
                if (enumerator != null)
                {
                    enumerator.Dispose();
                    enumerator = null;
                    lookup = null;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async Task<bool> MoveNextCore()
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        lookup = await Internal.Lookup<TKey, TSource>.CreateAsync(source, keySelector, comparer).ConfigureAwait(false);
                        enumerator = lookup.GetEnumerator();
                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        if (enumerator.MoveNext())
                        {
                            current = (IAsyncGrouping<TKey, TSource>)enumerator.Current;
                            return true;
                        }

                        await DisposeAsync().ConfigureAwait(false);
                        break;
                }

                return false;
            }

            public async Task<IAsyncGrouping<TKey, TSource>[]> ToArrayAsync(CancellationToken cancellationToken)
            {
                IIListProvider<IAsyncGrouping<TKey, TSource>> l = await Internal.Lookup<TKey, TSource>.CreateAsync(source, keySelector, comparer, cancellationToken).ConfigureAwait(false);
                return await l.ToArrayAsync(cancellationToken).ConfigureAwait(false);
            }

            public async Task<List<IAsyncGrouping<TKey, TSource>>> ToListAsync(CancellationToken cancellationToken)
            {
                IIListProvider<IAsyncGrouping<TKey, TSource>> l = await Internal.Lookup<TKey, TSource>.CreateAsync(source, keySelector, comparer, cancellationToken).ConfigureAwait(false);
                return await l.ToListAsync(cancellationToken).ConfigureAwait(false);
            }

            public async Task<int> GetCountAsync(bool onlyIfCheap, CancellationToken cancellationToken)
            {
                if (onlyIfCheap)
                {
                    return -1;
                }

                var l = await Internal.Lookup<TKey, TSource>.CreateAsync(source, keySelector, comparer).ConfigureAwait(false);

                return l.Count;
            }
        }

        internal sealed class GroupedAsyncEnumerableWithTask<TSource, TKey> : AsyncIterator<IAsyncGrouping<TKey, TSource>>, IIListProvider<IAsyncGrouping<TKey, TSource>>
        {
            private readonly IAsyncEnumerable<TSource> source;
            private readonly Func<TSource, Task<TKey>> keySelector;
            private readonly IEqualityComparer<TKey> comparer;

            private Internal.LookupWithTask<TKey, TSource> lookup;
            private IEnumerator<IGrouping<TKey, TSource>> enumerator;

            public GroupedAsyncEnumerableWithTask(IAsyncEnumerable<TSource> source, Func<TSource, Task<TKey>> keySelector, IEqualityComparer<TKey> comparer)
            {
                Debug.Assert(source != null);
                Debug.Assert(keySelector != null);
                Debug.Assert(comparer != null);

                this.source = source;
                this.keySelector = keySelector;
                this.comparer = comparer;
            }

            public override AsyncIterator<IAsyncGrouping<TKey, TSource>> Clone()
            {
                return new GroupedAsyncEnumerableWithTask<TSource, TKey>(source, keySelector, comparer);
            }
            public override async Task DisposeAsync()
            {
                if (enumerator != null)
                {
                    enumerator.Dispose();
                    enumerator = null;
                    lookup = null;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async Task<bool> MoveNextCore()
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        lookup = await Internal.LookupWithTask<TKey, TSource>.CreateAsync(source, keySelector, comparer).ConfigureAwait(false);
                        enumerator = lookup.GetEnumerator();
                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        if (enumerator.MoveNext())
                        {
                            current = (IAsyncGrouping<TKey, TSource>)enumerator.Current;
                            return true;
                        }

                        await DisposeAsync().ConfigureAwait(false);
                        break;
                }

                return false;
            }

            public async Task<IAsyncGrouping<TKey, TSource>[]> ToArrayAsync(CancellationToken cancellationToken)
            {
                IIListProvider<IAsyncGrouping<TKey, TSource>> l = await Internal.LookupWithTask<TKey, TSource>.CreateAsync(source, keySelector, comparer, cancellationToken).ConfigureAwait(false);
                return await l.ToArrayAsync(cancellationToken).ConfigureAwait(false);
            }

            public async Task<List<IAsyncGrouping<TKey, TSource>>> ToListAsync(CancellationToken cancellationToken)
            {
                IIListProvider<IAsyncGrouping<TKey, TSource>> l = await Internal.LookupWithTask<TKey, TSource>.CreateAsync(source, keySelector, comparer, cancellationToken).ConfigureAwait(false);
                return await l.ToListAsync(cancellationToken).ConfigureAwait(false);
            }

            public async Task<int> GetCountAsync(bool onlyIfCheap, CancellationToken cancellationToken)
            {
                if (onlyIfCheap)
                {
                    return -1;
                }

                var l = await Internal.LookupWithTask<TKey, TSource>.CreateAsync(source, keySelector, comparer).ConfigureAwait(false);

                return l.Count;
            }
        }
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
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

        public static IAsyncEnumerable<IAsyncGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return new GroupedAsyncEnumerable<TSource, TKey>(source, keySelector, EqualityComparer<TKey>.Default);
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

            return source.GroupBy(keySelector, elementSelector, comparer)
                         .Select(g => resultSelector(g.Key, g));
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

            return source.GroupBy(keySelector, elementSelector, EqualityComparer<TKey>.Default)
                         .Select(g => resultSelector(g.Key, g));
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

        private static IEnumerable<IGrouping<TKey, TElement>> GroupUntil<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IComparer<TKey> comparer)
        {
            var group = default(EnumerableGrouping<TKey, TElement>);
            foreach (var x in source)
            {
                var key = keySelector(x);
                if (group == null || comparer.Compare(group.Key, key) != 0)
                {
                    group = new EnumerableGrouping<TKey, TElement>(key);
                    yield return group;
                }
                group.Add(elementSelector(x));
            }
        }

        internal sealed class GroupedResultAsyncEnumerable<TSource, TKey, TResult> : IIListProvider<TResult>
        {
            private readonly IAsyncEnumerable<TSource> source;
            private readonly Func<TSource, TKey> keySelector;
            private readonly Func<TKey, IAsyncEnumerable<TSource>, TResult> resultSelector;
            private readonly IEqualityComparer<TKey> comparer;

            public GroupedResultAsyncEnumerable(IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TKey, IAsyncEnumerable<TSource>, TResult> resultSelector, IEqualityComparer<TKey> comparer)
            {
                if (source == null) throw new ArgumentNullException(nameof(source));
                if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));
                if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

                this.source = source;
                this.keySelector = keySelector;
                this.resultSelector = resultSelector;
                this.comparer = comparer;
            }


            public IAsyncEnumerator<TResult> GetEnumerator()
            {
                Internal.Lookup<TKey, TSource> lookup = null;
                IEnumerator<TResult> enumerator = null;

                return CreateEnumerator(
                    async ct =>
                    {
                        if (lookup == null)
                        {
                            lookup = await Internal.Lookup<TKey, TSource>.CreateAsync(source, keySelector, comparer, ct).ConfigureAwait(false);
                            enumerator = lookup.ApplyResultSelector(resultSelector).GetEnumerator();
                        }

                        // By the time we get here, the lookup is sync
                        if (ct.IsCancellationRequested)
                            return false;

                        return enumerator?.MoveNext() ?? false;
                    },
                    () => enumerator.Current,
                    () =>
                    {
                        if (enumerator != null)
                        {
                            enumerator.Dispose();
                            enumerator = null;
                        }
                    });
            }

            public async Task<TResult[]> ToArrayAsync(CancellationToken cancellationToken)
            {
                var lookup = await Internal.Lookup<TKey, TSource>.CreateAsync(source, keySelector, comparer, cancellationToken).ConfigureAwait(false);
                return lookup.ToArray(resultSelector);
            }

            public async Task<List<TResult>> ToListAsync(CancellationToken cancellationToken)
            {
                var lookup = await Internal.Lookup<TKey, TSource>.CreateAsync(source, keySelector, comparer, cancellationToken).ConfigureAwait(false);
                return lookup.ToList(resultSelector);
            }

            public async Task<int> GetCountAsync(bool onlyIfCheap, CancellationToken cancellationToken)
            {
                if (onlyIfCheap)
                {
                    return -1;
                }

                var lookup = await Internal.Lookup<TKey, TSource>.CreateAsync(source, keySelector, comparer, cancellationToken).ConfigureAwait(false);

                return lookup.Count;
            }
        }

        internal sealed class GroupedAsyncEnumerable<TSource, TKey, TElement> : IIListProvider<IAsyncGrouping<TKey, TElement>>
        {
            private readonly IAsyncEnumerable<TSource> source;
            private readonly Func<TSource, TKey> keySelector;
            private readonly Func<TSource, TElement> elementSelector;
            private readonly IEqualityComparer<TKey> comparer;

            public GroupedAsyncEnumerable(IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
            {
                if (source == null) throw new ArgumentNullException(nameof(source));
                if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));
                if (elementSelector == null) throw new ArgumentNullException(nameof(elementSelector));

                this.source = source;
                this.keySelector = keySelector;
                this.elementSelector = elementSelector;
                this.comparer = comparer;
            }


            public IAsyncEnumerator<IAsyncGrouping<TKey, TElement>> GetEnumerator()
            {
                Internal.Lookup<TKey, TElement> lookup = null;
                IEnumerator<IGrouping<TKey, TElement>> enumerator = null;

                return CreateEnumerator(
                    async ct =>
                    {
                        if (lookup == null)
                        {
                            lookup = await Internal.Lookup<TKey, TElement>.CreateAsync(source, keySelector, elementSelector, comparer, ct).ConfigureAwait(false);
                            enumerator = lookup.GetEnumerator();
                        }

                        // By the time we get here, the lookup is sync
                        if (ct.IsCancellationRequested)
                            return false;

                        return enumerator?.MoveNext() ?? false;
                    },
                    () => (IAsyncGrouping<TKey, TElement>)enumerator?.Current,
                    () =>
                    {
                        if (enumerator != null)
                        {
                            enumerator.Dispose();
                            enumerator = null;
                        }
                    });
            }

            public async Task<IAsyncGrouping<TKey, TElement>[]> ToArrayAsync(CancellationToken cancellationToken)
            {
                IIListProvider<IAsyncGrouping<TKey, TElement>> lookup = await Internal.Lookup<TKey, TElement>.CreateAsync(source, keySelector, elementSelector, comparer, cancellationToken).ConfigureAwait(false);
                return await lookup.ToArrayAsync(cancellationToken).ConfigureAwait(false);
            }

            public async Task<List<IAsyncGrouping<TKey, TElement>>> ToListAsync(CancellationToken cancellationToken)
            {
                IIListProvider<IAsyncGrouping<TKey, TElement>> lookup = await Internal.Lookup<TKey, TElement>.CreateAsync(source, keySelector, elementSelector, comparer, cancellationToken).ConfigureAwait(false);
                return await lookup.ToListAsync(cancellationToken).ConfigureAwait(false);
            }

            public async Task<int> GetCountAsync(bool onlyIfCheap, CancellationToken cancellationToken)
            {
                if (onlyIfCheap)
                {
                    return -1;
                }

                var lookup = await Internal.Lookup<TKey, TElement>.CreateAsync(source, keySelector, elementSelector, comparer, cancellationToken).ConfigureAwait(false);

                return lookup.Count;
            }
        }

        internal sealed class GroupedAsyncEnumerable<TSource, TKey> : IIListProvider<IAsyncGrouping<TKey, TSource>>
        {
            private readonly IAsyncEnumerable<TSource> source;
            private readonly Func<TSource, TKey> keySelector;
            private readonly IEqualityComparer<TKey> comparer;

            public GroupedAsyncEnumerable(IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
            {
                if (source == null) throw new ArgumentNullException(nameof(source));
                if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));

                this.source = source;
                this.keySelector = keySelector;
                this.comparer = comparer;
            }


            public IAsyncEnumerator<IAsyncGrouping<TKey, TSource>> GetEnumerator()
            {
                Internal.Lookup<TKey, TSource> lookup = null;
                IEnumerator<IGrouping<TKey, TSource>> enumerator = null;

                return CreateEnumerator(
                    async ct =>
                    {
                        if (lookup == null)
                        {
                            lookup = await Internal.Lookup<TKey, TSource>.CreateAsync(source, keySelector, comparer, ct).ConfigureAwait(false);
                            enumerator = lookup.GetEnumerator();
                        }

                        // By the time we get here, the lookup is sync
                        if (ct.IsCancellationRequested)
                            return false;

                        return enumerator?.MoveNext() ?? false;
                    },
                    () => (IAsyncGrouping<TKey, TSource>)enumerator?.Current,
                    () =>
                        {
                            if (enumerator != null)
                            {
                                enumerator.Dispose();
                                enumerator = null;
                            }
                        });
            }

            public async Task<IAsyncGrouping<TKey, TSource>[]> ToArrayAsync(CancellationToken cancellationToken)
            {
                IIListProvider<IAsyncGrouping<TKey, TSource>> lookup = await Internal.Lookup<TKey, TSource>.CreateAsync(source, keySelector, comparer, cancellationToken).ConfigureAwait(false);
                return await lookup.ToArrayAsync(cancellationToken).ConfigureAwait(false);
            }

            public async Task<List<IAsyncGrouping<TKey, TSource>>> ToListAsync(CancellationToken cancellationToken)
            {
                IIListProvider<IAsyncGrouping<TKey, TSource>> lookup = await Internal.Lookup<TKey, TSource>.CreateAsync(source, keySelector, comparer, cancellationToken).ConfigureAwait(false);
                return await lookup.ToListAsync(cancellationToken).ConfigureAwait(false);
            }

            public async Task<int> GetCountAsync(bool onlyIfCheap, CancellationToken cancellationToken)
            {
                if (onlyIfCheap)
                {
                    return -1;
                }

                var lookup = await Internal.Lookup<TKey, TSource>.CreateAsync(source, keySelector, comparer, cancellationToken).ConfigureAwait(false);

                return lookup.Count;
            }
        }

    }
}

// Note: The type here has to be internal as System.Linq has it's own public copy we're not using

namespace System.Linq.Internal
{
    /// Adapted from System.Linq.Grouping from .NET Framework
    /// Source: https://github.com/dotnet/corefx/blob/b90532bc97b07234a7d18073819d019645285f1c/src/System.Linq/src/System/Linq/Grouping.cs#L64
    internal class Grouping<TKey, TElement> : IGrouping<TKey, TElement>, IList<TElement>, IAsyncGrouping<TKey, TElement>
    {
        internal int _count;
        internal TElement[] _elements;
        internal int _hashCode;
        internal Grouping<TKey, TElement> _hashNext;
        internal TKey _key;
        internal Grouping<TKey, TElement> _next;

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<TElement> GetEnumerator()
        {
            for (var i = 0; i < _count; i++)
            {
                yield return _elements[i];
            }
        }

        // DDB195907: implement IGrouping<>.Key implicitly
        // so that WPF binding works on this property.
        public TKey Key
        {
            get { return _key; }
        }

        int ICollection<TElement>.Count
        {
            get { return _count; }
        }

        bool ICollection<TElement>.IsReadOnly
        {
            get { return true; }
        }

        void ICollection<TElement>.Add(TElement item)
        {
            throw new NotSupportedException(Strings.NOT_SUPPORTED);
        }

        void ICollection<TElement>.Clear()
        {
            throw new NotSupportedException(Strings.NOT_SUPPORTED);
        }

        bool ICollection<TElement>.Contains(TElement item)
        {
            return Array.IndexOf(_elements, item, 0, _count) >= 0;
        }

        void ICollection<TElement>.CopyTo(TElement[] array, int arrayIndex)
        {
            Array.Copy(_elements, 0, array, arrayIndex, _count);
        }

        bool ICollection<TElement>.Remove(TElement item)
        {
            throw new NotSupportedException(Strings.NOT_SUPPORTED);
        }

        int IList<TElement>.IndexOf(TElement item)
        {
            return Array.IndexOf(_elements, item, 0, _count);
        }

        void IList<TElement>.Insert(int index, TElement item)
        {
            throw new NotSupportedException(Strings.NOT_SUPPORTED);
        }

        void IList<TElement>.RemoveAt(int index)
        {
            throw new NotSupportedException(Strings.NOT_SUPPORTED);
        }

        TElement IList<TElement>.this[int index]
        {
            get
            {
                if (index < 0 || index >= _count)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                return _elements[index];
            }

            set { throw new NotSupportedException(Strings.NOT_SUPPORTED); }
        }

        internal void Add(TElement element)
        {
            if (_elements.Length == _count)
            {
                Array.Resize(ref _elements, checked(_count*2));
            }

            _elements[_count] = element;
            _count++;
        }

        internal void Trim()
        {
            if (_elements.Length != _count)
            {
                Array.Resize(ref _elements, _count);
            }
        }

        IAsyncEnumerator<TElement> IAsyncEnumerable<TElement>.GetEnumerator()
        {
            return this.ToAsyncEnumerable().GetEnumerator();
        }
    }
}
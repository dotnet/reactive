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
                throw Error.ArgumentNull(nameof(source));
            if (keySelector == null)
                throw Error.ArgumentNull(nameof(keySelector));

            return new GroupedAsyncEnumerable<TSource, TKey>(source, keySelector, comparer: null);
        }

        public static IAsyncEnumerable<IAsyncGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (keySelector == null)
                throw Error.ArgumentNull(nameof(keySelector));

            return new GroupedAsyncEnumerable<TSource, TKey>(source, keySelector, comparer);
        }

        public static IAsyncEnumerable<IAsyncGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<TKey>> keySelector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (keySelector == null)
                throw Error.ArgumentNull(nameof(keySelector));

            return new GroupedAsyncEnumerableWithTask<TSource, TKey>(source, keySelector, comparer: null);
        }

        public static IAsyncEnumerable<IAsyncGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<TKey>> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (keySelector == null)
                throw Error.ArgumentNull(nameof(keySelector));

            return new GroupedAsyncEnumerableWithTask<TSource, TKey>(source, keySelector, comparer);
        }

        public static IAsyncEnumerable<IAsyncGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (keySelector == null)
                throw Error.ArgumentNull(nameof(keySelector));
            if (elementSelector == null)
                throw Error.ArgumentNull(nameof(elementSelector));

            return new GroupedAsyncEnumerable<TSource, TKey, TElement>(source, keySelector, elementSelector, comparer: null);
        }

        public static IAsyncEnumerable<IAsyncGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (keySelector == null)
                throw Error.ArgumentNull(nameof(keySelector));
            if (elementSelector == null)
                throw Error.ArgumentNull(nameof(elementSelector));

            return new GroupedAsyncEnumerable<TSource, TKey, TElement>(source, keySelector, elementSelector, comparer);
        }

        public static IAsyncEnumerable<IAsyncGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<TKey>> keySelector, Func<TSource, Task<TElement>> elementSelector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (keySelector == null)
                throw Error.ArgumentNull(nameof(keySelector));
            if (elementSelector == null)
                throw Error.ArgumentNull(nameof(elementSelector));

            return new GroupedAsyncEnumerableWithTask<TSource, TKey, TElement>(source, keySelector, elementSelector, comparer: null);
        }

        public static IAsyncEnumerable<IAsyncGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<TKey>> keySelector, Func<TSource, Task<TElement>> elementSelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (keySelector == null)
                throw Error.ArgumentNull(nameof(keySelector));
            if (elementSelector == null)
                throw Error.ArgumentNull(nameof(elementSelector));

            return new GroupedAsyncEnumerableWithTask<TSource, TKey, TElement>(source, keySelector, elementSelector, comparer);
        }

        public static IAsyncEnumerable<TResult> GroupBy<TSource, TKey, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TKey, IAsyncEnumerable<TSource>, TResult> resultSelector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (keySelector == null)
                throw Error.ArgumentNull(nameof(keySelector));
            if (resultSelector == null)
                throw Error.ArgumentNull(nameof(resultSelector));

            return new GroupedResultAsyncEnumerable<TSource, TKey, TResult>(source, keySelector, resultSelector, comparer: null);
        }

        public static IAsyncEnumerable<TResult> GroupBy<TSource, TKey, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TKey, IAsyncEnumerable<TSource>, TResult> resultSelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (keySelector == null)
                throw Error.ArgumentNull(nameof(keySelector));
            if (resultSelector == null)
                throw Error.ArgumentNull(nameof(resultSelector));

            return new GroupedResultAsyncEnumerable<TSource, TKey, TResult>(source, keySelector, resultSelector, comparer);
        }

        public static IAsyncEnumerable<TResult> GroupBy<TSource, TKey, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<TKey>> keySelector, Func<TKey, IAsyncEnumerable<TSource>, Task<TResult>> resultSelector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (keySelector == null)
                throw Error.ArgumentNull(nameof(keySelector));
            if (resultSelector == null)
                throw Error.ArgumentNull(nameof(resultSelector));

            return new GroupedResultAsyncEnumerableWithTask<TSource, TKey, TResult>(source, keySelector, resultSelector, comparer: null);
        }

        public static IAsyncEnumerable<TResult> GroupBy<TSource, TKey, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<TKey>> keySelector, Func<TKey, IAsyncEnumerable<TSource>, Task<TResult>> resultSelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (keySelector == null)
                throw Error.ArgumentNull(nameof(keySelector));
            if (resultSelector == null)
                throw Error.ArgumentNull(nameof(resultSelector));

            return new GroupedResultAsyncEnumerableWithTask<TSource, TKey, TResult>(source, keySelector, resultSelector, comparer);
        }

        public static IAsyncEnumerable<TResult> GroupBy<TSource, TKey, TElement, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<TKey, IAsyncEnumerable<TElement>, TResult> resultSelector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (keySelector == null)
                throw Error.ArgumentNull(nameof(keySelector));
            if (elementSelector == null)
                throw Error.ArgumentNull(nameof(elementSelector));
            if (resultSelector == null)
                throw Error.ArgumentNull(nameof(resultSelector));

            return source.GroupBy(keySelector, elementSelector, comparer: null).Select(g => resultSelector(g.Key, g));
        }

        public static IAsyncEnumerable<TResult> GroupBy<TSource, TKey, TElement, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<TKey, IAsyncEnumerable<TElement>, TResult> resultSelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (keySelector == null)
                throw Error.ArgumentNull(nameof(keySelector));
            if (elementSelector == null)
                throw Error.ArgumentNull(nameof(elementSelector));
            if (resultSelector == null)
                throw Error.ArgumentNull(nameof(resultSelector));

            return source.GroupBy(keySelector, elementSelector, comparer).Select(g => resultSelector(g.Key, g));
        }

        public static IAsyncEnumerable<TResult> GroupBy<TSource, TKey, TElement, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<TKey>> keySelector, Func<TSource, Task<TElement>> elementSelector, Func<TKey, IAsyncEnumerable<TElement>, Task<TResult>> resultSelector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (keySelector == null)
                throw Error.ArgumentNull(nameof(keySelector));
            if (elementSelector == null)
                throw Error.ArgumentNull(nameof(elementSelector));
            if (resultSelector == null)
                throw Error.ArgumentNull(nameof(resultSelector));

            return source.GroupBy<TSource, TKey, TElement>(keySelector, elementSelector, comparer: null).Select(g => resultSelector(g.Key, g));
        }

        public static IAsyncEnumerable<TResult> GroupBy<TSource, TKey, TElement, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<TKey>> keySelector, Func<TSource, Task<TElement>> elementSelector, Func<TKey, IAsyncEnumerable<TElement>, Task<TResult>> resultSelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (keySelector == null)
                throw Error.ArgumentNull(nameof(keySelector));
            if (elementSelector == null)
                throw Error.ArgumentNull(nameof(elementSelector));
            if (resultSelector == null)
                throw Error.ArgumentNull(nameof(resultSelector));

            return source.GroupBy(keySelector, elementSelector, comparer).Select(g => resultSelector(g.Key, g));
        }

        internal sealed class GroupedResultAsyncEnumerable<TSource, TKey, TResult> : AsyncIterator<TResult>, IAsyncIListProvider<TResult>
        {
            private readonly IAsyncEnumerable<TSource> _source;
            private readonly Func<TSource, TKey> _keySelector;
            private readonly Func<TKey, IAsyncEnumerable<TSource>, TResult> _resultSelector;
            private readonly IEqualityComparer<TKey> _comparer;

            private Internal.Lookup<TKey, TSource> _lookup;
            private IEnumerator<TResult> _enumerator;

            public GroupedResultAsyncEnumerable(IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TKey, IAsyncEnumerable<TSource>, TResult> resultSelector, IEqualityComparer<TKey> comparer)
            {
                Debug.Assert(source != null);
                Debug.Assert(keySelector != null);
                Debug.Assert(resultSelector != null);

                _source = source;
                _keySelector = keySelector;
                _resultSelector = resultSelector;
                _comparer = comparer;
            }

            public override AsyncIterator<TResult> Clone()
            {
                return new GroupedResultAsyncEnumerable<TSource, TKey, TResult>(_source, _keySelector, _resultSelector, _comparer);
            }

            public override async ValueTask DisposeAsync()
            {
                if (_enumerator != null)
                {
                    _enumerator.Dispose();
                    _enumerator = null;
                    _lookup = null;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async ValueTask<bool> MoveNextCore()
            {
                switch (_state)
                {
                    case AsyncIteratorState.Allocated:
                        _lookup = await Internal.Lookup<TKey, TSource>.CreateAsync(_source, _keySelector, _comparer, _cancellationToken).ConfigureAwait(false);
                        _enumerator = _lookup.ApplyResultSelector(_resultSelector).GetEnumerator();
                        _state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        if (_enumerator.MoveNext())
                        {
                            _current = _enumerator.Current;
                            return true;
                        }

                        await DisposeAsync().ConfigureAwait(false);
                        break;
                }

                return false;
            }

            public async Task<TResult[]> ToArrayAsync(CancellationToken cancellationToken)
            {
                var l = await Internal.Lookup<TKey, TSource>.CreateAsync(_source, _keySelector, _comparer, cancellationToken).ConfigureAwait(false);
                return l.ToArray(_resultSelector);
            }

            public async Task<List<TResult>> ToListAsync(CancellationToken cancellationToken)
            {
                var l = await Internal.Lookup<TKey, TSource>.CreateAsync(_source, _keySelector, _comparer, cancellationToken).ConfigureAwait(false);
                return l.ToList(_resultSelector);
            }

            public Task<int> GetCountAsync(bool onlyIfCheap, CancellationToken cancellationToken)
            {
                if (onlyIfCheap)
                {
                    return TaskExt.MinusOne;
                }

                return Core();

                async Task<int> Core()
                {
                    var l = await Internal.Lookup<TKey, TSource>.CreateAsync(_source, _keySelector, _comparer, cancellationToken).ConfigureAwait(false);

                    return l.Count;
                }
            }
        }

        internal sealed class GroupedResultAsyncEnumerableWithTask<TSource, TKey, TResult> : AsyncIterator<TResult>, IAsyncIListProvider<TResult>
        {
            private readonly IAsyncEnumerable<TSource> _source;
            private readonly Func<TSource, Task<TKey>> _keySelector;
            private readonly Func<TKey, IAsyncEnumerable<TSource>, Task<TResult>> _resultSelector;
            private readonly IEqualityComparer<TKey> _comparer;

            private Internal.LookupWithTask<TKey, TSource> _lookup;
            private IAsyncEnumerator<TResult> _enumerator;

            public GroupedResultAsyncEnumerableWithTask(IAsyncEnumerable<TSource> source, Func<TSource, Task<TKey>> keySelector, Func<TKey, IAsyncEnumerable<TSource>, Task<TResult>> resultSelector, IEqualityComparer<TKey> comparer)
            {
                Debug.Assert(source != null);
                Debug.Assert(keySelector != null);
                Debug.Assert(resultSelector != null);

                _source = source;
                _keySelector = keySelector;
                _resultSelector = resultSelector;
                _comparer = comparer;
            }

            public override AsyncIterator<TResult> Clone()
            {
                return new GroupedResultAsyncEnumerableWithTask<TSource, TKey, TResult>(_source, _keySelector, _resultSelector, _comparer);
            }

            public override async ValueTask DisposeAsync()
            {
                if (_enumerator != null)
                {
                    await _enumerator.DisposeAsync().ConfigureAwait(false);
                    _enumerator = null;
                    _lookup = null;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async ValueTask<bool> MoveNextCore()
            {
                switch (_state)
                {
                    case AsyncIteratorState.Allocated:
                        _lookup = await Internal.LookupWithTask<TKey, TSource>.CreateAsync(_source, _keySelector, _comparer, _cancellationToken).ConfigureAwait(false);
                        _enumerator = _lookup.Select(async g => await _resultSelector(g.Key, g).ConfigureAwait(false)).GetAsyncEnumerator(_cancellationToken);
                        _state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        if (await _enumerator.MoveNextAsync().ConfigureAwait(false))
                        {
                            _current = _enumerator.Current;
                            return true;
                        }

                        await DisposeAsync().ConfigureAwait(false);
                        break;
                }

                return false;
            }

            public async Task<TResult[]> ToArrayAsync(CancellationToken cancellationToken)
            {
                var l = await Internal.LookupWithTask<TKey, TSource>.CreateAsync(_source, _keySelector, _comparer, cancellationToken).ConfigureAwait(false);
                return await l.ToArray(_resultSelector).ConfigureAwait(false);
            }

            public async Task<List<TResult>> ToListAsync(CancellationToken cancellationToken)
            {
                var l = await Internal.LookupWithTask<TKey, TSource>.CreateAsync(_source, _keySelector, _comparer, cancellationToken).ConfigureAwait(false);
                return await l.ToList(_resultSelector).ConfigureAwait(false);
            }

            public Task<int> GetCountAsync(bool onlyIfCheap, CancellationToken cancellationToken)
            {
                if (onlyIfCheap)
                {
                    return TaskExt.MinusOne;
                }

                return Core();

                async Task<int> Core()
                {
                    var l = await Internal.LookupWithTask<TKey, TSource>.CreateAsync(_source, _keySelector, _comparer, cancellationToken).ConfigureAwait(false);

                    return l.Count;
                }
            }
        }

        internal sealed class GroupedAsyncEnumerable<TSource, TKey, TElement> : AsyncIterator<IAsyncGrouping<TKey, TElement>>, IAsyncIListProvider<IAsyncGrouping<TKey, TElement>>
        {
            private readonly IAsyncEnumerable<TSource> _source;
            private readonly Func<TSource, TKey> _keySelector;
            private readonly Func<TSource, TElement> _elementSelector;
            private readonly IEqualityComparer<TKey> _comparer;

            private Internal.Lookup<TKey, TElement> _lookup;
            private IEnumerator<IGrouping<TKey, TElement>> _enumerator;

            public GroupedAsyncEnumerable(IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
            {
                Debug.Assert(source != null);
                Debug.Assert(keySelector != null);
                Debug.Assert(elementSelector != null);

                _source = source;
                _keySelector = keySelector;
                _elementSelector = elementSelector;
                _comparer = comparer;
            }

            public override AsyncIterator<IAsyncGrouping<TKey, TElement>> Clone()
            {
                return new GroupedAsyncEnumerable<TSource, TKey, TElement>(_source, _keySelector, _elementSelector, _comparer);
            }

            public override async ValueTask DisposeAsync()
            {
                if (_enumerator != null)
                {
                    _enumerator.Dispose();
                    _enumerator = null;
                    _lookup = null;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async ValueTask<bool> MoveNextCore()
            {
                switch (_state)
                {
                    case AsyncIteratorState.Allocated:
                        _lookup = await Internal.Lookup<TKey, TElement>.CreateAsync(_source, _keySelector, _elementSelector, _comparer, _cancellationToken).ConfigureAwait(false);
                        _enumerator = _lookup.GetEnumerator();
                        _state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        if (_enumerator.MoveNext())
                        {
                            _current = (IAsyncGrouping<TKey, TElement>)_enumerator.Current;
                            return true;
                        }

                        await DisposeAsync().ConfigureAwait(false);
                        break;
                }

                return false;
            }
            
            public async Task<IAsyncGrouping<TKey, TElement>[]> ToArrayAsync(CancellationToken cancellationToken)
            {
                IAsyncIListProvider<IAsyncGrouping<TKey, TElement>> l = await Internal.Lookup<TKey, TElement>.CreateAsync(_source, _keySelector, _elementSelector, _comparer, cancellationToken).ConfigureAwait(false);
                return await l.ToArrayAsync(cancellationToken).ConfigureAwait(false);
            }

            public async Task<List<IAsyncGrouping<TKey, TElement>>> ToListAsync(CancellationToken cancellationToken)
            {
                IAsyncIListProvider<IAsyncGrouping<TKey, TElement>> l = await Internal.Lookup<TKey, TElement>.CreateAsync(_source, _keySelector, _elementSelector, _comparer, cancellationToken).ConfigureAwait(false);
                return await l.ToListAsync(cancellationToken).ConfigureAwait(false);
            }

            public Task<int> GetCountAsync(bool onlyIfCheap, CancellationToken cancellationToken)
            {
                if (onlyIfCheap)
                {
                    return TaskExt.MinusOne;
                }

                return Core();

                async Task<int> Core()
                {
                    var l = await Internal.Lookup<TKey, TElement>.CreateAsync(_source, _keySelector, _elementSelector, _comparer, cancellationToken).ConfigureAwait(false);

                    return l.Count;
                }
            }
        }

        internal sealed class GroupedAsyncEnumerableWithTask<TSource, TKey, TElement> : AsyncIterator<IAsyncGrouping<TKey, TElement>>, IAsyncIListProvider<IAsyncGrouping<TKey, TElement>>
        {
            private readonly IAsyncEnumerable<TSource> _source;
            private readonly Func<TSource, Task<TKey>> _keySelector;
            private readonly Func<TSource, Task<TElement>> _elementSelector;
            private readonly IEqualityComparer<TKey> _comparer;

            private Internal.LookupWithTask<TKey, TElement> _lookup;
            private IEnumerator<IGrouping<TKey, TElement>> _enumerator;

            public GroupedAsyncEnumerableWithTask(IAsyncEnumerable<TSource> source, Func<TSource, Task<TKey>> keySelector, Func<TSource, Task<TElement>> elementSelector, IEqualityComparer<TKey> comparer)
            {
                Debug.Assert(source != null);
                Debug.Assert(keySelector != null);
                Debug.Assert(elementSelector != null);

                _source = source;
                _keySelector = keySelector;
                _elementSelector = elementSelector;
                _comparer = comparer;
            }

            public override AsyncIterator<IAsyncGrouping<TKey, TElement>> Clone()
            {
                return new GroupedAsyncEnumerableWithTask<TSource, TKey, TElement>(_source, _keySelector, _elementSelector, _comparer);
            }

            public override async ValueTask DisposeAsync()
            {
                if (_enumerator != null)
                {
                    _enumerator.Dispose();
                    _enumerator = null;
                    _lookup = null;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async ValueTask<bool> MoveNextCore()
            {
                switch (_state)
                {
                    case AsyncIteratorState.Allocated:
                        _lookup = await Internal.LookupWithTask<TKey, TElement>.CreateAsync(_source, _keySelector, _elementSelector, _comparer, _cancellationToken).ConfigureAwait(false);
                        _enumerator = _lookup.GetEnumerator();
                        _state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        if (_enumerator.MoveNext())
                        {
                            _current = (IAsyncGrouping<TKey, TElement>)_enumerator.Current;
                            return true;
                        }

                        await DisposeAsync().ConfigureAwait(false);
                        break;
                }

                return false;
            }

            public async Task<IAsyncGrouping<TKey, TElement>[]> ToArrayAsync(CancellationToken cancellationToken)
            {
                IAsyncIListProvider<IAsyncGrouping<TKey, TElement>> l = await Internal.LookupWithTask<TKey, TElement>.CreateAsync(_source, _keySelector, _elementSelector, _comparer, cancellationToken).ConfigureAwait(false);
                return await l.ToArrayAsync(cancellationToken).ConfigureAwait(false);
            }

            public async Task<List<IAsyncGrouping<TKey, TElement>>> ToListAsync(CancellationToken cancellationToken)
            {
                IAsyncIListProvider<IAsyncGrouping<TKey, TElement>> l = await Internal.LookupWithTask<TKey, TElement>.CreateAsync(_source, _keySelector, _elementSelector, _comparer, cancellationToken).ConfigureAwait(false);
                return await l.ToListAsync(cancellationToken).ConfigureAwait(false);
            }

            public Task<int> GetCountAsync(bool onlyIfCheap, CancellationToken cancellationToken)
            {
                if (onlyIfCheap)
                {
                    return TaskExt.MinusOne;
                }

                return Core();

                async Task<int> Core()
                {
                    var l = await Internal.LookupWithTask<TKey, TElement>.CreateAsync(_source, _keySelector, _elementSelector, _comparer, cancellationToken).ConfigureAwait(false);

                    return l.Count;
                }
            }
        }

        internal sealed class GroupedAsyncEnumerable<TSource, TKey> : AsyncIterator<IAsyncGrouping<TKey, TSource>>, IAsyncIListProvider<IAsyncGrouping<TKey, TSource>>
        {
            private readonly IAsyncEnumerable<TSource> _source;
            private readonly Func<TSource, TKey> _keySelector;
            private readonly IEqualityComparer<TKey> _comparer;

            private Internal.Lookup<TKey, TSource> _lookup;
            private IEnumerator<IGrouping<TKey, TSource>> _enumerator;

            public GroupedAsyncEnumerable(IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
            {
                Debug.Assert(source != null);
                Debug.Assert(keySelector != null);

                _source = source;
                _keySelector = keySelector;
                _comparer = comparer;
            }

            public override AsyncIterator<IAsyncGrouping<TKey, TSource>> Clone()
            {
                return new GroupedAsyncEnumerable<TSource, TKey>(_source, _keySelector, _comparer);
            }
            public override async ValueTask DisposeAsync()
            {
                if (_enumerator != null)
                {
                    _enumerator.Dispose();
                    _enumerator = null;
                    _lookup = null;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async ValueTask<bool> MoveNextCore()
            {
                switch (_state)
                {
                    case AsyncIteratorState.Allocated:
                        _lookup = await Internal.Lookup<TKey, TSource>.CreateAsync(_source, _keySelector, _comparer, _cancellationToken).ConfigureAwait(false);
                        _enumerator = _lookup.GetEnumerator();
                        _state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        if (_enumerator.MoveNext())
                        {
                            _current = (IAsyncGrouping<TKey, TSource>)_enumerator.Current;
                            return true;
                        }

                        await DisposeAsync().ConfigureAwait(false);
                        break;
                }

                return false;
            }

            public async Task<IAsyncGrouping<TKey, TSource>[]> ToArrayAsync(CancellationToken cancellationToken)
            {
                IAsyncIListProvider<IAsyncGrouping<TKey, TSource>> l = await Internal.Lookup<TKey, TSource>.CreateAsync(_source, _keySelector, _comparer, cancellationToken).ConfigureAwait(false);
                return await l.ToArrayAsync(cancellationToken).ConfigureAwait(false);
            }

            public async Task<List<IAsyncGrouping<TKey, TSource>>> ToListAsync(CancellationToken cancellationToken)
            {
                IAsyncIListProvider<IAsyncGrouping<TKey, TSource>> l = await Internal.Lookup<TKey, TSource>.CreateAsync(_source, _keySelector, _comparer, cancellationToken).ConfigureAwait(false);
                return await l.ToListAsync(cancellationToken).ConfigureAwait(false);
            }

            public Task<int> GetCountAsync(bool onlyIfCheap, CancellationToken cancellationToken)
            {
                if (onlyIfCheap)
                {
                    return TaskExt.MinusOne;
                }

                return Core();

                async Task<int> Core()
                {
                    var l = await Internal.Lookup<TKey, TSource>.CreateAsync(_source, _keySelector, _comparer, cancellationToken).ConfigureAwait(false);

                    return l.Count;
                }
            }
        }

        internal sealed class GroupedAsyncEnumerableWithTask<TSource, TKey> : AsyncIterator<IAsyncGrouping<TKey, TSource>>, IAsyncIListProvider<IAsyncGrouping<TKey, TSource>>
        {
            private readonly IAsyncEnumerable<TSource> _source;
            private readonly Func<TSource, Task<TKey>> _keySelector;
            private readonly IEqualityComparer<TKey> _comparer;

            private Internal.LookupWithTask<TKey, TSource> _lookup;
            private IEnumerator<IGrouping<TKey, TSource>> _enumerator;

            public GroupedAsyncEnumerableWithTask(IAsyncEnumerable<TSource> source, Func<TSource, Task<TKey>> keySelector, IEqualityComparer<TKey> comparer)
            {
                Debug.Assert(source != null);
                Debug.Assert(keySelector != null);

                _source = source;
                _keySelector = keySelector;
                _comparer = comparer;
            }

            public override AsyncIterator<IAsyncGrouping<TKey, TSource>> Clone()
            {
                return new GroupedAsyncEnumerableWithTask<TSource, TKey>(_source, _keySelector, _comparer);
            }
            public override async ValueTask DisposeAsync()
            {
                if (_enumerator != null)
                {
                    _enumerator.Dispose();
                    _enumerator = null;
                    _lookup = null;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async ValueTask<bool> MoveNextCore()
            {
                switch (_state)
                {
                    case AsyncIteratorState.Allocated:
                        _lookup = await Internal.LookupWithTask<TKey, TSource>.CreateAsync(_source, _keySelector, _comparer, _cancellationToken).ConfigureAwait(false);
                        _enumerator = _lookup.GetEnumerator();
                        _state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        if (_enumerator.MoveNext())
                        {
                            _current = (IAsyncGrouping<TKey, TSource>)_enumerator.Current;
                            return true;
                        }

                        await DisposeAsync().ConfigureAwait(false);
                        break;
                }

                return false;
            }

            public async Task<IAsyncGrouping<TKey, TSource>[]> ToArrayAsync(CancellationToken cancellationToken)
            {
                IAsyncIListProvider<IAsyncGrouping<TKey, TSource>> l = await Internal.LookupWithTask<TKey, TSource>.CreateAsync(_source, _keySelector, _comparer, cancellationToken).ConfigureAwait(false);
                return await l.ToArrayAsync(cancellationToken).ConfigureAwait(false);
            }

            public async Task<List<IAsyncGrouping<TKey, TSource>>> ToListAsync(CancellationToken cancellationToken)
            {
                IAsyncIListProvider<IAsyncGrouping<TKey, TSource>> l = await Internal.LookupWithTask<TKey, TSource>.CreateAsync(_source, _keySelector, _comparer, cancellationToken).ConfigureAwait(false);
                return await l.ToListAsync(cancellationToken).ConfigureAwait(false);
            }

            public Task<int> GetCountAsync(bool onlyIfCheap, CancellationToken cancellationToken)
            {
                if (onlyIfCheap)
                {
                    return TaskExt.MinusOne;
                }

                return Core();

                async Task<int> Core()
                {
                    var l = await Internal.LookupWithTask<TKey, TSource>.CreateAsync(_source, _keySelector, _comparer, cancellationToken).ConfigureAwait(false);

                    return l.Count;
                }
            }
        }
    }
}

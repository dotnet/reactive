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
        public static IAsyncEnumerable<TResult> Select<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            switch (source)
            {
                case AsyncIterator<TSource> iterator:
                    return iterator.Select(selector);
                case IList<TSource> list:
                    return new SelectIListIterator<TSource, TResult>(list, selector);
            }

            return new SelectEnumerableAsyncIterator<TSource, TResult>(source, selector);
        }

        public static IAsyncEnumerable<TResult> Select<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, int, TResult> selector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return new SelectEnumerableWithIndexAsyncIterator<TSource, TResult>(source, selector);
        }

        public static IAsyncEnumerable<TResult> Select<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TResult>> selector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            switch (source)
            {
                case AsyncIterator<TSource> iterator:
                    return iterator.Select(selector);
                case IList<TSource> list:
                    return new SelectIListIteratorWithTask<TSource, TResult>(list, selector);
            }

            return new SelectEnumerableAsyncIteratorWithTask<TSource, TResult>(source, selector);
        }

#if !NO_DEEP_CANCELLATION
        public static IAsyncEnumerable<TResult> Select<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TResult>> selector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            switch (source)
            {
                case AsyncIterator<TSource> iterator:
                    return iterator.Select(selector);
                case IList<TSource> list:
                    return new SelectIListIteratorWithTaskAndCancellation<TSource, TResult>(list, selector);
            }

            return new SelectEnumerableAsyncIteratorWithTaskAndCancellation<TSource, TResult>(source, selector);
        }
#endif

        public static IAsyncEnumerable<TResult> Select<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, int, ValueTask<TResult>> selector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return new SelectEnumerableWithIndexAsyncIteratorWithTask<TSource, TResult>(source, selector);
        }

#if !NO_DEEP_CANCELLATION
        public static IAsyncEnumerable<TResult> Select<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, int, CancellationToken, ValueTask<TResult>> selector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return new SelectEnumerableWithIndexAsyncIteratorWithTaskAndCancellation<TSource, TResult>(source, selector);
        }
#endif

        private static Func<TSource, TResult> CombineSelectors<TSource, TMiddle, TResult>(Func<TSource, TMiddle> selector1, Func<TMiddle, TResult> selector2)
        {
            return x => selector2(selector1(x));
        }

        private static Func<TSource, ValueTask<TResult>> CombineSelectors<TSource, TMiddle, TResult>(Func<TSource, ValueTask<TMiddle>> selector1, Func<TMiddle, ValueTask<TResult>> selector2)
        {
            return async x => await selector2(await selector1(x).ConfigureAwait(false)).ConfigureAwait(false);
        }

#if !NO_DEEP_CANCELLATION
        private static Func<TSource, CancellationToken, ValueTask<TResult>> CombineSelectors<TSource, TMiddle, TResult>(Func<TSource, CancellationToken, ValueTask<TMiddle>> selector1, Func<TMiddle, CancellationToken, ValueTask<TResult>> selector2)
        {
            return async (x, ct) => await selector2(await selector1(x, ct).ConfigureAwait(false), ct).ConfigureAwait(false);
        }
#endif

        internal sealed class SelectEnumerableAsyncIterator<TSource, TResult> : AsyncIterator<TResult>
        {
            private readonly Func<TSource, TResult> _selector;
            private readonly IAsyncEnumerable<TSource> _source;

            private IAsyncEnumerator<TSource> _enumerator;

            public SelectEnumerableAsyncIterator(IAsyncEnumerable<TSource> source, Func<TSource, TResult> selector)
            {
                Debug.Assert(source != null);
                Debug.Assert(selector != null);

                _source = source;
                _selector = selector;
            }

            public override AsyncIteratorBase<TResult> Clone()
            {
                return new SelectEnumerableAsyncIterator<TSource, TResult>(_source, _selector);
            }

            public override async ValueTask DisposeAsync()
            {
                if (_enumerator != null)
                {
                    await _enumerator.DisposeAsync().ConfigureAwait(false);
                    _enumerator = null;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            public override IAsyncEnumerable<TResult1> Select<TResult1>(Func<TResult, TResult1> selector)
            {
                return new SelectEnumerableAsyncIterator<TSource, TResult1>(_source, CombineSelectors(_selector, selector));
            }

            protected override async ValueTask<bool> MoveNextCore()
            {
                switch (_state)
                {
                    case AsyncIteratorState.Allocated:
                        _enumerator = _source.GetAsyncEnumerator(_cancellationToken);
                        _state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        if (await _enumerator.MoveNextAsync().ConfigureAwait(false))
                        {
                            _current = _selector(_enumerator.Current);
                            return true;
                        }

                        break;
                }

                await DisposeAsync().ConfigureAwait(false);
                return false;
            }
        }

        private sealed class SelectEnumerableWithIndexAsyncIterator<TSource, TResult> : AsyncIterator<TResult>
        {
            private readonly Func<TSource, int, TResult> _selector;
            private readonly IAsyncEnumerable<TSource> _source;
            private IAsyncEnumerator<TSource> _enumerator;
            private int _index;

            public SelectEnumerableWithIndexAsyncIterator(IAsyncEnumerable<TSource> source, Func<TSource, int, TResult> selector)
            {
                Debug.Assert(source != null);
                Debug.Assert(selector != null);

                _source = source;
                _selector = selector;
            }

            public override AsyncIteratorBase<TResult> Clone()
            {
                return new SelectEnumerableWithIndexAsyncIterator<TSource, TResult>(_source, _selector);
            }

            public override async ValueTask DisposeAsync()
            {
                if (_enumerator != null)
                {
                    await _enumerator.DisposeAsync().ConfigureAwait(false);
                    _enumerator = null;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async ValueTask<bool> MoveNextCore()
            {
                switch (_state)
                {
                    case AsyncIteratorState.Allocated:
                        _enumerator = _source.GetAsyncEnumerator(_cancellationToken);
                        _index = -1;
                        _state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        if (await _enumerator.MoveNextAsync().ConfigureAwait(false))
                        {
                            var item = _enumerator.Current;

                            checked
                            {
                                _index++;
                            }

                            _current = _selector(item, _index);
                            return true;
                        }

                        break;
                }

                await DisposeAsync().ConfigureAwait(false);
                return false;
            }
        }

        internal sealed class SelectIListIterator<TSource, TResult> : AsyncIterator<TResult>, IAsyncIListProvider<TResult>
        {
            private readonly Func<TSource, TResult> _selector;
            private readonly IList<TSource> _source;
            private IEnumerator<TSource> _enumerator;

            public SelectIListIterator(IList<TSource> source, Func<TSource, TResult> selector)
            {
                Debug.Assert(source != null);
                Debug.Assert(selector != null);

                _source = source;
                _selector = selector;
            }

            public override AsyncIteratorBase<TResult> Clone()
            {
                return new SelectIListIterator<TSource, TResult>(_source, _selector);
            }

            public override async ValueTask DisposeAsync()
            {
                if (_enumerator != null)
                {
                    _enumerator.Dispose();
                    _enumerator = null;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            public ValueTask<int> GetCountAsync(bool onlyIfCheap, CancellationToken cancellationToken)
            {
                if (onlyIfCheap)
                {
                    return new ValueTask<int>(-1);
                }

                cancellationToken.ThrowIfCancellationRequested();

                var count = 0;

                foreach (var item in _source)
                {
                    _selector(item);

                    checked
                    {
                        count++;
                    }
                }

                return new ValueTask<int>(count);
            }

            public override IAsyncEnumerable<TResult1> Select<TResult1>(Func<TResult, TResult1> selector)
            {
                return new SelectIListIterator<TSource, TResult1>(_source, CombineSelectors(_selector, selector));
            }

            public ValueTask<TResult[]> ToArrayAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var n = _source.Count;

                var res = new TResult[n];

                for (var i = 0; i < n; i++)
                {
                    res[i] = _selector(_source[i]);
                }

                return new ValueTask<TResult[]>(res);
            }

            public ValueTask<List<TResult>> ToListAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var n = _source.Count;

                var res = new List<TResult>(n);

                for (var i = 0; i < n; i++)
                {
                    res.Add(_selector(_source[i]));
                }

                return new ValueTask<List<TResult>>(res);
            }

            protected override async ValueTask<bool> MoveNextCore()
            {
                switch (_state)
                {
                    case AsyncIteratorState.Allocated:
                        _enumerator = _source.GetEnumerator();
                        _state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        if (_enumerator.MoveNext())
                        {
                            _current = _selector(_enumerator.Current);
                            return true;
                        }

                        await DisposeAsync().ConfigureAwait(false);
                        break;
                }

                return false;
            }
        }

        internal sealed class SelectEnumerableAsyncIteratorWithTask<TSource, TResult> : AsyncIterator<TResult>
        {
            private readonly Func<TSource, ValueTask<TResult>> _selector;
            private readonly IAsyncEnumerable<TSource> _source;

            private IAsyncEnumerator<TSource> _enumerator;

            public SelectEnumerableAsyncIteratorWithTask(IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TResult>> selector)
            {
                Debug.Assert(source != null);
                Debug.Assert(selector != null);

                _source = source;
                _selector = selector;
            }

            public override AsyncIteratorBase<TResult> Clone()
            {
                return new SelectEnumerableAsyncIteratorWithTask<TSource, TResult>(_source, _selector);
            }

            public override async ValueTask DisposeAsync()
            {
                if (_enumerator != null)
                {
                    await _enumerator.DisposeAsync().ConfigureAwait(false);
                    _enumerator = null;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            public override IAsyncEnumerable<TResult1> Select<TResult1>(Func<TResult, ValueTask<TResult1>> selector)
            {
                return new SelectEnumerableAsyncIteratorWithTask<TSource, TResult1>(_source, CombineSelectors(_selector, selector));
            }

            protected override async ValueTask<bool> MoveNextCore()
            {
                switch (_state)
                {
                    case AsyncIteratorState.Allocated:
                        _enumerator = _source.GetAsyncEnumerator(_cancellationToken);
                        _state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        if (await _enumerator.MoveNextAsync().ConfigureAwait(false))
                        {
                            _current = await _selector(_enumerator.Current).ConfigureAwait(false);
                            return true;
                        }

                        break;
                }

                await DisposeAsync().ConfigureAwait(false);
                return false;
            }
        }

#if !NO_DEEP_CANCELLATION
        internal sealed class SelectEnumerableAsyncIteratorWithTaskAndCancellation<TSource, TResult> : AsyncIterator<TResult>
        {
            private readonly Func<TSource, CancellationToken, ValueTask<TResult>> _selector;
            private readonly IAsyncEnumerable<TSource> _source;

            private IAsyncEnumerator<TSource> _enumerator;

            public SelectEnumerableAsyncIteratorWithTaskAndCancellation(IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TResult>> selector)
            {
                Debug.Assert(source != null);
                Debug.Assert(selector != null);

                _source = source;
                _selector = selector;
            }

            public override AsyncIteratorBase<TResult> Clone()
            {
                return new SelectEnumerableAsyncIteratorWithTaskAndCancellation<TSource, TResult>(_source, _selector);
            }

            public override async ValueTask DisposeAsync()
            {
                if (_enumerator != null)
                {
                    await _enumerator.DisposeAsync().ConfigureAwait(false);
                    _enumerator = null;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            public override IAsyncEnumerable<TResult1> Select<TResult1>(Func<TResult, CancellationToken, ValueTask<TResult1>> selector)
            {
                return new SelectEnumerableAsyncIteratorWithTaskAndCancellation<TSource, TResult1>(_source, CombineSelectors(_selector, selector));
            }

            protected override async ValueTask<bool> MoveNextCore()
            {
                switch (_state)
                {
                    case AsyncIteratorState.Allocated:
                        _enumerator = _source.GetAsyncEnumerator(_cancellationToken);
                        _state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        if (await _enumerator.MoveNextAsync().ConfigureAwait(false))
                        {
                            _current = await _selector(_enumerator.Current, _cancellationToken).ConfigureAwait(false);
                            return true;
                        }

                        break;
                }

                await DisposeAsync().ConfigureAwait(false);
                return false;
            }
        }
#endif

        private sealed class SelectEnumerableWithIndexAsyncIteratorWithTask<TSource, TResult> : AsyncIterator<TResult>
        {
            private readonly Func<TSource, int, ValueTask<TResult>> _selector;
            private readonly IAsyncEnumerable<TSource> _source;
            private IAsyncEnumerator<TSource> _enumerator;
            private int _index;

            public SelectEnumerableWithIndexAsyncIteratorWithTask(IAsyncEnumerable<TSource> source, Func<TSource, int, ValueTask<TResult>> selector)
            {
                Debug.Assert(source != null);
                Debug.Assert(selector != null);

                _source = source;
                _selector = selector;
            }

            public override AsyncIteratorBase<TResult> Clone()
            {
                return new SelectEnumerableWithIndexAsyncIteratorWithTask<TSource, TResult>(_source, _selector);
            }

            public override async ValueTask DisposeAsync()
            {
                if (_enumerator != null)
                {
                    await _enumerator.DisposeAsync().ConfigureAwait(false);
                    _enumerator = null;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async ValueTask<bool> MoveNextCore()
            {
                switch (_state)
                {
                    case AsyncIteratorState.Allocated:
                        _enumerator = _source.GetAsyncEnumerator(_cancellationToken);
                        _index = -1;
                        _state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        if (await _enumerator.MoveNextAsync().ConfigureAwait(false))
                        {
                            var item = _enumerator.Current;

                            checked
                            {
                                _index++;
                            }

                            _current = await _selector(item, _index).ConfigureAwait(false);
                            return true;
                        }

                        break;
                }

                await DisposeAsync().ConfigureAwait(false);
                return false;
            }
        }

#if !NO_DEEP_CANCELLATION
        private sealed class SelectEnumerableWithIndexAsyncIteratorWithTaskAndCancellation<TSource, TResult> : AsyncIterator<TResult>
        {
            private readonly Func<TSource, int, CancellationToken, ValueTask<TResult>> _selector;
            private readonly IAsyncEnumerable<TSource> _source;
            private IAsyncEnumerator<TSource> _enumerator;
            private int _index;

            public SelectEnumerableWithIndexAsyncIteratorWithTaskAndCancellation(IAsyncEnumerable<TSource> source, Func<TSource, int, CancellationToken, ValueTask<TResult>> selector)
            {
                Debug.Assert(source != null);
                Debug.Assert(selector != null);

                _source = source;
                _selector = selector;
            }

            public override AsyncIteratorBase<TResult> Clone()
            {
                return new SelectEnumerableWithIndexAsyncIteratorWithTaskAndCancellation<TSource, TResult>(_source, _selector);
            }

            public override async ValueTask DisposeAsync()
            {
                if (_enumerator != null)
                {
                    await _enumerator.DisposeAsync().ConfigureAwait(false);
                    _enumerator = null;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async ValueTask<bool> MoveNextCore()
            {
                switch (_state)
                {
                    case AsyncIteratorState.Allocated:
                        _enumerator = _source.GetAsyncEnumerator(_cancellationToken);
                        _index = -1;
                        _state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        if (await _enumerator.MoveNextAsync().ConfigureAwait(false))
                        {
                            var item = _enumerator.Current;

                            checked
                            {
                                _index++;
                            }

                            _current = await _selector(item, _index, _cancellationToken).ConfigureAwait(false);
                            return true;
                        }

                        break;
                }

                await DisposeAsync().ConfigureAwait(false);
                return false;
            }
        }
#endif

        // NB: LINQ to Objects implements IPartition<TResult> for this. However, it seems incorrect to do so in a trivial
        //     manner where e.g. TryGetLast simply indexes into the list without running the selector for the first n - 1
        //     elements in order to ensure side-effects. We should consider whether we want to follow this implementation
        //     strategy or support IAsyncPartition<TResult> in a less efficient but more correct manner here.

        private sealed class SelectIListIteratorWithTask<TSource, TResult> : AsyncIterator<TResult>, IAsyncIListProvider<TResult>
        {
            private readonly Func<TSource, ValueTask<TResult>> _selector;
            private readonly IList<TSource> _source;
            private IEnumerator<TSource> _enumerator;

            public SelectIListIteratorWithTask(IList<TSource> source, Func<TSource, ValueTask<TResult>> selector)
            {
                Debug.Assert(source != null);
                Debug.Assert(selector != null);

                _source = source;
                _selector = selector;
            }

            public override AsyncIteratorBase<TResult> Clone()
            {
                return new SelectIListIteratorWithTask<TSource, TResult>(_source, _selector);
            }

            public override async ValueTask DisposeAsync()
            {
                if (_enumerator != null)
                {
                    _enumerator.Dispose();
                    _enumerator = null;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            public ValueTask<int> GetCountAsync(bool onlyIfCheap, CancellationToken cancellationToken)
            {
                if (onlyIfCheap)
                {
                    return new ValueTask<int>(-1);
                }

                return Core();

                async ValueTask<int> Core()
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var count = 0;

                    foreach (var item in _source)
                    {
                        await _selector(item).ConfigureAwait(false);

                        checked
                        {
                            count++;
                        }
                    }

                    return count;
                }
            }

            public override IAsyncEnumerable<TResult1> Select<TResult1>(Func<TResult, ValueTask<TResult1>> selector)
            {
                return new SelectIListIteratorWithTask<TSource, TResult1>(_source, CombineSelectors(_selector, selector));
            }

            public async ValueTask<TResult[]> ToArrayAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var n = _source.Count;

                var res = new TResult[n];

                for (var i = 0; i < n; i++)
                {
                    res[i] = await _selector(_source[i]).ConfigureAwait(false);
                }

                return res;
            }

            public async ValueTask<List<TResult>> ToListAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var n = _source.Count;

                var res = new List<TResult>(n);

                for (var i = 0; i < n; i++)
                {
                    res.Add(await _selector(_source[i]).ConfigureAwait(false));
                }

                return res;
            }

            protected override async ValueTask<bool> MoveNextCore()
            {
                switch (_state)
                {
                    case AsyncIteratorState.Allocated:
                        _enumerator = _source.GetEnumerator();
                        _state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        if (_enumerator.MoveNext())
                        {
                            _current = await _selector(_enumerator.Current).ConfigureAwait(false);
                            return true;
                        }

                        break;
                }

                await DisposeAsync().ConfigureAwait(false);
                return false;
            }
        }

#if !NO_DEEP_CANCELLATION
        private sealed class SelectIListIteratorWithTaskAndCancellation<TSource, TResult> : AsyncIterator<TResult>, IAsyncIListProvider<TResult>
        {
            private readonly Func<TSource, CancellationToken, ValueTask<TResult>> _selector;
            private readonly IList<TSource> _source;
            private IEnumerator<TSource> _enumerator;

            public SelectIListIteratorWithTaskAndCancellation(IList<TSource> source, Func<TSource, CancellationToken, ValueTask<TResult>> selector)
            {
                Debug.Assert(source != null);
                Debug.Assert(selector != null);

                _source = source;
                _selector = selector;
            }

            public override AsyncIteratorBase<TResult> Clone()
            {
                return new SelectIListIteratorWithTaskAndCancellation<TSource, TResult>(_source, _selector);
            }

            public override async ValueTask DisposeAsync()
            {
                if (_enumerator != null)
                {
                    _enumerator.Dispose();
                    _enumerator = null;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            public ValueTask<int> GetCountAsync(bool onlyIfCheap, CancellationToken cancellationToken)
            {
                if (onlyIfCheap)
                {
                    return new ValueTask<int>(-1);
                }

                return Core();

                async ValueTask<int> Core()
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var count = 0;

                    foreach (var item in _source)
                    {
                        await _selector(item, cancellationToken).ConfigureAwait(false);

                        checked
                        {
                            count++;
                        }
                    }

                    return count;
                }
            }

            public override IAsyncEnumerable<TResult1> Select<TResult1>(Func<TResult, CancellationToken, ValueTask<TResult1>> selector)
            {
                return new SelectIListIteratorWithTaskAndCancellation<TSource, TResult1>(_source, CombineSelectors(_selector, selector));
            }

            public async ValueTask<TResult[]> ToArrayAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var n = _source.Count;

                var res = new TResult[n];

                for (var i = 0; i < n; i++)
                {
                    res[i] = await _selector(_source[i], cancellationToken).ConfigureAwait(false);
                }

                return res;
            }

            public async ValueTask<List<TResult>> ToListAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var n = _source.Count;

                var res = new List<TResult>(n);

                for (var i = 0; i < n; i++)
                {
                    res.Add(await _selector(_source[i], cancellationToken).ConfigureAwait(false));
                }

                return res;
            }

            protected override async ValueTask<bool> MoveNextCore()
            {
                switch (_state)
                {
                    case AsyncIteratorState.Allocated:
                        _enumerator = _source.GetEnumerator();
                        _state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        if (_enumerator.MoveNext())
                        {
                            _current = await _selector(_enumerator.Current, _cancellationToken).ConfigureAwait(false);
                            return true;
                        }

                        break;
                }

                await DisposeAsync().ConfigureAwait(false);
                return false;
            }
        }
#endif
    }
}

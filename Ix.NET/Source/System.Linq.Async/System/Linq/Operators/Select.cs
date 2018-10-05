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
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            if (source is AsyncIterator<TSource> iterator)
            {
                return iterator.Select(selector);
            }

            if (source is IList<TSource> ilist)
            {
                return new SelectIListIterator<TSource, TResult>(ilist, selector);
            }

            return new SelectEnumerableAsyncIterator<TSource, TResult>(source, selector);
        }

        public static IAsyncEnumerable<TResult> Select<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, int, TResult> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return new SelectEnumerableWithIndexAsyncIterator<TSource, TResult>(source, selector);
        }

        public static IAsyncEnumerable<TResult> Select<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<TResult>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            if (source is AsyncIterator<TSource> iterator)
            {
                return iterator.Select(selector);
            }

            if (source is IList<TSource> ilist)
            {
                return new SelectIListIteratorWithTask<TSource, TResult>(ilist, selector);
            }

            return new SelectEnumerableAsyncIteratorWithTask<TSource, TResult>(source, selector);
        }

        public static IAsyncEnumerable<TResult> Select<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, int, Task<TResult>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return new SelectEnumerableWithIndexAsyncIteratorWithTask<TSource, TResult>(source, selector);
        }

        private static Func<TSource, TResult> CombineSelectors<TSource, TMiddle, TResult>(Func<TSource, TMiddle> selector1, Func<TMiddle, TResult> selector2)
        {
            return x => selector2(selector1(x));
        }

        private static Func<TSource, Task<TResult>> CombineSelectors<TSource, TMiddle, TResult>(Func<TSource, Task<TMiddle>> selector1, Func<TMiddle, Task<TResult>> selector2)
        {
            return async x => await selector2(await selector1(x).ConfigureAwait(false)).ConfigureAwait(false);
        }

        internal sealed class SelectEnumerableAsyncIterator<TSource, TResult> : AsyncIterator<TResult>
        {
            private readonly Func<TSource, TResult> selector;
            private readonly IAsyncEnumerable<TSource> source;

            private IAsyncEnumerator<TSource> enumerator;

            public SelectEnumerableAsyncIterator(IAsyncEnumerable<TSource> source, Func<TSource, TResult> selector)
            {
                Debug.Assert(source != null);
                Debug.Assert(selector != null);

                this.source = source;
                this.selector = selector;
            }

            public override AsyncIterator<TResult> Clone()
            {
                return new SelectEnumerableAsyncIterator<TSource, TResult>(source, selector);
            }

            public override async ValueTask DisposeAsync()
            {
                if (enumerator != null)
                {
                    await enumerator.DisposeAsync().ConfigureAwait(false);
                    enumerator = null;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            public override IAsyncEnumerable<TResult1> Select<TResult1>(Func<TResult, TResult1> selector)
            {
                return new SelectEnumerableAsyncIterator<TSource, TResult1>(source, CombineSelectors(this.selector, selector));
            }

            protected override async ValueTask<bool> MoveNextCore()
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        enumerator = source.GetAsyncEnumerator();
                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        if (await enumerator.MoveNextAsync().ConfigureAwait(false))
                        {
                            current = selector(enumerator.Current);
                            return true;
                        }

                        break;
                }

                await DisposeAsync().ConfigureAwait(false);
                return false;
            }
        }

        internal sealed class SelectEnumerableWithIndexAsyncIterator<TSource, TResult> : AsyncIterator<TResult>
        {
            private readonly Func<TSource, int, TResult> selector;
            private readonly IAsyncEnumerable<TSource> source;
            private IAsyncEnumerator<TSource> enumerator;
            private int index;

            public SelectEnumerableWithIndexAsyncIterator(IAsyncEnumerable<TSource> source, Func<TSource, int, TResult> selector)
            {
                Debug.Assert(source != null);
                Debug.Assert(selector != null);

                this.source = source;
                this.selector = selector;
            }

            public override AsyncIterator<TResult> Clone()
            {
                return new SelectEnumerableWithIndexAsyncIterator<TSource, TResult>(source, selector);
            }

            public override async ValueTask DisposeAsync()
            {
                if (enumerator != null)
                {
                    await enumerator.DisposeAsync().ConfigureAwait(false);
                    enumerator = null;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async ValueTask<bool> MoveNextCore()
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        enumerator = source.GetAsyncEnumerator();
                        index = -1;
                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        if (await enumerator.MoveNextAsync().ConfigureAwait(false))
                        {
                            checked
                            {
                                index++;
                            }
                            current = selector(enumerator.Current, index);
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
            private readonly Func<TSource, TResult> selector;
            private readonly IList<TSource> source;
            private IEnumerator<TSource> enumerator;

            public SelectIListIterator(IList<TSource> source, Func<TSource, TResult> selector)
            {
                Debug.Assert(source != null);
                Debug.Assert(selector != null);

                this.source = source;
                this.selector = selector;
            }

            public override AsyncIterator<TResult> Clone()
            {
                return new SelectIListIterator<TSource, TResult>(source, selector);
            }

            public override async ValueTask DisposeAsync()
            {
                if (enumerator != null)
                {
                    enumerator.Dispose();
                    enumerator = null;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            public Task<int> GetCountAsync(bool onlyIfCheap, CancellationToken cancellationToken)
            {
                if (onlyIfCheap)
                {
                    return Task.FromResult(-1);
                }

                var count = 0;

                foreach (var item in source)
                {
                    selector(item);

                    checked
                    {
                        count++;
                    }
                }

                return Task.FromResult(count);
            }

            public override IAsyncEnumerable<TResult1> Select<TResult1>(Func<TResult, TResult1> selector)
            {
                return new SelectIListIterator<TSource, TResult1>(source, CombineSelectors(this.selector, selector));
            }

            public Task<TResult[]> ToArrayAsync(CancellationToken cancellationToken)
            {
                var n = source.Count;

                var res = new TResult[n];

                for (var i = 0; i < n; i++)
                {
                    res[i] = selector(source[i]);
                }

                return Task.FromResult(res);
            }

            public Task<List<TResult>> ToListAsync(CancellationToken cancellationToken)
            {
                var n = source.Count;

                var res = new List<TResult>(n);

                for (var i = 0; i < n; i++)
                {
                    res.Add(selector(source[i]));
                }

                return Task.FromResult(res);
            }

            protected override async ValueTask<bool> MoveNextCore()
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        enumerator = source.GetEnumerator();
                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        if (enumerator.MoveNext())
                        {
                            current = selector(enumerator.Current);
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
            private readonly Func<TSource, Task<TResult>> selector;
            private readonly IAsyncEnumerable<TSource> source;

            private IAsyncEnumerator<TSource> enumerator;

            public SelectEnumerableAsyncIteratorWithTask(IAsyncEnumerable<TSource> source, Func<TSource, Task<TResult>> selector)
            {
                Debug.Assert(source != null);
                Debug.Assert(selector != null);

                this.source = source;
                this.selector = selector;
            }

            public override AsyncIterator<TResult> Clone()
            {
                return new SelectEnumerableAsyncIteratorWithTask<TSource, TResult>(source, selector);
            }

            public override async ValueTask DisposeAsync()
            {
                if (enumerator != null)
                {
                    await enumerator.DisposeAsync().ConfigureAwait(false);
                    enumerator = null;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            public override IAsyncEnumerable<TResult1> Select<TResult1>(Func<TResult, Task<TResult1>> selector)
            {
                return new SelectEnumerableAsyncIteratorWithTask<TSource, TResult1>(source, CombineSelectors(this.selector, selector));
            }

            protected override async ValueTask<bool> MoveNextCore()
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        enumerator = source.GetAsyncEnumerator();
                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        if (await enumerator.MoveNextAsync().ConfigureAwait(false))
                        {
                            current = await selector(enumerator.Current).ConfigureAwait(false);
                            return true;
                        }

                        break;
                }

                await DisposeAsync().ConfigureAwait(false);
                return false;
            }
        }

        internal sealed class SelectEnumerableWithIndexAsyncIteratorWithTask<TSource, TResult> : AsyncIterator<TResult>
        {
            private readonly Func<TSource, int, Task<TResult>> selector;
            private readonly IAsyncEnumerable<TSource> source;
            private IAsyncEnumerator<TSource> enumerator;
            private int index;

            public SelectEnumerableWithIndexAsyncIteratorWithTask(IAsyncEnumerable<TSource> source, Func<TSource, int, Task<TResult>> selector)
            {
                Debug.Assert(source != null);
                Debug.Assert(selector != null);

                this.source = source;
                this.selector = selector;
            }

            public override AsyncIterator<TResult> Clone()
            {
                return new SelectEnumerableWithIndexAsyncIteratorWithTask<TSource, TResult>(source, selector);
            }

            public override async ValueTask DisposeAsync()
            {
                if (enumerator != null)
                {
                    await enumerator.DisposeAsync().ConfigureAwait(false);
                    enumerator = null;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async ValueTask<bool> MoveNextCore()
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        enumerator = source.GetAsyncEnumerator();
                        index = -1;
                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        if (await enumerator.MoveNextAsync().ConfigureAwait(false))
                        {
                            checked
                            {
                                index++;
                            }
                            current = await selector(enumerator.Current, index).ConfigureAwait(false);
                            return true;
                        }

                        break;
                }

                await DisposeAsync().ConfigureAwait(false);
                return false;
            }
        }

        // NB: LINQ to Objects implements IPartition<TResult> for this. However, it seems incorrect to do so in a trivial
        //     manner where e.g. TryGetLast simply indexes into the list without running the selector for the first n - 1
        //     elements in order to ensure side-effects. We should consider whether we want to follow this implementation
        //     strategy or support IAsyncPartition<TResult> in a less efficient but more correct manner here.

        internal sealed class SelectIListIteratorWithTask<TSource, TResult> : AsyncIterator<TResult>, IAsyncIListProvider<TResult>
        {
            private readonly Func<TSource, Task<TResult>> selector;
            private readonly IList<TSource> source;
            private IEnumerator<TSource> enumerator;

            public SelectIListIteratorWithTask(IList<TSource> source, Func<TSource, Task<TResult>> selector)
            {
                Debug.Assert(source != null);
                Debug.Assert(selector != null);

                this.source = source;
                this.selector = selector;
            }

            public override AsyncIterator<TResult> Clone()
            {
                return new SelectIListIteratorWithTask<TSource, TResult>(source, selector);
            }

            public override async ValueTask DisposeAsync()
            {
                if (enumerator != null)
                {
                    enumerator.Dispose();
                    enumerator = null;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            public async Task<int> GetCountAsync(bool onlyIfCheap, CancellationToken cancellationToken)
            {
                if (onlyIfCheap)
                {
                    return -1;
                }

                var count = 0;

                foreach (var item in source)
                {
                    await selector(item).ConfigureAwait(false);

                    checked
                    {
                        count++;
                    }
                }

                return count;
            }

            public override IAsyncEnumerable<TResult1> Select<TResult1>(Func<TResult, Task<TResult1>> selector)
            {
                return new SelectIListIteratorWithTask<TSource, TResult1>(source, CombineSelectors(this.selector, selector));
            }

            public async Task<TResult[]> ToArrayAsync(CancellationToken cancellationToken)
            {
                var n = source.Count;

                var res = new TResult[n];

                for (var i = 0; i < n; i++)
                {
                    res[i] = await selector(source[i]).ConfigureAwait(false);
                }

                return res;
            }

            public async Task<List<TResult>> ToListAsync(CancellationToken cancellationToken)
            {
                var n = source.Count;

                var res = new List<TResult>(n);

                for (var i = 0; i < n; i++)
                {
                    res.Add(await selector(source[i]).ConfigureAwait(false));
                }

                return res;
            }

            protected override async ValueTask<bool> MoveNextCore()
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        enumerator = source.GetEnumerator();
                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        if (enumerator.MoveNext())
                        {
                            current = await selector(enumerator.Current).ConfigureAwait(false);
                            return true;
                        }

                        break;
                }

                await DisposeAsync().ConfigureAwait(false);
                return false;
            }
        }
    }
}

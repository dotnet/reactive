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
        public static IAsyncEnumerable<TSource> Append<TSource>(this IAsyncEnumerable<TSource> source, TSource element)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (source is AppendPrependAsyncIterator<TSource> appendable)
            {
                return appendable.Append(element);
            }

            return new AppendPrepend1AsyncIterator<TSource>(source, element, appending: true);
        }

        public static IAsyncEnumerable<TSource> Prepend<TSource>(this IAsyncEnumerable<TSource> source, TSource element)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (source is AppendPrependAsyncIterator<TSource> appendable)
            {
                return appendable.Prepend(element);
            }

            return new AppendPrepend1AsyncIterator<TSource>(source, element, appending: false);
        }

        private abstract class AppendPrependAsyncIterator<TSource> : AsyncIterator<TSource>, IAsyncIListProvider<TSource>
        {
            protected readonly IAsyncEnumerable<TSource> source;
            protected IAsyncEnumerator<TSource> enumerator;

            protected AppendPrependAsyncIterator(IAsyncEnumerable<TSource> source)
            {
                Debug.Assert(source != null);

                this.source = source;
            }

            protected void GetSourceEnumerator()
            {
                Debug.Assert(enumerator == null);
                enumerator = source.GetAsyncEnumerator();
            }

            public abstract AppendPrependAsyncIterator<TSource> Append(TSource item);
            public abstract AppendPrependAsyncIterator<TSource> Prepend(TSource item);

            protected async Task<bool> LoadFromEnumeratorAsync()
            {
                if (await enumerator.MoveNextAsync().ConfigureAwait(false))
                {
                    current = enumerator.Current;
                    return true;
                }

                if (enumerator != null)
                {
                    await enumerator.DisposeAsync().ConfigureAwait(false);
                    enumerator = null;
                }

                return false;
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

            public abstract Task<TSource[]> ToArrayAsync(CancellationToken cancellationToken);
            public abstract Task<List<TSource>> ToListAsync(CancellationToken cancellationToken);
            public abstract Task<int> GetCountAsync(bool onlyIfCheap, CancellationToken cancellationToken);
        }

        private sealed class AppendPrepend1AsyncIterator<TSource> : AppendPrependAsyncIterator<TSource>
        {
            private readonly TSource item;
            private readonly bool appending;

            bool hasEnumerator;

            public AppendPrepend1AsyncIterator(IAsyncEnumerable<TSource> source, TSource item, bool appending)
                : base(source)
            {
                this.item = item;
                this.appending = appending;
            }

            public override AsyncIterator<TSource> Clone()
            {
                return new AppendPrepend1AsyncIterator<TSource>(source, item, appending);
            }

            protected override async ValueTask<bool> MoveNextCore()
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        hasEnumerator = false;
                        state = AsyncIteratorState.Iterating;
                        if (!appending)
                        {
                            current = item;
                            return true;
                        }

                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        if (!hasEnumerator)
                        {
                            GetSourceEnumerator();
                            hasEnumerator = true;
                        }

                        if (enumerator != null)
                        {
                            if (await LoadFromEnumeratorAsync()
                                .ConfigureAwait(false))
                            {
                                return true;
                            }

                            if (appending)
                            {
                                current = item;
                                return true;
                            }
                        }

                        break;
                }

                await DisposeAsync().ConfigureAwait(false);
                return false;
            }

            public override AppendPrependAsyncIterator<TSource> Append(TSource element)
            {
                if (appending)
                {
                    return new AppendPrependNAsyncIterator<TSource>(source, null, new SingleLinkedNode<TSource>(item).Add(element), prependCount: 0, appendCount: 2);
                }
                else
                {
                    return new AppendPrependNAsyncIterator<TSource>(source, new SingleLinkedNode<TSource>(item), new SingleLinkedNode<TSource>(element), prependCount: 1, appendCount: 1);
                }
            }

            public override AppendPrependAsyncIterator<TSource> Prepend(TSource element)
            {
                if (appending)
                {
                    return new AppendPrependNAsyncIterator<TSource>(source, new SingleLinkedNode<TSource>(element), new SingleLinkedNode<TSource>(item), prependCount: 1, appendCount: 1);
                }
                else
                {
                    return new AppendPrependNAsyncIterator<TSource>(source, new SingleLinkedNode<TSource>(item).Add(element), null, prependCount: 2, appendCount: 0);
                }
            }

            public override async Task<TSource[]> ToArrayAsync(CancellationToken cancellationToken)
            {
                var count = await GetCountAsync(onlyIfCheap: true, cancellationToken: cancellationToken).ConfigureAwait(false);
                if (count == -1)
                {
                    return await AsyncEnumerableHelpers.ToArray(this, cancellationToken).ConfigureAwait(false);
                }

                var array = new TSource[count];
                int index;
                if (appending)
                {
                    index = 0;
                }
                else
                {
                    array[0] = item;
                    index = 1;
                }

                if (source is ICollection<TSource> sourceCollection)
                {
                    sourceCollection.CopyTo(array, index);
                }
                else
                {
                    var en = source.GetAsyncEnumerator();

                    try
                    {
                        while (await en.MoveNextAsync(cancellationToken).ConfigureAwait(false))
                        {
                            array[index] = en.Current;
                            ++index;
                        }
                    }
                    finally
                    {
                        await en.DisposeAsync().ConfigureAwait(false);
                    }
                }

                if (appending)
                {
                    array[array.Length - 1] = item;
                }

                return array;
            }

            public override async Task<List<TSource>> ToListAsync(CancellationToken cancellationToken)
            {
                var count = await GetCountAsync(onlyIfCheap: true, cancellationToken: cancellationToken).ConfigureAwait(false);
                var list = count == -1 ? new List<TSource>() : new List<TSource>(count);

                if (!appending)
                {
                    list.Add(item);
                }


                var en = source.GetAsyncEnumerator();

                try
                {
                    while (await en.MoveNextAsync(cancellationToken).ConfigureAwait(false))
                    {
                        list.Add(en.Current);
                    }
                }
                finally
                {
                    await en.DisposeAsync().ConfigureAwait(false);
                }

                if (appending)
                {
                    list.Add(item);
                }

                return list;
            }

            public override async Task<int> GetCountAsync(bool onlyIfCheap, CancellationToken cancellationToken)
            {
                if (source is IAsyncIListProvider<TSource> listProv)
                {
                    var count = await listProv.GetCountAsync(onlyIfCheap, cancellationToken).ConfigureAwait(false);
                    return count == -1 ? -1 : count + 1;
                }

                return !onlyIfCheap || source is ICollection<TSource> ? await source.Count(cancellationToken).ConfigureAwait(false) + 1 : -1;
            }
        }

        private sealed class AppendPrependNAsyncIterator<TSource> : AppendPrependAsyncIterator<TSource>
        {
            private readonly SingleLinkedNode<TSource> prepended;
            private readonly SingleLinkedNode<TSource> appended;
            private readonly int prependCount;
            private readonly int appendCount;
            private SingleLinkedNode<TSource> node;

            public AppendPrependNAsyncIterator(IAsyncEnumerable<TSource> source, SingleLinkedNode<TSource> prepended, SingleLinkedNode<TSource> appended, int prependCount, int appendCount)
                : base(source)
            {
                Debug.Assert(prepended != null || appended != null);
                Debug.Assert(prependCount > 0 || appendCount > 0);
                Debug.Assert(prependCount + appendCount >= 2);
                Debug.Assert((prepended?.GetCount() ?? 0) == prependCount);
                Debug.Assert((appended?.GetCount() ?? 0) == appendCount);

                this.prepended = prepended;
                this.appended = appended;
                this.prependCount = prependCount;
                this.appendCount = appendCount;
            }

            public override AsyncIterator<TSource> Clone()
            {
                return new AppendPrependNAsyncIterator<TSource>(source, prepended, appended, prependCount, appendCount);
            }

            int mode;
            IEnumerator<TSource> appendedEnumerator;

            public override async ValueTask DisposeAsync()
            {
                if (appendedEnumerator != null)
                {
                    appendedEnumerator.Dispose();
                    appendedEnumerator = null;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async ValueTask<bool> MoveNextCore()
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        mode = 1;
                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        switch (mode)
                        {
                            case 1:
                                node = prepended;
                                mode = 2;
                                goto case 2;

                            case 2:
                                if (node != null)
                                {
                                    current = node.Item;
                                    node = node.Linked;
                                    return true;
                                }

                                GetSourceEnumerator();
                                mode = 3;
                                goto case 3;

                            case 3:
                                if (await LoadFromEnumeratorAsync().ConfigureAwait(false))
                                {
                                    return true;
                                }

                                if (appended != null)
                                {
                                    appendedEnumerator = appended.GetEnumerator(appendCount);
                                    mode = 4;
                                    goto case 4;
                                }

                                break;


                            case 4:
                                if (appendedEnumerator.MoveNext())
                                {
                                    current = appendedEnumerator.Current;
                                    return true;
                                }
                                break;
                        }

                        break;
                }

                await DisposeAsync().ConfigureAwait(false);
                return false;
            }

            public override AppendPrependAsyncIterator<TSource> Append(TSource item)
            {
                var res = appended != null ? appended.Add(item) : new SingleLinkedNode<TSource>(item);
                return new AppendPrependNAsyncIterator<TSource>(source, prepended, res, prependCount, appendCount + 1);
            }

            public override AppendPrependAsyncIterator<TSource> Prepend(TSource item)
            {
                var res = prepended != null ? prepended.Add(item) : new SingleLinkedNode<TSource>(item);
                return new AppendPrependNAsyncIterator<TSource>(source, res, appended, prependCount + 1, appendCount);
            }

            public override async Task<TSource[]> ToArrayAsync(CancellationToken cancellationToken)
            {
                var count = await GetCountAsync(onlyIfCheap: true, cancellationToken: cancellationToken).ConfigureAwait(false);
                if (count == -1)
                {
                    return await AsyncEnumerableHelpers.ToArray(this, cancellationToken).ConfigureAwait(false);
                }

                var array = new TSource[count];
                var index = 0;
                for (var n = prepended; n != null; n = n.Linked)
                {
                    array[index] = n.Item;
                    ++index;
                }

                if (source is ICollection<TSource> sourceCollection)
                {
                    sourceCollection.CopyTo(array, index);
                }
                else
                {
                    var en = source.GetAsyncEnumerator();

                    try
                    {
                        while (await en.MoveNextAsync(cancellationToken).ConfigureAwait(false))
                        {
                            array[index] = en.Current;
                            ++index;
                        }
                    }
                    finally
                    {
                        await en.DisposeAsync().ConfigureAwait(false);
                    }
                }

                index = array.Length;
                for (var n = appended; n != null; n = n.Linked)
                {
                    --index;
                    array[index] = n.Item;
                }

                return array;
            }

            public override async Task<List<TSource>> ToListAsync(CancellationToken cancellationToken)
            {
                var count = await GetCountAsync(onlyIfCheap: true, cancellationToken: cancellationToken).ConfigureAwait(false);
                var list = count == -1 ? new List<TSource>() : new List<TSource>(count);
                for (var n = prepended; n != null; n = n.Linked)
                {
                    list.Add(n.Item);
                }

                var en = source.GetAsyncEnumerator();

                try
                {
                    while (await en.MoveNextAsync(cancellationToken).ConfigureAwait(false))
                    {
                        list.Add(en.Current);
                    }
                }
                finally
                {
                    await en.DisposeAsync().ConfigureAwait(false);
                }

                if (appended != null)
                {
                    using (var en2 = appended.GetEnumerator(appendCount))
                    {
                        while (en2.MoveNext())
                        {
                            list.Add(en2.Current);
                        }
                    }
                }

                return list;
            }

            public override async Task<int> GetCountAsync(bool onlyIfCheap, CancellationToken cancellationToken)
            {
                if (source is IAsyncIListProvider<TSource> listProv)
                {
                    var count = await listProv.GetCountAsync(onlyIfCheap, cancellationToken).ConfigureAwait(false);
                    return count == -1 ? -1 : count + appendCount + prependCount;
                }

                return !onlyIfCheap || source is ICollection<TSource> ? await source.Count(cancellationToken).ConfigureAwait(false) + appendCount + prependCount : -1;
            }
        }
    }
}

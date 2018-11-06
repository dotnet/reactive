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

            protected void GetSourceEnumerator(CancellationToken cancellationToken)
            {
                Debug.Assert(enumerator == null);
                enumerator = source.GetAsyncEnumerator(cancellationToken);
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
            private readonly TSource _item;
            private readonly bool _appending;

            private bool _hasEnumerator;

            public AppendPrepend1AsyncIterator(IAsyncEnumerable<TSource> source, TSource item, bool appending)
                : base(source)
            {
                _item = item;
                _appending = appending;
            }

            public override AsyncIterator<TSource> Clone()
            {
                return new AppendPrepend1AsyncIterator<TSource>(source, _item, _appending);
            }

            protected override async ValueTask<bool> MoveNextCore(CancellationToken cancellationToken)
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        _hasEnumerator = false;
                        state = AsyncIteratorState.Iterating;
                        if (!_appending)
                        {
                            current = _item;
                            return true;
                        }

                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        if (!_hasEnumerator)
                        {
                            GetSourceEnumerator(cancellationToken);
                            _hasEnumerator = true;
                        }

                        if (enumerator != null)
                        {
                            if (await LoadFromEnumeratorAsync()
                                .ConfigureAwait(false))
                            {
                                return true;
                            }

                            if (_appending)
                            {
                                current = _item;
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
                if (_appending)
                {
                    return new AppendPrependNAsyncIterator<TSource>(source, null, new SingleLinkedNode<TSource>(_item).Add(element), prependCount: 0, appendCount: 2);
                }
                else
                {
                    return new AppendPrependNAsyncIterator<TSource>(source, new SingleLinkedNode<TSource>(_item), new SingleLinkedNode<TSource>(element), prependCount: 1, appendCount: 1);
                }
            }

            public override AppendPrependAsyncIterator<TSource> Prepend(TSource element)
            {
                if (_appending)
                {
                    return new AppendPrependNAsyncIterator<TSource>(source, new SingleLinkedNode<TSource>(element), new SingleLinkedNode<TSource>(_item), prependCount: 1, appendCount: 1);
                }
                else
                {
                    return new AppendPrependNAsyncIterator<TSource>(source, new SingleLinkedNode<TSource>(_item).Add(element), null, prependCount: 2, appendCount: 0);
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
                if (_appending)
                {
                    index = 0;
                }
                else
                {
                    array[0] = _item;
                    index = 1;
                }

                if (source is ICollection<TSource> sourceCollection)
                {
                    sourceCollection.CopyTo(array, index);
                }
                else
                {
                    var en = source.GetAsyncEnumerator(cancellationToken);

                    try
                    {
                        while (await en.MoveNextAsync().ConfigureAwait(false))
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

                if (_appending)
                {
                    array[array.Length - 1] = _item;
                }

                return array;
            }

            public override async Task<List<TSource>> ToListAsync(CancellationToken cancellationToken)
            {
                var count = await GetCountAsync(onlyIfCheap: true, cancellationToken: cancellationToken).ConfigureAwait(false);
                var list = count == -1 ? new List<TSource>() : new List<TSource>(count);

                if (!_appending)
                {
                    list.Add(_item);
                }


                var en = source.GetAsyncEnumerator(cancellationToken);

                try
                {
                    while (await en.MoveNextAsync().ConfigureAwait(false))
                    {
                        list.Add(en.Current);
                    }
                }
                finally
                {
                    await en.DisposeAsync().ConfigureAwait(false);
                }

                if (_appending)
                {
                    list.Add(_item);
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
            private readonly SingleLinkedNode<TSource> _prepended;
            private readonly SingleLinkedNode<TSource> _appended;
            private readonly int _prependCount;
            private readonly int _appendCount;
            private SingleLinkedNode<TSource> _node;
            private int _mode;
            private IEnumerator<TSource> _appendedEnumerator;

            public AppendPrependNAsyncIterator(IAsyncEnumerable<TSource> source, SingleLinkedNode<TSource> prepended, SingleLinkedNode<TSource> appended, int prependCount, int appendCount)
                : base(source)
            {
                Debug.Assert(prepended != null || appended != null);
                Debug.Assert(prependCount > 0 || appendCount > 0);
                Debug.Assert(prependCount + appendCount >= 2);
                Debug.Assert((prepended?.GetCount() ?? 0) == prependCount);
                Debug.Assert((appended?.GetCount() ?? 0) == appendCount);

                _prepended = prepended;
                _appended = appended;
                _prependCount = prependCount;
                _appendCount = appendCount;
            }

            public override AsyncIterator<TSource> Clone()
            {
                return new AppendPrependNAsyncIterator<TSource>(source, _prepended, _appended, _prependCount, _appendCount);
            }

            public override async ValueTask DisposeAsync()
            {
                if (_appendedEnumerator != null)
                {
                    _appendedEnumerator.Dispose();
                    _appendedEnumerator = null;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async ValueTask<bool> MoveNextCore(CancellationToken cancellationToken)
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        _mode = 1;
                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        switch (_mode)
                        {
                            case 1:
                                _node = _prepended;
                                _mode = 2;
                                goto case 2;

                            case 2:
                                if (_node != null)
                                {
                                    current = _node.Item;
                                    _node = _node.Linked;
                                    return true;
                                }

                                GetSourceEnumerator(cancellationToken);
                                _mode = 3;
                                goto case 3;

                            case 3:
                                if (await LoadFromEnumeratorAsync().ConfigureAwait(false))
                                {
                                    return true;
                                }

                                if (_appended != null)
                                {
                                    _appendedEnumerator = _appended.GetEnumerator(_appendCount);
                                    _mode = 4;
                                    goto case 4;
                                }

                                break;


                            case 4:
                                if (_appendedEnumerator.MoveNext())
                                {
                                    current = _appendedEnumerator.Current;
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
                var res = _appended != null ? _appended.Add(item) : new SingleLinkedNode<TSource>(item);
                return new AppendPrependNAsyncIterator<TSource>(source, _prepended, res, _prependCount, _appendCount + 1);
            }

            public override AppendPrependAsyncIterator<TSource> Prepend(TSource item)
            {
                var res = _prepended != null ? _prepended.Add(item) : new SingleLinkedNode<TSource>(item);
                return new AppendPrependNAsyncIterator<TSource>(source, res, _appended, _prependCount + 1, _appendCount);
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
                for (var n = _prepended; n != null; n = n.Linked)
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
                    var en = source.GetAsyncEnumerator(cancellationToken);

                    try
                    {
                        while (await en.MoveNextAsync().ConfigureAwait(false))
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
                for (var n = _appended; n != null; n = n.Linked)
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
                for (var n = _prepended; n != null; n = n.Linked)
                {
                    list.Add(n.Item);
                }

                var en = source.GetAsyncEnumerator(cancellationToken);

                try
                {
                    while (await en.MoveNextAsync().ConfigureAwait(false))
                    {
                        list.Add(en.Current);
                    }
                }
                finally
                {
                    await en.DisposeAsync().ConfigureAwait(false);
                }

                if (_appended != null)
                {
                    using (var en2 = _appended.GetEnumerator(_appendCount))
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
                    return count == -1 ? -1 : count + _appendCount + _prependCount;
                }

                return !onlyIfCheap || source is ICollection<TSource> ? await source.Count(cancellationToken).ConfigureAwait(false) + _appendCount + _prependCount : -1;
            }
        }
    }
}

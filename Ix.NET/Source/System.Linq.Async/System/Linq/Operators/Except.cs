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
        public static IAsyncEnumerable<TSource> Except<TSource>(this IAsyncEnumerable<TSource> first, IAsyncEnumerable<TSource> second)
        {
            if (first == null)
                throw Error.ArgumentNull(nameof(first));
            if (second == null)
                throw Error.ArgumentNull(nameof(second));

            return first.Except(second, EqualityComparer<TSource>.Default);
        }

        public static IAsyncEnumerable<TSource> Except<TSource>(this IAsyncEnumerable<TSource> first, IAsyncEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
        {
            if (first == null)
                throw Error.ArgumentNull(nameof(first));
            if (second == null)
                throw Error.ArgumentNull(nameof(second));
            if (comparer == null)
                throw Error.ArgumentNull(nameof(comparer));

            return new ExceptAsyncIterator<TSource>(first, second, comparer);
        }

        private sealed class ExceptAsyncIterator<TSource> : AsyncIterator<TSource>
        {
            private readonly IEqualityComparer<TSource> _comparer;
            private readonly IAsyncEnumerable<TSource> _first;
            private readonly IAsyncEnumerable<TSource> _second;

            private Task _fillSetTask;

            private IAsyncEnumerator<TSource> _firstEnumerator;
            private Set<TSource> _set;

            private bool _setFilled;

            public ExceptAsyncIterator(IAsyncEnumerable<TSource> first, IAsyncEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
            {
                Debug.Assert(first != null);
                Debug.Assert(second != null);
                Debug.Assert(comparer != null);

                _first = first;
                _second = second;
                _comparer = comparer;
            }

            public override AsyncIterator<TSource> Clone()
            {
                return new ExceptAsyncIterator<TSource>(_first, _second, _comparer);
            }

            public override async ValueTask DisposeAsync()
            {
                if (_firstEnumerator != null)
                {
                    await _firstEnumerator.DisposeAsync().ConfigureAwait(false);
                    _firstEnumerator = null;
                }

                _set = null;

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async ValueTask<bool> MoveNextCore(CancellationToken cancellationToken)
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        _firstEnumerator = _first.GetAsyncEnumerator(cancellationToken);
                        _set = new Set<TSource>(_comparer);
                        _setFilled = false;
                        _fillSetTask = FillSetAsync();

                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        bool moveNext;
                        do
                        {
                            if (!_setFilled)
                            {
                                // This is here so we don't need to call Task.WhenAll each time after the set is filled
                                var moveNextTask = _firstEnumerator.MoveNextAsync();
                                await Task.WhenAll(moveNextTask.AsTask(), _fillSetTask).ConfigureAwait(false);

                                _setFilled = true;
                                moveNext = moveNextTask.Result;
                            }
                            else
                            {
                                moveNext = await _firstEnumerator.MoveNextAsync().ConfigureAwait(false);
                            }

                            if (moveNext)
                            {
                                var item = _firstEnumerator.Current;
                                if (_set.Add(item))
                                {
                                    current = item;
                                    return true;
                                }
                            }
                        } while (moveNext);

                        await DisposeAsync().ConfigureAwait(false);
                        break;
                }

                return false;
            }

            private async Task FillSetAsync()
            {
                var array = await _second.ToArray().ConfigureAwait(false);

                foreach (var t in array)
                {
                    _set.Add(t);
                }
            }
        }
    }
}

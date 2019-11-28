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
        /// <summary>
        /// Concatenates the second async-enumerable sequence to the first async-enumerable sequence upon successful termination of the first.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequences.</typeparam>
        /// <param name="first">First async-enumerable sequence.</param>
        /// <param name="second">Second async-enumerable sequence.</param>
        /// <returns>An async-enumerable sequence that contains the elements of the first sequence, followed by those of the second the sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="first"/> or <paramref name="second"/> is null.</exception>
        public static IAsyncEnumerable<TSource> Concat<TSource>(this IAsyncEnumerable<TSource> first, IAsyncEnumerable<TSource> second)
        {
            if (first == null)
                throw Error.ArgumentNull(nameof(first));
            if (second == null)
                throw Error.ArgumentNull(nameof(second));

            return first is ConcatAsyncIterator<TSource> concatFirst ?
                       concatFirst.Concat(second) :
                       new Concat2AsyncIterator<TSource>(first, second);
        }

        private sealed class Concat2AsyncIterator<TSource> : ConcatAsyncIterator<TSource>
        {
            private readonly IAsyncEnumerable<TSource> _first;
            private readonly IAsyncEnumerable<TSource> _second;

            internal Concat2AsyncIterator(IAsyncEnumerable<TSource> first, IAsyncEnumerable<TSource> second)
            {
                _first = first;
                _second = second;
            }

            public override AsyncIteratorBase<TSource> Clone()
            {
                return new Concat2AsyncIterator<TSource>(_first, _second);
            }

            internal override ConcatAsyncIterator<TSource> Concat(IAsyncEnumerable<TSource> next)
            {
                return new ConcatNAsyncIterator<TSource>(this, next, 2);
            }

            internal override IAsyncEnumerable<TSource>? GetAsyncEnumerable(int index)
            {
                return index switch
                {
                    0 => _first,
                    1 => _second,
                    _ => null,
                };
            }
        }

        private abstract class ConcatAsyncIterator<TSource> : AsyncIterator<TSource>, IAsyncIListProvider<TSource>
        {
            private int _counter;
            private IAsyncEnumerator<TSource>? _enumerator;

            public ValueTask<TSource[]> ToArrayAsync(CancellationToken cancellationToken)
            {
                return AsyncEnumerableHelpers.ToArray(this, cancellationToken);
            }

            public async ValueTask<List<TSource>> ToListAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var list = new List<TSource>();
                for (var i = 0; ; i++)
                {
                    var source = GetAsyncEnumerable(i);
                    if (source == null)
                    {
                        break;
                    }

                    await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                    {
                        list.Add(item);
                    }
                }

                return list;
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
                    for (var i = 0; ; i++)
                    {
                        var source = GetAsyncEnumerable(i);
                        if (source == null)
                        {
                            break;
                        }

                        checked
                        {
                            count += await source.CountAsync(cancellationToken).ConfigureAwait(false);
                        }
                    }

                    return count;
                }
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
                if (_state == AsyncIteratorState.Allocated)
                {
                    _enumerator = GetAsyncEnumerable(0)!.GetAsyncEnumerator(_cancellationToken);
                    _state = AsyncIteratorState.Iterating;
                    _counter = 2;
                }

                if (_state == AsyncIteratorState.Iterating)
                {
                    while (true)
                    {
                        if (await _enumerator!.MoveNextAsync().ConfigureAwait(false))
                        {
                            _current = _enumerator.Current;
                            return true;
                        }

                        //
                        // NB: This is simply to match the logic of 
                        //     https://github.com/dotnet/corefx/blob/f7539b726c4bc2385b7f49e5751c1cff2f2c7368/src/System.Linq/src/System/Linq/Concat.cs#L240
                        //

                        var next = GetAsyncEnumerable(_counter++ - 1);
                        if (next != null)
                        {
                            await _enumerator.DisposeAsync().ConfigureAwait(false);
                            _enumerator = next.GetAsyncEnumerator(_cancellationToken);
                            continue;
                        }

                        await DisposeAsync().ConfigureAwait(false);
                        break;
                    }
                }

                return false;
            }

            internal abstract ConcatAsyncIterator<TSource> Concat(IAsyncEnumerable<TSource> next);

            internal abstract IAsyncEnumerable<TSource>? GetAsyncEnumerable(int index);
        }

        // To handle chains of >= 3 sources, we chain the concat iterators together and allow
        // GetEnumerable to fetch enumerables from the previous sources.  This means that rather
        // than each MoveNext/Current calls having to traverse all of the previous sources, we
        // only have to traverse all of the previous sources once per chained enumerable.  An
        // alternative would be to use an array to store all of the enumerables, but this has
        // a much better memory profile and without much additional run-time cost.
        private sealed class ConcatNAsyncIterator<TSource> : ConcatAsyncIterator<TSource>
        {
            private readonly IAsyncEnumerable<TSource> _next;
            private readonly int _nextIndex;
            private readonly ConcatAsyncIterator<TSource> _previousConcat;

            internal ConcatNAsyncIterator(ConcatAsyncIterator<TSource> previousConcat, IAsyncEnumerable<TSource> next, int nextIndex)
            {
                Debug.Assert(nextIndex >= 2);

                _previousConcat = previousConcat;
                _next = next;
                _nextIndex = nextIndex;
            }

            public override AsyncIteratorBase<TSource> Clone()
            {
                return new ConcatNAsyncIterator<TSource>(_previousConcat, _next, _nextIndex);
            }

            internal override ConcatAsyncIterator<TSource> Concat(IAsyncEnumerable<TSource> next)
            {
                if (_nextIndex == int.MaxValue - 2)
                {
                    // In the unlikely case of this many concatenations, if we produced a ConcatNIterator
                    // with int.MaxValue then state would overflow before it matched it's index.
                    // So we use the naïve approach of just having a left and right sequence.
                    return new Concat2AsyncIterator<TSource>(this, next);
                }

                return new ConcatNAsyncIterator<TSource>(this, next, _nextIndex + 1);
            }

            internal override IAsyncEnumerable<TSource>? GetAsyncEnumerable(int index)
            {
                if (index > _nextIndex)
                {
                    return null;
                }

                // Walk back through the chain of ConcatNIterators looking for the one
                // that has its _nextIndex equal to index.  If we don't find one, then it
                // must be prior to any of them, so call GetEnumerable on the previous
                // Concat2Iterator.  This avoids a deep recursive call chain.
                var current = this;
                while (true)
                {
                    if (index == current._nextIndex)
                    {
                        return current._next;
                    }

                    if (current._previousConcat is ConcatNAsyncIterator<TSource> prevN)
                    {
                        current = prevN;
                        continue;
                    }

                    Debug.Assert(current._previousConcat is Concat2AsyncIterator<TSource>);
                    Debug.Assert(index == 0 || index == 1);
                    return current._previousConcat.GetAsyncEnumerable(index);
                }
            }
        }
    }
}

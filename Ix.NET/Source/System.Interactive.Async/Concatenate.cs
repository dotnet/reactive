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
        public static IAsyncEnumerable<TSource> Concat<TSource>(this IAsyncEnumerable<TSource> first, IAsyncEnumerable<TSource> second)
        {
            if (first == null)
            {
                throw new ArgumentNullException(nameof(first));
            }

            if (second == null)
            {
                throw new ArgumentNullException(nameof(second));
            }

            return first is ConcatAsyncIterator<TSource> concatFirst ?
                       concatFirst.Concat(second) :
                       new Concat2AsyncIterator<TSource>(first, second);
        }

        public static IAsyncEnumerable<TSource> Concat<TSource>(this IEnumerable<IAsyncEnumerable<TSource>> sources)
        {
            if (sources == null)
            {
                throw new ArgumentNullException(nameof(sources));
            }

            return sources.Concat_();
        }

        public static IAsyncEnumerable<TSource> Concat<TSource>(params IAsyncEnumerable<TSource>[] sources)
        {
            if (sources == null)
            {
                throw new ArgumentNullException(nameof(sources));
            }

            return sources.Concat_();
        }

        private static IAsyncEnumerable<TSource> Concat_<TSource>(this IEnumerable<IAsyncEnumerable<TSource>> sources)
        {
            return new ConcatEnumerableAsyncIterator<TSource>(sources);
        }

        private sealed class ConcatEnumerableAsyncIterator<TSource> : AsyncIterator<TSource>
        {
            private readonly IEnumerable<IAsyncEnumerable<TSource>> _source;

            public ConcatEnumerableAsyncIterator(IEnumerable<IAsyncEnumerable<TSource>> source)
            {
                Debug.Assert(source != null);

                _source = source;
            }

            public override AsyncIterator<TSource> Clone()
            {
                return new ConcatEnumerableAsyncIterator<TSource>(_source);
            }

            public override void Dispose()
            {
                if (_outerEnumerator != null)
                {
                    _outerEnumerator.Dispose();
                    _outerEnumerator = null;
                }

                if (_currentEnumerator != null)
                {
                    _currentEnumerator.Dispose();
                    _currentEnumerator = null;
                }

                base.Dispose();
            }

            // State machine vars
            private IEnumerator<IAsyncEnumerable<TSource>> _outerEnumerator;
            private IAsyncEnumerator<TSource> _currentEnumerator;
            private int _mode;
            private const int State_OuterNext = 1;
            private const int State_While = 4;

            protected override async Task<bool> MoveNextCore(CancellationToken cancellationToken)
            {

                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        _outerEnumerator = _source.GetEnumerator();
                        _mode = State_OuterNext;
                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        switch (_mode)
                        {
                            case State_OuterNext:
                                if (_outerEnumerator.MoveNext())
                                {
                                    // make sure we dispose the previous one if we're about to replace it
                                    _currentEnumerator?.Dispose();
                                    _currentEnumerator = _outerEnumerator.Current.GetEnumerator();

                                    _mode = State_While;
                                    goto case State_While;
                                }

                                break;
                            case State_While:
                                if (await _currentEnumerator.MoveNext(cancellationToken)
                                                           .ConfigureAwait(false))
                                {
                                    current = _currentEnumerator.Current;
                                    return true;
                                }

                                // No more on the inner enumerator, move to the next outer
                                goto case State_OuterNext;

                        }

                        Dispose();
                        break;
                }

                return false;
            }
        }

        private sealed class Concat2AsyncIterator<TSource> : ConcatAsyncIterator<TSource>
        {
            private readonly IAsyncEnumerable<TSource> _first;
            private readonly IAsyncEnumerable<TSource> _second;

            internal Concat2AsyncIterator(IAsyncEnumerable<TSource> first, IAsyncEnumerable<TSource> second)
            {
                Debug.Assert(first != null);
                Debug.Assert(second != null);

                _first = first;
                _second = second;
            }

            public override AsyncIterator<TSource> Clone()
            {
                return new Concat2AsyncIterator<TSource>(_first, _second);
            }

            internal override ConcatAsyncIterator<TSource> Concat(IAsyncEnumerable<TSource> next)
            {
                return new ConcatNAsyncIterator<TSource>(this, next, 2);
            }

            internal override IAsyncEnumerable<TSource> GetAsyncEnumerable(int index)
            {
                switch (index)
                {
                    case 0:
                        return _first;
                    case 1:
                        return _second;
                    default:
                        return null;
                }
            }
        }

        private abstract class ConcatAsyncIterator<TSource> : AsyncIterator<TSource>, IIListProvider<TSource>
        {
            private int _counter;
            private IAsyncEnumerator<TSource> _enumerator;

            public Task<TSource[]> ToArrayAsync(CancellationToken cancellationToken)
            {
                return AsyncEnumerableHelpers.ToArray(this, cancellationToken);
            }

            public async Task<List<TSource>> ToListAsync(CancellationToken cancellationToken)
            {
                var list = new List<TSource>();
                for (var i = 0; ; i++)
                {
                    var source = GetAsyncEnumerable(i);
                    if (source == null)
                    {
                        break;
                    }
                    using (var e = source.GetEnumerator())
                    {
                        while (await e.MoveNext(cancellationToken)
                                      .ConfigureAwait(false))
                        {
                            list.Add(e.Current);
                        }
                    }
                }

                return list;
            }

            public async Task<int> GetCountAsync(bool onlyIfCheap, CancellationToken cancellationToken)
            {
                if (onlyIfCheap)
                {
                    return -1;
                }

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
                        count += await source.Count(cancellationToken).ConfigureAwait(false);
                    }
                }

                return count;
            }

            public override void Dispose()
            {
                if (_enumerator != null)
                {
                    _enumerator.Dispose();
                    _enumerator = null;
                }

                base.Dispose();
            }

            protected override async Task<bool> MoveNextCore(CancellationToken cancellationToken)
            {
                if (state == AsyncIteratorState.Allocated)
                {
                    _enumerator = GetAsyncEnumerable(0)
                        .GetEnumerator();
                    state = AsyncIteratorState.Iterating;
                    _counter = 2;
                }

                if (state == AsyncIteratorState.Iterating)
                {
                    while (true)
                    {
                        if (await _enumerator.MoveNext(cancellationToken)
                                            .ConfigureAwait(false))
                        {
                            current = _enumerator.Current;
                            return true;
                        }
                        // note, this is simply to match the logic of 
                        // https://github.com/dotnet/corefx/blob/ec2685715b01d12f16b08d0dfa326649b12db8ec/src/system.linq/src/system/linq/concatenate.cs#L173-L173
                        var next = GetAsyncEnumerable(_counter++ - 1);
                        if (next != null)
                        {
                            _enumerator.Dispose();
                            _enumerator = next.GetEnumerator();
                            continue;
                        }

                        Dispose();
                        break;
                    }
                }

                return false;
            }

            internal abstract ConcatAsyncIterator<TSource> Concat(IAsyncEnumerable<TSource> next);

            internal abstract IAsyncEnumerable<TSource> GetAsyncEnumerable(int index);
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
                Debug.Assert(previousConcat != null);
                Debug.Assert(next != null);
                Debug.Assert(nextIndex >= 2);

                _previousConcat = previousConcat;
                _next = next;
                _nextIndex = nextIndex;
            }

            public override AsyncIterator<TSource> Clone()
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

            internal override IAsyncEnumerable<TSource> GetAsyncEnumerable(int index)
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

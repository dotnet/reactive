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
        public static IAsyncEnumerable<TResult> Zip<TFirst, TSecond, TResult>(this IAsyncEnumerable<TFirst> first, IAsyncEnumerable<TSecond> second, Func<TFirst, TSecond, TResult> selector)
        {
            if (first == null)
                throw Error.ArgumentNull(nameof(first));
            if (second == null)
                throw Error.ArgumentNull(nameof(second));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return new ZipAsyncIterator<TFirst, TSecond, TResult>(first, second, selector);
        }

        public static IAsyncEnumerable<TResult> Zip<TFirst, TSecond, TResult>(this IAsyncEnumerable<TFirst> first, IAsyncEnumerable<TSecond> second, Func<TFirst, TSecond, Task<TResult>> selector)
        {
            if (first == null)
                throw Error.ArgumentNull(nameof(first));
            if (second == null)
                throw Error.ArgumentNull(nameof(second));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return new ZipAsyncIteratorWithTask<TFirst, TSecond, TResult>(first, second, selector);
        }

        private sealed class ZipAsyncIterator<TFirst, TSecond, TResult> : AsyncIterator<TResult>
        {
            private readonly IAsyncEnumerable<TFirst> _first;
            private readonly IAsyncEnumerable<TSecond> _second;
            private readonly Func<TFirst, TSecond, TResult> _selector;

            private IAsyncEnumerator<TFirst> _firstEnumerator;
            private IAsyncEnumerator<TSecond> _secondEnumerator;

            public ZipAsyncIterator(IAsyncEnumerable<TFirst> first, IAsyncEnumerable<TSecond> second, Func<TFirst, TSecond, TResult> selector)
            {
                Debug.Assert(first != null);
                Debug.Assert(second != null);
                Debug.Assert(selector != null);

                _first = first;
                _second = second;
                _selector = selector;
            }

            public override AsyncIterator<TResult> Clone()
            {
                return new ZipAsyncIterator<TFirst, TSecond, TResult>(_first, _second, _selector);
            }

            public override async ValueTask DisposeAsync()
            {
                if (_secondEnumerator != null)
                {
                    await _secondEnumerator.DisposeAsync().ConfigureAwait(false);
                    _secondEnumerator = null;
                }

                if (_firstEnumerator != null)
                {
                    await _firstEnumerator.DisposeAsync().ConfigureAwait(false);
                    _firstEnumerator = null;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async ValueTask<bool> MoveNextCore()
            {
                // REVIEW: Earlier versions of this operator performed concurrent MoveNextAsync calls, which isn't a great default and
                //         results in an unexpected source of concurrency. However, a concurrent Zip may be a worthy addition to the
                //         API or System.Interactive.Async as a complementary implementation besides the conservative default.

                switch (_state)
                {
                    case AsyncIteratorState.Allocated:
                        _firstEnumerator = _first.GetAsyncEnumerator(_cancellationToken);
                        _secondEnumerator = _second.GetAsyncEnumerator(_cancellationToken);

                        _state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        if (await _firstEnumerator.MoveNextAsync().ConfigureAwait(false) && await _secondEnumerator.MoveNextAsync().ConfigureAwait(false))
                        {
                            _current = _selector(_firstEnumerator.Current, _secondEnumerator.Current);
                            return true;
                        }

                        await DisposeAsync().ConfigureAwait(false);
                        break;
                }

                return false;
            }
        }

        private sealed class ZipAsyncIteratorWithTask<TFirst, TSecond, TResult> : AsyncIterator<TResult>
        {
            private readonly IAsyncEnumerable<TFirst> _first;
            private readonly IAsyncEnumerable<TSecond> _second;
            private readonly Func<TFirst, TSecond, Task<TResult>> _selector;

            private IAsyncEnumerator<TFirst> _firstEnumerator;
            private IAsyncEnumerator<TSecond> _secondEnumerator;

            public ZipAsyncIteratorWithTask(IAsyncEnumerable<TFirst> first, IAsyncEnumerable<TSecond> second, Func<TFirst, TSecond, Task<TResult>> selector)
            {
                Debug.Assert(first != null);
                Debug.Assert(second != null);
                Debug.Assert(selector != null);

                _first = first;
                _second = second;
                _selector = selector;
            }

            public override AsyncIterator<TResult> Clone()
            {
                return new ZipAsyncIteratorWithTask<TFirst, TSecond, TResult>(_first, _second, _selector);
            }

            public override async ValueTask DisposeAsync()
            {
                if (_secondEnumerator != null)
                {
                    await _secondEnumerator.DisposeAsync().ConfigureAwait(false);
                    _secondEnumerator = null;
                }

                if (_firstEnumerator != null)
                {
                    await _firstEnumerator.DisposeAsync().ConfigureAwait(false);
                    _firstEnumerator = null;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async ValueTask<bool> MoveNextCore()
            {
                // REVIEW: Earlier versions of this operator performed concurrent MoveNextAsync calls, which isn't a great default and
                //         results in an unexpected source of concurrency. However, a concurrent Zip may be a worthy addition to the
                //         API or System.Interactive.Async as a complementary implementation besides the conservative default.

                switch (_state)
                {
                    case AsyncIteratorState.Allocated:
                        _firstEnumerator = _first.GetAsyncEnumerator(_cancellationToken);
                        _secondEnumerator = _second.GetAsyncEnumerator(_cancellationToken);

                        _state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        if (await _firstEnumerator.MoveNextAsync().ConfigureAwait(false) && await _secondEnumerator.MoveNextAsync().ConfigureAwait(false))
                        {
                            _current = await _selector(_firstEnumerator.Current, _secondEnumerator.Current).ConfigureAwait(false);
                            return true;
                        }

                        await DisposeAsync().ConfigureAwait(false);
                        break;
                }

                return false;
            }
        }
    }
}

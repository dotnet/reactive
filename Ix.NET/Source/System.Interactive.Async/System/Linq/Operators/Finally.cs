// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerableEx
    {
        public static IAsyncEnumerable<TSource> Finally<TSource>(this IAsyncEnumerable<TSource> source, Action finallyAction)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (finallyAction == null)
                throw Error.ArgumentNull(nameof(finallyAction));

#if USE_ASYNC_ITERATOR
            return Create(Core);

            async IAsyncEnumerator<TSource> Core(CancellationToken cancellationToken)
            {
                try
                {
                    await foreach (TSource item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                    {
                        yield return item;
                    }
                }
                finally
                {
                    finallyAction();
                }
            }
#else
            return new FinallyAsyncIterator<TSource>(source, finallyAction);
#endif
        }

        public static IAsyncEnumerable<TSource> Finally<TSource>(this IAsyncEnumerable<TSource> source, Func<Task> finallyAction)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (finallyAction == null)
                throw Error.ArgumentNull(nameof(finallyAction));

#if USE_ASYNC_ITERATOR
            return Create(Core);

            async IAsyncEnumerator<TSource> Core(CancellationToken cancellationToken)
            {
                try
                {
                    await foreach (TSource item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                    {
                        yield return item;
                    }
                }
                finally
                {
                    await finallyAction().ConfigureAwait(false);
                }
            }
#else
            return new FinallyAsyncIteratorWithTask<TSource>(source, finallyAction);
#endif
        }

        // REVIEW: No cancellation support for finally action.

#if !USE_ASYNC_ITERATOR
        private sealed class FinallyAsyncIterator<TSource> : AsyncIterator<TSource>
        {
            private readonly Action _finallyAction;
            private readonly IAsyncEnumerable<TSource> _source;

            private IAsyncEnumerator<TSource> _enumerator;

            public FinallyAsyncIterator(IAsyncEnumerable<TSource> source, Action finallyAction)
            {
                Debug.Assert(source != null);
                Debug.Assert(finallyAction != null);

                _source = source;
                _finallyAction = finallyAction;
            }

            public override AsyncIteratorBase<TSource> Clone()
            {
                return new FinallyAsyncIterator<TSource>(_source, _finallyAction);
            }

            public override async ValueTask DisposeAsync()
            {
                if (_enumerator != null)
                {
                    await _enumerator.DisposeAsync().ConfigureAwait(false);
                    _enumerator = null;

                    _finallyAction();
                }

                await base.DisposeAsync().ConfigureAwait(false);
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
                            _current = _enumerator.Current;
                            return true;
                        }

                        await DisposeAsync().ConfigureAwait(false);
                        break;
                }

                return false;
            }
        }

        private sealed class FinallyAsyncIteratorWithTask<TSource> : AsyncIterator<TSource>
        {
            private readonly Func<Task> _finallyAction;
            private readonly IAsyncEnumerable<TSource> _source;

            private IAsyncEnumerator<TSource> _enumerator;

            public FinallyAsyncIteratorWithTask(IAsyncEnumerable<TSource> source, Func<Task> finallyAction)
            {
                Debug.Assert(source != null);
                Debug.Assert(finallyAction != null);

                _source = source;
                _finallyAction = finallyAction;
            }

            public override AsyncIteratorBase<TSource> Clone()
            {
                return new FinallyAsyncIteratorWithTask<TSource>(_source, _finallyAction);
            }

            public override async ValueTask DisposeAsync()
            {
                if (_enumerator != null)
                {
                    await _enumerator.DisposeAsync().ConfigureAwait(false);
                    _enumerator = null;

                    await _finallyAction().ConfigureAwait(false);
                }

                await base.DisposeAsync().ConfigureAwait(false);
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
                            _current = _enumerator.Current;
                            return true;
                        }

                        await DisposeAsync().ConfigureAwait(false);
                        break;
                }

                return false;
            }
        }
    }
#endif
}

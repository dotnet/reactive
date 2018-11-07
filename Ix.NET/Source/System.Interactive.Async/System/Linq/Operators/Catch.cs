// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerableEx
    {
        public static IAsyncEnumerable<TSource> Catch<TSource, TException>(this IAsyncEnumerable<TSource> source, Func<TException, IAsyncEnumerable<TSource>> handler)
            where TException : Exception
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            return new CatchAsyncIterator<TSource, TException>(source, handler);
        }

        public static IAsyncEnumerable<TSource> Catch<TSource, TException>(this IAsyncEnumerable<TSource> source, Func<TException, Task<IAsyncEnumerable<TSource>>> handler)
            where TException : Exception
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            return new CatchAsyncIteratorWithTask<TSource, TException>(source, handler);
        }

        public static IAsyncEnumerable<TSource> Catch<TSource>(this IEnumerable<IAsyncEnumerable<TSource>> sources)
        {
            if (sources == null)
                throw new ArgumentNullException(nameof(sources));

            return CatchCore(sources);
        }

        public static IAsyncEnumerable<TSource> Catch<TSource>(params IAsyncEnumerable<TSource>[] sources)
        {
            if (sources == null)
                throw new ArgumentNullException(nameof(sources));

            return CatchCore(sources);
        }

        public static IAsyncEnumerable<TSource> Catch<TSource>(this IAsyncEnumerable<TSource> first, IAsyncEnumerable<TSource> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));

            return CatchCore(new[] { first, second });
        }

        private static IAsyncEnumerable<TSource> CatchCore<TSource>(IEnumerable<IAsyncEnumerable<TSource>> sources)
        {
            return new CatchAsyncIterator<TSource>(sources);
        }

        private sealed class CatchAsyncIterator<TSource, TException> : AsyncIterator<TSource> where TException : Exception
        {
            private readonly Func<TException, IAsyncEnumerable<TSource>> _handler;
            private readonly IAsyncEnumerable<TSource> _source;

            private IAsyncEnumerator<TSource> _enumerator;
            private bool _isDone;

            public CatchAsyncIterator(IAsyncEnumerable<TSource> source, Func<TException, IAsyncEnumerable<TSource>> handler)
            {
                Debug.Assert(source != null);
                Debug.Assert(handler != null);

                _source = source;
                _handler = handler;
            }

            public override AsyncIterator<TSource> Clone()
            {
                return new CatchAsyncIterator<TSource, TException>(_source, _handler);
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

            protected override async ValueTask<bool> MoveNextCore(CancellationToken cancellationToken)
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        _enumerator = _source.GetAsyncEnumerator(cancellationToken);
                        _isDone = false;

                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        while (true)
                        {
                            if (!_isDone)
                            {
                                try
                                {
                                    if (await _enumerator.MoveNextAsync().ConfigureAwait(false))
                                    {
                                        current = _enumerator.Current;
                                        return true;
                                    }
                                }
                                catch (TException ex)
                                {
                                    // Note: Ideally we'd dipose of the previous enumerator before
                                    // invoking the handler, but we use this order to preserve
                                    // current behavior
                                    var inner = _handler(ex);
                                    var err = inner.GetAsyncEnumerator(cancellationToken);

                                    if (_enumerator != null)
                                    {
                                        await _enumerator.DisposeAsync().ConfigureAwait(false);
                                    }

                                    _enumerator = err;
                                    _isDone = true;
                                    continue; // loop so we hit the catch state
                                }
                            }

                            if (await _enumerator.MoveNextAsync().ConfigureAwait(false))
                            {
                                current = _enumerator.Current;
                                return true;
                            }

                            break; // while
                        }

                        break; // case
                }

                await DisposeAsync().ConfigureAwait(false);
                return false;
            }
        }

        private sealed class CatchAsyncIteratorWithTask<TSource, TException> : AsyncIterator<TSource> where TException : Exception
        {
            private readonly Func<TException, Task<IAsyncEnumerable<TSource>>> _handler;
            private readonly IAsyncEnumerable<TSource> _source;

            private IAsyncEnumerator<TSource> _enumerator;
            private bool _isDone;

            public CatchAsyncIteratorWithTask(IAsyncEnumerable<TSource> source, Func<TException, Task<IAsyncEnumerable<TSource>>> handler)
            {
                Debug.Assert(source != null);
                Debug.Assert(handler != null);

                _source = source;
                _handler = handler;
            }

            public override AsyncIterator<TSource> Clone()
            {
                return new CatchAsyncIteratorWithTask<TSource, TException>(_source, _handler);
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

            protected override async ValueTask<bool> MoveNextCore(CancellationToken cancellationToken)
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        _enumerator = _source.GetAsyncEnumerator(cancellationToken);
                        _isDone = false;

                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        while (true)
                        {
                            if (!_isDone)
                            {
                                try
                                {
                                    if (await _enumerator.MoveNextAsync().ConfigureAwait(false))
                                    {
                                        current = _enumerator.Current;
                                        return true;
                                    }
                                }
                                catch (TException ex)
                                {
                                    // Note: Ideally we'd dipose of the previous enumerator before
                                    // invoking the handler, but we use this order to preserve
                                    // current behavior
                                    var inner = await _handler(ex).ConfigureAwait(false);
                                    var err = inner.GetAsyncEnumerator(cancellationToken);

                                    if (_enumerator != null)
                                    {
                                        await _enumerator.DisposeAsync().ConfigureAwait(false);
                                    }

                                    _enumerator = err;
                                    _isDone = true;
                                    continue; // loop so we hit the catch state
                                }
                            }

                            if (await _enumerator.MoveNextAsync().ConfigureAwait(false))
                            {
                                current = _enumerator.Current;
                                return true;
                            }

                            break; // while
                        }

                        break; // case
                }

                await DisposeAsync().ConfigureAwait(false);
                return false;
            }
        }

        private sealed class CatchAsyncIterator<TSource> : AsyncIterator<TSource>
        {
            private readonly IEnumerable<IAsyncEnumerable<TSource>> _sources;

            private IAsyncEnumerator<TSource> _enumerator;
            private ExceptionDispatchInfo _error;

            private IEnumerator<IAsyncEnumerable<TSource>> _sourcesEnumerator;

            public CatchAsyncIterator(IEnumerable<IAsyncEnumerable<TSource>> sources)
            {
                Debug.Assert(sources != null);

                _sources = sources;
            }

            public override AsyncIterator<TSource> Clone()
            {
                return new CatchAsyncIterator<TSource>(_sources);
            }

            public override async ValueTask DisposeAsync()
            {
                if (_sourcesEnumerator != null)
                {
                    _sourcesEnumerator.Dispose();
                    _sourcesEnumerator = null;
                }

                if (_enumerator != null)
                {
                    await _enumerator.DisposeAsync().ConfigureAwait(false);
                    _enumerator = null;
                }

                _error = null;

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async ValueTask<bool> MoveNextCore(CancellationToken cancellationToken)
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        _sourcesEnumerator = _sources.GetEnumerator();

                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        while (true)
                        {
                            if (_enumerator == null)
                            {
                                if (!_sourcesEnumerator.MoveNext())
                                {
                                    // only throw if we have an error on the last one
                                    _error?.Throw();
                                    break; // done, nothing else to do
                                }

                                _error = null;
                                _enumerator = _sourcesEnumerator.Current.GetAsyncEnumerator(cancellationToken);
                            }

                            try
                            {
                                if (await _enumerator.MoveNextAsync().ConfigureAwait(false))
                                {
                                    current = _enumerator.Current;
                                    return true;
                                }
                            }
                            catch (Exception ex)
                            {
                                // Done with the current one, go to the next
                                await _enumerator.DisposeAsync().ConfigureAwait(false);
                                _enumerator = null;
                                _error = ExceptionDispatchInfo.Capture(ex);
                                continue;
                            }

                            break; // while
                        }

                        break; // case
                }

                await DisposeAsync().ConfigureAwait(false);
                return false;
            }
        }
    }
}

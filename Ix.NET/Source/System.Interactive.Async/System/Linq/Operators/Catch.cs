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
        // REVIEW: All Catch operators may catch OperationCanceledException due to cancellation of the enumeration
        //         of the source. Should we explicitly avoid handling this? E.g. as follows:
        //
        //         catch (TException ex) when(!(ex is OperationCanceledException oce && oce.CancellationToken == cancellationToken))

        public static IAsyncEnumerable<TSource> Catch<TSource, TException>(this IAsyncEnumerable<TSource> source, Func<TException, IAsyncEnumerable<TSource>> handler)
            where TException : Exception
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (handler == null)
                throw Error.ArgumentNull(nameof(handler));

#if USE_ASYNC_ITERATOR
            return Create(Core);

            async IAsyncEnumerator<TSource> Core(CancellationToken cancellationToken)
            {
                // REVIEW: This implementation mirrors the Ix implementation, which does not protect GetEnumerator
                //         using the try statement either. A more trivial implementation would use await foreach
                //         and protect the entire loop using a try statement, with two breaking changes:
                //
                //         - Also protecting the call to GetAsyncEnumerator by the try statement.
                //         - Invocation of the handler after disposal of the failed first sequence.

                var err = default(IAsyncEnumerable<TSource>);

                await using (var e = source.GetAsyncEnumerator(cancellationToken).ConfigureAwait(false))
                {
                    while (true)
                    {
                        var c = default(TSource);

                        try
                        {
                            if (!await e.MoveNextAsync())
                                break;

                            c = e.Current;
                        }
                        catch (TException ex)
                        {
                            err = handler(ex);
                            break;
                        }

                        yield return c;
                    }
                }

                if (err != null)
                {
                    await foreach (var item in err.WithCancellation(cancellationToken).ConfigureAwait(false))
                    {
                        yield return item;
                    }
                }
            }
#else
            return new CatchAsyncIterator<TSource, TException>(source, handler);
#endif
        }

        public static IAsyncEnumerable<TSource> Catch<TSource, TException>(this IAsyncEnumerable<TSource> source, Func<TException, ValueTask<IAsyncEnumerable<TSource>>> handler)
            where TException : Exception
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (handler == null)
                throw Error.ArgumentNull(nameof(handler));

#if USE_ASYNC_ITERATOR
            return Create(Core);

            async IAsyncEnumerator<TSource> Core(CancellationToken cancellationToken)
            {
                // REVIEW: This implementation mirrors the Ix implementation, which does not protect GetEnumerator
                //         using the try statement either. A more trivial implementation would use await foreach
                //         and protect the entire loop using a try statement, with two breaking changes:
                //
                //         - Also protecting the call to GetAsyncEnumerator by the try statement.
                //         - Invocation of the handler after disposal of the failed first sequence.

                var err = default(IAsyncEnumerable<TSource>);

                await using (var e = source.GetAsyncEnumerator(cancellationToken).ConfigureAwait(false))
                {
                    while (true)
                    {
                        var c = default(TSource);

                        try
                        {
                            if (!await e.MoveNextAsync())
                                break;

                            c = e.Current;
                        }
                        catch (TException ex)
                        {
                            err = await handler(ex).ConfigureAwait(false);
                            break;
                        }

                        yield return c;
                    }
                }

                if (err != null)
                {
                    await foreach (var item in err.WithCancellation(cancellationToken).ConfigureAwait(false))
                    {
                        yield return item;
                    }
                }
            }
#else
            return new CatchAsyncIteratorWithTask<TSource, TException>(source, handler);
#endif
        }

#if !NO_DEEP_CANCELLATION
        public static IAsyncEnumerable<TSource> Catch<TSource, TException>(this IAsyncEnumerable<TSource> source, Func<TException, CancellationToken, ValueTask<IAsyncEnumerable<TSource>>> handler)
            where TException : Exception
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (handler == null)
                throw Error.ArgumentNull(nameof(handler));

#if USE_ASYNC_ITERATOR
            return Create(Core);

            async IAsyncEnumerator<TSource> Core(CancellationToken cancellationToken)
            {
                // REVIEW: This implementation mirrors the Ix implementation, which does not protect GetEnumerator
                //         using the try statement either. A more trivial implementation would use await foreach
                //         and protect the entire loop using a try statement, with two breaking changes:
                //
                //         - Also protecting the call to GetAsyncEnumerator by the try statement.
                //         - Invocation of the handler after disposal of the failed first sequence.

                var err = default(IAsyncEnumerable<TSource>);

                await using (var e = source.GetAsyncEnumerator(cancellationToken).ConfigureAwait(false))
                {
                    while (true)
                    {
                        var c = default(TSource);

                        try
                        {
                            if (!await e.MoveNextAsync())
                                break;

                            c = e.Current;
                        }
                        catch (TException ex)
                        {
                            err = await handler(ex, cancellationToken).ConfigureAwait(false);
                            break;
                        }

                        yield return c;
                    }
                }

                if (err != null)
                {
                    await foreach (var item in err.WithCancellation(cancellationToken).ConfigureAwait(false))
                    {
                        yield return item;
                    }
                }
            }
#else
            return new CatchAsyncIteratorWithTaskAndCancellation<TSource, TException>(source, handler);
#endif
        }
#endif

        public static IAsyncEnumerable<TSource> Catch<TSource>(this IEnumerable<IAsyncEnumerable<TSource>> sources)
        {
            if (sources == null)
                throw Error.ArgumentNull(nameof(sources));

            return CatchCore(sources);
        }

        public static IAsyncEnumerable<TSource> Catch<TSource>(params IAsyncEnumerable<TSource>[] sources)
        {
            if (sources == null)
                throw Error.ArgumentNull(nameof(sources));

            return CatchCore(sources);
        }

        public static IAsyncEnumerable<TSource> Catch<TSource>(this IAsyncEnumerable<TSource> first, IAsyncEnumerable<TSource> second)
        {
            if (first == null)
                throw Error.ArgumentNull(nameof(first));
            if (second == null)
                throw Error.ArgumentNull(nameof(second));

            return CatchCore(new[] { first, second });
        }

        private static IAsyncEnumerable<TSource> CatchCore<TSource>(IEnumerable<IAsyncEnumerable<TSource>> sources)
        {
#if USE_ASYNC_ITERATOR
            return Create(Core);

            async IAsyncEnumerator<TSource> Core(CancellationToken cancellationToken)
            {
                var error = default(ExceptionDispatchInfo);

                foreach (var source in sources)
                {
                    await using (var e = source.GetAsyncEnumerator(cancellationToken).ConfigureAwait(false))
                    {
                        error = null;

                        while (true)
                        {
                            var c = default(TSource);

                            try
                            {
                                if (!await e.MoveNextAsync())
                                    break;

                                c = e.Current;
                            }
                            catch (Exception ex)
                            {
                                error = ExceptionDispatchInfo.Capture(ex);
                                break;
                            }

                            yield return c;
                        }

                        if (error == null)
                            break;
                    }
                }

                error?.Throw();
            }
#else
            return new CatchAsyncIterator<TSource>(sources);
#endif
        }

#if !USE_ASYNC_ITERATOR
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

            public override AsyncIteratorBase<TSource> Clone()
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

            protected override async ValueTask<bool> MoveNextCore()
            {
                switch (_state)
                {
                    case AsyncIteratorState.Allocated:
                        _enumerator = _source.GetAsyncEnumerator(_cancellationToken);
                        _isDone = false;

                        _state = AsyncIteratorState.Iterating;
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
                                        _current = _enumerator.Current;
                                        return true;
                                    }
                                }
                                catch (TException ex)
                                {
                                    // Note: Ideally we'd dispose of the previous enumerator before
                                    // invoking the handler, but we use this order to preserve
                                    // current behavior
                                    var inner = _handler(ex);
                                    var err = inner.GetAsyncEnumerator(_cancellationToken);

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
                                _current = _enumerator.Current;
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
            private readonly Func<TException, ValueTask<IAsyncEnumerable<TSource>>> _handler;
            private readonly IAsyncEnumerable<TSource> _source;

            private IAsyncEnumerator<TSource> _enumerator;
            private bool _isDone;

            public CatchAsyncIteratorWithTask(IAsyncEnumerable<TSource> source, Func<TException, ValueTask<IAsyncEnumerable<TSource>>> handler)
            {
                Debug.Assert(source != null);
                Debug.Assert(handler != null);

                _source = source;
                _handler = handler;
            }

            public override AsyncIteratorBase<TSource> Clone()
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

            protected override async ValueTask<bool> MoveNextCore()
            {
                switch (_state)
                {
                    case AsyncIteratorState.Allocated:
                        _enumerator = _source.GetAsyncEnumerator(_cancellationToken);
                        _isDone = false;

                        _state = AsyncIteratorState.Iterating;
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
                                        _current = _enumerator.Current;
                                        return true;
                                    }
                                }
                                catch (TException ex)
                                {
                                    // Note: Ideally we'd dispose of the previous enumerator before
                                    // invoking the handler, but we use this order to preserve
                                    // current behavior
                                    var inner = await _handler(ex).ConfigureAwait(false);
                                    var err = inner.GetAsyncEnumerator(_cancellationToken);

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
                                _current = _enumerator.Current;
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

#if !NO_DEEP_CANCELLATION
        private sealed class CatchAsyncIteratorWithTaskAndCancellation<TSource, TException> : AsyncIterator<TSource> where TException : Exception
        {
            private readonly Func<TException, CancellationToken, ValueTask<IAsyncEnumerable<TSource>>> _handler;
            private readonly IAsyncEnumerable<TSource> _source;

            private IAsyncEnumerator<TSource> _enumerator;
            private bool _isDone;

            public CatchAsyncIteratorWithTaskAndCancellation(IAsyncEnumerable<TSource> source, Func<TException, CancellationToken, ValueTask<IAsyncEnumerable<TSource>>> handler)
            {
                Debug.Assert(source != null);
                Debug.Assert(handler != null);

                _source = source;
                _handler = handler;
            }

            public override AsyncIteratorBase<TSource> Clone()
            {
                return new CatchAsyncIteratorWithTaskAndCancellation<TSource, TException>(_source, _handler);
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
                        _isDone = false;

                        _state = AsyncIteratorState.Iterating;
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
                                        _current = _enumerator.Current;
                                        return true;
                                    }
                                }
                                catch (TException ex)
                                {
                                    // Note: Ideally we'd dispose of the previous enumerator before
                                    // invoking the handler, but we use this order to preserve
                                    // current behavior
                                    var inner = await _handler(ex, _cancellationToken).ConfigureAwait(false);
                                    var err = inner.GetAsyncEnumerator(_cancellationToken);

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
                                _current = _enumerator.Current;
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
#endif

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

            public override AsyncIteratorBase<TSource> Clone()
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

            protected override async ValueTask<bool> MoveNextCore()
            {
                switch (_state)
                {
                    case AsyncIteratorState.Allocated:
                        _sourcesEnumerator = _sources.GetEnumerator();

                        _state = AsyncIteratorState.Iterating;
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
                                _enumerator = _sourcesEnumerator.Current.GetAsyncEnumerator(_cancellationToken);
                            }

                            try
                            {
                                if (await _enumerator.MoveNextAsync().ConfigureAwait(false))
                                {
                                    _current = _enumerator.Current;
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
#endif
}

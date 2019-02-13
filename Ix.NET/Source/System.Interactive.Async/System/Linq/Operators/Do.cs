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
        // REVIEW: Should we convert Task-based overloads to ValueTask?

        public static IAsyncEnumerable<TSource> Do<TSource>(this IAsyncEnumerable<TSource> source, Action<TSource> onNext)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (onNext == null)
                throw Error.ArgumentNull(nameof(onNext));

            return DoCore(source, onNext: onNext, onError: null, onCompleted: null);
        }

        public static IAsyncEnumerable<TSource> Do<TSource>(this IAsyncEnumerable<TSource> source, Action<TSource> onNext, Action onCompleted)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (onNext == null)
                throw Error.ArgumentNull(nameof(onNext));
            if (onCompleted == null)
                throw Error.ArgumentNull(nameof(onCompleted));

            return DoCore(source, onNext: onNext, onError: null, onCompleted: onCompleted);
        }

        public static IAsyncEnumerable<TSource> Do<TSource>(this IAsyncEnumerable<TSource> source, Action<TSource> onNext, Action<Exception> onError)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (onNext == null)
                throw Error.ArgumentNull(nameof(onNext));
            if (onError == null)
                throw Error.ArgumentNull(nameof(onError));

            return DoCore(source, onNext: onNext, onError: onError, onCompleted: null);
        }

        public static IAsyncEnumerable<TSource> Do<TSource>(this IAsyncEnumerable<TSource> source, Action<TSource> onNext, Action<Exception> onError, Action onCompleted)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (onNext == null)
                throw Error.ArgumentNull(nameof(onNext));
            if (onError == null)
                throw Error.ArgumentNull(nameof(onError));
            if (onCompleted == null)
                throw Error.ArgumentNull(nameof(onCompleted));

            return DoCore(source, onNext, onError, onCompleted);
        }

        public static IAsyncEnumerable<TSource> Do<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task> onNext)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (onNext == null)
                throw Error.ArgumentNull(nameof(onNext));

            return DoCore(source, onNext: onNext, onError: null, onCompleted: null);
        }

        public static IAsyncEnumerable<TSource> Do<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task> onNext, Func<Task> onCompleted)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (onNext == null)
                throw Error.ArgumentNull(nameof(onNext));
            if (onCompleted == null)
                throw Error.ArgumentNull(nameof(onCompleted));

            return DoCore(source, onNext: onNext, onError: null, onCompleted: onCompleted);
        }

        public static IAsyncEnumerable<TSource> Do<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task> onNext, Func<Exception, Task> onError)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (onNext == null)
                throw Error.ArgumentNull(nameof(onNext));
            if (onError == null)
                throw Error.ArgumentNull(nameof(onError));

            return DoCore(source, onNext: onNext, onError: onError, onCompleted: null);
        }

        public static IAsyncEnumerable<TSource> Do<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task> onNext, Func<Exception, Task> onError, Func<Task> onCompleted)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (onNext == null)
                throw Error.ArgumentNull(nameof(onNext));
            if (onError == null)
                throw Error.ArgumentNull(nameof(onError));
            if (onCompleted == null)
                throw Error.ArgumentNull(nameof(onCompleted));

            return DoCore(source, onNext, onError, onCompleted);
        }

#if !NO_DEEP_CANCELLATION
        public static IAsyncEnumerable<TSource> Do<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, Task> onNext)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (onNext == null)
                throw Error.ArgumentNull(nameof(onNext));

            return DoCore(source, onNext: onNext, onError: null, onCompleted: null);
        }

        public static IAsyncEnumerable<TSource> Do<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, Task> onNext, Func<CancellationToken, Task> onCompleted)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (onNext == null)
                throw Error.ArgumentNull(nameof(onNext));
            if (onCompleted == null)
                throw Error.ArgumentNull(nameof(onCompleted));

            return DoCore(source, onNext: onNext, onError: null, onCompleted: onCompleted);
        }

        public static IAsyncEnumerable<TSource> Do<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, Task> onNext, Func<Exception, CancellationToken, Task> onError)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (onNext == null)
                throw Error.ArgumentNull(nameof(onNext));
            if (onError == null)
                throw Error.ArgumentNull(nameof(onError));

            return DoCore(source, onNext: onNext, onError: onError, onCompleted: null);
        }

        public static IAsyncEnumerable<TSource> Do<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, Task> onNext, Func<Exception, CancellationToken, Task> onError, Func<CancellationToken, Task> onCompleted)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (onNext == null)
                throw Error.ArgumentNull(nameof(onNext));
            if (onError == null)
                throw Error.ArgumentNull(nameof(onError));
            if (onCompleted == null)
                throw Error.ArgumentNull(nameof(onCompleted));

            return DoCore(source, onNext, onError, onCompleted);
        }
#endif

        public static IAsyncEnumerable<TSource> Do<TSource>(this IAsyncEnumerable<TSource> source, IObserver<TSource> observer)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (observer == null)
                throw Error.ArgumentNull(nameof(observer));

            return DoCore(source, new Action<TSource>(observer.OnNext), new Action<Exception>(observer.OnError), new Action(observer.OnCompleted));
        }

        private static IAsyncEnumerable<TSource> DoCore<TSource>(IAsyncEnumerable<TSource> source, Action<TSource> onNext, Action<Exception> onError, Action onCompleted)
        {
#if USE_ASYNC_ITERATOR
            return AsyncEnumerable.Create(Core);

            async IAsyncEnumerator<TSource> Core(CancellationToken cancellationToken)
            {
                var e = source.GetAsyncEnumerator(cancellationToken).ConfigureAwait(false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    while (true)
                    {
                        TSource item;

                        try
                        {
                            if (!await e.MoveNextAsync())
                            {
                                break;
                            }

                            item = e.Current;

                            onNext(item);
                        }
                        catch (OperationCanceledException)
                        {
                            throw;
                        }
                        catch (Exception ex) when (onError != null)
                        {
                            onError(ex);
                            throw;
                        }

                        yield return item;
                    }

                    onCompleted?.Invoke();
                }
                finally
                {
                    await e.DisposeAsync();
                }
            }
#else
            return new DoAsyncIterator<TSource>(source, onNext, onError, onCompleted);
#endif
        }

        private static IAsyncEnumerable<TSource> DoCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, Task> onNext, Func<Exception, Task> onError, Func<Task> onCompleted)
        {
#if USE_ASYNC_ITERATOR
            return AsyncEnumerable.Create(Core);

            async IAsyncEnumerator<TSource> Core(CancellationToken cancellationToken)
            {
                var e = source.GetAsyncEnumerator(cancellationToken).ConfigureAwait(false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    while (true)
                    {
                        TSource item;

                        try
                        {
                            if (!await e.MoveNextAsync())
                            {
                                break;
                            }

                            item = e.Current;

                            await onNext(item).ConfigureAwait(false);
                        }
                        catch (OperationCanceledException)
                        {
                            throw;
                        }
                        catch (Exception ex) when (onError != null)
                        {
                            await onError(ex).ConfigureAwait(false);
                            throw;
                        }

                        yield return item;
                    }

                    if (onCompleted != null)
                    {
                        await onCompleted().ConfigureAwait(false);
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }
            }
#else
            return new DoAsyncIteratorWithTask<TSource>(source, onNext, onError, onCompleted);
#endif
        }

#if !NO_DEEP_CANCELLATION
        private static IAsyncEnumerable<TSource> DoCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, Task> onNext, Func<Exception, CancellationToken, Task> onError, Func<CancellationToken, Task> onCompleted)
        {
#if USE_ASYNC_ITERATOR
            return AsyncEnumerable.Create(Core);

            async IAsyncEnumerator<TSource> Core(CancellationToken cancellationToken)
            {
                var e = source.GetAsyncEnumerator(cancellationToken).ConfigureAwait(false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    while (true)
                    {
                        TSource item;

                        try
                        {
                            if (!await e.MoveNextAsync())
                            {
                                break;
                            }

                            item = e.Current;

                            await onNext(item, cancellationToken).ConfigureAwait(false);
                        }
                        catch (OperationCanceledException)
                        {
                            throw;
                        }
                        catch (Exception ex) when (onError != null)
                        {
                            await onError(ex, cancellationToken).ConfigureAwait(false);
                            throw;
                        }

                        yield return item;
                    }

                    if (onCompleted != null)
                    {
                        await onCompleted(cancellationToken).ConfigureAwait(false);
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }
            }
#else
            return new DoAsyncIteratorWithTaskAndCancellation<TSource>(source, onNext, onError, onCompleted);
#endif
        }
#endif

#if !USE_ASYNC_ITERATOR
        private sealed class DoAsyncIterator<TSource> : AsyncIterator<TSource>
        {
            private readonly Action _onCompleted;
            private readonly Action<Exception> _onError;
            private readonly Action<TSource> _onNext;
            private readonly IAsyncEnumerable<TSource> _source;

            private IAsyncEnumerator<TSource> _enumerator;

            public DoAsyncIterator(IAsyncEnumerable<TSource> source, Action<TSource> onNext, Action<Exception> onError, Action onCompleted)
            {
                Debug.Assert(source != null);
                Debug.Assert(onNext != null);

                _source = source;
                _onNext = onNext;
                _onError = onError;
                _onCompleted = onCompleted;
            }

            public override AsyncIteratorBase<TSource> Clone()
            {
                return new DoAsyncIterator<TSource>(_source, _onNext, _onError, _onCompleted);
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
                        _state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        try
                        {
                            if (await _enumerator.MoveNextAsync().ConfigureAwait(false))
                            {
                                _current = _enumerator.Current;
                                _onNext(_current);

                                return true;
                            }
                        }
                        catch (OperationCanceledException)
                        {
                            throw;
                        }
                        catch (Exception ex) when (_onError != null)
                        {
                            _onError(ex);
                            throw;
                        }

                        _onCompleted?.Invoke();

                        await DisposeAsync().ConfigureAwait(false);
                        break;
                }

                return false;
            }
        }

        private sealed class DoAsyncIteratorWithTask<TSource> : AsyncIterator<TSource>
        {
            private readonly Func<Task> _onCompleted;
            private readonly Func<Exception, Task> _onError;
            private readonly Func<TSource, Task> _onNext;
            private readonly IAsyncEnumerable<TSource> _source;

            private IAsyncEnumerator<TSource> _enumerator;

            public DoAsyncIteratorWithTask(IAsyncEnumerable<TSource> source, Func<TSource, Task> onNext, Func<Exception, Task> onError, Func<Task> onCompleted)
            {
                Debug.Assert(source != null);
                Debug.Assert(onNext != null);

                _source = source;
                _onNext = onNext;
                _onError = onError;
                _onCompleted = onCompleted;
            }

            public override AsyncIteratorBase<TSource> Clone()
            {
                return new DoAsyncIteratorWithTask<TSource>(_source, _onNext, _onError, _onCompleted);
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
                        _state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        try
                        {
                            if (await _enumerator.MoveNextAsync().ConfigureAwait(false))
                            {
                                _current = _enumerator.Current;
                                await _onNext(_current).ConfigureAwait(false);

                                return true;
                            }
                        }
                        catch (OperationCanceledException)
                        {
                            throw;
                        }
                        catch (Exception ex) when (_onError != null)
                        {
                            await _onError(ex).ConfigureAwait(false);
                            throw;
                        }

                        if (_onCompleted != null)
                        {
                            await _onCompleted().ConfigureAwait(false);
                        }

                        await DisposeAsync().ConfigureAwait(false);
                        break;
                }

                return false;
            }
        }

#if !NO_DEEP_CANCELLATION
        private sealed class DoAsyncIteratorWithTaskAndCancellation<TSource> : AsyncIterator<TSource>
        {
            private readonly Func<CancellationToken, Task> _onCompleted;
            private readonly Func<Exception, CancellationToken, Task> _onError;
            private readonly Func<TSource, CancellationToken, Task> _onNext;
            private readonly IAsyncEnumerable<TSource> _source;

            private IAsyncEnumerator<TSource> _enumerator;

            public DoAsyncIteratorWithTaskAndCancellation(IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, Task> onNext, Func<Exception, CancellationToken, Task> onError, Func<CancellationToken, Task> onCompleted)
            {
                Debug.Assert(source != null);
                Debug.Assert(onNext != null);

                _source = source;
                _onNext = onNext;
                _onError = onError;
                _onCompleted = onCompleted;
            }

            public override AsyncIteratorBase<TSource> Clone()
            {
                return new DoAsyncIteratorWithTaskAndCancellation<TSource>(_source, _onNext, _onError, _onCompleted);
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
                        _state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        try
                        {
                            if (await _enumerator.MoveNextAsync().ConfigureAwait(false))
                            {
                                _current = _enumerator.Current;
                                await _onNext(_current, _cancellationToken).ConfigureAwait(false);

                                return true;
                            }
                        }
                        catch (OperationCanceledException)
                        {
                            throw;
                        }
                        catch (Exception ex) when (_onError != null)
                        {
                            await _onError(ex, _cancellationToken).ConfigureAwait(false);
                            throw;
                        }

                        if (_onCompleted != null)
                        {
                            await _onCompleted(_cancellationToken).ConfigureAwait(false);
                        }

                        await DisposeAsync().ConfigureAwait(false);
                        break;
                }

                return false;
            }
        }
#endif
#endif
    }
}

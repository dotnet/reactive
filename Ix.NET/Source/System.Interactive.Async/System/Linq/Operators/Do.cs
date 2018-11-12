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
            return new DoAsyncIterator<TSource>(source, onNext, onError, onCompleted);
        }

        private static IAsyncEnumerable<TSource> DoCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, Task> onNext, Func<Exception, Task> onError, Func<Task> onCompleted)
        {
            return new DoAsyncIteratorWithTask<TSource>(source, onNext, onError, onCompleted);
        }

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

            public override AsyncIterator<TSource> Clone()
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
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        _enumerator = _source.GetAsyncEnumerator(cancellationToken);
                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        try
                        {
                            if (await _enumerator.MoveNextAsync().ConfigureAwait(false))
                            {
                                current = _enumerator.Current;
                                _onNext(current);

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

            public override AsyncIterator<TSource> Clone()
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
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        _enumerator = _source.GetAsyncEnumerator(cancellationToken);
                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        try
                        {
                            if (await _enumerator.MoveNextAsync().ConfigureAwait(false))
                            {
                                current = _enumerator.Current;
                                await _onNext(current).ConfigureAwait(false);

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
    }
}

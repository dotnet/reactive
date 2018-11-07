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
        public static IAsyncEnumerable<T> CreateEnumerable<T>(Func<IAsyncEnumerator<T>> getEnumerator)
        {
            if (getEnumerator == null)
                throw new ArgumentNullException(nameof(getEnumerator));

            return new AnonymousAsyncEnumerable<T>(_ => getEnumerator());
        }

        public static IAsyncEnumerable<T> CreateEnumerable<T>(Func<CancellationToken, IAsyncEnumerator<T>> getEnumerator)
        {
            if (getEnumerator == null)
                throw new ArgumentNullException(nameof(getEnumerator));

            return new AnonymousAsyncEnumerable<T>(getEnumerator);
        }

        public static IAsyncEnumerable<T> CreateEnumerable<T>(Func<Task<IAsyncEnumerator<T>>> getEnumerator)
        {
            if (getEnumerator == null)
                throw new ArgumentNullException(nameof(getEnumerator));

            return new AnonymousAsyncEnumerableWithTask<T>(_ => getEnumerator());
        }

        public static IAsyncEnumerable<T> CreateEnumerable<T>(Func<CancellationToken, Task<IAsyncEnumerator<T>>> getEnumerator)
        {
            if (getEnumerator == null)
                throw new ArgumentNullException(nameof(getEnumerator));

            return new AnonymousAsyncEnumerableWithTask<T>(getEnumerator);
        }

        public static IAsyncEnumerator<T> CreateEnumerator<T>(Func<ValueTask<bool>> moveNext, Func<T> current, Func<ValueTask> dispose)
        {
            return AsyncEnumerator.Create(moveNext, current, dispose);
        }

        private static IAsyncEnumerator<T> CreateEnumerator<T>(Func<TaskCompletionSource<bool>, ValueTask<bool>> moveNext, Func<T> current, Func<ValueTask> dispose)
        {
            return AsyncEnumerator.Create(moveNext, current, dispose);
        }

        private sealed class AnonymousAsyncEnumerable<T> : IAsyncEnumerable<T>
        {
            private readonly Func<CancellationToken, IAsyncEnumerator<T>> _getEnumerator;

            public AnonymousAsyncEnumerable(Func<CancellationToken, IAsyncEnumerator<T>> getEnumerator)
            {
                Debug.Assert(getEnumerator != null);

                _getEnumerator = getEnumerator;
            }

            public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken) => _getEnumerator(cancellationToken);
        }

        private sealed class AnonymousAsyncEnumerableWithTask<T> : IAsyncEnumerable<T>
        {
            private readonly Func<CancellationToken, Task<IAsyncEnumerator<T>>> _getEnumerator;

            public AnonymousAsyncEnumerableWithTask(Func<CancellationToken, Task<IAsyncEnumerator<T>>> getEnumerator)
            {
                Debug.Assert(getEnumerator != null);

                _getEnumerator = getEnumerator;
            }

            public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken) => new Enumerator(_getEnumerator, cancellationToken);

            private sealed class Enumerator : IAsyncEnumerator<T>
            {
                private Func<CancellationToken, Task<IAsyncEnumerator<T>>> _getEnumerator;
                private readonly CancellationToken _cancellationToken;
                private IAsyncEnumerator<T> _enumerator;

                public Enumerator(Func<CancellationToken, Task<IAsyncEnumerator<T>>> getEnumerator, CancellationToken cancellationToken)
                {
                    Debug.Assert(getEnumerator != null);

                    _getEnumerator = getEnumerator;
                    _cancellationToken = cancellationToken;
                }

                public T Current
                {
                    get
                    {
                        if (_enumerator == null)
                            throw new InvalidOperationException();

                        return _enumerator.Current;
                    }
                }

                public async ValueTask DisposeAsync()
                {
                    var old = Interlocked.Exchange(ref _enumerator, DisposedEnumerator.Instance);

                    if (_enumerator != null)
                    {
                        await _enumerator.DisposeAsync().ConfigureAwait(false);
                    }
                }

                public ValueTask<bool> MoveNextAsync()
                {
                    if (_enumerator == null)
                    {
                        return InitAndMoveNextAsync();
                    }

                    return _enumerator.MoveNextAsync();
                }

                private async ValueTask<bool> InitAndMoveNextAsync()
                {
                    try
                    {
                        _enumerator = await _getEnumerator(_cancellationToken).ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        _enumerator = Throw<T>(ex).GetAsyncEnumerator(_cancellationToken);
                        throw;
                    }
                    finally
                    {
                        _getEnumerator = null;
                    }

                    return await _enumerator.MoveNextAsync().ConfigureAwait(false);
                }

                private sealed class DisposedEnumerator : IAsyncEnumerator<T>
                {
                    public static readonly DisposedEnumerator Instance = new DisposedEnumerator();

                    public T Current => throw new ObjectDisposedException("this");

                    public ValueTask DisposeAsync() => TaskExt.CompletedTask;

                    public ValueTask<bool> MoveNextAsync() => throw new ObjectDisposedException("this");
                }
            }
        }
    }
}

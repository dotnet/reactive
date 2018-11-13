// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using static System.Linq.AsyncEnumerable;

namespace System.Linq
{
    public static partial class AsyncEnumerableEx
    {
        public static IAsyncEnumerable<TSource> Defer<TSource>(Func<IAsyncEnumerable<TSource>> factory)
        {
            if (factory == null)
                throw Error.ArgumentNull(nameof(factory));

            return CreateEnumerable(ct => factory().GetAsyncEnumerator(ct));
        }

        public static IAsyncEnumerable<TSource> Defer<TSource>(Func<Task<IAsyncEnumerable<TSource>>> factory)
        {
            if (factory == null)
                throw Error.ArgumentNull(nameof(factory));

            return new AnonymousAsyncEnumerableWithTask<TSource>(async ct => (await factory().ConfigureAwait(false)).GetAsyncEnumerator(ct));
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

                    public ValueTask DisposeAsync() => default;

                    public ValueTask<bool> MoveNextAsync() => throw new ObjectDisposedException("this");
                }
            }
        }
    }
}

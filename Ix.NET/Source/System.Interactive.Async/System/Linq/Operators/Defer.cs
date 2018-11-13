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

            return new DeferIterator<TSource>(factory);
        }

        public static IAsyncEnumerable<TSource> Defer<TSource>(Func<Task<IAsyncEnumerable<TSource>>> factory)
        {
            if (factory == null)
                throw Error.ArgumentNull(nameof(factory));

            return new AsyncDeferIterator<TSource>(factory);
        }

        private sealed class DeferIterator<T> : AsyncIteratorBase<T>
        {
            private readonly Func<IAsyncEnumerable<T>> _factory;
            private IAsyncEnumerator<T> _enumerator;

            public DeferIterator(Func<IAsyncEnumerable<T>> factory)
            {
                Debug.Assert(factory != null);

                _factory = factory;
            }

            public override T Current => _enumerator == null ? default : _enumerator.Current;

            public override AsyncIteratorBase<T> Clone()
            {
                return new DeferIterator<T>(_factory);
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

            protected override ValueTask<bool> MoveNextCore()
            {
                if (_enumerator == null)
                {
                    return InitializeAndMoveNextAsync();
                }

                return _enumerator.MoveNextAsync();
            }

            private async ValueTask<bool> InitializeAndMoveNextAsync()
            {
                // NB: Using an async method to ensure any exception is reported via the task.

                try
                {
                    _enumerator = _factory().GetAsyncEnumerator(_cancellationToken);
                }
                catch (Exception ex)
                {
                    _enumerator = Throw<T>(ex).GetAsyncEnumerator(_cancellationToken);
                    throw;
                }

                return await _enumerator.MoveNextAsync().ConfigureAwait(false);
            }
        }

        private sealed class AsyncDeferIterator<T> : AsyncIteratorBase<T>
        {
            private readonly Func<Task<IAsyncEnumerable<T>>> _factory;
            private IAsyncEnumerator<T> _enumerator;

            public AsyncDeferIterator(Func< Task<IAsyncEnumerable<T>>> factory)
            {
                Debug.Assert(factory != null);

                _factory = factory;
            }

            public override T Current => _enumerator == null ? default : _enumerator.Current;

            public override AsyncIteratorBase<T> Clone()
            {
                return new AsyncDeferIterator<T>(_factory);
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

            protected override ValueTask<bool> MoveNextCore()
            {
                if (_enumerator == null)
                {
                    return InitializeAndMoveNextAsync();
                }

                return _enumerator.MoveNextAsync();
            }

            private async ValueTask<bool> InitializeAndMoveNextAsync()
            {
                try
                {
                    _enumerator = (await _factory().ConfigureAwait(false)).GetAsyncEnumerator(_cancellationToken);
                }
                catch (Exception ex)
                {
                    _enumerator = Throw<T>(ex).GetAsyncEnumerator(_cancellationToken);
                    throw;
                }

                return await _enumerator.MoveNextAsync().ConfigureAwait(false);
            }
        }
    }
}

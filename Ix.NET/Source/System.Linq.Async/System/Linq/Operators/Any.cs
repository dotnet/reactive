// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        public static ValueTask<bool> AnyAsync<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return Core(source, cancellationToken);

            static async ValueTask<bool> Core(IAsyncEnumerable<TSource> _source, CancellationToken _cancellationToken)
            {
                var e = _source.GetConfiguredAsyncEnumerator(_cancellationToken, false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    return await e.MoveNextAsync();
                }
                finally
                {
                    await e.DisposeAsync();
                }
            }
        }

        public static ValueTask<bool> AnyAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (predicate == null)
                throw Error.ArgumentNull(nameof(predicate));

            return Core(source, predicate, cancellationToken);

            static async ValueTask<bool> Core(IAsyncEnumerable<TSource> _source, Func<TSource, bool> _predicate, CancellationToken _cancellationToken)
            {
                await foreach (var item in _source.WithCancellation(_cancellationToken).ConfigureAwait(false))
                {
                    if (_predicate(item))
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public static ValueTask<bool> AnyAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<bool>> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (predicate == null)
                throw Error.ArgumentNull(nameof(predicate));

            return Core(source, predicate, cancellationToken);

            static async ValueTask<bool> Core(IAsyncEnumerable<TSource> _source, Func<TSource, ValueTask<bool>> _predicate, CancellationToken _cancellationToken)
            {
                await foreach (var item in _source.WithCancellation(_cancellationToken).ConfigureAwait(false))
                {
                    if (await _predicate(item).ConfigureAwait(false))
                    {
                        return true;
                    }
                }

                return false;
            }
        }

#if !NO_DEEP_CANCELLATION
        public static ValueTask<bool> AnyAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<bool>> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (predicate == null)
                throw Error.ArgumentNull(nameof(predicate));

            return Core(source, predicate, cancellationToken);

            static async ValueTask<bool> Core(IAsyncEnumerable<TSource> _source, Func<TSource, CancellationToken, ValueTask<bool>> _predicate, CancellationToken _cancellationToken)
            {
                await foreach (var item in _source.WithCancellation(_cancellationToken).ConfigureAwait(false))
                {
                    if (await _predicate(item, _cancellationToken).ConfigureAwait(false))
                    {
                        return true;
                    }
                }

                return false;
            }
        }
#endif
    }
}

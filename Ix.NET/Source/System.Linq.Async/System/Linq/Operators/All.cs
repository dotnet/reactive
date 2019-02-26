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
        public static ValueTask<bool> AllAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (predicate == null)
                throw Error.ArgumentNull(nameof(predicate));

            return Core(source, predicate, cancellationToken);

            static async ValueTask<bool> Core(IAsyncEnumerable<TSource> _source, Func<TSource, bool> _predicate, CancellationToken _cancellationToken)
            {
                await foreach (var item in AsyncEnumerableExtensions.WithCancellation(_source, _cancellationToken).ConfigureAwait(false))
                {
                    if (!_predicate(item))
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public static ValueTask<bool> AllAwaitAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<bool>> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (predicate == null)
                throw Error.ArgumentNull(nameof(predicate));

            return Core(source, predicate, cancellationToken);

            static async ValueTask<bool> Core(IAsyncEnumerable<TSource> _source, Func<TSource, ValueTask<bool>> _predicate, CancellationToken _cancellationToken)
            {
                await foreach (var item in AsyncEnumerableExtensions.WithCancellation(_source, _cancellationToken).ConfigureAwait(false))
                {
                    if (!await _predicate(item).ConfigureAwait(false))
                    {
                        return false;
                    }
                }

                return true;
            }
        }

#if !NO_DEEP_CANCELLATION
        public static ValueTask<bool> AllAwaitWithCancellationAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<bool>> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (predicate == null)
                throw Error.ArgumentNull(nameof(predicate));

            return Core(source, predicate, cancellationToken);

            static async ValueTask<bool> Core(IAsyncEnumerable<TSource> _source, Func<TSource, CancellationToken, ValueTask<bool>> _predicate, CancellationToken _cancellationToken)
            {
                await foreach (var item in AsyncEnumerableExtensions.WithCancellation(_source, _cancellationToken).ConfigureAwait(false))
                {
                    if (!await _predicate(item, _cancellationToken).ConfigureAwait(false))
                    {
                        return false;
                    }
                }

                return true;
            }
        }
#endif
    }
}

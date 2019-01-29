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
        public static Task<bool> ContainsAsync<TSource>(this IAsyncEnumerable<TSource> source, TSource value, CancellationToken cancellationToken = default) =>
            source is ICollection<TSource> collection ? Task.FromResult(collection.Contains(value)) :
            ContainsAsync(source, value, comparer: null, cancellationToken);

        public static Task<bool> ContainsAsync<TSource>(this IAsyncEnumerable<TSource> source, TSource value, IEqualityComparer<TSource> comparer, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            //
            // See https://github.com/dotnet/corefx/pull/25097 for the optimization here.
            //
            if (comparer == null)
            {
                return Core(source, value, cancellationToken);

                static async Task<bool> Core(IAsyncEnumerable<TSource> _source, TSource _value, CancellationToken _cancellationToken)
                {
#if USE_AWAIT_FOREACH
                    await foreach (TSource item in _source.WithCancellation(_cancellationToken).ConfigureAwait(false))
                    {
                        if (EqualityComparer<TSource>.Default.Equals(item, _value))
                        {
                            return true;
                        }
                    }
#else
                    var e = _source.GetAsyncEnumerator(_cancellationToken);

                    try
                    {
                        while (await e.MoveNextAsync().ConfigureAwait(false))
                        {
                            if (EqualityComparer<TSource>.Default.Equals(e.Current, _value))
                            {
                                return true;
                            }
                        }
                    }
                    finally
                    {
                        await e.DisposeAsync().ConfigureAwait(false);
                    }
#endif

                    return false;
                }
            }
            else
            {
                return Core(source, value, comparer, cancellationToken);

                static async Task<bool> Core(IAsyncEnumerable<TSource> _source, TSource _value, IEqualityComparer<TSource> _comparer, CancellationToken _cancellationToken)
                {
#if USE_AWAIT_FOREACH
                    await foreach (TSource item in _source.WithCancellation(_cancellationToken).ConfigureAwait(false))
                    {
                        if (_comparer.Equals(item, _value))
                        {
                            return true;
                        }
                    }
#else
                    var e = _source.GetAsyncEnumerator(_cancellationToken);

                    try
                    {
                        while (await e.MoveNextAsync().ConfigureAwait(false))
                        {
                            if (_comparer.Equals(e.Current, _value))
                            {
                                return true;
                            }
                        }
                    }
                    finally
                    {
                        await e.DisposeAsync().ConfigureAwait(false);
                    }
#endif

                    return false;
                }
            }
        }
    }
}

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
        public static ValueTask<bool> ContainsAsync<TSource>(this IAsyncEnumerable<TSource> source, TSource value, CancellationToken cancellationToken = default) =>
            source is ICollection<TSource> collection ? new ValueTask<bool>(collection.Contains(value)) :
            ContainsAsync(source, value, comparer: null, cancellationToken);

        public static ValueTask<bool> ContainsAsync<TSource>(this IAsyncEnumerable<TSource> source, TSource value, IEqualityComparer<TSource> comparer, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            //
            // See https://github.com/dotnet/corefx/pull/25097 for the optimization here.
            //
            if (comparer == null)
            {
                return Core(source, value, cancellationToken);

                static async ValueTask<bool> Core(IAsyncEnumerable<TSource> _source, TSource _value, CancellationToken _cancellationToken)
                {
                    await foreach (var item in AsyncEnumerableExtensions.WithCancellation(_source, _cancellationToken).ConfigureAwait(false))
                    {
                        if (EqualityComparer<TSource>.Default.Equals(item, _value))
                        {
                            return true;
                        }
                    }

                    return false;
                }
            }
            else
            {
                return Core(source, value, comparer, cancellationToken);

                static async ValueTask<bool> Core(IAsyncEnumerable<TSource> _source, TSource _value, IEqualityComparer<TSource> _comparer, CancellationToken _cancellationToken)
                {
                    await foreach (var item in AsyncEnumerableExtensions.WithCancellation(_source, _cancellationToken).ConfigureAwait(false))
                    {
                        if (_comparer.Equals(item, _value))
                        {
                            return true;
                        }
                    }

                    return false;
                }
            }
        }
    }
}

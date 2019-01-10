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
        public static Task<bool> ContainsAsync<TSource>(this IAsyncEnumerable<TSource> source, TSource value, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            if (source is ICollection<TSource> collection)
            {
                return Task.FromResult(collection.Contains(value));
            }

            return ContainsCore(source, value, comparer: null, cancellationToken);
        }

        public static Task<bool> ContainsAsync<TSource>(this IAsyncEnumerable<TSource> source, TSource value, IEqualityComparer<TSource> comparer, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return ContainsCore(source, value, comparer, cancellationToken);
        }

        private static async Task<bool> ContainsCore<TSource>(IAsyncEnumerable<TSource> source, TSource value, IEqualityComparer<TSource> comparer, CancellationToken cancellationToken)
        {
#if CSHARP8 && AETOR_HAS_CT // CS0656 Missing compiler required member 'System.Collections.Generic.IAsyncEnumerable`1.GetAsyncEnumerator'
            //
            // See https://github.com/dotnet/corefx/pull/25097 for the optimization here.
            //
            if (comparer == null)
            {
                await foreach (TSource item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    if (EqualityComparer<TSource>.Default.Equals(item, value))
                    {
                        return true;
                    }
                }
            }
            else
            {
                await foreach (TSource item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    if (comparer.Equals(item, value))
                    {
                        return true;
                    }
                }
            }
#else
            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                //
                // See https://github.com/dotnet/corefx/pull/25097 for the optimization here.
                //
                if (comparer == null)
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        if (EqualityComparer<TSource>.Default.Equals(e.Current, value))
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        if (comparer.Equals(e.Current, value))
                        {
                            return true;
                        }
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

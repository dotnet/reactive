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
        public static Task<bool> SequenceEqualAsync<TSource>(this IAsyncEnumerable<TSource> first, IAsyncEnumerable<TSource> second)
        {
            if (first == null)
                throw Error.ArgumentNull(nameof(first));
            if (second == null)
                throw Error.ArgumentNull(nameof(second));

            return SequenceEqualCore(first, second, comparer: null, CancellationToken.None);
        }

        public static Task<bool> SequenceEqualAsync<TSource>(this IAsyncEnumerable<TSource> first, IAsyncEnumerable<TSource> second, CancellationToken cancellationToken)
        {
            if (first == null)
                throw Error.ArgumentNull(nameof(first));
            if (second == null)
                throw Error.ArgumentNull(nameof(second));

            return SequenceEqualCore(first, second, comparer: null, cancellationToken);
        }

        public static Task<bool> SequenceEqualAsync<TSource>(this IAsyncEnumerable<TSource> first, IAsyncEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
        {
            if (first == null)
                throw Error.ArgumentNull(nameof(first));
            if (second == null)
                throw Error.ArgumentNull(nameof(second));

            return SequenceEqualCore(first, second, comparer, CancellationToken.None);
        }

        public static Task<bool> SequenceEqualAsync<TSource>(this IAsyncEnumerable<TSource> first, IAsyncEnumerable<TSource> second, IEqualityComparer<TSource> comparer, CancellationToken cancellationToken)
        {
            if (first == null)
                throw Error.ArgumentNull(nameof(first));
            if (second == null)
                throw Error.ArgumentNull(nameof(second));

            return SequenceEqualCore(first, second, comparer, cancellationToken);
        }

        private static Task<bool> SequenceEqualCore<TSource>(IAsyncEnumerable<TSource> first, IAsyncEnumerable<TSource> second, IEqualityComparer<TSource> comparer, CancellationToken cancellationToken)
        {
            if (comparer == null)
            {
                comparer = EqualityComparer<TSource>.Default;
            }

            if (first is ICollection<TSource> firstCol && second is ICollection<TSource> secondCol)
            {
                if (firstCol.Count != secondCol.Count)
                {
                    return Task.FromResult(false);
                }

                if (firstCol is IList<TSource> firstList && secondCol is IList<TSource> secondList)
                {
                    int count = firstCol.Count;

                    for (int i = 0; i < count; i++)
                    {
                        if (!comparer.Equals(firstList[i], secondList[i]))
                        {
                            return Task.FromResult(false);
                        }
                    }

                    return Task.FromResult(true);
                }
            }

            return Core();

            async Task<bool> Core()
            {
                var e1 = first.GetAsyncEnumerator(cancellationToken);

                try
                {
                    var e2 = second.GetAsyncEnumerator(cancellationToken);

                    try
                    {
                        while (await e1.MoveNextAsync().ConfigureAwait(false))
                        {
                            if (!(await e2.MoveNextAsync().ConfigureAwait(false) && comparer.Equals(e1.Current, e2.Current)))
                            {
                                return false;
                            }
                        }

                        return !await e2.MoveNextAsync().ConfigureAwait(false);
                    }
                    finally
                    {
                        await e2.DisposeAsync().ConfigureAwait(false);
                    }
                }
                finally
                {
                    await e1.DisposeAsync().ConfigureAwait(false);
                }
            }
        }
    }
}

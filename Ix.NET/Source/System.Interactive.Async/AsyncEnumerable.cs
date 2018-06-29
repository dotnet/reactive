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
        public static IAsyncEnumerable<TSource> AsAsyncEnumerable<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Select(x => x);
        }

        public static IAsyncEnumerable<TValue> Empty<TValue>()
        {
            return CreateEnumerable(
                () => CreateEnumerator<TValue>(
                    ct => TaskExt.False, null, null)
            );
        }

        public static Task<bool> IsEmpty<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.IsEmpty(CancellationToken.None);
        }

        public static Task<bool> IsEmpty<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return IsEmpty_(source, cancellationToken);
        }

        public static IAsyncEnumerable<TValue> Never<TValue>()
        {
            return CreateEnumerable(
                () => CreateEnumerator<TValue>(
                    (ct, tcs) => tcs.Task,
                    null,
                    null)
            );
        }


        public static IAsyncEnumerable<TValue> Return<TValue>(TValue value)
        {
            return new[] { value }.ToAsyncEnumerable();
        }

        public static IAsyncEnumerable<TValue> Throw<TValue>(Exception exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            return CreateEnumerable(
                () => CreateEnumerator<TValue>(
                    ct =>
                    {
                        var tcs = new TaskCompletionSource<bool>();
                        tcs.TrySetException(exception);
                        return tcs.Task;
                    },
                    null,
                    null)
            );
        }

        private static async Task<bool> IsEmpty_<TSource>(IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
        {
            return !await source.Any(cancellationToken)
                                .ConfigureAwait(false);
        }
    }
}
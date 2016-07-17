// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        public static IAsyncEnumerable<TSource> ToAsyncEnumerable<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return CreateEnumerable(() =>
            {
                var e = source.GetEnumerator();

                return CreateEnumerator(
                    ct => Task.Run(() =>
                    {
                        var res = false;
                        try
                        {
                            res = e.MoveNext();
                        }
                        finally
                        {
                            if (!res)
                                e.Dispose();
                        }
                        return res;
                    }, ct),
                    () => e.Current,
                    () => e.Dispose()
                );
            });
        }

        public static IEnumerable<TSource> ToEnumerable<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return ToEnumerable_(source);
        }

        private static IEnumerable<TSource> ToEnumerable_<TSource>(IAsyncEnumerable<TSource> source)
        {
            using (var e = source.GetEnumerator())
            {
                while (true)
                {
                    if (!e.MoveNext(CancellationToken.None).Result)
                        break;
                    var c = e.Current;
                    yield return c;
                }
            }
        }

        public static IAsyncEnumerable<TSource> ToAsyncEnumerable<TSource>(this Task<TSource> task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));
            
            return CreateEnumerable(() =>
            {
                var called = 0;

                var value = default(TSource);
                return CreateEnumerator(
                    async ct =>
                    {
                        if (Interlocked.CompareExchange(ref called, 1, 0) == 0)
                        {
                            value = await task.ConfigureAwait(false);
                            return true;
                        }
                        return false;
                    },
                    () => value,
                    () => { });
            });
        }


    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        public static IEnumerable<TSource> ToEnumerable<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return ToEnumerableCore(source);
        }

        private static IEnumerable<TSource> ToEnumerableCore<TSource>(IAsyncEnumerable<TSource> source)
        {
            var e = source.GetAsyncEnumerator();

            try
            {
                while (true)
                {
                    if (!e.MoveNextAsync().Result)
                        break;

                    var c = e.Current;

                    yield return c;
                }
            }
            finally
            {
                // Wait
                e.DisposeAsync().GetAwaiter().GetResult();
            }
        }
    }
}

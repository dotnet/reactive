// // Licensed to the .NET Foundation under one or more agreements.
// // The .NET Foundation licenses this file to you under the Apache 2.0 License.
// // See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        public static IAsyncEnumerable<TSource> Except<TSource>(this IAsyncEnumerable<TSource> first, IAsyncEnumerable<TSource> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));

            return first.Except(second, EqualityComparer<TSource>.Default);
        }

        public static IAsyncEnumerable<TSource> Except<TSource>(this IAsyncEnumerable<TSource> first, IAsyncEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return Create(() =>
                          {
                              var e = first.GetEnumerator();

                              var cts = new CancellationTokenDisposable();
                              var d = Disposable.Create(cts, e);

                              var mapTask = default(Task<Dictionary<TSource, TSource>>);
                              var getMapTask = new Func<CancellationToken, Task<Dictionary<TSource, TSource>>>(
                                  ct => mapTask ?? (mapTask = second.ToDictionary(x => x, comparer, ct)));

                              var f = default(Func<CancellationToken, Task<bool>>);
                              f = async ct =>
                                  {
                                      if (await e.MoveNext(ct)
                                                 .Zip(getMapTask(ct), (b, _) => b)
                                                 .ConfigureAwait(false))
                                      {
                                          if (!mapTask.Result.ContainsKey(e.Current))
                                              return true;
                                          return await f(ct)
                                                     .ConfigureAwait(false);
                                      }
                                      return false;
                                  };

                              return Create(
                                  f,
                                  () => e.Current,
                                  d.Dispose,
                                  e
                              );
                          });
        }
    }
}
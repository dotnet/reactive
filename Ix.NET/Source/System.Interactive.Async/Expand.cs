// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        public static IAsyncEnumerable<TSource> Expand<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, IAsyncEnumerable<TSource>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create(() =>
                          {
                              var e = default(IAsyncEnumerator<TSource>);

                              var cts = new CancellationTokenDisposable();
                              var a = new AssignableDisposable();
                              var d = Disposable.Create(cts, a);

                              var queue = new Queue<IAsyncEnumerable<TSource>>();
                              queue.Enqueue(source);

                              var current = default(TSource);

                              var f = default(Func<CancellationToken, Task<bool>>);
                              f = async ct =>
                                  {
                                      if (e == null)
                                      {
                                          if (queue.Count > 0)
                                          {
                                              var src = queue.Dequeue();

                                              e = src.GetEnumerator();

                                              a.Disposable = e;
                                              return await f(ct)
                                                         .ConfigureAwait(false);
                                          }
                                          return false;
                                      }
                                      if (await e.MoveNext(ct)
                                                 .ConfigureAwait(false))
                                      {
                                          var item = e.Current;
                                          var next = selector(item);

                                          queue.Enqueue(next);
                                          current = item;
                                          return true;
                                      }
                                      e = null;
                                      return await f(ct)
                                                 .ConfigureAwait(false);
                                  };

                              return Create(
                                  f,
                                  () => current,
                                  d.Dispose,
                                  e
                              );
                          });
        }
    }
}
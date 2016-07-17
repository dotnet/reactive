// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        public static IAsyncEnumerable<TResult> Select<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return CreateEnumerable(() =>
                          {
                              var e = source.GetEnumerator();
                              var current = default(TResult);

                              var cts = new CancellationTokenDisposable();
                              var d = Disposable.Create(cts, e);

                              return CreateEnumerator(
                                  async ct =>
                                  {
                                      if (await e.MoveNext(cts.Token)
                                                 .ConfigureAwait(false))
                                      {
                                          current = selector(e.Current);
                                          return true;
                                      }
                                      return false;
                                  },
                                  () => current,
                                  d.Dispose,
                                  e
                              );
                          });
        }

        public static IAsyncEnumerable<TResult> Select<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, int, TResult> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return CreateEnumerable(() =>
                          {
                              var e = source.GetEnumerator();
                              var current = default(TResult);
                              var index = 0;

                              var cts = new CancellationTokenDisposable();
                              var d = Disposable.Create(cts, e);

                              return CreateEnumerator(
                                  async ct =>
                                  {
                                      if (await e.MoveNext(cts.Token)
                                                 .ConfigureAwait(false))
                                      {
                                          current = selector(e.Current, checked(index++));
                                          return true;
                                      }
                                      return false;
                                  },
                                  () => current,
                                  d.Dispose,
                                  e
                              );
                          });
        }
    }
}
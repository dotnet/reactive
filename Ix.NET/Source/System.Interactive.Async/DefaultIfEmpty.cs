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
        public static IAsyncEnumerable<TSource> DefaultIfEmpty<TSource>(this IAsyncEnumerable<TSource> source, TSource defaultValue)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Create(() =>
                          {
                              var done = false;
                              var hasElements = false;
                              var e = source.GetEnumerator();
                              var current = default(TSource);

                              var cts = new CancellationTokenDisposable();
                              var d = Disposable.Create(cts, e);

                              var f = default(Func<CancellationToken, Task<bool>>);
                              f = async ct =>
                                  {
                                      if (done)
                                          return false;
                                      if (await e.MoveNext(ct)
                                                 .ConfigureAwait(false))
                                      {
                                          hasElements = true;
                                          current = e.Current;
                                          return true;
                                      }
                                      done = true;
                                      if (!hasElements)
                                      {
                                          current = defaultValue;
                                          return true;
                                      }
                                      return false;
                                  };

                              return Create(
                                  f,
                                  () => current,
                                  d.Dispose,
                                  e
                              );
                          });
        }

        public static IAsyncEnumerable<TSource> DefaultIfEmpty<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.DefaultIfEmpty(default(TSource));
        }
    }
}
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
        public static IAsyncEnumerable<TSource> IgnoreElements<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Create(() =>
                          {
                              var e = source.GetEnumerator();

                              var cts = new CancellationTokenDisposable();
                              var d = Disposable.Create(cts, e);

                              var f = default(Func<CancellationToken, Task<bool>>);
                              f = async ct =>
                                  {
                                      if (!await e.MoveNext(ct)
                                                  .ConfigureAwait(false))
                                      {
                                          return false;
                                      }

                                      return await f(ct)
                                                 .ConfigureAwait(false);
                                  };

                              return Create<TSource>(
                                  f,
                                  () => { throw new InvalidOperationException(); },
                                  d.Dispose,
                                  e
                              );
                          });
        }
    }
}
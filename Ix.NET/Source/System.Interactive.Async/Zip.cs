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
        public static IAsyncEnumerable<TResult> Zip<TFirst, TSecond, TResult>(this IAsyncEnumerable<TFirst> first, IAsyncEnumerable<TSecond> second, Func<TFirst, TSecond, TResult> selector)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return CreateEnumerable(() =>
                          {
                              var e1 = first.GetEnumerator();
                              var e2 = second.GetEnumerator();
                              var current = default(TResult);

                              var cts = new CancellationTokenDisposable();
                              var d = Disposable.Create(cts, e1, e2);

                              return CreateEnumerator(
                                  ct => e1.MoveNext(cts.Token)
                                          .Zip(e2.MoveNext(cts.Token), (f, s) =>
                                                                       {
                                                                           var result = f && s;
                                                                           if (result)
                                                                               current = selector(e1.Current, e2.Current);
                                                                           return result;
                                                                       }),
                                  () => current,
                                  d.Dispose
                              );
                          });
        }
    }
}
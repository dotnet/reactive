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
        public static IAsyncEnumerable<TSource> Reverse<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Create(() =>
                          {
                              var e = source.GetEnumerator();
                              var stack = default(Stack<TSource>);

                              var cts = new CancellationTokenDisposable();
                              var d = Disposable.Create(cts, e);

                              return Create(
                                  async ct =>
                                  {
                                      if (stack == null)
                                      {
                                          stack = await Create(() => e)
                                                      .Aggregate(new Stack<TSource>(), (s, x) =>
                                                                                       {
                                                                                           s.Push(x);
                                                                                           return s;
                                                                                       }, cts.Token)
                                                      .ConfigureAwait(false);
                                          return stack.Count > 0;
                                      }
                                      stack.Pop();
                                      return stack.Count > 0;
                                  },
                                  () => stack.Peek(),
                                  d.Dispose,
                                  e
                              );
                          });
        }
    }
}
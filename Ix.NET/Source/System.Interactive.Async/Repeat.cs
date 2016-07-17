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
        public static IAsyncEnumerable<TResult> Repeat<TResult>(TResult element, int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            return Enumerable.Repeat(element, count)
                             .ToAsyncEnumerable();
        }

        public static IAsyncEnumerable<TResult> Repeat<TResult>(TResult element)
        {
            return Create(() =>
                          {
                              return Create(
                                  ct => TaskExt.True,
                                  () => element,
                                  () => { }
                              );
                          });
        }


        public static IAsyncEnumerable<TSource> Repeat<TSource>(this IAsyncEnumerable<TSource> source, int count)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            return Create(() =>
                          {
                              var e = default(IAsyncEnumerator<TSource>);
                              var a = new AssignableDisposable();
                              var n = count;
                              var current = default(TSource);

                              var cts = new CancellationTokenDisposable();
                              var d = Disposable.Create(cts, a);

                              var f = default(Func<CancellationToken, Task<bool>>);
                              f = async ct =>
                                  {
                                      if (e == null)
                                      {
                                          if (n-- == 0)
                                          {
                                              return false;
                                          }

                                          e = source.GetEnumerator();

                                          a.Disposable = e;
                                      }

                                      if (await e.MoveNext(ct)
                                                 .ConfigureAwait(false))
                                      {
                                          current = e.Current;
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

        public static IAsyncEnumerable<TSource> Repeat<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Create(() =>
                          {
                              var e = default(IAsyncEnumerator<TSource>);
                              var a = new AssignableDisposable();
                              var current = default(TSource);

                              var cts = new CancellationTokenDisposable();
                              var d = Disposable.Create(cts, a);

                              var f = default(Func<CancellationToken, Task<bool>>);
                              f = async ct =>
                                  {
                                      if (e == null)
                                      {
                                          e = source.GetEnumerator();

                                          a.Disposable = e;
                                      }

                                      if (await e.MoveNext(ct)
                                                 .ConfigureAwait(false))
                                      {
                                          current = e.Current;
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
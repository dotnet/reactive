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
        public static IAsyncEnumerable<TSource> Concat<TSource>(this IAsyncEnumerable<TSource> first, IAsyncEnumerable<TSource> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));

            return Create(() =>
                          {
                              var switched = false;
                              var e = first.GetEnumerator();

                              var cts = new CancellationTokenDisposable();
                              var a = new AssignableDisposable
                              {
                                  Disposable = e
                              };
                              var d = Disposable.Create(cts, a);

                              var f = default(Func<CancellationToken, Task<bool>>);
                              f = async ct =>
                                  {
                                      if (await e.MoveNext(ct)
                                                 .ConfigureAwait(false))
                                      {
                                          return true;
                                      }
                                      if (switched)
                                      {
                                          return false;
                                      }
                                      switched = true;

                                      e = second.GetEnumerator();
                                      a.Disposable = e;

                                      return await f(ct)
                                                 .ConfigureAwait(false);
                                  };

                              return Create(
                                  f,
                                  () => e.Current,
                                  d.Dispose,
                                  e
                              );
                          });
        }

        public static IAsyncEnumerable<TSource> Concat<TSource>(this IEnumerable<IAsyncEnumerable<TSource>> sources)
        {
            if (sources == null)
                throw new ArgumentNullException(nameof(sources));

            return sources.Concat_();
        }

        public static IAsyncEnumerable<TSource> Concat<TSource>(params IAsyncEnumerable<TSource>[] sources)
        {
            if (sources == null)
                throw new ArgumentNullException(nameof(sources));

            return sources.Concat_();
        }

        private static IAsyncEnumerable<TSource> Concat_<TSource>(this IEnumerable<IAsyncEnumerable<TSource>> sources)
        {
            return Create(() =>
                          {
                              var se = sources.GetEnumerator();
                              var e = default(IAsyncEnumerator<TSource>);

                              var cts = new CancellationTokenDisposable();
                              var a = new AssignableDisposable();
                              var d = Disposable.Create(cts, se, a);

                              var f = default(Func<CancellationToken, Task<bool>>);
                              f = async ct =>
                                  {
                                      if (e == null)
                                      {
                                          var b = false;
                                          b = se.MoveNext();
                                          if (b)
                                              e = se.Current.GetEnumerator();

                                          if (!b)
                                          {
                                              return false;
                                          }

                                          a.Disposable = e;
                                      }

                                      if (await e.MoveNext(ct)
                                                 .ConfigureAwait(false))
                                      {
                                          return true;
                                      }
                                      e.Dispose();
                                      e = null;

                                      return await f(ct)
                                                 .ConfigureAwait(false);
                                  };

                              return Create(
                                  f,
                                  () => e.Current,
                                  d.Dispose,
                                  a
                              );
                          });
        }
    }
}
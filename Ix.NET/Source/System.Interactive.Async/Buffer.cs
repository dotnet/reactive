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
        public static IAsyncEnumerable<IList<TSource>> Buffer<TSource>(this IAsyncEnumerable<TSource> source, int count)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (count <= 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            return source.Buffer_(count, count);
        }

        public static IAsyncEnumerable<IList<TSource>> Buffer<TSource>(this IAsyncEnumerable<TSource> source, int count, int skip)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (count <= 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (skip <= 0)
                throw new ArgumentOutOfRangeException(nameof(skip));

            return source.Buffer_(count, skip);
        }

        private static IAsyncEnumerable<IList<TSource>> Buffer_<TSource>(this IAsyncEnumerable<TSource> source, int count, int skip)
        {
            return Create(() =>
                          {
                              var e = source.GetEnumerator();

                              var cts = new CancellationTokenDisposable();
                              var d = Disposable.Create(cts, e);

                              var buffers = new Queue<IList<TSource>>();

                              var i = 0;

                              var current = default(IList<TSource>);
                              var stopped = false;

                              var f = default(Func<CancellationToken, Task<bool>>);
                              f = async ct =>
                                  {
                                      if (!stopped)
                                      {
                                          if (await e.MoveNext(ct)
                                                     .ConfigureAwait(false))
                                          {
                                              var item = e.Current;

                                              if (i++%skip == 0)
                                                  buffers.Enqueue(new List<TSource>(count));

                                              foreach (var buffer in buffers)
                                                  buffer.Add(item);

                                              if (buffers.Count > 0 && buffers.Peek()
                                                                              .Count == count)
                                              {
                                                  current = buffers.Dequeue();
                                                  return true;
                                              }
                                              return await f(ct)
                                                         .ConfigureAwait(false);
                                          }
                                          stopped = true;
                                          e.Dispose();

                                          return await f(ct)
                                                     .ConfigureAwait(false);
                                      }
                                      if (buffers.Count > 0)
                                      {
                                          current = buffers.Dequeue();
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
    }
}
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
        public static IAsyncEnumerable<TSource> Do<TSource>(this IAsyncEnumerable<TSource> source, Action<TSource> onNext)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (onNext == null)
                throw new ArgumentNullException(nameof(onNext));

            return DoHelper(source, onNext, _ => { }, () => { });
        }

        public static IAsyncEnumerable<TSource> Do<TSource>(this IAsyncEnumerable<TSource> source, Action<TSource> onNext, Action onCompleted)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (onNext == null)
                throw new ArgumentNullException(nameof(onNext));
            if (onCompleted == null)
                throw new ArgumentNullException(nameof(onCompleted));

            return DoHelper(source, onNext, _ => { }, onCompleted);
        }

        public static IAsyncEnumerable<TSource> Do<TSource>(this IAsyncEnumerable<TSource> source, Action<TSource> onNext, Action<Exception> onError)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (onNext == null)
                throw new ArgumentNullException(nameof(onNext));
            if (onError == null)
                throw new ArgumentNullException(nameof(onError));

            return DoHelper(source, onNext, onError, () => { });
        }

        public static IAsyncEnumerable<TSource> Do<TSource>(this IAsyncEnumerable<TSource> source, Action<TSource> onNext, Action<Exception> onError, Action onCompleted)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (onNext == null)
                throw new ArgumentNullException(nameof(onNext));
            if (onError == null)
                throw new ArgumentNullException(nameof(onError));
            if (onCompleted == null)
                throw new ArgumentNullException(nameof(onCompleted));

            return DoHelper(source, onNext, onError, onCompleted);
        }

        public static IAsyncEnumerable<TSource> Do<TSource>(this IAsyncEnumerable<TSource> source, IObserver<TSource> observer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            return DoHelper(source, observer.OnNext, observer.OnError, observer.OnCompleted);
        }

        private static IAsyncEnumerable<TSource> DoHelper<TSource>(this IAsyncEnumerable<TSource> source, Action<TSource> onNext, Action<Exception> onError, Action onCompleted)
        {
            return Create(() =>
                          {
                              var e = source.GetEnumerator();

                              var cts = new CancellationTokenDisposable();
                              var d = Disposable.Create(cts, e);

                              var current = default(TSource);

                              var f = default(Func<CancellationToken, Task<bool>>);
                              f = async ct =>
                                  {
                                      try
                                      {
                                          var result = await e.MoveNext(ct)
                                                              .ConfigureAwait(false);
                                          if (!result)
                                          {
                                              onCompleted();
                                          }
                                          else
                                          {
                                              current = e.Current;
                                              onNext(current);
                                          }
                                          return result;
                                      }
                                      catch (OperationCanceledException)
                                      {
                                          throw;
                                      }
                                      catch (Exception ex)
                                      {
                                          onError(ex);
                                          throw;
                                      }
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
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
        public static IOrderedAsyncEnumerable<TSource> OrderBy<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return new OrderedAsyncEnumerable<TSource, TKey>(
                Create(() =>
                       {
                           var current = default(IEnumerable<TSource>);

                           return Create(
                               async ct =>
                               {
                                   if (current == null)
                                   {
                                       current = await source.ToList(ct)
                                                             .ConfigureAwait(false);
                                       return true;
                                   }
                                   return false;
                               },
                               () => current,
                               () => { }
                           );
                       }),
                keySelector,
                comparer
            );
        }

        public static IOrderedAsyncEnumerable<TSource> OrderBy<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return source.OrderBy(keySelector, Comparer<TKey>.Default);
        }

        public static IOrderedAsyncEnumerable<TSource> OrderByDescending<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return source.OrderBy(keySelector, new ReverseComparer<TKey>(comparer));
        }

        public static IOrderedAsyncEnumerable<TSource> OrderByDescending<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return source.OrderByDescending(keySelector, Comparer<TKey>.Default);
        }

        public static IOrderedAsyncEnumerable<TSource> ThenBy<TSource, TKey>(this IOrderedAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return source.ThenBy(keySelector, Comparer<TKey>.Default);
        }

        public static IOrderedAsyncEnumerable<TSource> ThenBy<TSource, TKey>(this IOrderedAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return source.CreateOrderedEnumerable(keySelector, comparer, false);
        }

        public static IOrderedAsyncEnumerable<TSource> ThenByDescending<TSource, TKey>(this IOrderedAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return source.ThenByDescending(keySelector, Comparer<TKey>.Default);
        }

        public static IOrderedAsyncEnumerable<TSource> ThenByDescending<TSource, TKey>(this IOrderedAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return source.CreateOrderedEnumerable(keySelector, comparer, true);
        }

        private class OrderedAsyncEnumerable<T, K> : IOrderedAsyncEnumerable<T>
        {
            private readonly IComparer<K> comparer;
            private readonly IAsyncEnumerable<IEnumerable<T>> equivalenceClasses;
            private readonly Func<T, K> keySelector;

            public OrderedAsyncEnumerable(IAsyncEnumerable<IEnumerable<T>> equivalenceClasses, Func<T, K> keySelector, IComparer<K> comparer)
            {
                this.equivalenceClasses = equivalenceClasses;
                this.keySelector = keySelector;
                this.comparer = comparer;
            }

            public IOrderedAsyncEnumerable<T> CreateOrderedEnumerable<TKey>(Func<T, TKey> keySelector, IComparer<TKey> comparer, bool descending)
            {
                if (descending)
                    comparer = new ReverseComparer<TKey>(comparer);

                return new OrderedAsyncEnumerable<T, TKey>(Classes(), keySelector, comparer);
            }

            public IAsyncEnumerator<T> GetEnumerator()
            {
                return Classes()
                    .SelectMany(x => x.ToAsyncEnumerable())
                    .GetEnumerator();
            }

            private IAsyncEnumerable<IEnumerable<T>> Classes()
            {
                return Create(() =>
                              {
                                  var e = equivalenceClasses.GetEnumerator();
                                  var list = new List<IEnumerable<T>>();
                                  var e1 = default(IEnumerator<IEnumerable<T>>);

                                  var cts = new CancellationTokenDisposable();
                                  var d1 = new AssignableDisposable();
                                  var d = Disposable.Create(cts, e, d1);

                                  var f = default(Func<CancellationToken, Task<bool>>);

                                  f = async ct =>
                                      {
                                          if (await e.MoveNext(ct)
                                                     .ConfigureAwait(false))
                                          {
                                              list.AddRange(e.Current.OrderBy(keySelector, comparer)
                                                             .GroupUntil(keySelector, x => x, comparer));
                                              return await f(ct)
                                                         .ConfigureAwait(false);
                                          }
                                          e.Dispose();

                                          e1 = list.GetEnumerator();
                                          d1.Disposable = e1;

                                          return e1.MoveNext();
                                      };

                                  return Create(
                                      async ct =>
                                      {
                                          if (e1 != null)
                                          {
                                              return e1.MoveNext();
                                          }
                                          return await f(cts.Token)
                                                     .ConfigureAwait(false);
                                      },
                                      () => e1.Current,
                                      d.Dispose,
                                      e
                                  );
                              });
            }
        }

        private class ReverseComparer<T> : IComparer<T>
        {
            private readonly IComparer<T> comparer;

            public ReverseComparer(IComparer<T> comparer)
            {
                this.comparer = comparer;
            }

            public int Compare(T x, T y)
            {
                return -comparer.Compare(x, y);
            }
        }
    }
}
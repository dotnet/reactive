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
        public static IAsyncEnumerable<TOther> SelectMany<TSource, TOther>(this IAsyncEnumerable<TSource> source, IAsyncEnumerable<TOther> other)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            return source.SelectMany(_ => other);
        }


        public static IAsyncEnumerable<TResult> SelectMany<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, IAsyncEnumerable<TResult>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return CreateEnumerable(
                () =>
                {
                    var e = source.GetEnumerator();
                    var ie = default(IAsyncEnumerator<TResult>);

                    var innerDisposable = new AssignableDisposable();

                    var cts = new CancellationTokenDisposable();
                    var d = Disposable.Create(cts, innerDisposable, e);

                    var inner = default(Func<CancellationToken, Task<bool>>);
                    var outer = default(Func<CancellationToken, Task<bool>>);

                    inner = async ct =>
                            {
                                if (await ie.MoveNext(ct)
                                            .ConfigureAwait(false))
                                {
                                    return true;
                                }
                                innerDisposable.Disposable = null;
                                return await outer(ct)
                                           .ConfigureAwait(false);
                            };

                    outer = async ct =>
                            {
                                if (await e.MoveNext(ct)
                                           .ConfigureAwait(false))
                                {
                                    var enumerable = selector(e.Current);
                                    ie = enumerable.GetEnumerator();
                                    innerDisposable.Disposable = ie;

                                    return await inner(ct)
                                               .ConfigureAwait(false);
                                }
                                return false;
                            };

                    return CreateEnumerator(ct => ie == null ? outer(cts.Token) : inner(cts.Token),
                                            () => ie.Current,
                                            d.Dispose,
                                            e
                    );
                });
        }

        public static IAsyncEnumerable<TResult> SelectMany<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, int, IAsyncEnumerable<TResult>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return CreateEnumerable(
                () =>
                {
                    var e = source.GetEnumerator();
                    var ie = default(IAsyncEnumerator<TResult>);

                    var index = 0;

                    var innerDisposable = new AssignableDisposable();

                    var cts = new CancellationTokenDisposable();
                    var d = Disposable.Create(cts, innerDisposable, e);

                    var inner = default(Func<CancellationToken, Task<bool>>);
                    var outer = default(Func<CancellationToken, Task<bool>>);

                    inner = async ct =>
                            {
                                if (await ie.MoveNext(ct)
                                            .ConfigureAwait(false))
                                {
                                    return true;
                                }
                                innerDisposable.Disposable = null;
                                return await outer(ct)
                                           .ConfigureAwait(false);
                            };

                    outer = async ct =>
                            {
                                if (await e.MoveNext(ct)
                                           .ConfigureAwait(false))
                                {
                                    var enumerable = selector(e.Current, checked(index++));
                                    ie = enumerable.GetEnumerator();
                                    innerDisposable.Disposable = ie;

                                    return await inner(ct)
                                               .ConfigureAwait(false);
                                }
                                return false;
                            };

                    return CreateEnumerator(ct => ie == null ? outer(cts.Token) : inner(cts.Token),
                                            () => ie.Current,
                                            d.Dispose,
                                            e
                    );
                });
        }

        public static IAsyncEnumerable<TResult> SelectMany<TSource, TCollection, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, IAsyncEnumerable<TCollection>> selector, Func<TSource, TCollection, TResult> resultSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            return source.SelectMany(x => selector(x)
                                         .Select(y => resultSelector(x, y)));
        }

        public static IAsyncEnumerable<TResult> SelectMany<TSource, TCollection, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, int, IAsyncEnumerable<TCollection>> selector, Func<TSource, TCollection, TResult> resultSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            return source.SelectMany((x, i) => selector(x, i)
                                         .Select(y => resultSelector(x, y)));
        }
    }
}
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
        public static IAsyncEnumerable<TSource> Skip<TSource>(this IAsyncEnumerable<TSource> source, int count)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            return CreateEnumerable(
                () =>
                {
                    var e = source.GetEnumerator();
                    var n = count;

                    var cts = new CancellationTokenDisposable();
                    var d = Disposable.Create(cts, e);

                    var f = default(Func<CancellationToken, Task<bool>>);
                    f = async ct =>
                        {
                            var moveNext = await e.MoveNext(ct)
                                                  .ConfigureAwait(false);
                            if (n == 0)
                            {
                                return moveNext;
                            }
                            --n;
                            if (!moveNext)
                            {
                                return false;
                            }
                            return await f(ct)
                                       .ConfigureAwait(false);
                        };

                    return CreateEnumerator(
                        ct => f(cts.Token),
                        () => e.Current,
                        d.Dispose,
                        e
                    );
                });
        }

        public static IAsyncEnumerable<TSource> SkipLast<TSource>(this IAsyncEnumerable<TSource> source, int count)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            return CreateEnumerable(
                () =>
                {
                    var e = source.GetEnumerator();

                    var cts = new CancellationTokenDisposable();
                    var d = Disposable.Create(cts, e);

                    var q = new Queue<TSource>();
                    var current = default(TSource);

                    var f = default(Func<CancellationToken, Task<bool>>);
                    f = async ct =>
                        {
                            if (await e.MoveNext(ct)
                                       .ConfigureAwait(false))
                            {
                                var item = e.Current;

                                q.Enqueue(item);
                                if (q.Count > count)
                                {
                                    current = q.Dequeue();
                                    return true;
                                }
                                return await f(ct)
                                           .ConfigureAwait(false);
                            }
                            return false;
                        };

                    return CreateEnumerator(
                        f,
                        () => current,
                        d.Dispose,
                        e
                    );
                });
        }

        public static IAsyncEnumerable<TSource> SkipWhile<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return CreateEnumerable(
                () =>
                {
                    var e = source.GetEnumerator();
                    var skipping = true;

                    var cts = new CancellationTokenDisposable();
                    var d = Disposable.Create(cts, e);

                    var f = default(Func<CancellationToken, Task<bool>>);
                    f = async ct =>
                        {
                            if (skipping)
                            {
                                if (await e.MoveNext(ct)
                                           .ConfigureAwait(false))
                                {
                                    if (predicate(e.Current))
                                        return await f(ct)
                                                   .ConfigureAwait(false);
                                    skipping = false;
                                    return true;
                                }
                                return false;
                            }
                            return await e.MoveNext(ct)
                                          .ConfigureAwait(false);
                        };

                    return CreateEnumerator(
                        f,
                        () => e.Current,
                        d.Dispose,
                        e
                    );
                });
        }

        public static IAsyncEnumerable<TSource> SkipWhile<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return CreateEnumerable(
                () =>
                {
                    var e = source.GetEnumerator();
                    var skipping = true;
                    var index = 0;

                    var cts = new CancellationTokenDisposable();
                    var d = Disposable.Create(cts, e);

                    var f = default(Func<CancellationToken, Task<bool>>);
                    f = async ct =>
                        {
                            if (skipping)
                            {
                                if (await e.MoveNext(ct)
                                           .ConfigureAwait(false))
                                {
                                    if (predicate(e.Current, checked(index++)))
                                        return await f(ct)
                                                   .ConfigureAwait(false);
                                    skipping = false;
                                    return true;
                                }
                                return false;
                            }
                            return await e.MoveNext(ct)
                                          .ConfigureAwait(false);
                        };

                    return CreateEnumerator(
                        f,
                        () => e.Current,
                        d.Dispose,
                        e
                    );
                });
        }
    }
}
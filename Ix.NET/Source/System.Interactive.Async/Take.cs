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
        public static IAsyncEnumerable<TSource> Take<TSource>(this IAsyncEnumerable<TSource> source, int count)
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
                    var current = default(TSource);

                    return CreateEnumerator(
                        async ct =>
                        {
                            if (n == 0)
                                return false;

                            var result = await e.MoveNext(cts.Token)
                                                .ConfigureAwait(false);

                            --n;
                            if (result)
                                current = e.Current;

                            if (n == 0)
                                e.Dispose();

                            return result;
                        },
                        () => current,
                        d.Dispose,
                        e
                    );
                });
        }

        public static IAsyncEnumerable<TSource> TakeLast<TSource>(this IAsyncEnumerable<TSource> source, int count)
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

                    var q = new Queue<TSource>(count);
                    var done = false;
                    var current = default(TSource);

                    var f = default(Func<CancellationToken, Task<bool>>);
                    f = async ct =>
                        {
                            if (!done)
                            {
                                if (await e.MoveNext(ct)
                                           .ConfigureAwait(false))
                                {
                                    if (count > 0)
                                    {
                                        var item = e.Current;
                                        if (q.Count >= count)
                                            q.Dequeue();
                                        q.Enqueue(item);
                                    }
                                }
                                else
                                {
                                    done = true;
                                    e.Dispose();
                                }

                                return await f(ct)
                                           .ConfigureAwait(false);
                            }
                            if (q.Count > 0)
                            {
                                current = q.Dequeue();
                                return true;
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

        public static IAsyncEnumerable<TSource> TakeWhile<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return CreateEnumerable(
                () =>
                {
                    var e = source.GetEnumerator();

                    var cts = new CancellationTokenDisposable();
                    var d = Disposable.Create(cts, e);

                    return CreateEnumerator(
                        async ct =>
                        {
                            if (await e.MoveNext(cts.Token)
                                       .ConfigureAwait(false))
                            {
                                return predicate(e.Current);
                            }
                            return false;
                        },
                        () => e.Current,
                        d.Dispose,
                        e
                    );
                });
        }

        public static IAsyncEnumerable<TSource> TakeWhile<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return CreateEnumerable(
                () =>
                {
                    var e = source.GetEnumerator();
                    var index = 0;

                    var cts = new CancellationTokenDisposable();
                    var d = Disposable.Create(cts, e);

                    return CreateEnumerator(
                        async ct =>
                        {
                            if (await e.MoveNext(cts.Token)
                                       .ConfigureAwait(false))
                            {
                                return predicate(e.Current, checked(index++));
                            }
                            return false;
                        },
                        () => e.Current,
                        d.Dispose,
                        e
                    );
                });
        }
    }
}
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
        public static IAsyncEnumerable<TAccumulate> Scan<TSource, TAccumulate>(this IAsyncEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> accumulator)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (accumulator == null)
                throw new ArgumentNullException(nameof(accumulator));

            return CreateEnumerable(
                () =>
                {
                    var e = source.GetEnumerator();

                    var cts = new CancellationTokenDisposable();
                    var d = Disposable.Create(cts, e);

                    var acc = seed;
                    var current = default(TAccumulate);

                    var f = default(Func<CancellationToken, Task<bool>>);
                    f = async ct =>
                        {
                            if (!await e.MoveNext(ct)
                                        .ConfigureAwait(false))
                            {
                                return false;
                            }

                            var item = e.Current;
                            acc = accumulator(acc, item);

                            current = acc;
                            return true;
                        };

                    return CreateEnumerator(
                        f,
                        () => current,
                        d.Dispose,
                        e
                    );
                });
        }

        public static IAsyncEnumerable<TSource> Scan<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, TSource, TSource> accumulator)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (accumulator == null)
                throw new ArgumentNullException(nameof(accumulator));

            return CreateEnumerable(
                () =>
                {
                    var e = source.GetEnumerator();

                    var cts = new CancellationTokenDisposable();
                    var d = Disposable.Create(cts, e);

                    var hasSeed = false;
                    var acc = default(TSource);
                    var current = default(TSource);

                    var f = default(Func<CancellationToken, Task<bool>>);
                    f = async ct =>
                        {
                            if (!await e.MoveNext(ct)
                                        .ConfigureAwait(false))
                            {
                                return false;
                            }

                            var item = e.Current;

                            if (!hasSeed)
                            {
                                hasSeed = true;
                                acc = item;
                                return await f(ct)
                                           .ConfigureAwait(false);
                            }

                            acc = accumulator(acc, item);

                            current = acc;
                            return true;
                        };

                    return CreateEnumerator(
                        f,
                        () => current,
                        d.Dispose,
                        e
                    );
                });
        }
    }
}
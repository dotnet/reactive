// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        public static IAsyncEnumerable<TSource> Catch<TSource, TException>(this IAsyncEnumerable<TSource> source, Func<TException, IAsyncEnumerable<TSource>> handler)
            where TException : Exception
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            return CreateEnumerable(() =>
            {
                var e = source.GetEnumerator();

                var cts = new CancellationTokenDisposable();
                var a = new AssignableDisposable
                {
                    Disposable = e
                };
                var d = Disposable.Create(cts, a);
                var done = false;

                var f = default(Func<CancellationToken, Task<bool>>);
                f = async ct =>
                {
                    if (!done)
                    {
                        try
                        {
                            return await e.MoveNext(ct)
                                          .ConfigureAwait(false);
                        }
                        catch (TException ex)
                        {
                            var err = handler(ex)
                                .GetEnumerator();
                            e = err;
                            a.Disposable = e;
                            done = true;
                            return await f(ct)
                                       .ConfigureAwait(false);
                        }
                    }
                    return await e.MoveNext(ct)
                                  .ConfigureAwait(false);
                };

                return CreateEnumerator(
                    f,
                    () => e.Current,
                    d.Dispose,
                    a
                );
            });
        }

        public static IAsyncEnumerable<TSource> Catch<TSource>(this IEnumerable<IAsyncEnumerable<TSource>> sources)
        {
            if (sources == null)
                throw new ArgumentNullException(nameof(sources));

            return sources.Catch_();
        }

        public static IAsyncEnumerable<TSource> Catch<TSource>(params IAsyncEnumerable<TSource>[] sources)
        {
            if (sources == null)
                throw new ArgumentNullException(nameof(sources));

            return sources.Catch_();
        }

        public static IAsyncEnumerable<TSource> Catch<TSource>(this IAsyncEnumerable<TSource> first, IAsyncEnumerable<TSource> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));

            return new[] { first, second }.Catch_();
        }

        private static IAsyncEnumerable<TSource> Catch_<TSource>(this IEnumerable<IAsyncEnumerable<TSource>> sources)
        {
            return CreateEnumerable(() =>
            {
                var se = sources.GetEnumerator();
                var e = default(IAsyncEnumerator<TSource>);

                var cts = new CancellationTokenDisposable();
                var a = new AssignableDisposable();
                var d = Disposable.Create(cts, se, a);

                var error = default(ExceptionDispatchInfo);

                var f = default(Func<CancellationToken, Task<bool>>);
                f = async ct =>
                {
                    if (e == null)
                    {
                        if (se.MoveNext())
                        {
                            e = se.Current.GetEnumerator();
                        }
                        else
                        {
                            error?.Throw();
                            return false;
                        }

                        error = null;

                        a.Disposable = e;
                    }

                    try
                    {
                        return await e.MoveNext(ct)
                                      .ConfigureAwait(false);
                    }
                    catch (Exception exception)
                    {
                        e.Dispose();
                        e = null;
                        error = ExceptionDispatchInfo.Capture(exception);
                        return await f(ct)
                                   .ConfigureAwait(false);
                    }
                };

                return CreateEnumerator(
                    f,
                    () => e.Current,
                    d.Dispose,
                    a
                );
            });
        }
    }
}
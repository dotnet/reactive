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

            return Create(() =>
            {
                var e = source.GetEnumerator();

                var cts = new CancellationTokenDisposable();
                var a = new AssignableDisposable { Disposable = e };
                var d = Disposable.Create(cts, a);
                var done = false;

                var f = default(Func<CancellationToken, Task<bool>>);
                f = async ct =>
                {
                    if (!done)
                    {
                        try
                        {
                            return await e.MoveNext(ct).ConfigureAwait(false);
                        }
                        catch (TException ex)
                        {
                            var err = handler(ex).GetEnumerator();
                            e = err;
                            a.Disposable = e;
                            done = true;
                            return await f(ct).ConfigureAwait(false);
                        }
                    }
                    return await e.MoveNext(ct).ConfigureAwait(false);
                };

                return Create(
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
            return Create(() =>
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
                        return await e.MoveNext(ct).ConfigureAwait(false);
                    }
                    catch (Exception exception)
                    {
                        e.Dispose();
                        e = null;
                        error = ExceptionDispatchInfo.Capture(exception);
                        return await f(ct).ConfigureAwait(false);
                    }
                };

                return Create(
                    f,
                    () => e.Current,
                    d.Dispose,
                    a
                );
            });
        }

        public static IAsyncEnumerable<TSource> Finally<TSource>(this IAsyncEnumerable<TSource> source, Action finallyAction)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (finallyAction == null)
                throw new ArgumentNullException(nameof(finallyAction));

            return Create(() =>
            {
                var e = source.GetEnumerator();

                var cts = new CancellationTokenDisposable();
                var r = new Disposable(finallyAction);
                var d = Disposable.Create(cts, e, r);

                return Create(
                    ct => e.MoveNext(ct),
                    () => e.Current,
                    d.Dispose,
                    r
                );
            });
        }

        public static IAsyncEnumerable<TSource> OnErrorResumeNext<TSource>(this IAsyncEnumerable<TSource> first, IAsyncEnumerable<TSource> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));

            return OnErrorResumeNext_(new[] { first, second });
        }

        public static IAsyncEnumerable<TSource> OnErrorResumeNext<TSource>(params IAsyncEnumerable<TSource>[] sources)
        {
            if (sources == null)
                throw new ArgumentNullException(nameof(sources));

            return OnErrorResumeNext_(sources);
        }

        public static IAsyncEnumerable<TSource> OnErrorResumeNext<TSource>(this IEnumerable<IAsyncEnumerable<TSource>> sources)
        {
            if (sources == null)
                throw new ArgumentNullException(nameof(sources));

            return OnErrorResumeNext_(sources);
        }

        private static IAsyncEnumerable<TSource> OnErrorResumeNext_<TSource>(IEnumerable<IAsyncEnumerable<TSource>> sources)
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
                        else
                        {
                            return false;
                        }

                        a.Disposable = e;
                    }

                    try
                    {
                        if (await e.MoveNext(ct).ConfigureAwait(false))
                        {
                            return true;
                        }
                    }
                    catch
                    {
                        // ignore
                    }

                    e.Dispose();
                    e = null;
                    return await f(ct).ConfigureAwait(false);
                };

                return Create(
                    f,
                    () => e.Current,
                    d.Dispose,
                    a
                );
            });
        }

        public static IAsyncEnumerable<TSource> Retry<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new[] { source }.Repeat().Catch();
        }

        public static IAsyncEnumerable<TSource> Retry<TSource>(this IAsyncEnumerable<TSource> source, int retryCount)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (retryCount < 0)
                throw new ArgumentOutOfRangeException(nameof(retryCount));

            return new[] { source }.Repeat(retryCount).Catch();
        }

        private static IEnumerable<TSource> Repeat<TSource>(this IEnumerable<TSource> source)
        {
            while (true)
                foreach (var item in source)
                    yield return item;
        }

        private static IEnumerable<TSource> Repeat<TSource>(this IEnumerable<TSource> source, int count)
        {
            for (var i = 0; i < count; i++)
                foreach (var item in source)
                    yield return item;
        }
    }
}

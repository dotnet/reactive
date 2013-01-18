// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.
using System;
using System.Collections.Generic;
using System.Linq;
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
                throw new ArgumentNullException("source");
            if (handler == null)
                throw new ArgumentNullException("handler");

            return Create(() =>
            {
                var e = source.GetEnumerator();

                var cts = new CancellationTokenDisposable();
                var a = new AssignableDisposable { Disposable = e };
                var d = new CompositeDisposable(cts, a);
                var done = false;

                var f = default(Action<TaskCompletionSource<bool>, CancellationToken>);
                f = (tcs, ct) =>
                {
                    if (!done)
                    {
                        e.MoveNext(ct).ContinueWith(t =>
                        {
                            t.Handle(tcs,
                                res =>
                                {
                                    tcs.TrySetResult(res);
                                },
                                ex =>
                                {
                                    var err = default(IAsyncEnumerator<TSource>);

                                    try
                                    {
                                        ex.Flatten().Handle(ex_ =>
                                        {
                                            var exx = ex_ as TException;
                                            if (exx != null)
                                            {
                                                err = handler(exx).GetEnumerator();
                                                return true;
                                            }

                                            return false;
                                        });
                                    }
                                    catch (Exception ex2)
                                    {
                                        tcs.TrySetException(ex2);
                                        return;
                                    }

                                    if (err != null)
                                    {
                                        e = err;
                                        a.Disposable = e;

                                        done = true;
                                        f(tcs, ct);
                                    }
                                }
                            );
                        });
                    }
                    else
                    {
                        e.MoveNext(ct).ContinueWith(t =>
                        {
                            t.Handle(tcs, res =>
                            {
                                tcs.TrySetResult(res);
                            });
                        });
                    }
                };

                return Create(
                    (ct, tcs) =>
                    {
                        f(tcs, cts.Token);
                        return tcs.Task.UsingEnumerator(a);
                    },
                    () => e.Current,
                    d.Dispose
                );
            });
        }

        public static IAsyncEnumerable<TSource> Catch<TSource>(this IEnumerable<IAsyncEnumerable<TSource>> sources)
        {
            if (sources == null)
                throw new ArgumentNullException("sources");

            return sources.Catch_();
        }

        public static IAsyncEnumerable<TSource> Catch<TSource>(params IAsyncEnumerable<TSource>[] sources)
        {
            if (sources == null)
                throw new ArgumentNullException("sources");

            return sources.Catch_();
        }

        public static IAsyncEnumerable<TSource> Catch<TSource>(this IAsyncEnumerable<TSource> first, IAsyncEnumerable<TSource> second)
        {
            if (first == null)
                throw new ArgumentNullException("first");
            if (second == null)
                throw new ArgumentNullException("second");

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
                var d = new CompositeDisposable(cts, se, a);

                var error = default(Exception);

                var f = default(Action<TaskCompletionSource<bool>, CancellationToken>);
                f = (tcs, ct) =>
                {
                    if (e == null)
                    {
                        var b = false;
                        try
                        {
                            b = se.MoveNext();
                            if (b)
                                e = se.Current.GetEnumerator();
                        }
                        catch (Exception ex)
                        {
                            tcs.TrySetException(ex);
                            return;
                        }

                        if (!b)
                        {
                            if (error != null)
                            {
                                tcs.TrySetException(error);
                                return;
                            }

                            tcs.TrySetResult(false);
                            return;
                        }

                        error = null;

                        a.Disposable = e;
                    }

                    e.MoveNext(ct).ContinueWith(t =>
                    {
                        t.Handle(tcs,
                            res =>
                            {
                                tcs.TrySetResult(res);
                            },
                            ex =>
                            {
                                e.Dispose();
                                e = null;

                                error = ex;
                                f(tcs, ct);
                            }
                        );
                    });
                };

                return Create(
                    (ct, tcs) =>
                    {
                        f(tcs, cts.Token);
                        return tcs.Task.UsingEnumerator(a);
                    },
                    () => e.Current,
                    d.Dispose
                );
            });
        }

        public static IAsyncEnumerable<TSource> Finally<TSource>(this IAsyncEnumerable<TSource> source, Action finallyAction)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (finallyAction == null)
                throw new ArgumentNullException("finallyAction");

            return Create(() =>
            {
                var e = source.GetEnumerator();

                var cts = new CancellationTokenDisposable();
                var r = new Disposable(finallyAction);
                var d = new CompositeDisposable(cts, e, r);

                var f = default(Action<TaskCompletionSource<bool>, CancellationToken>);
                f = (tcs, ct) =>
                {
                    e.MoveNext(ct).ContinueWith(t =>
                    {
                        t.Handle(tcs, res =>
                        {
                            tcs.TrySetResult(res);
                        });
                    });
                };

                return Create(
                    (ct, tcs) =>
                    {
                        f(tcs, cts.Token);
                        return tcs.Task.UsingEnumeratorSync(r);
                    },
                    () => e.Current,
                    d.Dispose
                );
            });
        }

        public static IAsyncEnumerable<TSource> OnErrorResumeNext<TSource>(this IAsyncEnumerable<TSource> first, IAsyncEnumerable<TSource> second)
        {
            if (first == null)
                throw new ArgumentNullException("first");
            if (second == null)
                throw new ArgumentNullException("second");

            return OnErrorResumeNext_(new[] { first, second });
        }

        public static IAsyncEnumerable<TSource> OnErrorResumeNext<TSource>(params IAsyncEnumerable<TSource>[] sources)
        {
            if (sources == null)
                throw new ArgumentNullException("sources");

            return OnErrorResumeNext_(sources);
        }

        public static IAsyncEnumerable<TSource> OnErrorResumeNext<TSource>(this IEnumerable<IAsyncEnumerable<TSource>> sources)
        {
            if (sources == null)
                throw new ArgumentNullException("sources");

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
                var d = new CompositeDisposable(cts, se, a);

                var f = default(Action<TaskCompletionSource<bool>, CancellationToken>);
                f = (tcs, ct) =>
                {
                    if (e == null)
                    {
                        var b = false;
                        try
                        {
                            b = se.MoveNext();
                            if (b)
                                e = se.Current.GetEnumerator();
                        }
                        catch (Exception ex)
                        {
                            tcs.TrySetException(ex);
                            return;
                        }

                        if (!b)
                        {
                            tcs.TrySetResult(false);
                            return;
                        }

                        a.Disposable = e;
                    }

                    e.MoveNext(ct).ContinueWith(t =>
                    {
                        t.Handle(tcs,
                            res =>
                            {
                                if (res)
                                {
                                    tcs.TrySetResult(true);
                                }
                                else
                                {
                                    e.Dispose();
                                    e = null;

                                    f(tcs, ct);
                                }
                            },
                            ex =>
                            {
                                e.Dispose();
                                e = null;

                                f(tcs, ct);
                            }
                        );
                    });
                };

                return Create(
                    (ct, tcs) =>
                    {
                        f(tcs, cts.Token);
                        return tcs.Task.UsingEnumerator(a);
                    },
                    () => e.Current,
                    d.Dispose
                );
            });
        }

        public static IAsyncEnumerable<TSource> Retry<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return new[] { source }.Repeat().Catch();
        }

        public static IAsyncEnumerable<TSource> Retry<TSource>(this IAsyncEnumerable<TSource> source, int retryCount)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (retryCount < 0)
                throw new ArgumentOutOfRangeException("retryCount");

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

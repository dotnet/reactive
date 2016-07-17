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
        public static IAsyncEnumerable<TSource> OnErrorResumeNext<TSource>(this IAsyncEnumerable<TSource> first, IAsyncEnumerable<TSource> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));

            return OnErrorResumeNext_(new[] {first, second});
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
            return CreateEnumerable(
                () =>
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
                                if (await e.MoveNext(ct)
                                           .ConfigureAwait(false))
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
                            return await f(ct)
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
    }
}
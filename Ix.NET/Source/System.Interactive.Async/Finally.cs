// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        public static IAsyncEnumerable<TSource> Finally<TSource>(this IAsyncEnumerable<TSource> source, Action finallyAction)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (finallyAction == null)
                throw new ArgumentNullException(nameof(finallyAction));

            return CreateEnumerable(
                () =>
                {
                    var e = source.GetEnumerator();

                    var cts = new CancellationTokenDisposable();
                    var r = new Disposable(finallyAction);
                    var d = Disposable.Create(cts, e, r);

                    return CreateEnumerator(
                        ct => e.MoveNext(ct),
                        () => e.Current,
                        d.Dispose,
                        r
                    );
                });
        }
    }
}
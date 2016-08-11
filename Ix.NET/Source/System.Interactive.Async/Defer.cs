// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        public static IAsyncEnumerable<TSource> Defer<TSource>(Func<IAsyncEnumerable<TSource>> factory)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            return CreateEnumerable(
                () => factory()
                    .GetEnumerator());
        }

        public static IAsyncEnumerable<TSource> Defer<TSource>(Func<CancellationToken, Task<IAsyncEnumerable<TSource>>> asyncFactory)
        {
            if (asyncFactory == null)
                throw new ArgumentNullException(nameof(asyncFactory));

            return CreateEnumerable(
                () => 
                {
                    var baseEnumerator = default(IAsyncEnumerator<TSource>);

                    return CreateEnumerator(
                        async ct =>
                        {
                            if (baseEnumerator == null)
                                baseEnumerator = (await asyncFactory(ct).ConfigureAwait(false)).GetEnumerator();

                            return await baseEnumerator.MoveNext(ct).ConfigureAwait(false);
                        },
                        () =>
                        {
                            if (baseEnumerator == null)
                                throw new InvalidOperationException();

                            return baseEnumerator.Current;
                        },
                        () => baseEnumerator?.Dispose());
                });
        }
    }
}
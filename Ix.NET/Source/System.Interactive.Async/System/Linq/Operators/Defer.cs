// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Threading.Tasks;
using static System.Linq.AsyncEnumerable;

namespace System.Linq
{
    public static partial class AsyncEnumerableEx
    {
        public static IAsyncEnumerable<TSource> Defer<TSource>(Func<IAsyncEnumerable<TSource>> factory)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            return CreateEnumerable(() => factory().GetAsyncEnumerator());
        }

        public static IAsyncEnumerable<TSource> Defer<TSource>(Func<Task<IAsyncEnumerable<TSource>>> factory)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            return CreateEnumerable(async () => (await factory().ConfigureAwait(false)).GetAsyncEnumerator());
        }
    }
}

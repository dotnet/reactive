// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        public static IAsyncEnumerable<TResult> Repeat<TResult>(TResult element, int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            return Enumerable.Repeat(element, count).ToAsyncEnumerable();
        }
    }
}

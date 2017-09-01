// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        public static IAsyncEnumerable<TValue> Empty<TValue>()
        {
            return CreateEnumerable(() => CreateEnumerator<TValue>(ct => TaskExt.False, current: null, dispose: null));
        }
    }
}

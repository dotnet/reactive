// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 
using System;
using System.Collections.Generic;

namespace System.Linq
{
    public interface IAsyncGrouping<
#if !NO_VARIANCE
        out 
#endif
        TKey,
#if !NO_VARIANCE
        out 
#endif
        TElement> : IAsyncEnumerable<TElement>
    {
        TKey Key { get; }
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 
using System;
using System.Collections.Generic;

namespace System.Linq
{
    public interface IOrderedAsyncEnumerable<
#if !NO_VARIANCE
out 
#endif
        TElement> : IAsyncEnumerable<TElement>
    {
        IOrderedAsyncEnumerable<TElement> CreateOrderedEnumerable<TKey>(Func<TElement, TKey> keySelector, IComparer<TKey> comparer, bool descending);
    }
}

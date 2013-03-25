// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.
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

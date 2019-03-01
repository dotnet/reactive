// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public interface IOrderedAsyncEnumerable<out TElement> : IAsyncEnumerable<TElement>
    {
        IOrderedAsyncEnumerable<TElement> CreateOrderedEnumerable<TKey>(Func<TElement, TKey> keySelector, IComparer<TKey> comparer, bool descending);
        IOrderedAsyncEnumerable<TElement> CreateOrderedEnumerable<TKey>(Func<TElement, ValueTask<TKey>> keySelector, IComparer<TKey> comparer, bool descending);

#if !NO_DEEP_CANCELLATION
        IOrderedAsyncEnumerable<TElement> CreateOrderedEnumerable<TKey>(Func<TElement, CancellationToken, ValueTask<TKey>> keySelector, IComparer<TKey> comparer, bool descending);
#endif
    }
}

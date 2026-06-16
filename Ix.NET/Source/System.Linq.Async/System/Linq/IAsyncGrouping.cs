// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;

namespace System.Linq
{
    // System.Linq.AsyncEnumerable defines no equivalent for this, because it uses an IAsyncEnumerable<IGrouping<TKey, TElement>>
    // In theory that is less good because it allows await only at the per-group level. In practice System.Linq.Async never
    // made use of that because the only operators that returned an IAsyncGrouping<TKey, TElement> fully enumerated the source
    // on the very first MoveNextAsync, meaning that by the time you got to inspect a group, its contents were available
    // immediately. So this interface merely forced code to use await foreach unnecessarily. We retain this for binary
    // compatibility, but it serves no long term purpose.
    public interface IAsyncGrouping<out TKey, out TElement> : IAsyncEnumerable<TElement>
    {
        TKey Key { get; }
    }
}

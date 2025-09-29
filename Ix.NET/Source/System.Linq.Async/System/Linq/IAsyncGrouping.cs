﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;

namespace System.Linq
{
    // TODO: System.Linq does this as an IAsyncEnumerable<IGrouping<TKey, TElement>>
    public interface IAsyncGrouping<out TKey, out TElement> : IAsyncEnumerable<TElement>
    {
        TKey Key { get; }
    }
}

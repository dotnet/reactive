// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.
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

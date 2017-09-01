// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

#if NO_ARRAY_EMPTY

namespace System.Linq
{
    internal sealed class EmptyArray<TElement>
    {
        public static readonly TElement[] Value = new TElement[0];
    }
}

#endif

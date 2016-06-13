// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.
#if NO_ARRAY_EMPTY
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.Linq
{
    internal sealed class EmptyArray<TElement>
    {
        public static readonly TElement[] Value = new TElement[0];
    }
}
#endif

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

#nullable disable

using System.Collections.Generic;
using System.Reactive.Subjects;

namespace System.Reactive
{
    internal sealed class Grouping<TKey, TElement> : Dictionary<TKey, Subject<TElement>>
    {
        public Grouping(IEqualityComparer<TKey> comparer)
            : base(comparer)
        {
        }

        public Grouping(int capacity, IEqualityComparer<TKey> comparer)
            : base(capacity, comparer)
        {
        }
    }
}

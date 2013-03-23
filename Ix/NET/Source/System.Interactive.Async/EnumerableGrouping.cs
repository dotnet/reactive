// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.
using System;
using System.Collections.Generic;
using System.Linq;

namespace System.Linq
{
    class EnumerableGrouping<TKey, TElement> : IGrouping<TKey, TElement>
    {
        List<TElement> elements = new List<TElement>();

        public EnumerableGrouping(TKey key)
        {
            Key = key;
        }

        public void Add(TElement element)
        {
            elements.Add(element);
        }

        public TKey Key { get; private set; }

        public IEnumerator<TElement> GetEnumerator()
        {
            return elements.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}

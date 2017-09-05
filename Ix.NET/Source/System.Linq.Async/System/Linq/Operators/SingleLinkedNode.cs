// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Diagnostics;

namespace System.Linq
{
    internal sealed class SingleLinkedNode<TSource>
    {
        public SingleLinkedNode(TSource first, TSource second)
        {
            Linked = new SingleLinkedNode<TSource>(first);
            Item = second;
            Count = 2;
        }

        public SingleLinkedNode(TSource item)
        {
            Item = item;
            Count = 1;
        }

        private SingleLinkedNode(SingleLinkedNode<TSource> linked, TSource item)
        {
            Debug.Assert(linked != null);
            Linked = linked;
            Item = item;
            Count = linked.Count + 1;
        }

        public TSource Item { get; }

        public SingleLinkedNode<TSource> Linked { get; }

        public int Count { get; }

        public SingleLinkedNode<TSource> Add(TSource item) => new SingleLinkedNode<TSource>(this, item);

        public IEnumerator<TSource> GetEnumerator()
        {
            var array = new TSource[Count];
            var index = Count;
            for (var n = this; n != null; n = n.Linked)
            {
                --index;
                array[index] = n.Item;
            }

            Debug.Assert(index == 0);
            return ((IEnumerable<TSource>)array).GetEnumerator();
        }
    }
}

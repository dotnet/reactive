// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Diagnostics;

namespace System.Linq
{
    internal sealed class RefCountList<T> : IRefCountList<T>
    {
        private readonly IDictionary<int, RefCount> _list;

        public RefCountList(int readerCount)
        {
            ReaderCount = readerCount;
            _list = new Dictionary<int, RefCount>();
        }

        public int ReaderCount { get; set; }

        public void Clear() => _list.Clear();

        public int Count { get; private set; }

        public T this[int i]
        {
            get
            {
                Debug.Assert(i < Count);

                if (!_list.TryGetValue(i, out var res))
                    throw new InvalidOperationException("Element no longer available in the buffer.");

                var val = res.Value;

                if (--res.Count == 0)
                {
                    _list.Remove(i);
                }

                return val;
            }
        }

        public void Add(T item)
        {
            _list[Count] = new RefCount(item, ReaderCount);

            Count++;
        }

        public void Done(int index)
        {
            for (var i = index; i < Count; i++)
            {
                _ = this[i];
            }

            ReaderCount--;
        }

        private sealed class RefCount
        {
            public RefCount(T value, int count)
            {
                Value = value;
                Count = count;
            }

            public int Count { get; set; }
            public T Value { get; }
        }
    }
}

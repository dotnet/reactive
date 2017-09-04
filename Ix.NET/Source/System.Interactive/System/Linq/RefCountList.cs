// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Diagnostics;

namespace System.Linq
{
    internal sealed class RefCountList<T> : IRefCountList<T>
    {
        private int _readerCount;
        private readonly IDictionary<int, RefCount> _list;
        private int _count;

        public RefCountList(int readerCount)
        {
            _readerCount = readerCount;
            _list = new Dictionary<int, RefCount>();
        }

        public int ReaderCount
        {
            get => _readerCount;
            set => _readerCount = value;
        }

        public void Clear() => _list.Clear();

        public int Count => _count;

        public T this[int i]
        {
            get
            {
                Debug.Assert(i < _count);

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
            _list[_count] = new RefCount { Value = item, Count = _readerCount };

            _count++;
        }

        public void Done(int index)
        {
            for (var i = index; i < _count; i++)
            {
                var ignore = this[i];
            }

            _readerCount--;
        }

        private sealed class RefCount
        {
            public int Count;
            public T Value;
        }
    }
}

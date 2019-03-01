// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Linq;

namespace System.Linq
{
    internal sealed class MaxRefCountList<T> : IRefCountList<T>
    {
        private readonly IList<T> _list = new List<T>();

        public void Clear() => _list.Clear();

        public int Count => _list.Count;

        public T this[int i] => _list[i];

        public void Add(T item) => _list.Add(item);

        public void Done(int index) { }
    }
}

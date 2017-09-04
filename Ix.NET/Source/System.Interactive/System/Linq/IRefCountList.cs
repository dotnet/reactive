// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

namespace System.Linq
{
    internal interface IRefCountList<T>
    {
        void Clear();

        int Count { get; }

        T this[int i] { get; }

        void Add(T item);

        void Done(int index);
    }
}

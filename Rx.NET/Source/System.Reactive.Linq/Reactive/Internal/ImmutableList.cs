// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive
{
    class ImmutableList<T>
    {
        T[] data;

        public ImmutableList()
        {
            data = new T[0];
        }

        public ImmutableList(T[] data)
        {
            this.data = data;
        }

        public ImmutableList<T> Add(T value)
        {
            var newData = new T[data.Length + 1];
            Array.Copy(data, newData, data.Length);
            newData[data.Length] = value;
            return new ImmutableList<T>(newData);
        }

        public ImmutableList<T> Remove(T value)
        {
            var i = IndexOf(value);
            if (i < 0)
                return this;
            var newData = new T[data.Length - 1];
            Array.Copy(data, 0, newData, 0, i);
            Array.Copy(data, i + 1, newData, i, data.Length - i - 1);
            return new ImmutableList<T>(newData);
        }

        public int IndexOf(T value)
        {
            for (var i = 0; i < data.Length; ++i)
                if (data[i].Equals(value))
                    return i;
            return -1;
        }

        public T[] Data
        {
            get { return data; }
        }
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive
{
    internal sealed class InternalImmutableList<T>
    {
        public static readonly InternalImmutableList<T> Empty = new InternalImmutableList<T>();

        private readonly T[] _data;

        private InternalImmutableList()
        {
            _data = new T[0];
        }

        public InternalImmutableList(T[] data)
        {
            _data = data;
        }

        public T[] Data => _data;

        public InternalImmutableList<T> Add(T value)
        {
            var newData = new T[_data.Length + 1];

            Array.Copy(_data, newData, _data.Length);
            newData[_data.Length] = value;

            return new InternalImmutableList<T>(newData);
        }

        public InternalImmutableList<T> Remove(T value)
        {
            var i = IndexOf(value);
            if (i < 0)
            {
                return this;
            }

            var length = _data.Length;
            if (length == 1)
            {
                return Empty;
            }

            var newData = new T[length - 1];

            Array.Copy(_data, 0, newData, 0, i);
            Array.Copy(_data, i + 1, newData, i, length - i - 1);

            return new InternalImmutableList<T>(newData);
        }

        private int IndexOf(T value)
        {
            for (var i = 0; i < _data.Length; ++i)
            {
                if (Equals(_data[i], value))
                {
                    return i;
                }
            }

            return -1;
        }
    }
}

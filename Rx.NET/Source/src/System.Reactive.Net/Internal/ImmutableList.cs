// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive
{
    internal sealed class ImmutableList<T>
    {
        public static readonly ImmutableList<T> Empty = new();

        private readonly T[] _data;

        private ImmutableList() => _data = [];

        public ImmutableList(T[] data) => _data = data;

        public T[] Data => _data;

        public ImmutableList<T> Add(T value)
        {
            var newData = new T[_data.Length + 1];

            Array.Copy(_data, newData, _data.Length);
            newData[_data.Length] = value;

            return new ImmutableList<T>(newData);
        }

        public ImmutableList<T> Remove(T value)
        {
            var i = Array.IndexOf(_data, value);
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

            return new ImmutableList<T>(newData);
        }
    }
}

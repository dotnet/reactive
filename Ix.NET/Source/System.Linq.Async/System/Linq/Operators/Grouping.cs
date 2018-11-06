// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections;
using System.Collections.Generic;
using System.Threading;

// Note: The type here has to be internal as System.Linq has it's own public copy we're not using

namespace System.Linq.Internal
{
    /// Adapted from System.Linq.Grouping from .NET Framework
    /// Source: https://github.com/dotnet/corefx/blob/b90532bc97b07234a7d18073819d019645285f1c/src/System.Linq/src/System/Linq/Grouping.cs#L64
    internal class Grouping<TKey, TElement> : IGrouping<TKey, TElement>, IList<TElement>, IAsyncGrouping<TKey, TElement>
    {
        internal int _count;
        internal TElement[] _elements;
        internal int _hashCode;
        internal Grouping<TKey, TElement> _hashNext;
        internal TKey _key;
        internal Grouping<TKey, TElement> _next;

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<TElement> GetEnumerator()
        {
            for (var i = 0; i < _count; i++)
            {
                yield return _elements[i];
            }
        }

        // DDB195907: implement IGrouping<>.Key implicitly
        // so that WPF binding works on this property.
        public TKey Key => _key;

        int ICollection<TElement>.Count => _count;

        bool ICollection<TElement>.IsReadOnly => true;

        void ICollection<TElement>.Add(TElement item)
        {
            throw new NotSupportedException(Strings.NOT_SUPPORTED);
        }

        void ICollection<TElement>.Clear()
        {
            throw new NotSupportedException(Strings.NOT_SUPPORTED);
        }

        bool ICollection<TElement>.Contains(TElement item) => Array.IndexOf(_elements, item, 0, _count) >= 0;

        void ICollection<TElement>.CopyTo(TElement[] array, int arrayIndex) => Array.Copy(_elements, 0, array, arrayIndex, _count);

        bool ICollection<TElement>.Remove(TElement item)
        {
            throw new NotSupportedException(Strings.NOT_SUPPORTED);
        }

        int IList<TElement>.IndexOf(TElement item) => Array.IndexOf(_elements, item, 0, _count);

        void IList<TElement>.Insert(int index, TElement item)
        {
            throw new NotSupportedException(Strings.NOT_SUPPORTED);
        }

        void IList<TElement>.RemoveAt(int index)
        {
            throw new NotSupportedException(Strings.NOT_SUPPORTED);
        }

        TElement IList<TElement>.this[int index]
        {
            get
            {
                if (index < 0 || index >= _count)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                return _elements[index];
            }

            set { throw new NotSupportedException(Strings.NOT_SUPPORTED); }
        }

        internal void Add(TElement element)
        {
            if (_elements.Length == _count)
            {
                Array.Resize(ref _elements, checked(_count*2));
            }

            _elements[_count] = element;
            _count++;
        }

        internal void Trim()
        {
            if (_elements.Length != _count)
            {
                Array.Resize(ref _elements, _count);
            }
        }

        IAsyncEnumerator<TElement> IAsyncEnumerable<TElement>.GetAsyncEnumerator(CancellationToken cancellationToken) => this.ToAsyncEnumerable().GetAsyncEnumerator(cancellationToken);
    }
}

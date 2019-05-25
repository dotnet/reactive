// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections;
using System.Collections.Generic;
using System.Threading;

// Note: The type here has to be internal as System.Linq has its own public copy we're not using.

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
        internal Grouping<TKey, TElement>? _next;

        public Grouping(TKey key, int hashCode, TElement[] elements, Grouping<TKey, TElement> hashNext)
        {
            _key = key;
            _hashCode = hashCode;
            _elements = elements;
            _hashNext = hashNext;
        }

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

        void ICollection<TElement>.Add(TElement item) => throw Error.NotSupported();

        void ICollection<TElement>.Clear() => throw Error.NotSupported();

        bool ICollection<TElement>.Contains(TElement item) => Array.IndexOf(_elements, item, 0, _count) >= 0;

        void ICollection<TElement>.CopyTo(TElement[] array, int arrayIndex) => Array.Copy(_elements, 0, array, arrayIndex, _count);

        bool ICollection<TElement>.Remove(TElement item) => throw Error.NotSupported();

        int IList<TElement>.IndexOf(TElement item) => Array.IndexOf(_elements, item, 0, _count);

        void IList<TElement>.Insert(int index, TElement item) => throw Error.NotSupported();

        void IList<TElement>.RemoveAt(int index) => throw Error.NotSupported();

        TElement IList<TElement>.this[int index]
        {
            get
            {
                if (index < 0 || index >= _count)
                {
                    throw Error.ArgumentOutOfRange(nameof(index));
                }

                return _elements[index];
            }

            set => throw Error.NotSupported();
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

        IAsyncEnumerator<TElement> IAsyncEnumerable<TElement>.GetAsyncEnumerator(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested(); // NB: [LDM-2018-11-28] Equivalent to async iterator behavior.

            return this.ToAsyncEnumerable().GetAsyncEnumerator(cancellationToken);
        }
    }
}

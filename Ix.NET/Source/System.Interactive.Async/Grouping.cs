using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.Linq.Internal
{
    internal class Grouping<TKey, TElement> : IGrouping<TKey, TElement>, IList<TElement>
    {
        internal TKey _key;
        internal int _hashCode;
        internal TElement[] _elements;
        internal int _count;
        internal Grouping<TKey, TElement> _hashNext;
        internal Grouping<TKey, TElement> _next;

        internal Grouping()
        {
        }

        internal void Add(TElement element)
        {
            if (_elements.Length == _count)
            {
                Array.Resize(ref _elements, checked(_count * 2));
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

        public IEnumerator<TElement> GetEnumerator()
        {
            for (int i = 0; i < _count; i++)
            {
                yield return _elements[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        // DDB195907: implement IGrouping<>.Key implicitly
        // so that WPF binding works on this property.
        public TKey Key
        {
            get { return _key; }
        }

        int ICollection<TElement>.Count
        {
            get { return _count; }
        }

        bool ICollection<TElement>.IsReadOnly
        {
            get { return true; }
        }

        void ICollection<TElement>.Add(TElement item)
        {
            throw new NotSupportedException(Strings.NOT_SUPPORTED);
        }

        void ICollection<TElement>.Clear()
        {
            throw new NotSupportedException(Strings.NOT_SUPPORTED);
        }

        bool ICollection<TElement>.Contains(TElement item)
        {
            return Array.IndexOf(_elements, item, 0, _count) >= 0;
        }

        void ICollection<TElement>.CopyTo(TElement[] array, int arrayIndex)
        {
            Array.Copy(_elements, 0, array, arrayIndex, _count);
        }

        bool ICollection<TElement>.Remove(TElement item)
        {
            throw new NotSupportedException(Strings.NOT_SUPPORTED);
        }

        int IList<TElement>.IndexOf(TElement item)
        {
            return Array.IndexOf(_elements, item, 0, _count);
        }

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

            set
            {
                throw new NotSupportedException(Strings.NOT_SUPPORTED);
            }
        }
    }


}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace System.Reactive.Disposables
{
    /// <summary>
    /// Represents a group of disposable resources that are disposed together.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "Backward compat + ideally want to get rid of the ICollection nature of the type.")]
    public sealed class CompositeDisposable : ICollection<IDisposable>, ICancelable
    {
        private readonly object _gate = new object();

        private bool _disposed;
        private List<IDisposable> _disposables;
        private int _count;
        private const int SHRINK_THRESHOLD = 64;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeDisposable"/> class with no disposables contained by it initially.
        /// </summary>
        public CompositeDisposable()
        {
            _disposables = new List<IDisposable>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeDisposable"/> class with the specified number of disposables.
        /// </summary>
        /// <param name="capacity">The number of disposables that the new CompositeDisposable can initially store.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="capacity"/> is less than zero.</exception>
        public CompositeDisposable(int capacity)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity));

            _disposables = new List<IDisposable>(capacity);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeDisposable"/> class from a group of disposables.
        /// </summary>
        /// <param name="disposables">Disposables that will be disposed together.</param>
        /// <exception cref="ArgumentNullException"><paramref name="disposables"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Any of the disposables in the <paramref name="disposables"/> collection is <c>null</c>.</exception>
        public CompositeDisposable(params IDisposable[] disposables)
            : this((IEnumerable<IDisposable>)disposables)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeDisposable"/> class from a group of disposables.
        /// </summary>
        /// <param name="disposables">Disposables that will be disposed together.</param>
        /// <exception cref="ArgumentNullException"><paramref name="disposables"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Any of the disposables in the <paramref name="disposables"/> collection is <c>null</c>.</exception>
        public CompositeDisposable(IEnumerable<IDisposable> disposables)
        {
            if (disposables == null)
                throw new ArgumentNullException(nameof(disposables));

            _disposables = new List<IDisposable>(disposables);

            //
            // Doing this on the list to avoid duplicate enumeration of disposables.
            //
            if (_disposables.Contains(null))
                throw new ArgumentException(Strings_Core.DISPOSABLES_CANT_CONTAIN_NULL, nameof(disposables));

            _count = _disposables.Count;
        }

        /// <summary>
        /// Gets the number of disposables contained in the <see cref="CompositeDisposable"/>.
        /// </summary>
        public int Count => _count;

        /// <summary>
        /// Adds a disposable to the <see cref="CompositeDisposable"/> or disposes the disposable if the <see cref="CompositeDisposable"/> is disposed.
        /// </summary>
        /// <param name="item">Disposable to add.</param>
        /// <exception cref="ArgumentNullException"><paramref name="item"/> is <c>null</c>.</exception>
        public void Add(IDisposable item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            var shouldDispose = false;
            lock (_gate)
            {
                shouldDispose = _disposed;
                if (!_disposed)
                {
                    _disposables.Add(item);
                    _count++;
                }
            }

            if (shouldDispose)
            {
                item.Dispose();
            }
        }

        /// <summary>
        /// Removes and disposes the first occurrence of a disposable from the <see cref="CompositeDisposable"/>.
        /// </summary>
        /// <param name="item">Disposable to remove.</param>
        /// <returns>true if found; false otherwise.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="item"/> is <c>null</c>.</exception>
        public bool Remove(IDisposable item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            var shouldDispose = false;

            lock (_gate)
            {
                if (!_disposed)
                {
                    //
                    // List<T> doesn't shrink the size of the underlying array but does collapse the array
                    // by copying the tail one position to the left of the removal index. We don't need
                    // index-based lookup but only ordering for sequential disposal. So, instead of spending
                    // cycles on the Array.Copy imposed by Remove, we use a null sentinel value. We also
                    // do manual Swiss cheese detection to shrink the list if there's a lot of holes in it.
                    //
                    var i = _disposables.IndexOf(item);
                    if (i >= 0)
                    {
                        shouldDispose = true;
                        _disposables[i] = null;
                        _count--;

                        if (_disposables.Capacity > SHRINK_THRESHOLD && _count < _disposables.Capacity / 2)
                        {
                            var old = _disposables;
                            _disposables = new List<IDisposable>(_disposables.Capacity / 2);

                            foreach (var d in old)
                            {
                                if (d != null)
                                {
                                    _disposables.Add(d);
                                }
                            }
                        }
                    }
                }
            }

            if (shouldDispose)
            {
                item.Dispose();
            }

            return shouldDispose;
        }

        /// <summary>
        /// Disposes all disposables in the group and removes them from the group.
        /// </summary>
        public void Dispose()
        {
            var currentDisposables = default(IDisposable[]);
            lock (_gate)
            {
                if (!_disposed)
                {
                    _disposed = true;
                    currentDisposables = _disposables.ToArray();
                    _disposables.Clear();
                    _count = 0;
                }
            }

            if (currentDisposables != null)
            {
                foreach (var d in currentDisposables)
                {
                    d?.Dispose();
                }
            }
        }

        /// <summary>
        /// Removes and disposes all disposables from the <see cref="CompositeDisposable"/>, but does not dispose the <see cref="CompositeDisposable"/>.
        /// </summary>
        public void Clear()
        {
            var currentDisposables = default(IDisposable[]);
            lock (_gate)
            {
                currentDisposables = _disposables.ToArray();
                _disposables.Clear();
                _count = 0;
            }

            foreach (var d in currentDisposables)
            {
                d?.Dispose();
            }
        }

        /// <summary>
        /// Determines whether the <see cref="CompositeDisposable"/> contains a specific disposable.
        /// </summary>
        /// <param name="item">Disposable to search for.</param>
        /// <returns>true if the disposable was found; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="item"/> is <c>null</c>.</exception>
        public bool Contains(IDisposable item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            lock (_gate)
            {
                return _disposables.Contains(item);
            }
        }

        /// <summary>
        /// Copies the disposables contained in the <see cref="CompositeDisposable"/> to an array, starting at a particular array index.
        /// </summary>
        /// <param name="array">Array to copy the contained disposables to.</param>
        /// <param name="arrayIndex">Target index at which to copy the first disposable of the group.</param>
        /// <exception cref="ArgumentNullException"><paramref name="array"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than zero. -or - <paramref name="arrayIndex"/> is larger than or equal to the array length.</exception>
        public void CopyTo(IDisposable[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (arrayIndex < 0 || arrayIndex >= array.Length)
                throw new ArgumentOutOfRangeException(nameof(arrayIndex));

            lock (_gate)
            {
                Array.Copy(_disposables.Where(d => d != null).ToArray(), 0, array, arrayIndex, array.Length - arrayIndex);
            }
        }

        /// <summary>
        /// Always returns false.
        /// </summary>
        public bool IsReadOnly => false;

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="CompositeDisposable"/>.
        /// </summary>
        /// <returns>An enumerator to iterate over the disposables.</returns>
        public IEnumerator<IDisposable> GetEnumerator()
        {
            var res = default(IEnumerable<IDisposable>);

            lock (_gate)
            {
                res = _disposables.Where(d => d != null).ToList();
            }

            return res.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="CompositeDisposable"/>.
        /// </summary>
        /// <returns>An enumerator to iterate over the disposables.</returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Gets a value that indicates whether the object is disposed.
        /// </summary>
        public bool IsDisposed => _disposed;
    }
}

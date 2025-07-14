﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace System.Reactive.Disposables
{
    /// <summary>
    /// Represents a group of disposable resources that are disposed together.
    /// </summary>
    [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "Backward compat + ideally want to get rid of the ICollection nature of the type.")]
    public sealed class CompositeDisposable : ICollection<IDisposable>, ICancelable
    {
        private readonly object _gate = new();
        private bool _disposed;
        private object _disposables;
        private int _count;
        private const int ShrinkThreshold = 64;

        // The maximum number of items to keep in a list before switching to a dictionary.
        // Issue https://github.com/dotnet/reactive/issues/2005 reported that when a SelectMany
        // observes large numbers (1000s) of observables, the CompositeDisposable it uses to
        // keep track of all of the inner observables it creates becomes a bottleneck when the
        // subscription completes.
        private const int MaximumLinearSearchThreshold = 1024;

        // Default initial capacity of the _disposables list in case
        // The number of items is not known upfront
        private const int DefaultCapacity = 16;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeDisposable"/> class with no disposables contained by it initially.
        /// </summary>
        public CompositeDisposable()
        {
            _disposables = new List<IDisposable?>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeDisposable"/> class with the specified number of disposables.
        /// </summary>
        /// <param name="capacity">The number of disposables that the new CompositeDisposable can initially store.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="capacity"/> is less than zero.</exception>
        public CompositeDisposable(int capacity)
        {
            if (capacity < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(capacity));
            }

            _disposables = new List<IDisposable?>(capacity);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeDisposable"/> class from a group of disposables.
        /// </summary>
        /// <param name="disposables">Disposables that will be disposed together.</param>
        /// <exception cref="ArgumentNullException"><paramref name="disposables"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Any of the disposables in the <paramref name="disposables"/> collection is <c>null</c>.</exception>
        public CompositeDisposable(params IDisposable[] disposables)
        {
            if (disposables == null)
            {
                throw new ArgumentNullException(nameof(disposables));
            }

            (_disposables, _) = ToListOrDictionary(disposables);

            // _count can be read by other threads and thus should be properly visible
            // also releases the _disposables contents so it becomes thread-safe
            Volatile.Write(ref _count, disposables.Length);
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
            {
                throw new ArgumentNullException(nameof(disposables));
            }

            (_disposables, var count) = ToListOrDictionary(disposables);

            // _count can be read by other threads and thus should be properly visible
            // also releases the _disposables contents so it becomes thread-safe
            Volatile.Write(ref _count, count);
        }

        private static (object Collection, int Count) ToListOrDictionary(IEnumerable<IDisposable> disposables)
        {
            var capacity = disposables switch
            {
                IDisposable[] a => a.Length,
                ICollection<IDisposable> c => c.Count,
                _ => DefaultCapacity
            };

            if (capacity > MaximumLinearSearchThreshold)
            {
                var dictionary = new Dictionary<IDisposable, int>(capacity);
                var disposableCount = 0;
                foreach (var d in disposables)
                {
                    if (d == null)
                    {
                        throw new ArgumentException(Strings_Core.DISPOSABLES_CANT_CONTAIN_NULL, nameof(disposables));
                    }

                    dictionary.TryGetValue(d, out var thisDisposableCount);
                    dictionary[d] = thisDisposableCount + 1;

                    disposableCount += 1;
                }

                return (dictionary, disposableCount);
            }

            var list = new List<IDisposable?>(capacity);

            // do the copy and null-check in one step to avoid a
            // second loop for just checking for null items
            foreach (var d in disposables)
            {
                if (d == null)
                {
                    throw new ArgumentException(Strings_Core.DISPOSABLES_CANT_CONTAIN_NULL, nameof(disposables));
                }

                list.Add(d);
            }

            if (list.Count > MaximumLinearSearchThreshold)
            {
                // We end up here if we didn't know the count up front because it's an
                // IEnumerable<IDisposable> and not an ICollection<IDisposable>, and it then turns out that
                // the number of items exceeds our maximum tolerance for linear search.
            }

            return (list, list.Count);
        }

        /// <summary>
        /// Gets the number of disposables contained in the <see cref="CompositeDisposable"/>.
        /// </summary>
        public int Count => Volatile.Read(ref _count);

        /// <summary>
        /// Adds a disposable to the <see cref="CompositeDisposable"/> or disposes the disposable if the <see cref="CompositeDisposable"/> is disposed.
        /// </summary>
        /// <param name="item">Disposable to add.</param>
        /// <exception cref="ArgumentNullException"><paramref name="item"/> is <c>null</c>.</exception>
        public void Add(IDisposable item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            lock (_gate)
            {
                if (!_disposed)
                {
                    if (_disposables is List<IDisposable?> listDisposables)
                    {
                        listDisposables.Add(item);

                        // Once we get to thousands of items (which happens with wide fan-out/in configurations)
                        // the cost of linear search becomes too high. We switch to a dictionary at that point.
                        // See https://github.com/dotnet/reactive/issues/2005
                        if (listDisposables.Count > MaximumLinearSearchThreshold)
                        {
                            // If we've blown through this threshold, chances are there's more to come,
                            // so allocate some more spare capacity.
                            var dictionary = new Dictionary<IDisposable, int>(listDisposables.Count + (listDisposables.Count / 4));
                            foreach (var d in listDisposables)
                            {
                                if (d is not null)
                                {
                                    dictionary.TryGetValue(d, out var thisDisposableCount);
                                    dictionary[d] = thisDisposableCount + 1;
                                }
                            }

                            _disposables = dictionary;
                        }

                    }
                    else
                    {
                        var dictionaryDisposables = (Dictionary<IDisposable, int>)_disposables;
                        dictionaryDisposables.TryGetValue(item, out var thisDisposableCount);
                        dictionaryDisposables[item] = thisDisposableCount + 1;
                    }

                    // If read atomically outside the lock, it should be written atomically inside
                    // the plain read on _count is fine here because manipulation always happens
                    // from inside a lock.
                    Volatile.Write(ref _count, _count + 1);
                    return;
                }
            }

            item.Dispose();
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
            {
                throw new ArgumentNullException(nameof(item));
            }

            lock (_gate)
            {
                // this composite was already disposed and if the item was in there
                // it has been already removed/disposed
                if (_disposed)
                {
                    return false;
                }

                //
                // List<T> doesn't shrink the size of the underlying array but does collapse the array
                // by copying the tail one position to the left of the removal index. We don't need
                // index-based lookup but only ordering for sequential disposal. So, instead of spending
                // cycles on the Array.Copy imposed by Remove, we use a null sentinel value. We also
                // do manual Swiss cheese detection to shrink the list if there's a lot of holes in it.
                //

                // read fields as infrequently as possible
                var current = _disposables;

                if (current is List<IDisposable?> currentList)
                {
                    var i = currentList.IndexOf(item);
                    if (i < 0)
                    {
                        // not found, just return
                        return false;
                    }

                    currentList[i] = null;

                    if (currentList.Capacity > ShrinkThreshold && _count < currentList.Capacity / 2)
                    {
                        var fresh = new List<IDisposable?>(currentList.Capacity / 2);

                        foreach (var d in currentList)
                        {
                            if (d != null)
                            {
                                fresh.Add(d);
                            }
                        }

                        _disposables = fresh;
                    } 
                }
                else
                {
                    var dictionaryDisposables = (Dictionary<IDisposable, int>)_disposables;
                    if (!dictionaryDisposables.TryGetValue(item, out var thisDisposableCount))
                    {
                        return false;
                    }

                    thisDisposableCount -= 1;
                    if (thisDisposableCount == 0)
                    {
                        dictionaryDisposables.Remove(item);
                    }
                    else
                    {
                        dictionaryDisposables[item] = thisDisposableCount;
                    }
                }

                // make sure the Count property sees an atomic update
                Volatile.Write(ref _count, _count - 1);
            }

            // if we get here, the item was found and removed from the list
            // just dispose it and report success

            item.Dispose();

            return true;
        }

        /// <summary>
        /// Disposes all disposables in the group and removes them from the group.
        /// </summary>
        public void Dispose()
        {
            List<IDisposable?>? currentDisposablesList = null;
            Dictionary<IDisposable, int>? currentDisposablesDictionary = null;

            lock (_gate)
            {
                if (!_disposed)
                {
                    currentDisposablesList = _disposables as List<IDisposable?>;
                    currentDisposablesDictionary = _disposables as Dictionary<IDisposable, int>;

                    // nulling out the reference is faster no risk to
                    // future Add/Remove because _disposed will be true
                    // and thus _disposables won't be touched again.
                    _disposables = null!; // NB: All accesses are guarded by _disposed checks.

                    Volatile.Write(ref _count, 0);
                    Volatile.Write(ref _disposed, true);
                }
            }

            if (currentDisposablesList is not null)
            {
                foreach (var d in currentDisposablesList)
                {
                    // Although we don't all nulls in from the outside, we implement Remove
                    // by setting entries to null, and shrinking the list if it gets too sparse.
                    // So some entries may be null.
                    d?.Dispose();
                }
            }

            if (currentDisposablesDictionary is not null)
            {
                foreach (var kv in currentDisposablesDictionary)
                {
                    kv.Key.Dispose();
                }
            }
        }

        /// <summary>
        /// Removes and disposes all disposables from the <see cref="CompositeDisposable"/>, but does not dispose the <see cref="CompositeDisposable"/>.
        /// </summary>
        public void Clear()
        {
            IDisposable?[] previousDisposables;

            lock (_gate)
            {
                // disposed composites are always clear
                if (_disposed)
                {
                    return;
                }

                var current = _disposables;

                if (current is List<IDisposable?> currentList)
                {
                    previousDisposables = currentList.ToArray();
                    currentList.Clear();
                }
                else
                {
                    var currentDictionary = (Dictionary<IDisposable, int>)current;
                    previousDisposables = new IDisposable[currentDictionary.Count];
                    currentDictionary.Keys.CopyTo(previousDisposables!, 0);
                    currentDictionary.Clear();
                }

                Volatile.Write(ref _count, 0);
            }

            foreach (var d in previousDisposables)
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
            {
                throw new ArgumentNullException(nameof(item));
            }

            lock (_gate)
            {
                if (_disposed)
                {
                    return false;
                }

                var current = _disposables;
                return current is List<IDisposable?> list
                    ? list.Contains(item)
                    : ((Dictionary<IDisposable, int>) current).ContainsKey(item);
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
            {
                throw new ArgumentNullException(nameof(array));
            }

            if (arrayIndex < 0 || arrayIndex >= array.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(arrayIndex));
            }

            lock (_gate)
            {
                // disposed composites are always empty
                if (_disposed)
                {
                    return;
                }

                if (arrayIndex + _count > array.Length)
                {
                    // there is not enough space beyond arrayIndex 
                    // to accommodate all _count disposables in this composite
                    throw new ArgumentOutOfRangeException(nameof(arrayIndex));
                }
                
                var i = arrayIndex;

                var current = _disposables;

                if (current is List<IDisposable?> currentList)
                {
                    foreach (var d in currentList)
                    {
                        if (d != null)
                        {
                            array[i++] = d;
                        }
                    }
                }
                else
                {
                    foreach (var kv in (Dictionary<IDisposable, int>)current)
                    {
                        for (var j = 0; j < kv.Value; j++)
                        {
                            array[i++] = kv.Key;
                        }
                    }
                }
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
            lock (_gate)
            {
                if (_disposed || _count == 0)
                {
                    return EmptyEnumerator;
                }

                var current = _disposables;

                // the copy is unavoidable but the creation
                // of an outer IEnumerable is avoidable
                return new CompositeEnumerator(current is List<IDisposable?> currentList ? currentList.ToArray() : ((Dictionary<IDisposable, int>)current).Keys.ToArray());
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="CompositeDisposable"/>.
        /// </summary>
        /// <returns>An enumerator to iterate over the disposables.</returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Gets a value that indicates whether the object is disposed.
        /// </summary>
        public bool IsDisposed => Volatile.Read(ref _disposed);

        /// <summary>
        /// An empty enumerator for the <see cref="GetEnumerator"/>
        /// method to avoid allocation on disposed or empty composites.
        /// </summary>
        private static readonly CompositeEnumerator EmptyEnumerator =
            new([]);

        /// <summary>
        /// An enumerator for an array of disposables.
        /// </summary>
        private sealed class CompositeEnumerator : IEnumerator<IDisposable>
        {
            private readonly IDisposable?[] _disposables;
            private int _index;

            public CompositeEnumerator(IDisposable?[] disposables)
            {
                _disposables = disposables;
                _index = -1;
            }

            public IDisposable Current => _disposables[_index]!; // NB: _index is only advanced to non-null positions.

            object IEnumerator.Current => _disposables[_index]!;

            public void Dispose()
            {
                // Avoid retention of the referenced disposables
                // beyond the lifecycle of the enumerator.
                // Not sure if this happens by default to
                // generic array enumerators though.
                var disposables = _disposables;
                Array.Clear(disposables, 0, disposables.Length);
            }

            public bool MoveNext()
            {
                var disposables = _disposables;

                for (; ; )
                {
                    var idx = ++_index;
                    
                    if (idx >= disposables.Length)
                    {
                        return false;
                    }

                    // inlined that filter for null elements
                    if (disposables[idx] != null)
                    {
                        return true;
                    }
                }
            }

            public void Reset()
            {
                _index = -1;
            }
        }
    }
}

﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace System.Reactive
{
    // CONSIDER: Deprecate this functionality or invest in an asynchronous variant.

    /// <summary>
    /// Represents an object that retains the elements of the observable sequence and signals the end of the sequence.
    /// </summary>
    /// <typeparam name="T">The type of elements received from the source sequence.</typeparam>
    [Experimental]
    public class ListObservable<T> : IList<T>, IObservable<object>
    {
        private readonly IDisposable _subscription;
        private readonly AsyncSubject<object> _subject = new();
        private readonly List<T> _results = [];

        /// <summary>
        /// Constructs an object that retains the values of source and signals the end of the sequence.
        /// </summary>
        /// <param name="source">The observable sequence whose elements will be retained in the list.</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is <c>null</c>.</exception>
        public ListObservable(IObservable<T> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            _subscription = source.Subscribe(_results.Add, _subject.OnError, _subject.OnCompleted);
        }

        private void Wait()
        {
            _subject.DefaultIfEmpty().Wait();
        }

        /// <summary>
        /// Returns the last value of the observable sequence.
        /// </summary>
        public T Value
        {
            get
            {
                Wait();

                if (_results.Count == 0)
                {
                    throw new InvalidOperationException(Strings_Linq.NO_ELEMENTS);
                }

                return _results[_results.Count - 1];
            }
        }
        /// <summary>
        /// Determines the index of a specific item in the ListObservable.
        /// </summary>
        /// <param name="item">The element to determine the index for.</param>
        /// <returns>The index of the specified item in the list; -1 if not found.</returns>
        public int IndexOf(T item)
        {
            Wait();
            return _results.IndexOf(item);
        }

        /// <summary>
        /// Inserts an item to the ListObservable at the specified index.
        /// </summary>
        /// <param name="index">The index to insert the item at.</param>
        /// <param name="item">The item to insert in the list.</param>
        public void Insert(int index, T item)
        {
            Wait();
            _results.Insert(index, item);
        }

        /// <summary>
        /// Removes the ListObservable item at the specified index.
        /// </summary>
        /// <param name="index">The index of the item to remove.</param>
        public void RemoveAt(int index)
        {
            Wait();
            _results.RemoveAt(index);
        }

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">The index of the item to retrieve or set.</param>
        public T this[int index]
        {
            get
            {
                Wait();
                return _results[index];
            }
            set
            {
                Wait();
                _results[index] = value;
            }
        }

        /// <summary>
        /// Adds an item to the ListObservable.
        /// </summary>
        /// <param name="item">The item to add to the list.</param>
        public void Add(T item)
        {
            Wait();
            _results.Add(item);
        }

        /// <summary>
        /// Removes all items from the ListObservable.
        /// </summary>
        public void Clear()
        {
            Wait();
            _results.Clear();
        }

        /// <summary>
        /// Determines whether the ListObservable contains a specific value.
        /// </summary>
        /// <param name="item">The item to search for in the list.</param>
        /// <returns>true if found; false otherwise.</returns>
        public bool Contains(T item)
        {
            Wait();
            return _results.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the ListObservable to an System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array">The array to copy elements to.</param>
        /// <param name="arrayIndex">The start index in the array to start copying elements to.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            Wait();
            _results.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Gets the number of elements contained in the ListObservable.
        /// </summary>
        public int Count
        {
            get
            {
                Wait();
                return _results.Count;
            }
        }

        /// <summary>
        /// Gets a value that indicates whether the ListObservable is read-only.
        /// </summary>
        public bool IsReadOnly => false;

        /// <summary>
        /// Removes the first occurrence of a specific object from the ListObservable.
        /// </summary>
        /// <param name="item">The item to remove from the list.</param>
        /// <returns>true if the item was found; false otherwise.</returns>
        public bool Remove(T item)
        {
            Wait();
            return _results.Remove(item);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>Enumerator over the list.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            Wait();
            return _results.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Subscribes an observer to the ListObservable which will be notified upon completion.
        /// </summary>
        /// <param name="observer">The observer to send completion or error messages to.</param>
        /// <returns>The disposable resource that can be used to unsubscribe.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="observer"/> is <c>null</c>.</exception>
        public IDisposable Subscribe(IObserver<object> observer)
        {
            if (observer == null)
            {
                throw new ArgumentNullException(nameof(observer));
            }

            return StableCompositeDisposable.Create(_subscription, _subject.Subscribe(observer));
        }
    }
}

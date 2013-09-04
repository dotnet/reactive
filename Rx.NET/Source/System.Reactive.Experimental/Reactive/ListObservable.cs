// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System.Collections;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace System.Reactive
{
    /// <summary>
    /// Represents an object that retains the elements of the observable sequence and signals the end of the sequence.
    /// </summary>
    /// <typeparam name="T">The type of elements received from the source sequence.</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "By design; Observable suffix takes precedence.")]
    [Experimental]
    public class ListObservable<T> : IList<T>, IObservable<object>
    {
        IDisposable subscription;
        AsyncSubject<object> subject = new AsyncSubject<object>();
        List<T> results = new List<T>();

        /// <summary>
        /// Constructs an object that retains the values of source and signals the end of the sequence.
        /// </summary>
        /// <param name="source">The observable sequence whose elements will be retained in the list.</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        public ListObservable(IObservable<T> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            subscription = source.Subscribe(results.Add, subject.OnError, subject.OnCompleted);
        }

        void Wait()
        {
            subject.DefaultIfEmpty().Wait();
        }

        /// <summary>
        /// Returns the last value of the observable sequence.
        /// </summary>
        public T Value
        {
            get
            {
                Wait();
                if (results.Count == 0)
                    throw new InvalidOperationException(Strings_Linq.NO_ELEMENTS);
                return results[results.Count - 1];
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
            return results.IndexOf(item);
        }

        /// <summary>
        /// Inserts an item to the ListObservable at the specified index.
        /// </summary>
        /// <param name="index">The index to insert the item at.</param>
        /// <param name="item">The item to insert in the list.</param>
        public void Insert(int index, T item)
        {
            Wait();
            results.Insert(index, item);
        }

        /// <summary>
        /// Removes the ListObservable item at the specified index.
        /// </summary>
        /// <param name="index">The index of the item to remove.</param>
        public void RemoveAt(int index)
        {
            Wait();
            results.RemoveAt(index);
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
                return results[index];
            }
            set
            {
                Wait();
                results[index] = value;
            }
        }

        /// <summary>
        /// Adds an item to the ListObservable.
        /// </summary>
        /// <param name="item">The item to add to the list.</param>
        public void Add(T item)
        {
            Wait();
            results.Add(item);
        }

        /// <summary>
        /// Removes all items from the ListObservable.
        /// </summary>
        public void Clear()
        {
            Wait();
            results.Clear();
        }

        /// <summary>
        /// Determines whether the ListObservable contains a specific value.
        /// </summary>
        /// <param name="item">The item to search for in the list.</param>
        /// <returns>true if found; false otherwise.</returns>
        public bool Contains(T item)
        {
            Wait();
            return results.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the ListObservable to an System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array">The array to copy elements to.</param>
        /// <param name="arrayIndex">The start index in the array to start copying elements to.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            Wait();
            results.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Gets the number of elements contained in the ListObservable.
        /// </summary>
        public int Count
        {
            get
            {
                Wait();
                return results.Count;
            }
        }

        /// <summary>
        /// Gets a value that indicates whether the ListObservable is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the ListObservable.
        /// </summary>
        /// <param name="item">The item to remove from the list.</param>
        /// <returns>true if the item was found; false otherwise.</returns>
        public bool Remove(T item)
        {
            Wait();
            return results.Remove(item);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>Enumerator over the list.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            Wait();
            return results.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Subscribes an observer to the ListObservable which will be notified upon completion.
        /// </summary>
        /// <param name="observer">The observer to send completion or error messages to.</param>
        /// <returns>The disposable resource that can be used to unsubscribe.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="observer"/> is null.</exception>
        public IDisposable Subscribe(IObserver<object> observer)
        {
            if (observer == null)
                throw new ArgumentNullException("observer");

            return new CompositeDisposable(subscription, subject.Subscribe(observer));
        }
    }
}
// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Reactive.Concurrency;

#if !NO_TPL
using System.Reactive.Threading.Tasks; // needed for doc comments
using System.Threading;
using System.Threading.Tasks;
#endif

namespace System.Reactive.Linq
{
    public static partial class Observable
    {
        #region + Cast +

        /// <summary>
        /// Converts the elements of an observable sequence to the specified type.
        /// </summary>
        /// <typeparam name="TResult">The type to convert the elements in the source sequence to.</typeparam>
        /// <param name="source">The observable sequence that contains the elements to be converted.</param>
        /// <returns>An observable sequence that contains each element of the source sequence converted to the specified type.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static IObservable<TResult> Cast<TResult>(this IObservable<object> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return s_impl.Cast<TResult>(source);
        }

        #endregion

        #region + DefaultIfEmpty +

        /// <summary>
        /// Returns the elements of the specified sequence or the type parameter's default value in a singleton sequence if the sequence is empty.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence (if any), whose default value will be taken if the sequence is empty.</typeparam>
        /// <param name="source">The sequence to return a default value for if it is empty.</param>
        /// <returns>An observable sequence that contains the default value for the TSource type if the source is empty; otherwise, the elements of the source itself.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static IObservable<TSource> DefaultIfEmpty<TSource>(this IObservable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return s_impl.DefaultIfEmpty<TSource>(source);
        }

        /// <summary>
        /// Returns the elements of the specified sequence or the specified value in a singleton sequence if the sequence is empty.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence (if any), and the specified default value which will be taken if the sequence is empty.</typeparam>
        /// <param name="source">The sequence to return the specified value for if it is empty.</param>
        /// <param name="defaultValue">The value to return if the sequence is empty.</param>
        /// <returns>An observable sequence that contains the specified default value if the source is empty; otherwise, the elements of the source itself.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static IObservable<TSource> DefaultIfEmpty<TSource>(this IObservable<TSource> source, TSource defaultValue)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return s_impl.DefaultIfEmpty<TSource>(source, defaultValue);
        }

        #endregion

        #region + Distinct +

        /// <summary>
        /// Returns an observable sequence that contains only distinct elements.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">An observable sequence to retain distinct elements for.</param>
        /// <returns>An observable sequence only containing the distinct elements from the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <remarks>Usage of this operator should be considered carefully due to the maintenance of an internal lookup structure which can grow large.</remarks>
        public static IObservable<TSource> Distinct<TSource>(this IObservable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return s_impl.Distinct<TSource>(source);
        }

        /// <summary>
        /// Returns an observable sequence that contains only distinct elements according to the comparer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">An observable sequence to retain distinct elements for.</param>
        /// <param name="comparer">Equality comparer for source elements.</param>
        /// <returns>An observable sequence only containing the distinct elements from the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="comparer"/> is null.</exception>
        /// <remarks>Usage of this operator should be considered carefully due to the maintenance of an internal lookup structure which can grow large.</remarks>
        public static IObservable<TSource> Distinct<TSource>(this IObservable<TSource> source, IEqualityComparer<TSource> comparer)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            return s_impl.Distinct<TSource>(source, comparer);
        }

        /// <summary>
        /// Returns an observable sequence that contains only distinct elements according to the keySelector.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">The type of the discriminator key computed for each element in the source sequence.</typeparam>
        /// <param name="source">An observable sequence to retain distinct elements for.</param>
        /// <param name="keySelector">A function to compute the comparison key for each element.</param>
        /// <returns>An observable sequence only containing the distinct elements, based on a computed key value, from the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> is null.</exception>
        /// <remarks>Usage of this operator should be considered carefully due to the maintenance of an internal lookup structure which can grow large.</remarks>
        public static IObservable<TSource> Distinct<TSource, TKey>(this IObservable<TSource> source, Func<TSource, TKey> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");

            return s_impl.Distinct<TSource, TKey>(source, keySelector);
        }

        /// <summary>
        /// Returns an observable sequence that contains only distinct elements according to the keySelector and the comparer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">The type of the discriminator key computed for each element in the source sequence.</typeparam>
        /// <param name="source">An observable sequence to retain distinct elements for.</param>
        /// <param name="keySelector">A function to compute the comparison key for each element.</param>
        /// <param name="comparer">Equality comparer for source elements.</param>
        /// <returns>An observable sequence only containing the distinct elements, based on a computed key value, from the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> or <paramref name="comparer"/> is null.</exception>
        /// <remarks>Usage of this operator should be considered carefully due to the maintenance of an internal lookup structure which can grow large.</remarks>
        public static IObservable<TSource> Distinct<TSource, TKey>(this IObservable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            return s_impl.Distinct<TSource, TKey>(source, keySelector, comparer);
        }

        #endregion

        #region + GroupBy +

        /// <summary>
        /// Groups the elements of an observable sequence according to a specified key selector function.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">The type of the grouping key computed for each element in the source sequence.</typeparam>
        /// <param name="source">An observable sequence whose elements to group.</param>
        /// <param name="keySelector">A function to extract the key for each element.</param>
        /// <returns>A sequence of observable groups, each of which corresponds to a unique key value, containing all elements that share that same key value.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> is null.</exception>
        public static IObservable<IGroupedObservable<TKey, TSource>> GroupBy<TSource, TKey>(this IObservable<TSource> source, Func<TSource, TKey> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");

            return s_impl.GroupBy<TSource, TKey>(source, keySelector);
        }

        /// <summary>
        /// Groups the elements of an observable sequence according to a specified key selector function and comparer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">The type of the grouping key computed for each element in the source sequence.</typeparam>
        /// <param name="source">An observable sequence whose elements to group.</param>
        /// <param name="keySelector">A function to extract the key for each element.</param>
        /// <param name="comparer">An equality comparer to compare keys with.</param>
        /// <returns>A sequence of observable groups, each of which corresponds to a unique key value, containing all elements that share that same key value.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> or <paramref name="comparer"/> is null.</exception>
        public static IObservable<IGroupedObservable<TKey, TSource>> GroupBy<TSource, TKey>(this IObservable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            return s_impl.GroupBy<TSource, TKey>(source, keySelector, comparer);
        }

        /// <summary>
        /// Groups the elements of an observable sequence and selects the resulting elements by using a specified function.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">The type of the grouping key computed for each element in the source sequence.</typeparam>
        /// <typeparam name="TElement">The type of the elements within the groups computed for each element in the source sequence.</typeparam>
        /// <param name="source">An observable sequence whose elements to group.</param>
        /// <param name="keySelector">A function to extract the key for each element.</param>
        /// <param name="elementSelector">A function to map each source element to an element in an observable group.</param>
        /// <returns>A sequence of observable groups, each of which corresponds to a unique key value, containing all elements that share that same key value.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> or <paramref name="elementSelector"/> is null.</exception>
        public static IObservable<IGroupedObservable<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");
            if (elementSelector == null)
                throw new ArgumentNullException("elementSelector");

            return s_impl.GroupBy<TSource, TKey, TElement>(source, keySelector, elementSelector);
        }

        /// <summary>
        /// Groups the elements of an observable sequence according to a specified key selector function and comparer and selects the resulting elements by using a specified function.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">The type of the grouping key computed for each element in the source sequence.</typeparam>
        /// <typeparam name="TElement">The type of the elements within the groups computed for each element in the source sequence.</typeparam>
        /// <param name="source">An observable sequence whose elements to group.</param>
        /// <param name="keySelector">A function to extract the key for each element.</param>
        /// <param name="elementSelector">A function to map each source element to an element in an observable group.</param>
        /// <param name="comparer">An equality comparer to compare keys with.</param>
        /// <returns>A sequence of observable groups, each of which corresponds to a unique key value, containing all elements that share that same key value.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> or <paramref name="elementSelector"/> or <paramref name="comparer"/> is null.</exception>
        public static IObservable<IGroupedObservable<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");
            if (elementSelector == null)
                throw new ArgumentNullException("elementSelector");
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            return s_impl.GroupBy<TSource, TKey, TElement>(source, keySelector, elementSelector, comparer);
        }

        /// <summary>
        /// Groups the elements of an observable sequence with the specified initial capacity according to a specified key selector function.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">The type of the grouping key computed for each element in the source sequence.</typeparam>
        /// <param name="source">An observable sequence whose elements to group.</param>
        /// <param name="keySelector">A function to extract the key for each element.</param>
        /// <param name="capacity">The initial number of elements that the underlying dictionary can contain.</param>
        /// <returns>A sequence of observable groups, each of which corresponds to a unique key value, containing all elements that share that same key value.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="capacity"/> is less than 0.</exception>
        public static IObservable<IGroupedObservable<TKey, TSource>> GroupBy<TSource, TKey>(this IObservable<TSource> source, Func<TSource, TKey> keySelector, int capacity)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");
            if (capacity < 0)
                throw new ArgumentOutOfRangeException("capacity");

            return s_impl.GroupBy<TSource, TKey>(source, keySelector, capacity);
        }

        /// <summary>
        /// Groups the elements of an observable sequence with the specified initial capacity according to a specified key selector function and comparer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">The type of the grouping key computed for each element in the source sequence.</typeparam>
        /// <param name="source">An observable sequence whose elements to group.</param>
        /// <param name="keySelector">A function to extract the key for each element.</param>
        /// <param name="capacity">The initial number of elements that the underlying dictionary can contain.</param>
        /// <param name="comparer">An equality comparer to compare keys with.</param>
        /// <returns>A sequence of observable groups, each of which corresponds to a unique key value, containing all elements that share that same key value.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> or <paramref name="comparer"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="capacity"/> is less than 0.</exception>
        public static IObservable<IGroupedObservable<TKey, TSource>> GroupBy<TSource, TKey>(this IObservable<TSource> source, Func<TSource, TKey> keySelector, int capacity, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");
            if (capacity < 0)
                throw new ArgumentOutOfRangeException("capacity");
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            return s_impl.GroupBy<TSource, TKey>(source, keySelector, capacity, comparer);
        }

        /// <summary>
        /// Groups the elements of an observable sequence with the specified initial capacity and selects the resulting elements by using a specified function.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">The type of the grouping key computed for each element in the source sequence.</typeparam>
        /// <typeparam name="TElement">The type of the elements within the groups computed for each element in the source sequence.</typeparam>
        /// <param name="source">An observable sequence whose elements to group.</param>
        /// <param name="keySelector">A function to extract the key for each element.</param>
        /// <param name="elementSelector">A function to map each source element to an element in an observable group.</param>
        /// <param name="capacity">The initial number of elements that the underlying dictionary can contain.</param>
        /// <returns>A sequence of observable groups, each of which corresponds to a unique key value, containing all elements that share that same key value.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> or <paramref name="elementSelector"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="capacity"/> is less than 0.</exception>
        public static IObservable<IGroupedObservable<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, int capacity)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");
            if (elementSelector == null)
                throw new ArgumentNullException("elementSelector");
            if (capacity < 0)
                throw new ArgumentOutOfRangeException("capacity");

            return s_impl.GroupBy<TSource, TKey, TElement>(source, keySelector, elementSelector, capacity);
        }

        /// <summary>
        /// Groups the elements of an observable sequence with the specified initial capacity according to a specified key selector function and comparer and selects the resulting elements by using a specified function.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">The type of the grouping key computed for each element in the source sequence.</typeparam>
        /// <typeparam name="TElement">The type of the elements within the groups computed for each element in the source sequence.</typeparam>
        /// <param name="source">An observable sequence whose elements to group.</param>
        /// <param name="keySelector">A function to extract the key for each element.</param>
        /// <param name="elementSelector">A function to map each source element to an element in an observable group.</param>
        /// <param name="capacity">The initial number of elements that the underlying dictionary can contain.</param>
        /// <param name="comparer">An equality comparer to compare keys with.</param>
        /// <returns>A sequence of observable groups, each of which corresponds to a unique key value, containing all elements that share that same key value.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> or <paramref name="elementSelector"/> or <paramref name="comparer"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="capacity"/> is less than 0.</exception>
        public static IObservable<IGroupedObservable<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, int capacity, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");
            if (elementSelector == null)
                throw new ArgumentNullException("elementSelector");
            if (capacity < 0)
                throw new ArgumentOutOfRangeException("capacity");
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            return s_impl.GroupBy<TSource, TKey, TElement>(source, keySelector, elementSelector, capacity, comparer);
        }

        #endregion

        #region + GroupByUntil +

        /// <summary>
        /// Groups the elements of an observable sequence according to a specified key selector function and comparer and selects the resulting elements by using a specified function.
        /// A duration selector function is used to control the lifetime of groups. When a group expires, it receives an OnCompleted notification. When a new element with the same
        /// key value as a reclaimed group occurs, the group will be reborn with a new lifetime request.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">The type of the grouping key computed for each element in the source sequence.</typeparam>
        /// <typeparam name="TElement">The type of the elements within the groups computed for each element in the source sequence.</typeparam>
        /// <typeparam name="TDuration">The type of the elements in the duration sequences obtained for each group to denote its lifetime.</typeparam>
        /// <param name="source">An observable sequence whose elements to group.</param>
        /// <param name="keySelector">A function to extract the key for each element.</param>
        /// <param name="elementSelector">A function to map each source element to an element in an observable group.</param>
        /// <param name="durationSelector">A function to signal the expiration of a group.</param>
        /// <param name="comparer">An equality comparer to compare keys with.</param>
        /// <returns>
        /// A sequence of observable groups, each of which corresponds to a unique key value, containing all elements that share that same key value.
        /// If a group's lifetime expires, a new group with the same key value can be created once an element with such a key value is encountered.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> or <paramref name="elementSelector"/> or <paramref name="durationSelector"/> or <paramref name="comparer"/> is null.</exception>
        public static IObservable<IGroupedObservable<TKey, TElement>> GroupByUntil<TSource, TKey, TElement, TDuration>(this IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<IGroupedObservable<TKey, TElement>, IObservable<TDuration>> durationSelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");
            if (elementSelector == null)
                throw new ArgumentNullException("elementSelector");
            if (durationSelector == null)
                throw new ArgumentNullException("durationSelector");
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            return s_impl.GroupByUntil<TSource, TKey, TElement, TDuration>(source, keySelector, elementSelector, durationSelector, comparer);
        }

        /// <summary>
        /// Groups the elements of an observable sequence according to a specified key selector function and selects the resulting elements by using a specified function.
        /// A duration selector function is used to control the lifetime of groups. When a group expires, it receives an OnCompleted notification. When a new element with the same
        /// key value as a reclaimed group occurs, the group will be reborn with a new lifetime request.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">The type of the grouping key computed for each element in the source sequence.</typeparam>
        /// <typeparam name="TElement">The type of the elements within the groups computed for each element in the source sequence.</typeparam>
        /// <typeparam name="TDuration">The type of the elements in the duration sequences obtained for each group to denote its lifetime.</typeparam>
        /// <param name="source">An observable sequence whose elements to group.</param>
        /// <param name="keySelector">A function to extract the key for each element.</param>
        /// <param name="elementSelector">A function to map each source element to an element in an observable group.</param>
        /// <param name="durationSelector">A function to signal the expiration of a group.</param>
        /// <returns>
        /// A sequence of observable groups, each of which corresponds to a unique key value, containing all elements that share that same key value.
        /// If a group's lifetime expires, a new group with the same key value can be created once an element with such a key value is encoutered.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> or <paramref name="elementSelector"/> or <paramref name="durationSelector"/> is null.</exception>
        public static IObservable<IGroupedObservable<TKey, TElement>> GroupByUntil<TSource, TKey, TElement, TDuration>(this IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<IGroupedObservable<TKey, TElement>, IObservable<TDuration>> durationSelector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");
            if (elementSelector == null)
                throw new ArgumentNullException("elementSelector");
            if (durationSelector == null)
                throw new ArgumentNullException("durationSelector");

            return s_impl.GroupByUntil<TSource, TKey, TElement, TDuration>(source, keySelector, elementSelector, durationSelector);
        }

        /// <summary>
        /// Groups the elements of an observable sequence according to a specified key selector function and comparer.
        /// A duration selector function is used to control the lifetime of groups. When a group expires, it receives an OnCompleted notification. When a new element with the same
        /// key value as a reclaimed group occurs, the group will be reborn with a new lifetime request.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">The type of the grouping key computed for each element in the source sequence.</typeparam>
        /// <typeparam name="TDuration">The type of the elements in the duration sequences obtained for each group to denote its lifetime.</typeparam>
        /// <param name="source">An observable sequence whose elements to group.</param>
        /// <param name="keySelector">A function to extract the key for each element.</param>
        /// <param name="durationSelector">A function to signal the expiration of a group.</param>
        /// <param name="comparer">An equality comparer to compare keys with.</param>
        /// <returns>
        /// A sequence of observable groups, each of which corresponds to a unique key value, containing all elements that share that same key value.
        /// If a group's lifetime expires, a new group with the same key value can be created once an element with such a key value is encoutered.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> or <paramref name="durationSelector"/> or <paramref name="comparer"/> is null.</exception>
        public static IObservable<IGroupedObservable<TKey, TSource>> GroupByUntil<TSource, TKey, TDuration>(this IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<IGroupedObservable<TKey, TSource>, IObservable<TDuration>> durationSelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");
            if (durationSelector == null)
                throw new ArgumentNullException("durationSelector");
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            return s_impl.GroupByUntil<TSource, TKey, TDuration>(source, keySelector, durationSelector, comparer);
        }

        /// <summary>
        /// Groups the elements of an observable sequence according to a specified key selector function.
        /// A duration selector function is used to control the lifetime of groups. When a group expires, it receives an OnCompleted notification. When a new element with the same
        /// key value as a reclaimed group occurs, the group will be reborn with a new lifetime request.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">The type of the grouping key computed for each element in the source sequence.</typeparam>
        /// <typeparam name="TDuration">The type of the elements in the duration sequences obtained for each group to denote its lifetime.</typeparam>
        /// <param name="source">An observable sequence whose elements to group.</param>
        /// <param name="keySelector">A function to extract the key for each element.</param>
        /// <param name="durationSelector">A function to signal the expiration of a group.</param>
        /// <returns>
        /// A sequence of observable groups, each of which corresponds to a unique key value, containing all elements that share that same key value.
        /// If a group's lifetime expires, a new group with the same key value can be created once an element with such a key value is encoutered.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> or <paramref name="durationSelector"/> is null.</exception>
        public static IObservable<IGroupedObservable<TKey, TSource>> GroupByUntil<TSource, TKey, TDuration>(this IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<IGroupedObservable<TKey, TSource>, IObservable<TDuration>> durationSelector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");
            if (durationSelector == null)
                throw new ArgumentNullException("durationSelector");

            return s_impl.GroupByUntil<TSource, TKey, TDuration>(source, keySelector, durationSelector);
        }

        /// <summary>
        /// Groups the elements of an observable sequence with the specified initial capacity according to a specified key selector function and comparer and selects the resulting elements by using a specified function.
        /// A duration selector function is used to control the lifetime of groups. When a group expires, it receives an OnCompleted notification. When a new element with the same
        /// key value as a reclaimed group occurs, the group will be reborn with a new lifetime request.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">The type of the grouping key computed for each element in the source sequence.</typeparam>
        /// <typeparam name="TElement">The type of the elements within the groups computed for each element in the source sequence.</typeparam>
        /// <typeparam name="TDuration">The type of the elements in the duration sequences obtained for each group to denote its lifetime.</typeparam>
        /// <param name="source">An observable sequence whose elements to group.</param>
        /// <param name="keySelector">A function to extract the key for each element.</param>
        /// <param name="elementSelector">A function to map each source element to an element in an observable group.</param>
        /// <param name="durationSelector">A function to signal the expiration of a group.</param>
        /// <param name="capacity">The initial number of elements that the underlying dictionary can contain.</param>
        /// <param name="comparer">An equality comparer to compare keys with.</param>
        /// <returns>
        /// A sequence of observable groups, each of which corresponds to a unique key value, containing all elements that share that same key value.
        /// If a group's lifetime expires, a new group with the same key value can be created once an element with such a key value is encountered.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> or <paramref name="elementSelector"/> or <paramref name="durationSelector"/> or <paramref name="comparer"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="capacity"/> is less than 0.</exception>
        public static IObservable<IGroupedObservable<TKey, TElement>> GroupByUntil<TSource, TKey, TElement, TDuration>(this IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<IGroupedObservable<TKey, TElement>, IObservable<TDuration>> durationSelector, int capacity, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");
            if (elementSelector == null)
                throw new ArgumentNullException("elementSelector");
            if (durationSelector == null)
                throw new ArgumentNullException("durationSelector");
            if (capacity < 0)
                throw new ArgumentOutOfRangeException("capacity");
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            return s_impl.GroupByUntil<TSource, TKey, TElement, TDuration>(source, keySelector, elementSelector, durationSelector, capacity, comparer);
        }

        /// <summary>
        /// Groups the elements of an observable sequence with the specified initial capacity according to a specified key selector function and selects the resulting elements by using a specified function.
        /// A duration selector function is used to control the lifetime of groups. When a group expires, it receives an OnCompleted notification. When a new element with the same
        /// key value as a reclaimed group occurs, the group will be reborn with a new lifetime request.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">The type of the grouping key computed for each element in the source sequence.</typeparam>
        /// <typeparam name="TElement">The type of the elements within the groups computed for each element in the source sequence.</typeparam>
        /// <typeparam name="TDuration">The type of the elements in the duration sequences obtained for each group to denote its lifetime.</typeparam>
        /// <param name="source">An observable sequence whose elements to group.</param>
        /// <param name="keySelector">A function to extract the key for each element.</param>
        /// <param name="elementSelector">A function to map each source element to an element in an observable group.</param>
        /// <param name="durationSelector">A function to signal the expiration of a group.</param>
        /// <param name="capacity">The initial number of elements that the underlying dictionary can contain.</param>
        /// <returns>
        /// A sequence of observable groups, each of which corresponds to a unique key value, containing all elements that share that same key value.
        /// If a group's lifetime expires, a new group with the same key value can be created once an element with such a key value is encoutered.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> or <paramref name="elementSelector"/> or <paramref name="durationSelector"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="capacity"/> is less than 0.</exception>
        public static IObservable<IGroupedObservable<TKey, TElement>> GroupByUntil<TSource, TKey, TElement, TDuration>(this IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<IGroupedObservable<TKey, TElement>, IObservable<TDuration>> durationSelector, int capacity)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");
            if (elementSelector == null)
                throw new ArgumentNullException("elementSelector");
            if (durationSelector == null)
                throw new ArgumentNullException("durationSelector");
            if (capacity < 0)
                throw new ArgumentOutOfRangeException("capacity");

            return s_impl.GroupByUntil<TSource, TKey, TElement, TDuration>(source, keySelector, elementSelector, durationSelector, capacity);
        }

        /// <summary>
        /// Groups the elements of an observable sequence with the specified initial capacity according to a specified key selector function and comparer.
        /// A duration selector function is used to control the lifetime of groups. When a group expires, it receives an OnCompleted notification. When a new element with the same
        /// key value as a reclaimed group occurs, the group will be reborn with a new lifetime request.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">The type of the grouping key computed for each element in the source sequence.</typeparam>
        /// <typeparam name="TDuration">The type of the elements in the duration sequences obtained for each group to denote its lifetime.</typeparam>
        /// <param name="source">An observable sequence whose elements to group.</param>
        /// <param name="keySelector">A function to extract the key for each element.</param>
        /// <param name="durationSelector">A function to signal the expiration of a group.</param>
        /// <param name="capacity">The initial number of elements that the underlying dictionary can contain.</param>
        /// <param name="comparer">An equality comparer to compare keys with.</param>
        /// <returns>
        /// A sequence of observable groups, each of which corresponds to a unique key value, containing all elements that share that same key value.
        /// If a group's lifetime expires, a new group with the same key value can be created once an element with such a key value is encoutered.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> or <paramref name="durationSelector"/> or <paramref name="comparer"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="capacity"/> is less than 0.</exception>
        public static IObservable<IGroupedObservable<TKey, TSource>> GroupByUntil<TSource, TKey, TDuration>(this IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<IGroupedObservable<TKey, TSource>, IObservable<TDuration>> durationSelector, int capacity, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");
            if (durationSelector == null)
                throw new ArgumentNullException("durationSelector");
            if (capacity < 0)
                throw new ArgumentOutOfRangeException("capacity");
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            return s_impl.GroupByUntil<TSource, TKey, TDuration>(source, keySelector, durationSelector, capacity, comparer);
        }

        /// <summary>
        /// Groups the elements of an observable sequence with the specified initial capacity according to a specified key selector function.
        /// A duration selector function is used to control the lifetime of groups. When a group expires, it receives an OnCompleted notification. When a new element with the same
        /// key value as a reclaimed group occurs, the group will be reborn with a new lifetime request.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">The type of the grouping key computed for each element in the source sequence.</typeparam>
        /// <typeparam name="TDuration">The type of the elements in the duration sequences obtained for each group to denote its lifetime.</typeparam>
        /// <param name="source">An observable sequence whose elements to group.</param>
        /// <param name="keySelector">A function to extract the key for each element.</param>
        /// <param name="durationSelector">A function to signal the expiration of a group.</param>
        /// <param name="capacity">The initial number of elements that the underlying dictionary can contain.</param>
        /// <returns>
        /// A sequence of observable groups, each of which corresponds to a unique key value, containing all elements that share that same key value.
        /// If a group's lifetime expires, a new group with the same key value can be created once an element with such a key value is encoutered.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> or <paramref name="durationSelector"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="capacity"/> is less than 0.</exception>
        public static IObservable<IGroupedObservable<TKey, TSource>> GroupByUntil<TSource, TKey, TDuration>(this IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<IGroupedObservable<TKey, TSource>, IObservable<TDuration>> durationSelector, int capacity)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");
            if (durationSelector == null)
                throw new ArgumentNullException("durationSelector");
            if (capacity < 0)
                throw new ArgumentOutOfRangeException("capacity");

            return s_impl.GroupByUntil<TSource, TKey, TDuration>(source, keySelector, durationSelector, capacity);
        }

        #endregion

        #region + GroupJoin +

        /// <summary>
        /// Correlates the elements of two sequences based on overlapping durations, and groups the results.
        /// </summary>
        /// <typeparam name="TLeft">The type of the elements in the left source sequence.</typeparam>
        /// <typeparam name="TRight">The type of the elements in the right source sequence.</typeparam>
        /// <typeparam name="TLeftDuration">The type of the elements in the duration sequence denoting the computed duration of each element in the left source sequence.</typeparam>
        /// <typeparam name="TRightDuration">The type of the elements in the duration sequence denoting the computed duration of each element in the right source sequence.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the result sequence, obtained by invoking the result selector function for source elements with overlapping duration.</typeparam>
        /// <param name="left">The left observable sequence to join elements for.</param>
        /// <param name="right">The right observable sequence to join elements for.</param>
        /// <param name="leftDurationSelector">A function to select the duration of each element of the left observable sequence, used to determine overlap.</param>
        /// <param name="rightDurationSelector">A function to select the duration of each element of the right observable sequence, used to determine overlap.</param>
        /// <param name="resultSelector">A function invoked to compute a result element for any element of the left sequence with overlapping elements from the right observable sequence.</param>
        /// <returns>An observable sequence that contains result elements computed from source elements that have an overlapping duration.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> or <paramref name="right"/> or <paramref name="leftDurationSelector"/> or <paramref name="rightDurationSelector"/> or <paramref name="resultSelector"/> is null.</exception>
        public static IObservable<TResult> GroupJoin<TLeft, TRight, TLeftDuration, TRightDuration, TResult>(this IObservable<TLeft> left, IObservable<TRight> right, Func<TLeft, IObservable<TLeftDuration>> leftDurationSelector, Func<TRight, IObservable<TRightDuration>> rightDurationSelector, Func<TLeft, IObservable<TRight>, TResult> resultSelector)
        {
            if (left == null)
                throw new ArgumentNullException("left");
            if (right == null)
                throw new ArgumentNullException("right");
            if (leftDurationSelector == null)
                throw new ArgumentNullException("leftDurationSelector");
            if (rightDurationSelector == null)
                throw new ArgumentNullException("rightDurationSelector");
            if (resultSelector == null)
                throw new ArgumentNullException("resultSelector");

            return s_impl.GroupJoin<TLeft, TRight, TLeftDuration, TRightDuration, TResult>(left, right, leftDurationSelector, rightDurationSelector, resultSelector);
        }

        #endregion

        #region + Join +

        /// <summary>
        /// Correlates the elements of two sequences based on overlapping durations.
        /// </summary>
        /// <typeparam name="TLeft">The type of the elements in the left source sequence.</typeparam>
        /// <typeparam name="TRight">The type of the elements in the right source sequence.</typeparam>
        /// <typeparam name="TLeftDuration">The type of the elements in the duration sequence denoting the computed duration of each element in the left source sequence.</typeparam>
        /// <typeparam name="TRightDuration">The type of the elements in the duration sequence denoting the computed duration of each element in the right source sequence.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the result sequence, obtained by invoking the result selector function for source elements with overlapping duration.</typeparam>
        /// <param name="left">The left observable sequence to join elements for.</param>
        /// <param name="right">The right observable sequence to join elements for.</param>
        /// <param name="leftDurationSelector">A function to select the duration of each element of the left observable sequence, used to determine overlap.</param>
        /// <param name="rightDurationSelector">A function to select the duration of each element of the right observable sequence, used to determine overlap.</param>
        /// <param name="resultSelector">A function invoked to compute a result element for any two overlapping elements of the left and right observable sequences.</param>
        /// <returns>An observable sequence that contains result elements computed from source elements that have an overlapping duration.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> or <paramref name="right"/> or <paramref name="leftDurationSelector"/> or <paramref name="rightDurationSelector"/> or <paramref name="resultSelector"/> is null.</exception>
        public static IObservable<TResult> Join<TLeft, TRight, TLeftDuration, TRightDuration, TResult>(this IObservable<TLeft> left, IObservable<TRight> right, Func<TLeft, IObservable<TLeftDuration>> leftDurationSelector, Func<TRight, IObservable<TRightDuration>> rightDurationSelector, Func<TLeft, TRight, TResult> resultSelector)
        {
            if (left == null)
                throw new ArgumentNullException("left");
            if (right == null)
                throw new ArgumentNullException("right");
            if (leftDurationSelector == null)
                throw new ArgumentNullException("leftDurationSelector");
            if (rightDurationSelector == null)
                throw new ArgumentNullException("rightDurationSelector");
            if (resultSelector == null)
                throw new ArgumentNullException("resultSelector");

            return s_impl.Join<TLeft, TRight, TLeftDuration, TRightDuration, TResult>(left, right, leftDurationSelector, rightDurationSelector, resultSelector);
        }

        #endregion

        #region + OfType +

        /// <summary>
        /// Filters the elements of an observable sequence based on the specified type.
        /// </summary>
        /// <typeparam name="TResult">The type to filter the elements in the source sequence on.</typeparam>
        /// <param name="source">The observable sequence that contains the elements to be filtered.</param>
        /// <returns>An observable sequence that contains elements from the input sequence of type TResult.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static IObservable<TResult> OfType<TResult>(this IObservable<object> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return s_impl.OfType<TResult>(source);
        }

        #endregion

        #region + Select +

        /// <summary>
        /// Projects each element of an observable sequence into a new form.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the result sequence, obtained by running the selector function for each element in the source sequence.</typeparam>
        /// <param name="source">A sequence of elements to invoke a transform function on.</param>
        /// <param name="selector">A transform function to apply to each source element.</param>
        /// <returns>An observable sequence whose elements are the result of invoking the transform function on each element of source.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        public static IObservable<TResult> Select<TSource, TResult>(this IObservable<TSource> source, Func<TSource, TResult> selector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return s_impl.Select<TSource, TResult>(source, selector);
        }

        /// <summary>
        /// Projects each element of an observable sequence into a new form by incorporating the element's index.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the result sequence, obtained by running the selector function for each element in the source sequence.</typeparam>
        /// <param name="source">A sequence of elements to invoke a transform function on.</param>
        /// <param name="selector">A transform function to apply to each source element; the second parameter of the function represents the index of the source element.</param>
        /// <returns>An observable sequence whose elements are the result of invoking the transform function on each element of source.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        public static IObservable<TResult> Select<TSource, TResult>(this IObservable<TSource> source, Func<TSource, int, TResult> selector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return s_impl.Select<TSource, TResult>(source, selector);
        }

        #endregion

        #region + SelectMany +

        /// <summary>
        /// Projects each element of the source observable sequence to the other observable sequence and merges the resulting observable sequences into one observable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TOther">The type of the elements in the other sequence and the elements in the result sequence.</typeparam>
        /// <param name="source">An observable sequence of elements to project.</param>
        /// <param name="other">An observable sequence to project each element from the source sequence onto.</param>
        /// <returns>An observable sequence whose elements are the result of projecting each source element onto the other sequence and merging all the resulting sequences together.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="other"/> is null.</exception>
        public static IObservable<TOther> SelectMany<TSource, TOther>(this IObservable<TSource> source, IObservable<TOther> other)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (other == null)
                throw new ArgumentNullException("other");

            return s_impl.SelectMany<TSource, TOther>(source, other);
        }

        /// <summary>
        /// Projects each element of an observable sequence to an observable sequence and merges the resulting observable sequences into one observable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the projected inner sequences and the elements in the merged result sequence.</typeparam>
        /// <param name="source">An observable sequence of elements to project.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>An observable sequence whose elements are the result of invoking the one-to-many transform function on each element of the input sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        public static IObservable<TResult> SelectMany<TSource, TResult>(this IObservable<TSource> source, Func<TSource, IObservable<TResult>> selector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return s_impl.SelectMany<TSource, TResult>(source, selector);
        }

        /// <summary>
        /// Projects each element of an observable sequence to an observable sequence by incorporating the element's index and merges the resulting observable sequences into one observable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the projected inner sequences and the elements in the merged result sequence.</typeparam>
        /// <param name="source">An observable sequence of elements to project.</param>
        /// <param name="selector">A transform function to apply to each element; the second parameter of the function represents the index of the source element.</param>
        /// <returns>An observable sequence whose elements are the result of invoking the one-to-many transform function on each element of the input sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        public static IObservable<TResult> SelectMany<TSource, TResult>(this IObservable<TSource> source, Func<TSource, int, IObservable<TResult>> selector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return s_impl.SelectMany<TSource, TResult>(source, selector);
        }

#if !NO_TPL
        /// <summary>
        /// Projects each element of an observable sequence to a task and merges all of the task results into one observable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TResult">The type of the result produced by the projected tasks and the elements in the merged result sequence.</typeparam>
        /// <param name="source">An observable sequence of elements to project.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>An observable sequence whose elements are the result of the tasks executed for each element of the input sequence.</returns>
        /// <remarks>This overload supports composition of observable sequences and tasks, without requiring manual conversion of the tasks to observable sequences using <see cref="TaskObservableExtensions.ToObservable&lt;TResult&gt;"/>.</remarks>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        public static IObservable<TResult> SelectMany<TSource, TResult>(this IObservable<TSource> source, Func<TSource, Task<TResult>> selector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return s_impl.SelectMany<TSource, TResult>(source, selector);
        }

        /// <summary>
        /// Projects each element of an observable sequence to a task by incorporating the element's index and merges all of the task results into one observable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TResult">The type of the result produced by the projected tasks and the elements in the merged result sequence.</typeparam>
        /// <param name="source">An observable sequence of elements to project.</param>
        /// <param name="selector">A transform function to apply to each element; the second parameter of the function represents the index of the source element.</param>
        /// <returns>An observable sequence whose elements are the result of the tasks executed for each element of the input sequence.</returns>
        /// <remarks>This overload supports composition of observable sequences and tasks, without requiring manual conversion of the tasks to observable sequences using <see cref="TaskObservableExtensions.ToObservable&lt;TResult&gt;"/>.</remarks>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        public static IObservable<TResult> SelectMany<TSource, TResult>(this IObservable<TSource> source, Func<TSource, int, Task<TResult>> selector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return s_impl.SelectMany<TSource, TResult>(source, selector);
        }

        /// <summary>
        /// Projects each element of an observable sequence to a task with cancellation support and merges all of the task results into one observable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TResult">The type of the result produced by the projected tasks and the elements in the merged result sequence.</typeparam>
        /// <param name="source">An observable sequence of elements to project.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>An observable sequence whose elements are the result of the tasks executed for each element of the input sequence.</returns>
        /// <remarks>This overload supports composition of observable sequences and tasks, without requiring manual conversion of the tasks to observable sequences using <see cref="TaskObservableExtensions.ToObservable&lt;TResult&gt;"/>.</remarks>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        public static IObservable<TResult> SelectMany<TSource, TResult>(this IObservable<TSource> source, Func<TSource, CancellationToken, Task<TResult>> selector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return s_impl.SelectMany<TSource, TResult>(source, selector);
        }

        /// <summary>
        /// Projects each element of an observable sequence to a task by incorporating the element's index with cancellation support and merges all of the task results into one observable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TResult">The type of the result produced by the projected tasks and the elements in the merged result sequence.</typeparam>
        /// <param name="source">An observable sequence of elements to project.</param>
        /// <param name="selector">A transform function to apply to each element; the second parameter of the function represents the index of the source element.</param>
        /// <returns>An observable sequence whose elements are the result of the tasks executed for each element of the input sequence.</returns>
        /// <remarks>This overload supports composition of observable sequences and tasks, without requiring manual conversion of the tasks to observable sequences using <see cref="TaskObservableExtensions.ToObservable&lt;TResult&gt;"/>.</remarks>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        public static IObservable<TResult> SelectMany<TSource, TResult>(this IObservable<TSource> source, Func<TSource, int, CancellationToken, Task<TResult>> selector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return s_impl.SelectMany<TSource, TResult>(source, selector);
        }
#endif

        /// <summary>
        /// Projects each element of an observable sequence to an observable sequence, invokes the result selector for the source element and each of the corresponding inner sequence's elements, and merges the results into one observable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TCollection">The type of the elements in the projected intermediate sequences.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the result sequence, obtained by using the selector to combine source sequence elements with their corresponding intermediate sequence elements.</typeparam>
        /// <param name="source">An observable sequence of elements to project.</param>
        /// <param name="collectionSelector">A transform function to apply to each element.</param>
        /// <param name="resultSelector">A transform function to apply to each element of the intermediate sequence.</param>
        /// <returns>An observable sequence whose elements are the result of invoking the one-to-many transform function collectionSelector on each element of the input sequence and then mapping each of those sequence elements and their corresponding source element to a result element.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="collectionSelector"/> or <paramref name="resultSelector"/> is null.</exception>
        public static IObservable<TResult> SelectMany<TSource, TCollection, TResult>(this IObservable<TSource> source, Func<TSource, IObservable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (collectionSelector == null)
                throw new ArgumentNullException("collectionSelector");
            if (resultSelector == null)
                throw new ArgumentNullException("resultSelector");

            return s_impl.SelectMany<TSource, TCollection, TResult>(source, collectionSelector, resultSelector);
        }

        /// <summary>
        /// Projects each element of an observable sequence to an observable sequence by incorporating the element's index, invokes the result selector for the source element and each of the corresponding inner sequence's elements, and merges the results into one observable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TCollection">The type of the elements in the projected intermediate sequences.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the result sequence, obtained by using the selector to combine source sequence elements with their corresponding intermediate sequence elements.</typeparam>
        /// <param name="source">An observable sequence of elements to project.</param>
        /// <param name="collectionSelector">A transform function to apply to each element; the second parameter of the function represents the index of the source element.</param>
        /// <param name="resultSelector">A transform function to apply to each element of the intermediate sequence; the second parameter of the function represents the index of the source element and the fourth parameter represents the index of the intermediate element.</param>
        /// <returns>An observable sequence whose elements are the result of invoking the one-to-many transform function collectionSelector on each element of the input sequence and then mapping each of those sequence elements and their corresponding source element to a result element.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="collectionSelector"/> or <paramref name="resultSelector"/> is null.</exception>
        public static IObservable<TResult> SelectMany<TSource, TCollection, TResult>(this IObservable<TSource> source, Func<TSource, int, IObservable<TCollection>> collectionSelector, Func<TSource, int, TCollection, int, TResult> resultSelector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (collectionSelector == null)
                throw new ArgumentNullException("collectionSelector");
            if (resultSelector == null)
                throw new ArgumentNullException("resultSelector");

            return s_impl.SelectMany<TSource, TCollection, TResult>(source, collectionSelector, resultSelector);
        }

#if !NO_TPL
        /// <summary>
        /// Projects each element of an observable sequence to a task, invokes the result selector for the source element and the task result, and merges the results into one observable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TTaskResult">The type of the results produced by the projected intermediate tasks.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the result sequence, obtained by using the selector to combine source sequence elements with their corresponding intermediate task results.</typeparam>
        /// <param name="source">An observable sequence of elements to project.</param>
        /// <param name="taskSelector">A transform function to apply to each element.</param>
        /// <param name="resultSelector">A transform function to apply to each element of the intermediate sequence.</param>
        /// <returns>An observable sequence whose elements are the result of obtaining a task for each element of the input sequence and then mapping the task's result and its corresponding source element to a result element.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="taskSelector"/> or <paramref name="resultSelector"/> is null.</exception>
        /// <remarks>This overload supports using LINQ query comprehension syntax in C# and Visual Basic to compose observable sequences and tasks, without requiring manual conversion of the tasks to observable sequences using <see cref="TaskObservableExtensions.ToObservable&lt;TResult&gt;"/>.</remarks>
        public static IObservable<TResult> SelectMany<TSource, TTaskResult, TResult>(this IObservable<TSource> source, Func<TSource, Task<TTaskResult>> taskSelector, Func<TSource, TTaskResult, TResult> resultSelector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (taskSelector == null)
                throw new ArgumentNullException("taskSelector");
            if (resultSelector == null)
                throw new ArgumentNullException("resultSelector");

            return s_impl.SelectMany<TSource, TTaskResult, TResult>(source, taskSelector, resultSelector);
        }

        /// <summary>
        /// Projects each element of an observable sequence to a task by incorporating the element's index, invokes the result selector for the source element and the task result, and merges the results into one observable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TTaskResult">The type of the results produced by the projected intermediate tasks.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the result sequence, obtained by using the selector to combine source sequence elements with their corresponding intermediate task results.</typeparam>
        /// <param name="source">An observable sequence of elements to project.</param>
        /// <param name="taskSelector">A transform function to apply to each element; the second parameter of the function represents the index of the source element.</param>
        /// <param name="resultSelector">A transform function to apply to each element of the intermediate sequence; the second parameter of the function represents the index of the source element.</param>
        /// <returns>An observable sequence whose elements are the result of obtaining a task for each element of the input sequence and then mapping the task's result and its corresponding source element to a result element.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="taskSelector"/> or <paramref name="resultSelector"/> is null.</exception>
        /// <remarks>This overload supports using LINQ query comprehension syntax in C# and Visual Basic to compose observable sequences and tasks, without requiring manual conversion of the tasks to observable sequences using <see cref="TaskObservableExtensions.ToObservable&lt;TResult&gt;"/>.</remarks>
        public static IObservable<TResult> SelectMany<TSource, TTaskResult, TResult>(this IObservable<TSource> source, Func<TSource, int, Task<TTaskResult>> taskSelector, Func<TSource, int, TTaskResult, TResult> resultSelector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (taskSelector == null)
                throw new ArgumentNullException("taskSelector");
            if (resultSelector == null)
                throw new ArgumentNullException("resultSelector");

            return s_impl.SelectMany<TSource, TTaskResult, TResult>(source, taskSelector, resultSelector);
        }

        /// <summary>
        /// Projects each element of an observable sequence to a task with cancellation support, invokes the result selector for the source element and the task result, and merges the results into one observable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TTaskResult">The type of the results produced by the projected intermediate tasks.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the result sequence, obtained by using the selector to combine source sequence elements with their corresponding intermediate task results.</typeparam>
        /// <param name="source">An observable sequence of elements to project.</param>
        /// <param name="taskSelector">A transform function to apply to each element.</param>
        /// <param name="resultSelector">A transform function to apply to each element of the intermediate sequence.</param>
        /// <returns>An observable sequence whose elements are the result of obtaining a task for each element of the input sequence and then mapping the task's result and its corresponding source element to a result element.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="taskSelector"/> or <paramref name="resultSelector"/> is null.</exception>
        /// <remarks>This overload supports using LINQ query comprehension syntax in C# and Visual Basic to compose observable sequences and tasks, without requiring manual conversion of the tasks to observable sequences using <see cref="TaskObservableExtensions.ToObservable&lt;TResult&gt;"/>.</remarks>
        public static IObservable<TResult> SelectMany<TSource, TTaskResult, TResult>(this IObservable<TSource> source, Func<TSource, CancellationToken, Task<TTaskResult>> taskSelector, Func<TSource, TTaskResult, TResult> resultSelector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (taskSelector == null)
                throw new ArgumentNullException("taskSelector");
            if (resultSelector == null)
                throw new ArgumentNullException("resultSelector");

            return s_impl.SelectMany<TSource, TTaskResult, TResult>(source, taskSelector, resultSelector);
        }

        /// <summary>
        /// Projects each element of an observable sequence to a task by incorporating the element's index with cancellation support, invokes the result selector for the source element and the task result, and merges the results into one observable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TTaskResult">The type of the results produced by the projected intermediate tasks.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the result sequence, obtained by using the selector to combine source sequence elements with their corresponding intermediate task results.</typeparam>
        /// <param name="source">An observable sequence of elements to project.</param>
        /// <param name="taskSelector">A transform function to apply to each element; the second parameter of the function represents the index of the source element.</param>
        /// <param name="resultSelector">A transform function to apply to each element of the intermediate sequence; the second parameter of the function represents the index of the source element.</param>
        /// <returns>An observable sequence whose elements are the result of obtaining a task for each element of the input sequence and then mapping the task's result and its corresponding source element to a result element.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="taskSelector"/> or <paramref name="resultSelector"/> is null.</exception>
        /// <remarks>This overload supports using LINQ query comprehension syntax in C# and Visual Basic to compose observable sequences and tasks, without requiring manual conversion of the tasks to observable sequences using <see cref="TaskObservableExtensions.ToObservable&lt;TResult&gt;"/>.</remarks>
        public static IObservable<TResult> SelectMany<TSource, TTaskResult, TResult>(this IObservable<TSource> source, Func<TSource, int, CancellationToken, Task<TTaskResult>> taskSelector, Func<TSource, int, TTaskResult, TResult> resultSelector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (taskSelector == null)
                throw new ArgumentNullException("taskSelector");
            if (resultSelector == null)
                throw new ArgumentNullException("resultSelector");

            return s_impl.SelectMany<TSource, TTaskResult, TResult>(source, taskSelector, resultSelector);
        }
#endif

        /// <summary>
        /// Projects each notification of an observable sequence to an observable sequence and merges the resulting observable sequences into one observable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the projected inner sequences and the elements in the merged result sequence.</typeparam>
        /// <param name="source">An observable sequence of notifications to project.</param>
        /// <param name="onNext">A transform function to apply to each element.</param>
        /// <param name="onError">A transform function to apply when an error occurs in the source sequence.</param>
        /// <param name="onCompleted">A transform function to apply when the end of the source sequence is reached.</param>
        /// <returns>An observable sequence whose elements are the result of invoking the one-to-many transform function corresponding to each notification in the input sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="onNext"/> or <paramref name="onError"/> or <paramref name="onCompleted"/> is null.</exception>
        public static IObservable<TResult> SelectMany<TSource, TResult>(this IObservable<TSource> source, Func<TSource, IObservable<TResult>> onNext, Func<Exception, IObservable<TResult>> onError, Func<IObservable<TResult>> onCompleted)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (onNext == null)
                throw new ArgumentNullException("onNext");
            if (onError == null)
                throw new ArgumentNullException("onError");
            if (onCompleted == null)
                throw new ArgumentNullException("onCompleted");

            return s_impl.SelectMany<TSource, TResult>(source, onNext, onError, onCompleted);
        }

        /// <summary>
        /// Projects each notification of an observable sequence to an observable sequence by incorporating the element's index and merges the resulting observable sequences into one observable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the projected inner sequences and the elements in the merged result sequence.</typeparam>
        /// <param name="source">An observable sequence of notifications to project.</param>
        /// <param name="onNext">A transform function to apply to each element; the second parameter of the function represents the index of the source element.</param>
        /// <param name="onError">A transform function to apply when an error occurs in the source sequence.</param>
        /// <param name="onCompleted">A transform function to apply when the end of the source sequence is reached.</param>
        /// <returns>An observable sequence whose elements are the result of invoking the one-to-many transform function corresponding to each notification in the input sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="onNext"/> or <paramref name="onError"/> or <paramref name="onCompleted"/> is null.</exception>
        public static IObservable<TResult> SelectMany<TSource, TResult>(this IObservable<TSource> source, Func<TSource, int, IObservable<TResult>> onNext, Func<Exception, IObservable<TResult>> onError, Func<IObservable<TResult>> onCompleted)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (onNext == null)
                throw new ArgumentNullException("onNext");
            if (onError == null)
                throw new ArgumentNullException("onError");
            if (onCompleted == null)
                throw new ArgumentNullException("onCompleted");

            return s_impl.SelectMany<TSource, TResult>(source, onNext, onError, onCompleted);
        }

        /// <summary>
        /// Projects each element of an observable sequence to an enumerable sequence and concatenates the resulting enumerable sequences into one observable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the projected inner enumerable sequences and the elements in the merged result sequence.</typeparam>
        /// <param name="source">An observable sequence of elements to project.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>An observable sequence whose elements are the result of invoking the one-to-many transform function on each element of the input sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        /// <remarks>The projected sequences are enumerated synchonously within the OnNext call of the source sequence. In order to do a concurrent, non-blocking merge, change the selector to return an observable sequence obtained using the <see cref="Observable.ToObservable&lt;TSource&gt;(IEnumerable&lt;TSource&gt;)"/> conversion.</remarks>
        public static IObservable<TResult> SelectMany<TSource, TResult>(this IObservable<TSource> source, Func<TSource, IEnumerable<TResult>> selector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return s_impl.SelectMany<TSource, TResult>(source, selector);
        }

        /// <summary>
        /// Projects each element of an observable sequence to an enumerable sequence by incorporating the element's index and concatenates the resulting enumerable sequences into one observable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the projected inner enumerable sequences and the elements in the merged result sequence.</typeparam>
        /// <param name="source">An observable sequence of elements to project.</param>
        /// <param name="selector">A transform function to apply to each element; the second parameter of the function represents the index of the source element.</param>
        /// <returns>An observable sequence whose elements are the result of invoking the one-to-many transform function on each element of the input sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        /// <remarks>The projected sequences are enumerated synchonously within the OnNext call of the source sequence. In order to do a concurrent, non-blocking merge, change the selector to return an observable sequence obtained using the <see cref="Observable.ToObservable&lt;TSource&gt;(IEnumerable&lt;TSource&gt;)"/> conversion.</remarks>
        public static IObservable<TResult> SelectMany<TSource, TResult>(this IObservable<TSource> source, Func<TSource, int, IEnumerable<TResult>> selector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return s_impl.SelectMany<TSource, TResult>(source, selector);
        }

        /// <summary>
        /// Projects each element of an observable sequence to an enumerable sequence, invokes the result selector for the source element and each of the corresponding inner sequence's elements, and merges the results into one observable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TCollection">The type of the elements in the projected intermediate enumerable sequences.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the result sequence, obtained by using the selector to combine source sequence elements with their corresponding intermediate sequence elements.</typeparam>
        /// <param name="source">An observable sequence of elements to project.</param>
        /// <param name="collectionSelector">A transform function to apply to each element.</param>
        /// <param name="resultSelector">A transform function to apply to each element of the intermediate sequence.</param>
        /// <returns>An observable sequence whose elements are the result of invoking the one-to-many transform function collectionSelector on each element of the input sequence and then mapping each of those sequence elements and their corresponding source element to a result element.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="collectionSelector"/> or <paramref name="resultSelector"/> is null.</exception>
        /// <remarks>The projected sequences are enumerated synchonously within the OnNext call of the source sequence. In order to do a concurrent, non-blocking merge, change the selector to return an observable sequence obtained using the <see cref="Observable.ToObservable&lt;TSource&gt;(IEnumerable&lt;TSource&gt;)"/> conversion.</remarks>
        public static IObservable<TResult> SelectMany<TSource, TCollection, TResult>(this IObservable<TSource> source, Func<TSource, IEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (collectionSelector == null)
                throw new ArgumentNullException("collectionSelector");
            if (resultSelector == null)
                throw new ArgumentNullException("resultSelector");

            return s_impl.SelectMany<TSource, TCollection, TResult>(source, collectionSelector, resultSelector);
        }

        /// <summary>
        /// Projects each element of an observable sequence to an enumerable sequence by incorporating the element's index, invokes the result selector for the source element and each of the corresponding inner sequence's elements, and merges the results into one observable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TCollection">The type of the elements in the projected intermediate enumerable sequences.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the result sequence, obtained by using the selector to combine source sequence elements with their corresponding intermediate sequence elements.</typeparam>
        /// <param name="source">An observable sequence of elements to project.</param>
        /// <param name="collectionSelector">A transform function to apply to each element; the second parameter of the function represents the index of the source element.</param>
        /// <param name="resultSelector">A transform function to apply to each element of the intermediate sequence; the second parameter of the function represents the index of the source element and the fourth parameter represents the index of the intermediate element.</param>
        /// <returns>An observable sequence whose elements are the result of invoking the one-to-many transform function collectionSelector on each element of the input sequence and then mapping each of those sequence elements and their corresponding source element to a result element.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="collectionSelector"/> or <paramref name="resultSelector"/> is null.</exception>
        /// <remarks>The projected sequences are enumerated synchonously within the OnNext call of the source sequence. In order to do a concurrent, non-blocking merge, change the selector to return an observable sequence obtained using the <see cref="Observable.ToObservable&lt;TSource&gt;(IEnumerable&lt;TSource&gt;)"/> conversion.</remarks>
        public static IObservable<TResult> SelectMany<TSource, TCollection, TResult>(this IObservable<TSource> source, Func<TSource, int, IEnumerable<TCollection>> collectionSelector, Func<TSource, int, TCollection, int, TResult> resultSelector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (collectionSelector == null)
                throw new ArgumentNullException("collectionSelector");
            if (resultSelector == null)
                throw new ArgumentNullException("resultSelector");

            return s_impl.SelectMany<TSource, TCollection, TResult>(source, collectionSelector, resultSelector);
        }

        #endregion

        #region + Skip +

        /// <summary>
        /// Bypasses a specified number of elements in an observable sequence and then returns the remaining elements.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">The sequence to take elements from.</param>
        /// <param name="count">The number of elements to skip before returning the remaining elements.</param>
        /// <returns>An observable sequence that contains the elements that occur after the specified index in the input sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is less than zero.</exception>
        public static IObservable<TSource> Skip<TSource>(this IObservable<TSource> source, int count)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (count < 0)
                throw new ArgumentOutOfRangeException("count");

            return s_impl.Skip<TSource>(source, count);
        }

        #endregion

        #region + SkipWhile +

        /// <summary>
        /// Bypasses elements in an observable sequence as long as a specified condition is true and then returns the remaining elements.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">An observable sequence to return elements from.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>An observable sequence that contains the elements from the input sequence starting at the first element in the linear series that does not pass the test specified by predicate.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="predicate"/> is null.</exception>
        public static IObservable<TSource> SkipWhile<TSource>(this IObservable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            return s_impl.SkipWhile<TSource>(source, predicate);
        }

        /// <summary>
        /// Bypasses elements in an observable sequence as long as a specified condition is true and then returns the remaining elements.
        /// The element's index is used in the logic of the predicate function.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">An observable sequence to return elements from.</param>
        /// <param name="predicate">A function to test each element for a condition; the second parameter of the function represents the index of the source element.</param>
        /// <returns>An observable sequence that contains the elements from the input sequence starting at the first element in the linear series that does not pass the test specified by predicate.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="predicate"/> is null.</exception>
        public static IObservable<TSource> SkipWhile<TSource>(this IObservable<TSource> source, Func<TSource, int, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            return s_impl.SkipWhile<TSource>(source, predicate);
        }

        #endregion

        #region + Take +

        /// <summary>
        /// Returns a specified number of contiguous elements from the start of an observable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">The sequence to take elements from.</param>
        /// <param name="count">The number of elements to return.</param>
        /// <returns>An observable sequence that contains the specified number of elements from the start of the input sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is less than zero.</exception>
        public static IObservable<TSource> Take<TSource>(this IObservable<TSource> source, int count)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (count < 0)
                throw new ArgumentOutOfRangeException("count");

            return s_impl.Take<TSource>(source, count);
        }

        /// <summary>
        /// Returns a specified number of contiguous elements from the start of an observable sequence, using the specified scheduler for the edge case of Take(0).
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">The sequence to take elements from.</param>
        /// <param name="count">The number of elements to return.</param>
        /// <param name="scheduler">Scheduler used to produce an OnCompleted message in case <paramref name="count">count</paramref> is set to 0.</param>
        /// <returns>An observable sequence that contains the specified number of elements from the start of the input sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="scheduler"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is less than zero.</exception>
        public static IObservable<TSource> Take<TSource>(this IObservable<TSource> source, int count, IScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (count < 0)
                throw new ArgumentOutOfRangeException("count");
            if (scheduler == null)
                throw new ArgumentNullException("scheduler");

            return s_impl.Take<TSource>(source, count, scheduler);
        }

        #endregion

        #region + TakeWhile +

        /// <summary>
        /// Returns elements from an observable sequence as long as a specified condition is true.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">A sequence to return elements from.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>An observable sequence that contains the elements from the input sequence that occur before the element at which the test no longer passes.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="predicate"/> is null.</exception>
        public static IObservable<TSource> TakeWhile<TSource>(this IObservable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            return s_impl.TakeWhile<TSource>(source, predicate);
        }

        /// <summary>
        /// Returns elements from an observable sequence as long as a specified condition is true.
        /// The element's index is used in the logic of the predicate function.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">A sequence to return elements from.</param>
        /// <param name="predicate">A function to test each element for a condition; the second parameter of the function represents the index of the source element.</param>
        /// <returns>An observable sequence that contains the elements from the input sequence that occur before the element at which the test no longer passes.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="predicate"/> is null.</exception>
        public static IObservable<TSource> TakeWhile<TSource>(this IObservable<TSource> source, Func<TSource, int, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            return s_impl.TakeWhile<TSource>(source, predicate);
        }

        #endregion

        #region + Where +

        /// <summary>
        /// Filters the elements of an observable sequence based on a predicate.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">An observable sequence whose elements to filter.</param>
        /// <param name="predicate">A function to test each source element for a condition.</param>
        /// <returns>An observable sequence that contains elements from the input sequence that satisfy the condition.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="predicate"/> is null.</exception>
        public static IObservable<TSource> Where<TSource>(this IObservable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            return s_impl.Where<TSource>(source, predicate);
        }

        /// <summary>
        /// Filters the elements of an observable sequence based on a predicate by incorporating the element's index.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">An observable sequence whose elements to filter.</param>
        /// <param name="predicate">A function to test each source element for a conditio; the second parameter of the function represents the index of the source element.</param>
        /// <returns>An observable sequence that contains elements from the input sequence that satisfy the condition.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="predicate"/> is null.</exception>
        public static IObservable<TSource> Where<TSource>(this IObservable<TSource> source, Func<TSource, int, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            return s_impl.Where<TSource>(source, predicate);
        }

        #endregion
    }
}

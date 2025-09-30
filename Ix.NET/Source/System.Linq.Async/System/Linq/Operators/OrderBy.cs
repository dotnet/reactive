// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
#if INCLUDE_SYSTEM_LINQ_ASYNCENUMERABLE_DUPLICATES
    public static partial class AsyncEnumerable
    {
#if INCLUDE_SYSTEM_LINQ_ASYNCENUMERABLE_DUPLICATES
        // https://learn.microsoft.com/en-us/dotnet/api/system.linq.asyncenumerable.orderby?view=net-9.0-pp#system-linq-asyncenumerable-orderby-2(system-collections-generic-iasyncenumerable((-0))-system-func((-0-1))-system-collections-generic-icomparer((-1)))
        // The method linked to above includes a comparer parameter with a default value of null.

        /// <summary>
        /// Sorts the elements of a sequence in ascending order according to a key.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by keySelector.</typeparam>
        /// <param name="source">An async-enumerable sequence of values to order.</param>
        /// <param name="keySelector">A function to extract a key from an element.</param>
        /// <returns>An ordered async-enumerable sequence whose elements are sorted according to a key.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> is null.</exception>
        public static IOrderedAsyncEnumerable<TSource> OrderBy<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector) =>
            new OrderedAsyncEnumerable<TSource, TKey>(source, keySelector, comparer: null, descending: false, parent: null);
#endif // INCLUDE_SYSTEM_LINQ_ASYNCENUMERABLE_DUPLICATES

        /// <summary>
        /// Sorts the elements of a sequence in ascending order according to a key obtained by invoking a transform function on each element and awaiting the result.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by keySelector.</typeparam>
        /// <param name="source">An async-enumerable sequence of values to order.</param>
        /// <param name="keySelector">An asynchronous function to extract a key from an element.</param>
        /// <returns>An ordered async-enumerable sequence whose elements are sorted according to a key.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> is null.</exception>
        [GenerateAsyncOverload]
        [Obsolete("Use OrderBy. IAsyncEnumerable LINQ is now in System.Linq.AsyncEnumerable, and the OrderByAwait functionality now exists as overloads of OrderBy.")]
        private static IOrderedAsyncEnumerable<TSource> OrderByAwaitCore<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector) =>
            new OrderedAsyncEnumerableWithTask<TSource, TKey>(source, keySelector, comparer: null, descending: false, parent: null);

#if !NO_DEEP_CANCELLATION
        [GenerateAsyncOverload]
        [Obsolete("Use OrderBy. IAsyncEnumerable LINQ is now in System.Linq.AsyncEnumerable, and the OrderByAwaitWithCancellation functionality now exists as overloads of OrderBy.")]
        private static IOrderedAsyncEnumerable<TSource> OrderByAwaitWithCancellationCore<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector) =>
            new OrderedAsyncEnumerableWithTaskAndCancellation<TSource, TKey>(source, keySelector, comparer: null, descending: false, parent: null);
#endif

#if INCLUDE_SYSTEM_LINQ_ASYNCENUMERABLE_DUPLICATES
        // https://learn.microsoft.com/en-us/dotnet/api/system.linq.asyncenumerable.orderby?view=net-9.0-pp#system-linq-asyncenumerable-orderby-2(system-collections-generic-iasyncenumerable((-0))-system-func((-0-1))-system-collections-generic-icomparer((-1)))

        /// <summary>
        /// Sorts the elements of a sequence in ascending order by using a specified comparer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by keySelector.</typeparam>
        /// <param name="source">An async-enumerable sequence of values to order.</param>
        /// <param name="keySelector">A function to extract a key from an element.</param>
        /// <param name="comparer">A comparer to compare keys.</param>
        /// <returns>An ordered async-enumerable sequence whose elements are sorted according to a key.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> is null.</exception>
        public static IOrderedAsyncEnumerable<TSource> OrderBy<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer) =>
            new OrderedAsyncEnumerable<TSource, TKey>(source, keySelector, comparer, descending: false, parent: null);
#endif // INCLUDE_SYSTEM_LINQ_ASYNCENUMERABLE_DUPLICATES

        /// <summary>
        /// Sorts the elements of a sequence in ascending order by using a specified comparer. The keys are obtained by invoking the transform function on each element and awaiting the result.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by keySelector.</typeparam>
        /// <param name="source">An async-enumerable sequence of values to order.</param>
        /// <param name="keySelector">An asynchronous function to extract a key from an element.</param>
        /// <param name="comparer">A comparer to compare keys.</param>
        /// <returns>An ordered async-enumerable sequence whose elements are sorted according to a key.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> is null.</exception>
        [GenerateAsyncOverload]
        [Obsolete("Use OrderBy. IAsyncEnumerable LINQ is now in System.Linq.AsyncEnumerable, and the OrderByAwait functionality now exists as overloads of OrderBy.")]
        private static IOrderedAsyncEnumerable<TSource> OrderByAwaitCore<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, IComparer<TKey> comparer) =>
            new OrderedAsyncEnumerableWithTask<TSource, TKey>(source, keySelector, comparer, descending: false, parent: null);

#if !NO_DEEP_CANCELLATION
        [GenerateAsyncOverload]
        [Obsolete("Use OrderBy. IAsyncEnumerable LINQ is now in System.Linq.AsyncEnumerable, and the OrderByAwaitWithCancellation functionality now exists as overloads of OrderBy.")]
        private static IOrderedAsyncEnumerable<TSource> OrderByAwaitWithCancellationCore<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector, IComparer<TKey> comparer) =>
            new OrderedAsyncEnumerableWithTaskAndCancellation<TSource, TKey>(source, keySelector, comparer, descending: false, parent: null);
#endif

#if INCLUDE_SYSTEM_LINQ_ASYNCENUMERABLE_DUPLICATES
        // https://learn.microsoft.com/en-us/dotnet/api/system.linq.asyncenumerable.orderbydescending?view=net-9.0-pp#system-linq-asyncenumerable-orderbydescending-2(system-collections-generic-iasyncenumerable((-0))-system-func((-0-1))-system-collections-generic-icomparer((-1)))
        // The method linked to above includes a comparer parameter with a default value of null.

        /// <summary>
        /// Sorts the elements of a sequence in descending order according to a key.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by keySelector.</typeparam>
        /// <param name="source">An async-enumerable sequence of values to order.</param>
        /// <param name="keySelector">A function to extract a key from an element.</param>
        /// <returns>An ordered async-enumerable sequence whose elements are sorted in descending order according to a key.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> is null.</exception>
        public static IOrderedAsyncEnumerable<TSource> OrderByDescending<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector) =>
            new OrderedAsyncEnumerable<TSource, TKey>(source, keySelector, comparer: null, descending: true, parent: null);
#endif // INCLUDE_SYSTEM_LINQ_ASYNCENUMERABLE_DUPLICATES

        /// <summary>
        /// Sorts the elements of a sequence in descending order according to a key obtained by invoking a transform function on each element and awaiting the result.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by keySelector.</typeparam>
        /// <param name="source">An async-enumerable sequence of values to order.</param>
        /// <param name="keySelector">An asynchronous function to extract a key from an element.</param>
        /// <returns>An ordered async-enumerable sequence whose elements are sorted in descending order according to a key.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> is null.</exception>
        [GenerateAsyncOverload]
        [Obsolete("Use OrderByDescending. IAsyncEnumerable LINQ is now in System.Linq.AsyncEnumerable, and the OrderByDescendingAwait functionality now exists as overloads of OrderByDescending.")]
        private static IOrderedAsyncEnumerable<TSource> OrderByDescendingAwaitCore<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector) =>
            new OrderedAsyncEnumerableWithTask<TSource, TKey>(source, keySelector, comparer: null, descending: true, parent: null);

#if !NO_DEEP_CANCELLATION
        [GenerateAsyncOverload]
        [Obsolete("Use OrderByDescending. IAsyncEnumerable LINQ is now in System.Linq.AsyncEnumerable, and the OrderByDescendingAwaitWithCancellation functionality now exists as overloads of OrderByDescending.")]
        private static IOrderedAsyncEnumerable<TSource> OrderByDescendingAwaitWithCancellationCore<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector) =>
            new OrderedAsyncEnumerableWithTaskAndCancellation<TSource, TKey>(source, keySelector, comparer: null, descending: true, parent: null);
#endif

#if INCLUDE_SYSTEM_LINQ_ASYNCENUMERABLE_DUPLICATES
        // https://learn.microsoft.com/en-us/dotnet/api/system.linq.asyncenumerable.orderbydescending?view=net-9.0-pp#system-linq-asyncenumerable-orderbydescending-2(system-collections-generic-iasyncenumerable((-0))-system-func((-0-1))-system-collections-generic-icomparer((-1)))

        /// <summary>
        /// Sorts the elements of a sequence in descending order by using a specified comparer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by keySelector.</typeparam>
        /// <param name="source">An async-enumerable sequence of values to order.</param>
        /// <param name="keySelector">A function to extract a key from an element.</param>
        /// <param name="comparer">A comparer to compare keys.</param>
        /// <returns>An ordered async-enumerable sequence whose elements are sorted in descending order according to a key.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> is null.</exception>
        public static IOrderedAsyncEnumerable<TSource> OrderByDescending<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer) =>
            new OrderedAsyncEnumerable<TSource, TKey>(source, keySelector, comparer, descending: true, parent: null);
#endif // INCLUDE_SYSTEM_LINQ_ASYNCENUMERABLE_DUPLICATES

        /// <summary>
        /// Sorts the elements of a sequence in descending order by using a specified comparer. The keys are obtained by invoking the transform function on each element and awaiting the result.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by keySelector.</typeparam>
        /// <param name="source">An async-enumerable sequence of values to order.</param>
        /// <param name="keySelector">An asynchronous function to extract a key from an element.</param>
        /// <param name="comparer">A comparer to compare keys.</param>
        /// <returns>An ordered async-enumerable sequence whose elements are sorted in descending order according to a key.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> is null.</exception>
        [GenerateAsyncOverload]
        [Obsolete("Use OrderByDescending. IAsyncEnumerable LINQ is now in System.Linq.AsyncEnumerable, and the OrderByDescendingAwait functionality now exists as overloads of OrderByDescending.")]
        private static IOrderedAsyncEnumerable<TSource> OrderByDescendingAwaitCore<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, IComparer<TKey> comparer) =>
            new OrderedAsyncEnumerableWithTask<TSource, TKey>(source, keySelector, comparer, descending: true, parent: null);

#if !NO_DEEP_CANCELLATION
        [GenerateAsyncOverload]
        [Obsolete("Use OrderByDescending. IAsyncEnumerable LINQ is now in System.Linq.AsyncEnumerable, and the OrderByDescendingAwaitWithCancellation functionality now exists as overloads of OrderByDescending.")]
        private static IOrderedAsyncEnumerable<TSource> OrderByDescendingAwaitWithCancellationCore<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector, IComparer<TKey> comparer) =>
            new OrderedAsyncEnumerableWithTaskAndCancellation<TSource, TKey>(source, keySelector, comparer, descending: true, parent: null);
#endif

#if INCLUDE_SYSTEM_LINQ_ASYNCENUMERABLE_DUPLICATES
        // https://learn.microsoft.com/en-us/dotnet/api/system.linq.asyncenumerable.thenby?view=net-9.0-pp#system-linq-asyncenumerable-thenby-2(system-linq-iorderedasyncenumerable((-0))-system-func((-0-1))-system-collections-generic-icomparer((-1)))
        // The method linked to above includes a comparer parameter with a default value of null.

        /// <summary>
        /// Performs a subsequent ordering of the elements in a sequence in ascending order according to a key.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by keySelector.</typeparam>
        /// <param name="source">An ordered async-enumerable sequence that contains elements to sort.</param>
        /// <param name="keySelector">A function to extract a key from each element.</param>
        /// <returns>An ordered async-enumerable whose elements are sorted according to a key.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> is null.</exception>
        public static IOrderedAsyncEnumerable<TSource> ThenBy<TSource, TKey>(this IOrderedAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return source.CreateOrderedEnumerable(keySelector, comparer: null, descending: false);
        }
#endif // INCLUDE_SYSTEM_LINQ_ASYNCENUMERABLE_DUPLICATES

        /// <summary>
        /// Performs a subsequent ordering of the elements in a sequence in ascending order according to a key obtained by invoking a transform function on each element and awaiting the result.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by keySelector.</typeparam>
        /// <param name="source">An ordered async-enumerable sequence that contains elements to sort.</param>
        /// <param name="keySelector">An asynchronous function to extract a key from each element.</param>
        /// <returns>An ordered async-enumerable whose elements are sorted according to a key.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> is null.</exception>
        [GenerateAsyncOverload]
        [Obsolete("Use ThenBy. IAsyncEnumerable LINQ is now in System.Linq.AsyncEnumerable, and the ThenByAwait functionality now exists as overloads of ThenBy.")]
        private static IOrderedAsyncEnumerable<TSource> ThenByAwaitCore<TSource, TKey>(this IOrderedAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return source.CreateOrderedEnumerable(keySelector, comparer: default(IComparer<TKey>), descending: false);
        }

#if !NO_DEEP_CANCELLATION
        [GenerateAsyncOverload]
        [Obsolete("Use ThenBy. IAsyncEnumerable LINQ is now in System.Linq.AsyncEnumerable, and the ThenByAwaitWithCancellation functionality now exists as overloads of ThenBy.")]
        private static IOrderedAsyncEnumerable<TSource> ThenByAwaitWithCancellationCore<TSource, TKey>(this IOrderedAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return source.CreateOrderedEnumerable(keySelector, comparer: null, descending: false);
        }
#endif

#if INCLUDE_SYSTEM_LINQ_ASYNCENUMERABLE_DUPLICATES
        // https://learn.microsoft.com/en-us/dotnet/api/system.linq.asyncenumerable.thenby?view=net-9.0-pp#system-linq-asyncenumerable-thenby-2(system-linq-iorderedasyncenumerable((-0))-system-func((-0-1))-system-collections-generic-icomparer((-1)))

        /// <summary>
        /// Performs a subsequent ordering of the elements in a sequence in ascending order by using a specified comparer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by keySelector.</typeparam>
        /// <param name="source">An ordered async-enumerable sequence that contains elements to sort.</param>
        /// <param name="keySelector">A function to extract a key from each element.</param>
        /// <param name="comparer">A comparer to compare keys.</param>
        /// <returns>An ordered async-enumerable whose elements are sorted according to a key.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> is null.</exception>
        public static IOrderedAsyncEnumerable<TSource> ThenBy<TSource, TKey>(this IOrderedAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return source.CreateOrderedEnumerable(keySelector, comparer, descending: false);
        }
#endif // INCLUDE_SYSTEM_LINQ_ASYNCENUMERABLE_DUPLICATES

        /// <summary>
        /// Performs a subsequent ordering of the elements in a sequence in ascending order by using a specified comparer. The keys are obtained by invoking a transform function on each element and awaiting the result.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by keySelector.</typeparam>
        /// <param name="source">An ordered async-enumerable sequence that contains elements to sort.</param>
        /// <param name="keySelector">An asynchronous function to extract a key from each element.</param>
        /// <param name="comparer">A comparer to compare keys.</param>
        /// <returns>An ordered async-enumerable whose elements are sorted according to a key.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> is null.</exception>
        [GenerateAsyncOverload]
        [Obsolete("Use ThenBy. IAsyncEnumerable LINQ is now in System.Linq.AsyncEnumerable, and the ThenByAwait functionality now exists as overloads of ThenBy.")]
        private static IOrderedAsyncEnumerable<TSource> ThenByAwaitCore<TSource, TKey>(this IOrderedAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, IComparer<TKey> comparer)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return source.CreateOrderedEnumerable(keySelector, comparer, descending: false);
        }

#if !NO_DEEP_CANCELLATION
        [GenerateAsyncOverload]
        [Obsolete("Use ThenBy. IAsyncEnumerable LINQ is now in System.Linq.AsyncEnumerable, and the ThenByAwaitWithCancellation functionality now exists as overloads of ThenBy.")]
        private static IOrderedAsyncEnumerable<TSource> ThenByAwaitWithCancellationCore<TSource, TKey>(this IOrderedAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector, IComparer<TKey> comparer)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return source.CreateOrderedEnumerable(keySelector, comparer, descending: false);
        }
#endif

#if INCLUDE_SYSTEM_LINQ_ASYNCENUMERABLE_DUPLICATES
        // https://learn.microsoft.com/en-us/dotnet/api/system.linq.asyncenumerable.thenbydescending?view=net-9.0-pp#system-linq-asyncenumerable-thenbydescending-2(system-linq-iorderedasyncenumerable((-0))-system-func((-0-1))-system-collections-generic-icomparer((-1)))
        // The method linked to above includes a comparer parameter with a default value of null.

        /// <summary>
        /// Performs a subsequent ordering of the elements in a sequence in descending order, according to a key.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by keySelector.</typeparam>
        /// <param name="source">An ordered async-enumerable sequence that contains elements to sort.</param>
        /// <param name="keySelector">A function to extract a key from each element.</param>
        /// <returns>An ordered async-enumerable sequence whose elements are sorted in descending order according to a key.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> is null.</exception>
        public static IOrderedAsyncEnumerable<TSource> ThenByDescending<TSource, TKey>(this IOrderedAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return source.CreateOrderedEnumerable(keySelector, comparer: null, descending: true);
        }
#endif // INCLUDE_SYSTEM_LINQ_ASYNCENUMERABLE_DUPLICATES

        /// <summary>
        /// Performs a subsequent ordering of the elements in a sequence in descending order, according to a key obtained by invoking a transform function on each element and awaiting the result.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by keySelector.</typeparam>
        /// <param name="source">An ordered async-enumerable sequence that contains elements to sort.</param>
        /// <param name="keySelector">An asynchronous function to extract a key from each element.</param>
        /// <returns>An ordered async-enumerable sequence whose elements are sorted in descending order according to a key.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> is null.</exception>
        [GenerateAsyncOverload]
        [Obsolete("Use ThenByDescending. IAsyncEnumerable LINQ is now in System.Linq.AsyncEnumerable, and the ThenByDescendingAwait functionality now exists as overloads of ThenByDescending.")]
        private static IOrderedAsyncEnumerable<TSource> ThenByDescendingAwaitCore<TSource, TKey>(this IOrderedAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return source.CreateOrderedEnumerable(keySelector, comparer: default(IComparer<TKey>), descending: true);
        }

#if !NO_DEEP_CANCELLATION
        [GenerateAsyncOverload]
        [Obsolete("Use ThenByDescending. IAsyncEnumerable LINQ is now in System.Linq.AsyncEnumerable, and the ThenByDescendingAwaitWithCancellation functionality now exists as overloads of ThenByDescending.")]
        private static IOrderedAsyncEnumerable<TSource> ThenByDescendingAwaitWithCancellationCore<TSource, TKey>(this IOrderedAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return source.CreateOrderedEnumerable(keySelector, comparer: null, descending: true);
        }
#endif

#if INCLUDE_SYSTEM_LINQ_ASYNCENUMERABLE_DUPLICATES
        // https://learn.microsoft.com/en-us/dotnet/api/system.linq.asyncenumerable.thenbydescending?view=net-9.0-pp#system-linq-asyncenumerable-thenbydescending-2(system-linq-iorderedasyncenumerable((-0))-system-func((-0-1))-system-collections-generic-icomparer((-1)))

        /// <summary>
        /// Performs a subsequent ordering of the elements in a sequence in descending order by using a specified comparer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by keySelector.</typeparam>
        /// <param name="source">An ordered async-enumerable sequence that contains elements to sort.</param>
        /// <param name="keySelector">A function to extract a key from each element.</param>
        /// <param name="comparer">A comparer to compare keys.</param>
        /// <returns>An ordered async-enumerable sequence whose elements are sorted in descending order according to a key.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> is null.</exception>
        public static IOrderedAsyncEnumerable<TSource> ThenByDescending<TSource, TKey>(this IOrderedAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return source.CreateOrderedEnumerable(keySelector, comparer, descending: true);
        }
#endif // INCLUDE_SYSTEM_LINQ_ASYNCENUMERABLE_DUPLICATES

        /// <summary>
        /// Performs a subsequent ordering of the elements in a sequence in descending order by using a specified comparer. The keys are obtained by invoking a transform function on each element and awaiting the result.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by keySelector.</typeparam>
        /// <param name="source">An ordered async-enumerable sequence that contains elements to sort.</param>
        /// <param name="keySelector">An asynchronous function to extract a key from each element.</param>
        /// <param name="comparer">A comparer to compare keys.</param>
        /// <returns>An ordered async-enumerable sequence whose elements are sorted in descending order according to a key.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> is null.</exception>
        [GenerateAsyncOverload]
        [Obsolete("Use ThenByDescending. IAsyncEnumerable LINQ is now in System.Linq.AsyncEnumerable, and the ThenByDescendingAwait functionality now exists as overloads of ThenByDescending.")]
        private static IOrderedAsyncEnumerable<TSource> ThenByDescendingAwaitCore<TSource, TKey>(this IOrderedAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, IComparer<TKey> comparer)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return source.CreateOrderedEnumerable(keySelector, comparer, descending: true);
        }

#if !NO_DEEP_CANCELLATION
        [GenerateAsyncOverload]
        [Obsolete("Use ThenByDescending. IAsyncEnumerable LINQ is now in System.Linq.AsyncEnumerable, and the ThenByDescendingAwaitWithCancellation functionality now exists as overloads of ThenByDescending.")]
        private static IOrderedAsyncEnumerable<TSource> ThenByDescendingAwaitWithCancellationCore<TSource, TKey>(this IOrderedAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector, IComparer<TKey> comparer)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return source.CreateOrderedEnumerable(keySelector, comparer, descending: true);
        }
#endif
    }
#endif // INCLUDE_SYSTEM_LINQ_ASYNCENUMERABLE_DUPLICATES
}

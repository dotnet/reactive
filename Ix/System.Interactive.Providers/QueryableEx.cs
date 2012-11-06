// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;

namespace System.Linq
{
    /// <summary>
    /// Provides a set of additional static methods that allow querying enumerable sequences.
    /// </summary>
    public static class QueryableEx
    {
        /// <summary>
        /// Determines whether an enumerable sequence is empty.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <returns>true if the sequence is empty; false otherwise.</returns>
        public static bool IsEmpty<TSource>(this IQueryable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return source.Provider.Execute<bool>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression
                )
            );
        }

#pragma warning disable 1591
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static bool IsEmpty<TSource>(IEnumerable<TSource> source)
        {
            return EnumerableEx.IsEmpty(source);
        }
#pragma warning restore 1591

        /// <summary>
        /// Returns the minimum value in the enumerable sequence by using the specified comparer to compare values.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="comparer">Comparer used to determine the minimum value.</param>
        /// <returns>Minimum value in the sequence.</returns>
        public static TSource Min<TSource>(this IQueryable<TSource> source, IComparer<TSource> comparer)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            return source.Provider.Execute<TSource>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression,
                    Expression.Constant(comparer, typeof(IComparer<TSource>))
                )
            );
        }

#pragma warning disable 1591
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static TSource Min<TSource>(IEnumerable<TSource> source, IComparer<TSource> comparer)
        {
            return EnumerableEx.Min(source, comparer);
        }
#pragma warning restore 1591

        /// <summary>
        /// Returns the elements with the minimum key value by using the default comparer to compare key values.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <typeparam name="TKey">Key type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="keySelector">Key selector used to extract the key for each element in the sequence.</param>
        /// <returns>List with the elements that share the same minimum key value.</returns>
        public static IList<TSource> MinBy<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");

            return source.Provider.Execute<IList<TSource>>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)),
                    source.Expression,
                    keySelector
                )
            );
        }

#pragma warning disable 1591
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static IList<TSource> MinBy<TSource, TKey>(IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            return EnumerableEx.MinBy(source, keySelector);
        }
#pragma warning restore 1591

        /// <summary>
        /// Returns the elements with the minimum key value by using the specified comparer to compare key values.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <typeparam name="TKey">Key type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="keySelector">Key selector used to extract the key for each element in the sequence.</param>
        /// <param name="comparer">Comparer used to determine the minimum key value.</param>
        /// <returns>List with the elements that share the same minimum key value.</returns>
        public static IList<TSource> MinBy<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, IComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            return source.Provider.Execute<IList<TSource>>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)),
                    source.Expression,
                    keySelector,
                    Expression.Constant(comparer, typeof(IComparer<TKey>))
                )
            );
        }

#pragma warning disable 1591
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static IList<TSource> MinBy<TSource, TKey>(IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
        {
            return EnumerableEx.MinBy(source, keySelector, comparer);
        }
#pragma warning restore 1591

        /// <summary>
        /// Returns the maximum value in the enumerable sequence by using the specified comparer to compare values.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="comparer">Comparer used to determine the maximum value.</param>
        /// <returns>Maximum value in the sequence.</returns>
        public static TSource Max<TSource>(this IQueryable<TSource> source, IComparer<TSource> comparer)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            return source.Provider.Execute<TSource>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression,
                    Expression.Constant(comparer, typeof(IComparer<TSource>))
                )
            );
        }

#pragma warning disable 1591
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static TSource Max<TSource>(IEnumerable<TSource> source, IComparer<TSource> comparer)
        {
            return EnumerableEx.Max(source, comparer);
        }
#pragma warning restore 1591

        /// <summary>
        /// Returns the elements with the maximum key value by using the default comparer to compare key values.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <typeparam name="TKey">Key type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="keySelector">Key selector used to extract the key for each element in the sequence.</param>
        /// <returns>List with the elements that share the same maximum key value.</returns>
        public static IList<TSource> MaxBy<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");

            return source.Provider.Execute<IList<TSource>>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)),
                    source.Expression,
                    keySelector
                )
            );
        }

#pragma warning disable 1591
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static IList<TSource> MaxBy<TSource, TKey>(IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            return EnumerableEx.MaxBy(source, keySelector);
        }
#pragma warning restore 1591

        /// <summary>
        /// Returns the elements with the minimum key value by using the specified comparer to compare key values.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <typeparam name="TKey">Key type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="keySelector">Key selector used to extract the key for each element in the sequence.</param>
        /// <param name="comparer">Comparer used to determine the maximum key value.</param>
        /// <returns>List with the elements that share the same maximum key value.</returns>
        public static IList<TSource> MaxBy<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, IComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            return source.Provider.Execute<IList<TSource>>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)),
                    source.Expression,
                    keySelector,
                    Expression.Constant(comparer, typeof(IComparer<TKey>))
                )
            );
        }

#pragma warning disable 1591
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static IList<TSource> MaxBy<TSource, TKey>(IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
        {
            return EnumerableEx.MaxBy(source, keySelector, comparer);
        }
#pragma warning restore 1591

        /// <summary>
        /// Shares the source sequence within a selector function where each enumerator can fetch the next element from the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <typeparam name="TResult">Result sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="selector">Selector function with shared access to the source sequence for each enumerator.</param>
        /// <returns>Sequence resulting from applying the selector function to the shared view over the source sequence.</returns>
        public static IQueryable<TResult> Share<TSource, TResult>(this IQueryable<TSource> source, Expression<Func<IEnumerable<TSource>, IEnumerable<TResult>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return source.Provider.CreateQuery<TResult>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TResult)),
                    source.Expression,
                    selector
                )
            );
        }

#pragma warning disable 1591
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static IEnumerable<TResult> Share<TSource, TResult>(IEnumerable<TSource> source, Func<IEnumerable<TSource>, IEnumerable<TResult>> selector)
        {
            return EnumerableEx.Share(source, selector);
        }
#pragma warning restore 1591

        /// <summary>
        /// Publishes the source sequence within a selector function where each enumerator can obtain a view over a tail of the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <typeparam name="TResult">Result sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="selector">Selector function with published access to the source sequence for each enumerator.</param>
        /// <returns>Sequence resulting from applying the selector function to the published view over the source sequence.</returns>
        public static IQueryable<TResult> Publish<TSource, TResult>(this IQueryable<TSource> source, Expression<Func<IEnumerable<TSource>, IEnumerable<TResult>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return source.Provider.CreateQuery<TResult>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TResult)),
                    source.Expression,
                    selector
                )
            );
        }

#pragma warning disable 1591
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static IEnumerable<TResult> Publish<TSource, TResult>(IEnumerable<TSource> source, Func<IEnumerable<TSource>, IEnumerable<TResult>> selector)
        {
            return EnumerableEx.Publish(source, selector);
        }
#pragma warning restore 1591

        /// <summary>
        /// Memoizes the source sequence within a selector function where each enumerator can get access to all of the sequence's elements without causing multiple enumerations over the source.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <typeparam name="TResult">Result sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="selector">Selector function with memoized access to the source sequence for each enumerator.</param>
        /// <returns>Sequence resulting from applying the selector function to the memoized view over the source sequence.</returns>
        public static IQueryable<TResult> Memoize<TSource, TResult>(this IQueryable<TSource> source, Expression<Func<IEnumerable<TSource>, IEnumerable<TResult>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return source.Provider.CreateQuery<TResult>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TResult)),
                    source.Expression,
                    selector
                )
            );
        }

#pragma warning disable 1591
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static IEnumerable<TResult> Memoize<TSource, TResult>(IEnumerable<TSource> source, Func<IEnumerable<TSource>, IEnumerable<TResult>> selector)
        {
            return EnumerableEx.Memoize(source, selector);
        }
#pragma warning restore 1591

        /// <summary>
        /// Memoizes the source sequence within a selector function where a specified number of enumerators can get access to all of the sequence's elements without causing multiple enumerations over the source.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <typeparam name="TResult">Result sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="readerCount">Number of enumerators that can access the underlying buffer. Once every enumerator has obtained an element from the buffer, the element is removed from the buffer.</param>
        /// <param name="selector">Selector function with memoized access to the source sequence for a specified number of enumerators.</param>
        /// <returns>Sequence resulting from applying the selector function to the memoized view over the source sequence.</returns>
        public static IQueryable<TResult> Memoize<TSource, TResult>(this IQueryable<TSource> source, int readerCount, Expression<Func<IEnumerable<TSource>, IEnumerable<TResult>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return source.Provider.CreateQuery<TResult>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TResult)),
                    source.Expression,
                    Expression.Constant(readerCount, typeof(int)),
                    selector
                )
            );
        }

#pragma warning disable 1591
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static IEnumerable<TResult> Memoize<TSource, TResult>(IEnumerable<TSource> source, int readerCount, Func<IEnumerable<TSource>, IEnumerable<TResult>> selector)
        {
            return EnumerableEx.Memoize(source, readerCount, selector);
        }
#pragma warning restore 1591

        /// <summary>
        /// Creates an enumerable sequence based on an enumerator factory function.
        /// </summary>
        /// <typeparam name="TResult">Result sequence element type.</typeparam>
        /// <param name="provider">Query provider.</param>
        /// <param name="getEnumerator">Enumerator factory function.</param>
        /// <returns>Sequence that will invoke the enumerator factory upon a call to GetEnumerator.</returns>
        public static IQueryable<TResult> Create<TResult>(this IQueryProvider provider, Expression<Func<IEnumerator<TResult>>> getEnumerator)
        {
            if (provider == null)
                throw new ArgumentNullException("provider");
            if (getEnumerator == null)
                throw new ArgumentNullException("getEnumerator");

            return provider.CreateQuery<TResult>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TResult)),
                    Expression.Constant(provider, typeof(IQueryProvider)),
                    getEnumerator
                )
            );
        }

#pragma warning disable 1591
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static IEnumerable<TResult> Create<TResult>(Func<IEnumerator<TResult>> getEnumerator)
        {
            return EnumerableEx.Create(getEnumerator);
        }
#pragma warning restore 1591

        /// <summary>
        /// Returns a sequence with a single element.
        /// </summary>
        /// <typeparam name="TResult">Result sequence element type.</typeparam>
        /// <param name="provider">Query provider.</param>
        /// <param name="value">Single element of the resulting sequence.</param>
        /// <returns>Sequence with a single element.</returns>
        public static IQueryable<TResult> Return<TResult>(this IQueryProvider provider, TResult value)
        {
            if (provider == null)
                throw new ArgumentNullException("provider");

            return provider.CreateQuery<TResult>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TResult)),
                    Expression.Constant(provider, typeof(IQueryProvider)),
                    Expression.Constant(value, typeof(TResult))
                )
            );
        }

#pragma warning disable 1591
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static /*!*/IQueryable<TResult> Return<TResult>(TResult value)
        {
            return EnumerableEx.Return(value).AsQueryable();
        }
#pragma warning restore 1591

        /// <summary>
        /// Returns a sequence that throws an exception upon enumeration.
        /// </summary>
        /// <typeparam name="TResult">Result sequence element type.</typeparam>
        /// <param name="provider">Query provider.</param>
        /// <param name="exception">Exception to throw upon enumerating the resulting sequence.</param>
        /// <returns>Sequence that throws the specified exception upon enumeration.</returns>
        public static IQueryable<TResult> Throw<TResult>(this IQueryProvider provider, Exception exception)
        {
            if (provider == null)
                throw new ArgumentNullException("provider");
            if (exception == null)
                throw new ArgumentNullException("exception");

            return provider.CreateQuery<TResult>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TResult)),
                    Expression.Constant(provider, typeof(IQueryProvider)),
                    Expression.Constant(exception, typeof(Exception))
                )
            );
        }

#pragma warning disable 1591
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static /*!*/IQueryable<TResult> Throw<TResult>(Exception exception)
        {
            return EnumerableEx.Throw<TResult>(exception).AsQueryable();
        }
#pragma warning restore 1591

        /// <summary>
        /// Creates an enumerable sequence based on an enumerable factory function.
        /// </summary>
        /// <typeparam name="TResult">Result sequence element type.</typeparam>
        /// <param name="provider">Query provider.</param>
        /// <param name="enumerableFactory">Enumerable factory function.</param>
        /// <returns>Sequence that will invoke the enumerable factory upon a call to GetEnumerator.</returns>
        public static IQueryable<TResult> Defer<TResult>(this IQueryProvider provider, Expression<Func<IEnumerable<TResult>>> enumerableFactory)
        {
            if (provider == null)
                throw new ArgumentNullException("provider");
            if (enumerableFactory == null)
                throw new ArgumentNullException("enumerableFactory");

            return provider.CreateQuery<TResult>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TResult)),
                    Expression.Constant(provider, typeof(IQueryProvider)),
                    enumerableFactory
                )
            );
        }

#pragma warning disable 1591
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static /*!*/IQueryable<TResult> Defer<TResult>(Func<IEnumerable<TResult>> enumerableFactory)
        {
            return EnumerableEx.Defer(enumerableFactory).AsQueryable();
        }
#pragma warning restore 1591

        /// <summary>
        /// Generates a sequence by mimicking a for loop.
        /// </summary>
        /// <typeparam name="TState">State type.</typeparam>
        /// <typeparam name="TResult">Result sequence element type.</typeparam>
        /// <param name="provider">Query provider.</param>
        /// <param name="initialState">Initial state of the generator loop.</param>
        /// <param name="condition">Loop condition.</param>
        /// <param name="iterate">State update function to run after every iteration of the generator loop.</param>
        /// <param name="resultSelector">Result selector to compute resulting sequence elements.</param>
        /// <returns>Sequence obtained by running the generator loop, yielding computed elements.</returns>
        public static IQueryable<TResult> Generate<TState, TResult>(this IQueryProvider provider, TState initialState, Expression<Func<TState, bool>> condition, Expression<Func<TState, TState>> iterate, Expression<Func<TState, TResult>> resultSelector)
        {
            if (provider == null)
                throw new ArgumentNullException("provider");
            if (condition == null)
                throw new ArgumentNullException("condition");
            if (iterate == null)
                throw new ArgumentNullException("iterate");
            if (resultSelector == null)
                throw new ArgumentNullException("resultSelector");

            return provider.CreateQuery<TResult>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TState), typeof(TResult)),
                    Expression.Constant(provider, typeof(IQueryProvider)),
                    Expression.Constant(initialState),
                    condition,
                    iterate,
                    resultSelector
                )
            );
        }

#pragma warning disable 1591
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static /*!*/IQueryable<TResult> Generate<TState, TResult>(TState initialState, Func<TState, bool> condition, Func<TState, TState> iterate, Func<TState, TResult> resultSelector)
        {
            return EnumerableEx.Generate(initialState, condition, iterate, resultSelector).AsQueryable();
        }
#pragma warning restore 1591

        /// <summary>
        /// Generates a sequence that's dependent on a resource object whose lifetime is determined by the sequence usage duration.
        /// </summary>
        /// <typeparam name="TSource">Source element type.</typeparam>
        /// <typeparam name="TResource">Resource type.</typeparam>
        /// <param name="provider">Query provider.</param>
        /// <param name="resourceFactory">Resource factory function.</param>
        /// <param name="enumerableFactory">Enumerable factory function, having access to the obtained resource.</param>
        /// <returns>Sequence whose use controls the lifetime of the associated obtained resource.</returns>
        public static IQueryable<TSource> Using<TSource, TResource>(this IQueryProvider provider, Expression<Func<TResource>> resourceFactory, Expression<Func<TResource, IEnumerable<TSource>>> enumerableFactory) where TResource : IDisposable
        {
            if (provider == null)
                throw new ArgumentNullException("provider");
            if (resourceFactory == null)
                throw new ArgumentNullException("resourceFactory");
            if (enumerableFactory == null)
                throw new ArgumentNullException("enumerableFactory");

            return provider.CreateQuery<TSource>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TResource)),
                    Expression.Constant(provider, typeof(IQueryProvider)),
                    resourceFactory,
                    enumerableFactory
                )
            );
        }

#pragma warning disable 1591
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static /*!*/IQueryable<TSource> Using<TSource, TResource>(Func<TResource> resourceFactory, Func<TResource, IEnumerable<TSource>> enumerableFactory) where TResource : IDisposable
        {
            return EnumerableEx.Using(resourceFactory, enumerableFactory).AsQueryable();
        }
#pragma warning restore 1591

        /// <summary>
        /// Generates a sequence by repeating the given value infinitely.
        /// </summary>
        /// <typeparam name="TResult">Result sequence element type.</typeparam>
        /// <param name="provider">Query provider.</param>
        /// <param name="value">Value to repreat in the resulting sequence.</param>
        /// <returns>Sequence repeating the given value infinitely.</returns>
        public static IEnumerable<TResult> Repeat<TResult>(this IQueryProvider provider, TResult value)
        {
            if (provider == null)
                throw new ArgumentNullException("provider");

            return provider.CreateQuery<TResult>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TResult)),
                    Expression.Constant(provider, typeof(IQueryProvider)),
                    Expression.Constant(value, typeof(TResult))
                )
            );
        }

#pragma warning disable 1591
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static /*!*/IQueryable<TResult> Repeat<TResult>(TResult value)
        {
            return EnumerableEx.Repeat(value).AsQueryable();
        }
#pragma warning restore 1591

        /// <summary>
        /// Creates a sequence that corresponds to the source sequence, concatenating it with the sequence resulting from calling an exception handler function in case of an error.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <typeparam name="TException">Exception type to catch.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="handler">Handler to invoke when an exception of the specified type occurs.</param>
        /// <returns>Source sequence, concatenated with an exception handler result sequence in case of an error.</returns>
        public static IQueryable<TSource> Catch<TSource, TException>(this IQueryable<TSource> source, Expression<Func<TException, IEnumerable<TSource>>> handler)
            where TException : Exception
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (handler == null)
                throw new ArgumentNullException("handler");

            return source.Provider.CreateQuery<TSource>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TException)),
                    source.Expression,
                    handler
                )
            );
        }

#pragma warning disable 1591
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static IEnumerable<TSource> Catch<TSource, TException>(IEnumerable<TSource> source, Func<TException, IEnumerable<TSource>> handler)
            where TException : Exception
        {
            return EnumerableEx.Catch(source, handler);
        }
#pragma warning restore 1591

        /// <summary>
        /// Creates a sequence by concatenating source sequences until a source sequence completes successfully.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="sources">Source sequences.</param>
        /// <returns>Sequence that continues to concatenate source sequences while errors occur.</returns>
        public static IQueryable<TSource> Catch<TSource>(this IQueryable<IEnumerable<TSource>> sources)
        {
            if (sources == null)
                throw new ArgumentNullException("sources");

            return sources.Provider.CreateQuery<TSource>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    sources.Expression
                )
            );
        }

#pragma warning disable 1591
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static IEnumerable<TSource> Catch<TSource>(IEnumerable<IEnumerable<TSource>> sources)
        {
            return EnumerableEx.Catch(sources);
        }
#pragma warning restore 1591

        /// <summary>
        /// Creates a sequence by concatenating source sequences until a source sequence completes successfully.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="provider">Query provider.</param>
        /// <param name="sources">Source sequences.</param>
        /// <returns>Sequence that continues to concatenate source sequences while errors occur.</returns>
        public static IQueryable<TSource> Catch<TSource>(this IQueryProvider provider, params IEnumerable<TSource>[] sources)
        {
            if (provider == null)
                throw new ArgumentNullException("provider");
            if (sources == null)
                throw new ArgumentNullException("sources");

            return provider.CreateQuery<TSource>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    Expression.Constant(provider, typeof(IQueryProvider)),
                    GetSourceExpression(sources)
                )
            );
        }

#pragma warning disable 1591
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static /*!*/IQueryable<TSource> Catch<TSource>(params IEnumerable<TSource>[] sources)
        {
            return EnumerableEx.Catch(sources).AsQueryable();
        }
#pragma warning restore 1591

        /// <summary>
        /// Creates a sequence that returns the elements of the first sequence, switching to the second in case of an error.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="first">First sequence.</param>
        /// <param name="second">Second sequence, concatenated to the result in case the first sequence completes exceptionally.</param>
        /// <returns>The first sequence, followed by the second sequence in case an error is produced.</returns>
        public static IQueryable<TSource> Catch<TSource>(this IQueryable<TSource> first, IEnumerable<TSource> second)
        {
            if (first == null)
                throw new ArgumentNullException("first");
            if (second == null)
                throw new ArgumentNullException("second");

            return first.Provider.CreateQuery<TSource>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    first.Expression,
                    GetSourceExpression(second)
                )
            );
        }

#pragma warning disable 1591
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static IEnumerable<TSource> Catch<TSource>(IEnumerable<TSource> first, IEnumerable<TSource> second)
        {
            return EnumerableEx.Catch(first, second);
        }
#pragma warning restore 1591

        /// <summary>
        /// Creates a sequence whose termination or disposal of an enumerator causes a finally action to be executed.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="finallyAction">Action to run upon termination of the sequence, or when an enumerator is disposed.</param>
        /// <returns>Source sequence with guarantees on the invocation of the finally action.</returns>
        public static IQueryable<TSource> Finally<TSource>(this IQueryable<TSource> source, Expression<Action> finallyAction)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (finallyAction == null)
                throw new ArgumentNullException("finallyAction");

            return source.Provider.CreateQuery<TSource>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression,
                    finallyAction
                )
            );
        }

#pragma warning disable 1591
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static IEnumerable<TSource> Finally<TSource>(IEnumerable<TSource> source, Action finallyAction)
        {
            return EnumerableEx.Finally(source, finallyAction);
        }
#pragma warning restore 1591

        /// <summary>
        /// Creates a sequence that concatenates both given sequences, regardless of whether an error occurs.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="first">First sequence.</param>
        /// <param name="second">Second sequence.</param>
        /// <returns>Sequence concatenating the elements of both sequences, ignoring errors.</returns>
        public static IQueryable<TSource> OnErrorResumeNext<TSource>(this IQueryable<TSource> first, IEnumerable<TSource> second)
        {
            if (first == null)
                throw new ArgumentNullException("first");
            if (second == null)
                throw new ArgumentNullException("second");

            return first.Provider.CreateQuery<TSource>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    first.Expression,
                    GetSourceExpression(second)
                )
            );
        }

#pragma warning disable 1591
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static IEnumerable<TSource> OnErrorResumeNext<TSource>(IEnumerable<TSource> first, IEnumerable<TSource> second)
        {
            return EnumerableEx.OnErrorResumeNext(first, second);
        }
#pragma warning restore 1591

        /// <summary>
        /// Creates a sequence that concatenates the given sequences, regardless of whether an error occurs in any of the sequences.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="provider">Query provider.</param>
        /// <param name="sources">Source sequences.</param>
        /// <returns>Sequence concatenating the elements of the given sequences, ignoring errors.</returns>
        public static IEnumerable<TSource> OnErrorResumeNext<TSource>(this IQueryProvider provider, params IEnumerable<TSource>[] sources)
        {
            if (provider == null)
                throw new ArgumentNullException("provider");
            if (sources == null)
                throw new ArgumentNullException("sources");

            return provider.CreateQuery<TSource>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    Expression.Constant(provider, typeof(IQueryProvider)),
                    GetSourceExpression(sources)
                )
            );
        }

#pragma warning disable 1591
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static /*!*/IQueryable<TSource> OnErrorResumeNext<TSource>(params IEnumerable<TSource>[] sources)
        {
            return EnumerableEx.OnErrorResumeNext(sources).AsQueryable();
        }
#pragma warning restore 1591

        /// <summary>
        /// Creates a sequence that concatenates the given sequences, regardless of whether an error occurs in any of the sequences.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="sources">Source sequences.</param>
        /// <returns>Sequence concatenating the elements of the given sequences, ignoring errors.</returns>
        public static IQueryable<TSource> OnErrorResumeNext<TSource>(this IQueryable<IEnumerable<TSource>> sources)
        {
            if (sources == null)
                throw new ArgumentNullException("sources");

            return sources.Provider.CreateQuery<TSource>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    sources.Expression
                )
            );
        }

#pragma warning disable 1591
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static IEnumerable<TSource> OnErrorResumeNext<TSource>(IEnumerable<IEnumerable<TSource>> sources)
        {
            return EnumerableEx.OnErrorResumeNext(sources);
        }
#pragma warning restore 1591

        /// <summary>
        /// Creates a sequence that retries enumerating the source sequence as long as an error occurs.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <returns>Sequence concatenating the results of the source sequence as long as an error occurs.</returns>
        public static IQueryable<TSource> Retry<TSource>(this IQueryable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return source.Provider.CreateQuery<TSource>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression
                )
            );
        }

#pragma warning disable 1591
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static IEnumerable<TSource> Retry<TSource>(IEnumerable<TSource> source)
        {
            return EnumerableEx.Retry(source);
        }
#pragma warning restore 1591

        /// <summary>
        /// Creates a sequence that retries enumerating the source sequence as long as an error occurs, with the specified maximum number of retries.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="retryCount">Maximum number of retries.</param>
        /// <returns>Sequence concatenating the results of the source sequence as long as an error occurs.</returns>
        public static IQueryable<TSource> Retry<TSource>(this IQueryable<TSource> source, int retryCount)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return source.Provider.CreateQuery<TSource>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression,
                    Expression.Constant(retryCount, typeof(int))
                )
            );
        }

#pragma warning disable 1591
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static IEnumerable<TSource> Retry<TSource>(IEnumerable<TSource> source, int retryCount)
        {
            return EnumerableEx.Retry(source, retryCount);
        }
#pragma warning restore 1591

        /// <summary>
        /// Generates an enumerable sequence by repeating a source sequence as long as the given loop condition holds.
        /// </summary>
        /// <typeparam name="TResult">Result sequence element type.</typeparam>
        /// <param name="provider">Query provider.</param>
        /// <param name="condition">Loop condition.</param>
        /// <param name="source">Sequence to repeat while the condition evaluates true.</param>
        /// <returns>Sequence generated by repeating the given sequence while the condition evaluates to true.</returns>
        public static IQueryable<TResult> While<TResult>(this IQueryProvider provider, Expression<Func<bool>> condition, IEnumerable<TResult> source)
        {
            if (provider == null)
                throw new ArgumentNullException("provider");
            if (condition == null)
                throw new ArgumentNullException("condition");
            if (source == null)
                throw new ArgumentNullException("source");

            return provider.CreateQuery<TResult>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TResult)),
                    Expression.Constant(provider, typeof(IQueryProvider)),
                    condition,
                    GetSourceExpression(source)
                )
            );
        }

#pragma warning disable 1591
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static /*!*/IQueryable<TResult> While<TResult>(Func<bool> condition, IEnumerable<TResult> source)
        {
            return EnumerableEx.While(condition, source).AsQueryable();
        }
#pragma warning restore 1591

        /// <summary>
        /// Returns an enumerable sequence based on the evaluation result of the given condition.
        /// </summary>
        /// <typeparam name="TResult">Result sequence element type.</typeparam>
        /// <param name="provider">Query provider.</param>
        /// <param name="condition">Condition to evaluate.</param>
        /// <param name="thenSource">Sequence to return in case the condition evaluates true.</param>
        /// <param name="elseSource">Sequence to return in case the condition evaluates false.</param>
        /// <returns>Either of the two input sequences based on the result of evaluating the condition.</returns>
        public static IQueryable<TResult> If<TResult>(this IQueryProvider provider, Expression<Func<bool>> condition, IEnumerable<TResult> thenSource, IEnumerable<TResult> elseSource)
        {
            if (provider == null)
                throw new ArgumentNullException("provider");
            if (condition == null)
                throw new ArgumentNullException("condition");
            if (thenSource == null)
                throw new ArgumentNullException("thenSource");
            if (elseSource == null)
                throw new ArgumentNullException("elseSource");

            return provider.CreateQuery<TResult>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TResult)),
                    Expression.Constant(provider, typeof(IQueryProvider)),
                    condition,
                    GetSourceExpression(thenSource),
                    GetSourceExpression(elseSource)
                )
            );
        }

#pragma warning disable 1591
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static /*!*/IQueryable<TResult> If<TResult>(Func<bool> condition, IEnumerable<TResult> thenSource, IEnumerable<TResult> elseSource)
        {
            return EnumerableEx.If(condition, thenSource, elseSource).AsQueryable();
        }
#pragma warning restore 1591

        /// <summary>
        /// Returns an enumerable sequence if the evaluation result of the given condition is true, otherwise returns an empty sequence.
        /// </summary>
        /// <typeparam name="TResult">Result sequence element type.</typeparam>
        /// <param name="provider">Query provider.</param>
        /// <param name="condition">Condition to evaluate.</param>
        /// <param name="thenSource">Sequence to return in case the condition evaluates true.</param>
        /// <returns>The given input sequence if the condition evaluates true; otherwise, an empty sequence.</returns>
        public static IQueryable<TResult> If<TResult>(this IQueryProvider provider, Expression<Func<bool>> condition, IEnumerable<TResult> thenSource)
        {
            if (provider == null)
                throw new ArgumentNullException("provider");
            if (condition == null)
                throw new ArgumentNullException("condition");
            if (thenSource == null)
                throw new ArgumentNullException("thenSource");

            return provider.CreateQuery<TResult>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TResult)),
                    Expression.Constant(provider, typeof(IQueryProvider)),
                    condition,
                    GetSourceExpression(thenSource)
                )
            );
        }

#pragma warning disable 1591
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static /*!*/IQueryable<TResult> If<TResult>(Func<bool> condition, IEnumerable<TResult> thenSource)
        {
            return EnumerableEx.If(condition, thenSource).AsQueryable();
        }
#pragma warning restore 1591

        /// <summary>
        /// Generates an enumerable sequence by repeating a source sequence as long as the given loop postcondition holds.
        /// </summary>
        /// <typeparam name="TResult">Result sequence element type.</typeparam>
        /// <param name="source">Source sequence to repeat while the condition evaluates true.</param>
        /// <param name="condition">Loop condition.</param>
        /// <returns>Sequence generated by repeating the given sequence until the condition evaluates to false.</returns>
        public static IQueryable<TResult> DoWhile<TResult>(this IQueryable<TResult> source, Expression<Func<bool>> condition)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (condition == null)
                throw new ArgumentNullException("condition");

            return source.Provider.CreateQuery<TResult>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TResult)),
                    source.Expression,
                    condition
                )
            );
        }

#pragma warning disable 1591
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static IEnumerable<TResult> DoWhile<TResult>(IEnumerable<TResult> source, Func<bool> condition)
        {
            return EnumerableEx.DoWhile(source, condition);
        }
#pragma warning restore 1591

        /// <summary>
        /// Returns a sequence from a dictionary based on the result of evaluating a selector function.
        /// </summary>
        /// <typeparam name="TValue">Type of the selector value.</typeparam>
        /// <typeparam name="TResult">Result sequence element type.</typeparam>
        /// <param name="provider">Query provider.</param>
        /// <param name="selector">Selector function used to pick a sequence from the given sources.</param>
        /// <param name="sources">Dictionary mapping selector values onto resulting sequences.</param>
        /// <returns>The source sequence corresponding with the evaluated selector value; otherwise, an empty sequence.</returns>
        public static IQueryable<TResult> Case<TValue, TResult>(this IQueryProvider provider, Expression<Func<TValue>> selector, IDictionary<TValue, IEnumerable<TResult>> sources)
        {
            if (provider == null)
                throw new ArgumentNullException("provider");
            if (selector == null)
                throw new ArgumentNullException("selector");
            if (sources == null)
                throw new ArgumentNullException("sources");

            return provider.CreateQuery<TResult>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TValue), typeof(TResult)),
                    selector,
                    Expression.Constant(sources, typeof(IDictionary<TValue, IEnumerable<TResult>>))
                )
            );
        }

#pragma warning disable 1591
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static /*!*/IQueryable<TResult> Case<TValue, TResult>(Func<TValue> selector, IDictionary<TValue, IEnumerable<TResult>> sources)
        {
            return EnumerableEx.Case(selector, sources).AsQueryable();
        }
#pragma warning restore 1591

        /// <summary>
        /// Returns a sequence from a dictionary based on the result of evaluating a selector function, also specifying a default sequence.
        /// </summary>
        /// <typeparam name="TValue">Type of the selector value.</typeparam>
        /// <typeparam name="TResult">Result sequence element type.</typeparam>
        /// <param name="provider">Query provider.</param>
        /// <param name="selector">Selector function used to pick a sequence from the given sources.</param>
        /// <param name="sources">Dictionary mapping selector values onto resulting sequences.</param>
        /// <param name="defaultSource">Default sequence to return in case there's no corresponding source for the computed selector value.</param>
        /// <returns>The source sequence corresponding with the evaluated selector value; otherwise, the default source.</returns>
        public static IQueryable<TResult> Case<TValue, TResult>(this IQueryProvider provider, Expression<Func<TValue>> selector, IDictionary<TValue, IEnumerable<TResult>> sources, IEnumerable<TResult> defaultSource)
        {
            if (provider == null)
                throw new ArgumentNullException("provider");
            if (selector == null)
                throw new ArgumentNullException("selector");
            if (sources == null)
                throw new ArgumentNullException("sources");
            if (defaultSource == null)
                throw new ArgumentNullException("defaultSource");

            return provider.CreateQuery<TResult>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TValue), typeof(TResult)),
                    selector,
                    Expression.Constant(sources, typeof(IDictionary<TValue, IEnumerable<TResult>>))
                )
            );
        }

#pragma warning disable 1591
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static /*!*/IQueryable<TResult> Case<TValue, TResult>(Func<TValue> selector, IDictionary<TValue, IEnumerable<TResult>> sources, IEnumerable<TResult> defaultSource)
        {
            return EnumerableEx.Case(selector, sources, defaultSource).AsQueryable();
        }
#pragma warning restore 1591

        /// <summary>
        /// Generates a sequence by enumerating a source sequence, mapping its elements on result sequences, and concatenating those sequences.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <typeparam name="TResult">Result sequence element type.</typeparam>
        /// <param name="provider">Query provider.</param>
        /// <param name="source">Source sequence.</param>
        /// <param name="resultSelector">Result selector to evaluate for each iteration over the source.</param>
        /// <returns>Sequence concatenating the inner sequences that result from evaluating the result selector on elements from the source.</returns>
        public static IQueryable<TResult> For<TSource, TResult>(this IQueryProvider provider, IEnumerable<TSource> source, Expression<Func<TSource, IEnumerable<TResult>>> resultSelector)
        {
            if (provider == null)
                throw new ArgumentNullException("provider");
            if (source == null)
                throw new ArgumentNullException("source");
            if (resultSelector == null)
                throw new ArgumentNullException("resultSelector");

            return provider.CreateQuery<TResult>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TResult)),
                    GetSourceExpression(source),
                    resultSelector
                )
            );
        }

#pragma warning disable 1591
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static /*!*/IQueryable<TResult> For<TSource, TResult>(IEnumerable<TSource> source, Func<TSource, IEnumerable<TResult>> resultSelector)
        {
            return EnumerableEx.For(source, resultSelector).AsQueryable();
        }
#pragma warning restore 1591

        /// <summary>
        /// Concatenates the input sequences.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="sources">Source sequences.</param>
        /// <returns>Sequence with the elements of the source sequences concatenated.</returns>
        public static IQueryable<TSource> Concat<TSource>(this IQueryable<IEnumerable<TSource>> sources)
        {
            if (sources == null)
                throw new ArgumentNullException("sources");

            return sources.Provider.CreateQuery<TSource>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    GetSourceExpression(sources)
                )
            );
        }

#pragma warning disable 1591
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static IEnumerable<TSource> Concat<TSource>(IEnumerable<IEnumerable<TSource>> sources)
        {
            return EnumerableEx.Concat(sources);
        }
#pragma warning restore 1591

        /// <summary>
        /// Concatenates the input sequences.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="provider">Query provider.</param>
        /// <param name="sources">Source sequences.</param>
        /// <returns>Sequence with the elements of the source sequences concatenated.</returns>
        public static IQueryable<TSource> Concat<TSource>(this IQueryProvider provider, params IEnumerable<TSource>[] sources)
        {
            if (provider == null)
                throw new ArgumentNullException("provider");
            if (sources == null)
                throw new ArgumentNullException("sources");

            return provider.CreateQuery<TSource>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    Expression.Constant(provider, typeof(IQueryProvider)),
                    GetSourceExpression(sources)
                )
            );
        }

#pragma warning disable 1591
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static /*!*/IQueryable<TSource> Concat<TSource>(params IEnumerable<TSource>[] sources)
        {
            return EnumerableEx.Concat(sources).AsQueryable();
        }
#pragma warning restore 1591

        /// <summary>
        /// Projects each element of a sequence to an given sequence and flattens the resulting sequences into one sequence.
        /// </summary>
        /// <typeparam name="TSource">First source sequence element type.</typeparam>
        /// <typeparam name="TOther">Second source sequence element type.</typeparam>
        /// <param name="source">A sequence of values to project.</param>
        /// <param name="other">Inner sequence each source sequenec element is projected onto.</param>
        /// <returns>Sequence flattening the sequences that result from projecting elements in the source sequence.</returns>
        public static IQueryable<TOther> SelectMany<TSource, TOther>(this IQueryable<TSource> source, IEnumerable<TOther> other)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (other == null)
                throw new ArgumentNullException("other");

            return source.Provider.CreateQuery<TOther>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TOther)),
                    source.Expression,
                    GetSourceExpression(other)
                )
            );
        }

#pragma warning disable 1591
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static IEnumerable<TOther> SelectMany<TSource, TOther>(IEnumerable<TSource> source, IEnumerable<TOther> other)
        {
            return EnumerableEx.SelectMany(source, other);
        }
#pragma warning restore 1591

#if NO_ZIP
        /// <summary>
        /// Merges two sequences by applying the specified selector function on index-based corresponding element pairs from both sequences.
        /// </summary>
        /// <typeparam name="TFirst">The type of the elements of the first input sequence.</typeparam>
        /// <typeparam name="TSecond">The type of the elements of the second input sequence.</typeparam>
        /// <typeparam name="TResult">The type of the elements of the result sequence.</typeparam>
        /// <param name="first">The first sequence to merge.</param>
        /// <param name="second">The second sequence to merge.</param>
        /// <param name="resultSelector">Function to apply to each pair of elements from both sequences.</param>
        /// <returns>Sequence consisting of the result of pairwise application of the selector function over pairs of elements from the source sequences.</returns>
        public static IQueryable<TResult> Zip<TFirst, TSecond, TResult>(this IQueryable<TFirst> first, IEnumerable<TSecond> second, Expression<Func<TFirst, TSecond, TResult>> resultSelector)
        {
            if (first == null)
                throw new ArgumentNullException("first");
            if (second == null)
                throw new ArgumentNullException("second");
            if (resultSelector == null)
                throw new ArgumentNullException("resultSelector");

            return first.Provider.CreateQuery<TResult>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TFirst), typeof(TSecond), typeof(TResult)),
                    first.Expression,
                    GetSourceExpression(second),
                    resultSelector
                )
            );
        }

#pragma warning disable 1591
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static IEnumerable<TResult> Zip<TFirst, TSecond, TResult>(IEnumerable<TFirst> first, IEnumerable<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector)
        {
            return EnumerableEx.Zip(first, second, resultSelector);
        }
#pragma warning restore 1591
#endif

        /// <summary>
        /// Hides the enumerable sequence object identity.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <returns>Enumerable sequence with the same behavior as the original, but hiding the source object identity.</returns>
        /// <remarks>AsQueryable doesn't hide the object identity, and simply acts as a cast to the IQueryable&lt;TSource&gt; interface.</remarks>
        public static IQueryable<TSource> Hide<TSource>(this IQueryable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return source.Provider.CreateQuery<TSource>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression
                )
            );
        }

#pragma warning disable 1591
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static IEnumerable<TSource> Hide<TSource>(IEnumerable<TSource> source)
        {
            return EnumerableEx.Hide(source);
        }
#pragma warning restore 1591

        /// <summary>
        /// Lazily invokes an action for each value in the sequence.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="onNext">Action to invoke for each element.</param>
        /// <returns>Sequence exhibiting the specified side-effects upon enumeration.</returns>
        public static IQueryable<TSource> Do<TSource>(this IQueryable<TSource> source, Expression<Action<TSource>> onNext)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (onNext == null)
                throw new ArgumentNullException("onNext");

            return source.Provider.CreateQuery<TSource>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression,
                    onNext
                )
            );
        }

#pragma warning disable 1591
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static IEnumerable<TSource> Do<TSource>(IEnumerable<TSource> source, Action<TSource> onNext)
        {
            return EnumerableEx.Do(source, onNext);
        }
#pragma warning restore 1591

        /// <summary>
        /// Lazily invokes an action for each value in the sequence, and executes an action for successful termination.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="onNext">Action to invoke for each element.</param>
        /// <param name="onCompleted">Action to invoke on successful termination of the sequence.</param>
        /// <returns>Sequence exhibiting the specified side-effects upon enumeration.</returns>
        public static IQueryable<TSource> Do<TSource>(this IQueryable<TSource> source, Expression<Action<TSource>> onNext, Expression<Action> onCompleted)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (onNext == null)
                throw new ArgumentNullException("onNext");
            if (onCompleted == null)
                throw new ArgumentNullException("onCompleted");

            return source.Provider.CreateQuery<TSource>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression,
                    onNext,
                    onCompleted
                )
            );
        }

#pragma warning disable 1591
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static IEnumerable<TSource> Do<TSource>(IEnumerable<TSource> source, Action<TSource> onNext, Action onCompleted)
        {
            return EnumerableEx.Do(source, onNext, onCompleted);
        }
#pragma warning restore 1591

        /// <summary>
        /// Lazily invokes an action for each value in the sequence, and executes an action upon exceptional termination.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="onNext">Action to invoke for each element.</param>
        /// <param name="onError">Action to invoke on exceptional termination of the sequence.</param>
        /// <returns>Sequence exhibiting the specified side-effects upon enumeration.</returns>
        public static IQueryable<TSource> Do<TSource>(this IQueryable<TSource> source, Expression<Action<TSource>> onNext, Expression<Action<Exception>> onError)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (onNext == null)
                throw new ArgumentNullException("onNext");
            if (onError == null)
                throw new ArgumentNullException("onError");

            return source.Provider.CreateQuery<TSource>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression,
                    onNext,
                    onError
                )
            );
        }

#pragma warning disable 1591
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static IEnumerable<TSource> Do<TSource>(IEnumerable<TSource> source, Action<TSource> onNext, Action<Exception> onError)
        {
            return EnumerableEx.Do(source, onNext, onError);
        }
#pragma warning restore 1591

        /// <summary>
        /// Lazily invokes an action for each value in the sequence, and executes an action upon successful or exceptional termination.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="onNext">Action to invoke for each element.</param>
        /// <param name="onError">Action to invoke on exceptional termination of the sequence.</param>
        /// <param name="onCompleted">Action to invoke on successful termination of the sequence.</param>
        /// <returns>Sequence exhibiting the specified side-effects upon enumeration.</returns>
        public static IQueryable<TSource> Do<TSource>(this IQueryable<TSource> source, Expression<Action<TSource>> onNext, Expression<Action<Exception>> onError, Expression<Action> onCompleted)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (onNext == null)
                throw new ArgumentNullException("onNext");
            if (onError == null)
                throw new ArgumentNullException("onError");
            if (onCompleted == null)
                throw new ArgumentNullException("onCompleted");

            return source.Provider.CreateQuery<TSource>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression,
                    onNext,
                    onError,
                    onCompleted
                )
            );
        }

#pragma warning disable 1591
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static IEnumerable<TSource> Do<TSource>(IEnumerable<TSource> source, Action<TSource> onNext, Action<Exception> onError, Action onCompleted)
        {
            return EnumerableEx.Do(source, onNext, onError, onCompleted);
        }
#pragma warning restore 1591

#if !NO_RXINTERFACES
        /// <summary>
        /// Lazily invokes observer methods for each value in the sequence, and upon successful or exceptional termination.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="observer">Observer to invoke notification calls on.</param>
        /// <returns>Sequence exhibiting the side-effects of observer method invocation upon enumeration.</returns>
        public static IQueryable<TSource> Do<TSource>(this IQueryable<TSource> source, IObserver<TSource> observer)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (observer == null)
                throw new ArgumentNullException("observer");

            return source.Provider.CreateQuery<TSource>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression,
                    Expression.Constant(observer, typeof(IObserver<TSource>))
                )
            );
        }

#pragma warning disable 1591
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static IEnumerable<TSource> Do<TSource>(IEnumerable<TSource> source, IObserver<TSource> observer)
        {
            return EnumerableEx.Do(source, observer);
        }
#pragma warning restore 1591
#endif

        /// <summary>
        /// Generates a sequence of non-overlapping adjacent buffers over the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="count">Number of elements for allocated buffers.</param>
        /// <returns>Sequence of buffers containing source sequence elements.</returns>
        public static IQueryable<IList<TSource>> Buffer<TSource>(this IQueryable<TSource> source, int count)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return source.Provider.CreateQuery<IList<TSource>>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression,
                    Expression.Constant(count, typeof(int))
                )
            );
        }

#pragma warning disable 1591
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static IEnumerable<IList<TSource>> Buffer<TSource>(IEnumerable<TSource> source, int count)
        {
            return EnumerableEx.Buffer(source, count);
        }
#pragma warning restore 1591

        /// <summary>
        /// Generates a sequence of buffers over the source sequence, with specified length and possible overlap.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="count">Number of elements for allocated buffers.</param>
        /// <param name="skip">Number of elements to skip between the start of consecutive buffers.</param>
        /// <returns>Sequence of buffers containing source sequence elements.</returns>
        public static IQueryable<IList<TSource>> Buffer<TSource>(this IQueryable<TSource> source, int count, int skip)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return source.Provider.CreateQuery<IList<TSource>>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression,
                    Expression.Constant(count, typeof(int)),
                    Expression.Constant(skip, typeof(int))
                )
            );
        }

#pragma warning disable 1591
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static IEnumerable<IList<TSource>> Buffer<TSource>(IEnumerable<TSource> source, int count, int skip)
        {
            return EnumerableEx.Buffer(source, count, skip);
        }
#pragma warning restore 1591

        /// <summary>
        /// Ignores all elements in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <returns>Source sequence without its elements.</returns>
        public static IQueryable<TSource> IgnoreElements<TSource>(this IQueryable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return source.Provider.CreateQuery<TSource>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression
                )
            );
        }

#pragma warning disable 1591
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static IEnumerable<TSource> IgnoreElements<TSource>(IEnumerable<TSource> source)
        {
            return EnumerableEx.IgnoreElements(source);
        }
#pragma warning restore 1591

        /// <summary>
        /// Returns elements with a distinct key value by using the default equality comparer to compare key values.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <typeparam name="TKey">Key type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="keySelector">Key selector.</param>
        /// <returns>Sequence that contains the elements from the source sequence with distinct key values.</returns>
        public static IQueryable<TSource> Distinct<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");

            return source.Provider.CreateQuery<TSource>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)),
                    source.Expression,
                    keySelector
                )
            );
        }

#pragma warning disable 1591
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static IEnumerable<TSource> Distinct<TSource, TKey>(IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            return EnumerableEx.Distinct(source, keySelector);
        }
#pragma warning restore 1591

        /// <summary>
        /// Returns elements with a distinct key value by using the specified equality comparer to compare key values.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <typeparam name="TKey">Key type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="keySelector">Key selector.</param>
        /// <param name="comparer">Comparer used to compare key values.</param>
        /// <returns>Sequence that contains the elements from the source sequence with distinct key values.</returns>
        public static IQueryable<TSource> Distinct<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            return source.Provider.CreateQuery<TSource>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)),
                    source.Expression,
                    keySelector,
                    Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))
                )
            );
        }

#pragma warning disable 1591
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static IEnumerable<TSource> Distinct<TSource, TKey>(IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            return EnumerableEx.Distinct(source, keySelector, comparer);
        }
#pragma warning restore 1591

        /// <summary>
        /// Returns consecutive distinct elements by using the default equality comparer to compare values.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <returns>Sequence without adjacent non-distinct elements.</returns>
        public static IQueryable<TSource> DistinctUntilChanged<TSource>(this IQueryable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return source.Provider.CreateQuery<TSource>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression
                )
            );
        }

#pragma warning disable 1591
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static IEnumerable<TSource> DistinctUntilChanged<TSource>(IEnumerable<TSource> source)
        {
            return EnumerableEx.DistinctUntilChanged(source);
        }
#pragma warning restore 1591

        /// <summary>
        /// Returns consecutive distinct elements by using the specified equality comparer to compare values.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="comparer">Comparer used to compare values.</param>
        /// <returns>Sequence without adjacent non-distinct elements.</returns>
        public static IQueryable<TSource> DistinctUntilChanged<TSource>(this IQueryable<TSource> source, IEqualityComparer<TSource> comparer)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            return source.Provider.CreateQuery<TSource>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression,
                    Expression.Constant(comparer, typeof(IEqualityComparer<TSource>))
                )
            );
        }

#pragma warning disable 1591
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static IEnumerable<TSource> DistinctUntilChanged<TSource>(IEnumerable<TSource> source, IEqualityComparer<TSource> comparer)
        {
            return EnumerableEx.DistinctUntilChanged(source, comparer);
        }
#pragma warning restore 1591

        /// <summary>
        /// Returns consecutive distinct elements based on a key value by using the specified equality comparer to compare key values.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <typeparam name="TKey">Key type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="keySelector">Key selector.</param>
        /// <returns>Sequence without adjacent non-distinct elements.</returns>
        public static IQueryable<TSource> DistinctUntilChanged<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");

            return source.Provider.CreateQuery<TSource>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)),
                    source.Expression,
                    keySelector
                )
            );
        }

#pragma warning disable 1591
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static IEnumerable<TSource> DistinctUntilChanged<TSource, TKey>(IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            return EnumerableEx.DistinctUntilChanged(source, keySelector);
        }
#pragma warning restore 1591

        /// <summary>
        /// Returns consecutive distinct elements based on a key value by using the specified equality comparer to compare key values.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <typeparam name="TKey">Key type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="keySelector">Key selector.</param>
        /// <param name="comparer">Comparer used to compare key values.</param>
        /// <returns>Sequence without adjacent non-distinct elements.</returns>
        public static IQueryable<TSource> DistinctUntilChanged<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            return source.Provider.CreateQuery<TSource>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)),
                    source.Expression,
                    keySelector,
                    Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))
                )
            );
        }

#pragma warning disable 1591
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static IEnumerable<TSource> DistinctUntilChanged<TSource, TKey>(IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            return EnumerableEx.DistinctUntilChanged(source, keySelector, comparer);
        }
#pragma warning restore 1591

        /// <summary>
        /// Expands the sequence by recursively applying a selector function.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="selector">Selector function to retrieve the next sequence to expand.</param>
        /// <returns>Sequence with results from the recursive expansion of the source sequence.</returns>
        public static IQueryable<TSource> Expand<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, IEnumerable<TSource>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return source.Provider.CreateQuery<TSource>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression,
                    selector
                )
            );
        }

#pragma warning disable 1591
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static IEnumerable<TSource> Expand<TSource>(IEnumerable<TSource> source, Func<TSource, IEnumerable<TSource>> selector)
        {
            return EnumerableEx.Expand(source, selector);
        }
#pragma warning restore 1591

        /// <summary>
        /// Returns the source sequence prefixed with the specified value.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="values">Values to prefix the sequence with.</param>
        /// <returns>Sequence starting with the specified prefix value, followed by the source sequence.</returns>
        public static IQueryable<TSource> StartWith<TSource>(this IQueryable<TSource> source, params TSource[] values)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return source.Provider.CreateQuery<TSource>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression,
                    Expression.Constant(values, typeof(TSource[]))
                )
            );
        }

#pragma warning disable 1591
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static IEnumerable<TSource> StartWith<TSource>(IEnumerable<TSource> source, params TSource[] values)
        {
            return EnumerableEx.StartWith(source, values);
        }
#pragma warning restore 1591

        /// <summary>
        /// Generates a sequence of accumulated values by scanning the source sequence and applying an accumulator function.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <typeparam name="TAccumulate">Accumulation type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="seed">Accumulator seed value.</param>
        /// <param name="accumulator">Accumulation function to apply to the current accumulation value and each element of the sequence.</param>
        /// <returns>Sequence with all intermediate accumulation values resulting from scanning the sequence.</returns>
        public static IQueryable<TAccumulate> Scan<TSource, TAccumulate>(this IQueryable<TSource> source, TAccumulate seed, Expression<Func<TAccumulate, TSource, TAccumulate>> accumulator)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (accumulator == null)
                throw new ArgumentNullException("accumulator");

            return source.Provider.CreateQuery<TAccumulate>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TAccumulate)),
                    source.Expression,
                    Expression.Constant(seed, typeof(TAccumulate)),
                    accumulator
                )
            );
        }

#pragma warning disable 1591
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static IEnumerable<TAccumulate> Scan<TSource, TAccumulate>(IEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> accumulator)
        {
            return EnumerableEx.Scan(source, seed, accumulator);
        }
#pragma warning restore 1591

        /// <summary>
        /// Generates a sequence of accumulated values by scanning the source sequence and applying an accumulator function.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="accumulator">Accumulation function to apply to the current accumulation value and each element of the sequence.</param>
        /// <returns>Sequence with all intermediate accumulation values resulting from scanning the sequence.</returns>
        public static IQueryable<TSource> Scan<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, TSource, TSource>> accumulator)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (accumulator == null)
                throw new ArgumentNullException("accumulator");

            return source.Provider.CreateQuery<TSource>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression,
                    accumulator
                )
            );
        }

#pragma warning disable 1591
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static IEnumerable<TSource> Scan<TSource>(IEnumerable<TSource> source, Func<TSource, TSource, TSource> accumulator)
        {
            return EnumerableEx.Scan(source, accumulator);
        }
#pragma warning restore 1591

        /// <summary>
        /// Returns a specified number of contiguous elements from the end of the sequence.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="count">The number of elements to take from the end of the sequence.</param>
        /// <returns>Sequence with the specified number of elements counting from the end of the source sequence.</returns>
        public static IQueryable<TSource> TakeLast<TSource>(this IQueryable<TSource> source, int count)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return source.Provider.CreateQuery<TSource>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression,
                    Expression.Constant(count, typeof(int))
                )
            );
        }

#pragma warning disable 1591
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static IEnumerable<TSource> TakeLast<TSource>(IEnumerable<TSource> source, int count)
        {
            return EnumerableEx.TakeLast(source, count);
        }
#pragma warning restore 1591

        /// <summary>
        /// Bypasses a specified number of contiguous elements from the end of the sequence and returns the remaining elements.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="count">The number of elements to skip from the end of the sequence before returning the remaining elements.</param>
        /// <returns>Sequence bypassing the specified number of elements counting from the end of the source sequence.</returns>
        public static IQueryable<TSource> SkipLast<TSource>(this IQueryable<TSource> source, int count)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return source.Provider.CreateQuery<TSource>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression,
                    Expression.Constant(count, typeof(int))
                )
            );
        }

#pragma warning disable 1591
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static IEnumerable<TSource> SkipLast<TSource>(IEnumerable<TSource> source, int count)
        {
            return EnumerableEx.SkipLast(source, count);
        }
#pragma warning restore 1591

        /// <summary>
        /// Repeats and concatenates the source sequence infinitely.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <returns>Sequence obtained by concatenating the source sequence to itself infinitely.</returns>
        public static IQueryable<TSource> Repeat<TSource>(this IQueryable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return source.Provider.CreateQuery<TSource>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression
                )
            );
        }

#pragma warning disable 1591
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static IEnumerable<TSource> Repeat<TSource>(IEnumerable<TSource> source)
        {
            return EnumerableEx.Repeat(source);
        }
#pragma warning restore 1591

        /// <summary>
        /// Repeats and concatenates the source sequence the given number of times.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="count">Number of times to repeat the source sequence.</param>
        /// <returns>Sequence obtained by concatenating the source sequence to itself the specified number of times.</returns>
        public static IQueryable<TSource> Repeat<TSource>(this IQueryable<TSource> source, int count)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return source.Provider.CreateQuery<TSource>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
                    source.Expression,
                    Expression.Constant(count, typeof(int))
                )
            );
        }

#pragma warning disable 1591
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static IEnumerable<TSource> Repeat<TSource>(IEnumerable<TSource> source, int count)
        {
            return EnumerableEx.Repeat(source, count);
        }
#pragma warning restore 1591

        /// <summary>
        /// Returns a sequence with no elements.
        /// </summary>
        /// <typeparam name="TResult">Result sequence element type.</typeparam>
        /// <param name="provider">Query provider.</param>
        /// <returns>Sequence with no elements.</returns>
        public static IQueryable<TResult> Empty<TResult>(this IQueryProvider provider)
        {
            if (provider == null)
                throw new ArgumentNullException("provider");

            return provider.CreateQuery<TResult>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TResult)),
                    Expression.Constant(provider, typeof(IQueryProvider))
                )
            );
        }

#pragma warning disable 1591
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static /*!*/IQueryable<TResult> Empty<TResult>()
        {
            return Enumerable.Empty<TResult>().AsQueryable();
        }
#pragma warning restore 1591

        /// <summary>
        /// Generates a sequence of integral numbers within a specified range.
        /// </summary>
        /// <param name="provider">Query provider.</param>
        /// <param name="start">The value of the first integer in the sequence.</param>
        /// <param name="count">The number of sequential integers to generate.</param>
        /// <returns>Sequence that contains a range of sequential integral numbers.</returns>
        public static IQueryable<int> Range(this IQueryProvider provider, int start, int count)
        {
            if (provider == null)
                throw new ArgumentNullException("provider");

            return provider.CreateQuery<int>(
                Expression.Call(
                    null,
                    (MethodInfo)MethodInfo.GetCurrentMethod(),
                    Expression.Constant(provider, typeof(IQueryProvider)),
                    Expression.Constant(start, typeof(int)),
                    Expression.Constant(count, typeof(int))
                )
            );
        }

#pragma warning disable 1591
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static /*!*/IQueryable<int> Range(int start, int count)
        {
            return Enumerable.Range(start, count).AsQueryable();
        }
#pragma warning restore 1591

        /// <summary>
        /// Generates a sequence that contains one repeated value.
        /// </summary>
        /// <typeparam name="TResult">Result sequence element type.</typeparam>
        /// <param name="provider">Query provider.</param>
        /// <param name="element">The value to be repeated.</param>
        /// <param name="count">The number of times to repeat the value in the generated sequence.</param>
        /// <returns>Sequence that contains a repeated value.</returns>
        public static IQueryable<TResult> Repeat<TResult>(this IQueryProvider provider, TResult element, int count)
        {
            if (provider == null)
                throw new ArgumentNullException("provider");

            return provider.CreateQuery<TResult>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TResult)),
                    Expression.Constant(provider, typeof(IQueryProvider)),
                    Expression.Constant(element, typeof(TResult)),
                    Expression.Constant(count, typeof(int))
                )
            );
        }

#pragma warning disable 1591
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static /*!*/IQueryable<TResult> Repeat<TResult>(TResult element, int count)
        {
            return EnumerableEx.Repeat<TResult>(element, count).AsQueryable();
        }
#pragma warning restore 1591

        /// <summary>
        /// Gets the local Queryable provider.
        /// </summary>
        public static IQueryProvider Provider
        {
            get
            {
                return new QueryProviderShim();
            }
        }

        class QueryProviderShim : IQueryProvider
        {
            public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
            {
                var provider = new TElement[0].AsQueryable().Provider;
                var res = Redir(expression);
                return provider.CreateQuery<TElement>(res);
            }

            public IQueryable CreateQuery(Expression expression)
            {
                return CreateQuery<object>(expression);
            }

            public TResult Execute<TResult>(Expression expression)
            {
                var provider = new TResult[0].AsQueryable().Provider;
                var res = Redir(expression);
                return provider.Execute<TResult>(res);
            }

            public object Execute(Expression expression)
            {
                return Execute<object>(expression);
            }

            private static Expression Redir(Expression expression)
            {
                var mce = expression as MethodCallExpression;
                if (mce != null && mce.Method.DeclaringType == typeof(QueryableEx))
                {
                    if (mce.Arguments.Count >= 1 && typeof(IQueryProvider).IsAssignableFrom(mce.Arguments[0].Type))
                    {
                        var ce = mce.Arguments[0] as ConstantExpression;
                        if (ce != null)
                        {
                            if (ce.Value is QueryProviderShim)
                            {
                                var targetType = typeof(QueryableEx);
                                var method = mce.Method;
                                var methods = GetMethods(targetType);
                                var arguments = mce.Arguments.Skip(1).ToList();

                                //
                                // From all the operators with the method's name, find the one that matches all arguments.
                                //
                                var typeArgs = method.IsGenericMethod ? method.GetGenericArguments() : null;
                                var targetMethod = methods[method.Name].FirstOrDefault(candidateMethod => ArgsMatch(candidateMethod, arguments, typeArgs));
                                if (targetMethod == null)
                                    throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "There is no method '{0}' on type '{1}' that matches the specified arguments", method.Name, targetType.Name));

                                //
                                // Restore generic arguments.
                                //
                                if (typeArgs != null)
                                    targetMethod = targetMethod.MakeGenericMethod(typeArgs);

                                //
                                // Finally, we need to deal with mismatches on Expression<Func<...>> versus Func<...>.
                                //
                                var parameters = targetMethod.GetParameters();
                                for (int i = 0, n = parameters.Length; i < n; i++)
                                {
                                    arguments[i] = Unquote(arguments[i]);
                                }

                                //
                                // Emit a new call to the discovered target method.
                                //
                                return Expression.Call(null, targetMethod, arguments);
                            }
                        }
                    }
                }

                return expression;
            }

            private static ILookup<string, MethodInfo> GetMethods(Type type)
            {
                return type.GetMethods(BindingFlags.Static | BindingFlags.Public).ToLookup(m => m.Name);
            }

            private static bool ArgsMatch(MethodInfo method, IList<Expression> arguments, Type[] typeArgs)
            {
                //
                // Number of parameters should match. Notice we've sanitized IQueryProvider "this"
                // parameters first (see Redir).
                //
                var parameters = method.GetParameters();
                if (parameters.Length != arguments.Count)
                    return false;

                //
                // Genericity should match too.
                //
                if (!method.IsGenericMethod && typeArgs != null && typeArgs.Length > 0)
                    return false;

                //
                // Reconstruct the generic method if needed.
                //
                if (method.IsGenericMethodDefinition)
                {
                    if (typeArgs == null)
                        return false;

                    if (method.GetGenericArguments().Length != typeArgs.Length)
                        return false;

                    var result = method.MakeGenericMethod(typeArgs);
                    parameters = result.GetParameters();
                }

                //
                // Check compatibility for the parameter types.
                //
                for (int i = 0, n = arguments.Count; i < n; i++)
                {
                    var parameterType = parameters[i].ParameterType;
                    var argument = arguments[i];

                    //
                    // For operators that take a function (like Where, Select), we'll be faced
                    // with a quoted argument and a discrepancy between Expression<Func<...>>
                    // and the underlying Func<...>.
                    //
                    if (!parameterType.IsAssignableFrom(argument.Type))
                    {
                        argument = Unquote(argument);
                        if (!parameterType.IsAssignableFrom(argument.Type))
                            return false;
                    }
                }

                return true;
            }

            private static Expression Unquote(Expression expression)
            {
                //
                // Get rid of all outer quotes around an expression.
                //
                while (expression.NodeType == ExpressionType.Quote)
                    expression = ((UnaryExpression)expression).Operand;

                return expression;
            }
        }

        internal static Expression GetSourceExpression<TSource>(IEnumerable<TSource> source)
        {
            var q = source as IQueryable<TSource>;
            if (q != null)
                return q.Expression;

            return Expression.Constant(source, typeof(IEnumerable<TSource>));
        }

        internal static Expression GetSourceExpression<TSource>(IEnumerable<TSource>[] sources)
        {
            return Expression.NewArrayInit(
                typeof(IEnumerable<TSource>),
                sources.Select(source => GetSourceExpression(source))
            );
        }
    }
}

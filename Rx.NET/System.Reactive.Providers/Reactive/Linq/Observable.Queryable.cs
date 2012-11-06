// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#pragma warning disable 1591

namespace System.Reactive.Linq
{
    public static partial class Qbservable
    {
        private static IQbservableProvider s_provider;

        /// <summary>
        /// Gets the local query provider which will retarget Qbservable-based queries to the corresponding Observable-based query for in-memory execution upon subscription.
        /// </summary>
        public static IQbservableProvider Provider
        {
            get
            {
                if (s_provider == null)
                    s_provider = new ObservableQueryProvider();

                return s_provider;
            }
        }

        /// <summary>
        /// Converts an in-memory observable sequence into an IQbservable&lt;T&gt; sequence with an expression tree representing the source sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <returns>IQbservable&lt;T&gt; sequence representing the given observable source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static IQbservable<TSource> AsQbservable<TSource>(this IObservable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return new ObservableQuery<TSource>(source);
        }
    }
}

#pragma warning restore 1591
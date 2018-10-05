// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

#pragma warning disable 1591

namespace System.Reactive.Linq
{
    public static partial class Qbservable
    {
#pragma warning disable IDE1006 // Naming Styles: 3rd party code is known to reflect for this specific field name
        private static IQbservableProvider s_provider;
#pragma warning restore IDE1006 // Naming Styles

        /// <summary>
        /// Gets the local query provider which will retarget Qbservable-based queries to the corresponding Observable-based query for in-memory execution upon subscription.
        /// </summary>
        public static IQbservableProvider Provider
        {
            get
            {
                if (s_provider == null)
                {
                    s_provider = new ObservableQueryProvider();
                }

                return s_provider;
            }
        }

        /// <summary>
        /// Converts an in-memory observable sequence into an <see cref="IQbservable{T}"/> sequence with an expression tree representing the source sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <returns><see cref="IQbservable{T}"/> sequence representing the given observable source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static IQbservable<TSource> AsQbservable<TSource>(this IObservable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return new ObservableQuery<TSource>(source);
        }
    }
}

#pragma warning restore 1591

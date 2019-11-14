// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;

namespace System.Linq
{
    public static partial class EnumerableEx
    {
        /// <summary>
        /// Creates a sequence that concatenates both given sequences, regardless of whether an error occurs.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="first">First sequence.</param>
        /// <param name="second">Second sequence.</param>
        /// <returns>Sequence concatenating the elements of both sequences, ignoring errors.</returns>
        public static IEnumerable<TSource> OnErrorResumeNext<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));

            return OnErrorResumeNextCore(new[] { first, second });
        }

        /// <summary>
        /// Creates a sequence that concatenates the given sequences, regardless of whether an error occurs in any of the
        /// sequences.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="sources">Source sequences.</param>
        /// <returns>Sequence concatenating the elements of the given sequences, ignoring errors.</returns>
        public static IEnumerable<TSource> OnErrorResumeNext<TSource>(params IEnumerable<TSource>[] sources)
        {
            if (sources == null)
                throw new ArgumentNullException(nameof(sources));

            return OnErrorResumeNextCore(sources);
        }

        /// <summary>
        /// Creates a sequence that concatenates the given sequences, regardless of whether an error occurs in any of the
        /// sequences.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="sources">Source sequences.</param>
        /// <returns>Sequence concatenating the elements of the given sequences, ignoring errors.</returns>
        public static IEnumerable<TSource> OnErrorResumeNext<TSource>(this IEnumerable<IEnumerable<TSource>> sources)
        {
            if (sources == null)
                throw new ArgumentNullException(nameof(sources));

            return OnErrorResumeNextCore(sources);
        }

        private static IEnumerable<TSource> OnErrorResumeNextCore<TSource>(IEnumerable<IEnumerable<TSource>> sources)
        {
            foreach (var source in sources)
            {
                using var innerEnumerator = source.GetEnumerator();

                while (true)
                {
                    TSource value;
                    try
                    {
                        if (!innerEnumerator.MoveNext())
                            break;

                        value = innerEnumerator.Current;
                    }
                    catch
                    {
                        break;
                    }

                    yield return value;
                }
            }
        }
    }
}

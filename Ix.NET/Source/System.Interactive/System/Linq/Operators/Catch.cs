// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;

namespace System.Linq
{
    public static partial class EnumerableEx
    {
        /// <summary>
        /// Creates a sequence that corresponds to the source sequence, concatenating it with the sequence resulting from
        /// calling an exception handler function in case of an error.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <typeparam name="TException">Exception type to catch.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="handler">Handler to invoke when an exception of the specified type occurs.</param>
        /// <returns>Source sequence, concatenated with an exception handler result sequence in case of an error.</returns>
        public static IEnumerable<TSource> Catch<TSource, TException>(this IEnumerable<TSource> source, Func<TException, IEnumerable<TSource>> handler)
            where TException : Exception
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            return CatchCore(source, handler);
        }

        /// <summary>
        /// Creates a sequence by concatenating source sequences until a source sequence completes successfully.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="sources">Source sequences.</param>
        /// <returns>Sequence that continues to concatenate source sequences while errors occur.</returns>
        public static IEnumerable<TSource> Catch<TSource>(this IEnumerable<IEnumerable<TSource>> sources)
        {
            if (sources == null)
                throw new ArgumentNullException(nameof(sources));

            return CatchCore(sources);
        }

        /// <summary>
        /// Creates a sequence by concatenating source sequences until a source sequence completes successfully.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="sources">Source sequences.</param>
        /// <returns>Sequence that continues to concatenate source sequences while errors occur.</returns>
        public static IEnumerable<TSource> Catch<TSource>(params IEnumerable<TSource>[] sources)
        {
            if (sources == null)
                throw new ArgumentNullException(nameof(sources));

            return CatchCore(sources);
        }

        /// <summary>
        /// Creates a sequence that returns the elements of the first sequence, switching to the second in case of an error.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="first">First sequence.</param>
        /// <param name="second">Second sequence, concatenated to the result in case the first sequence completes exceptionally.</param>
        /// <returns>The first sequence, followed by the second sequence in case an error is produced.</returns>
        public static IEnumerable<TSource> Catch<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));

            return CatchCore(new[] { first, second });
        }

        private static IEnumerable<TSource> CatchCore<TSource, TException>(IEnumerable<TSource> source, Func<TException, IEnumerable<TSource>> handler)
            where TException : Exception
        {
            var err = default(IEnumerable<TSource>);

            using (var e = source.GetEnumerator())
            {
                while (true)
                {
                    TSource c;

                    try
                    {
                        if (!e.MoveNext())
                            break;

                        c = e.Current;
                    }
                    catch (TException ex)
                    {
                        err = handler(ex);
                        break;
                    }

                    yield return c;
                }
            }

            if (err != null)
            {
                foreach (var item in err)
                {
                    yield return item;
                }
            }
        }

        private static IEnumerable<TSource> CatchCore<TSource>(IEnumerable<IEnumerable<TSource>> sources)
        {
            var error = default(Exception);

            foreach (var source in sources)
            {
                using var e = source.GetEnumerator();
                error = null;

                while (true)
                {
                    TSource c;

                    try
                    {
                        if (!e.MoveNext())
                            break;

                        c = e.Current;
                    }
                    catch (Exception ex)
                    {
                        error = ex;
                        break;
                    }

                    yield return c;
                }

                if (error == null)
                    break;
            }

            if (error != null)
                throw error;
        }
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;

namespace System.Linq
{
    public static partial class EnumerableEx
    {
        /// <summary>
        ///     Creates a sequence that retries enumerating the source sequence as long as an error occurs.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <returns>Sequence concatenating the results of the source sequence as long as an error occurs.</returns>
        public static IEnumerable<TSource> Retry<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return RetryInfinite(source);
        }

        /// <summary>
        ///     Creates a sequence that retries enumerating the source sequence as long as an error occurs, with the specified
        ///     maximum number of retries.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="retryCount">Maximum number of retries.</param>
        /// <returns>Sequence concatenating the results of the source sequence as long as an error occurs.</returns>
        public static IEnumerable<TSource> Retry<TSource>(this IEnumerable<TSource> source, int retryCount)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (retryCount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(retryCount));
            }

            return RetryFinite(source, retryCount);
        }

        private static IEnumerable<TSource> RetryInfinite<TSource>(IEnumerable<TSource> source)
        {
            while (true)
            {
                var enumerator = default(IEnumerator<TSource>);
                try
                {
                    enumerator = source.GetEnumerator();
                }
                catch
                {
                    continue;
                }

                using (enumerator)
                {
                    while (true)
                    {
                        var v = default(TSource);

                        try
                        {
                            if (!enumerator.MoveNext())
                            {
                                yield break;
                            }
                            v = enumerator.Current;
                        }
                        catch
                        {
                            break;
                        }

                        yield return v;
                    }
                }
            }
        }

        private static IEnumerable<TSource> RetryFinite<TSource>(IEnumerable<TSource> source, int retryCount)
        {
            var lastException = default(Exception);

            for (var i = 0; i < retryCount; i++)
            {
                var enumerator = default(IEnumerator<TSource>);
                try
                {
                    enumerator = source.GetEnumerator();
                }
                catch (Exception ex)
                {
                    lastException = ex;
                    continue;
                }

                using (enumerator)
                {
                    while (true)
                    {
                        var v = default(TSource);

                        try
                        {
                            if (!enumerator.MoveNext())
                            {
                                yield break;
                            }
                            v = enumerator.Current;
                        }
                        catch (Exception ex)
                        {
                            lastException = ex;
                            break;
                        }

                        yield return v;
                    }
                }
            }

            if (lastException != null)
            {
                throw lastException;
            }
        }
    }
}

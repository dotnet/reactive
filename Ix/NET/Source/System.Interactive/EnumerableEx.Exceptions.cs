// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.
using System;
using System.Collections.Generic;
using System.Linq;

namespace System.Linq
{
    public static partial class EnumerableEx
    {
        /// <summary>
        /// Creates a sequence that corresponds to the source sequence, concatenating it with the sequence resulting from calling an exception handler function in case of an error.
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
                throw new ArgumentNullException("source");
            if (handler == null)
                throw new ArgumentNullException("handler");

            return source.Catch_(handler);
        }

        private static IEnumerable<TSource> Catch_<TSource, TException>(this IEnumerable<TSource> source, Func<TException, IEnumerable<TSource>> handler)
            where TException : Exception
        {
            var err = default(IEnumerable<TSource>);

            using (var e = source.GetEnumerator())
            {
                while (true)
                {
                    var b = default(bool);
                    var c = default(TSource);

                    try
                    {
                        b = e.MoveNext();
                        c = e.Current;
                    }
                    catch (TException ex)
                    {
                        err = handler(ex);
                        break;
                    }

                    if (!b)
                        break;

                    yield return c;
                }
            }

            if (err != null)
            {
                foreach (var item in err)
                    yield return item;
            }
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
                throw new ArgumentNullException("sources");

            return sources.Catch_();
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
                throw new ArgumentNullException("sources");

            return sources.Catch_();
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
                throw new ArgumentNullException("first");
            if (second == null)
                throw new ArgumentNullException("second");

            return new[] { first, second }.Catch_();
        }

        private static IEnumerable<TSource> Catch_<TSource>(this IEnumerable<IEnumerable<TSource>> sources)
        {
            var error = default(Exception);

            foreach (var source in sources)
            {
                using (var e = source.GetEnumerator())
                {
                    error = null;

                    while (true)
                    {
                        var b = default(bool);
                        var c = default(TSource);

                        try
                        {
                            b = e.MoveNext();
                            c = e.Current;
                        }
                        catch (Exception ex)
                        {
                            error = ex;
                            break;
                        }

                        if (!b)
                            break;

                        yield return c;
                    }

                    if (error == null)
                        break;
                }
            }

            if (error != null)
                throw error;
        }

        /// <summary>
        /// Creates a sequence whose termination or disposal of an enumerator causes a finally action to be executed.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="finallyAction">Action to run upon termination of the sequence, or when an enumerator is disposed.</param>
        /// <returns>Source sequence with guarantees on the invocation of the finally action.</returns>
        public static IEnumerable<TSource> Finally<TSource>(this IEnumerable<TSource> source, Action finallyAction)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (finallyAction == null)
                throw new ArgumentNullException("finallyAction");

            return source.Finally_(finallyAction);
        }

        private static IEnumerable<TSource> Finally_<TSource>(this IEnumerable<TSource> source, Action finallyAction)
        {
            try
            {
                foreach (var item in source)
                    yield return item;
            }
            finally
            {
                finallyAction();
            }
        }

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
                throw new ArgumentNullException("first");
            if (second == null)
                throw new ArgumentNullException("second");

            return OnErrorResumeNext_(new[] { first, second });
        }

        /// <summary>
        /// Creates a sequence that concatenates the given sequences, regardless of whether an error occurs in any of the sequences.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="sources">Source sequences.</param>
        /// <returns>Sequence concatenating the elements of the given sequences, ignoring errors.</returns>
        public static IEnumerable<TSource> OnErrorResumeNext<TSource>(params IEnumerable<TSource>[] sources)
        {
            if (sources == null)
                throw new ArgumentNullException("sources");

            return OnErrorResumeNext_(sources);
        }

        /// <summary>
        /// Creates a sequence that concatenates the given sequences, regardless of whether an error occurs in any of the sequences.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="sources">Source sequences.</param>
        /// <returns>Sequence concatenating the elements of the given sequences, ignoring errors.</returns>
        public static IEnumerable<TSource> OnErrorResumeNext<TSource>(this IEnumerable<IEnumerable<TSource>> sources)
        {
            if (sources == null)
                throw new ArgumentNullException("sources");

            return OnErrorResumeNext_(sources);
        }

        private static IEnumerable<TSource> OnErrorResumeNext_<TSource>(IEnumerable<IEnumerable<TSource>> sources)
        {
            foreach (var source in sources)
            {
                using (var innerEnumerator = source.GetEnumerator())
                {
                    while (true)
                    {
                        var value = default(TSource);
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

        /// <summary>
        /// Creates a sequence that retries enumerating the source sequence as long as an error occurs.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <returns>Sequence concatenating the results of the source sequence as long as an error occurs.</returns>
        public static IEnumerable<TSource> Retry<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return new[] { source }.Repeat().Catch();
        }

        /// <summary>
        /// Creates a sequence that retries enumerating the source sequence as long as an error occurs, with the specified maximum number of retries.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="retryCount">Maximum number of retries.</param>
        /// <returns>Sequence concatenating the results of the source sequence as long as an error occurs.</returns>
        public static IEnumerable<TSource> Retry<TSource>(this IEnumerable<TSource> source, int retryCount)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (retryCount < 0)
                throw new ArgumentOutOfRangeException("retryCount");

            return new[] { source }.Repeat(retryCount).Catch();
        }
    }
}

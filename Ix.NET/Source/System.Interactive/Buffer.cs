// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;

namespace System.Linq
{
    public static partial class EnumerableEx
    {
        /// <summary>
        ///     Generates a sequence of non-overlapping adjacent buffers over the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="count">Number of elements for allocated buffers.</param>
        /// <returns>Sequence of buffers containing source sequence elements.</returns>
        public static IEnumerable<IList<TSource>> Buffer<TSource>(this IEnumerable<TSource> source, int count)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            return BufferExact(source, count);
        }

        /// <summary>
        ///     Generates a sequence of buffers over the source sequence, with specified length and possible overlap.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="count">Number of elements for allocated buffers.</param>
        /// <param name="skip">Number of elements to skip between the start of consecutive buffers.</param>
        /// <returns>Sequence of buffers containing source sequence elements.</returns>
        public static IEnumerable<IList<TSource>> Buffer<TSource>(this IEnumerable<TSource> source, int count, int skip)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            if (skip <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(skip));
            }

            if (count == skip)
            {
                return BufferExact(source, count);
            }
            if (count < skip)
            {
                return BufferSkip(source, count, skip);
            }

            return source.Buffer_(count, skip);
        }

        private static IEnumerable<IList<TSource>> Buffer_<TSource>(this IEnumerable<TSource> source, int count, int skip)
        {
            var buffers = new Queue<IList<TSource>>();

            var i = 0;
            foreach (var item in source)
            {
                if (i % skip == 0)
                {
                    buffers.Enqueue(new List<TSource>(count));
                }

                foreach (var buffer in buffers)
                {
                    buffer.Add(item);
                }

                if (buffers.Count > 0 && buffers.Peek()
                                                .Count == count)
                {
                    yield return buffers.Dequeue();
                }

                i++;
            }

            while (buffers.Count > 0)
            {
                yield return buffers.Dequeue();
            }
        }

        private static IEnumerable<IList<TSource>> BufferExact<TSource>(IEnumerable<TSource> source, int count)
        {
            IList<TSource> buffer = null;

            foreach (var v in source)
            {
                if (buffer == null)
                {
                    buffer = new List<TSource>();
                }

                buffer.Add(v);
                if (buffer.Count == count)
                {
                    yield return buffer;
                    buffer = null;
                }
            }

            if (buffer != null)
            {
                yield return buffer;
            }
        }

        private static IEnumerable<IList<TSource>> BufferSkip<TSource>(IEnumerable<TSource> source, int count, int skip)
        {
            IList<TSource> buffer = null;

            var index = 0;

            foreach (var v in source)
            {
                if (index == 0)
                {
                    buffer = new List<TSource>();
                }

                buffer?.Add(v);
                if (++index == count)
                {
                    yield return buffer;
                    buffer = null;
                }

                if (index == skip)
                {
                    index = 0;
                }
            }

            if (buffer != null)
            {
                yield return buffer;
            }
        }
    }

    /// <summary>
    ///     Represents a buffer exposing a shared view over an underlying enumerable sequence.
    /// </summary>
    /// <typeparam name="T">Element type.</typeparam>
    public interface IBuffer<out T> : IEnumerable<T>, IDisposable
    {
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections;
using System.Collections.Generic;

namespace System.Linq
{
    public static partial class EnumerableEx
    {
        /// <summary>
        /// Creates a buffer with a view over the source sequence, causing each enumerator to obtain access to all of the
        /// sequence's elements without causing multiple enumerations over the source.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <returns>
        /// Buffer enabling each enumerator to retrieve all elements from the shared source sequence, without duplicating
        /// source enumeration side-effects.
        /// </returns>
        /// <example>
        /// var rng = Enumerable.Range(0, 10).Do(x => Console.WriteLine(x)).Memoize();
        /// var e1 = rng.GetEnumerator();
        /// Assert.IsTrue(e1.MoveNext());    // Prints 0
        /// Assert.AreEqual(0, e1.Current);
        /// Assert.IsTrue(e1.MoveNext());    // Prints 1
        /// Assert.AreEqual(1, e1.Current);
        /// var e2 = rng.GetEnumerator();
        /// Assert.IsTrue(e2.MoveNext());    // Doesn't print anything; the side-effect of Do
        /// Assert.AreEqual(0, e2.Current);  // has already taken place during e1's iteration.
        /// Assert.IsTrue(e1.MoveNext());    // Prints 2
        /// Assert.AreEqual(2, e1.Current);
        /// </example>
        public static IBuffer<TSource> Memoize<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new MemoizedBuffer<TSource>(source.GetEnumerator());
        }

        /// <summary>
        /// Memoizes the source sequence within a selector function where each enumerator can get access to all of the
        /// sequence's elements without causing multiple enumerations over the source.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <typeparam name="TResult">Result sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="selector">Selector function with memoized access to the source sequence for each enumerator.</param>
        /// <returns>Sequence resulting from applying the selector function to the memoized view over the source sequence.</returns>
        public static IEnumerable<TResult> Memoize<TSource, TResult>(this IEnumerable<TSource> source, Func<IEnumerable<TSource>, IEnumerable<TResult>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create(() => selector(source.Memoize()).GetEnumerator());
        }

        /// <summary>
        /// Creates a buffer with a view over the source sequence, causing a specified number of enumerators to obtain access
        /// to all of the sequence's elements without causing multiple enumerations over the source.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="readerCount">
        /// Number of enumerators that can access the underlying buffer. Once every enumerator has
        /// obtained an element from the buffer, the element is removed from the buffer.
        /// </param>
        /// <returns>
        /// Buffer enabling a specified number of enumerators to retrieve all elements from the shared source sequence,
        /// without duplicating source enumeration side-effects.
        /// </returns>
        public static IBuffer<TSource> Memoize<TSource>(this IEnumerable<TSource> source, int readerCount)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (readerCount <= 0)
                throw new ArgumentOutOfRangeException(nameof(readerCount));

            return new MemoizedBuffer<TSource>(source.GetEnumerator(), readerCount);
        }

        /// <summary>
        /// Memoizes the source sequence within a selector function where a specified number of enumerators can get access to
        /// all of the sequence's elements without causing multiple enumerations over the source.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <typeparam name="TResult">Result sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="readerCount">
        /// Number of enumerators that can access the underlying buffer. Once every enumerator has
        /// obtained an element from the buffer, the element is removed from the buffer.
        /// </param>
        /// <param name="selector">
        /// Selector function with memoized access to the source sequence for a specified number of
        /// enumerators.
        /// </param>
        /// <returns>Sequence resulting from applying the selector function to the memoized view over the source sequence.</returns>
        public static IEnumerable<TResult> Memoize<TSource, TResult>(this IEnumerable<TSource> source, int readerCount, Func<IEnumerable<TSource>, IEnumerable<TResult>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (readerCount <= 0)
                throw new ArgumentOutOfRangeException(nameof(readerCount));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create(() => selector(source.Memoize(readerCount)).GetEnumerator());
        }

        private sealed class MemoizedBuffer<T> : IBuffer<T>
        {
            private readonly object _gate = new object();
            private readonly IRefCountList<T> _buffer;
            private readonly IEnumerator<T> _source;

            private bool _disposed;
            private Exception? _error;
            private bool _stopped;

            public MemoizedBuffer(IEnumerator<T> source)
                : this(source, new MaxRefCountList<T>())
            {
            }

            public MemoizedBuffer(IEnumerator<T> source, int readerCount)
                : this(source, new RefCountList<T>(readerCount))
            {
            }

            private MemoizedBuffer(IEnumerator<T> source, IRefCountList<T> buffer)
            {
                _source = source;
                _buffer = buffer;
            }

            public IEnumerator<T> GetEnumerator()
            {
                if (_disposed)
                    throw new ObjectDisposedException("");

                return GetEnumerator_();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                if (_disposed)
                    throw new ObjectDisposedException("");

                return GetEnumerator();
            }

            public void Dispose()
            {
                lock (_gate)
                {
                    if (!_disposed)
                    {
                        _source.Dispose();
                        _buffer.Clear();
                    }

                    _disposed = true;
                }
            }

            private IEnumerator<T> GetEnumerator_()
            {
                var i = 0;

                try
                {
                    while (true)
                    {
                        if (_disposed)
                            throw new ObjectDisposedException("");

                        var hasValue = default(bool);
                        var current = default(T)!;

                        lock (_gate)
                        {
                            if (i >= _buffer.Count)
                            {
                                if (!_stopped)
                                {
                                    try
                                    {
                                        hasValue = _source.MoveNext();
                                        if (hasValue)
                                            current = _source.Current;
                                    }
                                    catch (Exception ex)
                                    {
                                        _stopped = true;
                                        _error = ex;

                                        _source.Dispose();
                                    }
                                }

                                if (_stopped)
                                {
                                    if (_error != null)
                                        throw _error;
                                    else
                                        break;
                                }

                                if (hasValue)
                                {
                                    _buffer.Add(current);
                                }
                            }
                            else
                            {
                                hasValue = true;
                            }
                        }

                        if (hasValue)
                            yield return _buffer[i];
                        else
                            break;

                        i++;
                    }
                }
                finally
                {
                    if (_buffer != null)
                        _buffer.Done(i + 1);
                }
            }
        }
    }
}

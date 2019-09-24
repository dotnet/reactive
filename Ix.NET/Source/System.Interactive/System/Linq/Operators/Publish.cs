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
        /// Creates a buffer with a view over the source sequence, causing each enumerator to obtain access to the remainder of
        /// the sequence from the current index in the buffer.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <returns>
        /// Buffer enabling each enumerator to retrieve elements from the shared source sequence, starting from the index
        /// at the point of obtaining the enumerator.
        /// </returns>
        /// <example>
        /// var rng = Enumerable.Range(0, 10).Publish();
        /// var e1 = rng.GetEnumerator();    // e1 has a view on the source starting from element 0
        /// Assert.IsTrue(e1.MoveNext());
        /// Assert.AreEqual(0, e1.Current);
        /// Assert.IsTrue(e1.MoveNext());
        /// Assert.AreEqual(1, e1.Current);
        /// var e2 = rng.GetEnumerator();
        /// Assert.IsTrue(e2.MoveNext());    // e2 has a view on the source starting from element 2
        /// Assert.AreEqual(2, e2.Current);
        /// Assert.IsTrue(e1.MoveNext());    // e1 continues to enumerate over its view
        /// Assert.AreEqual(2, e1.Current);
        /// </example>
        public static IBuffer<TSource> Publish<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new PublishedBuffer<TSource>(source.GetEnumerator());
        }

        /// <summary>
        /// Publishes the source sequence within a selector function where each enumerator can obtain a view over a tail of the
        /// source sequence.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <typeparam name="TResult">Result sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="selector">Selector function with published access to the source sequence for each enumerator.</param>
        /// <returns>Sequence resulting from applying the selector function to the published view over the source sequence.</returns>
        public static IEnumerable<TResult> Publish<TSource, TResult>(this IEnumerable<TSource> source, Func<IEnumerable<TSource>, IEnumerable<TResult>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create(() => selector(source.Publish()).GetEnumerator());
        }

        private sealed class PublishedBuffer<T> : IBuffer<T>
        {
            private readonly object _gate = new object();
            private readonly RefCountList<T> _buffer;
            private readonly IEnumerator<T> _source;

            private bool _disposed;
            private Exception? _error;
            private bool _stopped;

            public PublishedBuffer(IEnumerator<T> source)
            {
                _buffer = new RefCountList<T>(0);
                _source = source;
            }

            public IEnumerator<T> GetEnumerator()
            {
                if (_disposed)
                {
                    throw new ObjectDisposedException("");
                }

                var i = default(int);
                lock (_gate)
                {
                    i = _buffer.Count;
                    _buffer.ReaderCount++;
                }

                return GetEnumeratorCore(i);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                if (_disposed)
                {
                    throw new ObjectDisposedException("");
                }

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

            private IEnumerator<T> GetEnumeratorCore(int i)
            {
                try
                {
                    while (true)
                    {
                        if (_disposed)
                        {
                            throw new ObjectDisposedException("");
                        }

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
                                        {
                                            current = _source.Current;
                                        }
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
                                    {
                                        throw _error;
                                    }
                                    else
                                    {
                                        break;
                                    }
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
                        {
                            yield return _buffer[i];
                        }
                        else
                        {
                            break;
                        }

                        i++;
                    }
                }
                finally
                {
                    if (_buffer != null)
                    {
                        _buffer.Done(i + 1);
                    }
                }
            }
        }
    }
}

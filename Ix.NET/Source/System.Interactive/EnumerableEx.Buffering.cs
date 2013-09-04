// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace System.Linq
{
    public static partial class EnumerableEx
    {
        /// <summary>
        /// Creates a buffer with a shared view over the source sequence, causing each enumerator to fetch the next element from the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <returns>Buffer enabling each enumerator to retrieve elements from the shared source sequence.</returns>
        /// <example>
        /// var rng = Enumerable.Range(0, 10).Share();
        /// 
        /// var e1 = rng.GetEnumerator();    // Both e1 and e2 will consume elements from
        /// var e2 = rng.GetEnumerator();    // the source sequence.
        /// 
        /// Assert.IsTrue(e1.MoveNext());
        /// Assert.AreEqual(0, e1.Current);
        /// 
        /// Assert.IsTrue(e1.MoveNext());
        /// Assert.AreEqual(1, e1.Current);
        /// 
        /// Assert.IsTrue(e2.MoveNext());    // e2 "steals" element 2
        /// Assert.AreEqual(2, e2.Current);
        /// 
        /// Assert.IsTrue(e1.MoveNext());    // e1 can't see element 2
        /// Assert.AreEqual(3, e1.Current);
        /// </example>
        public static IBuffer<TSource> Share<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return new SharedBuffer<TSource>(source.GetEnumerator());
        }

        /// <summary>
        /// Shares the source sequence within a selector function where each enumerator can fetch the next element from the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <typeparam name="TResult">Result sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="selector">Selector function with shared access to the source sequence for each enumerator.</param>
        /// <returns>Sequence resulting from applying the selector function to the shared view over the source sequence.</returns>
        public static IEnumerable<TResult> Share<TSource, TResult>(this IEnumerable<TSource> source, Func<IEnumerable<TSource>, IEnumerable<TResult>> selector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return Create<TResult>(() => selector(source.Share()).GetEnumerator());
        }

        class SharedBuffer<T> : IBuffer<T>
        {
            private IEnumerator<T> _source;
            private bool _disposed;

            public SharedBuffer(IEnumerator<T> source)
            {
                _source = source;
            }

            public IEnumerator<T> GetEnumerator()
            {
                if (_disposed)
                    throw new ObjectDisposedException("");

                return GetEnumerator_();
            }

            private IEnumerator<T> GetEnumerator_()
            {
                while (true)
                {
                    if (_disposed)
                        throw new ObjectDisposedException("");

                    var hasValue = default(bool);
                    var current = default(T);

                    lock (_source)
                    {
                        hasValue = _source.MoveNext();
                        if (hasValue)
                            current = _source.Current;
                    }

                    if (hasValue)
                        yield return current;
                    else
                        break;
                }
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                if (_disposed)
                    throw new ObjectDisposedException("");

                return GetEnumerator();
            }

            public void Dispose()
            {
                lock (_source)
                {
                    if (!_disposed)
                    {
                        _source.Dispose();
                        _source = null;
                    }

                    _disposed = true;
                }
            }
        }

        /// <summary>
        /// Creates a buffer with a view over the source sequence, causing each enumerator to obtain access to the remainder of the sequence from the current index in the buffer.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <returns>Buffer enabling each enumerator to retrieve elements from the shared source sequence, starting from the index at the point of obtaining the enumerator.</returns>
        /// <example>
        /// var rng = Enumerable.Range(0, 10).Publish();
        /// 
        /// var e1 = rng.GetEnumerator();    // e1 has a view on the source starting from element 0
        /// 
        /// Assert.IsTrue(e1.MoveNext());
        /// Assert.AreEqual(0, e1.Current);
        /// 
        /// Assert.IsTrue(e1.MoveNext());
        /// Assert.AreEqual(1, e1.Current);
        /// 
        /// var e2 = rng.GetEnumerator();
        /// 
        /// Assert.IsTrue(e2.MoveNext());    // e2 has a view on the source starting from element 2
        /// Assert.AreEqual(2, e2.Current);
        /// 
        /// Assert.IsTrue(e1.MoveNext());    // e1 continues to enumerate over its view
        /// Assert.AreEqual(2, e1.Current);
        /// </example>
        public static IBuffer<TSource> Publish<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return new PublishedBuffer<TSource>(source.GetEnumerator());
        }

        /// <summary>
        /// Publishes the source sequence within a selector function where each enumerator can obtain a view over a tail of the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <typeparam name="TResult">Result sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="selector">Selector function with published access to the source sequence for each enumerator.</param>
        /// <returns>Sequence resulting from applying the selector function to the published view over the source sequence.</returns>
        public static IEnumerable<TResult> Publish<TSource, TResult>(this IEnumerable<TSource> source, Func<IEnumerable<TSource>, IEnumerable<TResult>> selector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return Create<TResult>(() => selector(source.Publish()).GetEnumerator());
        }

        class PublishedBuffer<T> : IBuffer<T>
        {
            private IEnumerator<T> _source;
            private RefCountList<T> _buffer;
            private bool _stopped;
            private Exception _error;
            private bool _disposed;

            public PublishedBuffer(IEnumerator<T> source)
            {
                _buffer = new RefCountList<T>(0);
                _source = source;
            }

            public IEnumerator<T> GetEnumerator()
            {
                if (_disposed)
                    throw new ObjectDisposedException("");

                var i = default(int);
                lock (_source)
                {
                    i = _buffer.Count;
                    _buffer.ReaderCount++;
                } 
                
                return GetEnumerator_(i);
            }

            private IEnumerator<T> GetEnumerator_(int i)
            {
                try
                {
                    while (true)
                    {
                        if (_disposed)
                            throw new ObjectDisposedException("");

                        var hasValue = default(bool);
                        var current = default(T);

                        lock (_source)
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

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                if (_disposed)
                    throw new ObjectDisposedException("");

                return GetEnumerator();
            }

            public void Dispose()
            {
                lock (_source)
                {
                    if (!_disposed)
                    {
                        _source.Dispose();
                        _source = null;

                        _buffer.Clear();
                        _buffer = null;
                    }

                    _disposed = true;
                }
            }
        }

        /// <summary>
        /// Creates a buffer with a view over the source sequence, causing each enumerator to obtain access to all of the sequence's elements without causing multiple enumerations over the source.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <returns>Buffer enabling each enumerator to retrieve all elements from the shared source sequence, without duplicating source enumeration side-effects.</returns>
        /// <example>
        /// var rng = Enumerable.Range(0, 10).Do(x => Console.WriteLine(x)).Memoize();
        /// 
        /// var e1 = rng.GetEnumerator();
        /// 
        /// Assert.IsTrue(e1.MoveNext());    // Prints 0
        /// Assert.AreEqual(0, e1.Current);
        /// 
        /// Assert.IsTrue(e1.MoveNext());    // Prints 1
        /// Assert.AreEqual(1, e1.Current);
        /// 
        /// var e2 = rng.GetEnumerator();
        /// 
        /// Assert.IsTrue(e2.MoveNext());    // Doesn't print anything; the side-effect of Do
        /// Assert.AreEqual(0, e2.Current);  // has already taken place during e1's iteration.
        /// 
        /// Assert.IsTrue(e1.MoveNext());    // Prints 2
        /// Assert.AreEqual(2, e1.Current);
        /// </example>
        public static IBuffer<TSource> Memoize<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return new MemoizedBuffer<TSource>(source.GetEnumerator());
        }

        /// <summary>
        /// Memoizes the source sequence within a selector function where each enumerator can get access to all of the sequence's elements without causing multiple enumerations over the source.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <typeparam name="TResult">Result sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="selector">Selector function with memoized access to the source sequence for each enumerator.</param>
        /// <returns>Sequence resulting from applying the selector function to the memoized view over the source sequence.</returns>
        public static IEnumerable<TResult> Memoize<TSource, TResult>(this IEnumerable<TSource> source, Func<IEnumerable<TSource>, IEnumerable<TResult>> selector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return Create<TResult>(() => selector(source.Memoize()).GetEnumerator());
        }

        /// <summary>
        /// Creates a buffer with a view over the source sequence, causing a specified number of enumerators to obtain access to all of the sequence's elements without causing multiple enumerations over the source.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="readerCount">Number of enumerators that can access the underlying buffer. Once every enumerator has obtained an element from the buffer, the element is removed from the buffer.</param>
        /// <returns>Buffer enabling a specified number of enumerators to retrieve all elements from the shared source sequence, without duplicating source enumeration side-effects.</returns>
        public static IBuffer<TSource> Memoize<TSource>(this IEnumerable<TSource> source, int readerCount)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (readerCount <= 0)
                throw new ArgumentOutOfRangeException("readerCount");

            return new MemoizedBuffer<TSource>(source.GetEnumerator(), readerCount);
        }

        /// <summary>
        /// Memoizes the source sequence within a selector function where a specified number of enumerators can get access to all of the sequence's elements without causing multiple enumerations over the source.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <typeparam name="TResult">Result sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="readerCount">Number of enumerators that can access the underlying buffer. Once every enumerator has obtained an element from the buffer, the element is removed from the buffer.</param>
        /// <param name="selector">Selector function with memoized access to the source sequence for a specified number of enumerators.</param>
        /// <returns>Sequence resulting from applying the selector function to the memoized view over the source sequence.</returns>
        public static IEnumerable<TResult> Memoize<TSource, TResult>(this IEnumerable<TSource> source, int readerCount, Func<IEnumerable<TSource>, IEnumerable<TResult>> selector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (readerCount <= 0)
                throw new ArgumentOutOfRangeException("readerCount");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return Create<TResult>(() => selector(source.Memoize(readerCount)).GetEnumerator());
        }

        class MemoizedBuffer<T> : IBuffer<T>
        {
            private IEnumerator<T> _source;
            private IRefCountList<T> _buffer;
            private bool _stopped;
            private Exception _error;
            private bool _disposed;

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
                        var current = default(T);

                        lock (_source)
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

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                if (_disposed)
                    throw new ObjectDisposedException("");

                return GetEnumerator();
            }

            public void Dispose()
            {
                lock (_source)
                {
                    if (!_disposed)
                    {
                        _source.Dispose();
                        _source = null;

                        _buffer.Clear();
                        _buffer = null;
                    }

                    _disposed = true;
                }
            }
        }
    }

    /// <summary>
    /// Represents a buffer exposing a shared view over an underlying enumerable sequence.
    /// </summary>
    /// <typeparam name="T">Element type.</typeparam>
    public interface IBuffer<
#if !NO_VARIANCE && !SILVERLIGHT4 // SL4 has defined IEnumerable with invariant T
        out
#endif
        T> : IEnumerable<T>, IDisposable
    {
    }

    interface IRefCountList<T>
    {
        void Clear();

        int Count { get; }

        T this[int i]
        {
            get;
        }

        void Add(T item);

        void Done(int index);
    }

    class MaxRefCountList<T> : IRefCountList<T>
    {
        private IList<T> _list = new List<T>();

        public void Clear()
        {
            _list.Clear();
        }

        public int Count
        {
            get { return _list.Count; }
        }

        public T this[int i]
        {
            get { return _list[i]; }
        }

        public void Add(T item)
        {
            _list.Add(item);
        }

        public void Done(int index)
        {
        }
    }

    class RefCountList<T> : IRefCountList<T>
    {
        private int _readerCount;
        private readonly IDictionary<int, RefCount> _list;
        private int _count;

        public RefCountList(int readerCount)
        {
            _readerCount = readerCount;
            _list = new Dictionary<int, RefCount>();
        }

        public int ReaderCount
        {
            get
            {
                return _readerCount;
            }

            set
            {
                _readerCount = value;
            }
        }

        public void Clear()
        {
            _list.Clear();
        }

        public int Count
        {
            get { return _count; }
        }

        public T this[int i]
        {
            get
            {
                Debug.Assert(i < _count);

                var res = default(RefCount);
                if (!_list.TryGetValue(i, out res))
                    throw new InvalidOperationException("Element no longer available in the buffer.");

                var val = res.Value;
                if (--res.Count == 0)
                    _list.Remove(i);

                return val;
            }
        }

        public void Add(T item)
        {
            _list[_count] = new RefCount { Value = item, Count = _readerCount };
            _count++;
        }

        public void Done(int index)
        {
            for (int i = index; i < _count; i++)
            {
                var ignore = this[i];
            }

            _readerCount--;
        }

        class RefCount
        {
            public int Count;
            public T Value;
        }
    }
}

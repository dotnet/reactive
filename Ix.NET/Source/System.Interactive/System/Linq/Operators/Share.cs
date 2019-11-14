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
        ///     Creates a buffer with a shared view over the source sequence, causing each enumerator to fetch the next element
        ///     from the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <returns>Buffer enabling each enumerator to retrieve elements from the shared source sequence.</returns>
        /// <example>
        ///     var rng = Enumerable.Range(0, 10).Share();
        ///     var e1 = rng.GetEnumerator();    // Both e1 and e2 will consume elements from
        ///     var e2 = rng.GetEnumerator();    // the source sequence.
        ///     Assert.IsTrue(e1.MoveNext());
        ///     Assert.AreEqual(0, e1.Current);
        ///     Assert.IsTrue(e1.MoveNext());
        ///     Assert.AreEqual(1, e1.Current);
        ///     Assert.IsTrue(e2.MoveNext());    // e2 "steals" element 2
        ///     Assert.AreEqual(2, e2.Current);
        ///     Assert.IsTrue(e1.MoveNext());    // e1 can't see element 2
        ///     Assert.AreEqual(3, e1.Current);
        /// </example>
        public static IBuffer<TSource> Share<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new SharedBuffer<TSource>(source.GetEnumerator());
        }

        /// <summary>
        ///     Shares the source sequence within a selector function where each enumerator can fetch the next element from the
        ///     source sequence.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <typeparam name="TResult">Result sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="selector">Selector function with shared access to the source sequence for each enumerator.</param>
        /// <returns>Sequence resulting from applying the selector function to the shared view over the source sequence.</returns>
        public static IEnumerable<TResult> Share<TSource, TResult>(this IEnumerable<TSource> source, Func<IEnumerable<TSource>, IEnumerable<TResult>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create(() => selector(source.Share()).GetEnumerator());
        }

        private class SharedBuffer<T> : IBuffer<T>
        {
            private readonly IEnumerator<T> _source;
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

            IEnumerator IEnumerable.GetEnumerator()
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
                    }

                    _disposed = true;
                }
            }

            private IEnumerator<T> GetEnumerator_() => new ShareEnumerator(this);

            private sealed class ShareEnumerator : IEnumerator<T>
            {
                private readonly SharedBuffer<T> _parent;

                private bool _disposed;

                public ShareEnumerator(SharedBuffer<T> parent)
                {
                    _parent = parent;
                    Current = default!;
                }

                public T Current { get; private set; }

                object? IEnumerator.Current => Current;

                public void Dispose() => _disposed = true;

                public bool MoveNext()
                {
                    if (_disposed)
                    {
                        return false;
                    }
                    if (_parent._disposed)
                    {
                        throw new ObjectDisposedException("");
                    }

                    var hasValue = false;
                    var src = _parent._source;
                    lock (src)
                    {
                        hasValue = src.MoveNext();
                        if (hasValue)
                        {
                            Current = src.Current;
                        }
                    }
                    if (hasValue)
                    {
                        return true;
                    }
                    _disposed = true;
                    Current = default!;
                    return false;
                }

                public void Reset() => throw new NotSupportedException();
            }
        }
    }
}

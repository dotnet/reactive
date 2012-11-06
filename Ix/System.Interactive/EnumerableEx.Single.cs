// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace System.Linq
{
    public static partial class EnumerableEx
    {
        /// <summary>
        /// Hides the enumerable sequence object identity.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <returns>Enumerable sequence with the same behavior as the original, but hiding the source object identity.</returns>
        /// <remarks>AsEnumerable doesn't hide the object identity, and simply acts as a cast to the IEnumerable&lt;TSource&gt; interface.</remarks>
        public static IEnumerable<TSource> Hide<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return source.Hide_();
        }

        private static IEnumerable<TSource> Hide_<TSource>(this IEnumerable<TSource> source)
        {
            foreach (var item in source)
                yield return item;
        }

        /// <summary>
        /// Enumerates the sequence and invokes the given action for each value in the sequence.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="onNext">Action to invoke for each element.</param>
        public static void ForEach<TSource>(this IEnumerable<TSource> source, Action<TSource> onNext)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (onNext == null)
                throw new ArgumentNullException("onNext");

            foreach (var item in source)
                onNext(item);
        }

        /// <summary>
        /// Enumerates the sequence and invokes the given action for each value in the sequence.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="onNext">Action to invoke for each element.</param>
        public static void ForEach<TSource>(this IEnumerable<TSource> source, Action<TSource, int> onNext)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (onNext == null)
                throw new ArgumentNullException("onNext");

            var i = 0;
            foreach (var item in source)
                onNext(item, i++);
        }

        /// <summary>
        /// Lazily invokes an action for each value in the sequence.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="onNext">Action to invoke for each element.</param>
        /// <returns>Sequence exhibiting the specified side-effects upon enumeration.</returns>
        public static IEnumerable<TSource> Do<TSource>(this IEnumerable<TSource> source, Action<TSource> onNext)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (onNext == null)
                throw new ArgumentNullException("onNext");

            return DoHelper(source, onNext, _ => { }, () => { });
        }

        /// <summary>
        /// Lazily invokes an action for each value in the sequence, and executes an action for successful termination.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="onNext">Action to invoke for each element.</param>
        /// <param name="onCompleted">Action to invoke on successful termination of the sequence.</param>
        /// <returns>Sequence exhibiting the specified side-effects upon enumeration.</returns>
        public static IEnumerable<TSource> Do<TSource>(this IEnumerable<TSource> source, Action<TSource> onNext, Action onCompleted)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (onNext == null)
                throw new ArgumentNullException("onNext");
            if (onCompleted == null)
                throw new ArgumentNullException("onCompleted");

            return DoHelper(source, onNext, _ => { }, onCompleted);
        }

        /// <summary>
        /// Lazily invokes an action for each value in the sequence, and executes an action upon exceptional termination.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="onNext">Action to invoke for each element.</param>
        /// <param name="onError">Action to invoke on exceptional termination of the sequence.</param>
        /// <returns>Sequence exhibiting the specified side-effects upon enumeration.</returns>
        public static IEnumerable<TSource> Do<TSource>(this IEnumerable<TSource> source, Action<TSource> onNext, Action<Exception> onError)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (onNext == null)
                throw new ArgumentNullException("onNext");
            if (onError == null)
                throw new ArgumentNullException("onError");

            return DoHelper(source, onNext, onError, () => { });
        }

        /// <summary>
        /// Lazily invokes an action for each value in the sequence, and executes an action upon successful or exceptional termination.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="onNext">Action to invoke for each element.</param>
        /// <param name="onError">Action to invoke on exceptional termination of the sequence.</param>
        /// <param name="onCompleted">Action to invoke on successful termination of the sequence.</param>
        /// <returns>Sequence exhibiting the specified side-effects upon enumeration.</returns>
        public static IEnumerable<TSource> Do<TSource>(this IEnumerable<TSource> source, Action<TSource> onNext, Action<Exception> onError, Action onCompleted)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (onNext == null)
                throw new ArgumentNullException("onNext");
            if (onError == null)
                throw new ArgumentNullException("onError");
            if (onCompleted == null)
                throw new ArgumentNullException("onCompleted");

            return DoHelper(source, onNext, onError, onCompleted);
        }

#if !NO_RXINTERFACES
        /// <summary>
        /// Lazily invokes observer methods for each value in the sequence, and upon successful or exceptional termination.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="observer">Observer to invoke notification calls on.</param>
        /// <returns>Sequence exhibiting the side-effects of observer method invocation upon enumeration.</returns>
        public static IEnumerable<TSource> Do<TSource>(this IEnumerable<TSource> source, IObserver<TSource> observer)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (observer == null)
                throw new ArgumentNullException("observer");

            return DoHelper(source, observer.OnNext, observer.OnError, observer.OnCompleted);
        }
#endif

        private static IEnumerable<TSource> DoHelper<TSource>(this IEnumerable<TSource> source, Action<TSource> onNext, Action<Exception> onError, Action onCompleted)
        {
            using (var e = source.GetEnumerator())
            {
                while (true)
                {
                    var current = default(TSource);
                    try
                    {
                        if (!e.MoveNext())
                            break;

                        current = e.Current;
                    }
                    catch (Exception ex)
                    {
                        onError(ex);
                        throw;
                    }

                    onNext(current);
                    yield return current;
                }

                onCompleted();
            }
        }

        /// <summary>
        /// Generates a sequence of non-overlapping adjacent buffers over the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="count">Number of elements for allocated buffers.</param>
        /// <returns>Sequence of buffers containing source sequence elements.</returns>
        public static IEnumerable<IList<TSource>> Buffer<TSource>(this IEnumerable<TSource> source, int count)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (count <= 0)
                throw new ArgumentOutOfRangeException("count");

            return source.Buffer_(count, count);
        }

        /// <summary>
        /// Generates a sequence of buffers over the source sequence, with specified length and possible overlap.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="count">Number of elements for allocated buffers.</param>
        /// <param name="skip">Number of elements to skip between the start of consecutive buffers.</param>
        /// <returns>Sequence of buffers containing source sequence elements.</returns>
        public static IEnumerable<IList<TSource>> Buffer<TSource>(this IEnumerable<TSource> source, int count, int skip)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (count <= 0)
                throw new ArgumentOutOfRangeException("count");
            if (skip <= 0)
                throw new ArgumentOutOfRangeException("skip");

            return source.Buffer_(count, skip);
        }

        private static IEnumerable<IList<TSource>> Buffer_<TSource>(this IEnumerable<TSource> source, int count, int skip)
        {
            var buffers = new Queue<IList<TSource>>();

            var i = 0;
            foreach (var item in source)
            {
                if (i % skip == 0)
                    buffers.Enqueue(new List<TSource>(count));

                foreach (var buffer in buffers)
                    buffer.Add(item);

                if (buffers.Count > 0 && buffers.Peek().Count == count)
                    yield return buffers.Dequeue();

                i++;
            }

            while (buffers.Count > 0)
                yield return buffers.Dequeue();
        }

        /// <summary>
        /// Ignores all elements in the source sequence.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <returns>Source sequence without its elements.</returns>
        public static IEnumerable<TSource> IgnoreElements<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return source.IgnoreElements_();
        }

        private static IEnumerable<TSource> IgnoreElements_<TSource>(this IEnumerable<TSource> source)
        {
            foreach (var item in source)
                ;

            yield break;
        }

        /// <summary>
        /// Returns elements with a distinct key value by using the default equality comparer to compare key values.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <typeparam name="TKey">Key type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="keySelector">Key selector.</param>
        /// <returns>Sequence that contains the elements from the source sequence with distinct key values.</returns>
        public static IEnumerable<TSource> Distinct<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");

            return source.Distinct_(keySelector, EqualityComparer<TKey>.Default);
        }

        /// <summary>
        /// Returns elements with a distinct key value by using the specified equality comparer to compare key values.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <typeparam name="TKey">Key type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="keySelector">Key selector.</param>
        /// <param name="comparer">Comparer used to compare key values.</param>
        /// <returns>Sequence that contains the elements from the source sequence with distinct key values.</returns>
        public static IEnumerable<TSource> Distinct<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            return source.Distinct_(keySelector, comparer);
        }

        private static IEnumerable<TSource> Distinct_<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            var set = new HashSet<TKey>(comparer);

            foreach (var item in source)
            {
                var key = keySelector(item);
                if (set.Add(key))
                    yield return item;
            }
        }

#if NO_HASHSET
        class HashSet<T>
        {
            private Dictionary<T, object> _set;

            public HashSet(IEqualityComparer<T> comparer)
            {
                _set = new Dictionary<T, object>(comparer);
            }

            public bool Add(T value)
            {
                if (_set.ContainsKey(value))
                    return false;

                _set[value] = null;
                return true;
            }
        }
#endif

        /// <summary>
        /// Returns consecutive distinct elements by using the default equality comparer to compare values.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <returns>Sequence without adjacent non-distinct elements.</returns>
        public static IEnumerable<TSource> DistinctUntilChanged<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return source.DistinctUntilChanged_(x => x, EqualityComparer<TSource>.Default);
        }

        /// <summary>
        /// Returns consecutive distinct elements by using the specified equality comparer to compare values.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="comparer">Comparer used to compare values.</param>
        /// <returns>Sequence without adjacent non-distinct elements.</returns>
        public static IEnumerable<TSource> DistinctUntilChanged<TSource>(this IEnumerable<TSource> source, IEqualityComparer<TSource> comparer)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            return source.DistinctUntilChanged_(x => x, comparer);
        }

        /// <summary>
        /// Returns consecutive distinct elements based on a key value by using the specified equality comparer to compare key values.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <typeparam name="TKey">Key type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="keySelector">Key selector.</param>
        /// <returns>Sequence without adjacent non-distinct elements.</returns>
        public static IEnumerable<TSource> DistinctUntilChanged<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");

            return source.DistinctUntilChanged_(keySelector, EqualityComparer<TKey>.Default);
        }

        /// <summary>
        /// Returns consecutive distinct elements based on a key value by using the specified equality comparer to compare key values.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <typeparam name="TKey">Key type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="keySelector">Key selector.</param>
        /// <param name="comparer">Comparer used to compare key values.</param>
        /// <returns>Sequence without adjacent non-distinct elements.</returns>
        public static IEnumerable<TSource> DistinctUntilChanged<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            return source.DistinctUntilChanged_(keySelector, comparer);
        }

        private static IEnumerable<TSource> DistinctUntilChanged_<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            var currentKey = default(TKey);
            var hasCurrentKey = false;

            foreach (var item in source)
            {
                var key = keySelector(item);

                var comparerEquals = false;
                if (hasCurrentKey)
                {
                    comparerEquals = comparer.Equals(currentKey, key);
                }

                if (!hasCurrentKey || !comparerEquals)
                {
                    hasCurrentKey = true;
                    currentKey = key;
                    yield return item;
                }
            }
        }

        /// <summary>
        /// Expands the sequence by recursively applying a selector function.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="selector">Selector function to retrieve the next sequence to expand.</param>
        /// <returns>Sequence with results from the recursive expansion of the source sequence.</returns>
        public static IEnumerable<TSource> Expand<TSource>(this IEnumerable<TSource> source, Func<TSource, IEnumerable<TSource>> selector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return source.Expand_(selector);
        }

        private static IEnumerable<TSource> Expand_<TSource>(this IEnumerable<TSource> source, Func<TSource, IEnumerable<TSource>> selector)
        {
            var queue = new Queue<IEnumerable<TSource>>();
            queue.Enqueue(source);

            while (queue.Count > 0)
            {
                var src = queue.Dequeue();

                foreach (var item in src)
                {
                    queue.Enqueue(selector(item));
                    yield return item;
                }
            }
        }

        /// <summary>
        /// Returns the source sequence prefixed with the specified value.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="values">Values to prefix the sequence with.</param>
        /// <returns>Sequence starting with the specified prefix value, followed by the source sequence.</returns>
        public static IEnumerable<TSource> StartWith<TSource>(this IEnumerable<TSource> source, params TSource[] values)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return source.StartWith_(values);
        }

        private static IEnumerable<TSource> StartWith_<TSource>(this IEnumerable<TSource> source, params TSource[] values)
        {
            foreach (var x in values)
                yield return x;

            foreach (var item in source)
                yield return item;
        }

        /// <summary>
        /// Generates a sequence of accumulated values by scanning the source sequence and applying an accumulator function.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <typeparam name="TAccumulate">Accumulation type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="seed">Accumulator seed value.</param>
        /// <param name="accumulator">Accumulation function to apply to the current accumulation value and each element of the sequence.</param>
        /// <returns>Sequence with all intermediate accumulation values resulting from scanning the sequence.</returns>
        public static IEnumerable<TAccumulate> Scan<TSource, TAccumulate>(this IEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> accumulator)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (accumulator == null)
                throw new ArgumentNullException("accumulator");

            return source.Scan_(seed, accumulator);
        }

        private static IEnumerable<TAccumulate> Scan_<TSource, TAccumulate>(this IEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> accumulator)
        {
            var acc = seed;

            foreach (var item in source)
            {
                acc = accumulator(acc, item);
                yield return acc;
            }
        }

        /// <summary>
        /// Generates a sequence of accumulated values by scanning the source sequence and applying an accumulator function.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="accumulator">Accumulation function to apply to the current accumulation value and each element of the sequence.</param>
        /// <returns>Sequence with all intermediate accumulation values resulting from scanning the sequence.</returns>
        public static IEnumerable<TSource> Scan<TSource>(this IEnumerable<TSource> source, Func<TSource, TSource, TSource> accumulator)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (accumulator == null)
                throw new ArgumentNullException("accumulator");

            return source.Scan_(accumulator);
        }

        private static IEnumerable<TSource> Scan_<TSource>(this IEnumerable<TSource> source, Func<TSource, TSource, TSource> accumulator)
        {
            var hasSeed = false;
            var acc = default(TSource);

            foreach (var item in source)
            {
                if (!hasSeed)
                {
                    hasSeed = true;
                    acc = item;
                    continue;
                }

                acc = accumulator(acc, item);
                yield return acc;
            }
        }

        /// <summary>
        /// Returns a specified number of contiguous elements from the end of the sequence.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="count">The number of elements to take from the end of the sequence.</param>
        /// <returns>Sequence with the specified number of elements counting from the end of the source sequence.</returns>
        public static IEnumerable<TSource> TakeLast<TSource>(this IEnumerable<TSource> source, int count)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (count < 0)
                throw new ArgumentOutOfRangeException("count");

            return source.TakeLast_(count);
        }

        private static IEnumerable<TSource> TakeLast_<TSource>(this IEnumerable<TSource> source, int count)
        {
            var q = new Queue<TSource>(count);

            foreach (var item in source)
            {
                if (q.Count >= count)
                    q.Dequeue();
                q.Enqueue(item);
            }

            while (q.Count > 0)
                yield return q.Dequeue();
        }

        /// <summary>
        /// Bypasses a specified number of contiguous elements from the end of the sequence and returns the remaining elements.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="count">The number of elements to skip from the end of the sequence before returning the remaining elements.</param>
        /// <returns>Sequence bypassing the specified number of elements counting from the end of the source sequence.</returns>
        public static IEnumerable<TSource> SkipLast<TSource>(this IEnumerable<TSource> source, int count)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (count < 0)
                throw new ArgumentOutOfRangeException("count");

            return source.SkipLast_(count);
        }

        private static IEnumerable<TSource> SkipLast_<TSource>(this IEnumerable<TSource> source, int count)
        {
            var q = new Queue<TSource>();

            foreach (var x in source)
            {
                q.Enqueue(x);
                if (q.Count > count)
                    yield return q.Dequeue();
            }
        }

        /// <summary>
        /// Repeats and concatenates the source sequence infinitely.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <returns>Sequence obtained by concatenating the source sequence to itself infinitely.</returns>
        public static IEnumerable<TSource> Repeat<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return Repeat_(source);
        }

        private static IEnumerable<TSource> Repeat_<TSource>(IEnumerable<TSource> source)
        {
            while (true)
                foreach (var item in source)
                    yield return item;
        }

        /// <summary>
        /// Repeats and concatenates the source sequence the given number of times.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="count">Number of times to repeat the source sequence.</param>
        /// <returns>Sequence obtained by concatenating the source sequence to itself the specified number of times.</returns>
        public static IEnumerable<TSource> Repeat<TSource>(this IEnumerable<TSource> source, int count)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (count < 0)
                throw new ArgumentOutOfRangeException("count");

            return Repeat_(source, count);
        }

        private static IEnumerable<TSource> Repeat_<TSource>(IEnumerable<TSource> source, int count)
        {
            for (var i = 0; i < count; i++)
                foreach (var item in source)
                    yield return item;
        }
    }
}

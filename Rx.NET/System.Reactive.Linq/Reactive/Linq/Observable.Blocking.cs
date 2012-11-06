// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace System.Reactive.Linq
{
    public static partial class Observable
    {
        #region + Chunkify +

        /// <summary>
        /// Produces an enumerable sequence of consecutive (possibly empty) chunks of the source sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source observable sequence.</param>
        /// <returns>The enumerable sequence that returns consecutive (possibly empty) chunks upon each iteration.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static IEnumerable<IList<TSource>> Chunkify<TSource>(this IObservable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return s_impl.Chunkify<TSource>(source);
        }

        #endregion

        #region + Collect +

        /// <summary>
        /// Produces an enumerable sequence that returns elements collected/aggregated from the source sequence between consecutive iterations.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TResult">The type of the elements produced by the merge operation during collection.</typeparam>
        /// <param name="source">Source observable sequence.</param>
        /// <param name="newCollector">Factory to create a new collector object.</param>
        /// <param name="merge">Merges a sequence element with the current collector.</param>
        /// <returns>The enumerable sequence that returns collected/aggregated elements from the source sequence upon each iteration.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="newCollector"/> or <paramref name="merge"/> is null.</exception>
        public static IEnumerable<TResult> Collect<TSource, TResult>(this IObservable<TSource> source, Func<TResult> newCollector, Func<TResult, TSource, TResult> merge)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (newCollector == null)
                throw new ArgumentNullException("newCollector");
            if (merge == null)
                throw new ArgumentNullException("merge");

            return s_impl.Collect<TSource, TResult>(source, newCollector, merge);
        }

        /// <summary>
        /// Produces an enumerable sequence that returns elements collected/aggregated from the source sequence between consecutive iterations.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TResult">The type of the elements produced by the merge operation during collection.</typeparam>
        /// <param name="source">Source observable sequence.</param>
        /// <param name="getInitialCollector">Factory to create the initial collector object.</param>
        /// <param name="merge">Merges a sequence element with the current collector.</param>
        /// <param name="getNewCollector">Factory to replace the current collector by a new collector.</param>
        /// <returns>The enumerable sequence that returns collected/aggregated elements from the source sequence upon each iteration.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="getInitialCollector"/> or <paramref name="merge"/> or <paramref name="getNewCollector"/> is null.</exception>
        public static IEnumerable<TResult> Collect<TSource, TResult>(this IObservable<TSource> source, Func<TResult> getInitialCollector, Func<TResult, TSource, TResult> merge, Func<TResult, TResult> getNewCollector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (getInitialCollector == null)
                throw new ArgumentNullException("getInitialCollector");
            if (merge == null)
                throw new ArgumentNullException("merge");
            if (getNewCollector == null)
                throw new ArgumentNullException("getNewCollector");

            return s_impl.Collect<TSource, TResult>(source, getInitialCollector, merge, getNewCollector);
        }

        #endregion

        #region First

        /// <summary>
        /// Returns the first element of an observable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source observable sequence.</param>
        /// <returns>The first element in the observable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="InvalidOperationException">The source sequence is empty.</exception>
        /// <seealso cref="Observable.FirstAsync{TSource}(IObservable{TSource})"/>
#if PREFER_ASYNC
        [Obsolete(Constants_Linq.USE_ASYNC)]
#endif
        public static TSource First<TSource>(this IObservable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return s_impl.First<TSource>(source);
        }

        /// <summary>
        /// Returns the first element of an observable sequence that satisfies the condition in the predicate.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source observable sequence.</param>
        /// <param name="predicate">A predicate function to evaluate for elements in the source sequence.</param>
        /// <returns>The first element in the observable sequence that satisfies the condition in the predicate.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="predicate"/> is null.</exception>
        /// <exception cref="InvalidOperationException">No element satisfies the condition in the predicate. -or- The source sequence is empty.</exception>
        /// <seealso cref="Observable.FirstAsync{TSource}(IObservable{TSource}, Func{TSource, bool})"/>
#if PREFER_ASYNC
        [Obsolete(Constants_Linq.USE_ASYNC)]
#endif
        public static TSource First<TSource>(this IObservable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            return s_impl.First<TSource>(source, predicate);
        }

        #endregion

        #region FirstOrDefault

        /// <summary>
        /// Returns the first element of an observable sequence, or a default value if no such element exists.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source observable sequence.</param>
        /// <returns>The first element in the observable sequence, or a default value if no such element exists.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <seealso cref="Observable.FirstOrDefaultAsync{TSource}(IObservable{TSource})"/>
#if PREFER_ASYNC
        [Obsolete(Constants_Linq.USE_ASYNC)]
#endif
        public static TSource FirstOrDefault<TSource>(this IObservable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return s_impl.FirstOrDefault<TSource>(source);
        }

        /// <summary>
        /// Returns the first element of an observable sequence that satisfies the condition in the predicate, or a default value if no such element exists.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source observable sequence.</param>
        /// <param name="predicate">A predicate function to evaluate for elements in the source sequence.</param>
        /// <returns>The first element in the observable sequence that satisfies the condition in the predicate, or a default value if no such element exists.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="predicate"/> is null.</exception>
        /// <seealso cref="Observable.FirstOrDefaultAsync{TSource}(IObservable{TSource}, Func{TSource, bool})"/>
#if PREFER_ASYNC
        [Obsolete(Constants_Linq.USE_ASYNC)]
#endif
        public static TSource FirstOrDefault<TSource>(this IObservable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            return s_impl.FirstOrDefault<TSource>(source, predicate);
        }

        #endregion

        #region + ForEach +

        /// <summary>
        /// Invokes an action for each element in the observable sequence, and blocks until the sequence is terminated.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="onNext">Action to invoke for each element in the observable sequence.</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="onNext"/> is null.</exception>
        /// <remarks>Because of its blocking nature, this operator is mainly used for testing.</remarks>
#if PREFER_ASYNC
        [Obsolete(Constants_Linq.USE_ASYNC)]
#endif
        public static void ForEach<TSource>(this IObservable<TSource> source, Action<TSource> onNext)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (onNext == null)
                throw new ArgumentNullException("onNext");

            s_impl.ForEach<TSource>(source, onNext);
        }

        /// <summary>
        /// Invokes an action for each element in the observable sequence, incorporating the element's index, and blocks until the sequence is terminated.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="onNext">Action to invoke for each element in the observable sequence.</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="onNext"/> is null.</exception>
        /// <remarks>Because of its blocking nature, this operator is mainly used for testing.</remarks>
#if PREFER_ASYNC
        [Obsolete(Constants_Linq.USE_ASYNC)]
#endif
        public static void ForEach<TSource>(this IObservable<TSource> source, Action<TSource, int> onNext)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (onNext == null)
                throw new ArgumentNullException("onNext");

            s_impl.ForEach<TSource>(source, onNext);
        }

        #endregion

        #region + GetEnumerator +

        /// <summary>
        /// Returns an enumerator that enumerates all values of the observable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">An observable sequence to get an enumerator for.</param>
        /// <returns>The enumerator that can be used to enumerate over the elements in the observable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static IEnumerator<TSource> GetEnumerator<TSource>(this IObservable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return s_impl.GetEnumerator<TSource>(source);
        }

        #endregion

        #region Last

        /// <summary>
        /// Returns the last element of an observable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source observable sequence.</param>
        /// <returns>The last element in the observable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="InvalidOperationException">The source sequence is empty.</exception>
        /// <seealso cref="Observable.LastAsync{TSource}(IObservable{TSource})"/>
#if PREFER_ASYNC
        [Obsolete(Constants_Linq.USE_ASYNC)]
#endif
        public static TSource Last<TSource>(this IObservable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return s_impl.Last<TSource>(source);
        }

        /// <summary>
        /// Returns the last element of an observable sequence that satisfies the condition in the predicate.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source observable sequence.</param>
        /// <param name="predicate">A predicate function to evaluate for elements in the source sequence.</param>
        /// <returns>The last element in the observable sequence that satisfies the condition in the predicate.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="predicate"/> is null.</exception>
        /// <exception cref="InvalidOperationException">No element satisfies the condition in the predicate. -or- The source sequence is empty.</exception>
        /// <seealso cref="Observable.LastAsync{TSource}(IObservable{TSource}, Func{TSource, bool})"/>
#if PREFER_ASYNC
        [Obsolete(Constants_Linq.USE_ASYNC)]
#endif
        public static TSource Last<TSource>(this IObservable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            return s_impl.Last<TSource>(source, predicate);
        }

        #endregion

        #region LastOrDefault

        /// <summary>
        /// Returns the last element of an observable sequence, or a default value if no such element exists.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source observable sequence.</param>
        /// <returns>The last element in the observable sequence, or a default value if no such element exists.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <seealso cref="Observable.LastOrDefaultAsync{TSource}(IObservable{TSource})"/>
#if PREFER_ASYNC
        [Obsolete(Constants_Linq.USE_ASYNC)]
#endif
        public static TSource LastOrDefault<TSource>(this IObservable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return s_impl.LastOrDefault<TSource>(source);
        }

        /// <summary>
        /// Returns the last element of an observable sequence that satisfies the condition in the predicate, or a default value if no such element exists.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source observable sequence.</param>
        /// <param name="predicate">A predicate function to evaluate for elements in the source sequence.</param>
        /// <returns>The last element in the observable sequence that satisfies the condition in the predicate, or a default value if no such element exists.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="predicate"/> is null.</exception>
        /// <seealso cref="Observable.LastOrDefaultAsync{TSource}(IObservable{TSource}, Func{TSource, bool})"/>
#if PREFER_ASYNC
        [Obsolete(Constants_Linq.USE_ASYNC)]
#endif
        public static TSource LastOrDefault<TSource>(this IObservable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            return s_impl.LastOrDefault<TSource>(source, predicate);
        }

        #endregion

        #region + Latest +

        /// <summary>
        /// Returns an enumerable sequence whose enumeration returns the latest observed element in the source observable sequence.
        /// Enumerators on the resulting sequence will never produce the same element repeatedly, and will block until the next element becomes available.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source observable sequence.</param>
        /// <returns>The enumerable sequence that returns the last sampled element upon each iteration and subsequently blocks until the next element in the observable source sequence becomes available.</returns>
        public static IEnumerable<TSource> Latest<TSource>(this IObservable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return s_impl.Latest<TSource>(source);
        }

        #endregion

        #region + MostRecent +

        /// <summary>
        /// Returns an enumerable sequence whose enumeration returns the most recently observed element in the source observable sequence, using the specified initial value in case no element has been sampled yet.
        /// Enumerators on the resulting sequence never block and can produce the same element repeatedly.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source observable sequence.</param>
        /// <param name="initialValue">Initial value that will be yielded by the enumerable sequence if no element has been sampled yet.</param>
        /// <returns>The enumerable sequence that returns the last sampled element upon each iteration.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static IEnumerable<TSource> MostRecent<TSource>(this IObservable<TSource> source, TSource initialValue)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return s_impl.MostRecent<TSource>(source, initialValue);
        }

        #endregion

        #region + Next +

        /// <summary>
        /// Returns an enumerable sequence whose enumeration blocks until the next element in the source observable sequence becomes available.
        /// Enumerators on the resulting sequence will block until the next element becomes available.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source observable sequence.</param>
        /// <returns>The enumerable sequence that blocks upon each iteration until the next element in the observable source sequence becomes available.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static IEnumerable<TSource> Next<TSource>(this IObservable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return s_impl.Next<TSource>(source);
        }

        #endregion

        #region Single

        /// <summary>
        /// Returns the only element of an observable sequence, and throws an exception if there is not exactly one element in the observable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source observable sequence.</param>
        /// <returns>The single element in the observable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="InvalidOperationException">The source sequence contains more than one element. -or- The source sequence is empty.</exception>
        /// <seealso cref="Observable.SingleAsync{TSource}(IObservable{TSource})"/>
#if PREFER_ASYNC
        [Obsolete(Constants_Linq.USE_ASYNC)]
#endif
        public static TSource Single<TSource>(this IObservable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return s_impl.Single<TSource>(source);
        }

        /// <summary>
        /// Returns the only element of an observable sequence that satisfies the condition in the predicate, and throws an exception if there is not exactly one element matching the predicate in the observable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source observable sequence.</param>
        /// <param name="predicate">A predicate function to evaluate for elements in the source sequence.</param>
        /// <returns>The single element in the observable sequence that satisfies the condition in the predicate.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="predicate"/> is null.</exception>
        /// <exception cref="InvalidOperationException">No element satisfies the condition in the predicate. -or- More than one element satisfies the condition in the predicate. -or- The source sequence is empty.</exception>
        /// <seealso cref="Observable.SingleAsync{TSource}(IObservable{TSource}, Func{TSource, bool})"/>
#if PREFER_ASYNC
        [Obsolete(Constants_Linq.USE_ASYNC)]
#endif
        public static TSource Single<TSource>(this IObservable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            return s_impl.Single<TSource>(source, predicate);
        }

        #endregion

        #region SingleOrDefault

        /// <summary>
        /// Returns the only element of an observable sequence, or a default value if the observable sequence is empty; this method throws an exception if there is more than one element in the observable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source observable sequence.</param>
        /// <returns>The single element in the observable sequence, or a default value if no such element exists.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="InvalidOperationException">The source sequence contains more than one element.</exception>
        /// <seealso cref="Observable.SingleOrDefaultAsync{TSource}(IObservable{TSource})"/>
#if PREFER_ASYNC
        [Obsolete(Constants_Linq.USE_ASYNC)]
#endif
        public static TSource SingleOrDefault<TSource>(this IObservable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return s_impl.SingleOrDefault<TSource>(source);
        }

        /// <summary>
        /// Returns the only element of an observable sequence that satisfies the condition in the predicate, or a default value if no such element exists; this method throws an exception if there is more than one element matching the predicate in the observable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source observable sequence.</param>
        /// <param name="predicate">A predicate function to evaluate for elements in the source sequence.</param>
        /// <returns>The single element in the observable sequence that satisfies the condition in the predicate, or a default value if no such element exists.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="predicate"/> is null.</exception>
        /// <exception cref="InvalidOperationException">The sequence contains more than one element that satisfies the condition in the predicate.</exception>
        /// <seealso cref="Observable.SingleOrDefaultAsync{TSource}(IObservable{TSource}, Func{TSource, bool})"/>
#if PREFER_ASYNC
        [Obsolete(Constants_Linq.USE_ASYNC)]
#endif
        public static TSource SingleOrDefault<TSource>(this IObservable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            return s_impl.SingleOrDefault<TSource>(source, predicate);
        }

        #endregion

        #region Wait

        /// <summary>
        /// Waits for the observable sequence to complete and returns the last element of the sequence.
        /// If the sequence terminates with an OnError notification, the exception is throw.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source observable sequence.</param>
        /// <returns>The last element in the observable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="InvalidOperationException">The source sequence is empty.</exception>
        public static TSource Wait<TSource>(this IObservable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return s_impl.Wait<TSource>(source);
        }

        #endregion
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Reactive.Linq.ObservableImpl;
using System.Threading;

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
                throw new ArgumentNullException(nameof(source));

            return source.Collect<TSource, IList<TSource>>(() => new List<TSource>(), (lst, x) => { lst.Add(x); return lst; }, _ => new List<TSource>());
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
                throw new ArgumentNullException(nameof(source));
            if (newCollector == null)
                throw new ArgumentNullException(nameof(newCollector));
            if (merge == null)
                throw new ArgumentNullException(nameof(merge));

            return Collect_<TSource, TResult>(source, newCollector, merge, _ => newCollector());
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
                throw new ArgumentNullException(nameof(source));
            if (getInitialCollector == null)
                throw new ArgumentNullException(nameof(getInitialCollector));
            if (merge == null)
                throw new ArgumentNullException(nameof(merge));
            if (getNewCollector == null)
                throw new ArgumentNullException(nameof(getNewCollector));

            return Collect_<TSource, TResult>(source, getInitialCollector, merge, getNewCollector);
        }

        private static IEnumerable<TResult> Collect_<TSource, TResult>(IObservable<TSource> source, Func<TResult> getInitialCollector, Func<TResult, TSource, TResult> merge, Func<TResult, TResult> getNewCollector)
        {
            return new Collect<TSource, TResult>(source, getInitialCollector, merge, getNewCollector);
        }

        #endregion

        #region + First +

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
                throw new ArgumentNullException(nameof(source));

            return FirstOrDefaultInternal(source, true);
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
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return First(Where(source, predicate));
        }

        #endregion

        #region + FirstOrDefault +

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
                throw new ArgumentNullException(nameof(source));

            return FirstOrDefaultInternal(source, false);
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
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return FirstOrDefault(Where(source, predicate));
        }

        private static TSource FirstOrDefaultInternal<TSource>(IObservable<TSource> source, bool throwOnEmpty)
        {
            var value = default(TSource);
            var seenValue = false;
            var ex = default(Exception);

            using (var evt = new WaitAndSetOnce())
            {
                //
                // [OK] Use of unsafe Subscribe: fine to throw to our caller, behavior indistinguishable from going through the sink.
                //
                using (source.Subscribe/*Unsafe*/(new AnonymousObserver<TSource>(
                    v =>
                    {
                        if (!seenValue)
                        {
                            value = v;
                        }
                        seenValue = true;
                        evt.Set();
                    },
                    e =>
                    {
                        ex = e;
                        evt.Set();
                    },
                    () =>
                    {
                        evt.Set();
                    })))
                {
                    evt.WaitOne();
                }
            }

            ex.ThrowIfNotNull();

            if (throwOnEmpty && !seenValue)
                throw new InvalidOperationException(Strings_Linq.NO_ELEMENTS);

            return value;
        }

        class WaitAndSetOnce : IDisposable
        {
            private readonly ManualResetEvent _evt;
            private int _hasSet;

            public WaitAndSetOnce()
            {
                _evt = new ManualResetEvent(false);
            }

            public void Set()
            {
                if (Interlocked.Exchange(ref _hasSet, 1) == 0)
                {
                    _evt.Set();
                }
            }

            public void WaitOne()
            {
                _evt.WaitOne();
            }

            public void Dispose()
            {
#if HAS_MREEXPLICITDISPOSABLE
                ((IDisposable)_evt).Dispose();
#else
                _evt.Dispose();
#endif
            }
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
                throw new ArgumentNullException(nameof(source));
            if (onNext == null)
                throw new ArgumentNullException(nameof(onNext));

            using (var evt = new WaitAndSetOnce())
            {
                var sink = new ForEach<TSource>.Observer(onNext, () => evt.Set());

                using (source.SubscribeSafe(sink))
                {
                    evt.WaitOne();
                }

                sink.Error.ThrowIfNotNull();
            }
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
                throw new ArgumentNullException(nameof(source));
            if (onNext == null)
                throw new ArgumentNullException(nameof(onNext));

            using (var evt = new WaitAndSetOnce())
            {
                var sink = new ForEach<TSource>.ObserverIndexed(onNext, () => evt.Set());

                using (source.SubscribeSafe(sink))
                {
                    evt.WaitOne();
                }

                sink.Error.ThrowIfNotNull();
            }
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
                throw new ArgumentNullException(nameof(source));

            var e = new GetEnumerator<TSource>();
            return e.Run(source);
        }

        #endregion

        #region + Last +

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
                throw new ArgumentNullException(nameof(source));

            return LastOrDefaultInternal(source, true);
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
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return Last(Where(source, predicate));
        }

        #endregion

        #region + LastOrDefault +

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
                throw new ArgumentNullException(nameof(source));

            return LastOrDefaultInternal(source, false);
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
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return LastOrDefault(Where(source, predicate));
        }

        private static TSource LastOrDefaultInternal<TSource>(IObservable<TSource> source, bool throwOnEmpty)
        {
            var value = default(TSource);
            var seenValue = false;
            var ex = default(Exception);

            using (var evt = new WaitAndSetOnce())
            {
                //
                // [OK] Use of unsafe Subscribe: fine to throw to our caller, behavior indistinguishable from going through the sink.
                //
                using (source.Subscribe/*Unsafe*/(new AnonymousObserver<TSource>(
                    v =>
                    {
                        seenValue = true;
                        value = v;
                    },
                    e =>
                    {
                        ex = e;
                        evt.Set();
                    },
                    () =>
                    {
                        evt.Set();
                    })))
                {
                    evt.WaitOne();
                }
            }

            ex.ThrowIfNotNull();

            if (throwOnEmpty && !seenValue)
                throw new InvalidOperationException(Strings_Linq.NO_ELEMENTS);

            return value;
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
                throw new ArgumentNullException(nameof(source));

            return new Latest<TSource>(source);
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
                throw new ArgumentNullException(nameof(source));

            return new MostRecent<TSource>(source, initialValue);
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
                throw new ArgumentNullException(nameof(source));

            return new Next<TSource>(source);
        }

        #endregion

        #region + Single +

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
                throw new ArgumentNullException(nameof(source));

            return SingleOrDefaultInternal(source, true);
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
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return Single(Where(source, predicate));
        }

        #endregion

        #region + SingleOrDefault +

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
                throw new ArgumentNullException(nameof(source));

            return SingleOrDefaultInternal(source, false);
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
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return SingleOrDefault(Where(source, predicate));
        }

        private static TSource SingleOrDefaultInternal<TSource>(IObservable<TSource> source, bool throwOnEmpty)
        {
            var value = default(TSource);
            var seenValue = false;
            var ex = default(Exception);

            using (var evt = new WaitAndSetOnce())
            {
                //
                // [OK] Use of unsafe Subscribe: fine to throw to our caller, behavior indistinguishable from going through the sink.
                //
                using (source.Subscribe/*Unsafe*/(new AnonymousObserver<TSource>(
                    v =>
                    {
                        if (seenValue)
                        {
                            ex = new InvalidOperationException(Strings_Linq.MORE_THAN_ONE_ELEMENT);
                            evt.Set();
                        }

                        value = v;
                        seenValue = true;
                    },
                    e =>
                    {
                        ex = e;
                        evt.Set();
                    },
                    () =>
                    {
                        evt.Set();
                    })))
                {
                    evt.WaitOne();
                }
            }

            ex.ThrowIfNotNull();

            if (throwOnEmpty && !seenValue)
                throw new InvalidOperationException(Strings_Linq.NO_ELEMENTS);

            return value;
        }

        #endregion

        #region + Wait +

        /// <summary>
        /// Waits for the observable sequence to complete and returns the last element of the sequence.
        /// If the sequence terminates with an OnError notification, the exception is thrown.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source observable sequence.</param>
        /// <returns>The last element in the observable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="InvalidOperationException">The source sequence is empty.</exception>
        public static TSource Wait<TSource>(this IObservable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return LastOrDefaultInternal(source, true);
        }

        #endregion
    }
}

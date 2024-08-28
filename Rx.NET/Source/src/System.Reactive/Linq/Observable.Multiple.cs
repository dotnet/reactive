// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Configuration;
using System.Reactive.Concurrency;
using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    public static partial class Observable
    {
        #region + Amb +

        /// <summary>
        /// Propagates the observable sequence that reacts first.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequences.</typeparam>
        /// <param name="first">First observable sequence.</param>
        /// <param name="second">Second observable sequence.</param>
        /// <returns>An observable sequence that surfaces either of the given sequences, whichever reacted first.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="first"/> or <paramref name="second"/> is null.</exception>
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Amb", Justification = "In honor of McCarthy.")]
        public static IObservable<TSource> Amb<TSource>(this IObservable<TSource> first, IObservable<TSource> second)
        {
            if (first == null)
            {
                throw new ArgumentNullException(nameof(first));
            }

            if (second == null)
            {
                throw new ArgumentNullException(nameof(second));
            }

            return s_impl.Amb(first, second);
        }

        /// <summary>
        /// Propagates the observable sequence that reacts first.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequences.</typeparam>
        /// <param name="sources">Observable sources competing to react first.</param>
        /// <returns>An observable sequence that surfaces any of the given sequences, whichever reacted first.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="sources"/> is null.</exception>
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Amb", Justification = "In honor of McCarthy.")]
        public static IObservable<TSource> Amb<TSource>(params IObservable<TSource>[] sources)
        {
            if (sources == null)
            {
                throw new ArgumentNullException(nameof(sources));
            }

            return s_impl.Amb(sources);
        }

        /// <summary>
        /// Propagates the observable sequence that reacts first.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequences.</typeparam>
        /// <param name="sources">Observable sources competing to react first.</param>
        /// <returns>An observable sequence that surfaces any of the given sequences, whichever reacted first.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="sources"/> is null.</exception>
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Amb", Justification = "In honor of McCarthy.")]
        public static IObservable<TSource> Amb<TSource>(this IEnumerable<IObservable<TSource>> sources)
        {
            if (sources == null)
            {
                throw new ArgumentNullException(nameof(sources));
            }

            return s_impl.Amb(sources);
        }

        #endregion

        #region + Buffer +

        /// <summary>
        /// Projects each element of an observable sequence into consecutive non-overlapping buffers.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence, and in the lists in the result sequence.</typeparam>
        /// <typeparam name="TBufferClosing">The type of the elements in the sequences indicating buffer closing events.</typeparam>
        /// <param name="source">Source sequence to produce buffers over.</param>
        /// <param name="bufferClosingSelector">A function invoked to define the boundaries of the produced buffers. A new buffer is started when the previous one is closed.</param>
        /// <returns>An observable sequence of buffers.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="bufferClosingSelector"/> is null.</exception>
        public static IObservable<IList<TSource>> Buffer<TSource, TBufferClosing>(this IObservable<TSource> source, Func<IObservable<TBufferClosing>> bufferClosingSelector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (bufferClosingSelector == null)
            {
                throw new ArgumentNullException(nameof(bufferClosingSelector));
            }

            return s_impl.Buffer(source, bufferClosingSelector);
        }

        /// <summary>
        /// Projects each element of an observable sequence into zero or more buffers.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence, and in the lists in the result sequence.</typeparam>
        /// <typeparam name="TBufferOpening">The type of the elements in the sequence indicating buffer opening events, also passed to the closing selector to obtain a sequence of buffer closing events.</typeparam>
        /// <typeparam name="TBufferClosing">The type of the elements in the sequences indicating buffer closing events.</typeparam>
        /// <param name="source">Source sequence to produce buffers over.</param>
        /// <param name="bufferOpenings">Observable sequence whose elements denote the creation of new buffers.</param>
        /// <param name="bufferClosingSelector">A function invoked to define the closing of each produced buffer.</param>
        /// <returns>An observable sequence of buffers.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="bufferOpenings"/> or <paramref name="bufferClosingSelector"/> is null.</exception>
        public static IObservable<IList<TSource>> Buffer<TSource, TBufferOpening, TBufferClosing>(this IObservable<TSource> source, IObservable<TBufferOpening> bufferOpenings, Func<TBufferOpening, IObservable<TBufferClosing>> bufferClosingSelector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (bufferOpenings == null)
            {
                throw new ArgumentNullException(nameof(bufferOpenings));
            }

            if (bufferClosingSelector == null)
            {
                throw new ArgumentNullException(nameof(bufferClosingSelector));
            }

            return s_impl.Buffer(source, bufferOpenings, bufferClosingSelector);
        }

        /// <summary>
        /// Projects each element of an observable sequence into consecutive non-overlapping buffers.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence, and in the lists in the result sequence.</typeparam>
        /// <typeparam name="TBufferBoundary">The type of the elements in the sequences indicating buffer boundary events.</typeparam>
        /// <param name="source">Source sequence to produce buffers over.</param>
        /// <param name="bufferBoundaries">Sequence of buffer boundary markers. The current buffer is closed and a new buffer is opened upon receiving a boundary marker.</param>
        /// <returns>An observable sequence of buffers.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="bufferBoundaries"/> is null.</exception>
        public static IObservable<IList<TSource>> Buffer<TSource, TBufferBoundary>(this IObservable<TSource> source, IObservable<TBufferBoundary> bufferBoundaries)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (bufferBoundaries == null)
            {
                throw new ArgumentNullException(nameof(bufferBoundaries));
            }

            return s_impl.Buffer(source, bufferBoundaries);
        }

        #endregion

        #region + Catch +

        /// <summary>
        /// Continues an observable sequence that is terminated by an exception of the specified type with the observable sequence produced by the handler.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence and sequences returned by the exception handler function.</typeparam>
        /// <typeparam name="TException">The type of the exception to catch and handle. Needs to derive from <see cref="Exception"/>.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="handler">Exception handler function, producing another observable sequence.</param>
        /// <returns>An observable sequence containing the source sequence's elements, followed by the elements produced by the handler's resulting observable sequence in case an exception occurred.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="handler"/> is null.</exception>
        public static IObservable<TSource> Catch<TSource, TException>(this IObservable<TSource> source, Func<TException, IObservable<TSource>> handler) where TException : Exception
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            return s_impl.Catch(source, handler);
        }

        /// <summary>
        /// Continues an observable sequence that is terminated by an exception with the next observable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence and handler sequence.</typeparam>
        /// <param name="first">First observable sequence whose exception (if any) is caught.</param>
        /// <param name="second">Second observable sequence used to produce results when an error occurred in the first sequence.</param>
        /// <returns>An observable sequence containing the first sequence's elements, followed by the elements of the second sequence in case an exception occurred.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="first"/> or <paramref name="second"/> is null.</exception>
        public static IObservable<TSource> Catch<TSource>(this IObservable<TSource> first, IObservable<TSource> second)
        {
            if (first == null)
            {
                throw new ArgumentNullException(nameof(first));
            }

            if (second == null)
            {
                throw new ArgumentNullException(nameof(second));
            }

            return s_impl.Catch(first, second);
        }

        /// <summary>
        /// Continues an observable sequence that is terminated by an exception with the next observable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source and handler sequences.</typeparam>
        /// <param name="sources">Observable sequences to catch exceptions for.</param>
        /// <returns>An observable sequence containing elements from consecutive source sequences until a source sequence terminates successfully.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="sources"/> is null.</exception>
        public static IObservable<TSource> Catch<TSource>(params IObservable<TSource>[] sources)
        {
            if (sources == null)
            {
                throw new ArgumentNullException(nameof(sources));
            }

            return s_impl.Catch(sources);
        }

        /// <summary>
        /// Continues an observable sequence that is terminated by an exception with the next observable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source and handler sequences.</typeparam>
        /// <param name="sources">Observable sequences to catch exceptions for.</param>
        /// <returns>An observable sequence containing elements from consecutive source sequences until a source sequence terminates successfully.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="sources"/> is null.</exception>
        public static IObservable<TSource> Catch<TSource>(this IEnumerable<IObservable<TSource>> sources)
        {
            if (sources == null)
            {
                throw new ArgumentNullException(nameof(sources));
            }

            return s_impl.Catch(sources);
        }

        #endregion

        #region + CombineLatest +

        /// <summary>
        /// Merges two observable sequences into one observable sequence by using the selector function whenever one of the observable sequences produces an element.
        /// </summary>
        /// <typeparam name="TSource1">The type of the elements in the first source sequence.</typeparam>
        /// <typeparam name="TSource2">The type of the elements in the second source sequence.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the result sequence, returned by the selector function.</typeparam>
        /// <param name="first">First observable source.</param>
        /// <param name="second">Second observable source.</param>
        /// <param name="resultSelector">Function to invoke whenever either of the sources produces an element.</param>
        /// <returns>An observable sequence containing the result of combining elements of both sources using the specified result selector function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="first"/> or <paramref name="second"/> or <paramref name="resultSelector"/> is null.</exception>
        /// <remarks>If a non-empty source completes, its very last value will be used for creating subsequent combinations until all sources terminate.</remarks>
        public static IObservable<TResult> CombineLatest<TSource1, TSource2, TResult>(this IObservable<TSource1> first, IObservable<TSource2> second, Func<TSource1, TSource2, TResult> resultSelector)
        {
            if (first == null)
            {
                throw new ArgumentNullException(nameof(first));
            }

            if (second == null)
            {
                throw new ArgumentNullException(nameof(second));
            }

            if (resultSelector == null)
            {
                throw new ArgumentNullException(nameof(resultSelector));
            }

            return s_impl.CombineLatest(first, second, resultSelector);
        }

        /// <summary>
        /// Merges the specified observable sequences into one observable sequence by using the selector function whenever any of the observable sequences produces an element.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequences.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the result sequence, returned by the selector function.</typeparam>
        /// <param name="sources">Observable sources.</param>
        /// <param name="resultSelector">Function to invoke whenever any of the sources produces an element. For efficiency, the input list is reused after the selector returns. Either aggregate or copy the values during the function call.</param>
        /// <returns>An observable sequence containing the result of combining elements of the sources using the specified result selector function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="sources"/> or <paramref name="resultSelector"/> is null.</exception>
        /// <remarks>If a non-empty source completes, its very last value will be used for creating subsequent combinations until all sources terminate.</remarks>
        public static IObservable<TResult> CombineLatest<TSource, TResult>(this IEnumerable<IObservable<TSource>> sources, Func<IList<TSource>, TResult> resultSelector)
        {
            if (sources == null)
            {
                throw new ArgumentNullException(nameof(sources));
            }

            if (resultSelector == null)
            {
                throw new ArgumentNullException(nameof(resultSelector));
            }

            return s_impl.CombineLatest(sources, resultSelector);
        }

        /// <summary>
        /// Merges the specified observable sequences into one observable sequence by emitting a list with the latest source elements whenever any of the observable sequences produces an element.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequences, and in the lists in the result sequence.</typeparam>
        /// <param name="sources">Observable sources.</param>
        /// <returns>An observable sequence containing lists of the latest elements of the sources.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="sources"/> is null.</exception>
        /// <remarks>If a non-empty source completes, its very last value will be used for creating subsequent combinations until all sources terminate.</remarks>
        public static IObservable<IList<TSource>> CombineLatest<TSource>(this IEnumerable<IObservable<TSource>> sources)
        {
            if (sources == null)
            {
                throw new ArgumentNullException(nameof(sources));
            }

            return s_impl.CombineLatest(sources);
        }

        /// <summary>
        /// Merges the specified observable sequences into one observable sequence by emitting a list with the latest source elements whenever any of the observable sequences produces an element.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequences, and in the lists in the result sequence.</typeparam>
        /// <param name="sources">Observable sources.</param>
        /// <returns>An observable sequence containing lists of the latest elements of the sources.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="sources"/> is null.</exception>
        /// <remarks>If a non-empty source completes, its very last value will be used for creating subsequent combinations until all sources terminate.</remarks>
        public static IObservable<IList<TSource>> CombineLatest<TSource>(params IObservable<TSource>[] sources)
        {
            if (sources == null)
            {
                throw new ArgumentNullException(nameof(sources));
            }

            return s_impl.CombineLatest(sources);
        }

        #endregion

        #region + Concat +

        /// <summary>
        /// Concatenates the second observable sequence to the first observable sequence upon successful termination of the first.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequences.</typeparam>
        /// <param name="first">First observable sequence.</param>
        /// <param name="second">Second observable sequence.</param>
        /// <returns>An observable sequence that contains the elements of the first sequence, followed by those of the second the sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="first"/> or <paramref name="second"/> is null.</exception>
        public static IObservable<TSource> Concat<TSource>(this IObservable<TSource> first, IObservable<TSource> second)
        {
            if (first == null)
            {
                throw new ArgumentNullException(nameof(first));
            }

            if (second == null)
            {
                throw new ArgumentNullException(nameof(second));
            }

            return s_impl.Concat(first, second);
        }

        /// <summary>
        /// Concatenates all of the specified observable sequences, as long as the previous observable sequence terminated successfully.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequences.</typeparam>
        /// <param name="sources">Observable sequences to concatenate.</param>
        /// <returns>An observable sequence that contains the elements of each given sequence, in sequential order.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="sources"/> is null.</exception>
        public static IObservable<TSource> Concat<TSource>(params IObservable<TSource>[] sources)
        {
            if (sources == null)
            {
                throw new ArgumentNullException(nameof(sources));
            }

            return s_impl.Concat(sources);
        }

        /// <summary>
        /// Concatenates all observable sequences in the given enumerable sequence, as long as the previous observable sequence terminated successfully.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequences.</typeparam>
        /// <param name="sources">Observable sequences to concatenate.</param>
        /// <returns>An observable sequence that contains the elements of each given sequence, in sequential order.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="sources"/> is null.</exception>
        public static IObservable<TSource> Concat<TSource>(this IEnumerable<IObservable<TSource>> sources)
        {
            if (sources == null)
            {
                throw new ArgumentNullException(nameof(sources));
            }

            return s_impl.Concat(sources);
        }

        /// <summary>
        /// Concatenates all inner observable sequences, as long as the previous observable sequence terminated successfully.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequences.</typeparam>
        /// <param name="sources">Observable sequence of inner observable sequences.</param>
        /// <returns>An observable sequence that contains the elements of each observed inner sequence, in sequential order.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="sources"/> is null.</exception>
        public static IObservable<TSource> Concat<TSource>(this IObservable<IObservable<TSource>> sources)
        {
            if (sources == null)
            {
                throw new ArgumentNullException(nameof(sources));
            }

            return s_impl.Concat(sources);
        }

        /// <summary>
        /// Concatenates all task results, as long as the previous task terminated successfully.
        /// </summary>
        /// <typeparam name="TSource">The type of the results produced by the tasks.</typeparam>
        /// <param name="sources">Observable sequence of tasks.</param>
        /// <returns>An observable sequence that contains the results of each task, in sequential order.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="sources"/> is null.</exception>
        /// <remarks>If the tasks support cancellation, consider manual conversion of the tasks using <see cref="Observable.FromAsync{TSource}(Func{CancellationToken, Task{TSource}})"/>, followed by a concatenation operation using <see cref="Observable.Concat{TSource}(IObservable{IObservable{TSource}})"/>.</remarks>
        public static IObservable<TSource> Concat<TSource>(this IObservable<Task<TSource>> sources)
        {
            if (sources == null)
            {
                throw new ArgumentNullException(nameof(sources));
            }

            return s_impl.Concat(sources);
        }

        #endregion

        #region + Merge +

        /// <summary>
        /// Merges elements from all inner observable sequences into a single observable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequences.</typeparam>
        /// <param name="sources">Observable sequence of inner observable sequences.</param>
        /// <returns>The observable sequence that merges the elements of the inner sequences.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="sources"/> is null.</exception>
        public static IObservable<TSource> Merge<TSource>(this IObservable<IObservable<TSource>> sources)
        {
            if (sources == null)
            {
                throw new ArgumentNullException(nameof(sources));
            }

            return s_impl.Merge(sources);
        }

        /// <summary>
        /// Merges results from all source tasks into a single observable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the results produced by the source tasks.</typeparam>
        /// <param name="sources">Observable sequence of tasks.</param>
        /// <returns>The observable sequence that merges the results of the source tasks.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="sources"/> is null.</exception>
        /// <remarks>If the tasks support cancellation, consider manual conversion of the tasks using <see cref="Observable.FromAsync{TSource}(Func{CancellationToken, Task{TSource}})"/>, followed by a merge operation using <see cref="Observable.Merge{TSource}(IObservable{IObservable{TSource}})"/>.</remarks>
        public static IObservable<TSource> Merge<TSource>(this IObservable<Task<TSource>> sources)
        {
            if (sources == null)
            {
                throw new ArgumentNullException(nameof(sources));
            }

            return s_impl.Merge(sources);
        }

        /// <summary>
        /// Merges elements from all inner observable sequences into a single observable sequence, limiting the number of concurrent subscriptions to inner sequences.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequences.</typeparam>
        /// <param name="sources">Observable sequence of inner observable sequences.</param>
        /// <param name="maxConcurrent">Maximum number of inner observable sequences being subscribed to concurrently.</param>
        /// <returns>The observable sequence that merges the elements of the inner sequences.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="sources"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="maxConcurrent"/> is less than or equal to zero.</exception>
        public static IObservable<TSource> Merge<TSource>(this IObservable<IObservable<TSource>> sources, int maxConcurrent)
        {
            if (sources == null)
            {
                throw new ArgumentNullException(nameof(sources));
            }

            if (maxConcurrent <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxConcurrent));
            }

            return s_impl.Merge(sources, maxConcurrent);
        }

        /// <summary>
        /// Merges elements from all observable sequences in the given enumerable sequence into a single observable sequence, limiting the number of concurrent subscriptions to inner sequences.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequences.</typeparam>
        /// <param name="sources">Enumerable sequence of observable sequences.</param>
        /// <param name="maxConcurrent">Maximum number of observable sequences being subscribed to concurrently.</param>
        /// <returns>The observable sequence that merges the elements of the observable sequences.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="sources"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="maxConcurrent"/> is less than or equal to zero.</exception>
        public static IObservable<TSource> Merge<TSource>(this IEnumerable<IObservable<TSource>> sources, int maxConcurrent)
        {
            if (sources == null)
            {
                throw new ArgumentNullException(nameof(sources));
            }

            if (maxConcurrent <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxConcurrent));
            }

            return s_impl.Merge(sources, maxConcurrent);
        }

        /// <summary>
        /// Merges elements from all observable sequences in the given enumerable sequence into a single observable sequence, limiting the number of concurrent subscriptions to inner sequences, and using the specified scheduler for enumeration of and subscription to the sources.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequences.</typeparam>
        /// <param name="sources">Enumerable sequence of observable sequences.</param>
        /// <param name="maxConcurrent">Maximum number of observable sequences being subscribed to concurrently.</param>
        /// <param name="scheduler">Scheduler to run the enumeration of the sequence of sources on.</param>
        /// <returns>The observable sequence that merges the elements of the observable sequences.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="sources"/> or <paramref name="scheduler"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="maxConcurrent"/> is less than or equal to zero.</exception>
        public static IObservable<TSource> Merge<TSource>(this IEnumerable<IObservable<TSource>> sources, int maxConcurrent, IScheduler scheduler)
        {
            if (sources == null)
            {
                throw new ArgumentNullException(nameof(sources));
            }

            if (maxConcurrent <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxConcurrent));
            }

            if (scheduler == null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            return s_impl.Merge(sources, maxConcurrent, scheduler);
        }

        /// <summary>
        /// Merges elements from two observable sequences into a single observable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequences.</typeparam>
        /// <param name="first">First observable sequence.</param>
        /// <param name="second">Second observable sequence.</param>
        /// <returns>The observable sequence that merges the elements of the given sequences.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="first"/> or <paramref name="second"/> is null.</exception>
        public static IObservable<TSource> Merge<TSource>(this IObservable<TSource> first, IObservable<TSource> second)
        {
            if (first == null)
            {
                throw new ArgumentNullException(nameof(first));
            }

            if (second == null)
            {
                throw new ArgumentNullException(nameof(second));
            }

            return s_impl.Merge(first, second);
        }

        /// <summary>
        /// Merges elements from two observable sequences into a single observable sequence, using the specified scheduler for enumeration of and subscription to the sources.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequences.</typeparam>
        /// <param name="first">First observable sequence.</param>
        /// <param name="second">Second observable sequence.</param>
        /// <param name="scheduler">Scheduler used to introduce concurrency for making subscriptions to the given sequences.</param>
        /// <returns>The observable sequence that merges the elements of the given sequences.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="first"/> or <paramref name="second"/> or <paramref name="scheduler"/> is null.</exception>
        public static IObservable<TSource> Merge<TSource>(this IObservable<TSource> first, IObservable<TSource> second, IScheduler scheduler)
        {
            if (first == null)
            {
                throw new ArgumentNullException(nameof(first));
            }

            if (second == null)
            {
                throw new ArgumentNullException(nameof(second));
            }

            if (scheduler == null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            return s_impl.Merge(first, second, scheduler);
        }

        /// <summary>
        /// Merges elements from all of the specified observable sequences into a single observable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequences.</typeparam>
        /// <param name="sources">Observable sequences.</param>
        /// <returns>The observable sequence that merges the elements of the observable sequences.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="sources"/> is null.</exception>
        public static IObservable<TSource> Merge<TSource>(params IObservable<TSource>[] sources)
        {
            if (sources == null)
            {
                throw new ArgumentNullException(nameof(sources));
            }

            return s_impl.Merge(sources);
        }

        /// <summary>
        /// Merges elements from all of the specified observable sequences into a single observable sequence, using the specified scheduler for enumeration of and subscription to the sources.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequences.</typeparam>
        /// <param name="sources">Observable sequences.</param>
        /// <param name="scheduler">Scheduler to run the enumeration of the sequence of sources on.</param>
        /// <returns>The observable sequence that merges the elements of the observable sequences.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="scheduler"/> or <paramref name="sources"/> is null.</exception>
        public static IObservable<TSource> Merge<TSource>(IScheduler scheduler, params IObservable<TSource>[] sources)
        {
            if (scheduler == null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            if (sources == null)
            {
                throw new ArgumentNullException(nameof(sources));
            }

            return s_impl.Merge(scheduler, sources);
        }

        /// <summary>
        /// Merges elements from all observable sequences in the given enumerable sequence into a single observable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequences.</typeparam>
        /// <param name="sources">Enumerable sequence of observable sequences.</param>
        /// <returns>The observable sequence that merges the elements of the observable sequences.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="sources"/> is null.</exception>
        public static IObservable<TSource> Merge<TSource>(this IEnumerable<IObservable<TSource>> sources)
        {
            if (sources == null)
            {
                throw new ArgumentNullException(nameof(sources));
            }

            return s_impl.Merge(sources);
        }

        /// <summary>
        /// Merges elements from all observable sequences in the given enumerable sequence into a single observable sequence, using the specified scheduler for enumeration of and subscription to the sources.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequences.</typeparam>
        /// <param name="sources">Enumerable sequence of observable sequences.</param>
        /// <param name="scheduler">Scheduler to run the enumeration of the sequence of sources on.</param>
        /// <returns>The observable sequence that merges the elements of the observable sequences.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="sources"/> or <paramref name="scheduler"/> is null.</exception>
        public static IObservable<TSource> Merge<TSource>(this IEnumerable<IObservable<TSource>> sources, IScheduler scheduler)
        {
            if (sources == null)
            {
                throw new ArgumentNullException(nameof(sources));
            }

            if (scheduler == null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            return s_impl.Merge(sources, scheduler);
        }

        #endregion

        #region + OnErrorResumeNext +

        /// <summary>
        /// Concatenates the second observable sequence to the first observable sequence upon successful or exceptional termination of the first.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequences.</typeparam>
        /// <param name="first">First observable sequence whose exception (if any) is caught.</param>
        /// <param name="second">Second observable sequence used to produce results after the first sequence terminates.</param>
        /// <returns>An observable sequence that concatenates the first and second sequence, even if the first sequence terminates exceptionally.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="first"/> or <paramref name="second"/> is null.</exception>
        public static IObservable<TSource> OnErrorResumeNext<TSource>(this IObservable<TSource> first, IObservable<TSource> second)
        {
            if (first == null)
            {
                throw new ArgumentNullException(nameof(first));
            }

            if (second == null)
            {
                throw new ArgumentNullException(nameof(second));
            }

            return s_impl.OnErrorResumeNext(first, second);
        }

        /// <summary>
        /// Concatenates all of the specified observable sequences, even if the previous observable sequence terminated exceptionally.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequences.</typeparam>
        /// <param name="sources">Observable sequences to concatenate.</param>
        /// <returns>An observable sequence that concatenates the source sequences, even if a sequence terminates exceptionally.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="sources"/> is null.</exception>
        public static IObservable<TSource> OnErrorResumeNext<TSource>(params IObservable<TSource>[] sources)
        {
            if (sources == null)
            {
                throw new ArgumentNullException(nameof(sources));
            }

            return s_impl.OnErrorResumeNext(sources);
        }

        /// <summary>
        /// Concatenates all observable sequences in the given enumerable sequence, even if the previous observable sequence terminated exceptionally.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequences.</typeparam>
        /// <param name="sources">Observable sequences to concatenate.</param>
        /// <returns>An observable sequence that concatenates the source sequences, even if a sequence terminates exceptionally.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="sources"/> is null.</exception>
        public static IObservable<TSource> OnErrorResumeNext<TSource>(this IEnumerable<IObservable<TSource>> sources)
        {
            if (sources == null)
            {
                throw new ArgumentNullException(nameof(sources));
            }

            return s_impl.OnErrorResumeNext(sources);
        }

        #endregion

        #region + SkipUntil +

        /// <summary>
        /// Returns the elements from the source observable sequence only after the other observable sequence produces an element.
        /// Starting from Rx.NET 4.0, this will subscribe to <paramref name="other"/> before subscribing to <paramref name="source" />
        /// so in case <paramref name="other" /> emits an element right away, elements from <paramref name="source" /> are not missed.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TOther">The type of the elements in the other sequence that indicates the end of skip behavior.</typeparam>
        /// <param name="source">Source sequence to propagate elements for.</param>
        /// <param name="other">Observable sequence that triggers propagation of elements of the source sequence.</param>
        /// <returns>An observable sequence containing the elements of the source sequence starting from the point the other sequence triggered propagation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="other"/> is null.</exception>
        public static IObservable<TSource> SkipUntil<TSource, TOther>(this IObservable<TSource> source, IObservable<TOther> other)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            return s_impl.SkipUntil(source, other);
        }

        #endregion

        #region + Switch +

        /// <summary>
        /// Transforms an observable sequence of observable sequences into an observable sequence 
        /// producing values only from the most recent observable sequence.
        /// Each time a new inner observable sequence is received, unsubscribe from the 
        /// previous inner observable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequences.</typeparam>
        /// <param name="sources">Observable sequence of inner observable sequences.</param>
        /// <returns>The observable sequence that at any point in time produces the elements of the most recent inner observable sequence that has been received.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="sources"/> is null.</exception>
        public static IObservable<TSource> Switch<TSource>(this IObservable<IObservable<TSource>> sources)
        {
            if (sources == null)
            {
                throw new ArgumentNullException(nameof(sources));
            }

            return s_impl.Switch(sources);
        }

        /// <summary>
        /// Transforms an observable sequence of tasks into an observable sequence 
        /// producing values only from the most recent observable sequence.
        /// Each time a new task is received, the previous task's result is ignored.
        /// </summary>
        /// <typeparam name="TSource">The type of the results produced by the source tasks.</typeparam>
        /// <param name="sources">Observable sequence of tasks.</param>
        /// <returns>The observable sequence that at any point in time produces the result of the most recent task that has been received.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="sources"/> is null.</exception>
        /// <remarks>If the tasks support cancellation, consider manual conversion of the tasks using <see cref="Observable.FromAsync{TSource}(Func{CancellationToken, Task{TSource}})"/>, followed by a switch operation using <see cref="Observable.Switch{TSource}(IObservable{IObservable{TSource}})"/>.</remarks>
        public static IObservable<TSource> Switch<TSource>(this IObservable<Task<TSource>> sources)
        {
            if (sources == null)
            {
                throw new ArgumentNullException(nameof(sources));
            }

            return s_impl.Switch(sources);
        }

        #endregion

        #region + TakeUntil +

        /// <summary>
        /// Returns the elements from the source observable sequence until the other observable sequence produces an element.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TOther">The type of the elements in the other sequence that indicates the end of take behavior.</typeparam>
        /// <param name="source">Source sequence to propagate elements for.</param>
        /// <param name="other">Observable sequence that terminates propagation of elements of the source sequence.</param>
        /// <returns>An observable sequence containing the elements of the source sequence up to the point the other sequence interrupted further propagation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="other"/> is null.</exception>
        public static IObservable<TSource> TakeUntil<TSource, TOther>(this IObservable<TSource> source, IObservable<TOther> other)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            return s_impl.TakeUntil(source, other);
        }

        /// <summary>
        /// Relays elements from the source observable sequence and calls the predicate after an
        /// emission to check if the sequence should stop after that specific item.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source and result sequences.</typeparam>
        /// <param name="source">The source sequence to relay elements of.</param>
        /// <param name="stopPredicate">Called after each upstream item has been emitted with
        /// that upstream item and should return <code>true</code> to indicate the sequence should
        /// complete.</param>
        /// <returns>The observable sequence with the source elements until the stop predicate returns true.</returns>
        /// <example>
        /// The following sequence will stop after the value 5 has been encountered:
        /// <code>
        /// Observable.Range(1, 10)
        ///     .TakeUntil(item =&gt; item == 5)
        ///     .Subscribe(Console.WriteLine);
        /// </code>
        /// </example>
        /// <exception cref="ArgumentException">If <typeparamref name="TSource"/> or <paramref name="stopPredicate"/> is <code>null</code>.</exception>
        public static IObservable<TSource> TakeUntil<TSource>(this IObservable<TSource> source, Func<TSource, bool> stopPredicate)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (stopPredicate == null)
            {
                throw new ArgumentNullException(nameof(stopPredicate));
            }

            return s_impl.TakeUntil(source, stopPredicate);
        }

        #endregion

        #region + Window +

        /// <summary>
        /// Projects each element of an observable sequence into consecutive non-overlapping windows.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence, and in the windows in the result sequence.</typeparam>
        /// <typeparam name="TWindowClosing">The type of the elements in the sequences indicating window closing events.</typeparam>
        /// <param name="source">Source sequence to produce windows over.</param>
        /// <param name="windowClosingSelector">A function invoked to define the boundaries of the produced windows. A new window is started when the previous one is closed.</param>
        /// <returns>An observable sequence of windows.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="windowClosingSelector"/> is null.</exception>
        public static IObservable<IObservable<TSource>> Window<TSource, TWindowClosing>(this IObservable<TSource> source, Func<IObservable<TWindowClosing>> windowClosingSelector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (windowClosingSelector == null)
            {
                throw new ArgumentNullException(nameof(windowClosingSelector));
            }

            return s_impl.Window(source, windowClosingSelector);
        }

        /// <summary>
        /// Projects each element of an observable sequence into zero or more windows.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence, and in the windows in the result sequence.</typeparam>
        /// <typeparam name="TWindowOpening">The type of the elements in the sequence indicating window opening events, also passed to the closing selector to obtain a sequence of window closing events.</typeparam>
        /// <typeparam name="TWindowClosing">The type of the elements in the sequences indicating window closing events.</typeparam>
        /// <param name="source">Source sequence to produce windows over.</param>
        /// <param name="windowOpenings">Observable sequence whose elements denote the creation of new windows.</param>
        /// <param name="windowClosingSelector">A function invoked to define the closing of each produced window.</param>
        /// <returns>An observable sequence of windows.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="windowOpenings"/> or <paramref name="windowClosingSelector"/> is null.</exception>
        public static IObservable<IObservable<TSource>> Window<TSource, TWindowOpening, TWindowClosing>(this IObservable<TSource> source, IObservable<TWindowOpening> windowOpenings, Func<TWindowOpening, IObservable<TWindowClosing>> windowClosingSelector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (windowOpenings == null)
            {
                throw new ArgumentNullException(nameof(windowOpenings));
            }

            if (windowClosingSelector == null)
            {
                throw new ArgumentNullException(nameof(windowClosingSelector));
            }

            return s_impl.Window(source, windowOpenings, windowClosingSelector);
        }

        /// <summary>
        /// Projects each element of an observable sequence into consecutive non-overlapping windows.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence, and in the windows in the result sequence.</typeparam>
        /// <typeparam name="TWindowBoundary">The type of the elements in the sequences indicating window boundary events.</typeparam>
        /// <param name="source">Source sequence to produce windows over.</param>
        /// <param name="windowBoundaries">Sequence of window boundary markers. The current window is closed and a new window is opened upon receiving a boundary marker.</param>
        /// <returns>An observable sequence of windows.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="windowBoundaries"/> is null.</exception>
        public static IObservable<IObservable<TSource>> Window<TSource, TWindowBoundary>(this IObservable<TSource> source, IObservable<TWindowBoundary> windowBoundaries)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (windowBoundaries == null)
            {
                throw new ArgumentNullException(nameof(windowBoundaries));
            }

            return s_impl.Window(source, windowBoundaries);
        }

        #endregion

        #region + WithLatestFrom +

        /// <summary>
        /// Merges two observable sequences into one observable sequence by combining each element from the first source with the latest element from the second source, if any.
        /// Starting from Rx.NET 4.0, this will subscribe to <paramref name="second"/> before subscribing to <paramref name="first" /> to have a latest element readily available
        /// in case <paramref name="first" /> emits an element right away.
        /// </summary>
        /// <typeparam name="TFirst">The type of the elements in the first source sequence.</typeparam>
        /// <typeparam name="TSecond">The type of the elements in the second source sequence.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the result sequence, returned by the selector function.</typeparam>
        /// <param name="first">First observable source.</param>
        /// <param name="second">Second observable source.</param>
        /// <param name="resultSelector">Function to invoke for each element from the first source combined with the latest element from the second source, if any.</param>
        /// <returns>An observable sequence containing the result of combining each element of the first source with the latest element from the second source, if any, using the specified result selector function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="first"/> or <paramref name="second"/> or <paramref name="resultSelector"/> is null.</exception>
        public static IObservable<TResult> WithLatestFrom<TFirst, TSecond, TResult>(this IObservable<TFirst> first, IObservable<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector)
        {
            if (first == null)
            {
                throw new ArgumentNullException(nameof(first));
            }

            if (second == null)
            {
                throw new ArgumentNullException(nameof(second));
            }

            if (resultSelector == null)
            {
                throw new ArgumentNullException(nameof(resultSelector));
            }

            return s_impl.WithLatestFrom(first, second, resultSelector);
        }

        #endregion

        #region + Zip +

        /// <summary>
        /// Merges two observable sequences into one observable sequence by combining their elements in a pairwise fashion.
        /// </summary>
        /// <typeparam name="TSource1">The type of the elements in the first source sequence.</typeparam>
        /// <typeparam name="TSource2">The type of the elements in the second source sequence.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the result sequence, returned by the selector function.</typeparam>
        /// <param name="first">First observable source.</param>
        /// <param name="second">Second observable source.</param>
        /// <param name="resultSelector">Function to invoke for each consecutive pair of elements from the first and second source.</param>
        /// <returns>An observable sequence containing the result of pairwise combining the elements of the first and second source using the specified result selector function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="first"/> or <paramref name="second"/> or <paramref name="resultSelector"/> is null.</exception>
        public static IObservable<TResult> Zip<TSource1, TSource2, TResult>(this IObservable<TSource1> first, IObservable<TSource2> second, Func<TSource1, TSource2, TResult> resultSelector)
        {
            if (first == null)
            {
                throw new ArgumentNullException(nameof(first));
            }

            if (second == null)
            {
                throw new ArgumentNullException(nameof(second));
            }

            if (resultSelector == null)
            {
                throw new ArgumentNullException(nameof(resultSelector));
            }

            return s_impl.Zip(first, second, resultSelector);
        }

        /// <summary>
        /// Merges the specified observable sequences into one observable sequence by using the selector function whenever all of the observable sequences have produced an element at a corresponding index.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequences.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the result sequence, returned by the selector function.</typeparam>
        /// <param name="sources">Observable sources.</param>
        /// <param name="resultSelector">Function to invoke for each series of elements at corresponding indexes in the sources.</param>
        /// <returns>An observable sequence containing the result of combining elements of the sources using the specified result selector function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="sources"/> or <paramref name="resultSelector"/> is null.</exception>
        public static IObservable<TResult> Zip<TSource, TResult>(this IEnumerable<IObservable<TSource>> sources, Func<IList<TSource>, TResult> resultSelector)
        {
            if (sources == null)
            {
                throw new ArgumentNullException(nameof(sources));
            }

            if (resultSelector == null)
            {
                throw new ArgumentNullException(nameof(resultSelector));
            }

            return s_impl.Zip(sources, resultSelector);
        }

        /// <summary>
        /// Merges the specified observable sequences into one observable sequence by emitting a list with the elements of the observable sequences at corresponding indexes.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequences, and in the lists in the result sequence.</typeparam>
        /// <param name="sources">Observable sources.</param>
        /// <returns>An observable sequence containing lists of elements at corresponding indexes.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="sources"/> is null.</exception>
        public static IObservable<IList<TSource>> Zip<TSource>(this IEnumerable<IObservable<TSource>> sources)
        {
            if (sources == null)
            {
                throw new ArgumentNullException(nameof(sources));
            }

            return s_impl.Zip(sources);
        }

        /// <summary>
        /// Merges the specified observable sequences into one observable sequence by emitting a list with the elements of the observable sequences at corresponding indexes.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequences, and in the lists in the result sequence.</typeparam>
        /// <param name="sources">Observable sources.</param>
        /// <returns>An observable sequence containing lists of elements at corresponding indexes.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="sources"/> is null.</exception>
        public static IObservable<IList<TSource>> Zip<TSource>(params IObservable<TSource>[] sources)
        {
            if (sources == null)
            {
                throw new ArgumentNullException(nameof(sources));
            }

            return s_impl.Zip(sources);
        }

        /// <summary>
        /// Merges an observable sequence and an enumerable sequence into one observable sequence by using the selector function.
        /// </summary>
        /// <typeparam name="TSource1">The type of the elements in the first observable source sequence.</typeparam>
        /// <typeparam name="TSource2">The type of the elements in the second enumerable source sequence.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the result sequence, returned by the selector function.</typeparam>
        /// <param name="first">First observable source.</param>
        /// <param name="second">Second enumerable source.</param>
        /// <param name="resultSelector">Function to invoke for each consecutive pair of elements from the first and second source.</param>
        /// <returns>An observable sequence containing the result of pairwise combining the elements of the first and second source using the specified result selector function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="first"/> or <paramref name="second"/> or <paramref name="resultSelector"/> is null.</exception>
        public static IObservable<TResult> Zip<TSource1, TSource2, TResult>(this IObservable<TSource1> first, IEnumerable<TSource2> second, Func<TSource1, TSource2, TResult> resultSelector)
        {
            if (first == null)
            {
                throw new ArgumentNullException(nameof(first));
            }

            if (second == null)
            {
                throw new ArgumentNullException(nameof(second));
            }

            if (resultSelector == null)
            {
                throw new ArgumentNullException(nameof(resultSelector));
            }

            return s_impl.Zip(first, second, resultSelector);
        }

        #endregion
    }

#pragma warning disable CA1711 // (Don't use Ex suffix.) This has been a public type for many years, so we can't rename it now.
    public static partial class ObservableEx
#pragma warning restore CA1711
    {
        /// <summary>
        /// Merges two observable sequences into one observable sequence by combining each element from the first source with the latest element from the second source, if any.
        /// </summary>
        /// <typeparam name="TFirst">The type of the elements in the first source sequence.</typeparam>
        /// <typeparam name="TSecond">The type of the elements in the second source sequence.</typeparam>
        /// <param name="first">First observable source.</param>
        /// <param name="second">Second observable source.</param>
        /// <returns>An observable sequence containing the result of combining each element of the first source with the latest element from the second source, if any, as a tuple value.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="first"/> or <paramref name="second"/> is null.</exception>
        public static IObservable<(TFirst First, TSecond Second)> WithLatestFrom<TFirst, TSecond>(this IObservable<TFirst> first, IObservable<TSecond> second)
        {
            if (first == null)
            {
                throw new ArgumentNullException(nameof(first));
            }

            if (second == null)
            {
                throw new ArgumentNullException(nameof(second));
            }

            return s_impl.WithLatestFrom(first, second);
        }

        /// <summary>
        /// Merges an observable sequence and an enumerable sequence into one observable sequence of tuple values.
        /// </summary>
        /// <typeparam name="TFirst">The type of the elements in the first observable source sequence.</typeparam>
        /// <typeparam name="TSecond">The type of the elements in the second enumerable source sequence.</typeparam>
        /// <param name="first">First observable source.</param>
        /// <param name="second">Second enumerable source.</param>
        /// <returns>An observable sequence containing the result of pairwise combining the elements of the first and second source as a tuple value.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="first"/> or <paramref name="second"/> is null.</exception>
        public static IObservable<(TFirst First, TSecond Second)> Zip<TFirst, TSecond>(this IObservable<TFirst> first, IEnumerable<TSecond> second)
        {
            if (first == null)
            {
                throw new ArgumentNullException(nameof(first));
            }

            if (second == null)
            {
                throw new ArgumentNullException(nameof(second));
            }

            return s_impl.Zip(first, second);
        }
    }
}

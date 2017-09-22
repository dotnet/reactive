// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    // REVIEW: Expose Replay using ConcurrentAsyncAsyncSubject<T> underneath.

    partial class AsyncObservable
    {
        public static IConnectableAsyncObservable<TSource> Replay<TSource>(this IAsyncObservable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Multicast(source, new SequentialReplayAsyncSubject<TSource>());
        }

        public static IConnectableAsyncObservable<TSource> Replay<TSource>(this IAsyncObservable<TSource> source, IAsyncScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return Multicast(source, new SequentialReplayAsyncSubject<TSource>(scheduler));
        }

        public static IConnectableAsyncObservable<TSource> Replay<TSource>(this IAsyncObservable<TSource> source, int bufferSize)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (bufferSize < 0)
                throw new ArgumentOutOfRangeException(nameof(bufferSize));

            return Multicast(source, new SequentialReplayAsyncSubject<TSource>(bufferSize));
        }

        public static IConnectableAsyncObservable<TSource> Replay<TSource>(this IAsyncObservable<TSource> source, int bufferSize, IAsyncScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (bufferSize < 0)
                throw new ArgumentOutOfRangeException(nameof(bufferSize));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return Multicast(source, new SequentialReplayAsyncSubject<TSource>(bufferSize, scheduler));
        }

        public static IConnectableAsyncObservable<TSource> Replay<TSource>(this IAsyncObservable<TSource> source, TimeSpan window)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (window < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(window));

            return Multicast(source, new SequentialReplayAsyncSubject<TSource>(window));
        }

        public static IConnectableAsyncObservable<TSource> Replay<TSource>(this IAsyncObservable<TSource> source, TimeSpan window, IAsyncScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (window < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(window));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return Multicast(source, new SequentialReplayAsyncSubject<TSource>(window, scheduler));
        }

        public static IConnectableAsyncObservable<TSource> Replay<TSource>(this IAsyncObservable<TSource> source, int bufferSize, TimeSpan window)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (bufferSize < 0)
                throw new ArgumentOutOfRangeException(nameof(bufferSize));
            if (window < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(window));

            return Multicast(source, new SequentialReplayAsyncSubject<TSource>(bufferSize, window));
        }

        public static IConnectableAsyncObservable<TSource> Replay<TSource>(this IAsyncObservable<TSource> source, int bufferSize, TimeSpan window, IAsyncScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (bufferSize < 0)
                throw new ArgumentOutOfRangeException(nameof(bufferSize));
            if (window < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(window));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return Multicast(source, new SequentialReplayAsyncSubject<TSource>(bufferSize, window, scheduler));
        }

        public static IAsyncObservable<TResult> Replay<TSource, TResult>(this IAsyncObservable<TSource> source, Func<IAsyncObservable<TSource>, IAsyncObservable<TResult>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Multicast(source, () => new SequentialReplayAsyncSubject<TSource>(), selector);
        }

        public static IAsyncObservable<TResult> Replay<TSource, TResult>(this IAsyncObservable<TSource> source, Func<IAsyncObservable<TSource>, IAsyncObservable<TResult>> selector, IAsyncScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return Multicast(source, () => new SequentialReplayAsyncSubject<TSource>(scheduler), selector);
        }

        public static IAsyncObservable<TResult> Replay<TSource, TResult>(this IAsyncObservable<TSource> source, Func<IAsyncObservable<TSource>, IAsyncObservable<TResult>> selector, int bufferSize)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));
            if (bufferSize < 0)
                throw new ArgumentOutOfRangeException(nameof(bufferSize));

            return Multicast(source, () => new SequentialReplayAsyncSubject<TSource>(bufferSize), selector);
        }

        public static IAsyncObservable<TResult> Replay<TSource, TResult>(this IAsyncObservable<TSource> source, Func<IAsyncObservable<TSource>, IAsyncObservable<TResult>> selector, int bufferSize, IAsyncScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));
            if (bufferSize < 0)
                throw new ArgumentOutOfRangeException(nameof(bufferSize));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return Multicast(source, () => new SequentialReplayAsyncSubject<TSource>(bufferSize, scheduler), selector);
        }

        public static IAsyncObservable<TResult> Replay<TSource, TResult>(this IAsyncObservable<TSource> source, Func<IAsyncObservable<TSource>, IAsyncObservable<TResult>> selector, TimeSpan window)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));
            if (window < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(window));

            return Multicast(source, () => new SequentialReplayAsyncSubject<TSource>(window), selector);
        }

        public static IAsyncObservable<TResult> Replay<TSource, TResult>(this IAsyncObservable<TSource> source, Func<IAsyncObservable<TSource>, IAsyncObservable<TResult>> selector, TimeSpan window, IAsyncScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));
            if (window < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(window));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return Multicast(source, () => new SequentialReplayAsyncSubject<TSource>(window, scheduler), selector);
        }

        public static IAsyncObservable<TResult> Replay<TSource, TResult>(this IAsyncObservable<TSource> source, Func<IAsyncObservable<TSource>, IAsyncObservable<TResult>> selector, int bufferSize, TimeSpan window)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));
            if (bufferSize < 0)
                throw new ArgumentOutOfRangeException(nameof(bufferSize));
            if (window < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(window));

            return Multicast(source, () => new SequentialReplayAsyncSubject<TSource>(bufferSize, window), selector);
        }

        public static IAsyncObservable<TResult> Replay<TSource, TResult>(this IAsyncObservable<TSource> source, Func<IAsyncObservable<TSource>, IAsyncObservable<TResult>> selector, int bufferSize, TimeSpan window, IAsyncScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));
            if (bufferSize < 0)
                throw new ArgumentOutOfRangeException(nameof(bufferSize));
            if (window < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(window));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return Multicast(source, () => new SequentialReplayAsyncSubject<TSource>(bufferSize, window, scheduler), selector);
        }

        public static IAsyncObservable<TResult> Replay<TSource, TResult>(this IAsyncObservable<TSource> source, Func<IAsyncObservable<TSource>, Task<IAsyncObservable<TResult>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Multicast(source, () => Task.FromResult<IAsyncSubject<TSource, TSource>>(new SequentialReplayAsyncSubject<TSource>()), selector);
        }

        public static IAsyncObservable<TResult> Replay<TSource, TResult>(this IAsyncObservable<TSource> source, Func<IAsyncObservable<TSource>, Task<IAsyncObservable<TResult>>> selector, IAsyncScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return Multicast(source, () => Task.FromResult<IAsyncSubject<TSource, TSource>>(new SequentialReplayAsyncSubject<TSource>(scheduler)), selector);
        }

        public static IAsyncObservable<TResult> Replay<TSource, TResult>(this IAsyncObservable<TSource> source, Func<IAsyncObservable<TSource>, Task<IAsyncObservable<TResult>>> selector, int bufferSize)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));
            if (bufferSize < 0)
                throw new ArgumentOutOfRangeException(nameof(bufferSize));

            return Multicast(source, () => Task.FromResult<IAsyncSubject<TSource, TSource>>(new SequentialReplayAsyncSubject<TSource>(bufferSize)), selector);
        }

        public static IAsyncObservable<TResult> Replay<TSource, TResult>(this IAsyncObservable<TSource> source, Func<IAsyncObservable<TSource>, Task<IAsyncObservable<TResult>>> selector, int bufferSize, IAsyncScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));
            if (bufferSize < 0)
                throw new ArgumentOutOfRangeException(nameof(bufferSize));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return Multicast(source, () => Task.FromResult<IAsyncSubject<TSource, TSource>>(new SequentialReplayAsyncSubject<TSource>(bufferSize, scheduler)), selector);
        }

        public static IAsyncObservable<TResult> Replay<TSource, TResult>(this IAsyncObservable<TSource> source, Func<IAsyncObservable<TSource>, Task<IAsyncObservable<TResult>>> selector, TimeSpan window)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));
            if (window < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(window));

            return Multicast(source, () => Task.FromResult<IAsyncSubject<TSource, TSource>>(new SequentialReplayAsyncSubject<TSource>(window)), selector);
        }

        public static IAsyncObservable<TResult> Replay<TSource, TResult>(this IAsyncObservable<TSource> source, Func<IAsyncObservable<TSource>, Task<IAsyncObservable<TResult>>> selector, TimeSpan window, IAsyncScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));
            if (window < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(window));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return Multicast(source, () => Task.FromResult<IAsyncSubject<TSource, TSource>>(new SequentialReplayAsyncSubject<TSource>(window, scheduler)), selector);
        }

        public static IAsyncObservable<TResult> Replay<TSource, TResult>(this IAsyncObservable<TSource> source, Func<IAsyncObservable<TSource>, Task<IAsyncObservable<TResult>>> selector, int bufferSize, TimeSpan window)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));
            if (bufferSize < 0)
                throw new ArgumentOutOfRangeException(nameof(bufferSize));
            if (window < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(window));

            return Multicast(source, () => Task.FromResult<IAsyncSubject<TSource, TSource>>(new SequentialReplayAsyncSubject<TSource>(bufferSize, window)), selector);
        }

        public static IAsyncObservable<TResult> Replay<TSource, TResult>(this IAsyncObservable<TSource> source, Func<IAsyncObservable<TSource>, Task<IAsyncObservable<TResult>>> selector, int bufferSize, TimeSpan window, IAsyncScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));
            if (bufferSize < 0)
                throw new ArgumentOutOfRangeException(nameof(bufferSize));
            if (window < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(window));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return Multicast(source, () => Task.FromResult<IAsyncSubject<TSource, TSource>>(new SequentialReplayAsyncSubject<TSource>(bufferSize, window, scheduler)), selector);
        }
    }
}

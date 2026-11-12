// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;

using BenchmarkDotNet.Engines;

namespace Benchmarks.System.Reactive.Async.Infrastructure
{
    /// <summary>
    /// Push loops for hot (S2) and lifecycle (S4) benchmarks. Each <c>OnNextAsync</c> is awaited before the next
    /// one, which is the AsyncRx backpressure contract. The loops stay synchronous while the target completes
    /// synchronously and fall back to an async path otherwise.
    /// </summary>
    public static class Pump
    {
        /// <summary>Pushes <c>0..n-1</c> into <paramref name="target"/>, without completing it.</summary>
        public static ValueTask PushAsync(this IAsyncObserver<int> target, int n) => PushAsync(target, 0, n);

        /// <summary>Pushes <c>start..start+n-1</c> into <paramref name="target"/>, without completing it.</summary>
        public static ValueTask PushAsync(this IAsyncObserver<int> target, int start, int n)
        {
            var end = start + n;
            for (var i = start; i < end; i++)
            {
                var pending = target.OnNextAsync(i);
                if (!pending.IsCompletedSuccessfully)
                {
                    return SlowAsync(pending, target, i + 1, end);
                }
            }

            return default;

            static async ValueTask SlowAsync(ValueTask pending, IAsyncObserver<int> target, int next, int end)
            {
                await pending.ConfigureAwait(false);
                for (var i = next; i < end; i++)
                {
                    await target.OnNextAsync(i).ConfigureAwait(false);
                }
            }
        }

        /// <summary>Pushes <c>0..n-1</c> into <paramref name="target"/>, then completes it.</summary>
        public static async ValueTask PushAndCompleteAsync(this IAsyncObserver<int> target, int n)
        {
            await PushAsync(target, 0, n).ConfigureAwait(false);
            await target.OnCompletedAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Subscribes the cached consuming sink to <paramref name="pipeline"/> (built over
        /// <paramref name="source"/>), pushes <c>0..n-1</c> into <paramref name="source"/>, completes it, and waits
        /// for the sink to terminate.
        /// </summary>
        public static async ValueTask RunHotAsync<TResult>(this IAsyncObservable<TResult> pipeline, IAsyncObserver<int> source, int n, Consumer consumer)
        {
            var sink = Sinks.For<TResult>(consumer);
            await pipeline.SubscribeAsync(sink).ConfigureAwait(false);
            await PushAndCompleteAsync(source, n).ConfigureAwait(false);
            await sink.Completion.WaitAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// For operators that terminate before their source does (First, Any, IsEmpty, ElementAt, Take, Single
        /// faulting, ...): builds the pipeline over a fresh <see cref="PushSource{T}"/>, then pushes <c>1..n</c> and
        /// completes the source, stopping as soon as the sink has terminated, the way a producer that honours
        /// termination would.
        /// </summary>
        /// <remarks>
        /// This is a design choice, not a workaround. Over a cold source that pushes inside <c>SubscribeAsync</c>
        /// (such as <c>Range</c> on the immediate scheduler), the source has not yet returned its subscription when
        /// the operator terminates, so it cannot be cancelled. It pushes its remaining elements into the terminated
        /// observer, which drops them, so the work would grow with N instead of with the operator's needs.
        /// </remarks>
        public static ValueTask RunUntilDoneAsync<TResult>(int n, Consumer consumer, Func<IAsyncObservable<int>, IAsyncObservable<TResult>> build) =>
            RunUntilDoneAsync(n, consumer, build, static (source, b) => b(source));

        /// <inheritdoc cref="RunUntilDoneAsync{TResult}(int, Consumer, Func{IAsyncObservable{int}, IAsyncObservable{TResult}})"/>
        public static async ValueTask RunUntilDoneAsync<TState, TResult>(int n, Consumer consumer, TState state, Func<IAsyncObservable<int>, TState, IAsyncObservable<TResult>> build)
        {
            var source = new PushSource<int>();
            var sink = Sinks.For<TResult>(consumer);
            var done = sink.Completion;
            await build(source, state).SubscribeAsync(sink).ConfigureAwait(false);

            for (var i = 1; i <= n && !done.IsSet; i++)
            {
                await source.OnNextAsync(i).ConfigureAwait(false);
            }

            if (!done.IsSet)
            {
                await source.OnCompletedAsync().ConfigureAwait(false);
            }

            await done.WaitAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Subscribes the stateless consuming sink to <paramref name="pipeline"/> (built over
        /// <paramref name="sources"/>), pushes <c>0..n-1</c> into every source, then disposes the subscription
        /// without completing any source. With <paramref name="interleaved"/> the pushes go round-robin (each
        /// value into every source in turn); otherwise each source receives all N before the next one starts.
        /// </summary>
        /// <remarks>
        /// Used by <c>InterleavedCombiningBenchmarks</c> for <c>Zip</c>, to measure the interleaved pairing path
        /// alone. The sources are never completed.
        /// </remarks>
        public static async ValueTask PushAllAndDisposeAsync<TResult>(this IAsyncObservable<TResult> pipeline, PushSource<int>[] sources, int n, bool interleaved, Consumer consumer)
        {
            var subscription = await pipeline.SubscribeNoWaitAsync(consumer).ConfigureAwait(false);

            if (interleaved)
            {
                for (var i = 0; i < n; i++)
                {
                    for (var s = 0; s < sources.Length; s++)
                    {
                        await sources[s].OnNextAsync(i).ConfigureAwait(false);
                    }
                }
            }
            else
            {
                for (var s = 0; s < sources.Length; s++)
                {
                    await PushAsync(sources[s], 0, n).ConfigureAwait(false);
                }
            }

            await subscription.DisposeAsync().ConfigureAwait(false);
        }

        /// <summary>Creates <paramref name="count"/> fresh <see cref="PushSource{T}"/>s.</summary>
        public static PushSource<int>[] Sources(int count)
        {
            var sources = new PushSource<int>[count];
            for (var i = 0; i < count; i++)
            {
                sources[i] = new PushSource<int>();
            }

            return sources;
        }

        /// <summary>
        /// Subscribes the stateless consuming sink to <paramref name="pipeline"/>, pushes <c>0..n-1</c> into
        /// <paramref name="source"/> without completing it, then disposes the still-live subscription.
        /// </summary>
        public static async ValueTask RunAndDisposeAsync<TResult>(this IAsyncObservable<TResult> pipeline, IAsyncObserver<int> source, int n, Consumer consumer)
        {
            var subscription = await pipeline.SubscribeNoWaitAsync(consumer).ConfigureAwait(false);
            await PushAsync(source, 0, n).ConfigureAwait(false);
            await subscription.DisposeAsync().ConfigureAwait(false);
        }
    }
}

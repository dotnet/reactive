// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Reactive.Testing
{
    /// <summary>
    /// Virtual time scheduler used for testing applications and libraries built using Reactive Extensions.
    /// </summary>
    [DebuggerDisplay("\\{ " +
        nameof(Clock) + " = {" + nameof(Clock) + "} " +
        nameof(Now) + " = {" + nameof(Now) + ".ToString(\"O\")} " +
    "\\}")]
    public class TestAsyncScheduler : PriorityQueueVirtualTimeAsyncScheduler<long,long>
    {

        /// <summary>
        /// Converts the absolute virtual time value to a DateTimeOffset value.
        /// </summary>
        /// <param name="absolute">Absolute virtual time value to convert.</param>
        /// <returns>Corresponding DateTimeOffset value.</returns>
        protected override DateTimeOffset ToDateTimeOffset(long absolute)
        {
            return new DateTimeOffset(absolute, TimeSpan.Zero);
        }

        /// <summary>
        /// Converts the TimeSpan value to a relative virtual time value.
        /// </summary>
        /// <param name="timeSpan">TimeSpan value to convert.</param>
        /// <returns>Corresponding relative virtual time value.</returns>
        protected override long ToRelative(TimeSpan timeSpan)
        {
            return timeSpan.Ticks;
        }

        /// <summary>
        /// Adds a relative virtual time to an absolute virtual time value.
        /// </summary>
        /// <param name="absolute">Absolute virtual time value.</param>
        /// <param name="relative">Relative virtual time value to add.</param>
        /// <returns>Resulting absolute virtual time sum value.</returns>
        protected override long Add(long absolute, long relative)
        {
            return absolute + relative;
        }


        /// <summary>
        /// Schedules an action to be executed at the specified virtual time.
        /// </summary>
        /// <param name="action">Action to be executed.</param>
        /// <param name="dueTime">Absolute virtual time at which to execute the action.</param>
        /// <returns>Disposable object used to cancel the scheduled action (best effort).</returns>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> is null.</exception>
        public override Task<IAsyncDisposable> ScheduleAbsolute(long dueTime, Func<CancellationToken, Task> action)
        {
            if (dueTime <= Clock)
            {
                dueTime = Clock + 1;
            }

            return base.ScheduleAbsolute(dueTime, action);
        }

        




        /// <summary>
        /// Starts the test scheduler and uses the specified virtual times to invoke the factory function, subscribe to the resulting sequence, and dispose the subscription.
        /// </summary>
        /// <typeparam name="T">The element type of the observable sequence being tested.</typeparam>
        /// <param name="create">Factory method to create an observable sequence.</param>
        /// <param name="created">Virtual time at which to invoke the factory to create an observable sequence.</param>
        /// <param name="subscribed">Virtual time at which to subscribe to the created observable sequence.</param>
        /// <param name="disposed">Virtual time at which to dispose the subscription.</param>
        /// <returns>Observer with timestamped recordings of notification messages that were received during the virtual time window when the subscription to the source sequence was active.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="create"/> is null.</exception>
        public async Task<ITestableAsyncObserver<T>> Start<T>(Func<IAsyncObservable<T>> create, long created, long subscribed, long disposed)
        {
            if (create == null)
            {
                throw new ArgumentNullException(nameof(create));
            }

            var source = default(IAsyncObservable<T>);
            var subscription = default(IAsyncDisposable);
            var observer = new MockAsyncObserver<T>(this);

            await ScheduleAbsolute(created, _ => { source = create(); return Task.CompletedTask; }).ConfigureAwait(false);
            await ScheduleAbsolute(subscribed, async _ => { subscription = await source.SubscribeAsync(observer); }).ConfigureAwait(false);
            await ScheduleAbsolute(disposed, async _ => { await subscription.DisposeAsync(); }).ConfigureAwait(false);

            await Start().ConfigureAwait(false);

            return observer;
        }

        /// <summary>
        /// Starts the test scheduler and uses default virtual times to <see cref="ReactiveTest.Created">invoke the factory function</see>, to <see cref="ReactiveTest.Subscribed">subscribe to the resulting sequence</see>, and to <see cref="ReactiveTest.Disposed">dispose the subscription</see>.
        /// </summary>
        /// <typeparam name="T">The element type of the observable sequence being tested.</typeparam>
        /// <param name="create">Factory method to create an observable sequence.</param>
        /// <returns>Observer with timestamped recordings of notification messages that were received during the virtual time window when the subscription to the source sequence was active.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="create"/> is null.</exception>
        public Task<ITestableAsyncObserver<T>> Start<T>(Func<IAsyncObservable<T>> create)
        {
            if (create == null) throw new ArgumentNullException(nameof(create));

            return Start(create, ReactiveTest.Created, ReactiveTest.Subscribed, ReactiveTest.Disposed);
        }

        public Task<ITestableAsyncObservable<T>> CreateHotObservable<T>(params Recorded<Notification<T>>[] messages) => HotAsyncObservable<T>.Create(this, messages);
    }
}

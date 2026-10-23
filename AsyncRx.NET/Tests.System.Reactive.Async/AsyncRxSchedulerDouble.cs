// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Disposables;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

/// <summary>AsyncRx.NET's adapter over a <see cref="SchedulerDouble"/>.</summary>
/// <param name="scheduler">The double, which decides what happens to each unit of work.</param>
/// <remarks>
/// Each <c>ScheduleAsync</c> hands the action to the double as a <see cref="ScheduledWork"/>,
/// with a token the returned disposable cancels; an absolute due time becomes a delay from the
/// double's <see cref="SchedulerDouble.Now"/>. AsyncRx.NET has no long-running path, so a
/// long-running double receives every unit of work the same way.
/// </remarks>
internal sealed class AsyncRxSchedulerDouble(SchedulerDouble scheduler) : IAsyncScheduler
{
    public DateTimeOffset Now => scheduler.Now;

    public async ValueTask<IAsyncDisposable> ScheduleAsync<TState>(
        TState state,
        Func<TState, CancellationToken, ValueTask> action)
    {
        var cancel = new CancellationAsyncDisposable();

        await scheduler.ScheduleAsync(ct => action(state, ct), cancel.Token).ConfigureAwait(false);

        return cancel;
    }

    public async ValueTask<IAsyncDisposable> ScheduleAsync<TState>(
        TState state,
        TimeSpan dueTime,
        Func<TState, CancellationToken, ValueTask> action)
    {
        var cancel = new CancellationAsyncDisposable();

        await scheduler.ScheduleAsync(dueTime, ct => action(state, ct), cancel.Token)
            .ConfigureAwait(false);

        return cancel;
    }

    public ValueTask<IAsyncDisposable> ScheduleAsync<TState>(
        TState state,
        DateTimeOffset dueTime,
        Func<TState, CancellationToken, ValueTask> action) =>
        ScheduleAsync(state, dueTime - Now, action);
}

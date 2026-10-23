// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Disposables;

namespace Tests.System.Reactive.Shared.Rx;

/// <summary>Rx.NET's adapter over a <see cref="SchedulerDouble"/>.</summary>
/// <param name="scheduler">The double, which decides what happens to each unit of work.</param>
/// <remarks>
/// Each <c>Schedule</c> wraps <c>action(this, state)</c> as a <see cref="ScheduledWork"/> that
/// completes synchronously, hands it to the double with a token the returned disposable
/// cancels, and requires the double to have finished with it synchronously too. A long-running
/// double is reached only through <see cref="ISchedulerLongRunning"/>, which
/// <c>Scheduler.AsLongRunning</c> discovers through <see cref="IServiceProvider"/>; its ordinary
/// members throw, as every long-running double in Rx.NET's tests does.
/// </remarks>
internal sealed class RxSchedulerDouble(SchedulerDouble scheduler)
    : IScheduler, ISchedulerLongRunning, IServiceProvider
{
    public DateTimeOffset Now => scheduler.Now;

    public IDisposable Schedule<TState>(TState state, Func<IScheduler, TState, IDisposable> action)
    {
        RequireOrdinaryPath();

        var cancel = new CancellationDisposable();
        var inner = new SingleAssignmentDisposable();

        RunSynchronously(scheduler.ScheduleAsync(
            _ =>
            {
                inner.Disposable = action(this, state);
                return default;
            },
            cancel.Token));

        return StableCompositeDisposable.Create(cancel, inner);
    }

    public IDisposable Schedule<TState>(
        TState state,
        TimeSpan dueTime,
        Func<IScheduler, TState, IDisposable> action)
    {
        RequireOrdinaryPath();

        var cancel = new CancellationDisposable();
        var inner = new SingleAssignmentDisposable();

        RunSynchronously(scheduler.ScheduleAsync(
            dueTime,
            _ =>
            {
                inner.Disposable = action(this, state);
                return default;
            },
            cancel.Token));

        return StableCompositeDisposable.Create(cancel, inner);
    }

    public IDisposable Schedule<TState>(
        TState state,
        DateTimeOffset dueTime,
        Func<IScheduler, TState, IDisposable> action) =>
        Schedule(state, dueTime - Now, action);

    public IDisposable ScheduleLongRunning<TState>(TState state, Action<TState, ICancelable> action)
    {
        var cancel = new CancellationDisposable();

        RunSynchronously(scheduler.ScheduleAsync(
            _ =>
            {
                action(state, cancel);
                return default;
            },
            cancel.Token));

        return cancel;
    }

    public object? GetService(Type serviceType) =>
        serviceType == typeof(ISchedulerLongRunning) && scheduler.IsLongRunning ? this : null;

    private void RequireOrdinaryPath()
    {
        if (scheduler.IsLongRunning)
        {
            throw new NotImplementedException(
                "This double offers only the long-running path; "
                + "the operator should have taken it.");
        }
    }

    private static void RunSynchronously(ValueTask task)
    {
        if (!task.IsCompleted)
        {
            throw new InvalidOperationException(
                "A scheduler double must handle Rx.NET's synchronous work synchronously.");
        }

        task.GetAwaiter().GetResult();
    }
}

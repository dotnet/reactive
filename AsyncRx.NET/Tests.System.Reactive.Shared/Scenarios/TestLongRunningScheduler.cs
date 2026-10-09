// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>A scheduler that runs each unit of work on its own thread and reports on it.</summary>
/// <param name="target">The target under test.</param>
/// <remarks>
/// <para>
/// Rx.NET's <c>TestLongRunningScheduler</c>, shared by several of its test classes
/// (<c>ObserveOn</c>, <c>Range</c>, <c>Generate</c>, <c>Repeat</c>, <c>ToObservable</c>), so a
/// top-level type here. The original offers only
/// <c>ISchedulerLongRunning.ScheduleLongRunning</c>, running the work on a new task and setting
/// an event when it starts and one when it ends, with the exception it threw handed back; its
/// ordinary <c>Schedule</c> throws, which is how a test proves an operator took the long-running
/// path. As a <see cref="SchedulerDouble"/> with <see cref="IsLongRunning"/> set, that is what
/// it is on Rx.NET; on AsyncRx.NET, which has no long-running scheduler concept, every scheduled
/// action runs on its own task, and an operator's drain loop is one such action.
/// </para>
/// <para>
/// <see cref="Started"/> completes when the first unit of work begins, <see cref="Stopped"/> when
/// the last active unit ends, and <see cref="Faulted"/> when a unit ends with an exception,
/// which <see cref="Exception"/> then holds. The original has only the first two: its one unit
/// of work is the whole dispatch loop, so its "stopped" also means "the loop ended because the
/// observer threw". On AsyncRx.NET each burst of notifications is a unit of its own, and a test
/// that waits for the loop to end that way awaits <see cref="Faulted"/>.
/// </para>
/// </remarks>
public sealed class TestLongRunningScheduler(IRxTarget target)
    : SchedulerDouble(target, "TestLongRunningScheduler")
{
    private readonly TaskCompletionSource _started =
        new(TaskCreationOptions.RunContinuationsAsynchronously);

    private readonly TaskCompletionSource _stopped =
        new(TaskCreationOptions.RunContinuationsAsynchronously);

    private readonly TaskCompletionSource _faulted =
        new(TaskCreationOptions.RunContinuationsAsynchronously);

    private int _active;

    /// <summary>Completes when the first unit of work begins.</summary>
    public Task Started => _started.Task;

    /// <summary>Completes when the last active unit of work ends.</summary>
    public Task Stopped => _stopped.Task;

    /// <summary>Completes when a unit of work ends with an exception.</summary>
    public Task Faulted => _faulted.Task;

    /// <summary>The exception a unit of work threw, if any.</summary>
    public Exception? Exception { get; private set; }

    /// <inheritdoc/>
    public override bool IsLongRunning => true;

    /// <inheritdoc/>
    public override ValueTask ScheduleAsync(ScheduledWork work, CancellationToken cancellationToken)
    {
        _ = Task.Run(async () =>
        {
            Interlocked.Increment(ref _active);
            _started.TrySetResult();

            try
            {
                await work(cancellationToken).ConfigureAwait(false);
                OnEnded(null);
            }
            catch (Exception ex)
            {
                OnEnded(ex);
            }
        });

        return default;
    }

    private void OnEnded(Exception? exception)
    {
        if (exception is not null)
        {
            Exception = exception;
            _faulted.TrySetResult();
        }

        if (Interlocked.Decrement(ref _active) == 0)
        {
            _stopped.TrySetResult();
        }
    }
}

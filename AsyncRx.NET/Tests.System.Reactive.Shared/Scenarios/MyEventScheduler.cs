// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>
/// Rx.NET's scheduler double for the event conversion tests: runs each unit of work at once and
/// reports it, so a test can count how often the conversion scheduled.
/// </summary>
/// <remarks>
/// The original derives from Rx.NET's <c>LocalScheduler</c> and takes no delayed work; this one
/// is the shared <see cref="SchedulerDouble"/>, which each target adapts to its own scheduler
/// interface. Delayed work throws, as the original's does.
/// </remarks>
public sealed class MyEventScheduler(IRxTarget target, Action schedule)
    : SchedulerDouble(target, "MyEventScheduler")
{
    /// <inheritdoc/>
    public override ValueTask ScheduleAsync(ScheduledWork work, CancellationToken cancellationToken)
    {
        schedule();
        return work(cancellationToken);
    }
}

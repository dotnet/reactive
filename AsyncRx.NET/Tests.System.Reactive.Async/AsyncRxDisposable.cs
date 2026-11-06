// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Runtime.CompilerServices;

using Microsoft.Reactive.Testing.Async;

namespace Tests.System.Reactive.Async;

/// <summary>
/// Presents a subscription or connection to a scenario so that disposing it from the test body
/// completes at the current virtual time.
/// </summary>
/// <remarks>
/// <para>
/// The counterpart of the Rx.NET target's <c>RxDisposable</c>, for the other direction of the
/// same problem. There, a synchronous disposal has to meet the shared async-shaped surface.
/// Here, the disposal is already async, but under <see cref="ExecutionShape.ForcedYield"/> a
/// testable observable's unsubscribe suspends at a yield point and resumes only when the pump
/// runs, so a scenario that writes <c>await disconnect.DisposeAsync()</c> in its body, as the
/// Rx.NET tests write <c>disconnect.Dispose()</c> before <c>Start()</c>, would wait forever.
/// <see cref="DisposeAsync"/> hands the real disposal to
/// <see cref="TestAsyncScheduler.RunToCompletion(ValueTask)"/>, which pumps the current tick
/// when called from the body, does nothing when called from scheduled work, and, in a
/// real-time scenario where nothing is scheduled in virtual time, lets the disposal complete
/// on whatever thread is finishing it.
/// </para>
/// <para>
/// <see cref="For"/> returns the same wrapper for the same real disposable, so a scenario that
/// compares two results by identity (a connectable's <c>Connect()</c> returns the same
/// connection while it is connected) sees what it would see on the library itself.
/// </para>
/// </remarks>
internal sealed class AsyncRxDisposable : IAsyncDisposable
{
    private static readonly ConditionalWeakTable<IAsyncDisposable, AsyncRxDisposable> Wrappers =
        new();

    private readonly IAsyncDisposable _disposable;
    private readonly TestAsyncScheduler _scheduler;

    private AsyncRxDisposable(IAsyncDisposable disposable, TestAsyncScheduler scheduler)
    {
        _disposable = disposable;
        _scheduler = scheduler;
    }

    /// <summary>The wrapper for <paramref name="disposable"/>, the same one each time.</summary>
    /// <param name="disposable">The real subscription or connection.</param>
    /// <param name="scheduler">The scenario's scheduler, whose pump completes the disposal.</param>
    public static AsyncRxDisposable For(IAsyncDisposable disposable, TestAsyncScheduler scheduler)
    {
        ArgumentNullException.ThrowIfNull(disposable);
        ArgumentNullException.ThrowIfNull(scheduler);

        return Wrappers.GetValue(disposable, d => new AsyncRxDisposable(d, scheduler));
    }

    public ValueTask DisposeAsync() => _scheduler.RunToCompletion(_disposable.DisposeAsync());
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>A unit of work a scheduler double has been asked to run.</summary>
/// <param name="cancellationToken">Cancelled when the work's subscription is disposed.</param>
/// <remarks>
/// The target-neutral form of what a scheduler is handed: on Rx.NET the target's adapter wraps
/// the <c>action(scheduler, state)</c> call, which completes synchronously; on AsyncRx.NET it
/// wraps the <c>Func&lt;TState, CancellationToken, ValueTask&gt;</c> as it is.
/// </remarks>
public delegate ValueTask ScheduledWork(CancellationToken cancellationToken);

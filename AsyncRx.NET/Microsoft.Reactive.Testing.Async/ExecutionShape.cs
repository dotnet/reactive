// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Microsoft.Reactive.Testing.Async;

/// <summary>
/// Determines how the harness's own await points behave, so every scenario can be run
/// down both async code paths. This is a path-coverage axis, not schedule exploration:
/// under the canonical (continuation-priority) pump both shapes produce identical traces,
/// but <see cref="ForcedYield"/> makes the suspension machinery of async state machines
/// actually execute (continuation scheduling, context capture), catching broken state
/// machines and accidental assumptions of synchronous completion.
/// </summary>
public enum ExecutionShape
{
    /// <summary>
    /// Harness await points complete synchronously, short-circuiting async state machines
    /// the same way a synchronously-completed <see cref="ValueTask"/> does in production.
    /// </summary>
    SynchronousCompletion,

    /// <summary>
    /// Harness await points report incomplete and genuinely suspend, resuming via the
    /// pump's continuation queue (ahead of other work at the same virtual time, preserving
    /// the canonical order).
    /// </summary>
    ForcedYield,
}

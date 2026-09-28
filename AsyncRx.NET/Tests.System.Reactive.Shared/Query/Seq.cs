// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>
/// A description of an observable sequence of <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of the elements in the described sequence.</typeparam>
/// <remarks>
/// <para>
/// What a shared scenario holds where the Rx.NET test it was migrated from holds an
/// <c>IObservable&lt;T&gt;</c>.
/// </para>
/// <para>
/// A <see cref="Seq{T}"/> is a description, not an observable: nothing here runs. It is a tree
/// whose leaves are the target's own objects (<see cref="NativeSeq{T}"/>: a testable source the
/// test created through the scheduler, or an inner window or group handed to a callback) and the
/// neutral creation operators (<see cref="Seq"/>), and whose interior nodes are operator
/// applications, one node type per overload (see the <c>Operators</c> folder).
/// </para>
/// <para>
/// A target (<see cref="IRxTarget"/>) is a visitor that turns a tree into its own pipeline at the
/// moment the test's <c>Start</c> callback is invoked, and the result is native from end to end:
/// <c>xs.Window(...).Select((w, i) =&gt; ...).Merge()</c> materializes as the target's
/// <c>Window</c>, <c>Select</c> and <c>Merge</c> with nothing in between — the inner window
/// reaches the projection callback as a <see cref="NativeSeq{T}"/> value, and the description the
/// callback returns is materialized in place. A runtime abstraction that dispatched each operator
/// call to the target at once could not do that: it would have to wrap every inner window with a
/// <c>Select</c> and unwrap it with another, so the pipeline under test would not be the one
/// written. Everything around the query — the scheduler, the sources, <c>Start</c>, the
/// assertions — is in the <c>Harness</c> folder.
/// </para>
/// </remarks>
public abstract class Seq<T> : ISeq
{
    /// <inheritdoc/>
    public abstract object Accept(ISeqVisitor visitor);

    /// <summary>
    /// The description as the query was written in the scenario (for example
    /// <c>Hot(3 messages).Take(2)</c>), for diagnostics.
    /// </summary>
    public abstract override string ToString();
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>A description of a connectable sequence, one that starts when told to.</summary>
/// <remarks>
/// What <c>Publish</c> and its relatives produce: a <see cref="Seq{T}"/> that can also be
/// connected, the target's own <c>IConnectableObservable&lt;T&gt;</c> or
/// <c>IConnectableAsyncObservable&lt;T&gt;</c>. Subscribing attaches to the shared subject;
/// <see cref="ConnectAsync"/> subscribes the subject to the source. Because a description
/// materializes once, the sequence a scenario subscribes to at one tick and connects at another
/// is the same real object.
/// </remarks>
public abstract class ConnectableSeq<T> : Seq<T>
{
    /// <summary>Connects the sequence to its source, the target's own <c>Connect()</c>.</summary>
    /// <param name="scheduler">The test's scheduler, which carries the target.</param>
    /// <remarks>
    /// Returns the connection, whose disposal disconnects. Async-shaped, like the rest of the raw
    /// surface; on Rx.NET it completes synchronously.
    /// </remarks>
    public ValueTask<IAsyncDisposable> ConnectAsync(TestSchedulerRef scheduler)
    {
        ArgumentNullException.ThrowIfNull(scheduler);

        return scheduler.Target.ConnectAsync(scheduler, this);
    }
}

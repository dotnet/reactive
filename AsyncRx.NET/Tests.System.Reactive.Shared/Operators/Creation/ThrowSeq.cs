// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>Observable.Throw&lt;T&gt;(error)</c> or <c>Observable.Throw&lt;T&gt;(error, scheduler)</c>.</summary>
/// <remarks>Built by <see cref="Observable.Throw{T}(Exception)"/>; materialized by each target through <see cref="ISeqVisitor.Throw{T}(ThrowSeq{T})"/>.</remarks>
public sealed class ThrowSeq<T>(Exception error, SchedulerRef? scheduler) : Seq<T>
{
    public Exception Error => error;

    public SchedulerRef? Scheduler => scheduler;

    public override object Accept(ISeqVisitor visitor) => visitor.Throw(this);

    public override string ToString() =>
        scheduler is null ? $"Observable.Throw<{typeof(T).Name}>({error.GetType().Name})" : $"Observable.Throw<{typeof(T).Name}>({error.GetType().Name}, {scheduler})";
}

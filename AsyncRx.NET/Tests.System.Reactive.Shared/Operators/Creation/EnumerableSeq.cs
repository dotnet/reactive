// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.ToSeq()</c>, an enumerable as a sequence.</summary>
/// <remarks>
/// Built by <see cref="Seq.ToSeq{T}(IEnumerable{T})"/>; materialized by each target through
/// <see cref="ISeqVisitor.Enumerable{T}(EnumerableSeq{T})"/>.
/// </remarks>
public sealed class EnumerableSeq<T>(IEnumerable<T> source, SchedulerRef? scheduler) : Seq<T>
{
    /// <summary>Enumerable sequence to convert to an observable sequence.</summary>
    public IEnumerable<T> Source => source;

    /// <summary>
    /// Scheduler to run the enumeration of the input sequence on, or null for the target's
    /// default.
    /// </summary>
    public SchedulerRef? Scheduler => scheduler;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) => visitor.Enumerable(this);

    /// <inheritdoc/>
    public override string ToString() =>
        scheduler is null ? $"{SourceText}.ToSeq()" : $"{SourceText}.ToSeq({scheduler})";

    // A collection prints its elements; any other enumerable prints its type, since enumerating
    // it is the operator's job and an iterator may throw (the Step 36 and 68 rule).
    private string SourceText =>
        source is ICollection<T> ? "[" + string.Join(", ", source) + "]" : source.GetType().Name;
}

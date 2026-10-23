// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>
/// Describes <c>source.StartWith(scheduler, values)</c> over an enumerable of values.
/// </summary>
/// <remarks>
/// Built by
/// <see cref="StartWithExtensions.StartWith{T}(Seq{T}, SchedulerRef, IEnumerable{T})"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.StartWithSchedulerEnumerable{T}(StartWithSchedulerEnumerableSeq{T})"/>.
/// The text prints a collection's elements and names any other enumerable's type, since
/// enumerating it is the operator's job.
/// </remarks>
public sealed class StartWithSchedulerEnumerableSeq<T>(
    Seq<T> source,
    SchedulerRef scheduler,
    IEnumerable<T> values)
    : Seq<T>
{
    /// <summary>Source sequence to prepend values to.</summary>
    public Seq<T> Source => source;

    /// <summary>Scheduler to emit the prepended values on.</summary>
    public SchedulerRef Scheduler => scheduler;

    /// <summary>Values to prepend to the specified sequence.</summary>
    public IEnumerable<T> Values => values;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) =>
        visitor.StartWithSchedulerEnumerable(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.StartWith({scheduler}, {ValuesText})";

    private string ValuesText =>
        values is ICollection<T> ? "[" + string.Join(", ", values) + "]" : values.GetType().Name;
}

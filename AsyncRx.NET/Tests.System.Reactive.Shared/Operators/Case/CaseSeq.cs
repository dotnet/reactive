// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>Seq.Case(selector, sources[, defaultSource | scheduler])</c>.</summary>
/// <typeparam name="TValue">The type of the value returned by the selector function.</typeparam>
/// <typeparam name="TResult">The type of the elements in the result sequence.</typeparam>
/// <remarks>
/// Built by the <c>Seq.Case</c> overloads in <c>Seq.Case.cs</c>; materialized by each target
/// through <see cref="ISeqVisitor.Case{TValue, TResult}(CaseSeq{TValue, TResult})"/>, which
/// converts the dictionary of descriptions into one of the target's own sequences. At most one
/// of <see cref="DefaultSource"/> and <see cref="Scheduler"/> is set; with neither, the default
/// is the target's own empty sequence on its default scheduler.
/// </remarks>
public sealed class CaseSeq<TValue, TResult>(
    Func<TValue> selector,
    IDictionary<TValue, Seq<TResult>> sources,
    Seq<TResult>? defaultSource,
    SchedulerRef? scheduler,
    string text)
    : Seq<TResult>
    where TValue : notnull
{
    /// <summary>Selector function used to determine which source to run.</summary>
    public Func<TValue> Selector => selector;

    /// <summary>Dictionary of sources to select from based on the selector's result.</summary>
    public IDictionary<TValue, Seq<TResult>> Sources => sources;

    /// <summary>
    /// Default source to select if the selector's value is not in the dictionary, or null.
    /// </summary>
    public Seq<TResult>? DefaultSource => defaultSource;

    /// <summary>
    /// Scheduler to generate an empty sequence on if the selector's value is not in the
    /// dictionary, or null.
    /// </summary>
    public SchedulerRef? Scheduler => scheduler;

    /// <inheritdoc/>
    protected override Realized<Seq<TResult>> AcceptCore(ISeqVisitor visitor) =>
        visitor.Case(this);

    /// <inheritdoc/>
    public override string ToString() =>
        defaultSource is not null ? $"Seq.Case({text}, sources, {defaultSource})"
        : scheduler is not null ? $"Seq.Case({text}, sources, {scheduler})"
        : $"Seq.Case({text}, sources)";
}

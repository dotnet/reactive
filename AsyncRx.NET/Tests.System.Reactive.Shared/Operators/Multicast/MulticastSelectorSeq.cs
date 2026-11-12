// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Multicast(subjectSelector, selector)</c>.</summary>
/// <remarks>
/// Built by
/// <see cref="MulticastExtensions.Multicast{T, TResult}(Seq{T}, Func{SubjectSeq{T}}, Func{Seq{T}, Seq{TResult}}, string)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.MulticastSelector{T, TResult}(MulticastSelectorSeq{T, TResult})"/>. The
/// intermediate element type is the source's: Rx.NET's <c>TIntermediate</c> is always
/// <c>TSource</c> in its tests.
/// </remarks>
public sealed class MulticastSelectorSeq<T, TResult>(
    Seq<T> source,
    Func<SubjectSeq<T>> subjectSelector,
    Func<Seq<T>, Seq<TResult>> selector,
    string text) : Seq<TResult>
{
    /// <summary>
    /// Source sequence which will be multicasted in the specified selector function.
    /// </summary>
    public Seq<T> Source => source;

    /// <summary>
    /// Factory function to create an intermediate subject through which the source sequence's
    /// elements will be multicast to the selector function.
    /// </summary>
    public Func<SubjectSeq<T>> SubjectSelector => subjectSelector;

    /// <summary>
    /// Selector function which can use the multicasted source sequence subject to the policies
    /// enforced by the created subject.
    /// </summary>
    public Func<Seq<T>, Seq<TResult>> Selector => selector;

    /// <inheritdoc/>
    protected override Realized<Seq<TResult>> AcceptCore(ISeqVisitor visitor) =>
        visitor.MulticastSelector(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Multicast({text})";
}

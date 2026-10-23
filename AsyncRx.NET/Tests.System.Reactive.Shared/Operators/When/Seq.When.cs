// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public static partial class Seq
{
    /// <summary>Describes <c>Seq.When(plans)</c>.</summary>
    /// <typeparam name="TResult">
    /// The type of the elements in the result sequence, obtained from the specified patterns.
    /// </typeparam>
    /// <param name="plans">
    /// A series of plans created by use of the Then operator on patterns.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="WhenSeq{TResult}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.When{TResult}(WhenSeq{TResult})"/>.
    /// </remarks>
    public static Seq<TResult> When<TResult>(params SeqPlan<TResult>[] plans) =>
        new WhenSeq<TResult>(plans);

    /// <summary>Describes <c>plans.When()</c>.</summary>
    /// <typeparam name="TResult">
    /// The type of the elements in the result sequence, obtained from the specified patterns.
    /// </typeparam>
    /// <param name="plans">
    /// A series of plans created by use of the Then operator on patterns.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="WhenEnumerableSeq{TResult}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.WhenEnumerable{TResult}(WhenEnumerableSeq{TResult})"/>.
    /// </remarks>
    public static Seq<TResult> When<TResult>(this IEnumerable<SeqPlan<TResult>> plans) =>
        new WhenEnumerableSeq<TResult>(plans);
}

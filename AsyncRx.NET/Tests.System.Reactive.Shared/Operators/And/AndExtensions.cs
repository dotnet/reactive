// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>The <c>And</c> operator, which starts a join pattern.</summary>
/// <remarks>
/// Where an Rx.NET test writes <c>xs.And(ys).Then((x, y) =&gt; ...)</c>, a shared scenario
/// writes the same over descriptions: <c>And</c> builds a <see cref="SeqPattern2{T1, T2}"/>,
/// whose own <c>And</c> grows it a source at a time up to sixteen and whose <c>Then</c> turns it
/// into a <see cref="SeqPlan{TResult}"/> for <c>Seq.When</c>. One pattern type per arity, as the
/// library has.
/// </remarks>
public static class AndExtensions
{
    /// <summary>Describes <c>left.And(right)</c>, a pattern over two sequences.</summary>
    /// <typeparam name="T1">The type of the elements in the left sequence.</typeparam>
    /// <typeparam name="T2">The type of the elements in the right sequence.</typeparam>
    /// <param name="left">Observable sequence to match with the right sequence.</param>
    /// <param name="right">Observable sequence to match with the left sequence.</param>
    /// <remarks>
    /// Builds a <see cref="SeqPattern2{T1, T2}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.And2{T1, T2}(SeqPattern2{T1, T2})"/>.
    /// </remarks>
    public static SeqPattern2<T1, T2> And<T1, T2>(this Seq<T1> left, Seq<T2> right) =>
        new(left, right);
}

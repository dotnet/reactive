// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="WhenSeq{TResult}"/> as the target's own <c>When(plans)</c> over
    /// an array of plans.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.When{TResult}(SeqPlan{TResult}[])"/>.</remarks>
    Realized<Seq<TResult>> When<TResult>(WhenSeq<TResult> seq);

    /// <summary>
    /// Materializes a <see cref="WhenEnumerableSeq{TResult}"/> as the target's own
    /// <c>When(plans)</c> over an enumerable of plans.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.When{TResult}(IEnumerable{SeqPlan{TResult}})"/>.</remarks>
    Realized<Seq<TResult>> WhenEnumerable<TResult>(WhenEnumerableSeq<TResult> seq);
}

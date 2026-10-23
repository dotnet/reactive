// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes a plan: a pattern with a selector, which <c>Seq.When</c> runs.</summary>
/// <typeparam name="TResult">The type of the elements the plan produces.</typeparam>
/// <remarks>
/// Built by <see cref="ThenExtensions.Then{TSource, TResult}(Seq{TSource}, Func{TSource, TResult}, string)"/>
/// over one sequence and by the <c>Then</c> of each pattern over two or more; one plan type per
/// arity (<c>SeqPlan1</c> to <c>SeqPlan16</c>), as the library has. A plan is not a sequence:
/// what a target holds for it is its own plan type (Rx.NET's <c>Plan&lt;TResult&gt;</c>).
/// </remarks>
public abstract class SeqPlan<TResult> : Description<SeqPlan<TResult>>
{
}

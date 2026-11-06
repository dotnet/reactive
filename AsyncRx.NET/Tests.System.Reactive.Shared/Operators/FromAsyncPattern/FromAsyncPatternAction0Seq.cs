// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;

namespace Tests.System.Reactive.Shared;

/// <summary>
/// Describes <c>Seq.FromAsyncPattern(begin, end)()</c>: the call of the function
/// <c>FromAsyncPattern</c> returns, over a Begin/End pair of 0 arguments whose End returns nothing.
/// </summary>
/// <remarks>
/// Built by the <c>Seq.FromAsyncPattern</c> overloads in <c>Seq.FromAsyncPattern.cs</c>, which
/// return a function that builds this node when called; materialized by each target through
/// <see cref="ISeqVisitor.FromAsyncPatternAction0(FromAsyncPatternAction0Seq)"/>, which calls the target's own returned
/// function with the arguments held here.
/// </remarks>
public sealed class FromAsyncPatternAction0Seq(Func<AsyncCallback, object?, IAsyncResult> begin,
        Action<IAsyncResult> end)
    : Seq<Unit>
{
    /// <summary>The delegate that begins the asynchronous operation.</summary>
    public Func<AsyncCallback, object?, IAsyncResult> Begin => begin;

    /// <summary>The delegate that ends the asynchronous operation.</summary>
    public Action<IAsyncResult> End => end;

    /// <inheritdoc/>
    protected override Realized<Seq<Unit>> AcceptCore(ISeqVisitor visitor) =>
        visitor.FromAsyncPatternAction0(this);

    /// <inheritdoc/>
    public override string ToString() =>
        "Seq.FromAsyncPattern(begin, end)(" + "" + ")";
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>
/// Describes <c>Seq.FromAsyncPattern(begin, end)()</c>: the call of the function
/// <c>FromAsyncPattern</c> returns, over a Begin/End pair of 0 arguments.
/// </summary>
/// <typeparam name="TResult">The type of the result returned by the End delegate.</typeparam>
/// <remarks>
/// Built by the <c>Seq.FromAsyncPattern</c> overloads in <c>Seq.FromAsyncPattern.cs</c>, which
/// return a function that builds this node when called; materialized by each target through
/// <see cref="ISeqVisitor.FromAsyncPatternFunc0{TResult}(FromAsyncPatternFunc0Seq{TResult})"/>, which calls the target's own returned
/// function with the arguments held here.
/// </remarks>
public sealed class FromAsyncPatternFunc0Seq<TResult>(Func<AsyncCallback, object?,
        IAsyncResult> begin, Func<IAsyncResult, TResult> end)
    : Seq<TResult>
{
    /// <summary>The delegate that begins the asynchronous operation.</summary>
    public Func<AsyncCallback, object?, IAsyncResult> Begin => begin;

    /// <summary>The delegate that ends the asynchronous operation.</summary>
    public Func<IAsyncResult, TResult> End => end;

    /// <inheritdoc/>
    protected override Realized<Seq<TResult>> AcceptCore(ISeqVisitor visitor) =>
        visitor.FromAsyncPatternFunc0(this);

    /// <inheritdoc/>
    public override string ToString() =>
        "Seq.FromAsyncPattern(begin, end)(" + "" + ")";
}

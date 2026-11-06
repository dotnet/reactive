// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;

namespace Tests.System.Reactive.Shared;

/// <summary>
/// Describes <c>Seq.FromAsyncPattern(begin, end)(arg1)</c>: the call of the function
/// <c>FromAsyncPattern</c> returns, over a Begin/End pair of 1 argument whose End returns nothing.
/// </summary>
/// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
/// <remarks>
/// Built by the <c>Seq.FromAsyncPattern</c> overloads in <c>Seq.FromAsyncPattern.cs</c>, which
/// return a function that builds this node when called; materialized by each target through
/// <see cref="ISeqVisitor.FromAsyncPatternAction1{T1}(FromAsyncPatternAction1Seq{T1})"/>, which calls the target's own returned
/// function with the arguments held here.
/// </remarks>
public sealed class FromAsyncPatternAction1Seq<T1>(Func<T1, AsyncCallback, object?,
        IAsyncResult> begin, Action<IAsyncResult> end, T1 arg1)
    : Seq<Unit>
{
    /// <summary>The delegate that begins the asynchronous operation.</summary>
    public Func<T1, AsyncCallback, object?, IAsyncResult> Begin => begin;

    /// <summary>The delegate that ends the asynchronous operation.</summary>
    public Action<IAsyncResult> End => end;

    /// <summary>The first argument the function was called with.</summary>
    public T1 Arg1 => arg1;

    /// <inheritdoc/>
    protected override Realized<Seq<Unit>> AcceptCore(ISeqVisitor visitor) =>
        visitor.FromAsyncPatternAction1(this);

    /// <inheritdoc/>
    public override string ToString() =>
        "Seq.FromAsyncPattern(begin, end)(" + string.Join(", ", new object?[] { arg1 }) + ")";
}

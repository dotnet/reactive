// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>
/// Describes <c>Seq.FromAsyncPattern(begin, end)(arg1, arg2, arg3, arg4, arg5, arg6)</c>: the call
/// of the function
/// <c>FromAsyncPattern</c> returns, over a Begin/End pair of 6 arguments.
/// </summary>
/// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
/// <typeparam name="T2">The type of the second argument passed to the function.</typeparam>
/// <typeparam name="T3">The type of the third argument passed to the function.</typeparam>
/// <typeparam name="T4">The type of the fourth argument passed to the function.</typeparam>
/// <typeparam name="T5">The type of the fifth argument passed to the function.</typeparam>
/// <typeparam name="T6">The type of the sixth argument passed to the function.</typeparam>
/// <typeparam name="TResult">The type of the result returned by the End delegate.</typeparam>
/// <remarks>
/// Built by the <c>Seq.FromAsyncPattern</c> overloads in <c>Seq.FromAsyncPattern.cs</c>, which
/// return a function that builds this node when called; materialized by each target through
/// <see cref="ISeqVisitor.FromAsyncPatternFunc6{T1, T2, T3, T4, T5, T6, TResult}(FromAsyncPatternFunc6Seq{T1, T2, T3, T4, T5, T6, TResult})"/>, which calls the target's own returned
/// function with the arguments held here.
/// </remarks>
public sealed class FromAsyncPatternFunc6Seq<T1, T2, T3, T4, T5, T6, TResult>(Func<T1, T2, T3, T4,
        T5, T6, AsyncCallback, object?, IAsyncResult> begin, Func<IAsyncResult, TResult> end,
        T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
    : Seq<TResult>
{
    /// <summary>The delegate that begins the asynchronous operation.</summary>
    public Func<T1, T2, T3, T4, T5, T6, AsyncCallback, object?, IAsyncResult> Begin => begin;

    /// <summary>The delegate that ends the asynchronous operation.</summary>
    public Func<IAsyncResult, TResult> End => end;

    /// <summary>The first argument the function was called with.</summary>
    public T1 Arg1 => arg1;

    /// <summary>The second argument the function was called with.</summary>
    public T2 Arg2 => arg2;

    /// <summary>The third argument the function was called with.</summary>
    public T3 Arg3 => arg3;

    /// <summary>The fourth argument the function was called with.</summary>
    public T4 Arg4 => arg4;

    /// <summary>The fifth argument the function was called with.</summary>
    public T5 Arg5 => arg5;

    /// <summary>The sixth argument the function was called with.</summary>
    public T6 Arg6 => arg6;

    /// <inheritdoc/>
    protected override Realized<Seq<TResult>> AcceptCore(ISeqVisitor visitor) =>
        visitor.FromAsyncPatternFunc6(this);

    /// <inheritdoc/>
    public override string ToString() =>
        "Seq.FromAsyncPattern(begin, end)(" + string.Join(
            ", ",
            new object?[]
            {
                arg1, arg2, arg3, arg4, arg5, arg6,
            }) + ")";
}

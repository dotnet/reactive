// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;

namespace Tests.System.Reactive.Shared;

/// <summary>
/// Describes <c>Seq.FromAsyncPattern(begin, end)(arg1, arg2, arg3, arg4, arg5, arg6, arg7)</c>: the
/// call of the function
/// <c>FromAsyncPattern</c> returns, over a Begin/End pair of 7 arguments whose End returns nothing.
/// </summary>
/// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
/// <typeparam name="T2">The type of the second argument passed to the function.</typeparam>
/// <typeparam name="T3">The type of the third argument passed to the function.</typeparam>
/// <typeparam name="T4">The type of the fourth argument passed to the function.</typeparam>
/// <typeparam name="T5">The type of the fifth argument passed to the function.</typeparam>
/// <typeparam name="T6">The type of the sixth argument passed to the function.</typeparam>
/// <typeparam name="T7">The type of the seventh argument passed to the function.</typeparam>
/// <remarks>
/// Built by the <c>Seq.FromAsyncPattern</c> overloads in <c>Seq.FromAsyncPattern.cs</c>, which
/// return a function that builds this node when called; materialized by each target through
/// <see cref="ISeqVisitor.FromAsyncPatternAction7{T1, T2, T3, T4, T5, T6, T7}(FromAsyncPatternAction7Seq{T1, T2, T3, T4, T5, T6, T7})"/>, which calls the target's own returned
/// function with the arguments held here.
/// </remarks>
public sealed class FromAsyncPatternAction7Seq<T1, T2, T3, T4, T5, T6, T7>(Func<T1, T2, T3, T4, T5,
        T6, T7, AsyncCallback, object?, IAsyncResult> begin, Action<IAsyncResult> end, T1 arg1,
        T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
    : Seq<Unit>
{
    /// <summary>The delegate that begins the asynchronous operation.</summary>
    public Func<T1, T2, T3, T4, T5, T6, T7, AsyncCallback, object?, IAsyncResult> Begin => begin;

    /// <summary>The delegate that ends the asynchronous operation.</summary>
    public Action<IAsyncResult> End => end;

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

    /// <summary>The seventh argument the function was called with.</summary>
    public T7 Arg7 => arg7;

    /// <inheritdoc/>
    protected override Realized<Seq<Unit>> AcceptCore(ISeqVisitor visitor) =>
        visitor.FromAsyncPatternAction7(this);

    /// <inheritdoc/>
    public override string ToString() =>
        "Seq.FromAsyncPattern(begin, end)(" + string.Join(
            ", ",
            new object?[]
            {
                arg1, arg2, arg3, arg4, arg5, arg6, arg7,
            }) + ")";
}

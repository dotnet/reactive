// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;

namespace Tests.System.Reactive.Shared;

/// <summary>
/// Describes <c>Seq.ToAsync(action[, scheduler])(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8,
/// arg9, arg10, arg11, arg12, arg13)</c>: the call of the function
/// <c>ToAsync</c> returns, over an action of 13 arguments.
/// </summary>
/// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
/// <typeparam name="T2">The type of the second argument passed to the function.</typeparam>
/// <typeparam name="T3">The type of the third argument passed to the function.</typeparam>
/// <typeparam name="T4">The type of the fourth argument passed to the function.</typeparam>
/// <typeparam name="T5">The type of the fifth argument passed to the function.</typeparam>
/// <typeparam name="T6">The type of the sixth argument passed to the function.</typeparam>
/// <typeparam name="T7">The type of the seventh argument passed to the function.</typeparam>
/// <typeparam name="T8">The type of the eighth argument passed to the function.</typeparam>
/// <typeparam name="T9">The type of the ninth argument passed to the function.</typeparam>
/// <typeparam name="T10">The type of the tenth argument passed to the function.</typeparam>
/// <typeparam name="T11">The type of the eleventh argument passed to the function.</typeparam>
/// <typeparam name="T12">The type of the twelfth argument passed to the function.</typeparam>
/// <typeparam name="T13">The type of the thirteenth argument passed to the function.</typeparam>
/// <remarks>
/// Built by the <c>Seq.ToAsync</c> overloads in <c>Seq.ToAsync.cs</c>, which return a function
/// that builds this node when called; materialized by each target through
/// <see cref="ISeqVisitor.ToAsyncAction13{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13}(ToAsyncAction13Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13})"/>, which calls the target's own returned
/// function with the arguments held here.
/// </remarks>
public sealed class ToAsyncAction13Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
        Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action,
        SchedulerRef? scheduler, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7,
        T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13)
    : Seq<Unit>
{
    /// <summary>Action to convert to an asynchronous function.</summary>
    public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> Action => action;

    /// <summary>Scheduler to invoke the action on, or null for the target's default.</summary>
    public SchedulerRef? Scheduler => scheduler;

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

    /// <summary>The eighth argument the function was called with.</summary>
    public T8 Arg8 => arg8;

    /// <summary>The ninth argument the function was called with.</summary>
    public T9 Arg9 => arg9;

    /// <summary>The tenth argument the function was called with.</summary>
    public T10 Arg10 => arg10;

    /// <summary>The eleventh argument the function was called with.</summary>
    public T11 Arg11 => arg11;

    /// <summary>The twelfth argument the function was called with.</summary>
    public T12 Arg12 => arg12;

    /// <summary>The thirteenth argument the function was called with.</summary>
    public T13 Arg13 => arg13;

    /// <inheritdoc/>
    protected override Realized<Seq<Unit>> AcceptCore(ISeqVisitor visitor) =>
        visitor.ToAsyncAction13(this);

    /// <inheritdoc/>
    public override string ToString() =>
        (scheduler is null ? "Seq.ToAsync(action)(" : $"Seq.ToAsync(action, {scheduler})(")
        + string.Join(
            ", ",
            new object?[]
            {
                arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8,
                arg9, arg10, arg11, arg12, arg13,
            })
        + ")";
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>
/// Describes <c>Seq.Generate(initialState, condition, iterate, resultSelector, timeSelector[,
/// scheduler])</c>.
/// </summary>
/// <remarks>
/// Built by the <c>Seq.Generate</c> overloads in <c>Seq.Generate.cs</c>; materialized by each
/// target through
/// <see cref="ISeqVisitor.GenerateAbsolute{TState, TResult}(GenerateAbsoluteSeq{TState, TResult})"/>.
/// </remarks>
public sealed class GenerateAbsoluteSeq<TState, TResult>(
    TState initialState,
    Func<TState, bool> condition,
    Func<TState, TState> iterate,
    Func<TState, TResult> resultSelector,
    Func<TState, DateTimeOffset> timeSelector,
    SchedulerRef? scheduler,
    string text)
    : Seq<TResult>
{
    /// <summary>Initial state.</summary>
    public TState InitialState => initialState;

    /// <summary>Condition to terminate generation (upon returning false).</summary>
    public Func<TState, bool> Condition => condition;

    /// <summary>Iteration step function.</summary>
    public Func<TState, TState> Iterate => iterate;

    /// <summary>Selector function for results produced in the sequence.</summary>
    public Func<TState, TResult> ResultSelector => resultSelector;

    /// <summary>
    /// Time selector function to control the speed of values being produced each iteration.
    /// </summary>
    public Func<TState, DateTimeOffset> TimeSelector => timeSelector;

    /// <summary>
    /// Scheduler on which to run the generator loop, or null for the target's default.
    /// </summary>
    public SchedulerRef? Scheduler => scheduler;

    /// <inheritdoc/>
    protected override Realized<Seq<TResult>> AcceptCore(ISeqVisitor visitor) =>
        visitor.GenerateAbsolute(this);

    /// <inheritdoc/>
    public override string ToString() =>
        scheduler is null
            ? $"Seq.Generate({initialState}, {text})"
            : $"Seq.Generate({initialState}, {text}, {scheduler})";
}

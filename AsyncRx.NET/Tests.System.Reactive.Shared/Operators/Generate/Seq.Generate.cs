// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Runtime.CompilerServices;

namespace Tests.System.Reactive.Shared;

/// <summary>The <c>Generate</c> overloads, as static methods on <see cref="Seq"/>.</summary>
/// <remarks>
/// On <see cref="Seq"/> because the Rx.NET tests call them statically, as
/// <c>Observable.Generate(0, x =&gt; ..., ...)</c>; a shared scenario writes the same with
/// <see cref="Seq"/> in place of <c>Observable</c>. Three node types, one per family (no time
/// selector, a relative one, an absolute one), each with the scheduler optional; the condition's
/// source text is captured for diagnostics, which is enough to tell one query from another.
/// Each target turns a node into its own <c>Generate</c> call through the matching
/// <see cref="ISeqVisitor"/> member in <c>ISeqVisitor.Generate.cs</c>.
/// </remarks>
public static partial class Seq
{
    /// <summary>
    /// Describes <c>Seq.Generate(initialState, condition, iterate, resultSelector)</c>.
    /// </summary>
    /// <typeparam name="TState">The type of the state used in the generator loop.</typeparam>
    /// <typeparam name="TResult">The type of the elements in the produced sequence.</typeparam>
    /// <param name="initialState">Initial state.</param>
    /// <param name="condition">Condition to terminate generation (upon returning false).</param>
    /// <param name="iterate">Iteration step function.</param>
    /// <param name="resultSelector">Selector function for results produced in the sequence.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the condition), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    public static Seq<TResult> Generate<TState, TResult>(
        TState initialState,
        Func<TState, bool> condition,
        Func<TState, TState> iterate,
        Func<TState, TResult> resultSelector,
        [CallerArgumentExpression(nameof(condition))] string text = "") =>
        new GenerateSeq<TState, TResult>(
            initialState, condition, iterate, resultSelector, null, text);

    /// <summary>
    /// Describes <c>Seq.Generate(initialState, condition, iterate, resultSelector, scheduler)</c>.
    /// </summary>
    /// <typeparam name="TState">The type of the state used in the generator loop.</typeparam>
    /// <typeparam name="TResult">The type of the elements in the produced sequence.</typeparam>
    /// <param name="initialState">Initial state.</param>
    /// <param name="condition">Condition to terminate generation (upon returning false).</param>
    /// <param name="iterate">Iteration step function.</param>
    /// <param name="resultSelector">Selector function for results produced in the sequence.</param>
    /// <param name="scheduler">Scheduler on which to run the generator loop.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the condition), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    public static Seq<TResult> Generate<TState, TResult>(
        TState initialState,
        Func<TState, bool> condition,
        Func<TState, TState> iterate,
        Func<TState, TResult> resultSelector,
        SchedulerRef scheduler,
        [CallerArgumentExpression(nameof(condition))] string text = "") =>
        new GenerateSeq<TState, TResult>(
            initialState, condition, iterate, resultSelector, scheduler, text);

    /// <summary>
    /// Describes <c>Seq.Generate(initialState, condition, iterate, resultSelector,
    /// timeSelector)</c> with a relative time selector.
    /// </summary>
    /// <typeparam name="TState">The type of the state used in the generator loop.</typeparam>
    /// <typeparam name="TResult">The type of the elements in the produced sequence.</typeparam>
    /// <param name="initialState">Initial state.</param>
    /// <param name="condition">Condition to terminate generation (upon returning false).</param>
    /// <param name="iterate">Iteration step function.</param>
    /// <param name="resultSelector">Selector function for results produced in the sequence.</param>
    /// <param name="timeSelector">
    /// Time selector function to control the speed of values being produced each iteration.
    /// </param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the condition), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    public static Seq<TResult> Generate<TState, TResult>(
        TState initialState,
        Func<TState, bool> condition,
        Func<TState, TState> iterate,
        Func<TState, TResult> resultSelector,
        Func<TState, TimeSpan> timeSelector,
        [CallerArgumentExpression(nameof(condition))] string text = "") =>
        new GenerateTimeSeq<TState, TResult>(
            initialState, condition, iterate, resultSelector, timeSelector, null, text);

    /// <summary>
    /// Describes <c>Seq.Generate(initialState, condition, iterate, resultSelector,
    /// timeSelector, scheduler)</c> with a relative time selector.
    /// </summary>
    /// <typeparam name="TState">The type of the state used in the generator loop.</typeparam>
    /// <typeparam name="TResult">The type of the elements in the produced sequence.</typeparam>
    /// <param name="initialState">Initial state.</param>
    /// <param name="condition">Condition to terminate generation (upon returning false).</param>
    /// <param name="iterate">Iteration step function.</param>
    /// <param name="resultSelector">Selector function for results produced in the sequence.</param>
    /// <param name="timeSelector">
    /// Time selector function to control the speed of values being produced each iteration.
    /// </param>
    /// <param name="scheduler">Scheduler on which to run the generator loop.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the condition), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    public static Seq<TResult> Generate<TState, TResult>(
        TState initialState,
        Func<TState, bool> condition,
        Func<TState, TState> iterate,
        Func<TState, TResult> resultSelector,
        Func<TState, TimeSpan> timeSelector,
        SchedulerRef scheduler,
        [CallerArgumentExpression(nameof(condition))] string text = "") =>
        new GenerateTimeSeq<TState, TResult>(
            initialState, condition, iterate, resultSelector, timeSelector, scheduler, text);

    /// <summary>
    /// Describes <c>Seq.Generate(initialState, condition, iterate, resultSelector,
    /// timeSelector)</c> with an absolute time selector.
    /// </summary>
    /// <typeparam name="TState">The type of the state used in the generator loop.</typeparam>
    /// <typeparam name="TResult">The type of the elements in the produced sequence.</typeparam>
    /// <param name="initialState">Initial state.</param>
    /// <param name="condition">Condition to terminate generation (upon returning false).</param>
    /// <param name="iterate">Iteration step function.</param>
    /// <param name="resultSelector">Selector function for results produced in the sequence.</param>
    /// <param name="timeSelector">
    /// Time selector function to control the speed of values being produced each iteration.
    /// </param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the condition), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    public static Seq<TResult> Generate<TState, TResult>(
        TState initialState,
        Func<TState, bool> condition,
        Func<TState, TState> iterate,
        Func<TState, TResult> resultSelector,
        Func<TState, DateTimeOffset> timeSelector,
        [CallerArgumentExpression(nameof(condition))] string text = "") =>
        new GenerateAbsoluteSeq<TState, TResult>(
            initialState, condition, iterate, resultSelector, timeSelector, null, text);

    /// <summary>
    /// Describes <c>Seq.Generate(initialState, condition, iterate, resultSelector,
    /// timeSelector, scheduler)</c> with an absolute time selector.
    /// </summary>
    /// <typeparam name="TState">The type of the state used in the generator loop.</typeparam>
    /// <typeparam name="TResult">The type of the elements in the produced sequence.</typeparam>
    /// <param name="initialState">Initial state.</param>
    /// <param name="condition">Condition to terminate generation (upon returning false).</param>
    /// <param name="iterate">Iteration step function.</param>
    /// <param name="resultSelector">Selector function for results produced in the sequence.</param>
    /// <param name="timeSelector">
    /// Time selector function to control the speed of values being produced each iteration.
    /// </param>
    /// <param name="scheduler">Scheduler on which to run the generator loop.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the condition), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    public static Seq<TResult> Generate<TState, TResult>(
        TState initialState,
        Func<TState, bool> condition,
        Func<TState, TState> iterate,
        Func<TState, TResult> resultSelector,
        Func<TState, DateTimeOffset> timeSelector,
        SchedulerRef scheduler,
        [CallerArgumentExpression(nameof(condition))] string text = "") =>
        new GenerateAbsoluteSeq<TState, TResult>(
            initialState, condition, iterate, resultSelector, timeSelector, scheduler, text);
}

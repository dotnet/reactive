// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;

namespace Tests.System.Reactive.Shared;

public static partial class Seq
{
    /// <summary>Describes <c>Seq.Start(function)</c> over a function, run at the call.</summary>
    /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
    /// <param name="function">Function to run asynchronously.</param>
    /// <remarks>
    /// Builds a <see cref="StartSeq{TResult}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.Start{TResult}(StartSeq{TResult})"/>.
    /// </remarks>
    public static Seq<TResult> Start<TResult>(Func<TResult> function) =>
        new StartSeq<TResult>(function, null);

    /// <summary>
    /// Describes <c>Seq.Start(function, scheduler)</c> over a function, run at the call.
    /// </summary>
    /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
    /// <param name="function">Function to run asynchronously.</param>
    /// <param name="scheduler">Scheduler to run the function on.</param>
    /// <remarks>
    /// Builds a <see cref="StartSeq{TResult}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.Start{TResult}(StartSeq{TResult})"/>.
    /// </remarks>
    public static Seq<TResult> Start<TResult>(Func<TResult> function, SchedulerRef scheduler) =>
        new StartSeq<TResult>(function, scheduler);

    /// <summary>Describes <c>Seq.Start(action)</c> over an action, run at the call.</summary>
    /// <param name="action">Action to run asynchronously.</param>
    /// <remarks>
    /// Builds a <see cref="StartActionSeq"/>, which each target materializes through
    /// <see cref="ISeqVisitor.StartAction(StartActionSeq)"/>.
    /// </remarks>
    public static Seq<Unit> Start(Action action) =>
        new StartActionSeq(action, null);

    /// <summary>
    /// Describes <c>Seq.Start(action, scheduler)</c> over an action, run at the call.
    /// </summary>
    /// <param name="action">Action to run asynchronously.</param>
    /// <param name="scheduler">Scheduler to run the function on.</param>
    /// <remarks>
    /// Builds a <see cref="StartActionSeq"/>, which each target materializes through
    /// <see cref="ISeqVisitor.StartAction(StartActionSeq)"/>.
    /// </remarks>
    public static Seq<Unit> Start(Action action, SchedulerRef scheduler) =>
        new StartActionSeq(action, scheduler);
}

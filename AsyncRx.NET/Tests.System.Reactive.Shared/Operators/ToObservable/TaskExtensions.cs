// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;

namespace Tests.System.Reactive.Shared;

/// <summary>Describes a task as a sequence.</summary>
/// <remarks>
/// Where an Rx.NET test writes <c>task.ToObservable()</c> a shared scenario writes
/// <c>task.ToSeq()</c>, as it does for an enumerable; the bridge turns the result into the
/// target's own conversion.
/// </remarks>
public static class TaskExtensions
{
    /// <summary>Describes <c>task.ToObservable()</c> over a task with a result.</summary>
    /// <typeparam name="TResult">The type of the result produced by the task.</typeparam>
    /// <param name="task">Task to convert to an observable sequence.</param>
    /// <remarks>
    /// Builds a <see cref="TaskSeq{TResult}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.FromTask{TResult}(TaskSeq{TResult})"/>.
    /// </remarks>
    public static Seq<TResult> ToSeq<TResult>(this Task<TResult> task) =>
        new TaskSeq<TResult>(task, null);

    /// <summary>Describes <c>task.ToObservable(scheduler)</c> over a task with a result.</summary>
    /// <typeparam name="TResult">The type of the result produced by the task.</typeparam>
    /// <param name="task">Task to convert to an observable sequence.</param>
    /// <param name="scheduler">Scheduler on which to notify observers about completion.</param>
    /// <remarks>
    /// Builds a <see cref="TaskSeq{TResult}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.FromTask{TResult}(TaskSeq{TResult})"/>.
    /// </remarks>
    public static Seq<TResult> ToSeq<TResult>(this Task<TResult> task, SchedulerRef scheduler) =>
        new TaskSeq<TResult>(task, scheduler);

    /// <summary>Describes <c>task.ToObservable()</c> over a plain task.</summary>
    /// <param name="task">Task to convert to an observable sequence.</param>
    /// <remarks>
    /// Builds a <see cref="VoidTaskSeq"/>, which each target materializes through
    /// <see cref="ISeqVisitor.FromVoidTask(VoidTaskSeq)"/>.
    /// </remarks>
    public static Seq<Unit> ToSeq(this Task task) => new VoidTaskSeq(task, null);

    /// <summary>Describes <c>task.ToObservable(scheduler)</c> over a plain task.</summary>
    /// <param name="task">Task to convert to an observable sequence.</param>
    /// <param name="scheduler">Scheduler on which to notify observers about completion.</param>
    /// <remarks>
    /// Builds a <see cref="VoidTaskSeq"/>, which each target materializes through
    /// <see cref="ISeqVisitor.FromVoidTask(VoidTaskSeq)"/>.
    /// </remarks>
    public static Seq<Unit> ToSeq(this Task task, SchedulerRef scheduler) =>
        new VoidTaskSeq(task, scheduler);
}

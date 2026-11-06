// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

namespace Tests.System.Reactive.Shared.Rx;

/// <summary>A task scheduler that runs every task inline, on the thread that queues it.</summary>
/// <remarks>
/// Rx.NET's test project's <c>TestTaskScheduler</c>, as it stands, named for what it does. Its
/// <c>TaskObservableExtensionsTest</c> runs each virtual-time test inside a task factory over
/// this scheduler, because Rx.NET's conversion of a task without a scheduler queues its
/// continuation to the ambient <see cref="TaskScheduler.Current"/> rather than running it
/// synchronously; under this scheduler the continuation runs on the thread completing the task,
/// which is the scheduled action, so the delivery lands in that tick. <see cref="Subscribing{T}"/>
/// does for one subscription what the original's factory does for the whole test.
/// </remarks>
internal sealed class InlineTaskScheduler : TaskScheduler
{
    private static readonly TaskFactory Factory = new(new InlineTaskScheduler());

    /// <summary>
    /// Wraps <paramref name="source"/> so that each subscription to it is made inside a task on
    /// this scheduler, making it the <see cref="TaskScheduler.Current"/> that the subscription's
    /// continuations are queued to.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    /// <param name="source">The sequence to subscribe to under this scheduler.</param>
    /// <remarks>
    /// The task runs to completion inside <c>StartNew</c>, since queuing executes it inline, so
    /// reading its result does not wait.
    /// </remarks>
    public static IObservable<T> Subscribing<T>(IObservable<T> source) =>
        Observable.Create<T>(observer => Factory.StartNew(() => source.Subscribe(observer)).Result);

    protected override void QueueTask(Task task)
    {
        TryExecuteTaskInline(task, false);
    }

    protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
    {
        return TryExecuteTask(task);
    }

    protected override IEnumerable<Task>? GetScheduledTasks()
    {
        return null;
    }
}

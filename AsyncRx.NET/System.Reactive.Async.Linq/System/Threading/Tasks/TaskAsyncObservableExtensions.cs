// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace System.Threading.Tasks
{
    // TODO: Add ToTask.

    public static class TaskAsyncObservableExtensions
    {
        public static IAsyncObservable<Unit> ToAsyncObservable(this Task task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));

            return AsyncObservable.Create<Unit>(observer => task.AcceptAsync(observer));
        }

        public static IAsyncObservable<Unit> ToAsyncObservable(this Task task, IAsyncScheduler scheduler)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return AsyncObservable.Create<Unit>(observer => task.AcceptAsync(observer, scheduler));
        }

        public static IAsyncObservable<TResult> ToAsyncObservable<TResult>(this Task<TResult> task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));

            return AsyncObservable.Create<TResult>(observer => task.AcceptAsync(observer));
        }

        public static IAsyncObservable<TResult> ToAsyncObservable<TResult>(this Task<TResult> task, IAsyncScheduler scheduler)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return AsyncObservable.Create<TResult>(observer => task.AcceptAsync(observer, scheduler));
        }

        public static Task<IAsyncDisposable> AcceptAsync(this Task task, IAsyncObserver<Unit> observer) => AcceptAsync(task, observer, ImmediateAsyncScheduler.Instance);

        public static Task<IAsyncDisposable> AcceptAsync(this Task task, IAsyncObserver<Unit> observer, IAsyncScheduler scheduler)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            Task<IAsyncDisposable> CompleteAsync()
            {
                return scheduler.ScheduleAsync(async ct =>
                {
                    if (ct.IsCancellationRequested)
                    {
                        return;
                    }

                    switch (task.Status)
                    {
                        case TaskStatus.RanToCompletion:
                            await observer.OnNextAsync(Unit.Default).RendezVous(scheduler, ct);
                            await observer.OnCompletedAsync().RendezVous(scheduler, ct);
                            break;
                        case TaskStatus.Faulted:
                            await observer.OnErrorAsync(task.Exception.InnerException).RendezVous(scheduler, ct);
                            break;
                        case TaskStatus.Canceled:
                            await observer.OnErrorAsync(new TaskCanceledException(task)).RendezVous(scheduler, ct);
                            break;
                    }
                });
            }

            Task<IAsyncDisposable> CoreAsync()
            {
                if (task.IsCompleted)
                {
                    return CompleteAsync();
                }
                else
                {
                    var tco = TaskContinuationOptions.None;

                    if (scheduler == ImmediateAsyncScheduler.Instance)
                    {
                        tco = TaskContinuationOptions.ExecuteSynchronously;
                    }

                    var subject = new SequentialAsyncAsyncSubject<Unit>();

                    task.ContinueWith(t => CompleteAsync(), tco);

                    return subject.SubscribeAsync(observer);
                }
            }

            return CoreAsync();
        }

        public static Task<IAsyncDisposable> AcceptAsync<TResult>(this Task<TResult> task, IAsyncObserver<TResult> observer) => AcceptAsync(task, observer, ImmediateAsyncScheduler.Instance);

        public static Task<IAsyncDisposable> AcceptAsync<TResult>(this Task<TResult> task, IAsyncObserver<TResult> observer, IAsyncScheduler scheduler)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            Task<IAsyncDisposable> CompleteAsync()
            {
                return scheduler.ScheduleAsync(async ct =>
                {
                    if (ct.IsCancellationRequested)
                    {
                        return;
                    }

                    switch (task.Status)
                    {
                        case TaskStatus.RanToCompletion:
                            await observer.OnNextAsync(task.Result).RendezVous(scheduler, ct);
                            await observer.OnCompletedAsync().RendezVous(scheduler, ct);
                            break;
                        case TaskStatus.Faulted:
                            await observer.OnErrorAsync(task.Exception.InnerException).RendezVous(scheduler, ct);
                            break;
                        case TaskStatus.Canceled:
                            await observer.OnErrorAsync(new TaskCanceledException(task)).RendezVous(scheduler, ct);
                            break;
                    }
                });
            }

            Task<IAsyncDisposable> CoreAsync()
            {
                if (task.IsCompleted)
                {
                    return CompleteAsync();
                }
                else
                {
                    var tco = TaskContinuationOptions.None;

                    if (scheduler == ImmediateAsyncScheduler.Instance)
                    {
                        tco = TaskContinuationOptions.ExecuteSynchronously;
                    }

                    var subject = new SequentialAsyncAsyncSubject<TResult>();

                    task.ContinueWith(t => CompleteAsync(), tco);

                    return subject.SubscribeAsync(observer);
                }
            }

            return CoreAsync();
        }
    }
}

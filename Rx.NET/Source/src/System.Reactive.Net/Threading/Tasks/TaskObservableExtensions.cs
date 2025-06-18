// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Linq.ObservableImpl;
using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive.Threading.Tasks
{
    /// <summary>
    /// Provides a set of static methods for converting tasks to observable sequences.
    /// </summary>
    public static class TaskObservableExtensions
    {
        private sealed class SlowTaskObservable : IObservable<Unit>
        {
            private readonly Task _task;
            private readonly IScheduler? _scheduler;
            private readonly bool _ignoreExceptionsAfterUnsubscribe;

            public SlowTaskObservable(Task task, IScheduler? scheduler, bool ignoreExceptionsAfterUnsubscribe)
            {
                _task = task;
                _scheduler = scheduler;
                _ignoreExceptionsAfterUnsubscribe = ignoreExceptionsAfterUnsubscribe;
            }

            public IDisposable Subscribe(IObserver<Unit> observer)
            {
                if (observer == null)
                {
                    throw new ArgumentNullException(nameof(observer));
                }

                var cts = new CancellationDisposable();
                var options = GetTaskContinuationOptions(_scheduler);

                if (_scheduler == null)
                {
                    _task.ContinueWith(static (t, subjectObject) => t.EmitTaskResult((IObserver<Unit>)subjectObject!), observer, cts.Token, options, TaskScheduler.Current);
                }
                else
                {
                    _task.ContinueWithState(
                        static (task, tuple) => tuple.scheduler.ScheduleAction(
                            (task, tuple.observer),
                            static tuple2 => tuple2.task.EmitTaskResult(tuple2.observer)),
                        (scheduler: _scheduler, observer),
                        options,
                        cts.Token);
                }

                if (_ignoreExceptionsAfterUnsubscribe)
                {
                    _task.ContinueWith(t => _ = t.Exception, TaskContinuationOptions.OnlyOnFaulted);
                }

                return cts;
            }
        }

        private sealed class SlowTaskObservable<TResult> : IObservable<TResult>
        {
            private readonly Task<TResult> _task;
            private readonly IScheduler? _scheduler;
            private readonly bool _ignoreExceptionsAfterUnsubscribe;

            public SlowTaskObservable(Task<TResult> task, IScheduler? scheduler, bool ignoreExceptionsAfterUnsubscribe)
            {
                _task = task;
                _scheduler = scheduler;
                _ignoreExceptionsAfterUnsubscribe = ignoreExceptionsAfterUnsubscribe;
            }

            public IDisposable Subscribe(IObserver<TResult> observer)
            {
                if (observer == null)
                {
                    throw new ArgumentNullException(nameof(observer));
                }

                var cts = new CancellationDisposable();
                var options = GetTaskContinuationOptions(_scheduler);

                if (_scheduler == null)
                {
                    _task.ContinueWith(static (t, subjectObject) => t.EmitTaskResult((IObserver<TResult>)subjectObject!), observer, cts.Token, options, TaskScheduler.Current);
                }
                else
                {
                    _task.ContinueWithState(
                        static (task, tuple) => tuple.scheduler.ScheduleAction(
                            (task, tuple.observer),
                            static tuple2 => tuple2.task.EmitTaskResult(tuple2.observer)),
                        (scheduler: _scheduler, observer),
                        options,
                        cts.Token);
                }

                if (_ignoreExceptionsAfterUnsubscribe)
                {
                    _task.ContinueWith(t => _ = t.Exception, TaskContinuationOptions.OnlyOnFaulted);
                }

                return cts;
            }
        }

        /// <summary>
        /// Returns an observable sequence that signals when the task completes.
        /// </summary>
        /// <param name="task">Task to convert to an observable sequence.</param>
        /// <returns>An observable sequence that produces a unit value when the task completes, or propagates the exception produced by the task.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="task"/> is <c>null</c>.</exception>
        /// <remarks>If the specified task object supports cancellation, consider using <see cref="Observable.FromAsync(Func{CancellationToken, Task})"/> instead.</remarks>
        public static IObservable<Unit> ToObservable(this Task task)
        {
            if (task == null)
            {
                throw new ArgumentNullException(nameof(task));
            }

            return ToObservableImpl(task, scheduler: null, ignoreExceptionsAfterUnsubscribe: false);
        }

        /// <summary>
        /// Returns an observable sequence that signals when the task completes.
        /// </summary>
        /// <param name="task">Task to convert to an observable sequence.</param>
        /// <param name="scheduler">Scheduler on which to notify observers about completion, cancellation or failure.</param>
        /// <returns>An observable sequence that produces a unit value when the task completes, or propagates the exception produced by the task.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="task"/> is <c>null</c> or <paramref name="scheduler"/> is <c>null</c>.</exception>
        /// <remarks>If the specified task object supports cancellation, consider using <see cref="Observable.FromAsync(Func{CancellationToken, Task})"/> instead.</remarks>
        public static IObservable<Unit> ToObservable(this Task task, IScheduler scheduler)
        {
            if (scheduler == null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            return ToObservable(task, new TaskObservationOptions(scheduler, ignoreExceptionsAfterUnsubscribe: false));
        }

        /// <summary>
        /// Returns an observable sequence that signals when the task completes.
        /// </summary>
        /// <param name="task">Task to convert to an observable sequence.</param>
        /// <param name="options">Controls how the tasks's progress is observed.</param>
        /// <returns>An observable sequence that produces a unit value when the task completes, or propagates the exception produced by the task.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="task"/> is <c>null</c>.</exception>
        /// <remarks>If the specified task object supports cancellation, consider using <see cref="Observable.FromAsync(Func{CancellationToken, Task})"/> instead.</remarks>
        public static IObservable<Unit> ToObservable(this Task task, TaskObservationOptions options)
        {
            if (task == null)
            {
                throw new ArgumentNullException(nameof(task));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return ToObservableImpl(task, options.Scheduler, options.IgnoreExceptionsAfterUnsubscribe);
        }

        internal static IObservable<Unit> ToObservable(this Task task, TaskObservationOptions.Value options)
        {
            if (task == null)
            {
                throw new ArgumentNullException(nameof(task));
            }

            return ToObservableImpl(task, options.Scheduler, options.IgnoreExceptionsAfterUnsubscribe);
        }


        private static IObservable<Unit> ToObservableImpl(Task task, IScheduler? scheduler, bool ignoreExceptionsAfterUnsubscribe)
        {
            if (task.IsCompleted)
            {
                scheduler ??= ImmediateScheduler.Instance;

                return task.Status switch
                {
                    TaskStatus.Faulted => new Throw<Unit>(task.GetSingleException(), scheduler),
                    TaskStatus.Canceled => new Throw<Unit>(new TaskCanceledException(task), scheduler),
                    _ => new Return<Unit>(Unit.Default, scheduler)
                };
            }

            return new SlowTaskObservable(task, scheduler, ignoreExceptionsAfterUnsubscribe);
        }

        private static void EmitTaskResult(this Task task, IObserver<Unit> subject)
        {
            switch (task.Status)
            {
                case TaskStatus.RanToCompletion:
                    subject.OnNext(Unit.Default);
                    subject.OnCompleted();
                    break;
                case TaskStatus.Faulted:
                    subject.OnError(task.GetSingleException());
                    break;
                case TaskStatus.Canceled:
                    subject.OnError(new TaskCanceledException(task));
                    break;
            }
        }

        internal static IDisposable Subscribe(this Task task, IObserver<Unit> observer)
        {
            if (task.IsCompleted)
            {
                task.EmitTaskResult(observer);
                return Disposable.Empty;
            }

            var cts = new CancellationDisposable();

            task.ContinueWith(
                static (t, observerObject) => t.EmitTaskResult((IObserver<Unit>)observerObject!), 
                observer, 
                cts.Token, 
                TaskContinuationOptions.ExecuteSynchronously, 
                TaskScheduler.Current);

            return cts;
        }

        /// <summary>
        /// Returns an observable sequence that propagates the result of the task.
        /// </summary>
        /// <typeparam name="TResult">The type of the result produced by the task.</typeparam>
        /// <param name="task">Task to convert to an observable sequence.</param>
        /// <returns>An observable sequence that produces the task's result, or propagates the exception produced by the task.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="task"/> is <c>null</c>.</exception>
        /// <remarks>If the specified task object supports cancellation, consider using <see cref="Observable.FromAsync{TResult}(Func{CancellationToken, Task{TResult}})"/> instead.</remarks>
        public static IObservable<TResult> ToObservable<TResult>(this Task<TResult> task)
        {
            if (task == null)
            {
                throw new ArgumentNullException(nameof(task));
            }

            return ToObservableImpl(task, scheduler: null, ignoreExceptionsAfterUnsubscribe: false);
        }

        /// <summary>
        /// Returns an observable sequence that propagates the result of the task.
        /// </summary>
        /// <typeparam name="TResult">The type of the result produced by the task.</typeparam>
        /// <param name="task">Task to convert to an observable sequence.</param>
        /// <param name="scheduler">Scheduler on which to notify observers about completion, cancellation or failure.</param>
        /// <returns>An observable sequence that produces the task's result, or propagates the exception produced by the task.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="task"/> is <c>null</c> or <paramref name="scheduler"/> is <c>null</c>.</exception>
        /// <remarks>If the specified task object supports cancellation, consider using <see cref="Observable.FromAsync{TResult}(Func{CancellationToken, Task{TResult}})"/> instead.</remarks>
        public static IObservable<TResult> ToObservable<TResult>(this Task<TResult> task, IScheduler scheduler)
        {
            if (scheduler == null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            return ToObservable(task, new TaskObservationOptions(scheduler, ignoreExceptionsAfterUnsubscribe: false));
        }

        /// <summary>
        /// Returns an observable sequence that propagates the result of the task.
        /// </summary>
        /// <typeparam name="TResult">The type of the result produced by the task.</typeparam>
        /// <param name="task">Task to convert to an observable sequence.</param>
        /// <param name="options">Controls how the tasks's progress is observed.</param>
        /// <returns>An observable sequence that produces the task's result, or propagates the exception produced by the task.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="task"/> is <c>null</c>.</exception>
        /// <remarks>If the specified task object supports cancellation, consider using <see cref="Observable.FromAsync{TResult}(Func{CancellationToken, Task{TResult}})"/> instead.</remarks>
        public static IObservable<TResult> ToObservable<TResult>(this Task<TResult> task, TaskObservationOptions options)
        {
            if (task == null)
            {
                throw new ArgumentNullException(nameof(task));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return ToObservableImpl(task, options.Scheduler, options.IgnoreExceptionsAfterUnsubscribe);
        }

        internal static IObservable<TResult> ToObservable<TResult>(this Task<TResult> task, TaskObservationOptions.Value options)
        {
            if (task == null)
            {
                throw new ArgumentNullException(nameof(task));
            }

            return ToObservableImpl(task, options.Scheduler, options.IgnoreExceptionsAfterUnsubscribe);
        }

        private static IObservable<TResult> ToObservableImpl<TResult>(Task<TResult> task, IScheduler? scheduler, bool ignoreExceptionsAfterUnsubscribe)
        {
            if (task.IsCompleted)
            {
                scheduler ??= ImmediateScheduler.Instance;

                return task.Status switch
                {
                    TaskStatus.Faulted => new Throw<TResult>(task.GetSingleException(), scheduler),
                    TaskStatus.Canceled => new Throw<TResult>(new TaskCanceledException(task), scheduler),
                    _ => new Return<TResult>(task.Result, scheduler)
                };
            }

            return new SlowTaskObservable<TResult>(task, scheduler, ignoreExceptionsAfterUnsubscribe);
        }

        private static void EmitTaskResult<TResult>(this Task<TResult> task, IObserver<TResult> subject)
        {
            switch (task.Status)
            {
                case TaskStatus.RanToCompletion:
                    subject.OnNext(task.Result);
                    subject.OnCompleted();
                    break;
                case TaskStatus.Faulted:
                    subject.OnError(task.GetSingleException());
                    break;
                case TaskStatus.Canceled:
                    subject.OnError(new TaskCanceledException(task));
                    break;
            }
        }

        private static TaskContinuationOptions GetTaskContinuationOptions(IScheduler? scheduler)
        {
            var options = TaskContinuationOptions.None;

            if (scheduler != null)
            {
                //
                // We explicitly don't special-case the immediate scheduler here. If the user asks for a
                // synchronous completion, we'll try our best. However, there's no guarantee due to the
                // internal stack probing in the TPL, which may cause asynchronous completion on a thread
                // pool thread in order to avoid stack overflows. Therefore we can only attempt to be more
                // efficient in the case where the user specified a scheduler, hence we know that the
                // continuation will trigger a scheduling operation. In case of the immediate scheduler,
                // it really becomes "immediate scheduling" wherever the TPL decided to run the continuation,
                // i.e. not necessarily where the task was completed from.
                //
                options |= TaskContinuationOptions.ExecuteSynchronously;
            }

            return options;
        }

        internal static IDisposable Subscribe<TResult>(this Task<TResult> task, IObserver<TResult> observer)
        {
            if (task.IsCompleted)
            {
                task.EmitTaskResult(observer);
                return Disposable.Empty;
            }

            var cts = new CancellationDisposable();

            task.ContinueWith(
                static (t, observerObject) => t.EmitTaskResult((IObserver<TResult>)observerObject!), 
                observer, 
                cts.Token, 
                TaskContinuationOptions.ExecuteSynchronously, 
                TaskScheduler.Current);

            return cts;
        }

        /// <summary>
        /// Returns a task that will receive the last value or the exception produced by the observable sequence.
        /// </summary>
        /// <typeparam name="TResult">The type of the elements in the source sequence.</typeparam>
        /// <param name="observable">Observable sequence to convert to a task.</param>
        /// <returns>A task that will receive the last element or the exception produced by the observable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="observable"/> is <c>null</c>.</exception>
        public static Task<TResult> ToTask<TResult>(this IObservable<TResult> observable)
        {
            if (observable == null)
            {
                throw new ArgumentNullException(nameof(observable));
            }

            return observable.ToTask(new CancellationToken(), state: null);
        }


        /// <summary>
        /// Returns a task that will receive the last value or the exception produced by the observable sequence.
        /// </summary>
        /// <typeparam name="TResult">The type of the elements in the source sequence.</typeparam>
        /// <param name="observable">Observable sequence to convert to a task.</param>
        /// <param name="scheduler">The scheduler used for overriding where the task completion signals will be issued.</param>
        /// <returns>A task that will receive the last element or the exception produced by the observable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="observable"/> or <paramref name="scheduler"/> is <c>null</c>.</exception>
        public static Task<TResult> ToTask<TResult>(this IObservable<TResult> observable, IScheduler scheduler)
        {
            return observable.ToTask().ContinueOnScheduler(scheduler);
        }

        /// <summary>
        /// Returns a task that will receive the last value or the exception produced by the observable sequence.
        /// </summary>
        /// <typeparam name="TResult">The type of the elements in the source sequence.</typeparam>
        /// <param name="observable">Observable sequence to convert to a task.</param>
        /// <param name="state">The state to use as the underlying task's AsyncState.</param>
        /// <returns>A task that will receive the last element or the exception produced by the observable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="observable"/> is <c>null</c>.</exception>
        public static Task<TResult> ToTask<TResult>(this IObservable<TResult> observable, object? state)
        {
            if (observable == null)
            {
                throw new ArgumentNullException(nameof(observable));
            }

            return observable.ToTask(new CancellationToken(), state);
        }

        /// <summary>
        /// Returns a task that will receive the last value or the exception produced by the observable sequence.
        /// </summary>
        /// <typeparam name="TResult">The type of the elements in the source sequence.</typeparam>
        /// <param name="observable">Observable sequence to convert to a task.</param>
        /// <param name="state">The state to use as the underlying task's AsyncState.</param>
        /// <param name="scheduler">The scheduler used for overriding where the task completion signals will be issued.</param>
        /// <returns>A task that will receive the last element or the exception produced by the observable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="observable"/> or <paramref name="scheduler"/> is <c>null</c>.</exception>
        public static Task<TResult> ToTask<TResult>(this IObservable<TResult> observable, object? state, IScheduler scheduler)
        {
            return observable.ToTask(new CancellationToken(), state).ContinueOnScheduler(scheduler);
        }

        /// <summary>
        /// Returns a task that will receive the last value or the exception produced by the observable sequence.
        /// </summary>
        /// <typeparam name="TResult">The type of the elements in the source sequence.</typeparam>
        /// <param name="observable">Observable sequence to convert to a task.</param>
        /// <param name="cancellationToken">Cancellation token that can be used to cancel the task, causing unsubscription from the observable sequence.</param>
        /// <returns>A task that will receive the last element or the exception produced by the observable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="observable"/> is <c>null</c>.</exception>
        public static Task<TResult> ToTask<TResult>(this IObservable<TResult> observable, CancellationToken cancellationToken)
        {
            if (observable == null)
            {
                throw new ArgumentNullException(nameof(observable));
            }

            return observable.ToTask(cancellationToken, state: null);
        }

        /// <summary>
        /// Returns a task that will receive the last value or the exception produced by the observable sequence.
        /// </summary>
        /// <typeparam name="TResult">The type of the elements in the source sequence.</typeparam>
        /// <param name="observable">Observable sequence to convert to a task.</param>
        /// <param name="cancellationToken">Cancellation token that can be used to cancel the task, causing unsubscription from the observable sequence.</param>
        /// <param name="scheduler">The scheduler used for overriding where the task completion signals will be issued.</param>
        /// <returns>A task that will receive the last element or the exception produced by the observable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="observable"/> or <paramref name="scheduler"/> is <c>null</c>.</exception>
#pragma warning disable CA1068 // (CancellationToken parameters must come last.) Would be a breaking change.
        public static Task<TResult> ToTask<TResult>(this IObservable<TResult> observable, CancellationToken cancellationToken, IScheduler scheduler)
#pragma warning restore CA1068
        {
            return observable.ToTask(cancellationToken, state: null).ContinueOnScheduler(scheduler);
        }

        internal static Task<TResult> ContinueOnScheduler<TResult>(this Task<TResult> task, IScheduler scheduler)
        {
            if (scheduler == null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }
            var tcs = new TaskCompletionSource<TResult>(task.AsyncState);
            task.ContinueWith(
                static (t, o) =>
                {
                    var (scheduler, tcs) = ((IScheduler, TaskCompletionSource<TResult>))o!;

                    scheduler.ScheduleAction((t, tcs), static state =>
                    {
                        if (state.t.IsCanceled)
                        {
                            state.tcs.TrySetCanceled(new TaskCanceledException(state.t).CancellationToken);
                        }
                        else if (state.t.IsFaulted)
                        {
                            state.tcs.TrySetException(state.t.GetSingleException());
                        }
                        else
                        {
                            state.tcs.TrySetResult(state.t.Result);
                        }
                    });
                },
                (scheduler, tcs),
                TaskContinuationOptions.ExecuteSynchronously);
            return tcs.Task;
        }

        private sealed class ToTaskObserver<TResult> : SafeObserver<TResult>
        {
            private readonly CancellationToken _ct;
            private readonly TaskCompletionSource<TResult> _tcs;
            private readonly CancellationTokenRegistration _ctr;

            private bool _hasValue;
            private TResult? _lastValue;

            public ToTaskObserver(TaskCompletionSource<TResult> tcs, CancellationToken ct)
            {
                _ct = ct;
                _tcs = tcs;

                if (ct.CanBeCanceled)
                {
                    _ctr = ct.Register(static @this => ((ToTaskObserver<TResult>)@this!).Cancel(), this);
                }
            }

            public override void OnNext(TResult value)
            {
                _hasValue = true;
                _lastValue = value;
            }

            public override void OnError(Exception error)
            {
                _tcs.TrySetException(error);

                _ctr.Dispose(); // no null-check needed (struct)
                Dispose();
            }

            public override void OnCompleted()
            {
                if (_hasValue)
                {
                    _tcs.TrySetResult(_lastValue!);
                }
                else
                {
                    try
                    {
                        throw new InvalidOperationException(Strings_Linq.NO_ELEMENTS);
                    }
                    catch (Exception e)
                    {
                        _tcs.TrySetException(e);
                    }
                }

                _ctr.Dispose(); // no null-check needed (struct)
                Dispose();
            }

            private void Cancel()
            {
                Dispose();
                _tcs.TrySetCanceled(_ct);
            }
        }

        /// <summary>
        /// Returns a task that will receive the last value or the exception produced by the observable sequence.
        /// </summary>
        /// <typeparam name="TResult">The type of the elements in the source sequence.</typeparam>
        /// <param name="observable">Observable sequence to convert to a task.</param>
        /// <param name="cancellationToken">Cancellation token that can be used to cancel the task, causing unsubscription from the observable sequence.</param>
        /// <param name="state">The state to use as the underlying task's <see cref="Task.AsyncState"/>.</param>
        /// <returns>A task that will receive the last element or the exception produced by the observable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="observable"/> is <c>null</c>.</exception>
#pragma warning disable CA1068 // (CancellationToken parameters must come last.) Would be a breaking change.
        public static Task<TResult> ToTask<TResult>(this IObservable<TResult> observable, CancellationToken cancellationToken, object? state)
#pragma warning restore CA1068
        {
            if (observable == null)
            {
                throw new ArgumentNullException(nameof(observable));
            }

            var tcs = new TaskCompletionSource<TResult>(state);

            var taskCompletionObserver = new ToTaskObserver<TResult>(tcs, cancellationToken);

            //
            // Subtle race condition: if the source completes before we reach the line below, the SingleAssigmentDisposable
            // will already have been disposed. Upon assignment, the disposable resource being set will be disposed on the
            // spot, which may throw an exception.
            //
            try
            {
                //
                // [OK] Use of unsafe Subscribe: we're catching the exception here to set the TaskCompletionSource.
                //
                // Notice we could use a safe subscription to route errors through OnError, but we still need the
                // exception handling logic here for the reason explained above. We cannot afford to throw here
                // and as a result never set the TaskCompletionSource, so we tunnel everything through here.
                //
                taskCompletionObserver.SetResource(observable.Subscribe/*Unsafe*/(taskCompletionObserver));
            }
            catch (Exception ex)
            {
                tcs.TrySetException(ex);
            }

            return tcs.Task;
        }

        /// <summary>
        /// Returns a task that will receive the last value or the exception produced by the observable sequence.
        /// </summary>
        /// <typeparam name="TResult">The type of the elements in the source sequence.</typeparam>
        /// <param name="observable">Observable sequence to convert to a task.</param>
        /// <param name="cancellationToken">Cancellation token that can be used to cancel the task, causing unsubscription from the observable sequence.</param>
        /// <param name="state">The state to use as the underlying task's <see cref="Task.AsyncState"/>.</param>
        /// <param name="scheduler">The scheduler used for overriding where the task completion signals will be issued.</param>
        /// <returns>A task that will receive the last element or the exception produced by the observable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="observable"/> or <paramref name="scheduler"/> is <c>null</c>.</exception>
#pragma warning disable CA1068 // (CancellationToken parameters must come last.) Would be a breaking change.
        public static Task<TResult> ToTask<TResult>(this IObservable<TResult> observable, CancellationToken cancellationToken, object? state, IScheduler scheduler)
#pragma warning restore CA1068
        {
            return observable.ToTask(cancellationToken, state).ContinueOnScheduler(scheduler);
        }
    }
}

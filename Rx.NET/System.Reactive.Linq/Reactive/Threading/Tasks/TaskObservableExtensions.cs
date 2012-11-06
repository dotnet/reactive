// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_TPL
using System.Reactive.Disposables;
using System.Threading.Tasks;
using System.Threading;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace System.Reactive.Threading.Tasks
{
    /// <summary>
    /// Provides a set of static methods for converting tasks to observable sequences.
    /// </summary>
    public static class TaskObservableExtensions
    {
        /// <summary>
        /// Returns an observable sequence that signals when the task completes.
        /// </summary>
        /// <param name="task">Task to convert to an observable sequence.</param>
        /// <returns>An observable sequence that produces a unit value when the task completes, or propagates the exception produced by the task.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="task"/> is null.</exception>
        /// <remarks>If the specified task object supports cancellation, consider using <see cref="Observable.FromAsync(Func{CancellationToken, Task})"/> instead.</remarks>
        public static IObservable<Unit> ToObservable(this Task task)
        {
            if (task == null)
                throw new ArgumentNullException("task");

            var subject = new AsyncSubject<Unit>();

            switch (task.Status)
            {
                case TaskStatus.RanToCompletion:
                    subject.OnNext(Unit.Default);
                    subject.OnCompleted();
                    break;
                case TaskStatus.Faulted:
                    subject.OnError(task.Exception.InnerException);
                    break;
                case TaskStatus.Canceled:
                    subject.OnError(new TaskCanceledException(task));
                    break;
                default:
                    task.ContinueWith(t =>
                    {
                        switch (t.Status)
                        {
                            case TaskStatus.RanToCompletion:
                                subject.OnNext(Unit.Default);
                                subject.OnCompleted();
                                break;
                            case TaskStatus.Faulted:
                                subject.OnError(t.Exception.InnerException);
                                break;
                            case TaskStatus.Canceled:
                                subject.OnError(new TaskCanceledException(t));
                                break;
                        }
                    });
                    break;
            }

            return subject.AsObservable();
        }

        /// <summary>
        /// Returns an observable sequence that propagates the result of the task.
        /// </summary>
        /// <typeparam name="TResult">The type of the result produced by the task.</typeparam>
        /// <param name="task">Task to convert to an observable sequence.</param>
        /// <returns>An observable sequence that produces the task's result, or propagates the exception produced by the task.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="task"/> is null.</exception>
        /// <remarks>If the specified task object supports cancellation, consider using <see cref="Observable.FromAsync{TResult}(Func{CancellationToken, Task{TResult}})"/> instead.</remarks>
        public static IObservable<TResult> ToObservable<TResult>(this Task<TResult> task)
        {
            if (task == null)
                throw new ArgumentNullException("task");

            var subject = new AsyncSubject<TResult>();

            switch (task.Status)
            {
                case TaskStatus.RanToCompletion:
                    subject.OnNext(task.Result);
                    subject.OnCompleted();
                    break;
                case TaskStatus.Faulted:
                    subject.OnError(task.Exception.InnerException);
                    break;
                case TaskStatus.Canceled:
                    subject.OnError(new TaskCanceledException(task));
                    break;
                default:
                    task.ContinueWith(t =>
                    {
                        switch (t.Status)
                        {
                            case TaskStatus.RanToCompletion:
                                subject.OnNext(t.Result);
                                subject.OnCompleted();
                                break;
                            case TaskStatus.Faulted:
                                subject.OnError(t.Exception.InnerException);
                                break;
                            case TaskStatus.Canceled:
                                subject.OnError(new TaskCanceledException(t));
                                break;
                        }
                    });
                    break;
            }

            return subject.AsObservable();
        }

        /// <summary>
        /// Returns a task that will receive the last value or the exception produced by the observable sequence.
        /// </summary>
        /// <typeparam name="TResult">The type of the elements in the source sequence.</typeparam>
        /// <param name="observable">Observable sequence to convert to a task.</param>
        /// <returns>A task that will receive the last element or the exception produced by the observable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="observable"/> is null.</exception>
        public static Task<TResult> ToTask<TResult>(this IObservable<TResult> observable)
        {
            if (observable == null)
                throw new ArgumentNullException("observable");

            return observable.ToTask(new CancellationToken(), null);
        }

        /// <summary>
        /// Returns a task that will receive the last value or the exception produced by the observable sequence.
        /// </summary>
        /// <typeparam name="TResult">The type of the elements in the source sequence.</typeparam>
        /// <param name="observable">Observable sequence to convert to a task.</param>
        /// <param name="state">The state to use as the underlying task's AsyncState.</param>
        /// <returns>A task that will receive the last element or the exception produced by the observable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="observable"/> is null.</exception>
        public static Task<TResult> ToTask<TResult>(this IObservable<TResult> observable, object state)
        {
            if (observable == null)
                throw new ArgumentNullException("observable");

            return observable.ToTask(new CancellationToken(), state);
        }

        /// <summary>
        /// Returns a task that will receive the last value or the exception produced by the observable sequence.
        /// </summary>
        /// <typeparam name="TResult">The type of the elements in the source sequence.</typeparam>
        /// <param name="observable">Observable sequence to convert to a task.</param>
        /// <param name="cancellationToken">Cancellation token that can be used to cancel the task, causing unsubscription from the observable sequence.</param>
        /// <returns>A task that will receive the last element or the exception produced by the observable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="observable"/> is null.</exception>
        public static Task<TResult> ToTask<TResult>(this IObservable<TResult> observable, CancellationToken cancellationToken)
        {
            if (observable == null)
                throw new ArgumentNullException("observable");

            return observable.ToTask(cancellationToken, null);
        }

        /// <summary>
        /// Returns a task that will receive the last value or the exception produced by the observable sequence.
        /// </summary>
        /// <typeparam name="TResult">The type of the elements in the source sequence.</typeparam>
        /// <param name="observable">Observable sequence to convert to a task.</param>
        /// <param name="cancellationToken">Cancellation token that can be used to cancel the task, causing unsubscription from the observable sequence.</param>
        /// <param name="state">The state to use as the underlying task's AsyncState.</param>
        /// <returns>A task that will receive the last element or the exception produced by the observable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="observable"/> is null.</exception>
        public static Task<TResult> ToTask<TResult>(this IObservable<TResult> observable, CancellationToken cancellationToken, object state)
        {
            if (observable == null)
                throw new ArgumentNullException("observable");

            var hasValue = false;
            var lastValue = default(TResult);

            var tcs = new TaskCompletionSource<TResult>(state);

            var disposable = new SingleAssignmentDisposable();

            cancellationToken.Register(() =>
            {
                disposable.Dispose();
                tcs.TrySetCanceled();
            });

            var taskCompletionObserver = new AnonymousObserver<TResult>(
                value =>
                {
                    hasValue = true;
                    lastValue = value;
                },
                ex =>
                {
                    tcs.TrySetException(ex);
                    disposable.Dispose();
                },
                () =>
                {
                    if (hasValue)
                        tcs.TrySetResult(lastValue);
                    else
                        tcs.TrySetException(new InvalidOperationException(Strings_Linq.NO_ELEMENTS));
                    disposable.Dispose();
                }
            );

            //
            // Subtle race condition: if the source completes before we reach the line below, the SingleAssigmentDisposable
            // will already have been disposed. Upon assignment, the disposable resource being set will be disposed on the
            // spot, which may throw an exception. (Similar to TFS 487142)
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
                disposable.Disposable = observable.Subscribe/*Unsafe*/(taskCompletionObserver);
            }
            catch (Exception ex)
            {
                tcs.TrySetException(ex);
            }

            return tcs.Task;
        }
    }
}
#endif

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive.Concurrency
{
    public static class AsyncScheduler
    {
        public static IAwaitable RendezVous(this IAsyncScheduler scheduler)
        {
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return new RendezVousAwaitable(scheduler, CancellationToken.None);
        }

        public static IAwaitable RendezVous(this IAsyncScheduler scheduler, CancellationToken token)
        {
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            token.ThrowIfCancellationRequested();

            return new RendezVousAwaitable(scheduler, token);
        }

        public static IAwaitable RendezVous(this Task task, IAsyncScheduler scheduler) => RendezVous(task, scheduler, CancellationToken.None);

        public static IAwaitable RendezVous(this Task task, IAsyncScheduler scheduler, CancellationToken token)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return new TaskAwaitable(task, false, scheduler, token);
        }

        public static IAwaitable<T> RendezVous<T>(this Task<T> task, IAsyncScheduler scheduler) => RendezVous(task, scheduler, CancellationToken.None);

        public static IAwaitable<T> RendezVous<T>(this Task<T> task, IAsyncScheduler scheduler, CancellationToken token)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return new TaskAwaitable<T>(task, false, scheduler, token);
        }

        public static IAwaitable RendezVous(this ValueTask task, IAsyncScheduler scheduler) => RendezVous(task, scheduler, CancellationToken.None);

        public static IAwaitable RendezVous(this ValueTask task, IAsyncScheduler scheduler, CancellationToken token)
        {
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return new ValueTaskAwaitable(task, false, scheduler, token);
        }

        public static IAwaitable<T> RendezVous<T>(this ValueTask<T> task, IAsyncScheduler scheduler) => RendezVous(task, scheduler, CancellationToken.None);

        public static IAwaitable<T> RendezVous<T>(this ValueTask<T> task, IAsyncScheduler scheduler, CancellationToken token)
        {
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return new ValueTaskAwaitable<T>(task, false, scheduler, token);
        }

        public static async ValueTask Delay(this IAsyncScheduler scheduler, TimeSpan dueTime, CancellationToken token = default)
        {
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            var tcs = new TaskCompletionSource<bool>();

            var task = await scheduler.ScheduleAsync(ct =>
            {
                if (ct.IsCancellationRequested)
                {
                    tcs.TrySetCanceled(ct);
                }
                else
                {
                    tcs.SetResult(true);
                }

                return default;
            }, dueTime);

            using (token.Register(() => task.DisposeAsync()))
            {
                await tcs.Task;
            }
        }

        public static async ValueTask Delay(this IAsyncScheduler scheduler, DateTimeOffset dueTime, CancellationToken token = default)
        {
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            var tcs = new TaskCompletionSource<bool>();

            var task = await scheduler.ScheduleAsync(ct =>
            {
                if (ct.IsCancellationRequested)
                {
                    tcs.TrySetCanceled(ct);
                }
                else
                {
                    tcs.SetResult(true);
                }

                return default;
            }, dueTime);

            using (token.Register(() => task.DisposeAsync()))
            {
                await tcs.Task;
            }
        }

        public static async ValueTask ExecuteAsync(this IAsyncScheduler scheduler, Func<CancellationToken, ValueTask> action, CancellationToken token = default)
        {
            var tcs = new TaskCompletionSource<object>();

            var d = await scheduler.ScheduleAsync(async ct =>
            {
                try
                {
                    ct.ThrowIfCancellationRequested();

                    await action(ct).RendezVous(scheduler, ct);
                }
                catch (OperationCanceledException ex) when (ex.CancellationToken == ct)
                {
                    tcs.TrySetCanceled(ct);
                }
                catch (Exception ex)
                {
                    tcs.TrySetException(ex);
                }
                finally
                {
                    tcs.TrySetResult(null);
                }
            });

            using (token.Register(() =>
            {
                try
                {
                    d.DisposeAsync();
                }
                finally
                {
                    tcs.TrySetCanceled(token);
                }
            }))
            {
                await tcs.Task.ConfigureAwait(false);
            }
        }

        public static async ValueTask<TResult> ExecuteAsync<TResult>(this IAsyncScheduler scheduler, Func<CancellationToken, ValueTask<TResult>> action, CancellationToken token = default)
        {
            var tcs = new TaskCompletionSource<TResult>();

            var d = await scheduler.ScheduleAsync(async ct =>
            {
                var res = default(TResult);

                try
                {
                    ct.ThrowIfCancellationRequested();

                    res = await action(ct).RendezVous(scheduler, ct);
                }
                catch (OperationCanceledException ex) when (ex.CancellationToken == ct)
                {
                    tcs.TrySetCanceled(ct);
                }
                catch (Exception ex)
                {
                    tcs.TrySetException(ex);
                }
                finally
                {
                    tcs.TrySetResult(res);
                }
            });

            using (token.Register(() =>
            {
                try
                {
                    d.DisposeAsync();
                }
                finally
                {
                    tcs.TrySetCanceled(token);
                }
            }))
            {
                return await tcs.Task.ConfigureAwait(false);
            }
        }

        private sealed class RendezVousAwaitable : IAwaitable, IAwaiter // PERF: Can we avoid these allocations?
        {
            private readonly IAsyncScheduler _scheduler;
            private readonly CancellationToken _token;
            private ExceptionDispatchInfo _error;

            public RendezVousAwaitable(IAsyncScheduler scheduler, CancellationToken token)
            {
                _scheduler = scheduler;
                _token = token;
            }

            public bool IsCompleted { get; private set; }

            public IAwaiter GetAwaiter() => this;

            public void GetResult()
            {
                if (!IsCompleted)
                {
                    throw new InvalidOperationException(); // REVIEW: No support for blocking.
                }

                if (_error != null)
                {
                    _error.Throw();
                }
            }

            public void OnCompleted(Action continuation)
            {
                var t = _scheduler.ExecuteAsync(ct =>
                {
                    try
                    {
                        continuation();
                    }
                    catch (Exception ex)
                    {
                        _error = ExceptionDispatchInfo.Capture(ex);
                    }
                    finally
                    {
                        IsCompleted = true;
                    }

                    return default;
                }, _token);
            }
        }
    }
}

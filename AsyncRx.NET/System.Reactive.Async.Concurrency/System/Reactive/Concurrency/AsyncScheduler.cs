// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
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

        public static async Task Delay(this IAsyncScheduler scheduler, TimeSpan dueTime, CancellationToken token = default(CancellationToken))
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

                return Task.CompletedTask;
            }, dueTime);

            using (token.Register(() => task.DisposeAsync()))
            {
                await tcs.Task;
            }
        }

        public static async Task Delay(this IAsyncScheduler scheduler, DateTimeOffset dueTime, CancellationToken token = default(CancellationToken))
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

                return Task.CompletedTask;
            }, dueTime);

            using (token.Register(() => task.DisposeAsync()))
            {
                await tcs.Task;
            }
        }

        public static async Task ExecuteAsync(this IAsyncScheduler scheduler, Func<CancellationToken, Task> action, CancellationToken token = default(CancellationToken))
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

        public static async Task<TResult> ExecuteAsync<TResult>(this IAsyncScheduler scheduler, Func<CancellationToken, Task<TResult>> action, CancellationToken token = default(CancellationToken))
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

            private bool _done;
            private ExceptionDispatchInfo _error;

            public RendezVousAwaitable(IAsyncScheduler scheduler, CancellationToken token)
            {
                _scheduler = scheduler;
                _token = token;
            }

            public bool IsCompleted => _done;

            public IAwaiter GetAwaiter() => this;

            public void GetResult()
            {
                if (!_done)
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
                        _done = true;
                    }

                    return Task.CompletedTask;
                }, _token);
            }
        }
    }
}

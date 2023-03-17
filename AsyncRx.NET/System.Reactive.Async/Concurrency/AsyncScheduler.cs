// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive.Concurrency
{
    public static class AsyncScheduler
    {
        public static RendezVousAwaitable RendezVous(this IAsyncScheduler scheduler, CancellationToken token = default)
        {
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            token.ThrowIfCancellationRequested();

            return new RendezVousAwaitable(scheduler, token);
        }

        public static TaskAwaitable RendezVous(this Task task, IAsyncScheduler scheduler, CancellationToken token = default)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return new TaskAwaitable(task, continueOnCapturedContext: false, scheduler, token);
        }

        public static TaskAwaitable<T> RendezVous<T>(this Task<T> task, IAsyncScheduler scheduler, CancellationToken token = default)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return new TaskAwaitable<T>(task, continueOnCapturedContext: false, scheduler, token);
        }

        public static ValueTaskAwaitable RendezVous(this ValueTask task, IAsyncScheduler scheduler, CancellationToken token = default)
        {
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return new ValueTaskAwaitable(task, continueOnCapturedContext: false, scheduler, token);
        }

        public static ValueTaskAwaitable<T> RendezVous<T>(this ValueTask<T> task, IAsyncScheduler scheduler, CancellationToken token = default)
        {
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return new ValueTaskAwaitable<T>(task, continueOnCapturedContext: false, scheduler, token);
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

        public readonly struct RendezVousAwaitable
        {
            private readonly IAsyncScheduler _scheduler;
            private readonly CancellationToken _token;

            public RendezVousAwaitable(IAsyncScheduler scheduler, CancellationToken token)
            {
                _scheduler = scheduler;
                _token = token;
            }

            public RendezVousAwaiter GetAwaiter() => new(_scheduler, _token);

            public sealed class RendezVousAwaiter : INotifyCompletion
            {
                private readonly IAsyncScheduler _scheduler;
                private readonly CancellationToken _token;
                private ExceptionDispatchInfo _error;

                public RendezVousAwaiter(IAsyncScheduler scheduler, CancellationToken token)
                {
                    _scheduler = scheduler;
                    _token = token;
                }

                public bool IsCompleted { get; private set; }

                public void GetResult()
                {
                    if (!IsCompleted)
                    {
                        throw new InvalidOperationException(); // REVIEW: No support for blocking.
                    }

                    _error?.Throw();
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
}

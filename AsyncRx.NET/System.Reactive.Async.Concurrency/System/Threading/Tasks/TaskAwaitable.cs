// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Runtime.CompilerServices;

namespace System.Threading.Tasks
{
    public sealed class TaskAwaitable : IAwaitable, IAwaiter
    {
        private readonly ConfiguredTaskAwaitable.ConfiguredTaskAwaiter _task;
        private readonly IAsyncScheduler _scheduler;
        private readonly CancellationToken _token;

        public TaskAwaitable(Task task, bool continueOnCapturedContext, IAsyncScheduler scheduler, CancellationToken token)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));

            _task = task.ConfigureAwait(continueOnCapturedContext).GetAwaiter();
            _scheduler = scheduler;
            _token = token;
        }

        public bool IsCompleted => _task.IsCompleted;

        public IAwaiter GetAwaiter() => this;

        public void GetResult()
        {
            _token.ThrowIfCancellationRequested();

            _task.GetResult();
        }

        public void OnCompleted(Action continuation)
        {
            var cancel = default(IDisposable);

            if (_token.CanBeCanceled)
            {
                cancel = _token.Register(() =>
                {
                    Interlocked.Exchange(ref continuation, null)?.Invoke();
                });
            }

            try
            {
                _task.OnCompleted(() =>
                {
                    void Invoke()
                    {
                        cancel?.Dispose();

                        Interlocked.Exchange(ref continuation, null)?.Invoke();
                    }

                    if (_scheduler != null)
                    {
                        var t = _scheduler.ExecuteAsync(ct =>
                        {
                            Invoke();

                            return Task.CompletedTask;
                        }, _token);
                    }
                    else
                    {
                        Invoke();
                    }
                });
            }
            catch
            {
                cancel?.Dispose();
                throw;
            }
        }
    }

    public sealed class TaskAwaitable<T> : IAwaitable<T>, IAwaiter<T>
    {
        private readonly ConfiguredTaskAwaitable<T>.ConfiguredTaskAwaiter _task;
        private readonly IAsyncScheduler _scheduler;
        private readonly CancellationToken _token;

        public TaskAwaitable(Task<T> task, bool continueOnCapturedContext, IAsyncScheduler scheduler, CancellationToken token)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));

            _task = task.ConfigureAwait(continueOnCapturedContext).GetAwaiter();
            _scheduler = scheduler;
            _token = token;
        }

        public bool IsCompleted => _task.IsCompleted;

        public IAwaiter<T> GetAwaiter() => this;

        public T GetResult()
        {
            _token.ThrowIfCancellationRequested();

            return _task.GetResult();
        }

        public void OnCompleted(Action continuation)
        {
            var cancel = default(IDisposable);

            if (_token.CanBeCanceled)
            {
                cancel = _token.Register(() =>
                {
                    Interlocked.Exchange(ref continuation, null)?.Invoke();
                });
            }

            try
            {
                _task.OnCompleted(() =>
                {
                    void Invoke()
                    {
                        cancel?.Dispose();

                        Interlocked.Exchange(ref continuation, null)?.Invoke();
                    }

                    if (_scheduler != null)
                    {
                        var t = _scheduler.ExecuteAsync(ct =>
                        {
                            Invoke();

                            return Task.CompletedTask;
                        }, _token);
                    }
                    else
                    {
                        Invoke();
                    }
                });
            }
            catch
            {
                cancel?.Dispose();
                throw;
            }
        }
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Runtime.CompilerServices;

namespace System.Threading.Tasks
{
    public readonly struct TaskAwaitable
    {
        private readonly Task _task;
        private readonly bool _continueOnCapturedContext;
        private readonly IAsyncScheduler _scheduler;
        private readonly CancellationToken _token;

        public TaskAwaitable(Task task, bool continueOnCapturedContext, IAsyncScheduler scheduler, CancellationToken token)
        {
            _task = task;
            _continueOnCapturedContext = continueOnCapturedContext;
            _scheduler = scheduler;
            _token = token;
        }

        public TaskAwaiter GetAwaiter() => new(_task.ConfigureAwait(_continueOnCapturedContext).GetAwaiter(), _scheduler, _token);

        public readonly struct TaskAwaiter : INotifyCompletion
        {
            private readonly ConfiguredTaskAwaitable.ConfiguredTaskAwaiter _awaiter;
            private readonly IAsyncScheduler _scheduler;
            private readonly CancellationToken _token;

            public TaskAwaiter(ConfiguredTaskAwaitable.ConfiguredTaskAwaiter awaiter, IAsyncScheduler scheduler, CancellationToken token)
            {
                _awaiter = awaiter;
                _scheduler = scheduler;
                _token = token;
            }

            public bool IsCompleted => _awaiter.IsCompleted;

            public void GetResult()
            {
                _token.ThrowIfCancellationRequested();

                _awaiter.GetResult();
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
                    var scheduler = _scheduler;
                    var token = _token;

                    _awaiter.OnCompleted(() =>
                    {
                        void Invoke()
                        {
                            cancel?.Dispose();

                            Interlocked.Exchange(ref continuation, null)?.Invoke();
                        }

                        if (scheduler != null)
                        {
                            var t = scheduler.ExecuteAsync(ct =>
                            {
                                Invoke();

                                return default;
                            }, token);
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

    public readonly struct TaskAwaitable<T>
    {
        private readonly Task<T> _task;
        private readonly bool _continueOnCapturedContext;
        private readonly IAsyncScheduler _scheduler;
        private readonly CancellationToken _token;

        public TaskAwaitable(Task<T> task, bool continueOnCapturedContext, IAsyncScheduler scheduler, CancellationToken token)
        {
            _task = task;
            _continueOnCapturedContext = continueOnCapturedContext;
            _scheduler = scheduler;
            _token = token;
        }

        public TaskAwaiter GetAwaiter() => new(_task.ConfigureAwait(_continueOnCapturedContext).GetAwaiter(), _scheduler, _token);

        public readonly struct TaskAwaiter : INotifyCompletion
        {
            private readonly ConfiguredTaskAwaitable<T>.ConfiguredTaskAwaiter _awaiter;
            private readonly IAsyncScheduler _scheduler;
            private readonly CancellationToken _token;

            public TaskAwaiter(ConfiguredTaskAwaitable<T>.ConfiguredTaskAwaiter awaiter, IAsyncScheduler scheduler, CancellationToken token)
            {
                _awaiter = awaiter;
                _scheduler = scheduler;
                _token = token;
            }

            public bool IsCompleted => _awaiter.IsCompleted;

            public T GetResult()
            {
                _token.ThrowIfCancellationRequested();

                return _awaiter.GetResult();
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
                    var scheduler = _scheduler;
                    var token = _token;

                    _awaiter.OnCompleted(() =>
                    {
                        void Invoke()
                        {
                            cancel?.Dispose();

                            Interlocked.Exchange(ref continuation, null)?.Invoke();
                        }

                        if (scheduler != null)
                        {
                            var t = scheduler.ExecuteAsync(ct =>
                            {
                                Invoke();

                                return default;
                            }, token);
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
}

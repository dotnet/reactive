// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Runtime.CompilerServices;

namespace System.Threading.Tasks
{
    public readonly struct ValueTaskAwaitable
    {
        private readonly ValueTask _task;
        private readonly bool _continueOnCapturedContext;
        private readonly IAsyncScheduler _scheduler;
        private readonly CancellationToken _token;

        public ValueTaskAwaitable(ValueTask task, bool continueOnCapturedContext, IAsyncScheduler scheduler, CancellationToken token)
        {
            _task = task;
            _continueOnCapturedContext = continueOnCapturedContext;
            _scheduler = scheduler;
            _token = token;
        }

        public ValueTaskAwaiter GetAwaiter() => new(_task.ConfigureAwait(_continueOnCapturedContext).GetAwaiter(), _scheduler, _token);

        public readonly struct ValueTaskAwaiter : INotifyCompletion
        {
            private readonly ConfiguredValueTaskAwaitable.ConfiguredValueTaskAwaiter _awaiter;
            private readonly IAsyncScheduler _scheduler;
            private readonly CancellationToken _token;

            public ValueTaskAwaiter(ConfiguredValueTaskAwaitable.ConfiguredValueTaskAwaiter awaiter, IAsyncScheduler scheduler, CancellationToken token)
            {
                _awaiter = awaiter;
                _scheduler = scheduler;
                _token = token;
            }

            public bool IsCompleted => _awaiter.IsCompleted;

            public void GetResult()
            {
                // A fault in the awaited work wins over a cancellation: disposing the
                // subscription in response to an observer's exception (as an auto-detaching
                // observer does) cancels this token, and checking the token first would report
                // that exception as a cancellation. A cancellation ends work that completed.
                // The continuation also runs when the token is cancelled while the work is
                // still pending (see ScheduledContinuation); then there is no result to read.
                if (!_awaiter.IsCompleted)
                {
                    _token.ThrowIfCancellationRequested();

                    throw new InvalidOperationException();
                }

                _awaiter.GetResult();

                _token.ThrowIfCancellationRequested();
            }

            public void OnCompleted(Action continuation)
            {
                var c = ScheduledContinuation.Create(continuation, _scheduler, _token);

                try
                {
                    _awaiter.OnCompleted(c.Run);
                }
                catch
                {
                    c.DisposeCancellation();
                    throw;
                }
            }
        }
    }

    public readonly struct ValueTaskAwaitable<T>
    {
        private readonly ValueTask<T> _task;
        private readonly bool _continueOnCapturedContext;
        private readonly IAsyncScheduler _scheduler;
        private readonly CancellationToken _token;

        public ValueTaskAwaitable(ValueTask<T> task, bool continueOnCapturedContext, IAsyncScheduler scheduler, CancellationToken token)
        {
            _task = task;
            _continueOnCapturedContext = continueOnCapturedContext;
            _scheduler = scheduler;
            _token = token;
        }

        public ValueTaskAwaiter GetAwaiter() => new(_task.ConfigureAwait(_continueOnCapturedContext).GetAwaiter(), _scheduler, _token);

        public readonly struct ValueTaskAwaiter : INotifyCompletion
        {
            private readonly ConfiguredValueTaskAwaitable<T>.ConfiguredValueTaskAwaiter _awaiter;
            private readonly IAsyncScheduler _scheduler;
            private readonly CancellationToken _token;

            public ValueTaskAwaiter(ConfiguredValueTaskAwaitable<T>.ConfiguredValueTaskAwaiter awaiter, IAsyncScheduler scheduler, CancellationToken token)
            {
                _awaiter = awaiter;
                _scheduler = scheduler;
                _token = token;
            }

            public bool IsCompleted => _awaiter.IsCompleted;

            public T GetResult()
            {
                // A fault in the awaited work wins over a cancellation: disposing the
                // subscription in response to an observer's exception (as an auto-detaching
                // observer does) cancels this token, and checking the token first would report
                // that exception as a cancellation. A cancellation ends work that completed.
                // The continuation also runs when the token is cancelled while the work is
                // still pending (see ScheduledContinuation); then there is no result to read.
                if (!_awaiter.IsCompleted)
                {
                    _token.ThrowIfCancellationRequested();

                    throw new InvalidOperationException();
                }

                var result = _awaiter.GetResult();

                _token.ThrowIfCancellationRequested();

                return result;
            }

            public void OnCompleted(Action continuation)
            {
                var c = ScheduledContinuation.Create(continuation, _scheduler, _token);

                try
                {
                    _awaiter.OnCompleted(c.Run);
                }
                catch
                {
                    c.DisposeCancellation();
                    throw;
                }
            }
        }
    }
}

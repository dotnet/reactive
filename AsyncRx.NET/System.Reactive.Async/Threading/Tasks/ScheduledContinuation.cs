// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;

namespace System.Threading.Tasks
{
    /// <summary>
    /// The continuation of a rendez-vous awaiter: runs the awaiting state machine's continuation
    /// on the scheduler (through <see cref="AsyncScheduler.ExecuteAsync{TState}(IAsyncScheduler, TState, Func{TState, CancellationToken, ValueTask}, CancellationToken)"/>),
    /// or inline when there is no scheduler, and at most once even if the token is cancelled first.
    /// </summary>
    /// <remarks>
    /// This object is the only per-await allocation the awaiters make on the suspending path; it is
    /// its own state for every callback it registers, so no closures are created.
    /// </remarks>
    internal sealed class ScheduledContinuation
    {
        private readonly IAsyncScheduler _scheduler;
        private readonly CancellationToken _token;
        private Action _continuation;
        private IDisposable _cancel;

        private ScheduledContinuation(Action continuation, IAsyncScheduler scheduler, CancellationToken token)
        {
            _continuation = continuation;
            _scheduler = scheduler;
            _token = token;
        }

        public static ScheduledContinuation Create(Action continuation, IAsyncScheduler scheduler, CancellationToken token)
        {
            var c = new ScheduledContinuation(continuation, scheduler, token);

            if (token.CanBeCanceled)
            {
                c._cancel = token.Register(static s => ((ScheduledContinuation)s).InvokeCore(), c);
            }

            return c;
        }

        /// <summary>
        /// Called when the awaited operation completes. Pass as the awaiter's continuation.
        /// </summary>
        public void Run()
        {
            if (_scheduler != null)
            {
                _ = _scheduler.ExecuteAsync(this, static (c, ct) =>
                {
                    c.Invoke();

                    return default;
                }, _token);
            }
            else
            {
                Invoke();
            }
        }

        public void DisposeCancellation() => _cancel?.Dispose();

        private void Invoke()
        {
            DisposeCancellation();

            InvokeCore();
        }

        private void InvokeCore() => Interlocked.Exchange(ref _continuation, null)?.Invoke();
    }
}

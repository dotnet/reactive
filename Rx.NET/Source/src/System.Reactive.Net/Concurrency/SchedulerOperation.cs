﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;

namespace System.Reactive.Concurrency
{
    /// <summary>
    /// Represents an awaitable scheduler operation. Awaiting the object causes the continuation to be posted back to the originating scheduler's work queue.
    /// </summary>
    public sealed class SchedulerOperation
    {
        private readonly Func<Action, IDisposable> _schedule;
        private readonly CancellationToken _cancellationToken;
        private readonly bool _postBackToOriginalContext;

        internal SchedulerOperation(Func<Action, IDisposable> schedule, CancellationToken cancellationToken)
            : this(schedule, false, cancellationToken)
        {
        }

        internal SchedulerOperation(Func<Action, IDisposable> schedule, bool postBackToOriginalContext, CancellationToken cancellationToken)
        {
            _schedule = schedule;
            _cancellationToken = cancellationToken;
            _postBackToOriginalContext = postBackToOriginalContext;
        }

        /// <summary>
        /// Controls whether the continuation is run on the originating synchronization context (false by default).
        /// </summary>
        /// <param name="continueOnCapturedContext">true to run the continuation on the captured synchronization context; false otherwise (default).</param>
        /// <returns>Scheduler operation object with configured await behavior.</returns>
        public SchedulerOperation ConfigureAwait(bool continueOnCapturedContext)
        {
            return new SchedulerOperation(_schedule, continueOnCapturedContext, _cancellationToken);
        }

        /// <summary>
        /// Gets an awaiter for the scheduler operation, used to post back the continuation.
        /// </summary>
        /// <returns>Awaiter for the scheduler operation.</returns>
        public SchedulerOperationAwaiter GetAwaiter()
        {
            return new SchedulerOperationAwaiter(_schedule, _postBackToOriginalContext, _cancellationToken);
        }
    }

    /// <summary>
    /// (Infrastructure) Scheduler operation awaiter type used by the code generated for C# await and Visual Basic Await expressions.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public sealed class SchedulerOperationAwaiter : INotifyCompletion
    {
        private readonly Func<Action, IDisposable> _schedule;
        private readonly CancellationToken _cancellationToken;
        private readonly bool _postBackToOriginalContext;
        private readonly CancellationTokenRegistration _ctr;

        internal SchedulerOperationAwaiter(Func<Action, IDisposable> schedule, bool postBackToOriginalContext, CancellationToken cancellationToken)
        {
            _schedule = schedule;
            _cancellationToken = cancellationToken;
            _postBackToOriginalContext = postBackToOriginalContext;

            if (cancellationToken.CanBeCanceled)
            {
                _ctr = _cancellationToken.Register(static @this => ((SchedulerOperationAwaiter)@this!).Cancel(), this);
            }
        }

        /// <summary>
        /// Indicates whether the scheduler operation has completed. Returns false unless cancellation was already requested.
        /// </summary>
        public bool IsCompleted => _cancellationToken.IsCancellationRequested;

        /// <summary>
        /// Completes the scheduler operation, throwing an OperationCanceledException in case cancellation was requested.
        /// </summary>
        public void GetResult() => _cancellationToken.ThrowIfCancellationRequested();

        /// <summary>
        /// Registers the continuation with the scheduler operation.
        /// </summary>
        /// <param name="continuation">Continuation to be run on the originating scheduler.</param>
        public void OnCompleted(Action continuation)
        {
            if (continuation == null)
            {
                throw new ArgumentNullException(nameof(continuation));
            }

            if (_continuation != null)
            {
                throw new InvalidOperationException(Strings_Core.SCHEDULER_OPERATION_ALREADY_AWAITED);
            }

            if (_postBackToOriginalContext)
            {
                var ctx = SynchronizationContext.Current;
                if (ctx != null)
                {
                    var original = continuation;
                    continuation = () =>
                    {
                        //
                        // No need for OperationStarted and OperationCompleted calls here;
                        // this code is invoked through await support and will have a way
                        // to observe its start/complete behavior, either through returned
                        // Task objects or the async method builder's interaction with the
                        // SynchronizationContext object.
                        //
                        // In general though, Rx doesn't play nicely with synchronization
                        // contexts objects at the scheduler level. It's possible to start
                        // async operations by calling Schedule, without a way to observe
                        // their completion. Not interacting with SynchronizationContext
                        // is a conscious design decision as the performance impact was non
                        // negligible and our schedulers abstract over more constructs.
                        //
                        ctx.Post(static a => ((Action)a!)(), original);
                    };
                }
            }

            var ran = 0;

            _continuation = () =>
            {
                if (Interlocked.Exchange(ref ran, 1) == 0)
                {
                    _ctr.Dispose(); // no null-check needed (struct)
                    continuation();
                }
            };

            _work = _schedule(_continuation);
        }

        private volatile Action? _continuation;
        private volatile IDisposable? _work;

        private void Cancel()
        {
            _work?.Dispose();
            _continuation?.Invoke();
        }
    }
}

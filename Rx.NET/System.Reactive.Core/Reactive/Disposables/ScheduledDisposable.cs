// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System.Reactive.Concurrency;
using System.Threading;

namespace System.Reactive.Disposables
{
    /// <summary>
    /// Represents a disposable resource whose disposal invocation will be scheduled on the specified <seealso cref="T:System.Reactive.Concurrency.IScheduler"/>.
    /// </summary>
    public sealed class ScheduledDisposable : ICancelable
    {
        private readonly IScheduler _scheduler;
        private volatile IDisposable _disposable;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Reactive.Disposables.ScheduledDisposable"/> class that uses an <see cref="T:System.Reactive.Concurrency.IScheduler"/> on which to dispose the disposable.
        /// </summary>
        /// <param name="scheduler">Scheduler where the disposable resource will be disposed on.</param>
        /// <param name="disposable">Disposable resource to dispose on the given scheduler.</param>
        /// <exception cref="ArgumentNullException"><paramref name="scheduler"/> or <paramref name="disposable"/> is null.</exception>
        public ScheduledDisposable(IScheduler scheduler, IDisposable disposable)
        {
            if (scheduler == null)
                throw new ArgumentNullException("scheduler");
            if (disposable == null)
                throw new ArgumentNullException("disposable");

            _scheduler = scheduler;
            _disposable = disposable;
        }

        /// <summary>
        /// Gets the scheduler where the disposable resource will be disposed on.
        /// </summary>
        public IScheduler Scheduler
        {
            get { return _scheduler; }
        }

        /// <summary>
        /// Gets the underlying disposable. After disposal, the result is undefined.
        /// </summary>
        public IDisposable Disposable
        {
            get
            {
                var current = _disposable;

                if (current == BooleanDisposable.True)
                    return DefaultDisposable.Instance; // Don't leak the sentinel value.

                return current;
            }
        }

        /// <summary>
        /// Gets a value that indicates whether the object is disposed.
        /// </summary>
        public bool IsDisposed
        {
            get { return _disposable == BooleanDisposable.True; }
        }

        /// <summary>
        /// Disposes the wrapped disposable on the provided scheduler.
        /// </summary>
        public void Dispose()
        {
            Scheduler.Schedule(DisposeInner);
        }

        private void DisposeInner()
        {
#pragma warning disable 0420
            var disposable = Interlocked.Exchange(ref _disposable, BooleanDisposable.True);
#pragma warning restore 0420

            if (disposable != BooleanDisposable.True)
            {
                disposable.Dispose();
            }
        }
    }
}

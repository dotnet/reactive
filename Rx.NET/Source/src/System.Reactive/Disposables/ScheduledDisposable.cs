// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Threading;

namespace System.Reactive.Disposables
{
    /// <summary>
    /// Represents a disposable resource whose disposal invocation will be scheduled on the specified <seealso cref="IScheduler"/>.
    /// </summary>
    public sealed class ScheduledDisposable : ICancelable
    {
        private volatile IDisposable _disposable;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScheduledDisposable"/> class that uses an <see cref="IScheduler"/> on which to dispose the disposable.
        /// </summary>
        /// <param name="scheduler">Scheduler where the disposable resource will be disposed on.</param>
        /// <param name="disposable">Disposable resource to dispose on the given scheduler.</param>
        /// <exception cref="ArgumentNullException"><paramref name="scheduler"/> or <paramref name="disposable"/> is null.</exception>
        public ScheduledDisposable(IScheduler scheduler, IDisposable disposable)
        {
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));
            if (disposable == null)
                throw new ArgumentNullException(nameof(disposable));

            Scheduler = scheduler;
            _disposable = disposable;
        }

        /// <summary>
        /// Gets the scheduler where the disposable resource will be disposed on.
        /// </summary>
        public IScheduler Scheduler { get; }

        /// <summary>
        /// Gets the underlying disposable. After disposal, the result is undefined.
        /// </summary>
        public IDisposable Disposable
        {
            get
            {
                var current = _disposable;

                if (current == BooleanDisposable.True)
                {
                    return DefaultDisposable.Instance; // Don't leak the sentinel value.
                }

                return current;
            }
        }

        /// <summary>
        /// Gets a value that indicates whether the object is disposed.
        /// </summary>
        public bool IsDisposed => _disposable == BooleanDisposable.True;

        /// <summary>
        /// Disposes the wrapped disposable on the provided scheduler.
        /// </summary>
        public void Dispose() => Scheduler.Schedule(DisposeInner);

        private void DisposeInner()
        {
            var disposable = Interlocked.Exchange(ref _disposable, BooleanDisposable.True);

            if (disposable != BooleanDisposable.True)
            {
                disposable.Dispose();
            }
        }
    }
}

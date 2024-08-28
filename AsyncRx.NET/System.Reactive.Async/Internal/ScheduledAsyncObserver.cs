// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive
{
    internal sealed class ScheduledAsyncObserver<T> : ScheduledAsyncObserverBase<T>
    {
        private readonly IAsyncScheduler _scheduler;

        private readonly SerialAsyncDisposable _disposable = new();

        public ScheduledAsyncObserver(IAsyncObserver<T> observer, IAsyncScheduler scheduler)
            : base(observer)
        {
            _scheduler = scheduler;
        }

        public override ValueTask DisposeAsync() => _disposable.DisposeAsync();

        protected override ValueTaskAwaitable RendezVous(ValueTask task) => new(task, continueOnCapturedContext: false, _scheduler, CancellationToken.None);

        protected override ValueTaskAwaitable<R> RendezVous<R>(ValueTask<R> task) => new(task, continueOnCapturedContext: false, _scheduler, CancellationToken.None);

        protected override async ValueTask ScheduleAsync()
        {
            var d = await _scheduler.ScheduleAsync(RunAsync).ConfigureAwait(false);
            await _disposable.AssignAsync(d).ConfigureAwait(false);
        }
    }
}

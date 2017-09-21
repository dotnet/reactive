// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace System.Reactive
{
    internal sealed class ScheduledAsyncObserver<T> : ScheduledAsyncObserverBase<T>
    {
        private readonly IAsyncObserver<T> _observer;
        private readonly IAsyncScheduler _scheduler;

        private readonly SerialAsyncDisposable _disposable = new SerialAsyncDisposable();

        public ScheduledAsyncObserver(IAsyncObserver<T> observer, IAsyncScheduler scheduler)
            : base(observer)
        {
            _scheduler = scheduler;
        }

        public override Task DisposeAsync() => _disposable.DisposeAsync();

        // TODO: Implement proper RendezVous semantics.

        protected override ConfiguredTaskAwaitable RendezVous(Task task) => task.ConfigureAwait(false);

        protected override ConfiguredTaskAwaitable<R> RendezVous<R>(Task<R> task) => task.ConfigureAwait(false);

        protected override async Task ScheduleAsync()
        {
            var d = await _scheduler.ScheduleAsync(RunAsync).ConfigureAwait(false);
            await _disposable.AssignAsync(d).ConfigureAwait(false);
        }
    }
}

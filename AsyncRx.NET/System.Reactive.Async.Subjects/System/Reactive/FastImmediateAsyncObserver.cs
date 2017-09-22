// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Disposables;
using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive
{
    internal sealed class FastImmediateAsyncObserver<T> : ScheduledAsyncObserverBase<T>
    {
        private readonly CancellationAsyncDisposable _disposable = new CancellationAsyncDisposable();

        public FastImmediateAsyncObserver(IAsyncObserver<T> observer)
            : base(observer)
        {
        }

        public override Task DisposeAsync() => _disposable.DisposeAsync();

        protected override IAwaitable RendezVous(Task task) => new TaskAwaitable(task, false, null, CancellationToken.None);

        protected override IAwaitable<R> RendezVous<R>(Task<R> task) => new TaskAwaitable<R>(task, false, null, CancellationToken.None);

        protected override Task ScheduleAsync() => RunAsync(_disposable.Token);
    }
}

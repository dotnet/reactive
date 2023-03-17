// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Disposables;
using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive
{
    internal sealed class FastImmediateAsyncObserver<T> : ScheduledAsyncObserverBase<T>
    {
        private readonly CancellationAsyncDisposable _disposable = new();

        public FastImmediateAsyncObserver(IAsyncObserver<T> observer)
            : base(observer)
        {
        }

        public override ValueTask DisposeAsync() => _disposable.DisposeAsync();

        protected override ValueTaskAwaitable RendezVous(ValueTask task) => new(task, continueOnCapturedContext: false, scheduler: null, CancellationToken.None);

        protected override ValueTaskAwaitable<R> RendezVous<R>(ValueTask<R> task) => new(task, continueOnCapturedContext: false, scheduler: null, CancellationToken.None);

        protected override ValueTask ScheduleAsync() => RunAsync(_disposable.Token);
    }
}

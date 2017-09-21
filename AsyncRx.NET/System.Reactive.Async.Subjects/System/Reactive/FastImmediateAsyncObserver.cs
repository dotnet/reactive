// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Disposables;
using System.Runtime.CompilerServices;
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

        protected override ConfiguredTaskAwaitable RendezVous(Task task) => task.ConfigureAwait(false);

        protected override ConfiguredTaskAwaitable<R> RendezVous<R>(Task<R> task) => task.ConfigureAwait(false);

        protected override Task ScheduleAsync() => RunAsync(_disposable.Token);
    }
}

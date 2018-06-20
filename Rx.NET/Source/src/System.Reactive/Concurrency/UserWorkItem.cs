// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Disposables;

namespace System.Reactive.Concurrency
{
    internal sealed class UserWorkItem<TState> : IDisposable
    {
        private IDisposable _cancelRunDisposable;
        private IDisposable _cancelQueueDisposable;

        private readonly TState _state;
        private readonly IScheduler _scheduler;
        private readonly Func<IScheduler, TState, IDisposable> _action;

        public UserWorkItem(IScheduler scheduler, TState state, Func<IScheduler, TState, IDisposable> action)
        {
            _state = state;
            _action = action;
            _scheduler = scheduler;
        }

        public void Run()
        {
            if (!Disposable.GetIsDisposed(ref _cancelRunDisposable))
            {
                Disposable.SetSingle(ref _cancelRunDisposable, _action(_scheduler, _state));
            }
        }

        public IDisposable CancelQueueDisposable
        {
            get => Disposable.GetValue(ref _cancelQueueDisposable);
            set => Disposable.SetSingle(ref _cancelQueueDisposable, value);
        }

        public void Dispose()
        {
            Disposable.TryDispose(ref _cancelQueueDisposable);
            Disposable.TryDispose(ref _cancelRunDisposable);
        }
    }
}
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive.Disposables
{
    public static class AsyncDisposable
    {
        public static IAsyncDisposable Nop { get; } = new NopAsyncDisposable();

        public static IAsyncDisposable Create(Func<Task> dispose)
        {
            if (dispose == null)
                throw new ArgumentNullException(nameof(dispose));

            return new AnonymousAsyncDisposable(dispose);
        }

        private sealed class AnonymousAsyncDisposable : IAsyncDisposable
        {
            private Func<Task> _dispose;

            public AnonymousAsyncDisposable(Func<Task> dispose)
            {
                _dispose = dispose;
            }

            public Task DisposeAsync() => Interlocked.Exchange(ref _dispose, null)?.Invoke() ?? Task.CompletedTask;
        }

        private sealed class NopAsyncDisposable : IAsyncDisposable
        {
            public Task DisposeAsync() => Task.CompletedTask;
        }
    }
}

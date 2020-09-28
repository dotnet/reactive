// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive.Disposables
{
    public static class AsyncDisposable
    {
        public static IAsyncDisposable Nop { get; } = new NopAsyncDisposable();

        public static IAsyncDisposable Create(Func<ValueTask> dispose)
        {
            if (dispose == null)
                throw new ArgumentNullException(nameof(dispose));

            return new AnonymousAsyncDisposable(dispose);
        }

        private sealed class AnonymousAsyncDisposable : IAsyncDisposable
        {
            private Func<ValueTask> _dispose;

            public AnonymousAsyncDisposable(Func<ValueTask> dispose) => _dispose = dispose;

            public ValueTask DisposeAsync() => Interlocked.Exchange(ref _dispose, null)?.Invoke() ?? default;
        }

        private sealed class NopAsyncDisposable : IAsyncDisposable
        {
            public ValueTask DisposeAsync() => default;
        }
    }
}

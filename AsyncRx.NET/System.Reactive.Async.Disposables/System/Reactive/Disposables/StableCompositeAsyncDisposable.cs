// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive.Disposables
{
    public abstract class StableCompositeAsyncDisposable : IAsyncDisposable
    {
        public static StableCompositeAsyncDisposable Create(IAsyncDisposable disposable1, IAsyncDisposable disposable2)
        {
            if (disposable1 == null)
                throw new ArgumentNullException(nameof(disposable1));
            if (disposable2 == null)
                throw new ArgumentNullException(nameof(disposable2));

            return new Binary(disposable1, disposable2);
        }

        public static StableCompositeAsyncDisposable Create(params IAsyncDisposable[] disposables)
        {
            if (disposables == null)
                throw new ArgumentNullException(nameof(disposables));

            return new NAry(disposables);
        }

        public static StableCompositeAsyncDisposable Create(IEnumerable<IAsyncDisposable> disposables)
        {
            if (disposables == null)
                throw new ArgumentNullException(nameof(disposables));

            return new NAry(disposables);
        }

        public abstract Task DisposeAsync();

        private sealed class Binary : StableCompositeAsyncDisposable
        {
            private volatile IAsyncDisposable _disposable1;
            private volatile IAsyncDisposable _disposable2;

            public Binary(IAsyncDisposable disposable1, IAsyncDisposable disposable2)
            {
                _disposable1 = disposable1;
                _disposable2 = disposable2;
            }

            public override async Task DisposeAsync()
            {
                var d1 = Interlocked.Exchange(ref _disposable1, null);
                if (d1 != null)
                {
                    await d1.DisposeAsync().ConfigureAwait(false);
                }

                var d2 = Interlocked.Exchange(ref _disposable2, null);
                if (d2 != null)
                {
                    await d2.DisposeAsync().ConfigureAwait(false);
                }
            }
        }

        private sealed class NAry : StableCompositeAsyncDisposable
        {
            private volatile List<IAsyncDisposable> _disposables;

            public NAry(IAsyncDisposable[] disposables)
                : this((IEnumerable<IAsyncDisposable>)disposables)
            {
            }

            public NAry(IEnumerable<IAsyncDisposable> disposables)
            {
                _disposables = new List<IAsyncDisposable>(disposables);
            }

            public override async Task DisposeAsync()
            {
                var old = Interlocked.Exchange(ref _disposables, null);
                if (old != null)
                {
                    foreach (var d in old)
                    {
                        if (d != null)
                        {
                            await d.DisposeAsync().ConfigureAwait(false);
                        }
                    }
                }
            }
        }
    }
}
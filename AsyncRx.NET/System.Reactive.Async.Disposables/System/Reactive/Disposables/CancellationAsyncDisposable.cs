// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive.Disposables
{
    public sealed class CancellationAsyncDisposable : IAsyncDisposable
    {
        private readonly CancellationTokenSource _cts;

        public CancellationAsyncDisposable()
            : this(new CancellationTokenSource())
        {
        }

        public CancellationAsyncDisposable(CancellationTokenSource cts)
        {
            _cts = cts ?? throw new ArgumentNullException(nameof(cts));
        }

        public CancellationToken Token => _cts.Token;

        public ValueTask DisposeAsync()
        {
            _cts.Cancel();

            return default;
        }
    }
}

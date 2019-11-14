// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Threading;

namespace System.Linq
{
    internal sealed class CancellationTokenDisposable : IDisposable
    {
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        public CancellationToken Token => _cts.Token;

        public void Dispose()
        {
            if (!_cts.IsCancellationRequested)
            {
                _cts.Cancel();
            }
        }
    }

    internal static class Disposable
    {
        public static IDisposable Create(IDisposable d1, IDisposable d2) => new BinaryDisposable(d1, d2);

        public static IDisposable Create(Action action) => new AnonymousDisposable(action);
    }

    internal sealed class BinaryDisposable : IDisposable
    {
        private IDisposable? _d1;
        private IDisposable? _d2;

        public BinaryDisposable(IDisposable d1, IDisposable d2)
        {
            _d1 = d1;
            _d2 = d2;
        }

        public void Dispose()
        {
            var d1 = Interlocked.Exchange(ref _d1, null);
            if (d1 != null)
            {
                d1.Dispose();

                var d2 = Interlocked.Exchange(ref _d2, null);
                if (d2 != null)
                {
                    d2.Dispose();
                }
            }
        }
    }

    internal sealed class AnonymousDisposable : IDisposable
    {
        private Action? _action;

        public AnonymousDisposable(Action action)
        {
            _action = action;
        }

        public void Dispose() => Interlocked.Exchange(ref _action, null)?.Invoke();
    }
}

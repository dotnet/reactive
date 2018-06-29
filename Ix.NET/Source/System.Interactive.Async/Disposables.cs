// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 
using System.Threading;

namespace System.Linq
{
    internal class CancellationTokenDisposable : IDisposable
    {
        private readonly CancellationTokenSource cts = new CancellationTokenSource();

        public CancellationToken Token
        {
            get
            {
                return cts.Token;
            }
        }

        public void Dispose()
        {
            if (!cts.IsCancellationRequested)
            {
                cts.Cancel();
            }
        }
    }

    internal static class Disposable
    {
        public static IDisposable Create(IDisposable d1, IDisposable d2)
        {
            return new BinaryDisposable(d1, d2);
        }
    }

    internal class BinaryDisposable : IDisposable
    {
        private IDisposable _d1;
        private IDisposable _d2;

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
}

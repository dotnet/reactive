// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 
using System.Threading;

namespace System.Linq
{
    class CancellationTokenDisposable : IDisposable
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

    class CompositeDisposable : IDisposable
    {
        private static IDisposable[] s_empty = new IDisposable[0];
        private IDisposable[] _dispose;

        public CompositeDisposable(params IDisposable[] dispose)
        {
            _dispose = dispose;
        }

        public void Dispose()
        {
            var dispose = Interlocked.Exchange(ref _dispose, s_empty);

            foreach (var d in dispose)
            {
                d.Dispose();
            }
        }
    }

    class AssignableDisposable : IDisposable
    {
        private readonly object _gate = new object();
        private IDisposable _disposable;
        private bool _disposed;

        public IDisposable Disposable
        {
            set
            {
                lock (_gate)
                {
                    DisposeInner();

                    _disposable = value;

                    if (_disposed)
                    {
                        DisposeInner();
                    }
                }
            }
        }

        public void Dispose()
        {
            lock (_gate)
            {
                if (!_disposed)
                {
                    _disposed = true;
                    DisposeInner();
                }
            }
        }

        private void DisposeInner()
        {
            if (_disposable != null)
            {
                _disposable.Dispose();
                _disposable = null;
            }
        }
    }

    class Disposable : IDisposable
    {
        private static Action s_nop = () => { };
        private Action _dispose;

        public Disposable(Action dispose)
        {
            _dispose = dispose;
        }

        public static IDisposable Create(IDisposable d1, IDisposable d2)
        {
            return new BinaryDisposable(d1, d2);
        }

        public static IDisposable Create(params IDisposable[] disposables)
        {
            return new CompositeDisposable(disposables);
        }

        public void Dispose()
        {
            var dispose = Interlocked.Exchange(ref _dispose, s_nop);
            dispose();
        }
    }

    class BinaryDisposable : IDisposable
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

// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.
using System;
using System.Collections.Generic;
using System.Threading;

namespace System.Linq
{
    class CancellationTokenDisposable : IDisposable
    {
        private readonly CancellationTokenSource cts = new CancellationTokenSource();

        public CancellationToken Token { get { return cts.Token; } }

        public void Dispose()
        {
            if (!cts.IsCancellationRequested)
                cts.Cancel();
        }
    }

    class CompositeDisposable : IDisposable
    {
        private readonly IDisposable[] _dispose;

        public CompositeDisposable(params IDisposable[] dispose)
        {
            _dispose = dispose;
        }

        public void Dispose()
        {
            foreach (var d in _dispose)
                d.Dispose();
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
                    if (_disposable != null)
                        _disposable.Dispose();

                    _disposable = value;

                    if ((_disposable != null) && (_disposed))
                        _disposable.Dispose();
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

                    if (_disposable != null)
                        _disposable.Dispose();
                }
            }
        }
    }

    class Disposable : IDisposable
    {
        private readonly Action _dispose;

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
            _dispose();
        }
    }

    class BinaryDisposable : IDisposable
    {
        private readonly IDisposable _d1;
        private readonly IDisposable _d2;

        public BinaryDisposable(IDisposable d1, IDisposable d2)
        {
            _d1 = d1;
            _d2 = d2;
        }

        public void Dispose()
        {
            _d1.Dispose();
            _d2.Dispose();
        }
    }
}

// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.
using System;
using System.Collections.Generic;
using System.Threading;

namespace System.Linq
{
    class CancellationTokenDisposable : IDisposable
    {
        private CancellationTokenSource cts = new CancellationTokenSource();

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
        private object _gate = new object();
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

                    if (_disposed)
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

        public void Dispose()
        {
            _dispose();
        }
    }
}

// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_PERF && !NO_CDS
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive.Disposables;
using System.Threading;

namespace System.Reactive.Linq.Observαble
{
    class GetEnumerator<TSource> : IEnumerator<TSource>, IObserver<TSource>
    {
        private readonly ConcurrentQueue<TSource> _queue;
        private TSource _current;
        private Exception _error;
        private bool _done;
        private bool _disposed;

        private readonly SemaphoreSlim _gate;
        private readonly SingleAssignmentDisposable _subscription;

        public GetEnumerator()
        {
            _queue = new ConcurrentQueue<TSource>();
            _gate = new SemaphoreSlim(0);
            _subscription = new SingleAssignmentDisposable();
        }

        public IEnumerator<TSource> Run(IObservable<TSource> source)
        {
            //
            // [OK] Use of unsafe Subscribe: non-pretentious exact mirror with the dual GetEnumerator method.
            //
            _subscription.Disposable = source.Subscribe/*Unsafe*/(this);
            return this;
        }

        public void OnNext(TSource value)
        {
            _queue.Enqueue(value);
            _gate.Release();
        }

        public void OnError(Exception error)
        {
            _error = error;
            _subscription.Dispose();
            _gate.Release();
        }

        public void OnCompleted()
        {
            _done = true;
            _subscription.Dispose();
            _gate.Release();
        }

        public bool MoveNext()
        {
            _gate.Wait();

            if (_disposed)
                throw new ObjectDisposedException("");

            if (_queue.TryDequeue(out _current))
                return true;

            _error.ThrowIfNotNull();

            Debug.Assert(_done);

            _gate.Release(); // In the (rare) case the user calls MoveNext again we shouldn't block!
            return false;
        }

        public TSource Current
        {
            get { return _current; }
        }

        object Collections.IEnumerator.Current
        {
            get { return _current; }
        }

        public void Dispose()
        {
            _subscription.Dispose();

            _disposed = true;
            _gate.Release();
        }

        public void Reset()
        {
            throw new NotSupportedException();
        }
    }
}
#endif
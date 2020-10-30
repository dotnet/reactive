// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive.Disposables;
using System.Threading;

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class GetEnumerator<TSource> : IEnumerator<TSource>, IObserver<TSource>
    {
        private readonly ConcurrentQueue<TSource> _queue;
        private TSource? _current;
        private Exception? _error;
        private bool _done;
        private bool _disposed;
        private SingleAssignmentDisposableValue _subscription;

        private readonly SemaphoreSlim _gate;

        public GetEnumerator()
        {
            _queue = new ConcurrentQueue<TSource>();
            _gate = new SemaphoreSlim(0);
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
            {
                throw new ObjectDisposedException("");
            }

            if (_queue.TryDequeue(out _current))
            {
                return true;
            }

            _error?.Throw();

            Debug.Assert(_done);

            _gate.Release(); // In the (rare) case the user calls MoveNext again we shouldn't block!
            return false;
        }

        public TSource Current => _current!; // NB: Only called after MoveNext returns true and assigns a value.

        object Collections.IEnumerator.Current => _current!;

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

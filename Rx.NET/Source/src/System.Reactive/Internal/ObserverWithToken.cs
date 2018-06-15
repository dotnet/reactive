// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Text;

namespace System.Reactive
{
    /// <summary>
    /// Wraps another IObserver, relays signals to it
    /// and hosts an external IDisposable
    /// to be disposed upon termination.
    /// </summary>
    /// <typeparam name="T">The element type of the sequence.</typeparam>
    internal sealed class ObserverWithToken<T> : IObserver<T>
    {
        readonly IObserver<T> _downstream;

        IDisposable _tokenDisposable;

        public ObserverWithToken(IObserver<T> downstream)
        {
            this._downstream = downstream;
        }

        internal void SetTokenDisposable(IDisposable d)
        {
            Disposable.SetSingle(ref _tokenDisposable, d);
        }

        public void OnCompleted()
        {
            try
            {
                _downstream.OnCompleted();
            }
            finally
            {
                Disposable.TryDispose(ref _tokenDisposable);    
            }
        }

        public void OnError(Exception error)
        {
            try
            {
                _downstream.OnError(error);
            }
            finally
            {
                Disposable.TryDispose(ref _tokenDisposable);
            }
        }

        public void OnNext(T value)
        {
            _downstream.OnNext(value);
        }
    }
}

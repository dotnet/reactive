// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Text;
using System.Threading;

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class LastBlocking<T> : CountdownEvent, IObserver<T>
    {
        IDisposable _upstream;

        internal T _value;
        internal bool _hasValue;
        internal Exception _error;

        int once;

        internal LastBlocking() : base(1) { }

        internal void SetUpstream(IDisposable d)
        {
            Disposable.SetSingle(ref _upstream, d);
        }

        public void OnCompleted()
        {
            Unblock();
            Disposable.TryDispose(ref _upstream);
        }

        public void OnError(Exception error)
        {
            _value = default;
            this._error = error;
            Unblock();
            Disposable.TryDispose(ref _upstream);
        }

        public void OnNext(T value)
        {
            this._value = value;
            this._hasValue = true;
        }

        void Unblock()
        {
            if (Interlocked.CompareExchange(ref once, 1, 0) == 0)
            {
                Signal();
            }
        }
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Threading;

namespace System.Reactive.Linq.ObservableImpl
{
    internal abstract class BaseBlocking<T> : ManualResetEventSlim, IObserver<T>
    {
        internal T _value;
        internal bool _hasValue;
        internal Exception _error;

        internal BaseBlocking() { }

        public void OnCompleted()
        {
            Set();
        }

        public void OnError(Exception error)
        {
            _value = default;
            _error = error;
            Set();
        }

        public abstract void OnNext(T value);
    }

    internal sealed class FirstBlocking<T> : BaseBlocking<T>
    {
        public override void OnNext(T value)
        {
            if (!_hasValue)
            {
                _value = value;
                _hasValue = true;
                Set();
            }
        }
    }

    internal sealed class LastBlocking<T> : BaseBlocking<T>
    {
        public override void OnNext(T value)
        {
            _value = value;
            _hasValue = true;
        }
    }
}

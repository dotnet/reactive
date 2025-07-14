﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Threading;

namespace System.Reactive
{
    //
    // See AutoDetachObserver.cs for more information on the safeguarding requirement and
    // its implementation aspects.
    //

    /// <summary>
    /// This class fuses logic from ObserverBase, AnonymousObserver, and SafeObserver into one class. When an observer
    /// needs to be safeguarded, an instance of this type can be created by SafeObserver.Create when it detects its
    /// input is an AnonymousObserver, which is commonly used by end users when using the Subscribe extension methods
    /// that accept delegates for the On* handlers. By doing the fusion, we make the call stack depth shorter which
    /// helps debugging and some performance.
    /// </summary>
    internal sealed class AnonymousSafeObserver<T> : SafeObserver<T>
    {
        private readonly Action<T> _onNext;
        private readonly Action<Exception> _onError;
        private readonly Action _onCompleted;

        private int _isStopped;

        public AnonymousSafeObserver(Action<T> onNext, Action<Exception> onError, Action onCompleted)
        {
            _onNext = onNext;
            _onError = onError;
            _onCompleted = onCompleted;
        }

        public override void OnNext(T value)
        {
            if (_isStopped == 0)
            {
                var noError = false;
                try
                {
                    _onNext(value);
                    noError = true;
                }
                finally
                {
                    if (!noError)
                    {
                        Dispose();
                    }
                }
            }
        }

        public override void OnError(Exception error)
        {
            if (Interlocked.Exchange(ref _isStopped, 1) == 0)
            {
                using (this)
                {
                    _onError(error);
                }
            }
        }

        public override void OnCompleted()
        {
            if (Interlocked.Exchange(ref _isStopped, 1) == 0)
            {
                using (this)
                {
                    _onCompleted();
                }
            }
        }
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

#if !NO_PERF
using System;
using System.Reactive.Disposables;

namespace System.Reactive.Linq.ObservableImpl
{
    class Never<TResult> : IObservable<TResult>
    {
        public IDisposable Subscribe(IObserver<TResult> observer)
        {
            if (observer == null)
                throw new ArgumentNullException("observer");

            return Disposable.Empty;
        }
    }
}
#endif
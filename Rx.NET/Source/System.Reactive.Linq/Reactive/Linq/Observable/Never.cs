// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_PERF
using System;
using System.Reactive.Disposables;

namespace System.Reactive.Linq.Observαble
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
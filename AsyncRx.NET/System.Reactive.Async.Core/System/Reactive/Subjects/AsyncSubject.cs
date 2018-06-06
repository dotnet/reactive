// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive.Subjects
{
    public static class AsyncSubject
    {
        public static IAsyncSubject<T> Create<T>(IAsyncObserver<T> observer, IAsyncObservable<T> observable)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (observable == null)
                throw new ArgumentNullException(nameof(observable));

            return new AnonymousAsyncSubject<T>(observer, observable);
        }
    }
}

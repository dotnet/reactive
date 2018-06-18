// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Disposables;

namespace System.Reactive
{
    //
    // See AutoDetachObserver.cs for more information on the safeguarding requirement and
    // its implementation aspects.
    //

    internal sealed class SafeObserver<TSource> : ISafeObserver<TSource>
    {
        private readonly IObserver<TSource> _observer;

        private IDisposable _disposable;

        public static ISafeObserver<TSource> Create(IObserver<TSource> observer)
        {
            if (observer is AnonymousObserver<TSource> a)
            {
                return a.MakeSafe();
            }
            else
            {
                return new SafeObserver<TSource>(observer);
            }
        }

        private SafeObserver(IObserver<TSource> observer)
        {
            _observer = observer;
        }

        public void OnNext(TSource value)
        {
            var __noError = false;
            try
            {
                _observer.OnNext(value);
                __noError = true;
            }
            finally
            {
                if (!__noError)
                {
                    Dispose();
                }
            }
        }

        public void OnError(Exception error)
        {
            using (this)
            {
                _observer.OnError(error);
            }
        }

        public void OnCompleted()
        {
            using (this)
            {
                _observer.OnCompleted();
            }
        }

        public void SetResource(IDisposable resource)
        {
            Disposable.SetSingle(ref _disposable, resource);
        }

        public void Dispose()
        {
            Disposable.TryDispose(ref _disposable);
        }
    }
}

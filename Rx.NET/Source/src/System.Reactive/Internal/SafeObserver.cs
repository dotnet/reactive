// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive
{
    //
    // See AutoDetachObserver.cs for more information on the safeguarding requirement and
    // its implementation aspects.
    //

    internal sealed class SafeObserver<TSource> : IObserver<TSource>
    {
        private readonly IObserver<TSource> _observer;
        private readonly IDisposable _disposable;

        public static IObserver<TSource> Create(IObserver<TSource> observer, IDisposable disposable)
        {
            if (observer is AnonymousObserver<TSource> a)
            {
                return a.MakeSafe(disposable);
            }
            else
            {
                return new SafeObserver<TSource>(observer, disposable);
            }
        }

        private SafeObserver(IObserver<TSource> observer, IDisposable disposable)
        {
            _observer = observer;
            _disposable = disposable;
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
                    _disposable.Dispose();
                }
            }
        }

        public void OnError(Exception error)
        {
            try
            {
                _observer.OnError(error);
            }
            finally
            {
                _disposable.Dispose();
            }
        }

        public void OnCompleted()
        {
            try
            {
                _observer.OnCompleted();
            }
            finally
            {
                _disposable.Dispose();
            }
        }
    }
}

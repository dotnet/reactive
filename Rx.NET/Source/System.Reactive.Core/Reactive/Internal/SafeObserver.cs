// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;

namespace System.Reactive
{
    //
    // See AutoDetachObserver.cs for more information on the safeguarding requirement and
    // its implementation aspects.
    //

    class SafeObserver<TSource> : IObserver<TSource>
    {
        private readonly IObserver<TSource> _observer;
        private readonly IDisposable _disposable;

        public static IObserver<TSource> Create(IObserver<TSource> observer, IDisposable disposable)
        {
            var a = observer as AnonymousObserver<TSource>;
            if (a != null)
                return a.MakeSafe(disposable);
            else
                return new SafeObserver<TSource>(observer, disposable);
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
                    _disposable.Dispose();
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

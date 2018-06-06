// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

#if NO_PERF
using System.Collections.Generic;

namespace System.Reactive
{
    sealed class PushPullAdapter<T, R> : IObserver<T>, IEnumerator<R>
    {
        Action<Notification<T>> yield;
        Action dispose;
        Func<Notification<R>> moveNext;
        Notification<R> current;
        bool done = false;
        bool disposed;

        public PushPullAdapter(Action<Notification<T>> yield, Func<Notification<R>> moveNext, Action dispose)
        {
            this.yield = yield;
            this.moveNext = moveNext;
            this.dispose = dispose;
        }

        public void OnNext(T value)
        {
            yield(Notification.CreateOnNext<T>(value));
        }

        public void OnError(Exception exception)
        {
            yield(Notification.CreateOnError<T>(exception));
            dispose();
        }

        public void OnCompleted()
        {
            yield(Notification.CreateOnCompleted<T>());
            dispose();
        }

        public R Current
        {
            get { return current.Value; }
        }

        public void Dispose()
        {
            disposed = true;
            dispose();
        }

        object System.Collections.IEnumerator.Current
        {
            get { return this.Current; }
        }

        public bool MoveNext()
        {
            if (disposed)
                throw new ObjectDisposedException("");

            if (!done)
            {
                current = moveNext();
                done = current.Kind != NotificationKind.OnNext;
            }

            current.Exception.ThrowIfNotNull();

            return current.HasValue;
        }

        public void Reset()
        {
            throw new NotSupportedException();
        }
    }
}
#endif
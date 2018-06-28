// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Disposables;
using System.Threading;

namespace System.Reactive.Linq.InternalObservable
{
    /// <summary>
    /// Represents an observer of events and supports push-based disposable management.
    /// The protocol is as follows: <code>OnSubscribe OnNext* (OnError|OnCompleted)?</code>.
    /// The difference from the IObserver protocol is that OnSubscribe has to be called before
    /// any of the other OnXXX methods are.
    /// Note that protocol conversion is needed when talking to a regular IObservable.
    /// </summary>
    /// <typeparam name="T">The value type observed.</typeparam>
    internal interface IInternalObserver<in T> : IObserver<T>, IDisposable
    {
        /// <summary>
        /// Called by the InternalSourceObservable to push an IDisposable before
        /// any other OnXXX methods are called in order to provide the means
        /// to cancel the flow both synchronously and asynchronously.
        /// </summary>
        /// <param name="upstream">The IDisposable pushed by the source InternalSourceObservable that
        /// allows cancelling a flow.</param>
        void OnSubscribe(IDisposable upstream);
    }

    /// <summary>
    /// Wraps a regular IObserver into an IInternalObserver and adds 
    /// the deferred IDisposable management support.
    /// </summary>
    /// <typeparam name="T">The upstream value type observed.</typeparam>
    internal sealed class ToInternalObserver<T> : IInternalObserver<T>
    {
        readonly IObserver<T> downstream;

        IDisposable upstream;

        internal ToInternalObserver(IObserver<T> downstream)
        {
            this.downstream = downstream;
        }

        public void Dispose()
        {
            var d = Volatile.Read(ref upstream);
            if (d != BooleanDisposable.True)
            {
                Interlocked.Exchange(ref upstream, BooleanDisposable.True)?.Dispose();
            }
        }

        public void OnCompleted()
        {
            Volatile.Write(ref upstream, BooleanDisposable.True);
            downstream.OnCompleted();
        }

        public void OnError(Exception error)
        {
            Volatile.Write(ref upstream, BooleanDisposable.True);
            downstream.OnError(error);
        }

        public void OnNext(T value)
        {
            downstream.OnNext(value);
        }

        public void OnSubscribe(IDisposable upstream)
        {
            if (upstream == null)
            {
                throw new ArgumentNullException(nameof(upstream));
            }
            if (Interlocked.CompareExchange(ref this.upstream, upstream, null) != null)
            {
                upstream.Dispose();
            }
        }
    }

}

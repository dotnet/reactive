// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Text;

namespace System.Reactive.Linq.InternalObservable
{
    /// <summary>
    /// Base class for source-like operators that use an internal extended Observable protocol
    /// to talk to observers.
    /// The protocol is as follows: <code>OnSubscribe OnNext* (OnError|OnCompleted)?</code>.
    /// </summary>
    /// <typeparam name="T">The output value type.</typeparam>
    internal abstract class InternalBaseObservable<T> : IInternalObservable<T>
    {
        
        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (observer is IInternalObserver<T> o)
            {
                Subscribe(o);
                return o;
            }

            var parent = new ToInternalObserver<T>(observer);
            Subscribe(parent);
            return parent;
        }

        /// <summary>
        /// Handles the incoming IInternalObserver instance. Implement the
        /// operator's business logic here.
        /// </summary>
        /// <param name="observer">The observer to interact with</param>
        public abstract void Subscribe(IInternalObserver<T> observer);
    }
 
    /// <summary>
    /// Represents an intermediate operator (an operator that has an upstream source on its own).
    /// </summary>
    /// <typeparam name="T">The upstream value type.</typeparam>
    /// <typeparam name="R">The output value type</typeparam>
    internal abstract class InternalIntermediateObservable<T, R> : InternalBaseObservable<R>
    {
        protected readonly IInternalObservable<T> source;

        internal InternalIntermediateObservable(IObservable<T> source)
        {
            if (source is IInternalObservable<T> o)
            {
                this.source = o;
            } else {
                this.source = new ToInternalSourceObservable<T>(source);
            }
        }

    }

    /// <summary>
    /// Wraps a regular IObservable and performs the protocol translation between it
    /// and the InternalSourceObservable/IInternalObserver, namely, the calling of
    /// OnSubscribe.
    /// </summary>
    /// <typeparam name="T">The source and output value type</typeparam>
    internal sealed class ToInternalSourceObservable<T> : InternalBaseObservable<T>
    {
        readonly IObservable<T> source;

        internal ToInternalSourceObservable(IObservable<T> source)
        {
            this.source = source;
        }

        public override void Subscribe(IInternalObserver<T> observer)
        {
            if (source is IInternalObservable<T> o)
            {
                o.Subscribe(observer);
            }
            else
            {
                var sad = new SingleAssignmentDisposable();
                observer.OnSubscribe(sad);
                sad.Disposable = source.SubscribeSafe(observer);
            }
        }
    }
}

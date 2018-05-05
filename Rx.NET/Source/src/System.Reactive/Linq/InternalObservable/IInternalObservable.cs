using System;
using System.Collections.Generic;
using System.Text;

namespace System.Reactive.Linq.InternalObservable
{
    /// <summary>
    /// Base interface for source-like operators that use an internal extended Observable protocol
    /// to talk to observers.
    /// The protocol is as follows: <code>OnSubscribe OnNext* (OnError|OnCompleted)?</code>.
    /// </summary>
    /// <typeparam name="T">The output value type.</typeparam>
    internal interface IInternalObservable<out T> : IObservable<T>
    {
        /// <summary>
        /// Handles the incoming IInternalObserver instance. Implement the
        /// operator's business logic here.
        /// </summary>
        /// <param name="observer">The observer to interact with</param>
        void Subscribe(IInternalObserver<T> observer);
    }
}

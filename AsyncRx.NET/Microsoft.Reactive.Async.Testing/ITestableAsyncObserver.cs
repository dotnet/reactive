using System;
using System.Collections.Generic;
using System.Reactive;

namespace Microsoft.Reactive.Async.Testing
{
    /// <summary>
    /// Observer that records received notification messages and timestamps those.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    public interface ITestableAsyncObserver<T> : IAsyncObserver<T>
    {
        /// <summary>
        /// Gets recorded timestamped notification messages received by the observer.
        /// </summary>
        IReadOnlyList<Recorded<Notification<T>>> Messages { get; }
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Reactive;

namespace Microsoft.Reactive.Testing
{
    /// <summary>
    /// Observer that records received notification messages and timestamps those.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    public interface ITestableObserver<T> : IObserver<T>
    {
        /// <summary>
        /// Gets recorded timestamped notification messages received by the observer.
        /// </summary>
        IList<Recorded<Notification<T>>> Messages { get; }
    }
}

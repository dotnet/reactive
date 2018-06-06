// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive.Concurrency
{
    /// <summary>
    /// Represents a work item that has been scheduled.
    /// </summary>
    /// <typeparam name="TAbsolute">Absolute time representation type.</typeparam>
    public interface IScheduledItem<TAbsolute>
    {
        /// <summary>
        /// Gets the absolute time at which the item is due for invocation.
        /// </summary>
        TAbsolute DueTime { get; }

        /// <summary>
        /// Invokes the work item.
        /// </summary>
        void Invoke();
    }
}

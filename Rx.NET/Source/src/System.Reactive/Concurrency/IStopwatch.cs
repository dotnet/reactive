// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive.Concurrency
{
    /// <summary>
    /// Abstraction for a stopwatch to compute time relative to a starting point.
    /// </summary>
    public interface IStopwatch
    {
        /// <summary>
        /// Gets the time elapsed since the stopwatch object was obtained.
        /// </summary>
        TimeSpan Elapsed { get; }
    }
}

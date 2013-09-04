// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;

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

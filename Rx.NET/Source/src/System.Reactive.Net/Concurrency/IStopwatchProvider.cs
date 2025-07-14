﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive.Concurrency
{
    /*
     * The ability to request a stopwatch object has been introduced in Rx v2.0 to reduce the
     * number of allocations made by operators that use absolute time to compute relative time
     * diffs, such as TimeInterval and Delay. This causes a large number of related objects to
     * be allocated in the BCL, e.g. System.Globalization.DaylightTime.
     */

    /// <summary>
    /// Provider for <see cref="IStopwatch"/> objects.
    /// </summary>
    public interface IStopwatchProvider
    {
        /// <summary>
        /// Starts a new stopwatch object.
        /// </summary>
        /// <returns>New stopwatch object; started at the time of the request.</returns>
        IStopwatch StartStopwatch();
    }
}

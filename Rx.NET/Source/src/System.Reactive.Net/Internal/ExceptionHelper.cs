// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.IO;
using System.Threading;

namespace System.Reactive
{
    /// <summary>
    /// Utility methods to handle lock-free combining of Exceptions
    /// as well as hosting a terminal-exception indicator for
    /// lock-free termination support.
    /// </summary>
    internal static class ExceptionHelper
    {
        /// <summary>
        /// The singleton instance of the exception indicating a terminal state,
        /// DO NOT LEAK or signal this via OnError!
        /// </summary>
        public static Exception Terminated { get; } = new EndOfStreamException("On[Error|Completed]");

        /// <summary>
        /// Tries to atomically set the Exception on the given field if it is
        /// still null.
        /// </summary>
        /// <param name="field">The target field to try to set atomically.</param>
        /// <param name="ex">The exception to set, not null (not verified).</param>
        /// <returns>True if the operation succeeded, false if the target was not null.</returns>
        public static bool TrySetException(ref Exception? field, Exception ex)
        {
            return Interlocked.CompareExchange(ref field, ex, null) == null;
        }
    }
}

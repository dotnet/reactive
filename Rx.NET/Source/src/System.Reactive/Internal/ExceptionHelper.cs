// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
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
        public static Exception Terminated { get; } = new TerminatedException();

        /// <summary>
        /// Tries to atomically set the Exception on the given field if it is
        /// still null.
        /// </summary>
        /// <param name="field">The target field to try to set atomically.</param>
        /// <param name="ex">The exception to set, not null (not verified).</param>
        /// <returns>True if the operation succeeded, false if the target was not null.</returns>
        public static bool TrySetException(ref Exception field, Exception ex)
        {
            return Interlocked.CompareExchange(ref field, ex, null) == null;
        }

        /// <summary>
        /// Atomically swaps in the Terminated exception into the field and
        /// returns the previous exception in that field (which could be the
        /// Terminated instance too).
        /// </summary>
        /// <param name="field">The target field to terminate.</param>
        /// <returns>The previous exception in that field before the termination.</returns>
        public static Exception Terminate(ref Exception field)
        {
            var current = Volatile.Read(ref field);
            if (current != Terminated)
            {
                current = Interlocked.Exchange(ref field, Terminated);
            }
            return current;
        }

        /// <summary>
        /// Atomically sets the field to the given new exception or combines
        /// it with any pre-existing exception as a new AggregateException
        /// unless the field contains the Terminated instance.
        /// </summary>
        /// <param name="field">The field to set or combine with.</param>
        /// <param name="ex">The exception to combine with.</param>
        /// <returns>True if successful, false if the field contains the Terminated instance.</returns>
        /// <remarks>This type of atomic aggregation helps with operators that
        /// want to delay all errors until all of their sources terminate in some way.</remarks>
        public static bool TryAddException(ref Exception field, Exception ex)
        {
            for (; ; )
            {
                var current = Volatile.Read(ref field);
                if (current == Terminated)
                {
                    return false;
                }

                Exception b;

                if (current == null)
                {
                    b = ex;
                }
                else if (current is AggregateException a)
                {
                    var list = new List<Exception>(a.InnerExceptions)
                    {
                        ex
                    };
                    b = new AggregateException(list);
                }
                else
                {
                    b = new AggregateException(current, ex);
                }

                if (Interlocked.CompareExchange(ref field, b, current) == current)
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// The class indicating a terminal state as an Exception type.
        /// </summary>
        private sealed class TerminatedException : Exception
        {
            internal TerminatedException() : base("No further exceptions")
            {
            }
        }
    }
}

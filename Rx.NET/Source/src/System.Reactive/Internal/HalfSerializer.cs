// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Diagnostics;
using System.Threading;

namespace System.Reactive
{
    /// <summary>
    /// Utility methods for dealing with serializing OnXXX signals
    /// for an IObserver where concurrent OnNext is still not allowed
    /// but concurrent OnError/OnCompleted may happen.
    /// This serialization case is generally lower overhead than
    /// a full SerializedObserver wrapper and doesn't need
    /// allocation.
    /// </summary>
    internal static class HalfSerializer
    {
        /// <summary>
        /// Signals the given item to the observer in a serialized fashion
        /// allowing a concurrent OnError or OnCompleted emission to be delayed until
        /// the observer.OnNext returns.
        /// Do not call OnNext from multiple threads as it may lead to ignored items.
        /// Use a full SerializedObserver wrapper for merging multiple sequences.
        /// </summary>
        /// <typeparam name="T">The element type of the observer.</typeparam>
        /// <param name="sink">The observer to signal events in a serialized fashion.</param>
        /// <param name="item">The item to signal.</param>
        /// <param name="wip">Indicates there is an emission going on currently.</param>
        /// <param name="error">The field containing an error or terminal indicator.</param>
        public static void ForwardOnNext<T>(ISink<T> sink, T item, ref int wip, ref Exception error)
        {
            if (Interlocked.CompareExchange(ref wip, 1, 0) == 0)
            {
                sink.ForwardOnNext(item);
                if (Interlocked.Decrement(ref wip) != 0)
                {
                    var ex = error;
                    if (ex != ExceptionHelper.Terminated)
                    {
                        error = ExceptionHelper.Terminated;
                        sink.ForwardOnError(ex);
                    }
                    else
                    {
                        sink.ForwardOnCompleted();
                    }
                }
            }
#if (HAS_TRACE)
            else if (error == null)
                Trace.TraceWarning("OnNext called while another OnNext call was in progress on the same Observer.");
#endif
        }

        /// <summary>
        /// Signals the given exception to the observer. If there is a concurrent
        /// OnNext emission is happening, saves the exception into the given field
        /// otherwise to be picked up by <see cref="ForwardOnNext{T}"/>.
        /// This method can be called concurrently with itself and the other methods of this
        /// helper class but only one terminal signal may actually win.
        /// </summary>
        /// <typeparam name="T">The element type of the observer.</typeparam>
        /// <param name="sink">The observer to signal events in a serialized fashion.</param>
        /// <param name="ex">The exception to signal sooner or later.</param>
        /// <param name="wip">Indicates there is an emission going on currently.</param>
        /// <param name="error">The field containing an error or terminal indicator.</param>
        public static void ForwardOnError<T>(ISink<T> sink, Exception ex, ref int wip, ref Exception error)
        {
            if (ExceptionHelper.TrySetException(ref error, ex))
            {
                if (Interlocked.Increment(ref wip) == 1)
                {
                    error = ExceptionHelper.Terminated;
                    sink.ForwardOnError(ex);
                }
            }
        }

        /// <summary>
        /// Signals OnCompleted on the observer. If there is a concurrent
        /// OnNext emission happening, the error field will host a special
        /// terminal exception signal to be picked up by <see cref="ForwardOnNext{T}"/> once it finishes with OnNext and signal the
        /// OnCompleted as well.
        /// This method can be called concurrently with itself and the other methods of this
        /// helper class but only one terminal signal may actually win.
        /// </summary>
        /// <typeparam name="T">The element type of the observer.</typeparam>
        /// <param name="sink">The observer to signal events in a serialized fashion.</param>
        /// <param name="wip">Indicates there is an emission going on currently.</param>
        /// <param name="error">The field containing an error or terminal indicator.</param>
        public static void ForwardOnCompleted<T>(ISink<T> sink, ref int wip, ref Exception error)
        {
            if (ExceptionHelper.TrySetException(ref error, ExceptionHelper.Terminated))
            {
                if (Interlocked.Increment(ref wip) == 1)
                {
                    sink.ForwardOnCompleted();
                }
            }
        }
    }
}

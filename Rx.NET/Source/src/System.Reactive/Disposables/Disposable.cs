// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Threading;

namespace System.Reactive.Disposables
{
    /// <summary>
    /// Provides a set of static methods for creating <see cref="IDisposable"/> objects.
    /// </summary>
    public static class Disposable
    {
        /// <summary>
        /// Gets the disposable that does nothing when disposed.
        /// </summary>
        public static IDisposable Empty => DefaultDisposable.Instance;

        /// <summary>
        /// Creates a disposable object that invokes the specified action when disposed.
        /// </summary>
        /// <param name="dispose">Action to run during the first call to <see cref="IDisposable.Dispose"/>. The action is guaranteed to be run at most once.</param>
        /// <returns>The disposable object that runs the given action upon disposal.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="dispose"/> is <c>null</c>.</exception>
        public static IDisposable Create(Action dispose)
        {
            if (dispose == null)
                throw new ArgumentNullException(nameof(dispose));

            return new AnonymousDisposable(dispose);
        }

        /// <summary>
        /// Gets or sets the underlying disposable. After disposal, the result of getting this property is undefined.
        /// </summary>
        internal static IDisposable GetValue(ref IDisposable fieldRef)
        {
            var current = Volatile.Read(ref fieldRef);

            return current == BooleanDisposable.True 
                ? null
                : current;
        }

        /// <summary>
        /// Gets or sets the underlying disposable. After disposal, the result of getting this property is undefined.
        /// </summary>
        internal static IDisposable GetValueOrDefault(ref IDisposable fieldRef)
        {
            var current = Volatile.Read(ref fieldRef);

            return current == BooleanDisposable.True
                ? DefaultDisposable.Instance
                : current;
        }

        internal static bool TrySetSingle(ref IDisposable fieldRef, IDisposable value)
        {
            var old = Interlocked.CompareExchange(ref fieldRef, value, null);
            if (old == null)
                return true;

            if (old != BooleanDisposable.True)
                throw new InvalidOperationException(Strings_Core.DISPOSABLE_ALREADY_ASSIGNED);

            value?.Dispose();
            return false;
        }

        internal static bool TrySetMultiple(ref IDisposable fieldRef, IDisposable value)
        {
            // Let's read the current value atomically (also prevents reordering).
            var old = Volatile.Read(ref fieldRef);

            for (; ; )
            {
                // If it is the disposed instance, dispose the value.
                if (old == BooleanDisposable.True)
                {
                    value?.Dispose();
                    return false;
                }

                // Atomically swap in the new value and get back the old.
                var b = Interlocked.CompareExchange(ref fieldRef, value, old);

                // If the old and new are the same, the swap was successful and we can quit
                if (old == b)
                {
                    return true;
                }

                // Otherwise, make the old reference the current and retry.
                old = b;
            }
        }

        internal static bool TrySetSerial(ref IDisposable fieldRef, IDisposable value)
        {
            var copy = Volatile.Read(ref fieldRef);
            for (; ; )
            {
                if (copy == BooleanDisposable.True)
                {
                    value?.Dispose();
                    return false;
                }

                var current = Interlocked.CompareExchange(ref fieldRef, value, copy);
                if (current == copy)
                {
                    copy?.Dispose();
                    return true;
                }

                copy = current;
            }
        }

        internal static bool GetIsDisposed(ref IDisposable fieldRef)
        {
            // We use a sentinel value to indicate we've been disposed. This sentinel never leaks
            // to the outside world (see the Disposable property getter), so no-one can ever assign
            // this value to us manually.
            return Volatile.Read(ref fieldRef) == BooleanDisposable.True;
        }

        internal static bool TryDispose(ref IDisposable fieldRef)
        {
            var old = Interlocked.Exchange(ref fieldRef, BooleanDisposable.True);

            if (old == BooleanDisposable.True)
                return false;

            old?.Dispose();
            return true;
        }
    }
}

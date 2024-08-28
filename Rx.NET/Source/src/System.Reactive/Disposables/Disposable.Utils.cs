// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace System.Reactive.Disposables
{
    internal enum TrySetSingleResult
    {
        Success,
        AlreadyAssigned,
        Disposed
    }

    public static partial class Disposable
    {
        /// <summary>
        /// Gets the value stored in <paramref name="fieldRef" /> or a null if
        /// <paramref name="fieldRef" /> was already disposed.
        /// </summary>
        internal static IDisposable? GetValue([NotNullIfNotNull(nameof(fieldRef))] /*in*/ ref IDisposable? fieldRef)
        {
            var current = Volatile.Read(ref fieldRef);

            return current == BooleanDisposable.True
                ? null
                : current;
        }

        /// <summary>
        /// Gets the value stored in <paramref name="fieldRef" /> or a no-op-Disposable if
        /// <paramref name="fieldRef" /> was already disposed.
        /// </summary>
        [return: NotNullIfNotNull(nameof(fieldRef))]
        internal static IDisposable? GetValueOrDefault([NotNullIfNotNull(nameof(fieldRef))] /*in*/ ref IDisposable? fieldRef)
        {
            var current = Volatile.Read(ref fieldRef);

            return current == BooleanDisposable.True
                ? EmptyDisposable.Instance
                : current;
        }

        /// <summary>
        /// Tries to assign <paramref name="value" /> to <paramref name="fieldRef" />.
        /// </summary>
        /// <returns>A <see cref="TrySetSingleResult"/> value indicating the outcome of the operation.</returns>
        internal static TrySetSingleResult TrySetSingle([NotNullIfNotNull(nameof(value))] ref IDisposable? fieldRef, IDisposable? value)
        {
            var old = Interlocked.CompareExchange(ref fieldRef, value, null);
            if (old == null)
            {
                return TrySetSingleResult.Success;
            }

            if (old != BooleanDisposable.True)
            {
                return TrySetSingleResult.AlreadyAssigned;
            }

            value?.Dispose();
            return TrySetSingleResult.Disposed;
        }

        /// <summary>
        /// Tries to assign <paramref name="value" /> to <paramref name="fieldRef" />. If <paramref name="fieldRef" />
        /// is not disposed and is assigned a different value, it will not be disposed.
        /// </summary>
        /// <returns>true if <paramref name="value" /> was successfully assigned to <paramref name="fieldRef" />.</returns>
        /// <returns>false <paramref name="fieldRef" /> has been disposed.</returns>
        internal static bool TrySetMultiple([NotNullIfNotNull(nameof(value))] ref IDisposable? fieldRef, IDisposable? value)
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

        /// <summary>
        /// Tries to assign <paramref name="value" /> to <paramref name="fieldRef" />. If <paramref name="fieldRef" />
        /// is not disposed and is assigned a different value, it will be disposed.
        /// </summary>
        /// <returns>true if <paramref name="value" /> was successfully assigned to <paramref name="fieldRef" />.</returns>
        /// <returns>false <paramref name="fieldRef" /> has been disposed.</returns>
        internal static bool TrySetSerial([NotNullIfNotNull(nameof(value))] ref IDisposable? fieldRef, IDisposable? value)
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

        /// <summary>
        /// Disposes <paramref name="fieldRef" />. 
        /// </summary>
        internal static void Dispose([NotNullIfNotNull(nameof(fieldRef))] ref IDisposable? fieldRef)
        {
            var old = Interlocked.Exchange(ref fieldRef, BooleanDisposable.True);

            if (old != BooleanDisposable.True)
            {
                old?.Dispose();
            }
        }
    }
}

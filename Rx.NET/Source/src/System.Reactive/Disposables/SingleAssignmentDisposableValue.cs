// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Threading;

namespace System.Reactive.Disposables
{
    /// <summary>
    /// Represents a disposable resource which only allows a single assignment of its underlying disposable resource.
    /// If an underlying disposable resource has already been set, future attempts to set the underlying disposable resource will throw an <see cref="InvalidOperationException"/>.
    /// </summary>
    public struct SingleAssignmentDisposableValue
    {
        private IDisposable? _current;

        /// <summary>
        /// Gets a value that indicates whether the object is disposed.
        /// </summary>
        public bool IsDisposed =>
            // We use a sentinel value to indicate we've been disposed. This sentinel never leaks
            // to the outside world (see the Disposable property getter), so no-one can ever assign
            // this value to us manually.
            Volatile.Read(ref _current) == BooleanDisposable.True;

        /// <summary>
        /// Gets or sets the underlying disposable. After disposal, the result of getting this property is undefined.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if the <see cref="SingleAssignmentDisposable"/> has already been assigned to.</exception>
        public IDisposable? Disposable
        {
            get => Disposables.Disposable.GetValueOrDefault(ref _current);
            set
            {
                var result = Disposables.Disposable.TrySetSingle(ref _current, value);

                if (result == TrySetSingleResult.AlreadyAssigned)
                {
                    throw new InvalidOperationException(Strings_Core.DISPOSABLE_ALREADY_ASSIGNED);
                }
            }
        }

        /// <summary>
        /// Disposes the underlying disposable.
        /// </summary>
        public void Dispose()
        {
            Disposables.Disposable.Dispose(ref _current);
        }

        /// <inheritdoc/>
        public override readonly bool Equals(object? obj) => false;

        /// <inheritdoc/>
        public override readonly int GetHashCode() => 0;

#pragma warning disable IDE0060 // (Remove unused parameter.) Required part of public API
        public static bool operator ==(SingleAssignmentDisposableValue left, SingleAssignmentDisposableValue right) => false;

        public static bool operator !=(SingleAssignmentDisposableValue left, SingleAssignmentDisposableValue right) => true;
#pragma warning restore IDE0060
    }
}

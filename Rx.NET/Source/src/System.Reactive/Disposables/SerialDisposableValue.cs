// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Threading;

namespace System.Reactive.Disposables
{
    /// <summary>
    /// Represents a disposable resource whose underlying disposable resource can be replaced by another disposable resource, causing automatic disposal of the previous underlying disposable resource.
    /// </summary>
    internal struct SerialDisposableValue : ICancelable
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
        /// Gets or sets the underlying disposable.
        /// </summary>
        /// <remarks>If the SerialDisposable has already been disposed, assignment to this property causes immediate disposal of the given disposable object. Assigning this property disposes the previous disposable object.</remarks>
        public IDisposable? Disposable
        {
            get => Disposables.Disposable.GetValue(ref _current);
            set => Disposables.Disposable.TrySetSerial(ref _current, value);
        }

        public bool TrySetFirst(IDisposable disposable) => Disposables.Disposable.TrySetSingle(ref _current, disposable) == TrySetSingleResult.Success;

        /// <summary>
        /// Disposes the underlying disposable as well as all future replacements.
        /// </summary>
        public void Dispose()
        {
            Disposables.Disposable.Dispose(ref _current);
        }
    }
}

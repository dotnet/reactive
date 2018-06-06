// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive.Disposables
{
    /// <summary>
    /// Represents a disposable resource that can be checked for disposal status.
    /// </summary>
    public sealed class BooleanDisposable : ICancelable
    {
        // Keep internal! This is used as sentinel in other IDisposable implementations to detect disposal and
        // should never be exposed to user code in order to prevent users from swapping in the sentinel. Have
        // a look at the code in e.g. SingleAssignmentDisposable for usage patterns.
        internal static readonly BooleanDisposable True = new BooleanDisposable(true);

        private volatile bool _isDisposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="BooleanDisposable"/> class.
        /// </summary>
        public BooleanDisposable()
        {
        }

        private BooleanDisposable(bool isDisposed)
        {
            _isDisposed = isDisposed;
        }

        /// <summary>
        /// Gets a value that indicates whether the object is disposed.
        /// </summary>
        public bool IsDisposed => _isDisposed;

        /// <summary>
        /// Sets the status to disposed, which can be observer through the <see cref="IsDisposed"/> property.
        /// </summary>
        public void Dispose()
        {
            _isDisposed = true;
        }
    }
}

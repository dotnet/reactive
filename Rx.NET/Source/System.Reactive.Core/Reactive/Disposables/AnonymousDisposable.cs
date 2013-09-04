// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System.Threading;

namespace System.Reactive.Disposables
{
    /// <summary>
    /// Represents an Action-based disposable.
    /// </summary>
    internal sealed class AnonymousDisposable : ICancelable
    {
        private volatile Action _dispose;

        /// <summary>
        /// Constructs a new disposable with the given action used for disposal.
        /// </summary>
        /// <param name="dispose">Disposal action which will be run upon calling Dispose.</param>
        public AnonymousDisposable(Action dispose)
        {
            System.Diagnostics.Debug.Assert(dispose != null);

            _dispose = dispose;
        }

        /// <summary>
        /// Gets a value that indicates whether the object is disposed.
        /// </summary>
        public bool IsDisposed
        {
            get { return _dispose == null; }
        }

        /// <summary>
        /// Calls the disposal action if and only if the current instance hasn't been disposed yet.
        /// </summary>
        public void Dispose()
        {
#pragma warning disable 0420
            var dispose = Interlocked.Exchange(ref _dispose, null);
#pragma warning restore 0420
            if (dispose != null)
            {
                dispose();
            }
        }
    }
}

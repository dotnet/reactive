// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Threading;

namespace System.Reactive.Disposables
{
    /// <summary>
    /// Represents a disposable resource that has an associated <seealso cref="CancellationToken"/> that will be set to the cancellation requested state upon disposal.
    /// </summary>
    public sealed class CancellationDisposable : ICancelable
    {
        private readonly CancellationTokenSource _cts;

        /// <summary>
        /// Initializes a new instance of the <see cref="CancellationDisposable"/> class that uses an existing <seealso cref="CancellationTokenSource"/>.
        /// </summary>
        /// <param name="cts"><seealso cref="CancellationTokenSource"/> used for cancellation.</param>
        /// <exception cref="ArgumentNullException"><paramref name="cts"/> is <c>null</c>.</exception>
        public CancellationDisposable(CancellationTokenSource cts)
        {
            if (cts == null)
                throw new ArgumentNullException(nameof(cts));

            _cts = cts;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CancellationDisposable"/> class that uses a new <seealso cref="CancellationTokenSource"/>.
        /// </summary>
        public CancellationDisposable()
            : this(new CancellationTokenSource())
        {
        }

        /// <summary>
        /// Gets the <see cref="CancellationToken"/> used by this <see cref="CancellationDisposable"/>.
        /// </summary>
        public CancellationToken Token => _cts.Token;

        /// <summary>
        /// Cancels the underlying <seealso cref="CancellationTokenSource"/>.
        /// </summary>
        public void Dispose() => _cts.Cancel();

        /// <summary>
        /// Gets a value that indicates whether the object is disposed.
        /// </summary>
        public bool IsDisposed => _cts.IsCancellationRequested;
    }
}

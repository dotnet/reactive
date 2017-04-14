// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive.Disposables
{
    /// <summary>
    /// Represents a disposable resource whose underlying disposable resource can be replaced by another disposable resource, causing automatic disposal of the previous underlying disposable resource.
    /// </summary>
    public sealed class SerialDisposable : ICancelable
    {
        private readonly object _gate = new object();
        private IDisposable _current;
        private bool _disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="SerialDisposable"/> class.
        /// </summary>
        public SerialDisposable()
        {
        }

        /// <summary>
        /// Gets a value that indicates whether the object is disposed.
        /// </summary>
        public bool IsDisposed
        {
            get
            {
                lock (_gate)
                {
                    return _disposed;
                }
            }
        }

        /// <summary>
        /// Gets or sets the underlying disposable.
        /// </summary>
        /// <remarks>If the SerialDisposable has already been disposed, assignment to this property causes immediate disposal of the given disposable object. Assigning this property disposes the previous disposable object.</remarks>
        public IDisposable Disposable
        {
            get
            {
                return _current;
            }

            set
            {
                var shouldDispose = false;
                var old = default(IDisposable);
                lock (_gate)
                {
                    shouldDispose = _disposed;
                    if (!shouldDispose)
                    {
                        old = _current;
                        _current = value;
                    }
                }

                old?.Dispose();

                if (shouldDispose)
                {
                    value?.Dispose();
                }
            }
        }

        /// <summary>
        /// Disposes the underlying disposable as well as all future replacements.
        /// </summary>
        public void Dispose()
        {
            var old = default(IDisposable);

            lock (_gate)
            {
                if (!_disposed)
                {
                    _disposed = true;
                    old = _current;
                    _current = null;
                }
            }

            old?.Dispose();
        }
    }
}

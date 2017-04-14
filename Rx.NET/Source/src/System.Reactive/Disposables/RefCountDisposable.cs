// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Threading;

namespace System.Reactive.Disposables
{
    /// <summary>
    /// Represents a disposable resource that only disposes its underlying disposable resource when all <see cref="GetDisposable">dependent disposable objects</see> have been disposed.
    /// </summary>
    public sealed class RefCountDisposable : ICancelable
    {
        private readonly bool _throwWhenDisposed;
        private readonly object _gate = new object();
        private IDisposable _disposable;
        private bool _isPrimaryDisposed;
        private int _count;

        /// <summary>
        /// Initializes a new instance of the <see cref="RefCountDisposable"/> class with the specified disposable.
        /// </summary>
        /// <param name="disposable">Underlying disposable.</param>
        /// <exception cref="ArgumentNullException"><paramref name="disposable"/> is null.</exception>
        public RefCountDisposable(IDisposable disposable) : this(disposable, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RefCountDisposable"/> class with the specified disposable.
        /// </summary>
        /// <param name="disposable">Underlying disposable.</param>
        /// <param name="throwWhenDisposed">Indicates whether subsequent calls to <see cref="GetDisposable"/> should throw when this instance is disposed.</param>
        /// <exception cref="ArgumentNullException"><paramref name="disposable"/> is null.</exception>
        public RefCountDisposable(IDisposable disposable, bool throwWhenDisposed)
        {
            if (disposable == null)
                throw new ArgumentNullException(nameof(disposable));

            _disposable = disposable;
            _isPrimaryDisposed = false;
            _count = 0;
            _throwWhenDisposed = throwWhenDisposed;
        }

        /// <summary>
        /// Gets a value that indicates whether the object is disposed.
        /// </summary>
        public bool IsDisposed => _disposable == null;

        /// <summary>
        /// Returns a dependent disposable that when disposed decreases the refcount on the underlying disposable.
        /// </summary>
        /// <returns>A dependent disposable contributing to the reference count that manages the underlying disposable's lifetime.</returns>
        /// <exception cref="ObjectDisposedException">This instance has been disposed and is configured to throw in this case by <see cref="RefCountDisposable(IDisposable, bool)"/>.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Backward compat + non-trivial work for a property getter.")]
        public IDisposable GetDisposable()
        {
            lock (_gate)
            {
                if (_disposable == null)
                {
                    if (_throwWhenDisposed)
                        throw new ObjectDisposedException("RefCountDisposable");

                    return Disposable.Empty;
                }
                else
                {
                    _count++;
                    return new InnerDisposable(this);
                }
            }
        }

        /// <summary>
        /// Disposes the underlying disposable only when all dependent disposables have been disposed.
        /// </summary>
        public void Dispose()
        {
            var disposable = default(IDisposable);
            lock (_gate)
            {
                if (_disposable != null)
                {
                    if (!_isPrimaryDisposed)
                    {
                        _isPrimaryDisposed = true;

                        if (_count == 0)
                        {
                            disposable = _disposable;
                            _disposable = null;
                        }
                    }
                }
            }

            disposable?.Dispose();
        }

        private void Release()
        {
            var disposable = default(IDisposable);
            lock (_gate)
            {
                if (_disposable != null)
                {
                    _count--;

                    System.Diagnostics.Debug.Assert(_count >= 0);

                    if (_isPrimaryDisposed)
                    {
                        if (_count == 0)
                        {
                            disposable = _disposable;
                            _disposable = null;
                        }
                    }
                }
            }

            disposable?.Dispose();
        }

        private sealed class InnerDisposable : IDisposable
        {
            private RefCountDisposable _parent;

            public InnerDisposable(RefCountDisposable parent)
            {
                _parent = parent;
            }

            public void Dispose()
            {
                Interlocked.Exchange(ref _parent, null)?.Release();
            }
        }
    }
}

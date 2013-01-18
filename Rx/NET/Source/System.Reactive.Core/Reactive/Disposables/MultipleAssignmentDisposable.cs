// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

namespace System.Reactive.Disposables
{
    /// <summary>
    /// Represents a disposable resource whose underlying disposable resource can be swapped for another disposable resource.
    /// </summary>
    public sealed class MultipleAssignmentDisposable : ICancelable
    {
        private readonly object _gate = new object();
        private IDisposable _current;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Reactive.Disposables.MultipleAssignmentDisposable"/> class with no current underlying disposable.
        /// </summary>
        public MultipleAssignmentDisposable()
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
                    // We use a sentinel value to indicate we've been disposed. This sentinel never leaks
                    // to the outside world (see the Disposable property getter), so no-one can ever assign
                    // this value to us manually.
                    return _current == BooleanDisposable.True;
                }
            }
        }

        /// <summary>
        /// Gets or sets the underlying disposable. After disposal, the result of getting this property is undefined.
        /// </summary>
        /// <remarks>If the MutableDisposable has already been disposed, assignment to this property causes immediate disposal of the given disposable object.</remarks>
        public IDisposable Disposable
        {
            get
            {
                lock (_gate)
                {
                    if (_current == BooleanDisposable.True /* see IsDisposed */)
                        return DefaultDisposable.Instance; // Don't leak the sentinel value.

                    return _current;
                }
            }

            set
            {
                var shouldDispose = false;
                lock (_gate)
                {
                    shouldDispose = (_current == BooleanDisposable.True /* see IsDisposed */);
                    if (!shouldDispose)
                    {
                        _current = value;
                    }
                }
                if (shouldDispose && value != null)
                    value.Dispose();
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
                if (_current != BooleanDisposable.True /* see IsDisposed */)
                {
                    old = _current;
                    _current = BooleanDisposable.True /* see IsDisposed */;
                }
            }

            if (old != null)
                old.Dispose();
        }
    }
}

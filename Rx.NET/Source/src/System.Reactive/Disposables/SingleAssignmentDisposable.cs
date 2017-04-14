// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Threading;

namespace System.Reactive.Disposables
{
    /// <summary>
    /// Represents a disposable resource which only allows a single assignment of its underlying disposable resource.
    /// If an underlying disposable resource has already been set, future attempts to set the underlying disposable resource will throw an <see cref="InvalidOperationException"/>.
    /// </summary>
    public sealed class SingleAssignmentDisposable : ICancelable
    {
        private volatile IDisposable _current;

        /// <summary>
        /// Initializes a new instance of the <see cref="SingleAssignmentDisposable"/> class.
        /// </summary>
        public SingleAssignmentDisposable()
        {
        }

        /// <summary>
        /// Gets a value that indicates whether the object is disposed.
        /// </summary>
        public bool IsDisposed
        {
            get
            {
                // We use a sentinel value to indicate we've been disposed. This sentinel never leaks
                // to the outside world (see the Disposable property getter), so no-one can ever assign
                // this value to us manually.
                return _current == BooleanDisposable.True;
            }
        }

        /// <summary>
        /// Gets or sets the underlying disposable. After disposal, the result of getting this property is undefined.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if the <see cref="SingleAssignmentDisposable"/> has already been assigned to.</exception>
        public IDisposable Disposable
        {
            get
            {
                var current = _current;

                if (current == BooleanDisposable.True)
                {
                    return DefaultDisposable.Instance; // Don't leak the sentinel value.
                }

                return current;
            }

            set
            {
                var old = Interlocked.CompareExchange(ref _current, value, null);
                if (old == null)
                    return;

                if (old != BooleanDisposable.True)
                    throw new InvalidOperationException(Strings_Core.DISPOSABLE_ALREADY_ASSIGNED);

                value?.Dispose();
            }
        }

        /// <summary>
        /// Disposes the underlying disposable.
        /// </summary>
        public void Dispose()
        {
            Interlocked.Exchange(ref _current, BooleanDisposable.True)?.Dispose();
        }
    }
}

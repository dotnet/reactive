// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Threading;

namespace System.Reactive.Disposables
{
    /// <summary>
    /// Represents a disposable resource whose underlying disposable resource can be replaced by another disposable resource, causing automatic disposal of the previous underlying disposable resource.
    /// </summary>
    public sealed class SerialDisposable : ICancelable
    {
        private IDisposable _current;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Reactive.Disposables.SerialDisposable"/> class.
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
                // We use a sentinel value to indicate we've been disposed. This sentinel never leaks
                // to the outside world (see the Disposable property getter), so no-one can ever assign
                // this value to us manually.
                return Volatile.Read(ref _current) == BooleanDisposable.True;
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
                var a = Volatile.Read(ref _current);
                // Don't leak the DISPOSED sentinel
                if (a == BooleanDisposable.True)
                {
                    a = null;
                }
                return a;
            }

            set
            {
                var copy = Volatile.Read(ref _current);
                for (;;)
                {
                    if (copy == BooleanDisposable.True)
                    {
                        value?.Dispose();
                        return;
                    }
                    var current = Interlocked.CompareExchange(ref _current, value, copy);
                    if (current == copy)
                    {
                        copy?.Dispose();
                        return;
                    }
                    copy = current;
                }
            }
        }

        /// <summary>
        /// Disposes the underlying disposable as well as all future replacements.
        /// </summary>
        public void Dispose()
        {
            var old = Interlocked.Exchange(ref _current, BooleanDisposable.True);
            old?.Dispose();
        }
    }
}

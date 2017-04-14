// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Threading;

namespace System.Reactive.Disposables
{
    /// <summary>
    /// Represents a disposable resource whose underlying disposable resource can be swapped for another disposable resource.
    /// </summary>
    public sealed class MultipleAssignmentDisposable : ICancelable
    {
        private IDisposable _current;

        /// <summary>
        /// Initializes a new instance of the <see cref="MultipleAssignmentDisposable"/> class with no current underlying disposable.
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
                // We use a sentinel value to indicate we've been disposed. This sentinel never leaks
                // to the outside world (see the Disposable property getter), so no-one can ever assign
                // this value to us manually.
                return Volatile.Read(ref _current) == BooleanDisposable.True;
            }
        }

        /// <summary>
        /// Gets or sets the underlying disposable. After disposal, the result of getting this property is undefined.
        /// </summary>
        /// <remarks>If the <see cref="MultipleAssignmentDisposable"/> has already been disposed, assignment to this property causes immediate disposal of the given disposable object.</remarks>
        public IDisposable Disposable
        {
            get
            {
                var a = Volatile.Read(ref _current);

                // Don't leak the DISPOSED sentinel
                if (a == BooleanDisposable.True)
                {
                    a = DefaultDisposable.Instance;
                }

                return a;
            }

            set
            {
                // Let's read the current value atomically (also prevents reordering).
                var old = Volatile.Read(ref _current);

                for (;;)
                {
                    // If it is the disposed instance, dispose the value.
                    if (old == BooleanDisposable.True)
                    {
                        value?.Dispose();
                        return;
                    }

                    // Atomically swap in the new value and get back the old.
                    var b = Interlocked.CompareExchange(ref _current, value, old);

                    // If the old and new are the same, the swap was successful and we can quit
                    if (old == b)
                    {
                        return;
                    }

                    // Otherwise, make the old reference the current and retry.
                    old = b;
                }
            }
        }

        /// <summary>
        /// Disposes the underlying disposable as well as all future replacements.
        /// </summary>
        public void Dispose()
        {
            // Read the current atomically.
            var a = Volatile.Read(ref _current);

            // If it is the disposed instance, don't bother further.
            if (a != BooleanDisposable.True)
            {
                // Atomically swap in the disposed instance.
                a = Interlocked.Exchange(ref _current, BooleanDisposable.True);

                // It is possible there was a concurrent Dispose call so don't need to call Dispose()
                // on DISPOSED
                if (a != BooleanDisposable.True)
                {
                    a?.Dispose();
                }
            }
        }
    }
}

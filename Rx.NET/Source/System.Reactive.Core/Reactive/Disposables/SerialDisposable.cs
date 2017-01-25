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
        private ISerialCancelable _current = new ActiveSerialDisposable();

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Reactive.Disposables.SerialDisposable"/> class.
        /// </summary>
        public SerialDisposable()
        {
        }

        /// <summary>
        /// Gets a value that indicates whether the object is disposed.
        /// </summary>
        public bool IsDisposed => _current.IsDisposed;

        /// <summary>
        /// Gets or sets the underlying disposable.
        /// </summary>
        /// <remarks>If the SerialDisposable has already been disposed, assignment to this property causes immediate disposal of the given disposable object. Assigning this property disposes the previous disposable object.</remarks>
        public IDisposable Disposable
        {
            get { return _current.Disposable; }
            set { _current.Disposable = value; }
        }

        /// <summary>
        /// Disposes the underlying disposable as well as all future replacements.
        /// </summary>
        public void Dispose()
        {
            var old = Interlocked.Exchange(ref _current, DisposedSerialDisposable.Instance);
            old.Dispose();
        }

        /// <summary>
        /// Private interface to allow substitution of two implementations based on the state of the serial disposable.
        /// </summary>
        private interface ISerialCancelable : ICancelable
        {
            IDisposable Disposable { get; set; }
        }
        
        /// <summary>
        /// Internal implementation of a <see cref="SerialDisposable"/> that has been disposed.
        /// </summary>
        /// <remarks>
        /// Is a singleton implementation as it does not maintain any state.
        /// </remarks>
        private sealed class DisposedSerialDisposable : ISerialCancelable
        {
            public static readonly DisposedSerialDisposable Instance = new DisposedSerialDisposable();

            private DisposedSerialDisposable()
            { }

            public bool IsDisposed => true;

            public IDisposable Disposable
            {
                get { return null; }
                set { value?.Dispose(); }
            }

            public void Dispose()
            { }
        }

        /// <summary>
        /// Internal implementation of a <see cref="SerialDisposable"/> that is still active.
        /// </summary>
        private sealed class ActiveSerialDisposable : ISerialCancelable
        {
            private IDisposable _disposable;

            public bool IsDisposed => false;

            public IDisposable Disposable
            {
                get { return _disposable; }
                set
                {
                    var old = Interlocked.Exchange(ref _disposable, value);
                    old?.Dispose();
                }
            }

            public void Dispose()
            {
                _disposable?.Dispose();
            }
        }
    }
}

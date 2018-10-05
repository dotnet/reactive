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
        public bool IsDisposed => Disposables.Disposable.GetIsDisposed(ref _current);

        /// <summary>
        /// Gets or sets the underlying disposable.
        /// </summary>
        /// <remarks>If the SerialDisposable has already been disposed, assignment to this property causes immediate disposal of the given disposable object. Assigning this property disposes the previous disposable object.</remarks>
        public IDisposable Disposable
        {
            get => Disposables.Disposable.GetValue(ref _current);
            set => Disposables.Disposable.TrySetSerial(ref _current, value);
        }

        /// <summary>
        /// Disposes the underlying disposable as well as all future replacements.
        /// </summary>
        public void Dispose()
        {
            Disposables.Disposable.TryDispose(ref _current);
        }
    }
}

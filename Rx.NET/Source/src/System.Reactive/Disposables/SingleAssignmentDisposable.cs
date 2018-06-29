// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 


namespace System.Reactive.Disposables
{
    /// <summary>
    /// Represents a disposable resource which only allows a single assignment of its underlying disposable resource.
    /// If an underlying disposable resource has already been set, future attempts to set the underlying disposable resource will throw an <see cref="InvalidOperationException"/>.
    /// </summary>
    public sealed class SingleAssignmentDisposable : ICancelable
    {
        private IDisposable _current;

        /// <summary>
        /// Initializes a new instance of the <see cref="SingleAssignmentDisposable"/> class.
        /// </summary>
        public SingleAssignmentDisposable()
        {
        }

        /// <summary>
        /// Gets a value that indicates whether the object is disposed.
        /// </summary>
        public bool IsDisposed => Disposables.Disposable.GetIsDisposed(ref _current);

        /// <summary>
        /// Gets or sets the underlying disposable. After disposal, the result of getting this property is undefined.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if the <see cref="SingleAssignmentDisposable"/> has already been assigned to.</exception>
        public IDisposable Disposable
        {
            get => Disposables.Disposable.GetValueOrDefault(ref _current);
            set => Disposables.Disposable.SetSingle(ref _current, value);
        }

        /// <summary>
        /// Disposes the underlying disposable.
        /// </summary>
        public void Dispose()
        {
            Disposables.Disposable.TryDispose(ref _current);
        }
    }
}

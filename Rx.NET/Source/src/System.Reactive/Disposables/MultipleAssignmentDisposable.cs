﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive.Disposables
{
    /// <summary>
    /// Represents a disposable resource whose underlying disposable resource can be swapped for another disposable resource.
    /// </summary>
    public sealed class MultipleAssignmentDisposable : ICancelable
    {
        private MultipleAssignmentDisposableValue _current;

        /// <summary>
        /// Initializes a new instance of the <see cref="MultipleAssignmentDisposable"/> class with no current underlying disposable.
        /// </summary>
        public MultipleAssignmentDisposable()
        {
        }

        /// <summary>
        /// Gets a value that indicates whether the object is disposed.
        /// </summary>
        public bool IsDisposed => _current.IsDisposed;

        /// <summary>
        /// Gets or sets the underlying disposable. After disposal, the result of getting this property is undefined.
        /// </summary>
        /// <remarks>If the <see cref="MultipleAssignmentDisposable"/> has already been disposed, assignment to this property causes immediate disposal of the given disposable object.</remarks>
        public IDisposable? Disposable
        {
            get => _current.Disposable;
            set => _current.Disposable = value;
        }

        /// <summary>
        /// Disposes the underlying disposable as well as all future replacements.
        /// </summary>
        public void Dispose()
        {
            _current.Dispose();
        }
    }
}

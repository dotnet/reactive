// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

namespace System.Reactive.Disposables
{
    /// <summary>
    /// Represents a disposable that does nothing on disposal.
    /// </summary>
    internal sealed class DefaultDisposable : IDisposable
    {
        /// <summary>
        /// Singleton default disposable.
        /// </summary>
        public static readonly DefaultDisposable Instance = new DefaultDisposable();

        private DefaultDisposable()
        {
        }

        /// <summary>
        /// Does nothing.
        /// </summary>
        public void Dispose()
        {
            // no op
        }
    }
}

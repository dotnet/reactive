// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System.Threading;

namespace System.Reactive.Disposables
{
    /// <summary>
    /// Represents a resource that could be safely disposed in thread safe manner if that resource is a disposable one.
    /// </summary>
    public sealed class SafeDisposable : IDisposable
    {
        private IDisposable disposable;

        /// <summary>
        /// Initializes a new instance of the <see cref="SafeDisposable"/> class.
        /// </summary>
        /// <param name="disposable">The disposable is null.</param>
        /// <exception cref="ArgumentNullException">disposable parameter is null.</exception>
        public SafeDisposable(IDisposable disposable)
        {
            if (disposable == null)
            {
                throw new ArgumentNullException("disposable");
            }

            this.disposable = disposable;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            var disposable = Interlocked.Exchange(ref this.disposable, null);

            if (disposable != null)
            {
                try
                {
                    disposable.Dispose();
                }
                catch
                {
                    // ignored
                }
            }
        }
    }
}

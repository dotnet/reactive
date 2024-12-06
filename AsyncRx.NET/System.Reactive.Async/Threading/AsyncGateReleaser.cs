// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive.Threading
{
    /// <summary>
    /// Returned by <see cref="IAsyncGate.LockAsync"/>, enabling the caller to release the lock.
    /// </summary>
    public struct AsyncGateReleaser : IDisposable
    {
        // Holds either an IAsyncGate or an IDisposable.
        // In the case where this is an IAsyncGate, it's important that we try to avoid
        // calling Release more than once, because this releaser is associated with just one
        // call to LockAsync. IDisposable implementations are expected to be idempotent,
        // so we need to remember when we've already made our one call to Release. (This
        // can't be perfect because this is a struct, so callers might end up copying
        // this value and then disposing each copy. But for normal using usage that won't
        // be a problem, and this provides a reasonable best-effort approach. It's why
        // this can't be a readonly struct though.)
        private object _parentOrDisposable;

        /// <summary>
        /// Creates an <see cref="AsyncGateReleaser"/> that calls <see cref="IAsyncGate.Release"/>
        /// on its parent when disposed.
        /// </summary>
        /// <param name="parent"></param>
        public AsyncGateReleaser(IAsyncGate parent) => _parentOrDisposable = parent;

        /// <summary>
        /// Creates an <see cref="AsyncGateReleaser"/> that calls another disposable when disposed.
        /// </summary>
        /// <param name="disposable">
        /// The <see cref="IDisposable"/> implementation to which to defer.
        /// </param>
        /// <remarks>
        /// This can be convenient for custom <see cref="IAsyncGate"/> implementations in that wrap
        /// some underlying lock implementation that returns an <see cref="IDisposable"/> as the means
        /// by which the lock is released.
        /// </remarks>
        public AsyncGateReleaser(IDisposable disposable) => _parentOrDisposable = disposable;

        public void Dispose()
        {
            switch (_parentOrDisposable)
            {
                case IDisposable d: d.Dispose(); break;
                case IAsyncGate g: g.Release(); break;
            }

            _parentOrDisposable = null;
        }
    }
}

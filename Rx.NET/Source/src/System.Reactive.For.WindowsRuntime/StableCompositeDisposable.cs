// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Threading;

namespace System.Reactive.WindowsRuntime
{
    /// <summary>
    /// Represents a group of disposable resources that are disposed together.
    /// </summary>
    /// <remarks>
    /// This is a copy from <c>System.Reactive</c> and then trimmed down to provide just the one
    /// feature <see cref="System.Reactive.Linq.WindowsRuntimeAsyncInfoObservable"/>
    /// needs: access to the internal CreateTrusted method. I didn't want to make that a
    /// public-facing part of the main library. (And <c>InternalsVisibleTo</c> effectively makes it
    /// somewhat public: it means changes to the internals could break this library.) Better, then
    /// to copy over just the functionality required in this library. 
    /// </remarks>
    internal abstract class StableUncheckedCompositeDisposable : IDisposable
    {
        /// <summary>
        /// Creates a group of disposable resources that are disposed together
        /// and without copying or checking for nulls inside the group.
        /// </summary>
        /// <param name="disposables">The array of disposables that is trusted
        /// to not contain nulls and gives no need to defensively copy it.</param>
        /// <returns>Group of disposable resources that are disposed together.</returns>
        internal static IDisposable CreateTrusted(params IDisposable[] disposables)
        {
            return new NAryTrustedArray(disposables);
        }

        /// <summary>
        /// Disposes all disposables in the group.
        /// </summary>
        public abstract void Dispose();

        /// <summary>
        /// A stable composite that doesn't do defensive copy of
        /// the input disposable array nor checks it for null.
        /// </summary>
        private sealed class NAryTrustedArray : StableUncheckedCompositeDisposable
        {
            private IDisposable[]? _disposables;

            public NAryTrustedArray(IDisposable[] disposables)
            {
                Volatile.Write(ref _disposables, disposables);
            }

            public override void Dispose()
            {
                var old = Interlocked.Exchange(ref _disposables, null);
                if (old != null)
                {
                    foreach (var d in old)
                    {
                        d.Dispose();
                    }
                }
            }
        }
    }
}

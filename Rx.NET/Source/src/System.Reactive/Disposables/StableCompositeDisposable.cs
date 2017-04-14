// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Threading;

namespace System.Reactive.Disposables
{
    /// <summary>
    /// Represents a group of disposable resources that are disposed together.
    /// </summary>
    public abstract class StableCompositeDisposable : ICancelable
    {
        /// <summary>
        /// Creates a new group containing two disposable resources that are disposed together.
        /// </summary>
        /// <param name="disposable1">The first disposable resoruce to add to the group.</param>
        /// <param name="disposable2">The second disposable resoruce to add to the group.</param>
        /// <returns>Group of disposable resources that are disposed together.</returns>
        public static ICancelable Create(IDisposable disposable1, IDisposable disposable2)
        {
            if (disposable1 == null)
                throw new ArgumentNullException(nameof(disposable1));
            if (disposable2 == null)
                throw new ArgumentNullException(nameof(disposable2));

            return new Binary(disposable1, disposable2);
        }

        /// <summary>
        /// Creates a new group of disposable resources that are disposed together.
        /// </summary>
        /// <param name="disposables">Disposable resources to add to the group.</param>
        /// <returns>Group of disposable resources that are disposed together.</returns>
        public static ICancelable Create(params IDisposable[] disposables)
        {
            if (disposables == null)
                throw new ArgumentNullException(nameof(disposables));

            return new NAry(disposables);
        }

        /// <summary>
        /// Creates a new group of disposable resources that are disposed together.
        /// </summary>
        /// <param name="disposables">Disposable resources to add to the group.</param>
        /// <returns>Group of disposable resources that are disposed together.</returns>
        public static ICancelable Create(IEnumerable<IDisposable> disposables)
        {
            if (disposables == null)
                throw new ArgumentNullException(nameof(disposables));

            return new NAry(disposables);
        }

        /// <summary>
        /// Disposes all disposables in the group.
        /// </summary>
        public abstract void Dispose();

        /// <summary>
        /// Gets a value that indicates whether the object is disposed.
        /// </summary>
        public abstract bool IsDisposed
        {
            get;
        }

        private sealed class Binary : StableCompositeDisposable
        {
            private volatile IDisposable _disposable1;
            private volatile IDisposable _disposable2;

            public Binary(IDisposable disposable1, IDisposable disposable2)
            {
                _disposable1 = disposable1;
                _disposable2 = disposable2;
            }

            public override bool IsDisposed => _disposable1 == null;

            public override void Dispose()
            {
                Interlocked.Exchange(ref _disposable1, null)?.Dispose();
                Interlocked.Exchange(ref _disposable2, null)?.Dispose();
            }
        }

        private sealed class NAry : StableCompositeDisposable
        {
            private volatile List<IDisposable> _disposables;

            public NAry(IDisposable[] disposables)
                : this((IEnumerable<IDisposable>)disposables)
            {
            }

            public NAry(IEnumerable<IDisposable> disposables)
            {
                _disposables = new List<IDisposable>(disposables);

                //
                // Doing this on the list to avoid duplicate enumeration of disposables.
                //
                if (_disposables.Contains(null))
                    throw new ArgumentException(Strings_Core.DISPOSABLES_CANT_CONTAIN_NULL, nameof(disposables));
            }

            public override bool IsDisposed => _disposables == null;

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

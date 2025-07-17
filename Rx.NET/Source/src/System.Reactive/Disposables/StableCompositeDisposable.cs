// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Threading;

namespace System.Reactive.Disposables
{
    /// <summary>
    /// Represents a group of disposable resources that are disposed together.
    /// </summary>
#pragma warning disable CA1063 // (Overridable IDisposable.) This analyzer wants us to make breaking changes to its public API, which we can't do.
    public abstract class StableCompositeDisposable : ICancelable
    {
        /// <summary>
        /// Creates a new group containing two disposable resources that are disposed together.
        /// </summary>
        /// <param name="disposable1">The first disposable resource to add to the group.</param>
        /// <param name="disposable2">The second disposable resource to add to the group.</param>
        /// <returns>Group of disposable resources that are disposed together.</returns>
        public static ICancelable Create(IDisposable disposable1, IDisposable disposable2)
        {
            if (disposable1 == null)
            {
                throw new ArgumentNullException(nameof(disposable1));
            }

            if (disposable2 == null)
            {
                throw new ArgumentNullException(nameof(disposable2));
            }

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
            {
                throw new ArgumentNullException(nameof(disposables));
            }

            return new NAryArray(disposables);
        }

        /// <summary>
        /// Creates a group of disposable resources that are disposed together
        /// and without copying or checking for nulls inside the group.
        /// </summary>
        /// <param name="disposables">The array of disposables that is trusted
        /// to not contain nulls and gives no need to defensively copy it.</param>
        /// <returns>Group of disposable resources that are disposed together.</returns>
        internal static ICancelable CreateTrusted(params IDisposable[] disposables)
        {
            return new NAryTrustedArray(disposables);
        }

        /// <summary>
        /// Creates a new group of disposable resources that are disposed together.
        /// </summary>
        /// <param name="disposables">Disposable resources to add to the group.</param>
        /// <returns>Group of disposable resources that are disposed together.</returns>
        public static ICancelable Create(IEnumerable<IDisposable> disposables)
        {
            if (disposables == null)
            {
                throw new ArgumentNullException(nameof(disposables));
            }

            return new NAryEnumerable(disposables);
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
            private SingleAssignmentDisposableValue _disposable1;
            private SingleAssignmentDisposableValue _disposable2;

            public Binary(IDisposable disposable1, IDisposable disposable2)
            {
                _disposable1.Disposable = disposable1;
                _disposable2.Disposable = disposable2;
            }

            public override bool IsDisposed => _disposable1.IsDisposed;

            public override void Dispose()
            {
                _disposable1.Dispose();
                _disposable2.Dispose();
            }
        }

        private sealed class NAryEnumerable : StableCompositeDisposable
        {
            private volatile List<IDisposable>? _disposables;

            public NAryEnumerable(IEnumerable<IDisposable> disposables)
            {
                _disposables = new List<IDisposable>(disposables);

                //
                // Doing this on the list to avoid duplicate enumeration of disposables.
                //
                if (_disposables.Contains(null!))
                {
                    throw new ArgumentException(Strings_Core.DISPOSABLES_CANT_CONTAIN_NULL, nameof(disposables));
                }
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

        private sealed class NAryArray : StableCompositeDisposable
        {
            private IDisposable[]? _disposables;

            public NAryArray(IDisposable[] disposables)
            {
                if (Array.IndexOf(disposables, null!) != -1)
                {
                    throw new ArgumentException(Strings_Core.DISPOSABLES_CANT_CONTAIN_NULL, nameof(disposables));
                }

                var n = disposables.Length;
                var ds = new IDisposable[n];

                Array.Copy(disposables, 0, ds, 0, n);

                Volatile.Write(ref _disposables, ds);
            }

            public override bool IsDisposed => Volatile.Read(ref _disposables) == null;

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

        /// <summary>
        /// A stable composite that doesn't do defensive copy of
        /// the input disposable array nor checks it for null.
        /// </summary>
        private sealed class NAryTrustedArray : StableCompositeDisposable
        {
            private IDisposable[]? _disposables;

            public NAryTrustedArray(IDisposable[] disposables)
            {
                Volatile.Write(ref _disposables, disposables);
            }

            public override bool IsDisposed => Volatile.Read(ref _disposables) == null;

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
#pragma warning restore CA1063
}

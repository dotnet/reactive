// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Threading;

namespace System.Linq
{
    /// <summary>
    /// Provides a set of extension methods for <see cref="IAsyncEnumerable{T}"/>.
    /// </summary>
    public static partial class AsyncEnumerable
    {
        //
        // REVIEW: Create methods may not belong in System.Linq.Async. Async iterators can be
        //         used to implement these interfaces. Move to System.Interactive.Async?
        //

        /// <summary>
        /// Creates a new enumerable using the specified delegates implementing the members of <see cref="IAsyncEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements returned by the enumerable sequence.</typeparam>
        /// <param name="getAsyncEnumerator">The delegate implementing the <see cref="IAsyncEnumerable{T}.GetAsyncEnumerator"/> method.</param>
        /// <returns>A new enumerable instance.</returns>
        public static IAsyncEnumerable<T> Create<T>(Func<CancellationToken, IAsyncEnumerator<T>> getAsyncEnumerator)
        {
            if (getAsyncEnumerator == null)
                throw Error.ArgumentNull(nameof(getAsyncEnumerator));

            return new AnonymousAsyncEnumerable<T>(getAsyncEnumerator);
        }

        private sealed class AnonymousAsyncEnumerable<T> : IAsyncEnumerable<T>
        {
            private readonly Func<CancellationToken, IAsyncEnumerator<T>> _getEnumerator;

            public AnonymousAsyncEnumerable(Func<CancellationToken, IAsyncEnumerator<T>> getEnumerator) => _getEnumerator = getEnumerator;

            public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested(); // NB: [LDM-2018-11-28] Equivalent to async iterator behavior.

                return _getEnumerator(cancellationToken);
            }
        }
    }
}

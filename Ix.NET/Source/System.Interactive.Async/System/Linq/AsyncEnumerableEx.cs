// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Threading;

namespace System.Linq
{
    /// <summary>
    /// Provides an additional set of extension methods for writing in-memory queries, transformations of async-enumerable sequences.
    /// </summary>
    /// <seealso cref="AsyncEnumerable"/>
    public static partial class AsyncEnumerableEx
    {
        // NOTE:  This was originally in System.Linq.Async, but was moved here because we are
        //         deprecating that package. It's not clear if this is still useful: there was
        //         a REVIEW comment against this code saying "Async iterators can be
        //         used to implement these interfaces." We retain this mainly so that anyone
        //         who was using it can still have it. Unfortunately there's no way to do that
        //         without introducing a source-level breaking change: we've had to move it out
        //         of AsyncEnumerable because we can't have System.Linq.Async's public API defining
        //         a class of that name. (It causes amibuigous type errors if you try to invoke
        //         a static method such as AsyncEnumerable.Range: even if System.Linq.Async doesn't
        //         define Range on its AsyncEnumerable, the compiler chokes on the fact that there
        //         are two AsyncEnumerable types.)

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

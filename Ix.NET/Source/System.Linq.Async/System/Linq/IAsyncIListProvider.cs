// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    /// <summary>
    /// An iterator that can produce an array or <see cref="List{TElement}"/> through an optimized path.
    /// </summary>
    /// <remarks>
    /// This interface is primarily used for internal purposes as an optimization for LINQ operators. It was made public because
    /// it was defined in the <c>System.Linq.Async</c> package but also used in <c>System.Interactive.Async</c>. Now that
    /// <c>System.Linq.Async</c> is being retired in favor of .NET 10.0's <c>System.Linq.AsyncEnumerable</c>, the
    /// <c>System.Interactive.Async</c> package no longer takes a dependency on <c>System.Linq.Async</c>, and therefore defines
    /// its own version of this interface. We can't replace this with a type forwarder here because that would risk creating a
    /// circular dependency in cases where an application managed to get out-of-sync versions of the two packages.
    /// </remarks>
    [Obsolete("This interface was always unsupported, and the IAsyncEnumerable<T> LINQ implementation in System.Linq.AsyncEnumerable does not recognize it, so this no longer serves a purpose")]
    public interface IAsyncIListProvider<TElement> : IAsyncEnumerable<TElement>
    {
        /// <summary>
        /// Produce an array of the sequence through an optimized path.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>The array.</returns>
        ValueTask<TElement[]> ToArrayAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Produce a <see cref="List{TElement}"/> of the sequence through an optimized path.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>The <see cref="List{TElement}"/>.</returns>
        ValueTask<List<TElement>> ToListAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Returns the count of elements in the sequence.
        /// </summary>
        /// <param name="onlyIfCheap">If true then the count should only be calculated if doing
        /// so is quick (sure or likely to be constant time), otherwise -1 should be returned.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>The number of elements.</returns>
        ValueTask<int> GetCountAsync(bool onlyIfCheap, CancellationToken cancellationToken);
    }
}

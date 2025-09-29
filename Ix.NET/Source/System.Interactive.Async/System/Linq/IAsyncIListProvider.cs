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
    /// This interface is primarily used for internal purposes as an optimization for LINQ operators. Its use is discouraged.
    /// It was made public because it was originally defined in the <c>System.Linq.Async</c> package but also used in
    /// <c>System.Interactive.Async</c>. Now that <c>System.Linq.Async</c> is being retired in favor of .NET 10.0's
    /// <c>System.Linq.AsyncEnumerable</c>, the <c>System.Interactive.Async</c> package no longer takes a dependency on
    /// <c>System.Linq.Async</c>, which is why it now defines its own version of this interface here. We can't put a type
    /// forwarder in <c>System.Interactive.Async</c> to here because that would risk creating a circular dependency in
    /// cases where an application managed to get out-of-sync versions of the two packages, so this interface is not
    /// backwards compatible with the old one. If you were implementing this in your own types to get the associated
    /// optimizations, be aware that this is not supported, but implementing this copy of the interface (in place of
    /// the old version defined in the deprecated <c>System.Linq.Async</c> package) will continue to provide the
    /// same (unsupported) behaviour.
    /// </remarks>
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

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    /// <summary>
    /// An iterator that can produce an array or <see cref="List{TElement}"/> through an optimized path.
    /// </summary>
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    /// <summary>
    /// An iterator that can produce an array or <see cref="List{TElement}"/> through an optimized path.
    /// </summary>
    interface IIListProvider<TElement> : IAsyncEnumerable<TElement>
    {
        /// <summary>
        /// Produce an array of the sequence through an optimized path.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>The array.</returns>
        Task<TElement[]> ToArrayAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Produce a <see cref="List{TElement}"/> of the sequence through an optimized path.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>The <see cref="List{TElement}"/>.</returns>
        Task<List<TElement>> ToListAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Returns the count of elements in the sequence.
        /// </summary>
        /// <param name="onlyIfCheap">If true then the count should only be calculated if doing
        /// so is quick (sure or likely to be constant time), otherwise -1 should be returned.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>The number of elements.</returns>
        Task<int> GetCountAsync(bool onlyIfCheap, CancellationToken cancellationToken);
    }
}

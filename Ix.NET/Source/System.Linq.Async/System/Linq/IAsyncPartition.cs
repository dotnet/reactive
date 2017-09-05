// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Threading.Tasks;

namespace System.Linq
{
    /// <summary>
    /// An iterator that supports random access and can produce a partial sequence of its items through an optimized path.
    /// </summary>
    internal interface IAsyncPartition<TElement> : IAsyncIListProvider<TElement>
    {
        /// <summary>
        /// Creates a new partition that skips the specified number of elements from this sequence.
        /// </summary>
        /// <param name="count">The number of elements to skip.</param>
        /// <returns>An <see cref="IPartition{TElement}"/> with the first <paramref name="count"/> items removed.</returns>
        IAsyncPartition<TElement> Skip(int count);

        /// <summary>
        /// Creates a new partition that takes the specified number of elements from this sequence.
        /// </summary>
        /// <param name="count">The number of elements to take.</param>
        /// <returns>An <see cref="IPartition{TElement}"/> with only the first <paramref name="count"/> items.</returns>
        IAsyncPartition<TElement> Take(int count);

        /// <summary>
        /// Gets the item associated with a 0-based index in this sequence.
        /// </summary>
        /// <param name="index">The 0-based index to access.</param>
        /// <returns>The element if found, otherwise, the default value of <see cref="TElement"/>.</returns>
        Task<Maybe<TElement>> TryGetElementAsync(int index);

        /// <summary>
        /// Gets the first item in this sequence.
        /// </summary>
        /// <returns>The element if found, otherwise, the default value of <see cref="TElement"/>.</returns>
        Task<Maybe<TElement>> TryGetFirstAsync();

        /// <summary>
        /// Gets the last item in this sequence.
        /// </summary>
        /// <returns>The element if found, otherwise, the default value of <see cref="TElement"/>.</returns>
        Task<Maybe<TElement>> TryGetLastAsync();
    }
}

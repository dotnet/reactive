// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Threading;
using System.Threading.Tasks;

namespace System.Collections.Generic
{
    /// <summary>
    ///     Asynchronous version of the IEnumerator&lt;T&gt; interface, allowing elements to be
    ///     retrieved asynchronously.
    /// </summary>
    /// <typeparam name="T">Element type.</typeparam>
    public interface IAsyncEnumerator<out T> : IDisposable
    {
        /// <summary>
        ///     Gets the current element in the iteration.
        /// </summary>
        T Current { get; }

        /// <summary>
        ///     Advances the enumerator to the next element in the sequence, returning the result asynchronously.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token that can be used to cancel the operation.</param>
        /// <returns>
        ///     Task containing the result of the operation: true if the enumerator was successfully advanced
        ///     to the next element; false if the enumerator has passed the end of the sequence.
        /// </returns>
        Task<bool> MoveNext(CancellationToken cancellationToken);
    }
}
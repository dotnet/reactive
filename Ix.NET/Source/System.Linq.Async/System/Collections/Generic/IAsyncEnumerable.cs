// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

// See https://github.com/dotnet/csharplang/blob/master/proposals/async-streams.md for the definition of this interface
// and the design rationale. (8/30/2017)

#if !HAS_ASYNCENUMERABLE

using System.Threading;

namespace System.Collections.Generic
{
    /// <summary>
    /// Asynchronous version of the <see cref="IEnumerable{T}"/> interface, allowing elements of the enumerable sequence to be retrieved asynchronously.
    /// </summary>
    /// <typeparam name="T">Element type.</typeparam>
    public interface IAsyncEnumerable<out T>
    {
        /// <summary>
        /// Gets an asynchronous enumerator over the sequence.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token used to cancel the enumeration.</param>
        /// <returns>Enumerator for asynchronous enumeration over the sequence.</returns>
        IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default);
    }
}

#else
using System.Runtime.CompilerServices;

[assembly: TypeForwardedTo(typeof(System.Collections.Generic.IAsyncEnumerable<>))]

#endif

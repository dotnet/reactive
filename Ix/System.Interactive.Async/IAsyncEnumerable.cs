// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.
using System;

namespace System.Collections.Generic
{
    /// <summary>
    /// Asynchronous version of the IEnumerable&lt;T&gt; interface, allowing elements of the
    /// enumerable sequence to be retrieved asynchronously.
    /// </summary>
    /// <typeparam name="T">Element type.</typeparam>
    public interface IAsyncEnumerable<
#if DESKTOPCLR40 || SILVERLIGHT4
        out
#endif
        T>
    {
        /// <summary>
        /// Gets an asynchronous enumerator over the sequence.
        /// </summary>
        /// <returns>Enumerator for asynchronous enumeration over the sequence.</returns>
        IAsyncEnumerator<T> GetEnumerator();
    }
}

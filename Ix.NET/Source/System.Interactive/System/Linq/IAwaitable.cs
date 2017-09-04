// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

namespace System.Linq
{
    /// <summary>
    /// Interface for objects that can be awaited for the completion of an asynchronous operation.
    /// </summary>
    public interface IAwaitable
    {
        /// <summary>
        /// Gets an awaiter object.
        /// </summary>
        /// <returns>Awaiter object.</returns>
        IAwaiter GetAwaiter();
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

namespace System.Linq
{
    /// <summary>
    /// Interface for yielding elements to enumerator.
    /// </summary>
    /// <typeparam name="T">Type of the elements yielded to an enumerator.</typeparam>
    public interface IYielder<in T>
    {
        /// <summary>
        /// Stops the enumeration.
        /// </summary>
        /// <returns>Awaitable object for use in an asynchronous method.</returns>
        IAwaitable Break();

        /// <summary>
        /// Yields a value to the enumerator.
        /// </summary>
        /// <param name="value">Value to yield return.</param>
        /// <returns>Awaitable object for use in an asynchronous method.</returns>
        IAwaitable Return(T value);
    }
}

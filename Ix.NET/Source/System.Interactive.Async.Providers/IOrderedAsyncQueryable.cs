// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 
namespace System.Linq
{
    /// <summary>
    /// Ordered asynchronous enumerable sequence represented by an expression tree.
    /// </summary>
    public interface IOrderedAsyncQueryable : IAsyncQueryable
    {
    }

    /// <summary>
    /// Ordered asynchronous enumerable sequence represented by an expression tree.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    public interface IOrderedAsyncQueryable<
#if !NO_VARIANCE
out
#endif
        T> : IAsyncQueryable<T>
    {
    }
}
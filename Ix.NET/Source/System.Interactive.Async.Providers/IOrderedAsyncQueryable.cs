// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.
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
    public interface IOrderedAsyncQueryable<T> : IAsyncQueryable<T>
    {
    }
}
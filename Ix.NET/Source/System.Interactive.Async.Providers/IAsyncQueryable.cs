// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.
using System.Collections.Generic;
using System.Linq.Expressions;

namespace System.Linq
{
    /// <summary>
    /// Asynchronous enumerable sequence represented by an expression tree.
    /// </summary>
    public interface IAsyncQueryable
    {
        /// <summary>
        /// Gets the type of the elements in the sequence.
        /// </summary>
        Type ElementType { get; }

        /// <summary>
        /// Gets the expression representing the sequence.
        /// </summary>
        Expression Expression { get; }

        /// <summary>
        /// Gets the query provider used to execute the sequence.
        /// </summary>
        IAsyncQueryProvider Provider { get; }
    }

    /// <summary>
    /// Asynchronous enumerable sequence represented by an expression tree.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    public interface IAsyncQueryable<
#if !NO_VARIANCE
out
#endif
        T> : IAsyncEnumerable<T>, IAsyncQueryable
    {
    }
}
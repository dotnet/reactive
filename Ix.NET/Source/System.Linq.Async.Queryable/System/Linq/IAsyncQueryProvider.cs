// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    /// <summary>
    /// Represents a query provider for asynchronous enumerable sequences.
    /// </summary>
    public interface IAsyncQueryProvider
    {
        /// <summary>
        /// Creates a new asynchronous enumerable sequence represented by an expression tree.
        /// </summary>
        /// <typeparam name="TElement">The type of the elements in the sequence.</typeparam>
        /// <param name="expression">The expression tree representing the asynchronous enumerable sequence.</param>
        /// <returns>Asynchronous enumerable sequence represented by the specified expression tree.</returns>
        IAsyncQueryable<TElement> CreateQuery<TElement>(Expression expression);

        /// <summary>
        /// Executes an expression tree representing a computation over asynchronous enumerable sequences.
        /// </summary>
        /// <typeparam name="TResult">The type of the result of evaluating the expression tree.</typeparam>
        /// <param name="expression">The expression tree to evaluate.</param>
        /// <param name="token">Cancellation token used to cancel the evaluation.</param>
        /// <returns>Task representing the result of evaluating the specified expression tree.</returns>
        ValueTask<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken token);
    }
}

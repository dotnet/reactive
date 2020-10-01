// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Linq.Expressions;

namespace System.Reactive.Joins
{
    /// <summary>
    /// Abstract base class for join patterns represented by an expression tree.
    /// </summary>
    public abstract class QueryablePattern
    {
        /// <summary>
        /// Creates a new join pattern object using the specified expression tree representation.
        /// </summary>
        /// <param name="expression">Expression tree representing the join pattern.</param>
        protected QueryablePattern(Expression expression)
        {
            Expression = expression;
        }

        /// <summary>
        /// Gets the expression tree representing the join pattern.
        /// </summary>
        public Expression Expression { get; }
    }
}

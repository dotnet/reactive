// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Linq.Expressions;

namespace System.Reactive.Joins
{
    /// <summary>
    /// Represents an execution plan for join patterns represented by an expression tree.
    /// </summary>
    /// <typeparam name="TResult">The type of the results produced by the plan.</typeparam>
    public class QueryablePlan<TResult>
    {
        internal QueryablePlan(Expression expression)
        {
            Expression = expression;
        }

        /// <summary>
        /// Gets the expression tree representing the join pattern execution plan.
        /// </summary>
        public Expression Expression { get; }
    }
}

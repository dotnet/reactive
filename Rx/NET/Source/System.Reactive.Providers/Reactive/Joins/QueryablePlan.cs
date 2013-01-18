// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#pragma warning disable 1591

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
        public Expression Expression { get; private set; }
    }
}

#pragma warning restore 1591
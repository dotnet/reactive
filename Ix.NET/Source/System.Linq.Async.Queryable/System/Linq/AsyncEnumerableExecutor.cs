// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    /// <summary>
    /// Provides functionality to evaluate an expression tree representation of a computation over asynchronous enumerable sequences.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    internal class AsyncEnumerableExecutor<T>
    {
        private readonly Expression _expression;
        private Func<CancellationToken, ValueTask<T>>? _func;

        /// <summary>
        /// Creates a new execution helper instance for the specified expression tree representing a computation over asynchronous enumerable sequences.
        /// </summary>
        /// <param name="expression">Expression tree representing a computation over asynchronous enumerable sequences.</param>
        public AsyncEnumerableExecutor(Expression expression)
        {
            _expression = expression;
        }

        /// <summary>
        /// Evaluated the expression tree.
        /// </summary>
        /// <param name="token">Token to cancel the evaluation.</param>
        /// <returns>Task representing the evaluation of the expression tree.</returns>
        internal ValueTask<T> ExecuteAsync(CancellationToken token)
        {
            if (_func == null)
            {
                var expression = Expression.Lambda<Func<CancellationToken, ValueTask<T>>>(new AsyncEnumerableRewriter().Visit(_expression), Expression.Parameter(typeof(CancellationToken)));
                _func = expression.Compile();
            }

            return _func(token);
        }
    }
}

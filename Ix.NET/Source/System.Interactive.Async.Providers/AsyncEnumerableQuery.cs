// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    /// <summary>
    /// Representation of an asynchronous enumerable sequence using an expression tree.
    /// </summary>
    internal abstract class AsyncEnumerableQuery
    {
        /// <summary>
        /// Gets the enumerable sequence obtained from evaluating the expression tree.
        /// </summary>
        internal abstract object Enumerable { get; }

        /// <summary>
        /// Gets the expression tree representing the asynchronous enumerable sequence.
        /// </summary>
        internal abstract Expression Expression { get; }
    }

    /// <summary>
    /// Representation of an asynchronous enumerable sequence using an expression tree.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    internal class AsyncEnumerableQuery<T> : AsyncEnumerableQuery, IOrderedAsyncQueryable<T>, IAsyncQueryProvider
    {
        private readonly Expression _expression;
        private IAsyncEnumerable<T> _enumerable;

        /// <summary>
        /// Creates a new asynchronous enumerable sequence represented by the specified expression tree.
        /// </summary>
        /// <param name="expression">The expression tree representing the asynchronous enumerable sequence.</param>
        public AsyncEnumerableQuery(Expression expression)
        {
            _expression = expression;
        }

        /// <summary>
        /// Creates a new asynchronous enumerable sequence by wrapping the specified sequence in an expression tree representation.
        /// </summary>
        /// <param name="enumerable">The asynchronous enumerable sequence to represent using an expression tree.</param>
        public AsyncEnumerableQuery(IAsyncEnumerable<T> enumerable)
        {
            _enumerable = enumerable;
            _expression = Expression.Constant(this);
        }

        /// <summary>
        /// Gets the type of the elements in the sequence.
        /// </summary>
        Type IAsyncQueryable.ElementType
        {
            get
            {
                return typeof(T);
            }
        }

        /// <summary>
        /// Gets the expression representing the sequence.
        /// </summary>
        Expression IAsyncQueryable.Expression
        {
            get
            {
                return _expression;
            }
        }

        /// <summary>
        /// Gets the query provider used to execute the sequence.
        /// </summary>
        IAsyncQueryProvider IAsyncQueryable.Provider
        {
            get
            {
                return this;
            }
        }

        /// <summary>
        /// Gets the enumerable sequence obtained from evaluating the expression tree.
        /// </summary>
        internal override object Enumerable
        {
            get
            {
                return _enumerable;
            }
        }

        /// <summary>
        /// Gets the expression tree representing the asynchronous enumerable sequence.
        /// </summary>
        internal override Expression Expression
        {
            get
            {
                return _expression;
            }
        }

        /// <summary>
        /// Creates a new asynchronous enumerable sequence represented by an expression tree.
        /// </summary>
        /// <typeparam name="TElement">The type of the elements in the sequence.</typeparam>
        /// <param name="expression">The expression tree representing the asynchronous enumerable sequence.</param>
        /// <returns>Asynchronous enumerable sequence represented by the specified expression tree.</returns>
        IAsyncQueryable<TElement> IAsyncQueryProvider.CreateQuery<TElement>(Expression expression)
        {
            return new AsyncEnumerableQuery<TElement>(expression);
        }

        /// <summary>
        /// Executes an expression tree representing a computation over asynchronous enumerable sequences.
        /// </summary>
        /// <typeparam name="TResult">The type of the result of evaluating the expression tree.</typeparam>
        /// <param name="expression">The expression tree to evaluate.</param>
        /// <param name="token">Cancellation token used to cancel the evaluation.</param>
        /// <returns>Task representing the result of evaluating the specified expression tree.</returns>
        Task<TResult> IAsyncQueryProvider.ExecuteAsync<TResult>(Expression expression, CancellationToken token)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            if (!typeof(Task<TResult>).IsAssignableFrom(expression.Type))
            {
                throw new ArgumentException("The specified expression is not assignable to the result type.", nameof(expression));
            }

            return new AsyncEnumerableExecutor<TResult>(expression).ExecuteAsync(token);
        }

        /// <summary>
        /// Gets an enumerator to enumerate the elements in the sequence.
        /// </summary>
        /// <returns>A new enumerator instance used to enumerate the elements in the sequence.</returns>
        public IAsyncEnumerator<T> GetEnumerator()
        {
            if (_enumerable == null)
            {
                var expression = Expression.Lambda<Func<IAsyncEnumerable<T>>>(new AsyncEnumerableRewriter().Visit(_expression), null);
                _enumerable = expression.Compile()();
            }

            return _enumerable.GetEnumerator();
        }

        /// <summary>
        /// Gets a string representation of the enumerable sequence.
        /// </summary>
        /// <returns>String representation of the enumerable sequence.</returns>
        public override string ToString()
        {
            if (!(_expression is ConstantExpression ce) || ce.Value != this)
            {
                return _expression.ToString();
            }

            if (_enumerable != null)
            {
                return _enumerable.ToString();
            }

            return "null";
        }
    }
}

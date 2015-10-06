// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
	public static partial class AsyncQueryable
	{
		public static Task<TSource> Aggregate<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TSource, TSource>> accumulator)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (accumulator == null)
				throw new ArgumentNullException("accumulator");

			return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, accumulator), CancellationToken.None);
		}

		public static Task<TSource> Aggregate<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TSource, TSource>> accumulator, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (accumulator == null)
				throw new ArgumentNullException("accumulator");

			return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, accumulator, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<TAccumulate> Aggregate<TSource, TAccumulate>(this IAsyncQueryable<TSource> source, TAccumulate seed, Expression<Func<TAccumulate, TSource, TAccumulate>> accumulator)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (accumulator == null)
				throw new ArgumentNullException("accumulator");

			return source.Provider.ExecuteAsync<TAccumulate>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TAccumulate)), source.Expression, Expression.Constant(seed, typeof(TAccumulate)), accumulator), CancellationToken.None);
		}

		public static Task<TAccumulate> Aggregate<TSource, TAccumulate>(this IAsyncQueryable<TSource> source, TAccumulate seed, Expression<Func<TAccumulate, TSource, TAccumulate>> accumulator, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (accumulator == null)
				throw new ArgumentNullException("accumulator");

			return source.Provider.ExecuteAsync<TAccumulate>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TAccumulate)), source.Expression, Expression.Constant(seed, typeof(TAccumulate)), accumulator, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<TResult> Aggregate<TSource, TAccumulate, TResult>(this IAsyncQueryable<TSource> source, TAccumulate seed, Expression<Func<TAccumulate, TSource, TAccumulate>> accumulator, Expression<Func<TAccumulate, TResult>> resultSelector)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (accumulator == null)
				throw new ArgumentNullException("accumulator");
			if (resultSelector == null)
				throw new ArgumentNullException("resultSelector");

			return source.Provider.ExecuteAsync<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TAccumulate), typeof(TResult)), source.Expression, Expression.Constant(seed, typeof(TAccumulate)), accumulator, resultSelector), CancellationToken.None);
		}

		public static Task<TResult> Aggregate<TSource, TAccumulate, TResult>(this IAsyncQueryable<TSource> source, TAccumulate seed, Expression<Func<TAccumulate, TSource, TAccumulate>> accumulator, Expression<Func<TAccumulate, TResult>> resultSelector, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (accumulator == null)
				throw new ArgumentNullException("accumulator");
			if (resultSelector == null)
				throw new ArgumentNullException("resultSelector");

			return source.Provider.ExecuteAsync<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TAccumulate), typeof(TResult)), source.Expression, Expression.Constant(seed, typeof(TAccumulate)), accumulator, resultSelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<bool> All<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (predicate == null)
				throw new ArgumentNullException("predicate");

			return source.Provider.ExecuteAsync<bool>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate), CancellationToken.None);
		}

		public static Task<bool> All<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (predicate == null)
				throw new ArgumentNullException("predicate");

			return source.Provider.ExecuteAsync<bool>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<bool> Any<TSource>(this IAsyncQueryable<TSource> source)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<bool>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression), CancellationToken.None);
		}

		public static Task<bool> Any<TSource>(this IAsyncQueryable<TSource> source, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<bool>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<bool> Any<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (predicate == null)
				throw new ArgumentNullException("predicate");

			return source.Provider.ExecuteAsync<bool>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate), CancellationToken.None);
		}

		public static Task<bool> Any<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (predicate == null)
				throw new ArgumentNullException("predicate");

			return source.Provider.ExecuteAsync<bool>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<double> Average(this IAsyncQueryable<int> source)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<double>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression), CancellationToken.None);
		}

		public static Task<double?> Average(this IAsyncQueryable<int?> source)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<double?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression), CancellationToken.None);
		}

		public static Task<double> Average(this IAsyncQueryable<long> source)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<double>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression), CancellationToken.None);
		}

		public static Task<double?> Average(this IAsyncQueryable<long?> source)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<double?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression), CancellationToken.None);
		}

		public static Task<double> Average(this IAsyncQueryable<double> source)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<double>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression), CancellationToken.None);
		}

		public static Task<double?> Average(this IAsyncQueryable<double?> source)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<double?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression), CancellationToken.None);
		}

		public static Task<float> Average(this IAsyncQueryable<float> source)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<float>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression), CancellationToken.None);
		}

		public static Task<float?> Average(this IAsyncQueryable<float?> source)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<float?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression), CancellationToken.None);
		}

		public static Task<decimal> Average(this IAsyncQueryable<decimal> source)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<decimal>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression), CancellationToken.None);
		}

		public static Task<decimal?> Average(this IAsyncQueryable<decimal?> source)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<decimal?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression), CancellationToken.None);
		}

		public static Task<double> Average(this IAsyncQueryable<int> source, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<double>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<double?> Average(this IAsyncQueryable<int?> source, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<double?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<double> Average(this IAsyncQueryable<long> source, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<double>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<double?> Average(this IAsyncQueryable<long?> source, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<double?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<double> Average(this IAsyncQueryable<double> source, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<double>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<double?> Average(this IAsyncQueryable<double?> source, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<double?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<float> Average(this IAsyncQueryable<float> source, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<float>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<float?> Average(this IAsyncQueryable<float?> source, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<float?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<decimal> Average(this IAsyncQueryable<decimal> source, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<decimal>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<decimal?> Average(this IAsyncQueryable<decimal?> source, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<decimal?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<double?> Average<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int?>> selector)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<double?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
		}

		public static Task<double> Average<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int>> selector)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<double>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
		}

		public static Task<double> Average<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, long>> selector)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<double>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
		}

		public static Task<double?> Average<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, long?>> selector)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<double?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
		}

		public static Task<double> Average<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, double>> selector)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<double>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
		}

		public static Task<double?> Average<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, double?>> selector)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<double?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
		}

		public static Task<float> Average<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, float>> selector)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<float>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
		}

		public static Task<float?> Average<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, float?>> selector)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<float?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
		}

		public static Task<decimal> Average<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, decimal>> selector)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<decimal>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
		}

		public static Task<decimal?> Average<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, decimal?>> selector)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<decimal?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
		}

		public static Task<double?> Average<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int?>> selector, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<double?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<double> Average<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int>> selector, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<double>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<double> Average<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, long>> selector, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<double>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<double?> Average<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, long?>> selector, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<double?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<double> Average<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, double>> selector, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<double>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<double?> Average<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, double?>> selector, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<double?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<float> Average<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, float>> selector, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<float>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<float?> Average<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, float?>> selector, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<float?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<decimal> Average<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, decimal>> selector, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<decimal>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<decimal?> Average<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, decimal?>> selector, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<decimal?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static IAsyncQueryable<IList<TSource>> Buffer<TSource>(this IAsyncQueryable<TSource> source, int count)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.CreateQuery<IList<TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(count, typeof(int))));
		}

		public static IAsyncQueryable<IList<TSource>> Buffer<TSource>(this IAsyncQueryable<TSource> source, int count, int skip)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.CreateQuery<IList<TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(count, typeof(int)), Expression.Constant(skip, typeof(int))));
		}

		public static IAsyncQueryable<TResult> Cast<TResult>(this IAsyncQueryable<object> source)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.CreateQuery<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TResult)), source.Expression));
		}

		public static IAsyncQueryable<TSource> Catch<TSource, TException>(this IAsyncQueryable<TSource> source, Expression<Func<TException, IAsyncEnumerable<TSource>>> handler)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (handler == null)
				throw new ArgumentNullException("handler");

			return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TException)), source.Expression, handler));
		}

		public static IAsyncQueryable<TSource> Catch<TSource>(this IAsyncQueryable<TSource> first, IAsyncEnumerable<TSource> second)
		{
			if (first == null)
				throw new ArgumentNullException("first");
			if (second == null)
				throw new ArgumentNullException("second");

			return first.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), first.Expression, GetSourceExpression(second)));
		}

		public static IAsyncQueryable<TSource> Concat<TSource>(this IAsyncQueryable<TSource> first, IAsyncEnumerable<TSource> second)
		{
			if (first == null)
				throw new ArgumentNullException("first");
			if (second == null)
				throw new ArgumentNullException("second");

			return first.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), first.Expression, GetSourceExpression(second)));
		}

		public static Task<bool> Contains<TSource>(this IAsyncQueryable<TSource> source, TSource value)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<bool>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(value, typeof(TSource))), CancellationToken.None);
		}

		public static Task<bool> Contains<TSource>(this IAsyncQueryable<TSource> source, TSource value, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<bool>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(value, typeof(TSource)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<bool> Contains<TSource>(this IAsyncQueryable<TSource> source, TSource value, IEqualityComparer<TSource> comparer)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (comparer == null)
				throw new ArgumentNullException("comparer");

			return source.Provider.ExecuteAsync<bool>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(value, typeof(TSource)), Expression.Constant(comparer, typeof(IEqualityComparer<TSource>))), CancellationToken.None);
		}

		public static Task<bool> Contains<TSource>(this IAsyncQueryable<TSource> source, TSource value, IEqualityComparer<TSource> comparer, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (comparer == null)
				throw new ArgumentNullException("comparer");

			return source.Provider.ExecuteAsync<bool>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(value, typeof(TSource)), Expression.Constant(comparer, typeof(IEqualityComparer<TSource>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<int> Count<TSource>(this IAsyncQueryable<TSource> source)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<int>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression), CancellationToken.None);
		}

		public static Task<int> Count<TSource>(this IAsyncQueryable<TSource> source, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<int>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<int> Count<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (predicate == null)
				throw new ArgumentNullException("predicate");

			return source.Provider.ExecuteAsync<int>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate), CancellationToken.None);
		}

		public static Task<int> Count<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (predicate == null)
				throw new ArgumentNullException("predicate");

			return source.Provider.ExecuteAsync<int>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static IAsyncQueryable<TSource> DefaultIfEmpty<TSource>(this IAsyncQueryable<TSource> source)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression));
		}

		public static IAsyncQueryable<TSource> DefaultIfEmpty<TSource>(this IAsyncQueryable<TSource> source, TSource defaultValue)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(defaultValue, typeof(TSource))));
		}

		public static IAsyncQueryable<TSource> Distinct<TSource>(this IAsyncQueryable<TSource> source)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression));
		}

		public static IAsyncQueryable<TSource> Distinct<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (keySelector == null)
				throw new ArgumentNullException("keySelector");

			return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector));
		}

		public static IAsyncQueryable<TSource> Distinct<TSource>(this IAsyncQueryable<TSource> source, IEqualityComparer<TSource> comparer)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (comparer == null)
				throw new ArgumentNullException("comparer");

			return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(comparer, typeof(IEqualityComparer<TSource>))));
		}

		public static IAsyncQueryable<TSource> Distinct<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, IEqualityComparer<TKey> comparer)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (keySelector == null)
				throw new ArgumentNullException("keySelector");
			if (comparer == null)
				throw new ArgumentNullException("comparer");

			return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
		}

		public static IAsyncQueryable<TSource> DistinctUntilChanged<TSource>(this IAsyncQueryable<TSource> source)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression));
		}

		public static IAsyncQueryable<TSource> DistinctUntilChanged<TSource>(this IAsyncQueryable<TSource> source, IEqualityComparer<TSource> comparer)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (comparer == null)
				throw new ArgumentNullException("comparer");

			return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(comparer, typeof(IEqualityComparer<TSource>))));
		}

		public static IAsyncQueryable<TSource> DistinctUntilChanged<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (keySelector == null)
				throw new ArgumentNullException("keySelector");

			return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector));
		}

		public static IAsyncQueryable<TSource> DistinctUntilChanged<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, IEqualityComparer<TKey> comparer)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (keySelector == null)
				throw new ArgumentNullException("keySelector");
			if (comparer == null)
				throw new ArgumentNullException("comparer");

			return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
		}

		public static IAsyncQueryable<TSource> Do<TSource>(this IAsyncQueryable<TSource> source, Expression<Action<TSource>> onNext)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (onNext == null)
				throw new ArgumentNullException("onNext");

			return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, onNext));
		}

		public static IAsyncQueryable<TSource> Do<TSource>(this IAsyncQueryable<TSource> source, IObserver<TSource> observer)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (observer == null)
				throw new ArgumentNullException("observer");

			return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(observer, typeof(IObserver<TSource>))));
		}

		public static IAsyncQueryable<TSource> Do<TSource>(this IAsyncQueryable<TSource> source, Expression<Action<TSource>> onNext, Action onCompleted)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (onNext == null)
				throw new ArgumentNullException("onNext");
			if (onCompleted == null)
				throw new ArgumentNullException("onCompleted");

			return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, onNext, Expression.Constant(onCompleted, typeof(Action))));
		}

		public static IAsyncQueryable<TSource> Do<TSource>(this IAsyncQueryable<TSource> source, Expression<Action<TSource>> onNext, Expression<Action<Exception>> onError)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (onNext == null)
				throw new ArgumentNullException("onNext");
			if (onError == null)
				throw new ArgumentNullException("onError");

			return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, onNext, onError));
		}

		public static IAsyncQueryable<TSource> Do<TSource>(this IAsyncQueryable<TSource> source, Expression<Action<TSource>> onNext, Expression<Action<Exception>> onError, Action onCompleted)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (onNext == null)
				throw new ArgumentNullException("onNext");
			if (onError == null)
				throw new ArgumentNullException("onError");
			if (onCompleted == null)
				throw new ArgumentNullException("onCompleted");

			return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, onNext, onError, Expression.Constant(onCompleted, typeof(Action))));
		}

		public static Task<TSource> ElementAt<TSource>(this IAsyncQueryable<TSource> source, int index)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(index, typeof(int))), CancellationToken.None);
		}

		public static Task<TSource> ElementAt<TSource>(this IAsyncQueryable<TSource> source, int index, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(index, typeof(int)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<TSource> ElementAtOrDefault<TSource>(this IAsyncQueryable<TSource> source, int index)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(index, typeof(int))), CancellationToken.None);
		}

		public static Task<TSource> ElementAtOrDefault<TSource>(this IAsyncQueryable<TSource> source, int index, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(index, typeof(int)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static IAsyncQueryable<TSource> Except<TSource>(this IAsyncQueryable<TSource> first, IAsyncEnumerable<TSource> second)
		{
			if (first == null)
				throw new ArgumentNullException("first");
			if (second == null)
				throw new ArgumentNullException("second");

			return first.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), first.Expression, GetSourceExpression(second)));
		}

		public static IAsyncQueryable<TSource> Except<TSource>(this IAsyncQueryable<TSource> first, IAsyncEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
		{
			if (first == null)
				throw new ArgumentNullException("first");
			if (second == null)
				throw new ArgumentNullException("second");
			if (comparer == null)
				throw new ArgumentNullException("comparer");

			return first.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), first.Expression, GetSourceExpression(second), Expression.Constant(comparer, typeof(IEqualityComparer<TSource>))));
		}

		public static IAsyncQueryable<TSource> Expand<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, IAsyncEnumerable<TSource>>> selector)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));
		}

		public static IAsyncQueryable<TSource> Finally<TSource>(this IAsyncQueryable<TSource> source, Action finallyAction)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (finallyAction == null)
				throw new ArgumentNullException("finallyAction");

			return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(finallyAction, typeof(Action))));
		}

		public static Task<TSource> First<TSource>(this IAsyncQueryable<TSource> source)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression), CancellationToken.None);
		}

		public static Task<TSource> First<TSource>(this IAsyncQueryable<TSource> source, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<TSource> First<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (predicate == null)
				throw new ArgumentNullException("predicate");

			return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate), CancellationToken.None);
		}

		public static Task<TSource> First<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (predicate == null)
				throw new ArgumentNullException("predicate");

			return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<TSource> FirstOrDefault<TSource>(this IAsyncQueryable<TSource> source)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression), CancellationToken.None);
		}

		public static Task<TSource> FirstOrDefault<TSource>(this IAsyncQueryable<TSource> source, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<TSource> FirstOrDefault<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (predicate == null)
				throw new ArgumentNullException("predicate");

			return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate), CancellationToken.None);
		}

		public static Task<TSource> FirstOrDefault<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (predicate == null)
				throw new ArgumentNullException("predicate");

			return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static IAsyncQueryable<IAsyncGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (keySelector == null)
				throw new ArgumentNullException("keySelector");

			return source.Provider.CreateQuery<IAsyncGrouping<TKey, TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector));
		}

		public static IAsyncQueryable<IAsyncGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (keySelector == null)
				throw new ArgumentNullException("keySelector");
			if (elementSelector == null)
				throw new ArgumentNullException("elementSelector");

			return source.Provider.CreateQuery<IAsyncGrouping<TKey, TElement>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey), typeof(TElement)), source.Expression, keySelector, elementSelector));
		}

		public static IAsyncQueryable<IAsyncGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, IEqualityComparer<TKey> comparer)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (keySelector == null)
				throw new ArgumentNullException("keySelector");
			if (comparer == null)
				throw new ArgumentNullException("comparer");

			return source.Provider.CreateQuery<IAsyncGrouping<TKey, TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
		}

		public static IAsyncQueryable<TResult> GroupBy<TSource, TKey, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TKey, IAsyncEnumerable<TSource>, TResult>> resultSelector)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (keySelector == null)
				throw new ArgumentNullException("keySelector");
			if (resultSelector == null)
				throw new ArgumentNullException("resultSelector");

			return source.Provider.CreateQuery<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey), typeof(TResult)), source.Expression, keySelector, resultSelector));
		}

		public static IAsyncQueryable<IAsyncGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector, IEqualityComparer<TKey> comparer)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (keySelector == null)
				throw new ArgumentNullException("keySelector");
			if (elementSelector == null)
				throw new ArgumentNullException("elementSelector");
			if (comparer == null)
				throw new ArgumentNullException("comparer");

			return source.Provider.CreateQuery<IAsyncGrouping<TKey, TElement>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey), typeof(TElement)), source.Expression, keySelector, elementSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
		}

		public static IAsyncQueryable<TResult> GroupBy<TSource, TKey, TElement, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector, Expression<Func<TKey, IAsyncEnumerable<TElement>, TResult>> resultSelector)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (keySelector == null)
				throw new ArgumentNullException("keySelector");
			if (elementSelector == null)
				throw new ArgumentNullException("elementSelector");
			if (resultSelector == null)
				throw new ArgumentNullException("resultSelector");

			return source.Provider.CreateQuery<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey), typeof(TElement), typeof(TResult)), source.Expression, keySelector, elementSelector, resultSelector));
		}

		public static IAsyncQueryable<TResult> GroupBy<TSource, TKey, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TKey, IAsyncEnumerable<TSource>, TResult>> resultSelector, IEqualityComparer<TKey> comparer)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (keySelector == null)
				throw new ArgumentNullException("keySelector");
			if (resultSelector == null)
				throw new ArgumentNullException("resultSelector");
			if (comparer == null)
				throw new ArgumentNullException("comparer");

			return source.Provider.CreateQuery<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey), typeof(TResult)), source.Expression, keySelector, resultSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
		}

		public static IAsyncQueryable<TResult> GroupBy<TSource, TKey, TElement, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector, Expression<Func<TKey, IAsyncEnumerable<TElement>, TResult>> resultSelector, IEqualityComparer<TKey> comparer)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (keySelector == null)
				throw new ArgumentNullException("keySelector");
			if (elementSelector == null)
				throw new ArgumentNullException("elementSelector");
			if (resultSelector == null)
				throw new ArgumentNullException("resultSelector");
			if (comparer == null)
				throw new ArgumentNullException("comparer");

			return source.Provider.CreateQuery<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey), typeof(TElement), typeof(TResult)), source.Expression, keySelector, elementSelector, resultSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
		}

		public static IAsyncQueryable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this IAsyncQueryable<TOuter> outer, IAsyncEnumerable<TInner> inner, Expression<Func<TOuter, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TOuter, IAsyncEnumerable<TInner>, TResult>> resultSelector)
		{
			if (outer == null)
				throw new ArgumentNullException("outer");
			if (inner == null)
				throw new ArgumentNullException("inner");
			if (outerKeySelector == null)
				throw new ArgumentNullException("outerKeySelector");
			if (innerKeySelector == null)
				throw new ArgumentNullException("innerKeySelector");
			if (resultSelector == null)
				throw new ArgumentNullException("resultSelector");

			return outer.Provider.CreateQuery<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TOuter), typeof(TInner), typeof(TKey), typeof(TResult)), outer.Expression, GetSourceExpression(inner), outerKeySelector, innerKeySelector, resultSelector));
		}

		public static IAsyncQueryable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this IAsyncQueryable<TOuter> outer, IAsyncEnumerable<TInner> inner, Expression<Func<TOuter, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TOuter, IAsyncEnumerable<TInner>, TResult>> resultSelector, IEqualityComparer<TKey> comparer)
		{
			if (outer == null)
				throw new ArgumentNullException("outer");
			if (inner == null)
				throw new ArgumentNullException("inner");
			if (outerKeySelector == null)
				throw new ArgumentNullException("outerKeySelector");
			if (innerKeySelector == null)
				throw new ArgumentNullException("innerKeySelector");
			if (resultSelector == null)
				throw new ArgumentNullException("resultSelector");
			if (comparer == null)
				throw new ArgumentNullException("comparer");

			return outer.Provider.CreateQuery<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TOuter), typeof(TInner), typeof(TKey), typeof(TResult)), outer.Expression, GetSourceExpression(inner), outerKeySelector, innerKeySelector, resultSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
		}

		public static IAsyncQueryable<TSource> IgnoreElements<TSource>(this IAsyncQueryable<TSource> source)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression));
		}

		public static IAsyncQueryable<TSource> Intersect<TSource>(this IAsyncQueryable<TSource> first, IAsyncEnumerable<TSource> second)
		{
			if (first == null)
				throw new ArgumentNullException("first");
			if (second == null)
				throw new ArgumentNullException("second");

			return first.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), first.Expression, GetSourceExpression(second)));
		}

		public static IAsyncQueryable<TSource> Intersect<TSource>(this IAsyncQueryable<TSource> first, IAsyncEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
		{
			if (first == null)
				throw new ArgumentNullException("first");
			if (second == null)
				throw new ArgumentNullException("second");
			if (comparer == null)
				throw new ArgumentNullException("comparer");

			return first.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), first.Expression, GetSourceExpression(second), Expression.Constant(comparer, typeof(IEqualityComparer<TSource>))));
		}

		public static Task<bool> IsEmpty<TSource>(this IAsyncQueryable<TSource> source)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<bool>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression), CancellationToken.None);
		}

		public static Task<bool> IsEmpty<TSource>(this IAsyncQueryable<TSource> source, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<bool>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static IAsyncQueryable<TResult> Join<TOuter, TInner, TKey, TResult>(this IAsyncQueryable<TOuter> outer, IAsyncEnumerable<TInner> inner, Expression<Func<TOuter, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TOuter, TInner, TResult>> resultSelector)
		{
			if (outer == null)
				throw new ArgumentNullException("outer");
			if (inner == null)
				throw new ArgumentNullException("inner");
			if (outerKeySelector == null)
				throw new ArgumentNullException("outerKeySelector");
			if (innerKeySelector == null)
				throw new ArgumentNullException("innerKeySelector");
			if (resultSelector == null)
				throw new ArgumentNullException("resultSelector");

			return outer.Provider.CreateQuery<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TOuter), typeof(TInner), typeof(TKey), typeof(TResult)), outer.Expression, GetSourceExpression(inner), outerKeySelector, innerKeySelector, resultSelector));
		}

		public static IAsyncQueryable<TResult> Join<TOuter, TInner, TKey, TResult>(this IAsyncQueryable<TOuter> outer, IAsyncEnumerable<TInner> inner, Expression<Func<TOuter, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TOuter, TInner, TResult>> resultSelector, IEqualityComparer<TKey> comparer)
		{
			if (outer == null)
				throw new ArgumentNullException("outer");
			if (inner == null)
				throw new ArgumentNullException("inner");
			if (outerKeySelector == null)
				throw new ArgumentNullException("outerKeySelector");
			if (innerKeySelector == null)
				throw new ArgumentNullException("innerKeySelector");
			if (resultSelector == null)
				throw new ArgumentNullException("resultSelector");
			if (comparer == null)
				throw new ArgumentNullException("comparer");

			return outer.Provider.CreateQuery<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TOuter), typeof(TInner), typeof(TKey), typeof(TResult)), outer.Expression, GetSourceExpression(inner), outerKeySelector, innerKeySelector, resultSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
		}

		public static Task<TSource> Last<TSource>(this IAsyncQueryable<TSource> source)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression), CancellationToken.None);
		}

		public static Task<TSource> Last<TSource>(this IAsyncQueryable<TSource> source, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<TSource> Last<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (predicate == null)
				throw new ArgumentNullException("predicate");

			return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate), CancellationToken.None);
		}

		public static Task<TSource> Last<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (predicate == null)
				throw new ArgumentNullException("predicate");

			return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<TSource> LastOrDefault<TSource>(this IAsyncQueryable<TSource> source)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression), CancellationToken.None);
		}

		public static Task<TSource> LastOrDefault<TSource>(this IAsyncQueryable<TSource> source, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<TSource> LastOrDefault<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (predicate == null)
				throw new ArgumentNullException("predicate");

			return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate), CancellationToken.None);
		}

		public static Task<TSource> LastOrDefault<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (predicate == null)
				throw new ArgumentNullException("predicate");

			return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<long> LongCount<TSource>(this IAsyncQueryable<TSource> source)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<long>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression), CancellationToken.None);
		}

		public static Task<long> LongCount<TSource>(this IAsyncQueryable<TSource> source, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<long>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<long> LongCount<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (predicate == null)
				throw new ArgumentNullException("predicate");

			return source.Provider.ExecuteAsync<long>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate), CancellationToken.None);
		}

		public static Task<long> LongCount<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (predicate == null)
				throw new ArgumentNullException("predicate");

			return source.Provider.ExecuteAsync<long>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<int> Max(this IAsyncQueryable<int> source)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<int>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression), CancellationToken.None);
		}

		public static Task<long> Max(this IAsyncQueryable<long> source)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<long>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression), CancellationToken.None);
		}

		public static Task<double> Max(this IAsyncQueryable<double> source)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<double>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression), CancellationToken.None);
		}

		public static Task<float> Max(this IAsyncQueryable<float> source)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<float>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression), CancellationToken.None);
		}

		public static Task<decimal> Max(this IAsyncQueryable<decimal> source)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<decimal>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression), CancellationToken.None);
		}

		public static Task<int?> Max(this IAsyncQueryable<int?> source)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<int?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression), CancellationToken.None);
		}

		public static Task<long?> Max(this IAsyncQueryable<long?> source)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<long?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression), CancellationToken.None);
		}

		public static Task<double?> Max(this IAsyncQueryable<double?> source)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<double?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression), CancellationToken.None);
		}

		public static Task<float?> Max(this IAsyncQueryable<float?> source)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<float?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression), CancellationToken.None);
		}

		public static Task<decimal?> Max(this IAsyncQueryable<decimal?> source)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<decimal?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression), CancellationToken.None);
		}

		public static Task<TSource> Max<TSource>(this IAsyncQueryable<TSource> source)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression), CancellationToken.None);
		}

		public static Task<int> Max(this IAsyncQueryable<int> source, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<int>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<long> Max(this IAsyncQueryable<long> source, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<long>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<double> Max(this IAsyncQueryable<double> source, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<double>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<float> Max(this IAsyncQueryable<float> source, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<float>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<decimal> Max(this IAsyncQueryable<decimal> source, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<decimal>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<int?> Max(this IAsyncQueryable<int?> source, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<int?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<long?> Max(this IAsyncQueryable<long?> source, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<long?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<double?> Max(this IAsyncQueryable<double?> source, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<double?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<float?> Max(this IAsyncQueryable<float?> source, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<float?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<decimal?> Max(this IAsyncQueryable<decimal?> source, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<decimal?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<TSource> Max<TSource>(this IAsyncQueryable<TSource> source, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<TSource> Max<TSource>(this IAsyncQueryable<TSource> source, IComparer<TSource> comparer)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (comparer == null)
				throw new ArgumentNullException("comparer");

			return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(comparer, typeof(IComparer<TSource>))), CancellationToken.None);
		}

		public static Task<int> Max<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int>> selector)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<int>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
		}

		public static Task<long> Max<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, long>> selector)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<long>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
		}

		public static Task<double> Max<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, double>> selector)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<double>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
		}

		public static Task<float> Max<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, float>> selector)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<float>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
		}

		public static Task<decimal> Max<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, decimal>> selector)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<decimal>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
		}

		public static Task<int?> Max<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int?>> selector)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<int?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
		}

		public static Task<long?> Max<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, long?>> selector)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<long?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
		}

		public static Task<double?> Max<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, double?>> selector)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<double?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
		}

		public static Task<float?> Max<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, float?>> selector)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<float?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
		}

		public static Task<decimal?> Max<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, decimal?>> selector)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<decimal?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
		}

		public static Task<TResult> Max<TSource, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TResult>> selector)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TResult)), source.Expression, selector), CancellationToken.None);
		}

		public static Task<TResult> Max<TSource, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TResult>> selector, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TResult)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<TSource> Max<TSource>(this IAsyncQueryable<TSource> source, IComparer<TSource> comparer, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (comparer == null)
				throw new ArgumentNullException("comparer");

			return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(comparer, typeof(IComparer<TSource>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<int> Max<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int>> selector, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<int>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<long> Max<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, long>> selector, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<long>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<double> Max<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, double>> selector, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<double>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<float> Max<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, float>> selector, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<float>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<decimal> Max<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, decimal>> selector, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<decimal>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<int?> Max<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int?>> selector, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<int?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<long?> Max<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, long?>> selector, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<long?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<double?> Max<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, double?>> selector, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<double?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<float?> Max<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, float?>> selector, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<float?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<decimal?> Max<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, decimal?>> selector, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<decimal?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<IList<TSource>> MaxBy<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (keySelector == null)
				throw new ArgumentNullException("keySelector");

			return source.Provider.ExecuteAsync<IList<TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector), CancellationToken.None);
		}

		public static Task<IList<TSource>> MaxBy<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (keySelector == null)
				throw new ArgumentNullException("keySelector");

			return source.Provider.ExecuteAsync<IList<TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<IList<TSource>> MaxBy<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, IComparer<TKey> comparer)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (keySelector == null)
				throw new ArgumentNullException("keySelector");
			if (comparer == null)
				throw new ArgumentNullException("comparer");

			return source.Provider.ExecuteAsync<IList<TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IComparer<TKey>))), CancellationToken.None);
		}

		public static Task<IList<TSource>> MaxBy<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, IComparer<TKey> comparer, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (keySelector == null)
				throw new ArgumentNullException("keySelector");
			if (comparer == null)
				throw new ArgumentNullException("comparer");

			return source.Provider.ExecuteAsync<IList<TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IComparer<TKey>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<int> Min(this IAsyncQueryable<int> source)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<int>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression), CancellationToken.None);
		}

		public static Task<long> Min(this IAsyncQueryable<long> source)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<long>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression), CancellationToken.None);
		}

		public static Task<double> Min(this IAsyncQueryable<double> source)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<double>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression), CancellationToken.None);
		}

		public static Task<float> Min(this IAsyncQueryable<float> source)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<float>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression), CancellationToken.None);
		}

		public static Task<decimal> Min(this IAsyncQueryable<decimal> source)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<decimal>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression), CancellationToken.None);
		}

		public static Task<int?> Min(this IAsyncQueryable<int?> source)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<int?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression), CancellationToken.None);
		}

		public static Task<long?> Min(this IAsyncQueryable<long?> source)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<long?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression), CancellationToken.None);
		}

		public static Task<double?> Min(this IAsyncQueryable<double?> source)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<double?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression), CancellationToken.None);
		}

		public static Task<float?> Min(this IAsyncQueryable<float?> source)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<float?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression), CancellationToken.None);
		}

		public static Task<decimal?> Min(this IAsyncQueryable<decimal?> source)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<decimal?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression), CancellationToken.None);
		}

		public static Task<TSource> Min<TSource>(this IAsyncQueryable<TSource> source)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression), CancellationToken.None);
		}

		public static Task<int> Min(this IAsyncQueryable<int> source, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<int>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<long> Min(this IAsyncQueryable<long> source, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<long>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<double> Min(this IAsyncQueryable<double> source, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<double>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<float> Min(this IAsyncQueryable<float> source, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<float>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<decimal> Min(this IAsyncQueryable<decimal> source, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<decimal>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<int?> Min(this IAsyncQueryable<int?> source, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<int?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<long?> Min(this IAsyncQueryable<long?> source, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<long?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<double?> Min(this IAsyncQueryable<double?> source, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<double?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<float?> Min(this IAsyncQueryable<float?> source, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<float?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<decimal?> Min(this IAsyncQueryable<decimal?> source, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<decimal?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<TSource> Min<TSource>(this IAsyncQueryable<TSource> source, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<double?> Min<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, double?>> selector)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<double?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
		}

		public static Task<float?> Min<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, float?>> selector)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<float?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
		}

		public static Task<TSource> Min<TSource>(this IAsyncQueryable<TSource> source, IComparer<TSource> comparer)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (comparer == null)
				throw new ArgumentNullException("comparer");

			return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(comparer, typeof(IComparer<TSource>))), CancellationToken.None);
		}

		public static Task<int> Min<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int>> selector)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<int>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
		}

		public static Task<long> Min<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, long>> selector)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<long>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
		}

		public static Task<double> Min<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, double>> selector)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<double>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
		}

		public static Task<float> Min<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, float>> selector)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<float>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
		}

		public static Task<decimal> Min<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, decimal>> selector)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<decimal>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
		}

		public static Task<int?> Min<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int?>> selector)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<int?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
		}

		public static Task<long?> Min<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, long?>> selector)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<long?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
		}

		public static Task<decimal?> Min<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, decimal?>> selector)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<decimal?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
		}

		public static Task<TResult> Min<TSource, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TResult>> selector)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TResult)), source.Expression, selector), CancellationToken.None);
		}

		public static Task<int> Min<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int>> selector, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<int>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<long> Min<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, long>> selector, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<long>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<double> Min<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, double>> selector, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<double>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<float> Min<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, float>> selector, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<float>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<decimal> Min<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, decimal>> selector, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<decimal>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<int?> Min<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int?>> selector, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<int?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<long?> Min<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, long?>> selector, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<long?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<double?> Min<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, double?>> selector, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<double?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<float?> Min<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, float?>> selector, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<float?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<decimal?> Min<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, decimal?>> selector, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<decimal?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<TResult> Min<TSource, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TResult>> selector, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TResult)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<TSource> Min<TSource>(this IAsyncQueryable<TSource> source, IComparer<TSource> comparer, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (comparer == null)
				throw new ArgumentNullException("comparer");

			return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(comparer, typeof(IComparer<TSource>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<IList<TSource>> MinBy<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (keySelector == null)
				throw new ArgumentNullException("keySelector");

			return source.Provider.ExecuteAsync<IList<TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector), CancellationToken.None);
		}

		public static Task<IList<TSource>> MinBy<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (keySelector == null)
				throw new ArgumentNullException("keySelector");

			return source.Provider.ExecuteAsync<IList<TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<IList<TSource>> MinBy<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, IComparer<TKey> comparer)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (keySelector == null)
				throw new ArgumentNullException("keySelector");
			if (comparer == null)
				throw new ArgumentNullException("comparer");

			return source.Provider.ExecuteAsync<IList<TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IComparer<TKey>))), CancellationToken.None);
		}

		public static Task<IList<TSource>> MinBy<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, IComparer<TKey> comparer, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (keySelector == null)
				throw new ArgumentNullException("keySelector");
			if (comparer == null)
				throw new ArgumentNullException("comparer");

			return source.Provider.ExecuteAsync<IList<TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IComparer<TKey>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static IAsyncQueryable<TType> OfType<TType>(this IAsyncQueryable<object> source)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.CreateQuery<TType>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TType)), source.Expression));
		}

		public static IAsyncQueryable<TSource> OnErrorResumeNext<TSource>(this IAsyncQueryable<TSource> first, IAsyncEnumerable<TSource> second)
		{
			if (first == null)
				throw new ArgumentNullException("first");
			if (second == null)
				throw new ArgumentNullException("second");

			return first.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), first.Expression, GetSourceExpression(second)));
		}

		public static IOrderedAsyncQueryable<TSource> OrderBy<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (keySelector == null)
				throw new ArgumentNullException("keySelector");

			return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector));
		}

		public static IOrderedAsyncQueryable<TSource> OrderBy<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, IComparer<TKey> comparer)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (keySelector == null)
				throw new ArgumentNullException("keySelector");
			if (comparer == null)
				throw new ArgumentNullException("comparer");

			return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IComparer<TKey>))));
		}

		public static IOrderedAsyncQueryable<TSource> OrderByDescending<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (keySelector == null)
				throw new ArgumentNullException("keySelector");

			return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector));
		}

		public static IOrderedAsyncQueryable<TSource> OrderByDescending<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, IComparer<TKey> comparer)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (keySelector == null)
				throw new ArgumentNullException("keySelector");
			if (comparer == null)
				throw new ArgumentNullException("comparer");

			return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IComparer<TKey>))));
		}

		public static IAsyncQueryable<TSource> Repeat<TSource>(this IAsyncQueryable<TSource> source)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression));
		}

		public static IAsyncQueryable<TSource> Repeat<TSource>(this IAsyncQueryable<TSource> source, int count)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(count, typeof(int))));
		}

		public static IAsyncQueryable<TSource> Retry<TSource>(this IAsyncQueryable<TSource> source)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression));
		}

		public static IAsyncQueryable<TSource> Retry<TSource>(this IAsyncQueryable<TSource> source, int retryCount)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(retryCount, typeof(int))));
		}

		public static IAsyncQueryable<TSource> Reverse<TSource>(this IAsyncQueryable<TSource> source)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression));
		}

		public static IAsyncQueryable<TSource> Scan<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TSource, TSource>> accumulator)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (accumulator == null)
				throw new ArgumentNullException("accumulator");

			return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, accumulator));
		}

		public static IAsyncQueryable<TAccumulate> Scan<TSource, TAccumulate>(this IAsyncQueryable<TSource> source, TAccumulate seed, Expression<Func<TAccumulate, TSource, TAccumulate>> accumulator)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (accumulator == null)
				throw new ArgumentNullException("accumulator");

			return source.Provider.CreateQuery<TAccumulate>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TAccumulate)), source.Expression, Expression.Constant(seed, typeof(TAccumulate)), accumulator));
		}

		public static IAsyncQueryable<TResult> Select<TSource, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TResult>> selector)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.CreateQuery<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TResult)), source.Expression, selector));
		}

		public static IAsyncQueryable<TResult> Select<TSource, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int, TResult>> selector)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.CreateQuery<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TResult)), source.Expression, selector));
		}

		public static IAsyncQueryable<TOther> SelectMany<TSource, TOther>(this IAsyncQueryable<TSource> source, IAsyncEnumerable<TOther> other)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (other == null)
				throw new ArgumentNullException("other");

			return source.Provider.CreateQuery<TOther>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TOther)), source.Expression, GetSourceExpression(other)));
		}

		public static IAsyncQueryable<TResult> SelectMany<TSource, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, IAsyncEnumerable<TResult>>> selector)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.CreateQuery<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TResult)), source.Expression, selector));
		}

		public static IAsyncQueryable<TResult> SelectMany<TSource, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int, IAsyncEnumerable<TResult>>> selector)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.CreateQuery<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TResult)), source.Expression, selector));
		}

		public static IAsyncQueryable<TResult> SelectMany<TSource, TCollection, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, IAsyncEnumerable<TCollection>>> selector, Expression<Func<TSource, TCollection, TResult>> resultSelector)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");
			if (resultSelector == null)
				throw new ArgumentNullException("resultSelector");

			return source.Provider.CreateQuery<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TCollection), typeof(TResult)), source.Expression, selector, resultSelector));
		}

		public static IAsyncQueryable<TResult> SelectMany<TSource, TCollection, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int, IAsyncEnumerable<TCollection>>> selector, Expression<Func<TSource, TCollection, TResult>> resultSelector)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");
			if (resultSelector == null)
				throw new ArgumentNullException("resultSelector");

			return source.Provider.CreateQuery<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TCollection), typeof(TResult)), source.Expression, selector, resultSelector));
		}

		public static Task<bool> SequenceEqual<TSource>(this IAsyncQueryable<TSource> first, IAsyncEnumerable<TSource> second)
		{
			if (first == null)
				throw new ArgumentNullException("first");
			if (second == null)
				throw new ArgumentNullException("second");

			return first.Provider.ExecuteAsync<bool>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), first.Expression, GetSourceExpression(second)), CancellationToken.None);
		}

		public static Task<bool> SequenceEqual<TSource>(this IAsyncQueryable<TSource> first, IAsyncEnumerable<TSource> second, CancellationToken cancellationToken)
		{
			if (first == null)
				throw new ArgumentNullException("first");
			if (second == null)
				throw new ArgumentNullException("second");

			return first.Provider.ExecuteAsync<bool>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), first.Expression, GetSourceExpression(second), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<bool> SequenceEqual<TSource>(this IAsyncQueryable<TSource> first, IAsyncEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
		{
			if (first == null)
				throw new ArgumentNullException("first");
			if (second == null)
				throw new ArgumentNullException("second");
			if (comparer == null)
				throw new ArgumentNullException("comparer");

			return first.Provider.ExecuteAsync<bool>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), first.Expression, GetSourceExpression(second), Expression.Constant(comparer, typeof(IEqualityComparer<TSource>))), CancellationToken.None);
		}

		public static Task<bool> SequenceEqual<TSource>(this IAsyncQueryable<TSource> first, IAsyncEnumerable<TSource> second, IEqualityComparer<TSource> comparer, CancellationToken cancellationToken)
		{
			if (first == null)
				throw new ArgumentNullException("first");
			if (second == null)
				throw new ArgumentNullException("second");
			if (comparer == null)
				throw new ArgumentNullException("comparer");

			return first.Provider.ExecuteAsync<bool>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), first.Expression, GetSourceExpression(second), Expression.Constant(comparer, typeof(IEqualityComparer<TSource>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<TSource> Single<TSource>(this IAsyncQueryable<TSource> source)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression), CancellationToken.None);
		}

		public static Task<TSource> Single<TSource>(this IAsyncQueryable<TSource> source, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<TSource> Single<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (predicate == null)
				throw new ArgumentNullException("predicate");

			return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate), CancellationToken.None);
		}

		public static Task<TSource> Single<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (predicate == null)
				throw new ArgumentNullException("predicate");

			return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<TSource> SingleOrDefault<TSource>(this IAsyncQueryable<TSource> source)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression), CancellationToken.None);
		}

		public static Task<TSource> SingleOrDefault<TSource>(this IAsyncQueryable<TSource> source, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<TSource> SingleOrDefault<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (predicate == null)
				throw new ArgumentNullException("predicate");

			return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate), CancellationToken.None);
		}

		public static Task<TSource> SingleOrDefault<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (predicate == null)
				throw new ArgumentNullException("predicate");

			return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static IAsyncQueryable<TSource> Skip<TSource>(this IAsyncQueryable<TSource> source, int count)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(count, typeof(int))));
		}

		public static IAsyncQueryable<TSource> SkipLast<TSource>(this IAsyncQueryable<TSource> source, int count)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(count, typeof(int))));
		}

		public static IAsyncQueryable<TSource> SkipWhile<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (predicate == null)
				throw new ArgumentNullException("predicate");

			return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate));
		}

		public static IAsyncQueryable<TSource> SkipWhile<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int, bool>> predicate)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (predicate == null)
				throw new ArgumentNullException("predicate");

			return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate));
		}

		public static IAsyncQueryable<TSource> StartWith<TSource>(this IAsyncQueryable<TSource> source, params TSource[] values)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (values == null)
				throw new ArgumentNullException("values");

			return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(values, typeof(TSource[]))));
		}

		public static Task<int> Sum(this IAsyncQueryable<int> source)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<int>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression), CancellationToken.None);
		}

		public static Task<long> Sum(this IAsyncQueryable<long> source)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<long>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression), CancellationToken.None);
		}

		public static Task<double> Sum(this IAsyncQueryable<double> source)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<double>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression), CancellationToken.None);
		}

		public static Task<float> Sum(this IAsyncQueryable<float> source)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<float>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression), CancellationToken.None);
		}

		public static Task<decimal> Sum(this IAsyncQueryable<decimal> source)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<decimal>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression), CancellationToken.None);
		}

		public static Task<int?> Sum(this IAsyncQueryable<int?> source)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<int?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression), CancellationToken.None);
		}

		public static Task<long?> Sum(this IAsyncQueryable<long?> source)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<long?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression), CancellationToken.None);
		}

		public static Task<double?> Sum(this IAsyncQueryable<double?> source)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<double?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression), CancellationToken.None);
		}

		public static Task<float?> Sum(this IAsyncQueryable<float?> source)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<float?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression), CancellationToken.None);
		}

		public static Task<decimal?> Sum(this IAsyncQueryable<decimal?> source)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<decimal?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression), CancellationToken.None);
		}

		public static Task<int> Sum(this IAsyncQueryable<int> source, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<int>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<long> Sum(this IAsyncQueryable<long> source, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<long>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<double> Sum(this IAsyncQueryable<double> source, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<double>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<float> Sum(this IAsyncQueryable<float> source, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<float>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<decimal> Sum(this IAsyncQueryable<decimal> source, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<decimal>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<int?> Sum(this IAsyncQueryable<int?> source, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<int?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<long?> Sum(this IAsyncQueryable<long?> source, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<long?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<double?> Sum(this IAsyncQueryable<double?> source, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<double?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<float?> Sum(this IAsyncQueryable<float?> source, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<float?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<decimal?> Sum(this IAsyncQueryable<decimal?> source, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<decimal?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<int> Sum<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int>> selector)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<int>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
		}

		public static Task<long> Sum<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, long>> selector)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<long>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
		}

		public static Task<double> Sum<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, double>> selector)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<double>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
		}

		public static Task<float> Sum<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, float>> selector)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<float>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
		}

		public static Task<decimal> Sum<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, decimal>> selector)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<decimal>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
		}

		public static Task<int?> Sum<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int?>> selector)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<int?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
		}

		public static Task<long?> Sum<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, long?>> selector)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<long?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
		}

		public static Task<double?> Sum<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, double?>> selector)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<double?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
		}

		public static Task<float?> Sum<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, float?>> selector)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<float?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
		}

		public static Task<decimal?> Sum<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, decimal?>> selector)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<decimal?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
		}

		public static Task<int> Sum<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int>> selector, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<int>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<long> Sum<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, long>> selector, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<long>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<double> Sum<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, double>> selector, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<double>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<float> Sum<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, float>> selector, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<float>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<decimal> Sum<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, decimal>> selector, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<decimal>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<int?> Sum<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int?>> selector, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<int?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<long?> Sum<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, long?>> selector, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<long?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<double?> Sum<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, double?>> selector, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<double?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<float?> Sum<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, float?>> selector, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<float?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<decimal?> Sum<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, decimal?>> selector, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return source.Provider.ExecuteAsync<decimal?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static IAsyncQueryable<TSource> Take<TSource>(this IAsyncQueryable<TSource> source, int count)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(count, typeof(int))));
		}

		public static IAsyncQueryable<TSource> TakeLast<TSource>(this IAsyncQueryable<TSource> source, int count)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(count, typeof(int))));
		}

		public static IAsyncQueryable<TSource> TakeWhile<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (predicate == null)
				throw new ArgumentNullException("predicate");

			return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate));
		}

		public static IAsyncQueryable<TSource> TakeWhile<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int, bool>> predicate)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (predicate == null)
				throw new ArgumentNullException("predicate");

			return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate));
		}

		public static IOrderedAsyncQueryable<TSource> ThenBy<TSource, TKey>(this IOrderedAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (keySelector == null)
				throw new ArgumentNullException("keySelector");

			return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector));
		}

		public static IOrderedAsyncQueryable<TSource> ThenBy<TSource, TKey>(this IOrderedAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, IComparer<TKey> comparer)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (keySelector == null)
				throw new ArgumentNullException("keySelector");
			if (comparer == null)
				throw new ArgumentNullException("comparer");

			return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IComparer<TKey>))));
		}

		public static IOrderedAsyncQueryable<TSource> ThenByDescending<TSource, TKey>(this IOrderedAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (keySelector == null)
				throw new ArgumentNullException("keySelector");

			return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector));
		}

		public static IOrderedAsyncQueryable<TSource> ThenByDescending<TSource, TKey>(this IOrderedAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, IComparer<TKey> comparer)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (keySelector == null)
				throw new ArgumentNullException("keySelector");
			if (comparer == null)
				throw new ArgumentNullException("comparer");

			return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IComparer<TKey>))));
		}

		public static Task<TSource[]> ToArray<TSource>(this IAsyncQueryable<TSource> source)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<TSource[]>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression), CancellationToken.None);
		}

		public static Task<TSource[]> ToArray<TSource>(this IAsyncQueryable<TSource> source, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<TSource[]>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<Dictionary<TKey, TSource>> ToDictionary<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (keySelector == null)
				throw new ArgumentNullException("keySelector");

			return source.Provider.ExecuteAsync<Dictionary<TKey, TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector), CancellationToken.None);
		}

		public static Task<Dictionary<TKey, TSource>> ToDictionary<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (keySelector == null)
				throw new ArgumentNullException("keySelector");

			return source.Provider.ExecuteAsync<Dictionary<TKey, TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<Dictionary<TKey, TElement>> ToDictionary<TSource, TKey, TElement>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (keySelector == null)
				throw new ArgumentNullException("keySelector");
			if (elementSelector == null)
				throw new ArgumentNullException("elementSelector");

			return source.Provider.ExecuteAsync<Dictionary<TKey, TElement>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey), typeof(TElement)), source.Expression, keySelector, elementSelector), CancellationToken.None);
		}

		public static Task<Dictionary<TKey, TSource>> ToDictionary<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, IEqualityComparer<TKey> comparer)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (keySelector == null)
				throw new ArgumentNullException("keySelector");
			if (comparer == null)
				throw new ArgumentNullException("comparer");

			return source.Provider.ExecuteAsync<Dictionary<TKey, TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))), CancellationToken.None);
		}

		public static Task<Dictionary<TKey, TElement>> ToDictionary<TSource, TKey, TElement>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (keySelector == null)
				throw new ArgumentNullException("keySelector");
			if (elementSelector == null)
				throw new ArgumentNullException("elementSelector");

			return source.Provider.ExecuteAsync<Dictionary<TKey, TElement>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey), typeof(TElement)), source.Expression, keySelector, elementSelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<Dictionary<TKey, TSource>> ToDictionary<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (keySelector == null)
				throw new ArgumentNullException("keySelector");
			if (comparer == null)
				throw new ArgumentNullException("comparer");

			return source.Provider.ExecuteAsync<Dictionary<TKey, TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<Dictionary<TKey, TElement>> ToDictionary<TSource, TKey, TElement>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector, IEqualityComparer<TKey> comparer)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (keySelector == null)
				throw new ArgumentNullException("keySelector");
			if (elementSelector == null)
				throw new ArgumentNullException("elementSelector");
			if (comparer == null)
				throw new ArgumentNullException("comparer");

			return source.Provider.ExecuteAsync<Dictionary<TKey, TElement>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey), typeof(TElement)), source.Expression, keySelector, elementSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))), CancellationToken.None);
		}

		public static Task<Dictionary<TKey, TElement>> ToDictionary<TSource, TKey, TElement>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (keySelector == null)
				throw new ArgumentNullException("keySelector");
			if (elementSelector == null)
				throw new ArgumentNullException("elementSelector");
			if (comparer == null)
				throw new ArgumentNullException("comparer");

			return source.Provider.ExecuteAsync<Dictionary<TKey, TElement>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey), typeof(TElement)), source.Expression, keySelector, elementSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<List<TSource>> ToList<TSource>(this IAsyncQueryable<TSource> source)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<List<TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression), CancellationToken.None);
		}

		public static Task<List<TSource>> ToList<TSource>(this IAsyncQueryable<TSource> source, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.ExecuteAsync<List<TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<ILookup<TKey, TSource>> ToLookup<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (keySelector == null)
				throw new ArgumentNullException("keySelector");

			return source.Provider.ExecuteAsync<ILookup<TKey, TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector), CancellationToken.None);
		}

		public static Task<ILookup<TKey, TSource>> ToLookup<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (keySelector == null)
				throw new ArgumentNullException("keySelector");

			return source.Provider.ExecuteAsync<ILookup<TKey, TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<ILookup<TKey, TElement>> ToLookup<TSource, TKey, TElement>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (keySelector == null)
				throw new ArgumentNullException("keySelector");
			if (elementSelector == null)
				throw new ArgumentNullException("elementSelector");

			return source.Provider.ExecuteAsync<ILookup<TKey, TElement>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey), typeof(TElement)), source.Expression, keySelector, elementSelector), CancellationToken.None);
		}

		public static Task<ILookup<TKey, TSource>> ToLookup<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, IEqualityComparer<TKey> comparer)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (keySelector == null)
				throw new ArgumentNullException("keySelector");
			if (comparer == null)
				throw new ArgumentNullException("comparer");

			return source.Provider.ExecuteAsync<ILookup<TKey, TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))), CancellationToken.None);
		}

		public static Task<ILookup<TKey, TElement>> ToLookup<TSource, TKey, TElement>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (keySelector == null)
				throw new ArgumentNullException("keySelector");
			if (elementSelector == null)
				throw new ArgumentNullException("elementSelector");

			return source.Provider.ExecuteAsync<ILookup<TKey, TElement>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey), typeof(TElement)), source.Expression, keySelector, elementSelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<ILookup<TKey, TSource>> ToLookup<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (keySelector == null)
				throw new ArgumentNullException("keySelector");
			if (comparer == null)
				throw new ArgumentNullException("comparer");

			return source.Provider.ExecuteAsync<ILookup<TKey, TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static Task<ILookup<TKey, TElement>> ToLookup<TSource, TKey, TElement>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector, IEqualityComparer<TKey> comparer)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (keySelector == null)
				throw new ArgumentNullException("keySelector");
			if (elementSelector == null)
				throw new ArgumentNullException("elementSelector");
			if (comparer == null)
				throw new ArgumentNullException("comparer");

			return source.Provider.ExecuteAsync<ILookup<TKey, TElement>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey), typeof(TElement)), source.Expression, keySelector, elementSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))), CancellationToken.None);
		}

		public static Task<ILookup<TKey, TElement>> ToLookup<TSource, TKey, TElement>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (keySelector == null)
				throw new ArgumentNullException("keySelector");
			if (elementSelector == null)
				throw new ArgumentNullException("elementSelector");
			if (comparer == null)
				throw new ArgumentNullException("comparer");

			return source.Provider.ExecuteAsync<ILookup<TKey, TElement>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey), typeof(TElement)), source.Expression, keySelector, elementSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
		}

		public static IAsyncQueryable<TSource> Union<TSource>(this IAsyncQueryable<TSource> first, IAsyncEnumerable<TSource> second)
		{
			if (first == null)
				throw new ArgumentNullException("first");
			if (second == null)
				throw new ArgumentNullException("second");

			return first.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), first.Expression, GetSourceExpression(second)));
		}

		public static IAsyncQueryable<TSource> Union<TSource>(this IAsyncQueryable<TSource> first, IAsyncEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
		{
			if (first == null)
				throw new ArgumentNullException("first");
			if (second == null)
				throw new ArgumentNullException("second");
			if (comparer == null)
				throw new ArgumentNullException("comparer");

			return first.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), first.Expression, GetSourceExpression(second), Expression.Constant(comparer, typeof(IEqualityComparer<TSource>))));
		}

		public static IAsyncQueryable<TSource> Where<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (predicate == null)
				throw new ArgumentNullException("predicate");

			return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate));
		}

		public static IAsyncQueryable<TSource> Where<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int, bool>> predicate)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (predicate == null)
				throw new ArgumentNullException("predicate");

			return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate));
		}

		public static IAsyncQueryable<TResult> Zip<TFirst, TSecond, TResult>(this IAsyncQueryable<TFirst> first, IAsyncEnumerable<TSecond> second, Expression<Func<TFirst, TSecond, TResult>> selector)
		{
			if (first == null)
				throw new ArgumentNullException("first");
			if (second == null)
				throw new ArgumentNullException("second");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return first.Provider.CreateQuery<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TFirst), typeof(TSecond), typeof(TResult)), first.Expression, GetSourceExpression(second), selector));
		}

	}
}
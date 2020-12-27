using System;
using System.Linq;
using System.Linq.Expressions;

namespace Redux.NET
{
    /// <summary>
    /// Store Extensions with handy methods.
    /// </summary>
    public static class StoreExtensions
    {
        /// <summary>
        /// When your selector become too long, you can use CreateSelector to simplify expressions.
        /// </summary>
        public static Expression<Func<TIn, TOut>> CreateSelector<TIn, TInterstitial, TOut>(
            Expression<Func<TIn, TInterstitial>> inner,
            Expression<Func<TInterstitial, TOut>> outer)
        {
            var visitor = new SwapVisitor(outer.Parameters[0], inner.Body);
            return Expression.Lambda<Func<TIn, TOut>>(visitor.Visit(outer.Body), inner.Parameters);
        }

        /// <summary>
        /// This method will transform an expression into the location of a state slice.
        /// Also it filters out extension methods used in expression such as First().
        /// Which is necessary to specify in the expression when using lists.
        /// </summary>
        public static string Slice<State, T>(this Expression<Func<State, T>> expr)
        {
            var stateSlice = string.Join(".", expr.ToString()
                .Split('.')
                .Where(x => !x.Contains("("))
                .Skip(1));

            return stateSlice;
        }
    }
}

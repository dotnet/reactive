using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Redux.NET
{
    /// <summary>
    /// Contract for the State Store
    /// </summary>
    public interface IStore<IState>
    {
        /// <summary>
        /// Selects the specified state slice.
        /// </summary>
        /// <param name="expr">Expression locating the state slice.</param>
        /// <typeparam name="T">Type of the resulting state slice.</typeparam>
        /// <returns></returns>
        T Select<T>(Expression<Func<IState, T>> expr);

        /// <summary>
        /// After updating the state, it will trigger side effects.
        /// So if you have server operations, then use this async method.
        /// </summary>
        /// <param name="action">Action with a possible payload.</param>
        /// <returns>Awaits marked side effects and then completes.</returns>
        Task DispatchAsync(IAction action);

        /// <summary>
        /// Updates states and does not trigger any side effects.
        /// Use this when you already have the data from your side effect.
        /// </summary>
        /// <param name="action">Action with a possible payload.</param>
        /// <typeparam name="T">Type of the payload.</typeparam>
        void Dispatch<T>(Action<T> action);

        /// <summary>
        /// Sets the state slice with the specified value.
        /// A simplified alternative if you don't want reducers/effects.
        /// </summary>
        /// <param name="expr">Expression locating the state slice.</param>
        /// <param name="value">The data for the state slice.</param>
        /// <typeparam name="T">Type of the payload.</typeparam>
        void Dispatch<T>(Expression<Func<IState, T>> expr, object value);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Redux.NET
{
    /// <summary>
    /// Generic Store with a State to be specified.
    /// </summary>
    /// <typeparam name="IState"></typeparam>
    public class Store<IState> : IStore<IState>
    {
        /// <summary>
        /// Specified State will be stored.
        /// </summary>
        private protected IState state { get; set; }

        /// <summary>
        /// The reducer which will be used to reduce the state.
        /// </summary>
        private protected IReducer<IState> reducer { get; set; }

        /// <summary>
        /// Provider is needed to resolve dependencies for reflection.
        /// </summary>
        private protected IServiceProvider serviceProvider;

        /// <summary>
        /// Initialize State Store
        /// </summary>
        /// <param name="state">The state which will specify the state slices.</param>
        /// <param name="reducer">The reducer which will manage the state.</param>
        /// <param name="serviceProvider">The provider will be used to resolve dependencies</param>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="state"/> is null.</exception>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="reducer"/> is null.</exception>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="serviceProvider"/> is null.</exception>
        public Store(IState state, IReducer<IState> reducer, IServiceProvider serviceProvider)
        {
            this.state = state ?? throw new ArgumentNullException(nameof(state));
            this.reducer = reducer ?? throw new ArgumentNullException(nameof(reducer));
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        /// <summary>
        /// Selects the state slice specified in the expression.
        /// </summary>
        /// <param name="state">Currently selected state.</param>
        /// <typeparam name="T">Type of state slice.</typeparam>
        /// <returns>The selected state slice.</returns>
        public T Select<T>(Expression<Func<IState, T>> state)
        {
            return (T)ReflectionExtensions.Get(state.Slice(), this.state);
        }

        /// <summary>
        /// Reduces the state and executes the side effects.
        /// </summary>
        /// <param name="action">Action with a possible payload.</param>
        /// <returns>A completed task when done.</returns>
        public async Task DispatchAsync(IAction action)
        {
            this.state = this.reducer.Execute(this.state, action);

            await ExecuteEffects(action);
        }

        /// <summary>
        /// Reduces the state (and no further actions).
        /// </summary>
        /// <param name="action">Action with a payload.</param>
        /// <typeparam name="T">Type of the payload.</typeparam>
        public void Dispatch<T>(Action<T> action)
        {
            this.state = this.reducer.Execute(this.state, action);
        }

        [Obsolete]
        public void Dispatch<T>(Expression<Func<IState, T>> state, object value)
        {
            ReflectionExtensions.Set(state.Slice(), this.state, value);
        }

        /// <summary>
        /// Responsible for identifying effects and execution.
        /// The side effect will only trigger for actions specified.
        /// </summary>
        /// <param name="action">Action with a possible payload.</param>
        /// <typeparam name="T">Type of the payload.</typeparam>
        /// <returns>A completed task when done.</returns>
        private async Task ExecuteEffects<T>(T action)
        {
            var methods = SideEffects.Find();
            foreach (var method in methods)
            {
                var attr = method.GetCustomAttributes(typeof(EffectAttribute), true).FirstOrDefault() as EffectAttribute;
                if (attr.Actions.Contains(action.GetType()))
                {
                    var constructors = method.DeclaringType.GetConstructors();
                    var firstConstrutor = constructors.FirstOrDefault();
                    var parameters = new List<object>();

                    foreach (var param in firstConstrutor.GetParameters())
                    {
                        var service = this.serviceProvider.GetService(param.ParameterType);
                        parameters.Add(service);
                    }

                    var obj = Activator.CreateInstance(method.DeclaringType, parameters.ToArray());

                    dynamic awaitable = method.Invoke(obj, null);
                    await awaitable;
                }
            }
        }
    }
}

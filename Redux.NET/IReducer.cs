namespace Redux.NET
{
    /// <summary>
    /// Contract to make reducers more generic.
    /// </summary>
    public interface IReducer<IState>
    {
        /// <summary>
        /// Execute will reduce the state with the changes.
        /// </summary>
        /// <param name="state">Represents the current state.</param>
        /// <param name="action">Represents the current action.</param>
        /// <returns>A result for the specified state slice</returns>
        IState Execute(IState state, IAction action);
    }
}

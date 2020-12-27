namespace Redux.NET
{
    /// <summary>
    /// IAction is used to make reducers generic.
    /// </summary>
    public interface IAction { }

    /// <summary>
    /// Abstract Action without payload
    /// </summary>
    public abstract class Action : IAction { }

    /// <summary>
    /// Abstract Action with payload
    /// </summary>
    public abstract class Action<T> : IAction
    {
        /// <summary>
        /// Payload can be data for example in create/update.
        /// It can also be data that results from a load complete.
        /// </summary>
        public T Payload { get; private set; }

        /// <summary>
        /// Intialize Action and set payload.
        /// </summary>
        public Action(T payload)
        {
            this.Payload = payload;
        }
    }
}

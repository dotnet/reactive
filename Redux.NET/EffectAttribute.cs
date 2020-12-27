using System;
using System.Collections.Generic;
using System.Linq;

namespace Redux.NET
{
    /// <summary>
    /// Effect attribute can be used to mark side effects.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class EffectAttribute : Attribute
    {
        /// <summary>
        /// A list of actions for which a marked side effect will trigger.
        /// </summary>
        public List<Type> Actions { get; set; }

        /// <summary>
        /// Intialize EffectAttribute with the actions which will trigger the method marked.
        /// </summary>
        /// <param name="values">Multiple Actions can be specified to trigger this side effect.</param>
        public EffectAttribute(params Type[] values)
        {
            this.Actions = values.ToList();
        }
    }
}

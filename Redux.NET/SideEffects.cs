using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Redux.NET
{
    /// <summary>
    /// SideEffects is used to identify marked effects.
    /// </summary>
    public static class SideEffects
    {
        /// <summary>
        /// Property to keep a track of marked methods.
        /// </summary>
        private static IEnumerable<MethodInfo> sideEffects = null;

        /// <summary>
        /// You don't want to search for marked methods every call.
        /// Uisng singleton-ish behavior to set a property once.
        /// </summary>
        /// <returns>Methods which are marked with the side effect attribute.</returns>
        public static IEnumerable<MethodInfo> Find()
        {
            if (sideEffects == null)
            {
                sideEffects = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(x => x.GetTypes())
                    .Where(x => x.IsClass)
                    .SelectMany(x => x.GetMethods())
                    .Where(x => x.GetCustomAttributes(typeof(EffectAttribute), false).FirstOrDefault() != null);
            }
            return sideEffects;
        }
    }
}

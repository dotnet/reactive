// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 
#if CRIPPLED_REFLECTION
using System.Linq;
using System.Reflection;

namespace System.Reflection
{
    [Flags]
    internal enum BindingFlags
    {
        Instance = 4,
        Static = 8,
        Public = 16,
        NonPublic = 32,
    }
}

namespace System
{
    internal static class TypeExtensions
    {
        public static bool IsAssignableFrom(this Type t1, Type t2)
        {
            return t1.GetTypeInfo().IsAssignableFrom(t2.GetTypeInfo());
        }

        public static MethodInfo[] GetMethods(this Type t, BindingFlags flags)
        {
            return t.GetTypeInfo().DeclaredMethods.Where(m => IsVisible(m, flags)).ToArray();
        }

        private static bool IsVisible(MethodInfo method, BindingFlags flags)
        {
            if ((flags & BindingFlags.Public) != 0 != method.IsPublic)
            {
                return false;
            }

            if ((flags & BindingFlags.NonPublic) == 0 && !method.IsPublic)
            {
                return false;
            }

            if ((flags & BindingFlags.Static) != 0 != method.IsStatic)
            {
                return false;
            }

            return true;
        }
    }
}
#endif

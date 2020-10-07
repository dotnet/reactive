// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive
{
    internal static class Helpers
    {
        public static bool All(this bool[] values)
        {
            foreach (var value in values)
            {
                if (!value)
                {
                    return false;
                }
            }

            return true;
        }

        public static bool AllExcept(this bool[] values, int index)
        {
            for (var i = 0; i < values.Length; i++)
            {
                if (i != index)
                {
                    if (!values[i])
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}

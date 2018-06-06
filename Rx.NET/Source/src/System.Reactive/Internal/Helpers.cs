// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;

namespace System.Reactive
{
    internal static class Helpers
    {
        public static int? GetLength<T>(IEnumerable<T> source)
        {
            if (source is T[] array)
                return array.Length;

            if (source is IList<T> list)
                return list.Count;

            return null;
        }

        public static IObservable<T> Unpack<T>(IObservable<T> source)
        {
            var hasOpt = default(bool);

            do
            {
                hasOpt = false;

                if (source is IEvaluatableObservable<T> eval)
                {
                    source = eval.Eval();
                    hasOpt = true;
                }
            } while (hasOpt);

            return source;
        }

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

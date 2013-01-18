// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_PERF
using System;
using System.Collections.Generic;
using System.Reactive.Linq.Observαble;

namespace System.Reactive
{
    internal static class Helpers
    {
        public static int? GetLength<T>(IEnumerable<T> source)
        {
            var array = source as T[];
            if (array != null)
                return array.Length;

            var list = source as IList<T>;
            if (list != null)
                return list.Count;

            return null;
        }

        public static IObservable<T> Unpack<T>(IObservable<T> source)
        {
            var hasOpt = default(bool);

            do
            {
                hasOpt = false;

                var eval = source as IEvaluatableObservable<T>;
                if (eval != null)
                {
                    source = eval.Eval();
                    hasOpt = true;
                }
            } while (hasOpt);

            return source;
        }
    }
}
#endif
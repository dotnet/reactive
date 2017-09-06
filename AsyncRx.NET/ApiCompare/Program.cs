// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;

namespace ApiCompare
{
    class Program
    {
        static void Main()
        {
            var observable = typeof(Observable).GetMethods(BindingFlags.Public | BindingFlags.Static).Select(m => m.Name).Distinct();
            var asyncObservable = typeof(AsyncObservable).GetMethods(BindingFlags.Public | BindingFlags.Static).Select(m => m.Name).Distinct();

            var missing = observable.Except(asyncObservable).OrderBy(m => m);

            foreach (var m in missing)
            {
                Console.WriteLine(m);
            }

            Console.WriteLine();
            Console.WriteLine("Count = " + missing.Count());
        }
    }
}

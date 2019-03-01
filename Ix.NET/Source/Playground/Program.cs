// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Playground
{
    class Program
    {
        static void Main()
        {
            RunDemos();
        }

        [Demo(0, "Random experimentation")]
        static async Task Experiment()
        {
            // Add test code here
            await Task.Yield(); // Suppress CS1998
        }

        [Demo(11, "LINQ to Objects for IEnumerable<T>")]
        static void Linq()
        {
            var xs = new List<int> { 1, 2, 3 };
            var ys = xs.Where(x => x % 2 == 0);

            foreach (var y in ys)
            {
                Console.WriteLine(y);
            }
        }

        [Demo(12, "LINQ to Objects for IQueryable<T>")]
        static void LinqQueryable()
        {
            var xs = new List<int> { 1, 2, 3 }.AsQueryable();
            var ys = xs.Where(x => x % 2 == 0);

            foreach (var y in ys)
            {
                Console.WriteLine(y);
            }
        }

        [Demo(21, "LINQ to Objects for IEnumerable<T> - Interactive Extensions")]
        static void Ix()
        {
            var xs = new List<int> { 1, 2, 3 };
            var ys = xs.Distinct(x => x % 2);

            foreach (var y in ys)
            {
                Console.WriteLine(y);
            }
        }

        [Demo(22, "LINQ to Objects for IQueryable<T> - Interactive Extensions")]
        static void IxQueryable()
        {
            var xs = new List<int> { 1, 2, 3 }.AsQueryable();
            var ys = xs.Distinct(x => x % 2);

            foreach (var y in ys)
            {
                Console.WriteLine(y);
            }
        }

        [Demo(31, "LINQ to Objects for IAsyncEnumerable<T>")]
        static async Task AsyncLinq()
        {
            var xs = new List<int> { 1, 2, 3 };
            var ys = xs.ToAsyncEnumerable().Where(x => x % 2 == 0);

#if USE_AWAIT_FOREACH
            await foreach (var y in ys)
            {
                Console.WriteLine(y);
            }
#else
            var e = ys.GetAsyncEnumerator();

            try
            {
                while (await e.MoveNextAsync())
                {
                    var y = e.Current;

                    Console.WriteLine(y);
                }
            }
            finally
            {
                await e.DisposeAsync();
            }
#endif
        }

        [Demo(32, "LINQ to Objects for IAsyncQueryable<T>")]
        static async Task AsyncLinqQueryable()
        {
            var xs = new List<int> { 1, 2, 3 }.AsQueryable();
            var ys = xs.ToAsyncEnumerable().Where(x => x % 2 == 0);

#if USE_AWAIT_FOREACH
            await foreach (var y in ys)
            {
                Console.WriteLine(y);
            }
#else
            var e = ys.GetAsyncEnumerator();

            try
            {
                while (await e.MoveNextAsync())
                {
                    var y = e.Current;

                    Console.WriteLine(y);
                }
            }
            finally
            {
                await e.DisposeAsync();
            }
#endif
        }

        [Demo(41, "LINQ to Objects for IAsyncEnumerable<T> - Interactive Extensions")]
        static async Task AsyncIx()
        {
            var xs = new List<int> { 1, 2, 3 };
            var ys = xs.ToAsyncEnumerable().Distinct(x => x % 2);

#if USE_AWAIT_FOREACH
            await foreach (var y in ys)
            {
                Console.WriteLine(y);
            }
#else
            await ys.ForEachAsync(y =>
            {
                Console.WriteLine(y);
            });
#endif
        }

        [Demo(42, "LINQ to Objects for IAsyncQueryable<T> - Interactive Extensions")]
        static async Task AsyncIxQueryable()
        {
            var xs = new List<int> { 1, 2, 3 }.AsQueryable();
            var ys = xs.ToAsyncEnumerable().Distinct(x => x % 2);

#if USE_AWAIT_FOREACH
            await foreach (var y in ys)
            {
                Console.WriteLine(y);
            }
#else
            await ys.ForEachAsync(y =>
            {
                Console.WriteLine(y);
            });
#endif
        }

        static void RunDemos()
        {
            var methods = (from method in typeof(Program).GetTypeInfo().GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)
                           let demo = method.GetCustomAttribute<DemoAttribute>()
                           let parameters = method.GetParameters()
                           let returnType = method.ReturnType
                           where demo != null && parameters.Length == 0 && (returnType == typeof(void) || returnType == typeof(Task))
                           orderby demo.Index
                           select new { Demo = demo, Invoke = GetInvoker(method) })
                          .ToArray();

            var invokers = methods.ToDictionary(m => m.Demo.Index, m => m.Invoke);

            while (true)
            {
                foreach (var method in methods)
                {
                    Console.WriteLine($"{method.Demo.Index}. {method.Demo.Title}");
                }

                Console.WriteLine();

                var retry = true;

                while (retry)
                {
                    Console.Write("Enter demo [C: Clear, X: Exit]: ");
                    var input = Console.ReadLine().Trim().ToUpper();

                    switch (input)
                    {
                        case "C":
                            retry = false;
                            Console.Clear();
                            break;
                        case "X":
                            return;
                        default:
                            if (!int.TryParse(input, out var i) || !invokers.TryGetValue(i, out var invoke))
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Invalid input.");
                                Console.ResetColor();
                            }
                            else
                            {
                                retry = false;

                                Console.ForegroundColor = ConsoleColor.Cyan;

                                try
                                {
                                    invoke();
                                }
                                catch (Exception ex)
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine(ex.Message);
                                }
                                finally
                                {
                                    Console.ResetColor();
                                }
                            }

                            break;
                    }
                }
            }

            Action GetInvoker(MethodInfo method)
            {
                if (method.ReturnType == typeof(void))
                {
                    return (Action)method.CreateDelegate(typeof(Action));
                }
                else
                {
                    var invoke = (Func<Task>)method.CreateDelegate(typeof(Func<Task>));
                    return () => invoke().GetAwaiter().GetResult();
                }
            }
        }
    }
}

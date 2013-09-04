// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace ReactiveTests.Stress.Linq
{
    public class Delay
    {
        /// <summary>
        /// Tests OnError messages are propagated all the time.
        /// </summary>
        public static void Errors()
        {
            while (true)
            {
                foreach (var N in new[] { 1, 10, 100, 1000, 10000, 100000 })
                {
                    Console.WriteLine("N = {0}", N);
                    foreach (var d in new[] { 1, 10, 20, 50, 100, 200, 250, 500 })
                    {
                        try
                        {
                            var ex = new Exception();
                            Observable.Range(0, N, NewThreadScheduler.Default).Concat(Observable.Throw<int>(ex)).Delay(TimeSpan.FromMilliseconds(d), NewThreadScheduler.Default).Count().Wait();
                        }
                        catch (Exception)
                        {
                            Console.Write(".");
                            continue;
                        }

                        throw new InvalidOperationException("Didn't throw!");
                    }
                    Console.WriteLine();
                }
            }
        }

        /// <summary>
        /// Tests no OnNext messages are lost.
        /// </summary>
        public static void OnNextMessages()
        {
            while (true)
            {
                foreach (var N in new[] { 1, 10, 100, 1000, 10000, 100000 })
                {
                    Console.WriteLine("N = {0}", N);
                    foreach (var d in new[] { 1, 10, 20, 50, 100, 200, 250, 500 })
                    {
                        var n = Observable.Range(0, N, NewThreadScheduler.Default).Delay(TimeSpan.FromMilliseconds(d), NewThreadScheduler.Default).Count().Wait();
                        if (n != N)
                            throw new InvalidOperationException("Lost OnNext message!");

                        Console.Write(".");
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}

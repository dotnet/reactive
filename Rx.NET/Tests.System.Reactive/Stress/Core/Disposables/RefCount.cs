// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if STRESS
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Disposables;
using System.Reflection;
using System.Threading;

namespace ReactiveTests.Stress.Disposables
{
    public class RefCount
    {
        /// <summary>
        /// Disposes the primary disposable first, allocates a number of dependents on different threads, and disposes them on different threads.
        /// Ref count should reach zero, and the inner disposable should be called.
        /// </summary>
        public static void PrimaryFirst_DependentsTrigger()
        {
            Console.Title = MethodInfo.GetCurrentMethod().Name + " - 0% complete";

            var rnd = new Random();
            
            for (int i = 1; i <= 100; i++)
            {
                Impl(true, false, new[] { 0, 1, 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024 });
                Impl(true, false, new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
                Impl(true, false, Enumerable.Range(0, 10).Select(_ => rnd.Next(0, 1000)));

                Console.Title = MethodInfo.GetCurrentMethod().Name + " - " + i + "% complete";
            }
        }

        /// <summary>
        /// Allocates a number of dependents on different threads, disposes them on different threads, and disposes the primary disposable last.
        /// Ref count should reach zero, and the inner disposable should be called.
        /// </summary>
        public static void DependentsFirst_PrimaryTrigger()
        {
            Console.Title = MethodInfo.GetCurrentMethod().Name + " - 0% complete";

            var rnd = new Random();

            for (int i = 1; i <= 100; i++)
            {
                Impl(false, false, new[] { 0, 1, 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024 });
                Impl(false, false, new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
                Impl(false, false, Enumerable.Range(0, 10).Select(_ => rnd.Next(0, 1000)));

                Console.Title = MethodInfo.GetCurrentMethod().Name + " - " + i + "% complete";
            }
        }

        /// <summary>
        /// Allocates a number of dependents on different threads, disposes them on different threads, and disposes the primary disposable at a random time.
        /// Ref count should reach zero, and the inner disposable should be called.
        /// </summary>
        public static void DependentsFirst_PrimaryRandom()
        {
            Console.Title = MethodInfo.GetCurrentMethod().Name + " - 0% complete";

            var rnd = new Random();

            for (int i = 1; i <= 100; i++)
            {
                Impl(false, true, new[] { 0, 1, 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024 });
                Impl(false, true, new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
                Impl(false, true, Enumerable.Range(0, 10).Select(_ => rnd.Next(0, 1000)));

                Console.Title = MethodInfo.GetCurrentMethod().Name + " - " + i + "% complete";
            }
        }

        private static void Impl(bool primaryFirst, bool primaryRandom, IEnumerable<int> nDependents)
        {
            var rand = new Random();

            foreach (var n in nDependents)
            {
                var e = new ManualResetEvent(false);
                var hasDependent = new ManualResetEvent(false);
                var r = new RefCountDisposable(Disposable.Create(() => { e.Set(); }));

                var d = default(IDisposable);
                if (primaryFirst)
                {
                    d = r.GetDisposable();
                    r.Dispose();
                }
                else if (primaryRandom)
                {
                    var sleep = rand.Next(0, 10) == 0 /* 10% chance */ ? rand.Next(2, 100) : 0;

                    ThreadPool.QueueUserWorkItem(_ =>
                    {
                        hasDependent.WaitOne();
                        Helpers.SleepOrSpin(sleep);
                        r.Dispose();
                    });

                    if (n == 0)
                        hasDependent.Set();
                }

                Console.Write(n + " - ");

                var cd = new CountdownEvent(n * 2);
                for (int i = 0; i < n; i++)
                {
                    var j = i;

                    var sleep1 = rand.Next(0, 10) == 0 /* 10% chance */ ? rand.Next(2, 100) : 0;
                    var sleep2 = rand.Next(0, 10) == 0 /* 10% chance */ ? rand.Next(2, 100) : 0;
                    var sleep3 = rand.Next(0, 10) == 0 /* 10% chance */ ? rand.Next(2, 100) : 0;

                    ThreadPool.QueueUserWorkItem(_ =>
                    {
                        Helpers.SleepOrSpin(sleep1);

                        Console.Write("+");

                        var f = r.GetDisposable();

                        if (j == 0)
                            hasDependent.Set();

                        Helpers.SleepOrSpin(sleep2);

                        ThreadPool.QueueUserWorkItem(__ =>
                        {
                            Helpers.SleepOrSpin(sleep3);

                            f.Dispose();

                            Console.Write("-");

                            cd.Signal();
                        });

                        cd.Signal();
                    });
                }

                cd.Wait();

                if (primaryFirst)
                    d.Dispose();
                else if (!primaryRandom)
                    r.Dispose();

                e.WaitOne();

                Console.WriteLine(".");
            }
        }
    }
}
#endif
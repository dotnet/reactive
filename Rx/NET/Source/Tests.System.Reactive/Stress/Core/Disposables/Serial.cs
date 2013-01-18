// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if STRESS
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reflection;
using System.Threading;

namespace ReactiveTests.Stress.Disposables
{
    public class Serial
    {
        /// <summary>
        /// Allocates a SerialDisposable and performs random assignment operations. Checks that all contained disposables get properly disposed.
        /// The SerialDisposable is disposed either at the start, at the end, or at a random time.
        /// </summary>
        public static void RandomAssignAndDispose()
        {
            Console.Title = MethodInfo.GetCurrentMethod().Name + " - 0% complete";

            for (int i = 1; i <= 100; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    DisposeBeforeAssign();
                    DisposeDuringAssign();
                    DisposeDuringAssign();
                    DisposeDuringAssign();
                    DisposeAfterAssign();
                }

                Console.Title = MethodInfo.GetCurrentMethod().Name + " - " + i + "% complete";
            }
        }

        /// <summary>
        /// Allocates a SerialDisposable and performs random assignment operations. Checks that all contained disposables get properly disposed.
        /// The SerialDisposable is disposed at the start.
        /// </summary>
        public static void DisposeBeforeAssign()
        {
            Impl(0);
        }

        /// <summary>
        /// Allocates a SerialDisposable and performs random assignment operations. Checks that all contained disposables get properly disposed.
        /// The SerialDisposable is disposed at a random time.
        /// </summary>
        public static void DisposeDuringAssign()
        {
            Impl(1);
        }

        /// <summary>
        /// Allocates a SerialDisposable and performs random assignment operations. Checks that all contained disposables get properly disposed.
        /// The SerialDisposable is disposed at the end.
        /// </summary>
        public static void DisposeAfterAssign()
        {
            Impl(2);
        }

        static void Impl(int disposeAt)
        {
            var rand = new Random();

            var s = new SerialDisposable();

            Console.Write("Dispose @ = {0} - ", disposeAt);

            if (disposeAt == 0)
            {
                s.Dispose();
                Console.Write("{SD} ");
            }

            if (disposeAt == 1)
            {
                var sleep = rand.Next(0, 5) > 1 /* 60% chance */ ? rand.Next(2, 1000) : 0;

                ThreadPool.QueueUserWorkItem(_ =>
                {
                    Helpers.SleepOrSpin(sleep);
                    s.Dispose();
                    Console.Write("{SD} ");
                });
            }

            var n = rand.Next(0, 1000);
            var cd = new CountdownEvent(n);

            var ds = Enumerable.Range(0, n).Select(_ => Disposable.Create(() => cd.Signal())).ToArray();

            var m = rand.Next(1, 100);
            var jobs = ds.GroupBy(_ => rand.Next() % m).Select(Enumerable.ToList).ToList();

            Console.Write("N = {0}, M = {1} - ", n, m);

            var done = new CountdownEvent(jobs.Count);

            foreach (var job in jobs)
            {
                var sleep = rand.Next(0, 10) == 0 /* 10% chance */ ? rand.Next(2, 100) : 0;
                var sleepAt = Enumerable.Range(0, rand.Next(0, job.Count) / rand.Next(1, 100)).ToArray();
                var sleeps = sleepAt.Select(_ => rand.Next(0, 50)).ToArray();

                var rem = rand.Next(0, 3) == 0; /* 33% chance */
                var remAt = rand.Next(0, 10) == 0 /* 10% chance */ ? rand.Next(2, 100) : 0;

                var mine = job;
                ThreadPool.QueueUserWorkItem(_ =>
                {
                    Helpers.SleepOrSpin(sleep);

                    var j = 0;
                    foreach (var d in mine)
                    {
                        if (sleepAt.Contains(j))
                            Helpers.SleepOrSpin(sleeps[j]);

                        s.Disposable = d;
                        Console.Write("+");

                        j++;
                    }

                    done.Signal();
                });
            }

            done.Wait();

            if (disposeAt == 2)
            {
                s.Dispose();
                Console.Write("{SD} ");
            }

            cd.Wait();

            Console.WriteLine(".");
        }
    }
}
#endif
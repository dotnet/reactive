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
    public class Composite
    {
        /// <summary>
        /// Allocates a CompositeDisposable and performs random Add and Remove operations. Checks that all contained disposables get properly disposed.
        /// The CompositeDisposable is disposed either at the start, at the end, or at a random time.
        /// </summary>
        public static void Potpourri()
        {
            Console.Title = MethodInfo.GetCurrentMethod().Name + " - 0% complete";

            for (int i = 1; i <= 100; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    DisposeBeforeAddRemove();
                    DisposeDuringAddRemove();
                    DisposeDuringAddRemove();
                    DisposeDuringAddRemove();
                    DisposeAfterAddRemove();
                }

                Console.Title = MethodInfo.GetCurrentMethod().Name + " - " + i + "% complete";
            }
        }

        /// <summary>
        /// Allocates a CompositeDisposable and performs random Add and Remove operations. Checks that all contained disposables get properly disposed.
        /// The CompositeDisposable is disposed at the start.
        /// </summary>
        public static void DisposeBeforeAddRemove()
        {
            Impl(0);
        }

        /// <summary>
        /// Allocates a CompositeDisposable and performs random Add and Remove operations. Checks that all contained disposables get properly disposed.
        /// The CompositeDisposable is disposed at a random time.
        /// </summary>
        public static void DisposeDuringAddRemove()
        {
            Impl(1);
        }

        /// <summary>
        /// Allocates a CompositeDisposable and performs random Add and Remove operations. Checks that all contained disposables get properly disposed.
        /// The CompositeDisposable is disposed at the end.
        /// </summary>
        public static void DisposeAfterAddRemove()
        {
            Impl(2);
        }

        static void Impl(int disposeAt)
        {
            var rand = new Random();

            var g = new CompositeDisposable();

            Console.Write("Dispose @ = {0} - ", disposeAt);

            if (disposeAt == 0)
            {
                g.Dispose();
                Console.Write("{GD} ");
            }

            if (disposeAt == 1)
            {
                var sleep = rand.Next(0, 5) > 1 /* 60% chance */ ? rand.Next(2, 1000) : 0;

                ThreadPool.QueueUserWorkItem(_ =>
                {
                    Helpers.SleepOrSpin(sleep);
                    g.Dispose();
                    Console.Write("{GD} ");
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
                        var dd = d;

                        if (sleepAt.Contains(j))
                            Helpers.SleepOrSpin(sleeps[j]);

                        g.Add(dd);
                        Console.Write("+");

                        if (rem)
                        {
                            ThreadPool.QueueUserWorkItem(__ =>
                            {
                                Helpers.SleepOrSpin(remAt);
                                g.Remove(dd);
                                Console.Write("-");
                            });
                        }

                        j++;
                    }

                    done.Signal();
                });
            }

            done.Wait();

            if (disposeAt == 2)
            {
                g.Dispose();
                Console.Write("{GD} ");
            }

            cd.Wait();

            Console.WriteLine(".");
        }
    }
}
#endif
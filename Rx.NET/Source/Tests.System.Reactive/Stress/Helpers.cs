// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if STRESS
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Linq;

namespace ReactiveTests.Stress
{
    public static class Helpers
    {
        public static void RunWithMemoryPressure(int minBytes, int maxBytes, double activePercent, Action a)
        {
            var started = new ManualResetEvent(false);
            var stopped = 0;

            var avg = (minBytes + maxBytes) / 2;
            var MIN = avg / 1000;
            var MAX = avg / 10;

            var allocator = new Thread(() =>
            {
                var rand = new Random();
                var roots = new List<byte[]>();
                var bytes = 0;

                while (bytes < avg)
                {
                    var n = rand.Next(MIN, MAX);
                    bytes += n;
                    roots.Add(new byte[n]);
                }

                started.Set();

                long avgSum = 0;
                long count = 0;

                const int DEC = 0;
                const int INC = 1;

                var trendPhase = 0;
                var trendLength = 0;

                while (Thread.VolatileRead(ref stopped) == 0)
                {
                    if (trendLength-- == 0)
                    {
                        trendPhase = rand.Next(0, 1000) % 2;
                        trendLength = rand.Next(1, 10);
                    }

                    var mem = TimeSpan.Zero;

                    var sw = Stopwatch.StartNew();

                    var busy = new Stopwatch();

                    while (Thread.VolatileRead(ref stopped) == 0 && (double)mem.Ticks / (double)sw.ElapsedTicks < activePercent)
                    {
                        busy.Restart();

                        var runFor = rand.Next(10, 100);
                        while (busy.ElapsedMilliseconds < runFor)
                        {
                            if (trendPhase == INC)
                            {
                                if (bytes < maxBytes)
                                {
                                    var n = rand.Next(MIN, MAX);
                                    bytes += n;
                                    roots.Add(new byte[n]);
                                    continue;
                                }
                                else
                                {
                                    trendPhase = DEC;
                                }
                            }

                            if (trendPhase == DEC)
                            {
                                if (bytes > minBytes)
                                {
                                    if (roots.Count > 0)
                                    {
                                        var i = rand.Next(0, roots.Count);
                                        bytes -= roots[i].Length;
                                        roots.RemoveAt(i);
                                    }
                                    continue;
                                }
                                else
                                {
                                    trendPhase = INC;
                                }
                            }
                        }

                        mem += busy.Elapsed;
                    }

                    var sleepFor = rand.Next(100, 1000);
                    Thread.Sleep(sleepFor);

                    avgSum += bytes;
                    count++;
                    //Console.WriteLine(bytes + " - Avg = " + avgSum / count);
                }
            });

            allocator.Start();
            started.WaitOne();

            try
            {
                a();
            }
            finally
            {
                Interlocked.Exchange(ref stopped, 1);
                allocator.Join();
            }
        }

        public static void RunWithProcessorPressure(int threadCount, int lockCount, double activePercent, double lockChancePercent, Action a)
        {
            var stopped = 0;
            var started = new CountdownEvent(threadCount);

            var ts = new Thread[threadCount];
            var locks = Enumerable.Range(0, lockCount).Select(_ => new object()).ToArray();

            for (int i = 0; i < threadCount; i++)
            {
                var id = i;

                var t = ts[i] = new Thread(() =>
                {
                    var rand = new Random();

                    started.Signal();

                    var sw = Stopwatch.StartNew();
                    var run = TimeSpan.Zero;

                    while (Thread.VolatileRead(ref stopped) == 0)
                    {
                        var busy = new Stopwatch();

                        while (Thread.VolatileRead(ref stopped) == 0 && (double)run.Ticks / (double)sw.ElapsedTicks < activePercent)
                        {
                            busy.Restart();

                            const int RUN = 0;
                            const int BLOCK = 1;

                            var action = lockCount > 0 && rand.Next() % 100 <= lockChancePercent * 100 ? BLOCK : RUN;

                            switch (action)
                            {
                                case RUN:
                                    //Console.WriteLine("~" + id);
                                    while (busy.ElapsedMilliseconds < 10)
                                        ;
                                    break;
                                case BLOCK:
                                    //Console.WriteLine("!" + id);
                                    lock (locks[rand.Next(0, lockCount)])
                                        Thread.Sleep(rand.Next(100, 1000));
                                    break;
                            }

                            run += busy.Elapsed;
                        }

                        Thread.Sleep(rand.Next(100, 1000));
                    }
                });

                t.Start();
            }

            started.Wait();

            try
            {
                a();
            }
            finally
            {
                Interlocked.Exchange(ref stopped, 1);
                foreach (var t in ts)
                    t.Join();
            }
        }

        public static void SleepOrSpin(int ms)
        {
            if (ms == 0)
                return;

            if (ms % 2 == 0)
            {
                var sw = Stopwatch.StartNew();
                while (sw.Elapsed.TotalMilliseconds < ms)
                    ;
            }
            else
                Thread.Sleep(ms);
        }
    }
}
#endif
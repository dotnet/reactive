// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if STRESS
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reflection;
using System.Threading;

namespace ReactiveTests.Stress.Linq
{
    public class FromEvent
    {
        private static Lazy<Random> s_rand = new Lazy<Random>();

        /// <summary>
        /// Multiple threads are subscribing to a FromEventPattern sequence and disposing their subscriptions.
        /// While this is going on, one consumer does not want to be disturbed while receiving the sequence.
        /// 
        /// Runs a set of combinations of the RefCount_* tests.
        /// </summary>
        public static void RefCount_Mix()
        {
            Console.Title = MethodInfo.GetCurrentMethod().Name + " - 0% complete";

            for (int i = 1; i <= 100; i++)
            {
                var repeatCount = 10;

                foreach (var msgCount in new[] { 100, 1000, 10000, 100000 })
                {
                    // concurrency level {10, 20, ..., 100}
                    RefCount_ConcurrencyLevel_Linear(msgCount, repeatCount, 10, 100, 10);

                    // concurrency level {100, 200, ..., 1000}
                    RefCount_ConcurrencyLevel_Linear(msgCount, repeatCount, 100, 1000, 100);

                    // concurrency level {1, 2, 4, ..., 65536}
                    RefCount_ConcurrencyLevel_Exponential(msgCount, repeatCount, 1, 65536, 2);
                }

                foreach (var maxMsgCount in new[] { 10, 100, 1000, 10000, 100000 })
                {
                    foreach (var maxConcurrency in new[] { 10, 100, 1000, 10000, 100000 })
                    {
                        RefCount_Rand(repeatCount, maxMsgCount, maxConcurrency);
                    }
                }

                Console.Title = MethodInfo.GetCurrentMethod().Name + " - " + i + "% complete";
            }
        }

        /// <summary>
        /// Multiple threads are subscribing to a FromEventPattern sequence and disposing their subscriptions.
        /// While this is going on, one consumer does not want to be disturbed while receiving the sequence.
        /// Subscriptions are happening on the ThreadPool, possibly causing (expected) time gaps.
        /// 
        /// Runs a set of combinations of the RefCount_* tests.
        /// </summary>
        public static void RefCountWithPost_Mix()
        {
            Console.Title = MethodInfo.GetCurrentMethod().Name + " - 0% complete";

            for (int i = 1; i <= 100; i++)
            {
                var repeatCount = 10;

                foreach (var msgCount in new[] { 100, 1000, 10000, 100000 })
                {
                    // concurrency level {10, 20, ..., 100}
                    RefCountWithPost_ConcurrencyLevel_Linear(msgCount, repeatCount, 10, 100, 10);

                    // concurrency level {100, 200, ..., 1000}
                    RefCountWithPost_ConcurrencyLevel_Linear(msgCount, repeatCount, 100, 1000, 100);

                    // concurrency level {1, 2, 4, ..., 65536}
                    RefCountWithPost_ConcurrencyLevel_Exponential(msgCount, repeatCount, 1, 65536, 2);
                }

                foreach (var maxMsgCount in new[] { 10, 100, 1000, 10000, 100000 })
                {
                    foreach (var maxConcurrency in new[] { 10, 100, 1000, 10000, 100000 })
                    {
                        RefCountWithPost_Rand(repeatCount, maxMsgCount, maxConcurrency);
                    }
                }

                Console.Title = MethodInfo.GetCurrentMethod().Name + " - " + i + "% complete";
            }
        }

        /// <summary>
        /// Multiple threads are subscribing to a FromEventPattern sequence and disposing their subscriptions.
        /// While this is going on, one consumer does not want to be disturbed while receiving the sequence.
        /// 
        /// Uses random parameters for the number of messages and the level of concurrency.
        /// </summary>
        /// <param name="n">Number of iterations.</param>
        /// <param name="maxN">Maximum number of message.</param>
        /// <param name="maxM">Maximum level of concurrency.</param>
        public static void RefCount_Rand(int n, int maxN, int maxM)
        {
            RefCount_(RefCount_Rand_Params(n, maxN, maxM));
        }

        /// <summary>
        /// Multiple threads are subscribing to a FromEventPattern sequence and disposing their subscriptions.
        /// While this is going on, one consumer does not want to be disturbed while receiving the sequence.
        /// Subscriptions are happening on the ThreadPool, possibly causing (expected) time gaps.
        /// 
        /// Uses random parameters for the number of messages and the level of concurrency.
        /// </summary>
        /// <param name="n">Number of iterations.</param>
        /// <param name="maxN">Maximum number of message.</param>
        /// <param name="maxM">Maximum level of concurrency.</param>
        public static void RefCountWithPost_Rand(int n, int maxN, int maxM)
        {
            RefCountWithPost_(RefCount_Rand_Params(n, maxN, maxM));
        }

        private static IEnumerable<Tuple<int, int>> RefCount_Rand_Params(int n, int maxN, int maxM)
        {
            for (int i = 0; i < n; i++)
            {
                var N = s_rand.Value.Next(1, maxN);
                var M = s_rand.Value.Next(1, maxM);

                yield return new Tuple<int, int>(N, M);
            }
        }

        /// <summary>
        /// Multiple threads are subscribing to a FromEventPattern sequence and disposing their subscriptions.
        /// While this is going on, one consumer does not want to be disturbed while receiving the sequence.
        /// 
        /// Uses linear increments for the concurrency level.
        /// </summary>
        /// <param name="N">Number of messages.</param>
        /// <param name="n">Number of iterations.</param>
        /// <param name="min">Minimum level of concurrency.</param>
        /// <param name="max">Maximum level of concurrency.</param>
        /// <param name="step">Additive step size to increase level of concurrency.</param>
        public static void RefCount_ConcurrencyLevel_Linear(int N, int n, int min, int max, int step)
        {
            RefCount_(RefCount_ConcurrencyLevel_Linear_Params(N, n, min, max, step));
        }

        /// <summary>
        /// Multiple threads are subscribing to a FromEventPattern sequence and disposing their subscriptions.
        /// While this is going on, one consumer does not want to be disturbed while receiving the sequence.
        /// Subscriptions are happening on the ThreadPool, possibly causing (expected) time gaps.
        /// 
        /// Uses linear increments for the concurrency level.
        /// </summary>
        /// <param name="N">Number of messages.</param>
        /// <param name="n">Number of iterations.</param>
        /// <param name="min">Minimum level of concurrency.</param>
        /// <param name="max">Maximum level of concurrency.</param>
        /// <param name="step">Additive step size to increase level of concurrency.</param>
        public static void RefCountWithPost_ConcurrencyLevel_Linear(int N, int n, int min, int max, int step)
        {
            RefCountWithPost_(RefCount_ConcurrencyLevel_Linear_Params(N, n, min, max, step));
        }

        private static IEnumerable<Tuple<int, int>> RefCount_ConcurrencyLevel_Linear_Params(int N, int n, int min, int max, int step)
        {
            for (int i = 0; i < n; i++)
            {
                for (int M = min; M <= max; M += step)
                {
                    yield return new Tuple<int, int>(N, M);
                }
            }
        }

        /// <summary>
        /// Multiple threads are subscribing to a FromEventPattern sequence and disposing their subscriptions.
        /// While this is going on, one consumer does not want to be disturbed while receiving the sequence.
        /// 
        /// Uses exponential increments for the concurrency level.
        /// </summary>
        /// <param name="N">Number of messages.</param>
        /// <param name="n">Number of iterations.</param>
        /// <param name="min">Minimum level of concurrency.</param>
        /// <param name="max">Maximum level of concurrency.</param>
        /// <param name="step">Multiplicative step size to increase level of concurrency.</param>
        public static void RefCount_ConcurrencyLevel_Exponential(int N, int n, int min, int max, int step)
        {
            RefCount_(RefCount_ConcurrencyLevel_Exponential_Params(N, n, min, max, step));
        }

        /// <summary>
        /// Multiple threads are subscribing to a FromEventPattern sequence and disposing their subscriptions.
        /// While this is going on, one consumer does not want to be disturbed while receiving the sequence.
        /// Subscriptions are happening on the ThreadPool, possibly causing (expected) time gaps.
        /// 
        /// Uses exponential increments for the concurrency level.
        /// </summary>
        /// <param name="N">Number of messages.</param>
        /// <param name="n">Number of iterations.</param>
        /// <param name="min">Minimum level of concurrency.</param>
        /// <param name="max">Maximum level of concurrency.</param>
        /// <param name="step">Multiplicative step size to increase level of concurrency.</param>
        public static void RefCountWithPost_ConcurrencyLevel_Exponential(int N, int n, int min, int max, int step)
        {
            RefCountWithPost_(RefCount_ConcurrencyLevel_Exponential_Params(N, n, min, max, step));
        }

        private static IEnumerable<Tuple<int, int>> RefCount_ConcurrencyLevel_Exponential_Params(int N, int n, int min, int max, int step)
        {
            for (int i = 0; i < n; i++)
            {
                for (int M = min; M <= max; M *= step)
                {
                    yield return new Tuple<int, int>(N, M);
                }
            }
        }

        private static void RefCount_(IEnumerable<Tuple<int, int>> parameters)
        {
            foreach (var p in parameters)
            {
                var N = p.Item1;
                var M = p.Item2;

                Console.Write("N = {0}, M = {1} - ", N, M);

                var bar = new Bar();

                var foo = Observable.FromEventPattern<FooEventArgs>(h => { Console.Write("+"); bar.Foo += h; }, h => { bar.Foo -= h; Console.Write("-"); });

                var res = new List<int>();
                var n = 0;
                var e = new ManualResetEvent(false);

                var cd = new CountdownEvent(M * 2);
                for (int i = 0; i < M; i++)
                {
                    var f = new SingleAssignmentDisposable();

                    ThreadPool.QueueUserWorkItem(_ =>
                    {
                        f.Disposable = foo.Subscribe(__ => { Console.Write("!"); });
                        cd.Signal();
                    });

                    ThreadPool.QueueUserWorkItem(_ =>
                    {
                        f.Dispose();
                        cd.Signal();
                    });
                }

                Console.Write("{SB}");

                var d = foo.Subscribe(x =>
                {
                    //Console.Write("&");

                    if (++n == N)
                        e.Set();

                    res.Add(x.EventArgs.Qux);
                });

                Console.Write("{SE}");

                var t = new Thread(() =>
                {
                    Console.Write("{TB}");

                    for (int i = 0; i < N; i++)
                        bar.OnFoo(i);

                    Console.Write("{TE}");
                });

                t.Start();
                t.Join();

                cd.Wait();

                e.WaitOne();
                d.Dispose();

                if (!res.SequenceEqual(Enumerable.Range(0, N)))
                {
                    Console.WriteLine("Panic!");
                    break;
                }

                Console.WriteLine(".");
            }
        }

        private static void RefCountWithPost_(IEnumerable<Tuple<int, int>> parameters)
        {
            var worker = new Thread(() =>
            {
                SynchronizationContext.SetSynchronizationContext(new MySyncCtx());

                foreach (var p in parameters)
                {
                    var N = p.Item1;
                    var M = p.Item2;

                    Console.Write("N = {0}, M = {1} - ", N, M);

                    var bar = new Bar();

                    var foo = Observable.FromEventPattern<FooEventArgs>(h => { /*Console.Write("+");*/ bar.Foo += h; }, h => { bar.Foo -= h; /*Console.Write("-"); */});

                    var e = new ManualResetEvent(false);

                    var cd = new CountdownEvent(M * 2);
                    for (int i = 0; i < M; i++)
                    {
                        var f = new SingleAssignmentDisposable();

                        ThreadPool.QueueUserWorkItem(_ =>
                        {
                            f.Disposable = foo.Subscribe(__ => { /*Console.Write("!");*/ });
                            cd.Signal();
                        });

                        ThreadPool.QueueUserWorkItem(_ =>
                        {
                            f.Dispose();
                            cd.Signal();
                        });
                    }

                    var hasObserved = 0;

                    Console.Write("{SB}");

                    var d = foo.Subscribe(x =>
                    {
                        //
                        // [on BARTDE-M6500 with CPU and RAM pressure]
                        //
                        // Up to 8K concurrent observers, we typically don't see a time gap (expected worst-case behavior).
                        // The code below uses an event to check the desired behavior of eventually tuning in to the event stream.
                        //
                        Console.Write("&" + x.EventArgs.Qux);
                        e.Set();
                        Interlocked.Exchange(ref hasObserved, 1);
                    });

                    Console.Write("{SE}");

                    var t = new Thread(() =>
                    {
                        Console.Write("{TB}");

                        var i = 0;
                        while (Thread.VolatileRead(ref hasObserved) == 0)
                            bar.OnFoo(i++);

                        Console.Write("{TE}");
                    });

                    t.Start();
                    t.Join();

                    cd.Wait();

                    e.WaitOne();
                    d.Dispose();

                    Console.WriteLine(".");
                }
            });

            worker.Start();
            worker.Join();
        }

        class Bar
        {
            public event EventHandler<FooEventArgs> Foo;

            public void OnFoo(int x)
            {
                var foo = Foo;
                if (foo != null)
                    foo(this, new FooEventArgs { Qux = x });
            }
        }

        class FooEventArgs : EventArgs
        {
            public int Qux { get; set; }
        }

        class MySyncCtx : SynchronizationContext
        {
            public override void Post(SendOrPostCallback d, object state)
            {
                ThreadPool.QueueUserWorkItem(_ =>
                {
                    d(state);
                });
            }
        }
    }
}
#endif
// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if STRESS
using System;
using System.Reactive.Disposables;
using System.Reflection;
using System.Threading;

namespace ReactiveTests.Stress.Disposables
{
    public class SingleAssignment
    {
        /// <summary>
        /// Allocates a SingleAssignmentDisposable and assigns a disposable object at a random time. Also disposes the container at a random time.
        /// Expected behavior is to see the assigned disposable getting disposed no matter what.
        /// </summary>
        public static void RandomAssignAndDispose()
        {
            Console.Title = MethodInfo.GetCurrentMethod().Name + " - 0% complete";

            for (int i = 1; i <= 100; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    Impl();
                }

                Console.Title = MethodInfo.GetCurrentMethod().Name + " - " + i + "% complete";
            }
        }

        private static void Impl()
        {
            var rand = new Random();

            for (int i = 0; i < 1000; i++)
            {
                var d = new SingleAssignmentDisposable();
                var e = new ManualResetEvent(false);
                var cd = new CountdownEvent(2);

                var sleep1 = rand.Next(0, 1) == 0 ? 0 : rand.Next(2, 100);
                var sleep2 = rand.Next(0, 1) == 0 ? 0 : rand.Next(2, 100);

                ThreadPool.QueueUserWorkItem(_ =>
                {
                    Helpers.SleepOrSpin(sleep1);

                    Console.Write("{DB} ");
                    d.Dispose();
                    Console.Write("{DE} ");

                    cd.Signal();
                });

                ThreadPool.QueueUserWorkItem(_ =>
                {
                    Helpers.SleepOrSpin(sleep2);

                    Console.Write("{AB} ");
                    d.Disposable = Disposable.Create(() => e.Set());
                    Console.Write("{AE} ");

                    cd.Signal();
                });

                e.WaitOne();
                cd.Wait();

                Console.WriteLine(".");
            }
        }
    }
}
#endif
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

#if STRESS
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace ReactiveTests.Stress.Linq
{
    public class ReplaySubject
    {
        /// <summary>
        /// Tests the ReplaySubject with concurrent subscribers.
        /// </summary>
        public static void ConcurrentSubscribers()
        {
            var N = int.MaxValue;
            var M = int.MaxValue;

            var r = new ReplaySubject<int>(4);

            var rnd = new Random();
            var ts = new List<Task>();

            for (var i = 0; i < 16; i++)
            {
                var rnd2 = new Random(rnd.Next());

                ts.Add(Task.Factory.StartNew(async () =>
                {
                    var n = rnd2.Next(10, 1000);

                    for (var j = 0; j < M; j++)
                    {
                        var xs = new List<int>();
                        await r.Take(n).Scan((x1, x2) =>
                        {
                            if (x2 - x1 != 1)
                                Debugger.Break();

                            if (x2 == 0)
                                Debugger.Break();

                            return x2;
                        }).ForEachAsync(xs.Add);

                        var f = xs.First();
                        if (!xs.SequenceEqual(Enumerable.Range(f, xs.Count)))
                        {
                            Console.WriteLine("FAIL!");
                            Debugger.Break();
                        }
                        else
                        {
                            Console.Write(".");
                        }

                        if (j % 1000 == 0)
                        {
                            await Task.Delay(50);
                        }
                    }
                }));
            }

            for (var i = 0; i < N; i++)
            {
                r.OnNext(i);
            }

            Console.WriteLine("Done!");

            Task.WaitAll(ts.ToArray());
        }
    }
}
#endif
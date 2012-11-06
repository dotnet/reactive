// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_STOPWATCH
using System;
using System.Reactive.Concurrency;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ReactiveTests.Tests
{
    static class StopwatchTest
    {
        public static void Run(IStopwatchProvider stopwatchProvider)
        {
            /*
             * TODO: Temporarily disabled until we iron out all of the scheduler improvements.
             */

            //var N = 10;
            //var t = default(long);
            //var d = 1;

            //for (int i = 0; i < N; i++)
            //{
            //    var sw = stopwatchProvider.StartStopwatch();

            //    var e1 = sw.Elapsed;
            //    Thread.Sleep(d);
            //    var e2 = sw.Elapsed;

            //    Assert.IsTrue(e2.Ticks > e1.Ticks);
            //    t += (e2 - e1).Ticks;

            //    sw.Dispose();
            //}

            //Assert.IsTrue(TimeSpan.FromTicks(t / N).TotalMilliseconds < d * 10 /* 10x margin */);
        }
    }
}
#endif
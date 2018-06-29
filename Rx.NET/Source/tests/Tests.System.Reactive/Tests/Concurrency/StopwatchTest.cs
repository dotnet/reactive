// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;

namespace ReactiveTests.Tests
{
    internal static class StopwatchTest
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

            //    Assert.True(e2.Ticks > e1.Ticks);
            //    t += (e2 - e1).Ticks;

            //    sw.Dispose();
            //}

            //Assert.True(TimeSpan.FromTicks(t / N).TotalMilliseconds < d * 10 /* 10x margin */);
        }
    }
}

// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Reactive.Concurrency;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ReactiveTests.Tests
{
    [TestClass]
    public class ConcurrencyTest
    {
        [TestMethod]
        public void CurrentScheduler_EnsureTrampoline()
        {
            const int concurrency = 100;

            var passed = true;

            var s = new Semaphore(0, int.MaxValue);
            var e = new ManualResetEvent(false);

            for (var i = 0; i < concurrency; ++i)
                NewThreadScheduler.Default.Schedule(() =>
                    {
                        e.WaitOne();
                        try
                        {
                            if (Scheduler.CurrentThread.ScheduleRequired)
                                Scheduler.CurrentThread.Schedule(() => { });
                            else
                                new Action(() => { })();
                        }
                        catch (NullReferenceException)
                        {
                            passed = false;
                        }
                        finally
                        {
                            s.Release();
                        }
                    });

            e.Set();

            for (var i = 0; i < concurrency; ++i)
                s.WaitOne();

            Assert.IsTrue(passed);
        }

        [TestMethod]
        public void CurrentScheduler_Schedule()
        {
            const int concurrency = 100;

            var passed = true;

            var s = new Semaphore(0, int.MaxValue);
            var e = new ManualResetEvent(false);

            for (var i = 0; i < concurrency; ++i)
                NewThreadScheduler.Default.Schedule(() =>
                {
                    e.WaitOne();
                    try
                    {
                        if (Scheduler.CurrentThread.ScheduleRequired)
                            Scheduler.CurrentThread.Schedule(() => { });
                        else
                            new Action(() => { })();
                    }
                    catch (NullReferenceException)
                    {
                        passed = false;
                    }
                    finally
                    {
                        s.Release();
                    }
                });

            e.Set();

            for (var i = 0; i < concurrency; ++i)
                s.WaitOne();

            Assert.IsTrue(passed);
        }
    }
}

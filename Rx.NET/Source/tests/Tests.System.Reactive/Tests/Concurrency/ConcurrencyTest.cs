// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Reactive.Concurrency;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Assert = Xunit.Assert;

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
            {
                NewThreadScheduler.Default.Schedule(() =>
                    {
                        e.WaitOne();
                        try
                        {
                            if (Scheduler.CurrentThread.ScheduleRequired)
                            {
                                Scheduler.CurrentThread.Schedule(() => { });
                            }
                            else
                            {
                                new Action(() => { })();
                            }
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
            }

            e.Set();

            for (var i = 0; i < concurrency; ++i)
            {
                s.WaitOne();
            }

            Assert.True(passed);
        }

        [TestMethod]
        public void CurrentScheduler_Schedule()
        {
            const int concurrency = 100;

            var passed = true;

            var s = new Semaphore(0, int.MaxValue);
            var e = new ManualResetEvent(false);

            for (var i = 0; i < concurrency; ++i)
            {
                NewThreadScheduler.Default.Schedule(() =>
                {
                    e.WaitOne();
                    try
                    {
                        if (Scheduler.CurrentThread.ScheduleRequired)
                        {
                            Scheduler.CurrentThread.Schedule(() => { });
                        }
                        else
                        {
                            new Action(() => { })();
                        }
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
            }

            e.Set();

            for (var i = 0; i < concurrency; ++i)
            {
                s.WaitOne();
            }

            Assert.True(passed);
        }
    }
}

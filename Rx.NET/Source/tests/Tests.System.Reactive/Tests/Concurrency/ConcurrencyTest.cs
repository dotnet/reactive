// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Reactive.Concurrency;
using System.Threading;
using Xunit;

namespace ReactiveTests.Tests
{

    public class ConcurrencyTest
    {
        [Fact]
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

        [Fact]
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

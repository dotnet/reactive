// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Threading;
using System.Threading.Tasks;
using Xunit;


namespace ReactiveTests
{
    public class VirtualTimeAsyncSchedulerTests
    {
        private class VirtualSchedulerTestScheduler : PriorityQueueVirtualTimeAsyncScheduler<string, char>
        {
            public VirtualSchedulerTestScheduler()
            {
            }

            public VirtualSchedulerTestScheduler(string initialClock, IComparer<string> comparer)
                : base(initialClock, comparer)
            {
            }

            protected override string Add(string absolute, char relative)
            {
                return (absolute ?? string.Empty) + relative;
            }

            protected override DateTimeOffset ToDateTimeOffset(string absolute)
            {
                return new DateTimeOffset((absolute ?? string.Empty).Length, TimeSpan.Zero);
            }

            protected override char ToRelative(TimeSpan timeSpan)
            {
                return (char)(timeSpan.Ticks % char.MaxValue);
            }
        }

        // Is this supposed to be a [Fact] with AsyncRX.NET?
        public async Task Virtual_ScheduleAction()
        {
            var id = Thread.CurrentThread.ManagedThreadId;
            var ran = false;
            var scheduler = new VirtualSchedulerTestScheduler();
            await scheduler.ScheduleAsync(_ => { Assert.Equal(id, Thread.CurrentThread.ManagedThreadId); ran = true; return Task.CompletedTask; });
            await scheduler.Start();
            Assert.True(ran);
        }


        [Fact]
        public async Task Virtual_ScheduleActionError()
        {
            var ex = new Exception();

            try
            {
                var scheduler = new VirtualSchedulerTestScheduler();
                await scheduler.ScheduleAsync(_ => { throw ex; });
                await scheduler.Start();
                Assert.True(false);
            }
            catch (Exception e)
            {
                Assert.Same(e, ex);
            }
        }

        [Fact]
        public void Virtual_InitialAndComparer_Now()
        {
            var s = new VirtualSchedulerTestScheduler("Bar", Comparer<string>.Default);
            Assert.Equal(3, s.Now.Ticks);
        }

        [Fact]
        public async Task Immediate_ScheduleAction()
        {
            var id = Thread.CurrentThread.ManagedThreadId;
            var ran = false;
            await ImmediateAsyncScheduler.Instance.ScheduleAsync(_ => { Assert.Equal(id, Thread.CurrentThread.ManagedThreadId); ran = true; return Task.CompletedTask; });
            Assert.True(ran);
        }
    }
}

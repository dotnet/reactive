// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Reactive.Concurrency;
using Xunit;

namespace ReactiveTests.Tests
{

    public class AsyncLockTest
    {
        [Fact]
        public void Wait_ArgumentChecking()
        {
            var asyncLock = new AsyncLock();
            Assert.Throws<ArgumentNullException>(() => asyncLock.Wait(null));
        }

        [Fact]
        public void Wait_Graceful()
        {
            var ok = false;
            new AsyncLock().Wait(() => { ok = true; });
            Assert.True(ok);
        }

        [Fact]
        public void Wait_Fail()
        {
            var l = new AsyncLock();

            var ex = new Exception();
            try
            {
                l.Wait(() => { throw ex; });
                Assert.True(false);
            }
            catch (Exception e)
            {
                Assert.Same(ex, e);
            }

            // has faulted; should not run
            l.Wait(() => { Assert.True(false); });
        }

        [Fact]
        public void Wait_QueuesWork()
        {
            var l = new AsyncLock();

            var l1 = false;
            var l2 = false;
            l.Wait(() => { l.Wait(() => { Assert.True(l1); l2 = true; }); l1 = true; });
            Assert.True(l2);
        }

        [Fact]
        public void Dispose()
        {
            var l = new AsyncLock();

            var l1 = false;
            var l2 = false;
            var l3 = false;
            var l4 = false;

            l.Wait(() =>
            {
                l.Wait(() =>
                {
                    l.Wait(() =>
                    {
                        l3 = true;
                    });

                    l2 = true;

                    l.Dispose();

                    l.Wait(() =>
                    {
                        l4 = true;
                    });
                });

                l1 = true;
            });

            Assert.True(l1);
            Assert.True(l2);
            Assert.False(l3);
            Assert.False(l4);
        }
    }
}

// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !SILVERLIGHT // MethodAccessException
using System;
using System.Reactive.Concurrency;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ReactiveTests.Tests
{
    [TestClass]
    public class AsyncLockTest
    {
        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void Wait_ArgumentChecking()
        {
            var asyncLock = new AsyncLock();
            asyncLock.Wait(null);
        }

        [TestMethod]
        public void Wait_Graceful()
        {
            var ok = false;
            new AsyncLock().Wait(() => { ok = true; });
            Assert.IsTrue(ok);
        }

        [TestMethod]
        public void Wait_Fail()
        {
            var l = new AsyncLock();

            var ex = new Exception();
            try
            {
                l.Wait(() => { throw ex; });
                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.AreSame(ex, e);
            }

            // has faulted; should not run
            l.Wait(() => { Assert.Fail(); });
        }

        [TestMethod]
        public void Wait_QueuesWork()
        {
            var l = new AsyncLock();

            var l1 = false;
            var l2 = false;
            l.Wait(() => { l.Wait(() => { Assert.IsTrue(l1); l2 = true; }); l1 = true; });
            Assert.IsTrue(l2);
        }

        [TestMethod]
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

            Assert.IsTrue(l1);
            Assert.IsTrue(l2);
            Assert.IsFalse(l3);
            Assert.IsFalse(l4);
        }

        public class AsyncLock
        {
            object instance;

            public AsyncLock()
            {
                instance = typeof(Scheduler).Assembly.GetType("System.Reactive.Concurrency.AsyncLock").GetConstructor(new Type[] { }).Invoke(new object[] { });
            }

            public void Wait(Action action)
            {
                try
                {
                    instance.GetType().GetMethod("Wait").Invoke(instance, new object[] { action });
                }
                catch (TargetInvocationException ex)
                {
                    throw ex.InnerException;
                }
            }

            public void Dispose()
            {
                try
                {
                    instance.GetType().GetMethod("Dispose").Invoke(instance, new object[0]);
                }
                catch (TargetInvocationException ex)
                {
                    throw ex.InnerException;
                }
            }
        }
    }
}
#endif
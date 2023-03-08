// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Assert = Xunit.Assert;

namespace Tests.System.Reactive.Tests
{
    [TestClass]
    public class TaskLikeSupportTest
    {
        [TestMethod]
        public async Task Return()
        {
            Assert.Equal(42, await ManOrBoy_Return());
        }

#pragma warning disable 1998
        private async ITaskObservable<int> ManOrBoy_Return()
        {
            return 42;
        }
#pragma warning restore 1998

        [TestMethod]
        public async Task Throw()
        {
            await Assert.ThrowsAsync<DivideByZeroException>(async () => await ManOrBoy_Throw(42, 0));
        }

#pragma warning disable 1998
        private async ITaskObservable<int> ManOrBoy_Throw(int n, int d)
        {
            return n / d;
        }
#pragma warning restore 1998

        [TestMethod]
        public async Task Basics()
        {
            // We set up a synchronization context for the duration of the test, because this test
            // fails without that. When we were on xUnit, SynchronizationContext.Current was never
            // null (because they populate it with their AsyncTestSyncContext, apparently to ensure
            // that async void tests work. But this started failing intermittently when we switched
            // to MSTest, because that takes the more strict view that async void tests should not
            // be encouraged. (It has an analyzer to detect these and warn you about them). So
            // tests in MSTest get the default behaviour, i.e. SynchronizationContext.Current will
            // be null.
            // That causes this test to exercise different code paths than it does when there is a
            // context. AsyncSubject<T>.AwaitObserver.InvokeOnOriginalContext will go via the
            // context if there is one, and invokes its callback synchronously if not.
            // It's quite possible that the failure of this test on MSTest is indicative of a
            // subtle bug in Rx, because I don't see any obvious reason why this should be expected
            // to deadlock in the absence of a synchronization context. (And it doesn't do so if
            // you run the test in isolation. It only happens when running all the tests. So
            // there's some sort of race condition here.) But perhaps not. Maybe there's some
            // subtle reason why you should never attempt to do what this test is doing without a
            // SynchronizationContext. For now, we're providing a context to reproduce what was
            // happening under xUnit, so that this test continues to exercise the same code paths
            // it did before.
            // See https://github.com/dotnet/reactive/issues/1885
            SynchronizationContext ctx = SynchronizationContext.Current;
            try
            {
                SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
                Assert.Equal(45, await ManOrBoy_Basics());
            }
            finally
            {
                SynchronizationContext.SetSynchronizationContext(ctx);
            }
        }

#pragma warning disable 1998
        private async ITaskObservable<int> ManOrBoy_Basics()
        {
            var res = 0;

            for (var i = 0; i < 10; i++)
            {
                switch (i % 4)
                {
                    case 0:
                        res += await Observable.Return(i);
                        break;
                    case 1:
                        res += await Observable.Return(i).Delay(TimeSpan.FromMilliseconds(50));
                        break;
                    case 2:
                        res += await Task.FromResult(i);
                        break;
                    case 3:
                        res += await Task.Run(() => { Task.Delay(50).Wait(); return i; });
                        break;
                }
            }

            return res;
        }
#pragma warning restore 1998
    }
}

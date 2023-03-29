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

        // We execute the ManOrBoy_Basics tests twice, once without a SynchronizationContext, and
        // once without. When we were on xUnit, SynchronizationContext.Current was never null
        // (because they populate it with their AsyncTestSyncContext, apparently to ensure that
        // async void tests work. MSTest takes the more strict view that async void tests should
        // not be encouraged. (It has an analyzer to detect these and warn you about them). So
        // tests in MSTest get the default behaviour (i.e. SynchronizationContext.Current will be
        // null) unless the test sets one up explicitly.
        //
        // The ManOrBoy_Basics tests exercise different code paths depending on the availability of
        // a SynchronizationContext. AsyncSubject<T>.AwaitObserver.InvokeOnOriginalContext will go
        // via the context if there is one, and invokes its callback synchronously if not. This is
        // a significant difference, which is why, now that we can test both ways, we do.
        //
        // When we switched to MSTest, this test started failing intermittently. This seems likely
        // to be indicative of a subtle bug in Rx, because I don't see any obvious reason why this
        // should be expected to deadlock in the absence of a synchronization context. It doesn't
        // do so if you run the test in isolation. It only happens when running all the tests, end
        // even then it often doesn't. Since we modified the build to run tests with "-v n" with
        // the aim of trying to work out which tests were occasionally locking up, the failures
        // stopped, so there's some sort of race condition here that's finely balanced enough to
        // be affected by test settings.) But perhaps not. Maybe there's some subtle reason why you
        // should never attempt to do what this test is doing without a SynchronizationContext.
        // Issue https://github.com/dotnet/reactive/issues/1885 is tracking this until we
        // resolve the root cause.

        [TestMethod]
        public async Task BasicsNoSynchronizationContext()
        {
            Assert.Equal(45, await ManOrBoy_Basics());
        }

        [TestMethod]
        public async Task BasicsWithSynchronizationContext()
        {
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

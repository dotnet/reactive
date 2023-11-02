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
        // once with one. When we were on xUnit, SynchronizationContext.Current was never null
        // because xUnit populates it with their AsyncTestSyncContext, apparently to ensure that
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
        // When we switched to MSTest, and before we had added the tests to run both with and
        // without the SynchronizationContext (meaning we only tested without one) this test
        // started failing intermittently. It eventually became apparent that this was because
        // some test somewhere in the system is setting SynchronizationContext.Current to contain
        // a WindowsFormsSynchronizationContext. That's why this test explicitly sets it to
        // null - if a UI-based context is present, the test will hang because it will attempt
        // to use that context to handle completion but nothing will be running a message loop,
        // so completion never occurs. (It's intermittent because the order in which tests run
        // is not deterministic, so sometimes when this test runs, the SynchronizationContext
        // was already null.)

        [TestMethod]
        public async Task BasicsNoSynchronizationContext()
        {
            var ctx = SynchronizationContext.Current;
            try
            {
                SynchronizationContext.SetSynchronizationContext(null);
                Assert.Equal(45, await ManOrBoy_Basics());
            }
            finally
            {
                SynchronizationContext.SetSynchronizationContext(ctx);
            }
        }

        [TestMethod]
        public async Task BasicsWithSynchronizationContext()
        {
            var ctx = SynchronizationContext.Current;
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
    }
}

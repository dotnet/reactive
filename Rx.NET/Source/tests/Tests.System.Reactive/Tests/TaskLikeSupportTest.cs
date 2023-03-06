// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Reactive;
using System.Reactive.Linq;
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

        // idg10: disabling because this test sometimes never completes on MSTest. The proximate cause for that is
        // that whereas in xUnit, SynchronizationContext.Current will never be null (because they populate
        // it with their AsyncTestSyncContext, apparently to ensure that async void tests work), MSTest
        // takes the more strict view that async void tests should not be encouraged (it has an analyzer
        // to detect these and warn you about them) so we get the default behaviour, i.e.
        // SynchronizationContext.Current will be null. This causes this test to exercise different
        // code paths. AsyncSubject<T>.AwaitObserver.InvokeOnOriginalContext will go via the context if
        // there is one, and invokes its callback synchronously if not.
        // It's quite possible that the failure of this test on MSTest is indicative of a subtle bug
        // in Rx, because I don't see any obvious reason why this should be expected to deadlock in the
        // absence of a synchronization context. (And it doesn't do so if you run the test in isolation.
        // It only happens when running all the tests. So there's some sort of race condition here.)
        // But perhaps not. Maybe there's some subtle reason why you should never attempt to do what
        // this test is doing without a SynchronizationContext.
        // Not all of the cases below fail in this way. If I replace everything except case 0 with
        // "res += i;" the test never fails. But if I then restore case 1 (Return followed by Delay)
        // then we get the failure.
        //[TestMethod]
        public async Task Basics()
        {
            Assert.Equal(45, await ManOrBoy_Basics());
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

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Assert = Xunit.Assert;

namespace ReactiveTests.Tests
{
    [TestClass]
    public class WaitTest : ReactiveTest
    {

        [TestMethod]
        public void Wait_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Wait(default(IObservable<int>)));
        }

        [TestMethod]
        public void Wait_Return()
        {
            var x = 42;
            var xs = Observable.Return(x, ThreadPoolScheduler.Instance);
            var res = xs.Wait();
            Assert.Equal(x, res);
        }

        [TestMethod]
        public void Wait_Empty()
        {
            ReactiveAssert.Throws<InvalidOperationException>(() => Observable.Empty<int>().Wait());
        }

        [TestMethod]
        public void Wait_Throw()
        {
            var ex = new Exception();

            var xs = Observable.Throw<int>(ex);

            ReactiveAssert.Throws(ex, () => xs.Wait());
        }

        [TestMethod]
        public void Wait_Range()
        {
            var n = 42;
            var xs = Observable.Range(1, n, ThreadPoolScheduler.Instance);
            var res = xs.Wait();
            Assert.Equal(n, res);
        }
    }
}

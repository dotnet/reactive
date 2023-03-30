// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ReactiveTests.Tests
{
    [TestClass]
    public class NeverTest : ReactiveTest
    {

        [TestMethod]
        public void Never_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Never<int>().Subscribe(null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Never(42).Subscribe(null));
        }

        [TestMethod]
        public void Never_Basic()
        {
            var scheduler = new TestScheduler();

            var xs = Observable.Never<int>();

            var res = scheduler.CreateObserver<int>();

            xs.Subscribe(res);

            scheduler.Start();

            res.Messages.AssertEqual(
            );
        }

        [TestMethod]
        public void Never_Basic_Witness()
        {
            var scheduler = new TestScheduler();

            var xs = Observable.Never(42);

            var res = scheduler.CreateObserver<int>();

            xs.Subscribe(res);

            scheduler.Start();

            res.Messages.AssertEqual(
            );
        }

    }
}

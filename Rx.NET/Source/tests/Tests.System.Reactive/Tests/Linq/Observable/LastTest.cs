// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using ReactiveTests.Dummies;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Assert = Xunit.Assert;

namespace ReactiveTests.Tests
{
    [TestClass]
    public class LastTest : ReactiveTest
    {

        [TestMethod]
        public void Last_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Last(default(IObservable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Last(default(IObservable<int>), _ => true));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Last(DummyObservable<int>.Instance, default));
        }

        [TestMethod]
        public void Last_Empty()
        {
            ReactiveAssert.Throws<InvalidOperationException>(() => Observable.Empty<int>().Last());
        }

        [TestMethod]
        public void LastPredicate_Empty()
        {
            ReactiveAssert.Throws<InvalidOperationException>(() => Observable.Empty<int>().Last(_ => true));
        }

        [TestMethod]
        public void Last_Return()
        {
            var value = 42;
            Assert.Equal(value, Observable.Return(value).Last());
        }

        [TestMethod]
        public void Last_Throw()
        {
            var ex = new Exception();

            var xs = Observable.Throw<int>(ex);

            ReactiveAssert.Throws(ex, () => xs.Last());
        }

        [TestMethod]
        public void Last_Range()
        {
            var value = 42;
            Assert.Equal(value, Observable.Range(value - 9, 10).Last());
        }

        [TestMethod]
        public void LastPredicate_Range()
        {
            var value = 42;
            Assert.Equal(50, Observable.Range(value, 10).Last(i => i % 2 == 0));
        }

    }
}

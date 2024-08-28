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
    public class SingleTest : ReactiveTest
    {

        [TestMethod]
        public void Single_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Single(default(IObservable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Single(default(IObservable<int>), _ => true));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Single(DummyObservable<int>.Instance, default));
        }

        [TestMethod]
        public void Single_Empty()
        {
            ReactiveAssert.Throws<InvalidOperationException>(() => Observable.Empty<int>().Single());
        }

        [TestMethod]
        public void SinglePredicate_Empty()
        {
            ReactiveAssert.Throws<InvalidOperationException>(() => Observable.Empty<int>().Single(_ => true));
        }

        [TestMethod]
        public void Single_Return()
        {
            var value = 42;
            Assert.Equal(value, Observable.Return(value).Single());
        }

        [TestMethod]
        public void Single_Throw()
        {
            var ex = new Exception();

            var xs = Observable.Throw<int>(ex);

            ReactiveAssert.Throws(ex, () => xs.Single());
        }

        [TestMethod]
        public void Single_Range()
        {
            var value = 42;
            ReactiveAssert.Throws<InvalidOperationException>(() => Observable.Range(value, 10).Single());
        }

        [TestMethod]
        public void SinglePredicate_Range()
        {
            var value = 42;
            ReactiveAssert.Throws<InvalidOperationException>(() => Observable.Range(value, 10).Single(i => i % 2 == 0));
        }

        [TestMethod]
        public void SinglePredicate_Range_ReducesToSingle()
        {
            var value = 42;
            Assert.Equal(45, Observable.Range(value, 10).Single(i => i == 45));
        }


    }
}

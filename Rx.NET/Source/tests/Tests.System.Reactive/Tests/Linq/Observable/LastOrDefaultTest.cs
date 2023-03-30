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
    public class LastOrDefaultTest : ReactiveTest
    {

        [TestMethod]
        public void LastOrDefault_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.LastOrDefault(default(IObservable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.LastOrDefault(default(IObservable<int>), _ => true));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.LastOrDefault(DummyObservable<int>.Instance, default));
        }

        [TestMethod]
        public void LastOrDefault_Empty()
        {
            Assert.Equal(default, Observable.Empty<int>().LastOrDefault());
        }

        [TestMethod]
        public void LastOrDefaultPredicate_Empty()
        {
            Assert.Equal(default, Observable.Empty<int>().LastOrDefault(_ => true));
        }

        [TestMethod]
        public void LastOrDefault_Return()
        {
            var value = 42;
            Assert.Equal(value, Observable.Return(value).LastOrDefault());
        }

        [TestMethod]
        public void LastOrDefault_Throw()
        {
            var ex = new Exception();

            var xs = Observable.Throw<int>(ex);

            ReactiveAssert.Throws(ex, () => xs.LastOrDefault());
        }

        [TestMethod]
        public void LastOrDefault_Range()
        {
            var value = 42;
            Assert.Equal(value, Observable.Range(value - 9, 10).LastOrDefault());
        }

        [TestMethod]
        public void LastOrDefaultPredicate_Range()
        {
            var value = 42;
            Assert.Equal(50, Observable.Range(value, 10).LastOrDefault(i => i % 2 == 0));
        }

    }
}

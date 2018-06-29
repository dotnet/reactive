// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using ReactiveTests.Dummies;
using Xunit;

namespace ReactiveTests.Tests
{
    public class LastTest : ReactiveTest
    {

        [Fact]
        public void Last_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Last(default(IObservable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Last(default(IObservable<int>), _ => true));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Last(DummyObservable<int>.Instance, default));
        }

        [Fact]
        public void Last_Empty()
        {
            ReactiveAssert.Throws<InvalidOperationException>(() => Observable.Empty<int>().Last());
        }

        [Fact]
        public void LastPredicate_Empty()
        {
            ReactiveAssert.Throws<InvalidOperationException>(() => Observable.Empty<int>().Last(_ => true));
        }

        [Fact]
        public void Last_Return()
        {
            var value = 42;
            Assert.Equal(value, Observable.Return(value).Last());
        }

        [Fact]
        public void Last_Throw()
        {
            var ex = new Exception();

            var xs = Observable.Throw<int>(ex);

            ReactiveAssert.Throws(ex, () => xs.Last());
        }

        [Fact]
        public void Last_Range()
        {
            var value = 42;
            Assert.Equal(value, Observable.Range(value - 9, 10).Last());
        }

        [Fact]
        public void LastPredicate_Range()
        {
            var value = 42;
            Assert.Equal(50, Observable.Range(value, 10).Last(i => i % 2 == 0));
        }

    }
}

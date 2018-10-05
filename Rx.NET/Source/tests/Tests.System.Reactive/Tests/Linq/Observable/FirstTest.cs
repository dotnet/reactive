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
    public class FirstTest : ReactiveTest
    {

        [Fact]
        public void First_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.First(default(IObservable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.First(default(IObservable<int>), _ => true));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.First(DummyObservable<int>.Instance, default));
        }

        [Fact]
        public void First_Empty()
        {
            ReactiveAssert.Throws<InvalidOperationException>(() => Observable.Empty<int>().First());
        }

        [Fact]
        public void FirstPredicate_Empty()
        {
            ReactiveAssert.Throws<InvalidOperationException>(() => Observable.Empty<int>().First(_ => true));
        }

        [Fact]
        public void First_Return()
        {
            var value = 42;
            Assert.Equal(value, Observable.Return(value).First());
        }

        [Fact]
        public void FirstPredicate_Return()
        {
            var value = 42;
            Assert.Equal(value, Observable.Return(value).First(i => i % 2 == 0));
        }

        [Fact]
        public void FirstPredicate_Return_NoMatch()
        {
            var value = 42;
            ReactiveAssert.Throws<InvalidOperationException>(() => Observable.Return(value).First(i => i % 2 != 0));
        }

        [Fact]
        public void First_Throw()
        {
            var ex = new Exception();

            var xs = Observable.Throw<int>(ex);

            ReactiveAssert.Throws(ex, () => xs.First());
        }

        [Fact]
        public void FirstPredicate_Throw()
        {
            var ex = new Exception();

            var xs = Observable.Throw<int>(ex);

            ReactiveAssert.Throws(ex, () => xs.First(_ => true));
        }

        [Fact]
        public void First_Range()
        {
            var value = 42;
            Assert.Equal(value, Observable.Range(value, 10).First());
        }

        [Fact]
        public void FirstPredicate_Range()
        {
            var value = 42;
            Assert.Equal(46, Observable.Range(value, 10).First(i => i > 45));
        }

    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using Xunit;
using ReactiveTests.Dummies;
using System.Reflection;

namespace ReactiveTests.Tests
{
    public class SingleTest : ReactiveTest
    {

        [Fact]
        public void Single_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Single(default(IObservable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Single(default(IObservable<int>), _ => true));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Single(DummyObservable<int>.Instance, default(Func<int, bool>)));
        }

        [Fact]
        public void Single_Empty()
        {
            ReactiveAssert.Throws<InvalidOperationException>(() => Observable.Empty<int>().Single());
        }

        [Fact]
        public void SinglePredicate_Empty()
        {
            ReactiveAssert.Throws<InvalidOperationException>(() => Observable.Empty<int>().Single(_ => true));
        }

        [Fact]
        public void Single_Return()
        {
            var value = 42;
            Assert.Equal(value, Observable.Return<int>(value).Single());
        }

        [Fact]
        public void Single_Throw()
        {
            var ex = new Exception();

            var xs = Observable.Throw<int>(ex);

            ReactiveAssert.Throws(ex, () => xs.Single());
        }

        [Fact]
        public void Single_Range()
        {
            var value = 42;
            ReactiveAssert.Throws<InvalidOperationException>(() => Observable.Range(value, 10).Single());
        }

        [Fact]
        public void SinglePredicate_Range()
        {
            var value = 42;
            ReactiveAssert.Throws<InvalidOperationException>(() => Observable.Range(value, 10).Single(i => i % 2 == 0));
        }

        [Fact]
        public void SinglePredicate_Range_ReducesToSingle()
        {
            var value = 42;
            Assert.Equal(45, Observable.Range(value, 10).Single(i => i == 45));
        }


    }
}

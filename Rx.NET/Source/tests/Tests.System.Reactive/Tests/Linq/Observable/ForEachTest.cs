// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using Xunit;

namespace ReactiveTests.Tests
{
    public class ForEachTest : ReactiveTest
    {

        [Fact]
        public void ForEach_ArgumentChecking()
        {
            var someObservable = Observable.Empty<int>();

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ForEach(default(IObservable<int>), x => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ForEach(someObservable, default(Action<int>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ForEach(default(IObservable<int>), (x, i) => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ForEach(someObservable, default(Action<int, int>)));
        }

        [Fact]
        public void ForEach_Empty()
        {
            var lst = new List<int>();
            Observable.Empty<int>().ForEach(x => lst.Add(x));
            Assert.True(lst.SequenceEqual(Enumerable.Empty<int>()));
        }

        [Fact]
        public void ForEach_Index_Empty()
        {
            var lstX = new List<int>();
            Observable.Empty<int>().ForEach((x, i) => lstX.Add(x));
            Assert.True(lstX.SequenceEqual(Enumerable.Empty<int>()));
        }

        [Fact]
        public void ForEach_Return()
        {
            var lst = new List<int>();
            Observable.Return(42).ForEach(x => lst.Add(x));
            Assert.True(lst.SequenceEqual(new[] { 42 }));
        }

        [Fact]
        public void ForEach_Index_Return()
        {
            var lstX = new List<int>();
            var lstI = new List<int>();
            Observable.Return(42).ForEach((x, i) => { lstX.Add(x); lstI.Add(i); });
            Assert.True(lstX.SequenceEqual(new[] { 42 }));
            Assert.True(lstI.SequenceEqual(new[] { 0 }));
        }

        [Fact]
        public void ForEach_Throw()
        {
            var ex = new Exception();

            var xs = Observable.Throw<int>(ex);

            ReactiveAssert.Throws(ex, () => xs.ForEach(x => { Assert.True(false); }));
        }

        [Fact]
        public void ForEach_Index_Throw()
        {
            var ex = new Exception();

            var xs = Observable.Throw<int>(ex);

            ReactiveAssert.Throws(ex, () => xs.ForEach((x, i) => { Assert.True(false); }));
        }

        [Fact]
        public void ForEach_SomeData()
        {
            var lstX = new List<int>();
            Observable.Range(10, 10).ForEach(x => lstX.Add(x));
            Assert.True(lstX.SequenceEqual(Enumerable.Range(10, 10)));
        }

        [Fact]
        public void ForEach_Index_SomeData()
        {
            var lstX = new List<int>();
            var lstI = new List<int>();
            Observable.Range(10, 10).ForEach((x, i) => { lstX.Add(x); lstI.Add(i); });
            Assert.True(lstX.SequenceEqual(Enumerable.Range(10, 10)));
            Assert.True(lstI.SequenceEqual(Enumerable.Range(0, 10)));
        }

        [Fact]
        public void ForEach_OnNextThrows()
        {
            var ex = new Exception();

            var xs = Observable.Range(0, 10);

            ReactiveAssert.Throws(ex, () => xs.ForEach(x => { throw ex; }));
        }

        [Fact]
        public void ForEach_Index_OnNextThrows()
        {
            var ex = new Exception();

            var xs = Observable.Range(0, 10);

            ReactiveAssert.Throws(ex, () => xs.ForEach((x, i) => { throw ex; }));
        }

    }
}

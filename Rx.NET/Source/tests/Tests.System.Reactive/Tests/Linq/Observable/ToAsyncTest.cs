// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using Xunit;

namespace ReactiveTests.Tests
{
    public class ToAsyncTest : ReactiveTest
    {

        [Fact]
        public void ToAsync_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync(default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync(default(Action<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int>(default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync(default(Action<int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int>(default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync(default(Action<int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int>(default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync(default(Action<int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int>(default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int>(default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync(default(Action<int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync(default(Action<int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int>(default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync(default(Action<int, int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int>(default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync(default(Action<int, int, int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int>(default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync(default(Action<int, int, int, int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int>(default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync(default(Action<int, int, int, int, int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int, int>(default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync(default(Action<int, int, int, int, int, int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int>(default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync(default(Action<int, int, int, int, int, int, int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int>(default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync(default(Action<int, int, int, int, int, int, int, int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int>(default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync(default(Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync(default(Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync(default(Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(default));

            var someScheduler = new TestScheduler();
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync(default, someScheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync(default(Action<int>), someScheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int>(default, someScheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync(default(Action<int, int>), someScheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int>(default, someScheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync(default(Action<int, int, int>), someScheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int>(default, someScheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync(default(Action<int, int, int, int>), someScheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int>(default, someScheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int>(default, someScheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync(default(Action<int, int, int, int, int>), someScheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync(default(Action<int, int, int, int, int, int>), someScheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int>(default, someScheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync(default(Action<int, int, int, int, int, int, int>), someScheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int>(default, someScheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync(default(Action<int, int, int, int, int, int, int, int>), someScheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int>(default, someScheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync(default(Action<int, int, int, int, int, int, int, int, int>), someScheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int>(default, someScheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync(default(Action<int, int, int, int, int, int, int, int, int, int>), someScheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int, int>(default, someScheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync(default(Action<int, int, int, int, int, int, int, int, int, int, int>), someScheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int>(default, someScheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync(default(Action<int, int, int, int, int, int, int, int, int, int, int, int>), someScheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int>(default, someScheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync(default(Action<int, int, int, int, int, int, int, int, int, int, int, int, int>), someScheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int>(default, someScheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync(default(Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int>), someScheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(default, someScheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync(default(Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>), someScheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(default, someScheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync(default(Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>), someScheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(default, someScheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(default, someScheduler));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync(() => { }, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int>(a => { }, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync(() => 1, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int>((a, b) => { }, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int>(a => 1, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int>((a, b, c) => { }, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int>((a, b) => 1, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int>((a, b, c, d) => { }, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int>((a, b, c) => 1, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int>((a, b, c, d) => 1, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int>((a, b, c, d, e) => { }, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int>((a, b, c, d, e, f) => { }, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int>((a, b, c, d, e) => 1, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int>((a, b, c, d, e, f, g) => { }, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int>((a, b, c, d, e, f) => 1, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h) => { }, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g) => 1, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i) => { }, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h) => 1, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j) => { }, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i) => 1, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k) => { }, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j) => 1, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l) => { }, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k) => 1, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m) => { }, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l) => 1, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n) => { }, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m) => 1, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o) => { }, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n) => 1, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p) => { }, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o) => 1, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p) => 1, null));
        }

        [Fact]
        public void ToAsync0()
        {
            Assert.True(Observable.ToAsync(() => 0)().ToEnumerable().SequenceEqual(new[] { 0 }));
            Assert.True(Observable.ToAsync(() => 0, Scheduler.Default)().ToEnumerable().SequenceEqual(new[] { 0 }));
        }

        [Fact]
        public void ToAsync1()
        {
            Assert.True(Observable.ToAsync<int, int>(a => a)(1).ToEnumerable().SequenceEqual(new[] { 1 }));
            Assert.True(Observable.ToAsync<int, int>(a => a, Scheduler.Default)(1).ToEnumerable().SequenceEqual(new[] { 1 }));
        }

        [Fact]
        public void ToAsync2()
        {
            Assert.True(Observable.ToAsync<int, int, int>((a, b) => a + b)(1, 2).ToEnumerable().SequenceEqual(new[] { 1 + 2 }));
            Assert.True(Observable.ToAsync<int, int, int>((a, b) => a + b, Scheduler.Default)(1, 2).ToEnumerable().SequenceEqual(new[] { 1 + 2 }));
        }

        [Fact]
        public void ToAsync3()
        {
            Assert.True(Observable.ToAsync<int, int, int, int>((a, b, c) => a + b + c)(1, 2, 3).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 }));
            Assert.True(Observable.ToAsync<int, int, int, int>((a, b, c) => a + b + c, Scheduler.Default)(1, 2, 3).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 }));
        }

        [Fact]
        public void ToAsync4()
        {
            Assert.True(Observable.ToAsync<int, int, int, int, int>((a, b, c, d) => a + b + c + d)(1, 2, 3, 4).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 + 4 }));
            Assert.True(Observable.ToAsync<int, int, int, int, int>((a, b, c, d) => a + b + c + d, Scheduler.Default)(1, 2, 3, 4).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 + 4 }));
        }

        [Fact]
        public void ToAsync5()
        {
            Assert.True(Observable.ToAsync<int, int, int, int, int, int>((a, b, c, d, e) => a + b + c + d + e)(1, 2, 3, 4, 5).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 + 4 + 5 }));
            Assert.True(Observable.ToAsync<int, int, int, int, int, int>((a, b, c, d, e) => a + b + c + d + e, Scheduler.Default)(1, 2, 3, 4, 5).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 + 4 + 5 }));
        }

        [Fact]
        public void ToAsync6()
        {
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int>((a, b, c, d, e, f) => a + b + c + d + e + f)(1, 2, 3, 4, 5, 6).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 + 4 + 5 + 6 }));
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int>((a, b, c, d, e, f) => a + b + c + d + e + f, Scheduler.Default)(1, 2, 3, 4, 5, 6).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 + 4 + 5 + 6 }));
        }

        [Fact]
        public void ToAsync7()
        {
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g) => a + b + c + d + e + f + g)(1, 2, 3, 4, 5, 6, 7).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 + 4 + 5 + 6 + 7 }));
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g) => a + b + c + d + e + f + g, Scheduler.Default)(1, 2, 3, 4, 5, 6, 7).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 + 4 + 5 + 6 + 7 }));
        }

        [Fact]
        public void ToAsync8()
        {
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h) => a + b + c + d + e + f + g + h)(1, 2, 3, 4, 5, 6, 7, 8).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 }));
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h) => a + b + c + d + e + f + g + h, Scheduler.Default)(1, 2, 3, 4, 5, 6, 7, 8).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 }));
        }

        [Fact]
        public void ToAsync9()
        {
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i) => a + b + c + d + e + f + g + h + i)(1, 2, 3, 4, 5, 6, 7, 8, 9).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 }));
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i) => a + b + c + d + e + f + g + h + i, Scheduler.Default)(1, 2, 3, 4, 5, 6, 7, 8, 9).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 }));
        }

        [Fact]
        public void ToAsync10()
        {
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j) => a + b + c + d + e + f + g + h + i + j)(1, 2, 3, 4, 5, 6, 7, 8, 9, 10).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 }));
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j) => a + b + c + d + e + f + g + h + i + j, Scheduler.Default)(1, 2, 3, 4, 5, 6, 7, 8, 9, 10).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 }));
        }

        [Fact]
        public void ToAsync11()
        {
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k) => a + b + c + d + e + f + g + h + i + j + k)(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11 }));
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k) => a + b + c + d + e + f + g + h + i + j + k, Scheduler.Default)(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11 }));
        }

        [Fact]
        public void ToAsync12()
        {
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l) => a + b + c + d + e + f + g + h + i + j + k + l)(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11 + 12 }));
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l) => a + b + c + d + e + f + g + h + i + j + k + l, Scheduler.Default)(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11 + 12 }));
        }

        [Fact]
        public void ToAsync13()
        {
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m) => a + b + c + d + e + f + g + h + i + j + k + l + m)(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11 + 12 + 13 }));
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m) => a + b + c + d + e + f + g + h + i + j + k + l + m, Scheduler.Default)(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11 + 12 + 13 }));
        }

        [Fact]
        public void ToAsync14()
        {
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n) => a + b + c + d + e + f + g + h + i + j + k + l + m + n)(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11 + 12 + 13 + 14 }));
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n) => a + b + c + d + e + f + g + h + i + j + k + l + m + n, Scheduler.Default)(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11 + 12 + 13 + 14 }));
        }

        [Fact]
        public void ToAsync15()
        {
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o) => a + b + c + d + e + f + g + h + i + j + k + l + m + n + o)(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11 + 12 + 13 + 14 + 15 }));
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o) => a + b + c + d + e + f + g + h + i + j + k + l + m + n + o, Scheduler.Default)(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11 + 12 + 13 + 14 + 15 }));
        }

        [Fact]
        public void ToAsync16()
        {
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p) => a + b + c + d + e + f + g + h + i + j + k + l + m + n + o + p)(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11 + 12 + 13 + 14 + 15 + 16 }));
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p) => a + b + c + d + e + f + g + h + i + j + k + l + m + n + o + p, Scheduler.Default)(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16).ToEnumerable().SequenceEqual(new[] { 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11 + 12 + 13 + 14 + 15 + 16 }));
        }

        [Fact]
        public void ToAsync_Error0()
        {
            var ex = new Exception();
            Assert.True(Observable.ToAsync<int>(() => { throw ex; })().Materialize().ToEnumerable().SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [Fact]
        public void ToAsync_Error1()
        {
            var ex = new Exception();
            Assert.True(Observable.ToAsync<int, int>(a => { throw ex; })(1).Materialize().ToEnumerable().SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [Fact]
        public void ToAsync_Error2()
        {
            var ex = new Exception();
            Assert.True(Observable.ToAsync<int, int, int>((a, b) => { throw ex; })(1, 2).Materialize().ToEnumerable().SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [Fact]
        public void ToAsync_Error3()
        {
            var ex = new Exception();
            Assert.True(Observable.ToAsync<int, int, int, int>((a, b, c) => { throw ex; })(1, 2, 3).Materialize().ToEnumerable().SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [Fact]
        public void ToAsync_Error4()
        {
            var ex = new Exception();
            Assert.True(Observable.ToAsync<int, int, int, int, int>((a, b, c, d) => { throw ex; })(1, 2, 3, 4).Materialize().ToEnumerable().SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [Fact]
        public void ToAsync_Error5()
        {
            var ex = new Exception();
            Assert.True(Observable.ToAsync<int, int, int, int, int, int>((a, b, c, d, e) => { throw ex; })(1, 2, 3, 4, 5).Materialize().ToEnumerable().SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [Fact]
        public void ToAsync_Error6()
        {
            var ex = new Exception();
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int>((a, b, c, d, e, f) => { throw ex; })(1, 2, 3, 4, 5, 6).Materialize().ToEnumerable().SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [Fact]
        public void ToAsync_Error7()
        {
            var ex = new Exception();
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g) => { throw ex; })(1, 2, 3, 4, 5, 6, 7).Materialize().ToEnumerable().SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [Fact]
        public void ToAsync_Error8()
        {
            var ex = new Exception();
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h) => { throw ex; })(1, 2, 3, 4, 5, 6, 7, 8).Materialize().ToEnumerable().SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [Fact]
        public void ToAsync_Error9()
        {
            var ex = new Exception();
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i) => { throw ex; })(1, 2, 3, 4, 5, 6, 7, 8, 9).Materialize().ToEnumerable().SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [Fact]
        public void ToAsync_Error10()
        {
            var ex = new Exception();
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j) => { throw ex; })(1, 2, 3, 4, 5, 6, 7, 8, 9, 10).Materialize().ToEnumerable().SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [Fact]
        public void ToAsync_Error11()
        {
            var ex = new Exception();
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k) => { throw ex; })(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11).Materialize().ToEnumerable().SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [Fact]
        public void ToAsync_Error12()
        {
            var ex = new Exception();
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l) => { throw ex; })(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12).Materialize().ToEnumerable().SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [Fact]
        public void ToAsync_Error13()
        {
            var ex = new Exception();
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m) => { throw ex; })(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13).Materialize().ToEnumerable().SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [Fact]
        public void ToAsync_Error14()
        {
            var ex = new Exception();
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n) => { throw ex; })(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14).Materialize().ToEnumerable().SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [Fact]
        public void ToAsync_Error15()
        {
            var ex = new Exception();
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o) => { throw ex; })(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15).Materialize().ToEnumerable().SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [Fact]
        public void ToAsync_Error16()
        {
            var ex = new Exception();
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p) => { throw ex; })(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16).Materialize().ToEnumerable().SequenceEqual(new Notification<int>[] { Notification.CreateOnError<int>(ex) }));
        }

        [Fact]
        public void ToAsyncAction0()
        {
            var hasRun = false;
            Assert.True(Observable.ToAsync(() => { hasRun = true; })().ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.True(Observable.ToAsync(() => { hasRun = true; }, Scheduler.Default)().ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.True(hasRun, "has run");
        }

        [Fact]
        public void ToAsyncActionError0()
        {
            var ex = new Exception();
            Assert.True(Observable.ToAsync(() => { throw ex; })().Materialize().ToEnumerable().SequenceEqual(new[] { Notification.CreateOnError<Unit>(ex) }));
        }

        [Fact]
        public void ToAsyncAction1()
        {
            var hasRun = false;
            Assert.True(Observable.ToAsync<int>(a => { Assert.Equal(1, a); hasRun = true; })(1).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.True(Observable.ToAsync<int>(a => { Assert.Equal(1, a); hasRun = true; }, Scheduler.Default)(1).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.True(hasRun, "has run");
        }

        [Fact]
        public void ToAsyncActionError1()
        {
            var ex = new Exception();
            Assert.True(Observable.ToAsync<int>(a => { Assert.Equal(1, a); throw ex; })(1).Materialize().ToEnumerable().SequenceEqual(new[] { Notification.CreateOnError<Unit>(ex) }));
        }

        [Fact]
        public void ToAsyncAction2()
        {
            var hasRun = false;
            Assert.True(Observable.ToAsync<int, int>((a, b) => { Assert.Equal(1, a); Assert.Equal(2, b); hasRun = true; })(1, 2).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.True(Observable.ToAsync<int, int>((a, b) => { Assert.Equal(1, a); Assert.Equal(2, b); hasRun = true; }, Scheduler.Default)(1, 2).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.True(hasRun, "has run");
        }

        [Fact]
        public void ToAsyncActionError2()
        {
            var ex = new Exception();
            Assert.True(Observable.ToAsync<int, int>((a, b) => { Assert.Equal(1, a); Assert.Equal(2, b); throw ex; })(1, 2).Materialize().ToEnumerable().SequenceEqual(new[] { Notification.CreateOnError<Unit>(ex) }));
        }

        [Fact]
        public void ToAsyncAction3()
        {
            var hasRun = false;
            Assert.True(Observable.ToAsync<int, int, int>((a, b, c) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); hasRun = true; })(1, 2, 3).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.True(Observable.ToAsync<int, int, int>((a, b, c) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); hasRun = true; }, Scheduler.Default)(1, 2, 3).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.True(hasRun, "has run");
        }

        [Fact]
        public void ToAsyncActionError3()
        {
            var ex = new Exception();
            Assert.True(Observable.ToAsync<int, int, int>((a, b, c) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); throw ex; })(1, 2, 3).Materialize().ToEnumerable().SequenceEqual(new[] { Notification.CreateOnError<Unit>(ex) }));
        }

        [Fact]
        public void ToAsyncAction4()
        {
            var hasRun = false;
            Assert.True(Observable.ToAsync<int, int, int, int>((a, b, c, d) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); Assert.Equal(4, d); hasRun = true; })(1, 2, 3, 4).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.True(Observable.ToAsync<int, int, int, int>((a, b, c, d) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); Assert.Equal(4, d); hasRun = true; }, Scheduler.Default)(1, 2, 3, 4).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.True(hasRun, "has run");
        }

        [Fact]
        public void ToAsyncActionError4()
        {
            var ex = new Exception();
            Assert.True(Observable.ToAsync<int, int, int, int>((a, b, c, d) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); Assert.Equal(4, d); throw ex; })(1, 2, 3, 4).Materialize().ToEnumerable().SequenceEqual(new[] { Notification.CreateOnError<Unit>(ex) }));
        }

        [Fact]
        public void ToAsyncAction5()
        {
            var hasRun = false;
            Assert.True(Observable.ToAsync<int, int, int, int, int>((a, b, c, d, e) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); Assert.Equal(4, d); Assert.Equal(5, e); hasRun = true; })(1, 2, 3, 4, 5).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.True(Observable.ToAsync<int, int, int, int, int>((a, b, c, d, e) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); Assert.Equal(4, d); Assert.Equal(5, e); hasRun = true; }, Scheduler.Default)(1, 2, 3, 4, 5).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.True(hasRun, "has run");
        }

        [Fact]
        public void ToAsyncActionError5()
        {
            var ex = new Exception();
            Assert.True(Observable.ToAsync<int, int, int, int, int>((a, b, c, d, e) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); Assert.Equal(4, d); Assert.Equal(5, e); throw ex; })(1, 2, 3, 4, 5).Materialize().ToEnumerable().SequenceEqual(new[] { Notification.CreateOnError<Unit>(ex) }));
        }

        [Fact]
        public void ToAsyncAction6()
        {
            var hasRun = false;
            Assert.True(Observable.ToAsync<int, int, int, int, int, int>((a, b, c, d, e, f) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); Assert.Equal(4, d); Assert.Equal(5, e); Assert.Equal(6, f); hasRun = true; })(1, 2, 3, 4, 5, 6).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.True(Observable.ToAsync<int, int, int, int, int, int>((a, b, c, d, e, f) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); Assert.Equal(4, d); Assert.Equal(5, e); Assert.Equal(6, f); hasRun = true; }, Scheduler.Default)(1, 2, 3, 4, 5, 6).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.True(hasRun, "has run");
        }

        [Fact]
        public void ToAsyncActionError6()
        {
            var ex = new Exception();
            Assert.True(Observable.ToAsync<int, int, int, int, int, int>((a, b, c, d, e, f) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); Assert.Equal(4, d); Assert.Equal(5, e); Assert.Equal(6, f); throw ex; })(1, 2, 3, 4, 5, 6).Materialize().ToEnumerable().SequenceEqual(new[] { Notification.CreateOnError<Unit>(ex) }));
        }

        [Fact]
        public void ToAsyncAction7()
        {
            var hasRun = false;
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int>((a, b, c, d, e, f, g) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); Assert.Equal(4, d); Assert.Equal(5, e); Assert.Equal(6, f); Assert.Equal(7, g); hasRun = true; })(1, 2, 3, 4, 5, 6, 7).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int>((a, b, c, d, e, f, g) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); Assert.Equal(4, d); Assert.Equal(5, e); Assert.Equal(6, f); Assert.Equal(7, g); hasRun = true; }, Scheduler.Default)(1, 2, 3, 4, 5, 6, 7).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.True(hasRun, "has run");
        }

        [Fact]
        public void ToAsyncActionError7()
        {
            var ex = new Exception();
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int>((a, b, c, d, e, f, g) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); Assert.Equal(4, d); Assert.Equal(5, e); Assert.Equal(6, f); Assert.Equal(7, g); throw ex; })(1, 2, 3, 4, 5, 6, 7).Materialize().ToEnumerable().SequenceEqual(new[] { Notification.CreateOnError<Unit>(ex) }));
        }

        [Fact]
        public void ToAsyncAction8()
        {
            var hasRun = false;
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); Assert.Equal(4, d); Assert.Equal(5, e); Assert.Equal(6, f); Assert.Equal(7, g); Assert.Equal(8, h); hasRun = true; })(1, 2, 3, 4, 5, 6, 7, 8).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); Assert.Equal(4, d); Assert.Equal(5, e); Assert.Equal(6, f); Assert.Equal(7, g); Assert.Equal(8, h); hasRun = true; }, Scheduler.Default)(1, 2, 3, 4, 5, 6, 7, 8).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.True(hasRun, "has run");
        }

        [Fact]
        public void ToAsyncActionError8()
        {
            var ex = new Exception();
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); Assert.Equal(4, d); Assert.Equal(5, e); Assert.Equal(6, f); Assert.Equal(7, g); Assert.Equal(8, h); throw ex; })(1, 2, 3, 4, 5, 6, 7, 8).Materialize().ToEnumerable().SequenceEqual(new[] { Notification.CreateOnError<Unit>(ex) }));
        }

        [Fact]
        public void ToAsyncAction9()
        {
            var hasRun = false;
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); Assert.Equal(4, d); Assert.Equal(5, e); Assert.Equal(6, f); Assert.Equal(7, g); Assert.Equal(8, h); Assert.Equal(9, i); hasRun = true; })(1, 2, 3, 4, 5, 6, 7, 8, 9).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); Assert.Equal(4, d); Assert.Equal(5, e); Assert.Equal(6, f); Assert.Equal(7, g); Assert.Equal(8, h); Assert.Equal(9, i); hasRun = true; }, Scheduler.Default)(1, 2, 3, 4, 5, 6, 7, 8, 9).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.True(hasRun, "has run");
        }

        [Fact]
        public void ToAsyncActionError9()
        {
            var ex = new Exception();
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); Assert.Equal(4, d); Assert.Equal(5, e); Assert.Equal(6, f); Assert.Equal(7, g); Assert.Equal(8, h); Assert.Equal(9, i); throw ex; })(1, 2, 3, 4, 5, 6, 7, 8, 9).Materialize().ToEnumerable().SequenceEqual(new[] { Notification.CreateOnError<Unit>(ex) }));
        }

        [Fact]
        public void ToAsyncAction10()
        {
            var hasRun = false;
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); Assert.Equal(4, d); Assert.Equal(5, e); Assert.Equal(6, f); Assert.Equal(7, g); Assert.Equal(8, h); Assert.Equal(9, i); Assert.Equal(10, j); hasRun = true; })(1, 2, 3, 4, 5, 6, 7, 8, 9, 10).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); Assert.Equal(4, d); Assert.Equal(5, e); Assert.Equal(6, f); Assert.Equal(7, g); Assert.Equal(8, h); Assert.Equal(9, i); Assert.Equal(10, j); hasRun = true; }, Scheduler.Default)(1, 2, 3, 4, 5, 6, 7, 8, 9, 10).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.True(hasRun, "has run");
        }

        [Fact]
        public void ToAsyncActionError10()
        {
            var ex = new Exception();
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); Assert.Equal(4, d); Assert.Equal(5, e); Assert.Equal(6, f); Assert.Equal(7, g); Assert.Equal(8, h); Assert.Equal(9, i); Assert.Equal(10, j); throw ex; })(1, 2, 3, 4, 5, 6, 7, 8, 9, 10).Materialize().ToEnumerable().SequenceEqual(new[] { Notification.CreateOnError<Unit>(ex) }));
        }

        [Fact]
        public void ToAsyncAction11()
        {
            var hasRun = false;
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); Assert.Equal(4, d); Assert.Equal(5, e); Assert.Equal(6, f); Assert.Equal(7, g); Assert.Equal(8, h); Assert.Equal(9, i); Assert.Equal(10, j); Assert.Equal(11, k); hasRun = true; })(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); Assert.Equal(4, d); Assert.Equal(5, e); Assert.Equal(6, f); Assert.Equal(7, g); Assert.Equal(8, h); Assert.Equal(9, i); Assert.Equal(10, j); Assert.Equal(11, k); hasRun = true; }, Scheduler.Default)(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.True(hasRun, "has run");
        }

        [Fact]
        public void ToAsyncActionError11()
        {
            var ex = new Exception();
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); Assert.Equal(4, d); Assert.Equal(5, e); Assert.Equal(6, f); Assert.Equal(7, g); Assert.Equal(8, h); Assert.Equal(9, i); Assert.Equal(10, j); Assert.Equal(11, k); throw ex; })(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11).Materialize().ToEnumerable().SequenceEqual(new[] { Notification.CreateOnError<Unit>(ex) }));
        }

        [Fact]
        public void ToAsyncAction12()
        {
            var hasRun = false;
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); Assert.Equal(4, d); Assert.Equal(5, e); Assert.Equal(6, f); Assert.Equal(7, g); Assert.Equal(8, h); Assert.Equal(9, i); Assert.Equal(10, j); Assert.Equal(11, k); Assert.Equal(12, l); hasRun = true; })(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); Assert.Equal(4, d); Assert.Equal(5, e); Assert.Equal(6, f); Assert.Equal(7, g); Assert.Equal(8, h); Assert.Equal(9, i); Assert.Equal(10, j); Assert.Equal(11, k); Assert.Equal(12, l); hasRun = true; }, Scheduler.Default)(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.True(hasRun, "has run");
        }

        [Fact]
        public void ToAsyncActionError12()
        {
            var ex = new Exception();
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); Assert.Equal(4, d); Assert.Equal(5, e); Assert.Equal(6, f); Assert.Equal(7, g); Assert.Equal(8, h); Assert.Equal(9, i); Assert.Equal(10, j); Assert.Equal(11, k); Assert.Equal(12, l); throw ex; })(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12).Materialize().ToEnumerable().SequenceEqual(new[] { Notification.CreateOnError<Unit>(ex) }));
        }

        [Fact]
        public void ToAsyncAction13()
        {
            var hasRun = false;
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); Assert.Equal(4, d); Assert.Equal(5, e); Assert.Equal(6, f); Assert.Equal(7, g); Assert.Equal(8, h); Assert.Equal(9, i); Assert.Equal(10, j); Assert.Equal(11, k); Assert.Equal(12, l); Assert.Equal(13, m); hasRun = true; })(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); Assert.Equal(4, d); Assert.Equal(5, e); Assert.Equal(6, f); Assert.Equal(7, g); Assert.Equal(8, h); Assert.Equal(9, i); Assert.Equal(10, j); Assert.Equal(11, k); Assert.Equal(12, l); Assert.Equal(13, m); hasRun = true; }, Scheduler.Default)(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.True(hasRun, "has run");
        }

        [Fact]
        public void ToAsyncActionError13()
        {
            var ex = new Exception();
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); Assert.Equal(4, d); Assert.Equal(5, e); Assert.Equal(6, f); Assert.Equal(7, g); Assert.Equal(8, h); Assert.Equal(9, i); Assert.Equal(10, j); Assert.Equal(11, k); Assert.Equal(12, l); Assert.Equal(13, m); throw ex; })(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13).Materialize().ToEnumerable().SequenceEqual(new[] { Notification.CreateOnError<Unit>(ex) }));
        }

        [Fact]
        public void ToAsyncAction14()
        {
            var hasRun = false;
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); Assert.Equal(4, d); Assert.Equal(5, e); Assert.Equal(6, f); Assert.Equal(7, g); Assert.Equal(8, h); Assert.Equal(9, i); Assert.Equal(10, j); Assert.Equal(11, k); Assert.Equal(12, l); Assert.Equal(13, m); Assert.Equal(14, n); hasRun = true; })(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); Assert.Equal(4, d); Assert.Equal(5, e); Assert.Equal(6, f); Assert.Equal(7, g); Assert.Equal(8, h); Assert.Equal(9, i); Assert.Equal(10, j); Assert.Equal(11, k); Assert.Equal(12, l); Assert.Equal(13, m); Assert.Equal(14, n); hasRun = true; }, Scheduler.Default)(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.True(hasRun, "has run");
        }

        [Fact]
        public void ToAsyncActionError14()
        {
            var ex = new Exception();
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); Assert.Equal(4, d); Assert.Equal(5, e); Assert.Equal(6, f); Assert.Equal(7, g); Assert.Equal(8, h); Assert.Equal(9, i); Assert.Equal(10, j); Assert.Equal(11, k); Assert.Equal(12, l); Assert.Equal(13, m); Assert.Equal(14, n); throw ex; })(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14).Materialize().ToEnumerable().SequenceEqual(new[] { Notification.CreateOnError<Unit>(ex) }));
        }

        [Fact]
        public void ToAsyncAction15()
        {
            var hasRun = false;
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); Assert.Equal(4, d); Assert.Equal(5, e); Assert.Equal(6, f); Assert.Equal(7, g); Assert.Equal(8, h); Assert.Equal(9, i); Assert.Equal(10, j); Assert.Equal(11, k); Assert.Equal(12, l); Assert.Equal(13, m); Assert.Equal(14, n); Assert.Equal(15, o); hasRun = true; })(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); Assert.Equal(4, d); Assert.Equal(5, e); Assert.Equal(6, f); Assert.Equal(7, g); Assert.Equal(8, h); Assert.Equal(9, i); Assert.Equal(10, j); Assert.Equal(11, k); Assert.Equal(12, l); Assert.Equal(13, m); Assert.Equal(14, n); Assert.Equal(15, o); hasRun = true; }, Scheduler.Default)(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.True(hasRun, "has run");
        }

        [Fact]
        public void ToAsyncActionError15()
        {
            var ex = new Exception();
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); Assert.Equal(4, d); Assert.Equal(5, e); Assert.Equal(6, f); Assert.Equal(7, g); Assert.Equal(8, h); Assert.Equal(9, i); Assert.Equal(10, j); Assert.Equal(11, k); Assert.Equal(12, l); Assert.Equal(13, m); Assert.Equal(14, n); Assert.Equal(15, o); throw ex; })(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15).Materialize().ToEnumerable().SequenceEqual(new[] { Notification.CreateOnError<Unit>(ex) }));
        }

        [Fact]
        public void ToAsyncAction16()
        {
            var hasRun = false;
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); Assert.Equal(4, d); Assert.Equal(5, e); Assert.Equal(6, f); Assert.Equal(7, g); Assert.Equal(8, h); Assert.Equal(9, i); Assert.Equal(10, j); Assert.Equal(11, k); Assert.Equal(12, l); Assert.Equal(13, m); Assert.Equal(14, n); Assert.Equal(15, o); Assert.Equal(16, p); hasRun = true; })(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); Assert.Equal(4, d); Assert.Equal(5, e); Assert.Equal(6, f); Assert.Equal(7, g); Assert.Equal(8, h); Assert.Equal(9, i); Assert.Equal(10, j); Assert.Equal(11, k); Assert.Equal(12, l); Assert.Equal(13, m); Assert.Equal(14, n); Assert.Equal(15, o); Assert.Equal(16, p); hasRun = true; }, Scheduler.Default)(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16).ToEnumerable().SequenceEqual(new[] { new Unit() }));
            Assert.True(hasRun, "has run");
        }

        [Fact]
        public void ToAsyncActionError16()
        {
            var ex = new Exception();
            Assert.True(Observable.ToAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p) => { Assert.Equal(1, a); Assert.Equal(2, b); Assert.Equal(3, c); Assert.Equal(4, d); Assert.Equal(5, e); Assert.Equal(6, f); Assert.Equal(7, g); Assert.Equal(8, h); Assert.Equal(9, i); Assert.Equal(10, j); Assert.Equal(11, k); Assert.Equal(12, l); Assert.Equal(13, m); Assert.Equal(14, n); Assert.Equal(15, o); Assert.Equal(16, p); throw ex; })(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16).Materialize().ToEnumerable().SequenceEqual(new[] { Notification.CreateOnError<Unit>(ex) }));
        }

    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class SelectMany : AsyncEnumerableTests
    {
        [Fact]
        public void SelectMany_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.SelectMany(default, default(Func<int, IAsyncEnumerable<int>>)));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.SelectMany(default, default(Func<int, int, IAsyncEnumerable<int>>)));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.SelectMany(Return42, default(Func<int, IAsyncEnumerable<int>>)));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.SelectMany(Return42, default(Func<int, int, IAsyncEnumerable<int>>)));

            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.SelectMany(default, default(Func<int, IAsyncEnumerable<int>>), (x, y) => x));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.SelectMany(default, default(Func<int, int, IAsyncEnumerable<int>>), (x, y) => x));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.SelectMany(Return42, default(Func<int, IAsyncEnumerable<int>>), (x, y) => x));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.SelectMany(Return42, default(Func<int, int, IAsyncEnumerable<int>>), (x, y) => x));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.SelectMany(Return42, x => default, default(Func<int, int, int>)));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.SelectMany(Return42, (x, i) => default, default(Func<int, int, int>)));
        }

        [Fact]
        public async Task SelectMany1Async()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = xs.SelectMany(x => Enumerable.Range(0, x).ToAsyncEnumerable());

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 0);
            await HasNextAsync(e, 0);
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 0);
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task SelectMany2Async()
        {
            var ex = new Exception("Bang");
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = xs.SelectMany(x =>
            {
                if (x < 3)
                    return Enumerable.Range(0, x).ToAsyncEnumerable();
                else
                    return Throw<int>(ex);
            });

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 0);
            await HasNextAsync(e, 0);
            await HasNextAsync(e, 1);
            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        [Fact]
        public async Task SelectMany3Async()
        {
            var ex = new Exception("Bang");
            var xs = Throw<int>(ex);
            var ys = xs.SelectMany(x => Enumerable.Range(0, x).ToAsyncEnumerable());

            var e = ys.GetAsyncEnumerator();
            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        [Fact]
        public async Task SelectMany4Async()
        {
            var ex = new Exception("Bang");
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = xs.SelectMany(x =>
            {
                if (x < 3)
                    return Enumerable.Range(0, x).ToAsyncEnumerable();
                else
                    throw ex;
            });

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 0);
            await HasNextAsync(e, 0);
            await HasNextAsync(e, 1);
            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        [Fact]
        public async Task SelectMany5Async()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = xs.SelectMany((x, i) => Enumerable.Range(i + 5, x).ToAsyncEnumerable());

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 5);
            await HasNextAsync(e, 6);
            await HasNextAsync(e, 7);
            await HasNextAsync(e, 7);
            await HasNextAsync(e, 8);
            await HasNextAsync(e, 9);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task SelectMany6Async()
        {
            var ex = new Exception("Bang");
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = xs.SelectMany((x, i) =>
            {
                if (i < 2)
                    return Enumerable.Range(0, x).ToAsyncEnumerable();
                else
                    return Throw<int>(ex);
            });

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 0);
            await HasNextAsync(e, 0);
            await HasNextAsync(e, 1);
            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        [Fact]
        public async Task SelectMany7Async()
        {
            var ex = new Exception("Bang");
            var xs = Throw<int>(ex);
            var ys = xs.SelectMany((x, i) => Enumerable.Range(0, x).ToAsyncEnumerable());

            var e = ys.GetAsyncEnumerator();
            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        [Fact]
        public async Task SelectMany8Async()
        {
            var ex = new Exception("Bang");
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = xs.SelectMany((x, i) =>
            {
                if (i < 2)
                    return Enumerable.Range(0, x).ToAsyncEnumerable();
                else
                    throw ex;
            });

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 0);
            await HasNextAsync(e, 0);
            await HasNextAsync(e, 1);
            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        [Fact]
        public async Task SelectMany9Async()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = xs.SelectMany(x => Enumerable.Range(3, x).ToAsyncEnumerable(), (x, y) => x * y);

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 6);
            await HasNextAsync(e, 8);
            await HasNextAsync(e, 9);
            await HasNextAsync(e, 12);
            await HasNextAsync(e, 15);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task SelectMany10Async()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = xs.SelectMany((x, i) => Enumerable.Range(i + 3, x).ToAsyncEnumerable(), (x, y) => x * y);

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 8);
            await HasNextAsync(e, 10);
            await HasNextAsync(e, 15);
            await HasNextAsync(e, 18);
            await HasNextAsync(e, 21);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task SelectMany11Async()
        {
            var ex = new Exception("Bang");
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = xs.SelectMany(x => Enumerable.Range(3, x).ToAsyncEnumerable(), (x, y) =>
            {
                if (x * y > 10)
                    throw ex;
                return x * y;
            });

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 6);
            await HasNextAsync(e, 8);
            await HasNextAsync(e, 9);
            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        [Fact]
        public async Task SelectMany12Async()
        {
            var ex = new Exception("Bang");
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = xs.SelectMany((x, i) => Enumerable.Range(i + 3, x).ToAsyncEnumerable(), (x, y) =>
            {
                if (x * y > 10)
                    throw ex;
                return x * y;
            });

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 8);
            await HasNextAsync(e, 10);
            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        [Fact]
        public async Task SelectMany13()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = xs.SelectMany(x => Enumerable.Range(0, x).ToAsyncEnumerable());

            await SequenceIdentity(ys);
        }

        [Fact]
        public async Task SelectMany14()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = xs.SelectMany((x, i) => Enumerable.Range(i + 5, x).ToAsyncEnumerable());

            await SequenceIdentity(ys);
        }

        [Fact]
        public async Task SelectMany15()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = xs.SelectMany(x => Enumerable.Range(3, x).ToAsyncEnumerable(), (x, y) => x * y);

            await SequenceIdentity(ys);
        }

        [Fact]
        public async Task SelectMany16()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = xs.SelectMany((x, i) => Enumerable.Range(i + 3, x).ToAsyncEnumerable(), (x, y) => x * y);

            await SequenceIdentity(ys);
        }
    }
}

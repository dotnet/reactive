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
    public class GroupJoin : AsyncEnumerableTests
    {
        [Fact]
        public void GroupJoin_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupJoin<int, int, int, int>(default, Return42, x => x, x => x, (x, y) => x));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupJoin<int, int, int, int>(Return42, default, x => x, x => x, (x, y) => x));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupJoin(Return42, Return42, default, x => x, (x, y) => x));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupJoin(Return42, Return42, x => x, default, (x, y) => x));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupJoin<int, int, int, int>(Return42, Return42, x => x, x => x, default));

            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupJoin<int, int, int, int>(default, Return42, x => x, x => x, (x, y) => x, EqualityComparer<int>.Default));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupJoin<int, int, int, int>(Return42, default, x => x, x => x, (x, y) => x, EqualityComparer<int>.Default));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupJoin(Return42, Return42, default, x => x, (x, y) => x, EqualityComparer<int>.Default));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupJoin(Return42, Return42, x => x, default, (x, y) => x, EqualityComparer<int>.Default));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.GroupJoin<int, int, int, int>(Return42, Return42, x => x, x => x, default, EqualityComparer<int>.Default));
        }

        [Fact]
        public async Task GroupJoin1()
        {
            var xs = new[] { 0, 1, 2 }.ToAsyncEnumerable();
            var ys = new[] { 4, 7, 6, 2, 3, 4, 8, 9 }.ToAsyncEnumerable();

            var res = xs.GroupJoin(ys, x => x % 3, y => y % 3, (x, i) => x + " - " + i.AggregateAsync("", (s, j) => s + j).Result);

            var e = res.GetAsyncEnumerator();
            await HasNextAsync(e, "0 - 639");
            await HasNextAsync(e, "1 - 474");
            await HasNextAsync(e, "2 - 28");
            await NoNextAsync(e);
        }

        [Fact]
        public async Task GroupJoin2()
        {
            var xs = new[] { 0, 1, 2 }.ToAsyncEnumerable();
            var ys = new[] { 3, 6, 4 }.ToAsyncEnumerable();

            var res = xs.GroupJoin(ys, x => x % 3, y => y % 3, (x, i) => x + " - " + i.AggregateAsync("", (s, j) => s + j).Result);

            var e = res.GetAsyncEnumerator();
            await HasNextAsync(e, "0 - 36");
            await HasNextAsync(e, "1 - 4");
            await HasNextAsync(e, "2 - ");
            await NoNextAsync(e);
        }

        [Fact]
        public async Task GroupJoin3Async()
        {
            var ex = new Exception("Bang!");
            var xs = Throw<int>(ex);
            var ys = new[] { 3, 6, 4 }.ToAsyncEnumerable();

            var res = xs.GroupJoin(ys, x => x % 3, y => y % 3, (x, i) => x + " - " + i.AggregateAsync("", (s, j) => s + j).Result);

            var e = res.GetAsyncEnumerator();
            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        [Fact]
        public async Task GroupJoin4Async()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 0, 1, 2 }.ToAsyncEnumerable();
            var ys = Throw<int>(ex);

            var res = xs.GroupJoin(ys, x => x % 3, y => y % 3, (x, i) => x + " - " + i.AggregateAsync("", (s, j) => s + j).Result);

            var e = res.GetAsyncEnumerator();
            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        [Fact]
        public async Task GroupJoin5Async()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 0, 1, 2 }.ToAsyncEnumerable();
            var ys = new[] { 3, 6, 4 }.ToAsyncEnumerable();

            var res = xs.GroupJoin(ys, x => { throw ex; }, y => y % 3, (x, i) => x + " - " + i.AggregateAsync("", (s, j) => s + j).Result);

            var e = res.GetAsyncEnumerator();
            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        [Fact]
        public async Task GroupJoin6Async()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 0, 1, 2 }.ToAsyncEnumerable();
            var ys = new[] { 3, 6, 4 }.ToAsyncEnumerable();

            var res = xs.GroupJoin(ys, x => x % 3, y => { throw ex; }, (x, i) => x + " - " + i.AggregateAsync("", (s, j) => s + j).Result);

            var e = res.GetAsyncEnumerator();
            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        [Fact]
        public async Task GroupJoin7()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 0, 1, 2 }.ToAsyncEnumerable();
            var ys = new[] { 3, 6, 4 }.ToAsyncEnumerable();

            var res = xs.GroupJoin(ys, x => x % 3, y => y % 3, (x, i) =>
            {
                if (x == 1)
                    throw ex;
                return x + " - " + i.AggregateAsync("", (s, j) => s + j).Result;
            });

            var e = res.GetAsyncEnumerator();
            await HasNextAsync(e, "0 - 36");
            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }
    }
}

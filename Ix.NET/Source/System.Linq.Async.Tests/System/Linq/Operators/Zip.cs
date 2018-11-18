// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class Zip : AsyncEnumerableTests
    {
        [Fact]
        public void Zip_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.Zip<int, int, int>(default, Return42, (x, y) => x + y));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.Zip<int, int, int>(Return42, default, (x, y) => x + y));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.Zip(Return42, Return42, default(Func<int, int, int>)));
        }

        [Fact]
        public async Task Zip1Async()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = new[] { 4, 5, 6 }.ToAsyncEnumerable();
            var res = xs.Zip(ys, (x, y) => x * y);

            var e = res.GetAsyncEnumerator();
            await HasNextAsync(e, 1 * 4);
            await HasNextAsync(e, 2 * 5);
            await HasNextAsync(e, 3 * 6);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Zip2Async()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = new[] { 4, 5, 6, 7 }.ToAsyncEnumerable();
            var res = xs.Zip(ys, (x, y) => x * y);

            var e = res.GetAsyncEnumerator();
            await HasNextAsync(e, 1 * 4);
            await HasNextAsync(e, 2 * 5);
            await HasNextAsync(e, 3 * 6);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Zip3Async()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = new[] { 4, 5, 6 }.ToAsyncEnumerable();
            var res = xs.Zip(ys, (x, y) => x * y);

            var e = res.GetAsyncEnumerator();
            await HasNextAsync(e, 1 * 4);
            await HasNextAsync(e, 2 * 5);
            await HasNextAsync(e, 3 * 6);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Zip4Async()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = Throw<int>(ex);
            var res = xs.Zip(ys, (x, y) => x * y);

            var e = res.GetAsyncEnumerator();
            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        [Fact]
        public async Task Zip5Async()
        {
            var ex = new Exception("Bang!");
            var xs = Throw<int>(ex);
            var ys = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var res = xs.Zip(ys, (x, y) => x * y);

            var e = res.GetAsyncEnumerator();
            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        [Fact]
        public async Task Zip6Async()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var res = xs.Zip(ys, (x, y) => { if (x > 0) throw ex; return x * y; });

            var e = res.GetAsyncEnumerator();
            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        [Fact]
        public async Task Zip7()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = new[] { 4, 5, 6 }.ToAsyncEnumerable();
            var res = xs.Zip(ys, (x, y) => x * y);

            await SequenceIdentity(res);
        }

        [Fact]
        public async Task Zip_Concurrency()
        {
            var xs = new[] { 1, 2, 3 }.ToSharedStateAsyncEnumerable();
            var res = xs.Zip(xs, (x, y) => x * y);

            async Task f() => await res.LastAsync();

            await f(); // Should not throw
        }
    }
}

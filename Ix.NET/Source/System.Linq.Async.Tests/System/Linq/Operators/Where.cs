// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class Where : AsyncEnumerableTests
    {
        [Fact]
        public void Where_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.Where<int>(default, x => true));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.Where<int>(default, (x, i) => true));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.Where(Return42, default(Func<int, bool>)));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.Where(Return42, default(Func<int, int, bool>)));
        }

        [Fact]
        public async Task Where1()
        {
            var xs = new[] { 8, 5, 7, 4, 6, 9, 2, 1, 0 }.ToAsyncEnumerable();
            var ys = xs.Where(x => x % 2 == 0);
            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 8);
            await HasNextAsync(e, 4);
            await HasNextAsync(e, 6);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 0);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Where2()
        {
            var xs = new[] { 8, 5, 7, 4, 6, 9, 2, 1, 0 }.ToAsyncEnumerable();
            var ys = xs.Where((x, i) => i % 2 == 0);

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 8);
            await HasNextAsync(e, 7);
            await HasNextAsync(e, 6);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 0);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Where3()
        {
            var xs = new[] { 8, 5, 7, 4, 6, 9, 2, 1, 0 }.ToAsyncEnumerable();
            var ex = new Exception("Bang");
            var ys = xs.Where(x => { if (x == 4) throw ex; return true; });

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 8);
            await HasNextAsync(e, 5);
            await HasNextAsync(e, 7);
            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        [Fact]
        public async Task Where4()
        {
            var xs = new[] { 8, 5, 7, 4, 6, 9, 2, 1, 0 }.ToAsyncEnumerable();
            var ex = new Exception("Bang");
            var ys = xs.Where((x, i) => { if (i == 3) throw ex; return true; });

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 8);
            await HasNextAsync(e, 5);
            await HasNextAsync(e, 7);
            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        [Fact]
        public async Task Where5Async()
        {
            var ex = new Exception("Bang");
            var xs = Throw<int>(ex);
            var ys = xs.Where(x => true);

            var e = ys.GetAsyncEnumerator();
            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        [Fact]
        public async Task Where6Async()
        {
            var ex = new Exception("Bang");
            var xs = Throw<int>(ex);

            var ys = xs.Where((x, i) => true);
            var e = ys.GetAsyncEnumerator();
            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        [Fact]
        public async Task Where7()
        {
            var xs = new[] { 8, 5, 7, 4, 6, 9, 2, 1, 0 }.ToAsyncEnumerable();
            var ys = xs.Where(x => x % 2 == 0).Where(x => x > 5);
            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 8);
            await HasNextAsync(e, 6);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Where8()
        {
            var xs = new[] { 8, 5, 7, 4, 6, 9, 2, 1, 0 }.ToAsyncEnumerable();
            var ys = xs.Where(x => x % 2 == 0);

            await SequenceIdentity(ys);
        }

        [Fact]
        public async Task Where9()
        {
            var xs = new[] { 8, 5, 7, 4, 6, 9, 2, 1, 0 }.ToAsyncEnumerable();
            var ys = xs.Where((x, i) => i % 2 == 0);

            await SequenceIdentity(ys);
        }
    }
}

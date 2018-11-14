// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class Reverse : AsyncEnumerableTests
    {
        [Fact]
        public void Reverse_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.Reverse<int>(default));
        }

        [Fact]
        public async Task Reverse1Async()
        {
            var xs = AsyncEnumerable.Empty<int>();
            var ys = xs.Reverse();

            var e = ys.GetAsyncEnumerator();
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Reverse2Async()
        {
            var xs = Return42;
            var ys = xs.Reverse();

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 42);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Reverse3Async()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = xs.Reverse();

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 1);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Reverse4Async()
        {
            var ex = new Exception("Bang!");
            var xs = Throw<int>(ex);
            var ys = xs.Reverse();

            var e = ys.GetAsyncEnumerator();
            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        [Fact]
        public async Task Reverse5()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = xs.Reverse();

            Assert.Equal(new[] { 3, 2, 1 }, await ys.ToArrayAsync());
        }

        [Fact]
        public async Task Reverse6()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = xs.Reverse();

            Assert.Equal(new[] { 3, 2, 1 }, await ys.ToListAsync());
        }

        [Fact]
        public async Task Reverse7()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = xs.Reverse();

            Assert.Equal(3, await ys.CountAsync());
        }

        [Fact]
        public async Task Reverse8()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = xs.Reverse();

            await SequenceIdentity(ys);
        }

        [Fact]
        public async Task Reverse9()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = xs.Reverse().Prepend(4); // to trigger onlyIfCheap

            Assert.Equal(new[] { 4, 3, 2, 1 }, await ys.ToArrayAsync());
        }
    }
}

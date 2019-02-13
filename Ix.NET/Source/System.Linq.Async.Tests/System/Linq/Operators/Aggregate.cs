// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class Aggregate : AsyncEnumerableTests
    {
        [Fact]
        public async Task Aggregate_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AggregateAsync<int>(default, (x, y) => x + y).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AggregateAsync(Return42, default(Func<int, int, int>)).AsTask());

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AggregateAsync<int, int>(default, 0, (x, y) => x + y).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AggregateAsync(Return42, 0, default(Func<int, int, int>)).AsTask());

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AggregateAsync<int, int, int>(default, 0, (x, y) => x + y, z => z).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AggregateAsync(Return42, 0, default, z => z).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AggregateAsync<int, int, int>(Return42, 0, (x, y) => x + y, default).AsTask());

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AggregateAsync<int>(default, (x, y) => x + y, CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AggregateAsync(Return42, default(Func<int, int, int>), CancellationToken.None).AsTask());

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AggregateAsync<int, int>(default, 0, (x, y) => x + y, CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AggregateAsync(Return42, 0, default(Func<int, int, int>), CancellationToken.None).AsTask());

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AggregateAsync<int, int, int>(default, 0, (x, y) => x + y, z => z, CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AggregateAsync(Return42, 0, default, z => z, CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AggregateAsync<int, int, int>(Return42, 0, (x, y) => x + y, default, CancellationToken.None).AsTask());
        }

        [Fact]
        public async Task Aggregate1Async()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.AggregateAsync((x, y) => x * y);
            Assert.Equal(24, await ys);
        }

        [Fact]
        public async Task Aggregate2Async()
        {
            var xs = new int[0].ToAsyncEnumerable();
            var ys = xs.AggregateAsync((x, y) => x * y);
            await AssertThrowsAsync<InvalidOperationException>(ys.AsTask());
        }

        [Fact]
        public async Task Aggregate3Async()
        {
            var ex = new Exception("Bang!");
            var xs = Throw<int>(ex);
            var ys = xs.AggregateAsync((x, y) => x * y);
            await AssertThrowsAsync(ys, ex);
        }

        [Fact]
        public async Task Aggregate4Async()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.AggregateAsync(new Func<int, int, int>((x, y) => { throw ex; }));
            await AssertThrowsAsync(ys, ex);
        }

        [Fact]
        public async Task Aggregate5Async()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.AggregateAsync(1, (x, y) => x * y);
            Assert.Equal(24, await ys);
        }

        [Fact]
        public async Task Aggregate6Async()
        {
            var xs = new int[0].ToAsyncEnumerable();
            var ys = xs.AggregateAsync(1, (x, y) => x * y);
            Assert.Equal(1, await ys);
        }

        [Fact]
        public async Task Aggregate7Async()
        {
            var ex = new Exception("Bang!");
            var xs = Throw<int>(ex);
            var ys = xs.AggregateAsync(1, (x, y) => x * y);
            await AssertThrowsAsync(ys, ex);
        }

        [Fact]
        public async Task Aggregate8Async()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.AggregateAsync(1, new Func<int, int, int>((x, y) => { throw ex; }));
            await AssertThrowsAsync(ys, ex);
        }

        [Fact]
        public async Task Aggregate9Async()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.AggregateAsync(1, (x, y) => x * y, x => x + 1);
            Assert.Equal(25, await ys);
        }

        [Fact]
        public async Task Aggregate10Async()
        {
            var xs = new int[0].ToAsyncEnumerable();
            var ys = xs.AggregateAsync(1, (x, y) => x * y, x => x + 1);
            Assert.Equal(2, await ys);
        }

        [Fact]
        public async Task Aggregate11Async()
        {
            var ex = new Exception("Bang!");
            var xs = Throw<int>(ex);
            var ys = xs.AggregateAsync(1, (x, y) => x * y, x => x + 1);
            await AssertThrowsAsync(ys, ex);
        }

        [Fact]
        public async Task Aggregate12Async()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.AggregateAsync(1, (x, y) => { throw ex; }, x => x + 1);
            await AssertThrowsAsync(ys, ex);
        }

        [Fact]
        public async Task Aggregate13Async()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.AggregateAsync<int, int, int>(1, (x, y) => x * y, x => { throw ex; });
            await AssertThrowsAsync(ys, ex);
        }
    }
}

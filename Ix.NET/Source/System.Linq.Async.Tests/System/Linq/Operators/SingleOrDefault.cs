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
    public class SingleOrDefault : AsyncEnumerableTests
    {
        [Fact]
        public async Task SingleOrDefault_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SingleOrDefaultAsync<int>(default));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SingleOrDefaultAsync<int>(default, x => true));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SingleOrDefaultAsync(Return42, default(Func<int, bool>)));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SingleOrDefaultAsync<int>(default, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SingleOrDefaultAsync<int>(default, x => true, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SingleOrDefaultAsync(Return42, default(Func<int, bool>), CancellationToken.None));
        }

        [Fact]
        public async Task SingleOrDefault1Async()
        {
            var res = AsyncEnumerable.Empty<int>().SingleOrDefaultAsync();
            Assert.Equal(0, await res);
        }

        [Fact]
        public async Task SingleOrDefault2Async()
        {
            var res = AsyncEnumerable.Empty<int>().SingleOrDefaultAsync(x => true);
            Assert.Equal(0, await res);
        }

        [Fact]
        public async Task SingleOrDefault3Async()
        {
            var res = Return42.SingleOrDefaultAsync(x => x % 2 != 0);
            Assert.Equal(0, await res);
        }

        [Fact]
        public async Task SingleOrDefault4Async()
        {
            var res = Return42.SingleOrDefaultAsync();
            Assert.Equal(42, await res);
        }

        [Fact]
        public async Task SingleOrDefault5Async()
        {
            var res = Return42.SingleOrDefaultAsync(x => x % 2 == 0);
            Assert.Equal(42, await res);
        }

        [Fact]
        public async Task SingleOrDefault6Async()
        {
            var ex = new Exception("Bang!");
            var res = Throw<int>(ex).SingleOrDefaultAsync();
            await AssertThrowsAsync(res, ex);
        }

        [Fact]
        public async Task SingleOrDefault7Async()
        {
            var ex = new Exception("Bang!");
            var res = Throw<int>(ex).SingleOrDefaultAsync(x => true);
            await AssertThrowsAsync(res, ex);
        }

        [Fact]
        public async Task SingleOrDefault8Async()
        {
            var res = new[] { 42, 45, 90 }.ToAsyncEnumerable().SingleOrDefaultAsync();
            await AssertThrowsAsync<InvalidOperationException>(res);
        }

        [Fact]
        public async Task SingleOrDefault9Async()
        {
            var res = new[] { 42, 45, 90 }.ToAsyncEnumerable().SingleOrDefaultAsync(x => x % 2 != 0);
            Assert.Equal(45, await res);
        }

        [Fact]
        public async Task SingleOrDefault10Async()
        {
            var res = new[] { 42, 45, 90 }.ToAsyncEnumerable().SingleOrDefaultAsync(x => x < 10);
            Assert.Equal(0, await res);
        }

        [Fact]
        public async Task SingleOrDefault11Async()
        {
            var res = new[] { 42, 45, 90 }.ToAsyncEnumerable().SingleOrDefaultAsync(x => true);
            await AssertThrowsAsync<InvalidOperationException>(res);
        }

        [Fact]
        public async Task SingleOrDefault12Async()
        {
            var res = new int[0].ToAsyncEnumerable().SingleOrDefaultAsync();
            Assert.Equal(0, await res);
        }
    }
}

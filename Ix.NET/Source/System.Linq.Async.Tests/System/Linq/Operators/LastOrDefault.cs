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
    public class LastOrDefault : AsyncEnumerableTests
    {
        [Fact]
        public async Task LastOrDefault_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.LastOrDefaultAsync<int>(default));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.LastOrDefaultAsync<int>(default, x => true));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.LastOrDefaultAsync(Return42, default(Func<int, bool>)));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.LastOrDefaultAsync<int>(default, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.LastOrDefaultAsync<int>(default, x => true, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.LastOrDefaultAsync(Return42, default(Func<int, bool>), CancellationToken.None));
        }

        [Fact]
        public async Task LastOrDefault1Async()
        {
            var res = AsyncEnumerable.Empty<int>().LastOrDefaultAsync();
            Assert.Equal(0, await res);
        }

        [Fact]
        public async Task LastOrDefault2Async()
        {
            var res = AsyncEnumerable.Empty<int>().LastOrDefaultAsync(x => true);
            Assert.Equal(0, await res);
        }

        [Fact]
        public async Task LastOrDefault3Async()
        {
            var res = Return42.LastOrDefaultAsync(x => x % 2 != 0);
            Assert.Equal(0, await res);
        }

        [Fact]
        public async Task LastOrDefault4Async()
        {
            var res = Return42.LastOrDefaultAsync();
            Assert.Equal(42, await res);
        }

        [Fact]
        public async Task LastOrDefault5Async()
        {
            var res = Return42.LastOrDefaultAsync(x => x % 2 == 0);
            Assert.Equal(42, await res);
        }

        [Fact]
        public async Task LastOrDefault6Async()
        {
            var ex = new Exception("Bang!");
            var res = Throw<int>(ex).LastOrDefaultAsync();
            await AssertThrowsAsync(res, ex);
        }

        [Fact]
        public async Task LastOrDefault7Async()
        {
            var ex = new Exception("Bang!");
            var res = Throw<int>(ex).LastOrDefaultAsync(x => true);
            await AssertThrowsAsync(res, ex);
        }

        [Fact]
        public async Task LastOrDefault8Async()
        {
            var res = new[] { 42, 45, 90 }.ToAsyncEnumerable().LastOrDefaultAsync();
            Assert.Equal(90, await res);
        }

        [Fact]
        public async Task LastOrDefault9Async()
        {
            var res = new[] { 42, 23, 45, 90 }.ToAsyncEnumerable().LastOrDefaultAsync(x => x % 2 != 0);
            Assert.Equal(45, await res);
        }

        [Fact]
        public async Task LastOrDefault10Async()
        {
            var res = new[] { 42, 45, 90 }.ToAsyncEnumerable().LastOrDefaultAsync(x => x < 10);
            Assert.Equal(0, await res);
        }
    }
}

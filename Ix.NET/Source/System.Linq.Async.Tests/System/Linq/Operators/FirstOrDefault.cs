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
    public class FirstOrDefault : AsyncEnumerableTests
    {
        [Fact]
        public async Task FirstOrDefault_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.FirstOrDefaultAsync<int>(default));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.FirstOrDefaultAsync<int>(default, x => true));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.FirstOrDefaultAsync(Return42, default(Func<int, bool>)));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.FirstOrDefaultAsync<int>(default, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.FirstOrDefaultAsync<int>(default, x => true, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.FirstOrDefaultAsync(Return42, default(Func<int, bool>), CancellationToken.None));
        }

        [Fact]
        public async Task FirstOrDefault1Async()
        {
            var res = AsyncEnumerable.Empty<int>().FirstOrDefaultAsync();
            Assert.Equal(0, await res);
        }

        [Fact]
        public async Task FirstOrDefault2Async()
        {
            var res = AsyncEnumerable.Empty<int>().FirstOrDefaultAsync(x => true);
            Assert.Equal(0, await res);
        }

        [Fact]
        public async Task FirstOrDefault3Async()
        {
            var res = Return42.FirstOrDefaultAsync(x => x % 2 != 0);
            Assert.Equal(0, await res);
        }

        [Fact]
        public async Task FirstOrDefault4Async()
        {
            var res = Return42.FirstOrDefaultAsync();
            Assert.Equal(42, await res);
        }

        [Fact]
        public async Task FirstOrDefault5Async()
        {
            var res = Return42.FirstOrDefaultAsync(x => x % 2 == 0);
            Assert.Equal(42, await res);
        }

        [Fact]
        public async Task FirstOrDefault6Async()
        {
            var ex = new Exception("Bang!");
            var res = Throw<int>(ex).FirstOrDefaultAsync();
            await AssertThrowsAsync(res, ex);
        }

        [Fact]
        public async Task FirstOrDefault7Async()
        {
            var ex = new Exception("Bang!");
            var res = Throw<int>(ex).FirstOrDefaultAsync(x => true);
            await AssertThrowsAsync(res, ex);
        }

        [Fact]
        public async Task FirstOrDefault8Async()
        {
            var res = new[] { 42, 45, 90 }.ToAsyncEnumerable().FirstOrDefaultAsync();
            Assert.Equal(42, await res);
        }

        [Fact]
        public async Task FirstOrDefault9Async()
        {
            var res = new[] { 42, 45, 90 }.ToAsyncEnumerable().FirstOrDefaultAsync(x => x % 2 != 0);
            Assert.Equal(45, await res);
        }

        [Fact]
        public async Task FirstOrDefault10Async()
        {
            var res = new[] { 42, 45, 90 }.ToAsyncEnumerable().FirstOrDefaultAsync(x => x < 10);
            Assert.Equal(0, await res);
        }
    }
}

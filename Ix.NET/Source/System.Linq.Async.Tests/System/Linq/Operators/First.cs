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
    public class First : AsyncEnumerableTests
    {
        [Fact]
        public async Task First_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.FirstAsync<int>(default));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.FirstAsync<int>(default, x => true));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.FirstAsync(Return42, default(Func<int, bool>)));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.FirstAsync<int>(default, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.FirstAsync<int>(default, x => true, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.FirstAsync(Return42, default(Func<int, bool>), CancellationToken.None));
        }

        [Fact]
        public async Task First1Async()
        {
            var res = AsyncEnumerable.Empty<int>().FirstAsync();
            await AssertThrowsAsync<InvalidOperationException>(res);
        }

        [Fact]
        public async Task First2Async()
        {
            var res = AsyncEnumerable.Empty<int>().FirstAsync(x => true);
            await AssertThrowsAsync<InvalidOperationException>(res);
        }

        [Fact]
        public async Task First3Async()
        {
            var res = Return42.FirstAsync(x => x % 2 != 0);
            await AssertThrowsAsync<InvalidOperationException>(res);
        }

        [Fact]
        public async Task First4Async()
        {
            var res = Return42.FirstAsync();
            Assert.Equal(42, await res);
        }

        [Fact]
        public async Task First5Async()
        {
            var res = Return42.FirstAsync(x => x % 2 == 0);
            Assert.Equal(42, await res);
        }

        [Fact]
        public async Task First6Async()
        {
            var ex = new Exception("Bang!");
            var res = Throw<int>(ex).FirstAsync();
            await AssertThrowsAsync(res, ex);
        }

        [Fact]
        public async Task First7Async()
        {
            var ex = new Exception("Bang!");
            var res = Throw<int>(ex).FirstAsync(x => true);
            await AssertThrowsAsync(res, ex);
        }

        [Fact]
        public async Task First8Async()
        {
            var res = new[] { 42, 45, 90 }.ToAsyncEnumerable().FirstAsync();
            Assert.Equal(42, await res);
        }

        [Fact]
        public async Task First9Async()
        {
            var res = new[] { 42, 45, 90 }.ToAsyncEnumerable().FirstAsync(x => x % 2 != 0);
            Assert.Equal(45, await res);
        }
    }
}

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
    public class Count : AsyncEnumerableTests
    {
        [Fact]
        public async Task Count_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.CountAsync<int>(default).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.CountAsync<int>(default, x => true).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.CountAsync(Return42, default(Func<int, bool>)).AsTask());

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.CountAsync<int>(default, CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.CountAsync<int>(default, x => true, CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.CountAsync(Return42, default(Func<int, bool>), CancellationToken.None).AsTask());
        }

        [Fact]
        public async Task Count1()
        {
            Assert.Equal(0, await new int[0].ToAsyncEnumerable().CountAsync());
            Assert.Equal(3, await new[] { 1, 2, 3 }.ToAsyncEnumerable().CountAsync());
        }

        [Fact]
        public async Task Count2()
        {
            Assert.Equal(0, await new int[0].ToAsyncEnumerable().CountAsync(x => x < 3));
            Assert.Equal(2, await new[] { 1, 2, 3 }.ToAsyncEnumerable().CountAsync(x => x < 3));
        }

        [Fact]
        public async Task Count3Async()
        {
            var ex = new Exception("Bang!");
            var ys = new[] { 1, 2, 3 }.ToAsyncEnumerable().CountAsync(new Func<int, bool>(x => { throw ex; }));
            await AssertThrowsAsync(ys, ex);
        }

        [Fact]
        public async Task Count4Async()
        {
            var ex = new Exception("Bang!");
            await AssertThrowsAsync(Throw<int>(ex).CountAsync(), ex);
        }

        [Fact]
        public async Task Count5Async()
        {
            var ex = new Exception("Bang!");
            await AssertThrowsAsync(Throw<int>(ex).CountAsync(x => x < 3), ex);
        }
    }
}

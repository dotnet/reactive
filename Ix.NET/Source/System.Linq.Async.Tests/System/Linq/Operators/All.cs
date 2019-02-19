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
    public class All : AsyncEnumerableTests
    {
        [Fact]
        public async Task AllAsync_Sync_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AllAsync<int>(default, x => true).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AllAsync(Return42, default(Func<int, bool>)).AsTask());

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AllAsync<int>(default, x => true, CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AllAsync(Return42, default(Func<int, bool>), CancellationToken.None).AsTask());
        }

        [Fact]
        public async Task AllAsync_Sync_Simple_False()
        {
            var res = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable().AllAsync(x => x % 2 == 0);
            Assert.False(await res);
        }

        [Fact]
        public async Task AllAsync_Sync_Simple_True()
        {
            var res = new[] { 2, 8, 4 }.ToAsyncEnumerable().AllAsync(x => x % 2 == 0);
            Assert.True(await res);
        }

        [Fact]
        public async Task AllAsync_Sync_Throw_Source()
        {
            var ex = new Exception("Bang!");
            var res = Throw<int>(ex).AllAsync(x => x % 2 == 0);
            await AssertThrowsAsync(res, ex);
        }

        [Fact]
        public async Task AllAsync_Sync_Throw_Selector()
        {
            var ex = new Exception("Bang!");
            var res = new[] { 2, 8, 4 }.ToAsyncEnumerable().AllAsync(new Func<int, bool>(x => { throw ex; }));
            await AssertThrowsAsync(res, ex);
        }

        [Fact]
        public async Task AllAsync_Async_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AllAsync<int>(default, x => new ValueTask<bool>(true)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AllAsync(Return42, default(Func<int, ValueTask<bool>>)).AsTask());

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AllAsync<int>(default, x => new ValueTask<bool>(true), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AllAsync(Return42, default(Func<int, ValueTask<bool>>), CancellationToken.None).AsTask());
        }

        [Fact]
        public async Task AllAsync_Async_Simple_False()
        {
            var res = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable().AllAsync(x => new ValueTask<bool>(x % 2 == 0));
            Assert.False(await res);
        }

        [Fact]
        public async Task AllAsync_Async_Simple_True()
        {
            var res = new[] { 2, 8, 4 }.ToAsyncEnumerable().AllAsync(x => new ValueTask<bool>(x % 2 == 0));
            Assert.True(await res);
        }

        [Fact]
        public async Task AllAsync_Async_Throw_Source()
        {
            var ex = new Exception("Bang!");
            var res = Throw<int>(ex).AllAsync(x => new ValueTask<bool>(x % 2 == 0));
            await AssertThrowsAsync(res, ex);
        }

        [Fact]
        public async Task AllAsync_Async_Throw_Selector()
        {
            var ex = new Exception("Bang!");
            var res = new[] { 2, 8, 4 }.ToAsyncEnumerable().AllAsync(new Func<int, ValueTask<bool>>(x => { throw ex; }));
            await AssertThrowsAsync(res, ex);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task AllAsync_AsyncCancel_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AllAsync<int>(default, (x, ct) => new ValueTask<bool>(true)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AllAsync(Return42, default(Func<int, CancellationToken, ValueTask<bool>>)).AsTask());

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AllAsync<int>(default, (x, ct) => new ValueTask<bool>(true), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AllAsync(Return42, default(Func<int, CancellationToken, ValueTask<bool>>), CancellationToken.None).AsTask());
        }

        [Fact]
        public async Task AllAsync_AsyncCancel_Simple_False()
        {
            var res = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable().AllAsync((x, ct) => new ValueTask<bool>(x % 2 == 0));
            Assert.False(await res);
        }

        [Fact]
        public async Task AllAsync_AsyncCancel_Simple_True()
        {
            var res = new[] { 2, 8, 4 }.ToAsyncEnumerable().AllAsync((x, ct) => new ValueTask<bool>(x % 2 == 0));
            Assert.True(await res);
        }

        [Fact]
        public async Task AllAsync_AsyncCancel_Throw_Source()
        {
            var ex = new Exception("Bang!");
            var res = Throw<int>(ex).AllAsync((x, ct) => new ValueTask<bool>(x % 2 == 0));
            await AssertThrowsAsync(res, ex);
        }

        [Fact]
        public async Task AllAsync_AsyncCancel_Throw_Selector()
        {
            var ex = new Exception("Bang!");
            var res = new[] { 2, 8, 4 }.ToAsyncEnumerable().AllAsync(new Func<int, CancellationToken, ValueTask<bool>>((x, ct) => { throw ex; }));
            await AssertThrowsAsync(res, ex);
        }
#endif
    }
}

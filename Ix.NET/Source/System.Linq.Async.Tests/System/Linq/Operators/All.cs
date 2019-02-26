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
        public async Task AllAsync_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AllAsync<int>(default, x => true).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AllAsync(Return42, default).AsTask());

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AllAsync<int>(default, x => true, CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AllAsync(Return42, default, CancellationToken.None).AsTask());
        }

        [Fact]
        public async Task AllAsync_Simple_False()
        {
            var res = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable().AllAsync(x => x % 2 == 0);
            Assert.False(await res);
        }

        [Fact]
        public async Task AllAsync_Simple_True()
        {
            var res = new[] { 2, 8, 4 }.ToAsyncEnumerable().AllAsync(x => x % 2 == 0);
            Assert.True(await res);
        }

        [Fact]
        public async Task AllAsync_Throw_Source()
        {
            var ex = new Exception("Bang!");
            var res = Throw<int>(ex).AllAsync(x => x % 2 == 0);
            await AssertThrowsAsync(res, ex);
        }

        [Fact]
        public async Task AllAsync_Throw_Selector()
        {
            var ex = new Exception("Bang!");
            var res = new[] { 2, 8, 4 }.ToAsyncEnumerable().AllAsync(new Func<int, bool>(x => { throw ex; }));
            await AssertThrowsAsync(res, ex);
        }

        [Fact]
        public async Task AllAwaitAsync_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AllAwaitAsync<int>(default, x => new ValueTask<bool>(true)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AllAwaitAsync(Return42, default).AsTask());

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AllAwaitAsync<int>(default, x => new ValueTask<bool>(true), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AllAwaitAsync(Return42, default, CancellationToken.None).AsTask());
        }

        [Fact]
        public async Task AllAwaitAsync_Simple_False()
        {
            var res = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable().AllAwaitAsync(x => new ValueTask<bool>(x % 2 == 0));
            Assert.False(await res);
        }

        [Fact]
        public async Task AllAwaitAsync_Simple_True()
        {
            var res = new[] { 2, 8, 4 }.ToAsyncEnumerable().AllAwaitAsync(x => new ValueTask<bool>(x % 2 == 0));
            Assert.True(await res);
        }

        [Fact]
        public async Task AllAwaitAsync_Throw_Source()
        {
            var ex = new Exception("Bang!");
            var res = Throw<int>(ex).AllAwaitAsync(x => new ValueTask<bool>(x % 2 == 0));
            await AssertThrowsAsync(res, ex);
        }

        [Fact]
        public async Task AllAwaitAsync_Throw_Selector()
        {
            var ex = new Exception("Bang!");
            var res = new[] { 2, 8, 4 }.ToAsyncEnumerable().AllAwaitAsync(new Func<int, ValueTask<bool>>(x => { throw ex; }));
            await AssertThrowsAsync(res, ex);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task AllAwaitWithCancellationAsync_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AllAwaitWithCancellationAsync<int>(default, (x, ct) => new ValueTask<bool>(true)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AllAwaitWithCancellationAsync(Return42, default).AsTask());

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AllAwaitWithCancellationAsync<int>(default, (x, ct) => new ValueTask<bool>(true), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AllAwaitWithCancellationAsync(Return42, default, CancellationToken.None).AsTask());
        }

        [Fact]
        public async Task AllAwaitWithCancellationAsync_Simple_False()
        {
            var res = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable().AllAwaitWithCancellationAsync((x, ct) => new ValueTask<bool>(x % 2 == 0));
            Assert.False(await res);
        }

        [Fact]
        public async Task AllAwaitWithCancellationAsync_Simple_True()
        {
            var res = new[] { 2, 8, 4 }.ToAsyncEnumerable().AllAwaitWithCancellationAsync((x, ct) => new ValueTask<bool>(x % 2 == 0));
            Assert.True(await res);
        }

        [Fact]
        public async Task AllAwaitWithCancellationAsync_Throw_Source()
        {
            var ex = new Exception("Bang!");
            var res = Throw<int>(ex).AllAwaitWithCancellationAsync((x, ct) => new ValueTask<bool>(x % 2 == 0));
            await AssertThrowsAsync(res, ex);
        }

        [Fact]
        public async Task AllAwaitWithCancellationAsync_Throw_Selector()
        {
            var ex = new Exception("Bang!");
            var res = new[] { 2, 8, 4 }.ToAsyncEnumerable().AllAwaitWithCancellationAsync(new Func<int, CancellationToken, ValueTask<bool>>((x, ct) => { throw ex; }));
            await AssertThrowsAsync(res, ex);
        }
#endif
    }
}

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
    public class Single : AsyncEnumerableTests
    {
        [Fact]
        public async Task Single_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SingleAsync<int>(default).AsTask());

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SingleAsync<int>(default, CancellationToken.None).AsTask());

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SingleAsync<int>(default, x => true).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SingleAsync(Return42, default(Func<int, bool>)).AsTask());

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SingleAsync<int>(default, x => true, CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SingleAsync(Return42, default(Func<int, bool>), CancellationToken.None).AsTask());

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SingleAsync<int>(default, x => new ValueTask<bool>(true)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SingleAsync(Return42, default(Func<int, ValueTask<bool>>)).AsTask());

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SingleAsync<int>(default, x => new ValueTask<bool>(true), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SingleAsync(Return42, default(Func<int, ValueTask<bool>>), CancellationToken.None).AsTask());

#if !NO_DEEP_CANCELLATION
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SingleAsync<int>(default, (x, ct) => new ValueTask<bool>(true), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SingleAsync(Return42, default(Func<int, CancellationToken, ValueTask<bool>>), CancellationToken.None).AsTask());
#endif
        }

        [Fact]
        public async Task Single1Async()
        {
            var res = AsyncEnumerable.Empty<int>().SingleAsync();
            await AssertThrowsAsync<InvalidOperationException>(res.AsTask());
        }

        [Fact]
        public async Task Single2Async()
        {
            var res = AsyncEnumerable.Empty<int>().SingleAsync(x => true);
            await AssertThrowsAsync<InvalidOperationException>(res.AsTask());
        }

        [Fact]
        public async Task Single3Async()
        {
            var res = Return42.SingleAsync(x => x % 2 != 0);
            await AssertThrowsAsync<InvalidOperationException>(res.AsTask());
        }

        [Fact]
        public async Task Single4Async()
        {
            var res = Return42.SingleAsync();
            Assert.Equal(42, await res);
        }

        [Fact]
        public async Task Single5Async()
        {
            var res = Return42.SingleAsync(x => x % 2 == 0);
            Assert.Equal(42, await res);
        }

        [Fact]
        public async Task Single6Async()
        {
            var ex = new Exception("Bang!");
            var res = Throw<int>(ex).SingleAsync();
            await AssertThrowsAsync(res, ex);
        }

        [Fact]
        public async Task Single7Async()
        {
            var ex = new Exception("Bang!");
            var res = Throw<int>(ex).SingleAsync(x => true);
            await AssertThrowsAsync(res, ex);
        }

        [Fact]
        public async Task Single8Async()
        {
            var res = new[] { 42, 45, 90 }.ToAsyncEnumerable().SingleAsync();
            await AssertThrowsAsync<InvalidOperationException>(res.AsTask());
        }

        [Fact]
        public async Task Single9Async()
        {
            var res = new[] { 42, 45, 90 }.ToAsyncEnumerable().SingleAsync(x => x % 2 != 0);
            Assert.Equal(45, await res);
        }

        [Fact]
        public async Task Single10Async()
        {
            var res = new[] { 42, 23, 45, 90 }.ToAsyncEnumerable().SingleAsync(x => x % 2 != 0);
            await AssertThrowsAsync<InvalidOperationException>(res.AsTask());
        }

        [Fact]
        public async Task Single11Async()
        {
            var res = new int[0].ToAsyncEnumerable().SingleAsync();
            await AssertThrowsAsync<InvalidOperationException>(res.AsTask());
        }
    }
}

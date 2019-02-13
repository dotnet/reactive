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
    public class Any : AsyncEnumerableTests
    {
        [Fact]
        public async Task Any_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AnyAsync<int>(default).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AnyAsync<int>(default, CancellationToken.None).AsTask());

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AnyAsync<int>(default, x => true).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AnyAsync(Return42, default(Func<int, bool>)).AsTask());

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AnyAsync<int>(default, x => true, CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AnyAsync(Return42, default(Func<int, bool>), CancellationToken.None).AsTask());

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AnyAsync<int>(default, x => new ValueTask<bool>(true)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AnyAsync(Return42, default(Func<int, ValueTask<bool>>)).AsTask());

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AnyAsync<int>(default, x => new ValueTask<bool>(true), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AnyAsync(Return42, default(Func<int, ValueTask<bool>>), CancellationToken.None).AsTask());
        }

        [Fact]
        public async Task Any1Async()
        {
            var res = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable().AnyAsync(x => x % 2 == 0);
            Assert.True(await res);
        }

        [Fact]
        public async Task Any2Async()
        {
            var res = new[] { 2, 8, 4 }.ToAsyncEnumerable().AnyAsync(x => x % 2 != 0);
            Assert.False(await res);
        }

        [Fact]
        public async Task Any3Async()
        {
            var ex = new Exception("Bang!");
            var res = Throw<int>(ex).AnyAsync(x => x % 2 == 0);
            await AssertThrowsAsync(res, ex);
        }

        [Fact]
        public async Task Any4Async()
        {
            var ex = new Exception("Bang!");
            var res = new[] { 2, 8, 4 }.ToAsyncEnumerable().AnyAsync(new Func<int, bool>(x => { throw ex; }));
            await AssertThrowsAsync(res, ex);
        }

        [Fact]
        public async Task Any5Async()
        {
            var res = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable().AnyAsync();
            Assert.True(await res);
        }

        [Fact]
        public async Task Any6Async()
        {
            var res = new int[0].ToAsyncEnumerable().AnyAsync();
            Assert.False(await res);
        }

        [Fact]
        public async Task Any7Async()
        {
            var res = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable().AnyAsync(x => new ValueTask<bool>(x % 2 == 0));
            Assert.True(await res);
        }

        [Fact]
        public async Task Any8Async()
        {
            var res = new[] { 2, 8, 4 }.ToAsyncEnumerable().AnyAsync(x => new ValueTask<bool>(x % 2 != 0));
            Assert.False(await res);
        }

        [Fact]
        public async Task Any9Async()
        {
            var ex = new Exception("Bang!");
            var res = Throw<int>(ex).AnyAsync(x => new ValueTask<bool>(x % 2 == 0));
            await AssertThrowsAsync(res, ex);
        }

        [Fact]
        public async Task Any10Async()
        {
            var ex = new Exception("Bang!");
            var res = new[] { 2, 8, 4 }.ToAsyncEnumerable().AnyAsync(new Func<int, ValueTask<bool>>(x => { throw ex; }));
            await AssertThrowsAsync(res, ex);
        }
    }
}

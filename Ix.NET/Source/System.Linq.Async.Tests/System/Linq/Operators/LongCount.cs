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
    public class LongCount : AsyncEnumerableTests
    {
        [Fact]
        public async Task LongCountAsync_Simple_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.LongCountAsync<int>(default).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.LongCountAsync<int>(default, CancellationToken.None).AsTask());
        }

        [Fact]
        public async Task LongCountAsync_Simple()
        {
            Assert.Equal(0, await new int[0].ToAsyncEnumerable().LongCountAsync());
            Assert.Equal(3, await new[] { 1, 2, 3 }.ToAsyncEnumerable().LongCountAsync());
        }

        [Fact]
        public async Task LongCountAsync_Simple_Throws_Source()
        {
            var ex = new Exception("Bang!");
            await AssertThrowsAsync(Throw<int>(ex).LongCountAsync(), ex);
        }

        [Fact]
        public async Task LongCountAsync_Predicate_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.LongCountAsync<int>(default, x => true).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.LongCountAsync(Return42, default(Func<int, bool>)).AsTask());

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.LongCountAsync<int>(default, x => true, CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.LongCountAsync(Return42, default, CancellationToken.None).AsTask());
        }

        [Fact]
        public async Task LongCountAsync_Predicate_Simple()
        {
            Assert.Equal(0, await new int[0].ToAsyncEnumerable().LongCountAsync(x => x < 3));
            Assert.Equal(2, await new[] { 1, 2, 3 }.ToAsyncEnumerable().LongCountAsync(x => x < 3));
        }

        [Fact]
        public async Task LongCountAsync_Predicate_Throws_Predicate()
        {
            var ex = new Exception("Bang!");
            var ys = new[] { 1, 2, 3 }.ToAsyncEnumerable().LongCountAsync(new Func<int, bool>(x => { throw ex; }));
            await AssertThrowsAsync(ys, ex);
        }

        [Fact]
        public async Task LongCountAsync_Predicate_Throws_Source()
        {
            var ex = new Exception("Bang!");
            await AssertThrowsAsync(Throw<int>(ex).LongCountAsync(x => x < 3), ex);
        }

        [Fact]
        public async Task LongCountAwaitAsync_Predicate_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.LongCountAwaitAsync<int>(default, x => new ValueTask<bool>(true)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.LongCountAwaitAsync(Return42, default).AsTask());

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.LongCountAwaitAsync<int>(default, x => new ValueTask<bool>(true), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.LongCountAwaitAsync(Return42, default, CancellationToken.None).AsTask());
        }

        [Fact]
        public async Task LongCountAwaitAsync_Predicate()
        {
            Assert.Equal(0, await new int[0].ToAsyncEnumerable().LongCountAwaitAsync(x => new ValueTask<bool>(x < 3)));
            Assert.Equal(2, await new[] { 1, 2, 3 }.ToAsyncEnumerable().LongCountAwaitAsync(x => new ValueTask<bool>(x < 3)));
        }

        [Fact]
        public async Task LongCountAwaitAsync_Predicate_Throws_Predicate()
        {
            var ex = new Exception("Bang!");
            var ys = new[] { 1, 2, 3 }.ToAsyncEnumerable().LongCountAwaitAsync(new Func<int, ValueTask<bool>>(x => { throw ex; }));
            await AssertThrowsAsync(ys, ex);
        }

        [Fact]
        public async Task LongCountAwaitAsync_Predicate_Throws_Source()
        {
            var ex = new Exception("Bang!");
            await AssertThrowsAsync(Throw<int>(ex).LongCountAwaitAsync(x => new ValueTask<bool>(x < 3)), ex);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task LongCountAwaitWithCancellationAsync_Predicate_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.LongCountAwaitWithCancellationAsync<int>(default, (x, ct) => new ValueTask<bool>(true)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.LongCountAwaitWithCancellationAsync(Return42, default).AsTask());

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.LongCountAwaitWithCancellationAsync<int>(default, (x, ct) => new ValueTask<bool>(true), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.LongCountAwaitWithCancellationAsync(Return42, default, CancellationToken.None).AsTask());
        }

        [Fact]
        public async Task LongCountAwaitWithCancellationAsync_Predicate()
        {
            Assert.Equal(0, await new int[0].ToAsyncEnumerable().LongCountAwaitWithCancellationAsync((x, ct) => new ValueTask<bool>(x < 3)));
            Assert.Equal(2, await new[] { 1, 2, 3 }.ToAsyncEnumerable().LongCountAwaitWithCancellationAsync((x, ct) => new ValueTask<bool>(x < 3)));
        }

        [Fact]
        public async Task LongCountAwaitWithCancellationAsync_Predicate_Throws_Predicate()
        {
            var ex = new Exception("Bang!");
            var ys = new[] { 1, 2, 3 }.ToAsyncEnumerable().LongCountAwaitWithCancellationAsync(new Func<int, CancellationToken, ValueTask<bool>>((x, ct) => { throw ex; }));
            await AssertThrowsAsync(ys, ex);
        }

        [Fact]
        public async Task LongCountAwaitWithCancellationAsync_Predicate_Throws_Source()
        {
            var ex = new Exception("Bang!");
            await AssertThrowsAsync(Throw<int>(ex).LongCountAwaitWithCancellationAsync((x, ct) => new ValueTask<bool>(x < 3)), ex);
        }
#endif
    }
}

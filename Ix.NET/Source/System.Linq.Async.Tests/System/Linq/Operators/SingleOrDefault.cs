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
        public async Task SingleOrDefaultAsync_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SingleOrDefaultAsync<int>(default).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SingleOrDefaultAsync<int>(default, CancellationToken.None).AsTask());
        }

        [Fact]
        public async Task SingleOrDefaultAsync_Empty()
        {
            var res = AsyncEnumerable.Empty<int>().SingleOrDefaultAsync();
            Assert.Equal(0, await res);
        }

        [Fact]
        public async Task SingleOrDefaultAsync_Empty_IList()
        {
            var res = new int[0].ToAsyncEnumerable().SingleOrDefaultAsync();
            Assert.Equal(0, await res);
        }

        [Fact]
        public async Task SingleOrDefaultAsync_Single_IList()
        {
            var res = Return42.SingleOrDefaultAsync();
            Assert.Equal(42, await res);
        }

        [Fact]
        public async Task SingleOrDefaultAsync_Single()
        {
            var res = new[] { 42 }.ToAsyncEnumerable().Where(x => x > 0).SingleOrDefaultAsync();
            Assert.Equal(42, await res);
        }

        [Fact]
        public async Task SingleOrDefaultAsync_Throws_Source()
        {
            var ex = new Exception("Bang!");
            var res = Throw<int>(ex).SingleOrDefaultAsync();
            await AssertThrowsAsync(res, ex);
        }

        [Fact]
        public async Task SingleOrDefaultAsync_MoreThanOne_IList()
        {
            var res = new[] { 42, 45, 90 }.ToAsyncEnumerable().SingleOrDefaultAsync();
            await AssertThrowsAsync<InvalidOperationException>(res.AsTask());
        }

        [Fact]
        public async Task SingleOrDefaultAsync_MoreThanOne()
        {
            var res = new[] { 42, 45, 90 }.ToAsyncEnumerable().Select(x => x).SingleOrDefaultAsync();
            await AssertThrowsAsync<InvalidOperationException>(res.AsTask());
        }

        [Fact]
        public async Task SingleOrDefaultAsync_Predicate_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SingleOrDefaultAsync<int>(default, x => true).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SingleOrDefaultAsync(Return42, default(Func<int, bool>)).AsTask());

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SingleOrDefaultAsync<int>(default, x => true, CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SingleOrDefaultAsync(Return42, default, CancellationToken.None).AsTask());
        }

        [Fact]
        public async Task SingleOrDefaultAsync_Predicate_Empty()
        {
            var res = AsyncEnumerable.Empty<int>().SingleOrDefaultAsync(x => true);
            Assert.Equal(0, await res);
        }

        [Fact]
        public async Task SingleOrDefaultAsync_Predicate_NoMatch_Single()
        {
            var res = Return42.SingleOrDefaultAsync(x => x % 2 != 0);
            Assert.Equal(0, await res);
        }

        [Fact]
        public async Task SingleOrDefaultAsync_Predicate_OneMatch_Single()
        {
            var res = Return42.SingleOrDefaultAsync(x => x % 2 == 0);
            Assert.Equal(42, await res);
        }

        [Fact]
        public async Task SingleOrDefaultAsync_Predicate_OneMatch_Many()
        {
            var res = new[] { 42, 45, 90 }.ToAsyncEnumerable().SingleOrDefaultAsync(x => x % 2 != 0);
            Assert.Equal(45, await res);
        }

        [Fact]
        public async Task SingleOrDefaultAsync_Predicate_NoMatch_Many()
        {
            var res = new[] { 42, 45, 90 }.ToAsyncEnumerable().SingleOrDefaultAsync(x => x < 10);
            Assert.Equal(0, await res);
        }

        [Fact]
        public async Task SingleOrDefaultAsync_Predicate_MoreThanOne()
        {
            var res = new[] { 42, 45, 90 }.ToAsyncEnumerable().SingleOrDefaultAsync(x => true);
            await AssertThrowsAsync<InvalidOperationException>(res.AsTask());
        }

        [Fact]
        public async Task SingleOrDefaultAsync_Predicate_Throws_Source()
        {
            var ex = new Exception("Bang!");
            var res = Throw<int>(ex).SingleOrDefaultAsync(x => true);
            await AssertThrowsAsync(res, ex);
        }

        [Fact]
        public async Task SingleOrDefaultAwaitAsync_Predicate_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SingleOrDefaultAwaitAsync<int>(default, x => new ValueTask<bool>(true)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SingleOrDefaultAwaitAsync(Return42, default).AsTask());

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SingleOrDefaultAwaitAsync<int>(default, x => new ValueTask<bool>(true), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SingleOrDefaultAwaitAsync(Return42, default, CancellationToken.None).AsTask());
        }

        [Fact]
        public async Task SingleOrDefaultAwaitAsync_Predicate_Empty()
        {
            var res = AsyncEnumerable.Empty<int>().SingleOrDefaultAwaitAsync(x => new ValueTask<bool>(true));
            Assert.Equal(0, await res);
        }

        [Fact]
        public async Task SingleOrDefaultAwaitAsync_Predicate_NoMatch_Single()
        {
            var res = Return42.SingleOrDefaultAwaitAsync(x => new ValueTask<bool>(x % 2 != 0));
            Assert.Equal(0, await res);
        }

        [Fact]
        public async Task SingleOrDefaultAwaitAsync_Predicate_OneMatch_Single()
        {
            var res = Return42.SingleOrDefaultAwaitAsync(x => new ValueTask<bool>(x % 2 == 0));
            Assert.Equal(42, await res);
        }

        [Fact]
        public async Task SingleOrDefaultAwaitAsync_Predicate_OneMatch_Many()
        {
            var res = new[] { 42, 45, 90 }.ToAsyncEnumerable().SingleOrDefaultAwaitAsync(x => new ValueTask<bool>(x % 2 != 0));
            Assert.Equal(45, await res);
        }

        [Fact]
        public async Task SingleOrDefaultAwaitAsync_Predicate_NoMatch_Many()
        {
            var res = new[] { 42, 45, 90 }.ToAsyncEnumerable().SingleOrDefaultAwaitAsync(x => new ValueTask<bool>(x < 10));
            Assert.Equal(0, await res);
        }

        [Fact]
        public async Task SingleOrDefaultAwaitAsync_Predicate_MoreThanOne()
        {
            var res = new[] { 42, 45, 90 }.ToAsyncEnumerable().SingleOrDefaultAwaitAsync(x => new ValueTask<bool>(true));
            await AssertThrowsAsync<InvalidOperationException>(res.AsTask());
        }

        [Fact]
        public async Task SingleOrDefaultAwaitAsync_Predicate_Throws_Source()
        {
            var ex = new Exception("Bang!");
            var res = Throw<int>(ex).SingleOrDefaultAwaitAsync(x => new ValueTask<bool>(true));
            await AssertThrowsAsync(res, ex);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task SingleOrDefaultAwaitWithCancellationAsync_Predicate_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SingleOrDefaultAwaitWithCancellationAsync<int>(default, (x, ct) => new ValueTask<bool>(true), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SingleOrDefaultAwaitWithCancellationAsync(Return42, default, CancellationToken.None).AsTask());
        }

        [Fact]
        public async Task SingleOrDefaultAwaitWithCancellationAsync_Predicate_Empty()
        {
            var res = AsyncEnumerable.Empty<int>().SingleOrDefaultAwaitWithCancellationAsync((x, ct) => new ValueTask<bool>(true));
            Assert.Equal(0, await res);
        }

        [Fact]
        public async Task SingleOrDefaultAwaitWithCancellationAsync_Predicate_NoMatch_Single()
        {
            var res = Return42.SingleOrDefaultAwaitWithCancellationAsync((x, ct) => new ValueTask<bool>(x % 2 != 0));
            Assert.Equal(0, await res);
        }

        [Fact]
        public async Task SingleOrDefaultAwaitWithCancellationAsync_Predicate_OneMatch_Single()
        {
            var res = Return42.SingleOrDefaultAwaitWithCancellationAsync((x, ct) => new ValueTask<bool>(x % 2 == 0));
            Assert.Equal(42, await res);
        }

        [Fact]
        public async Task SingleOrDefaultAwaitWithCancellationAsync_Predicate_OneMatch_Many()
        {
            var res = new[] { 42, 45, 90 }.ToAsyncEnumerable().SingleOrDefaultAwaitWithCancellationAsync((x, ct) => new ValueTask<bool>(x % 2 != 0));
            Assert.Equal(45, await res);
        }

        [Fact]
        public async Task SingleOrDefaultAwaitWithCancellationAsync_Predicate_NoMatch_Many()
        {
            var res = new[] { 42, 45, 90 }.ToAsyncEnumerable().SingleOrDefaultAwaitWithCancellationAsync((x, ct) => new ValueTask<bool>(x < 10));
            Assert.Equal(0, await res);
        }

        [Fact]
        public async Task SingleOrDefaultAwaitWithCancellationAsync_Predicate_MoreThanOne()
        {
            var res = new[] { 42, 45, 90 }.ToAsyncEnumerable().SingleOrDefaultAwaitWithCancellationAsync((x, ct) => new ValueTask<bool>(true));
            await AssertThrowsAsync<InvalidOperationException>(res.AsTask());
        }

        [Fact]
        public async Task SingleOrDefaultAwaitWithCancellationAsync_Predicate_Throws_Source()
        {
            var ex = new Exception("Bang!");
            var res = Throw<int>(ex).SingleOrDefaultAwaitWithCancellationAsync((x, ct) => new ValueTask<bool>(true));
            await AssertThrowsAsync(res, ex);
        }
#endif
    }
}

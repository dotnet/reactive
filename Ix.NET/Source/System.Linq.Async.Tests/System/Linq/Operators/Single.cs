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
        public async Task SingleAsync_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SingleAsync<int>(default).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SingleAsync<int>(default, CancellationToken.None).AsTask());
        }

        [Fact]
        public async Task SingleAsync_Empty()
        {
            var res = AsyncEnumerable.Empty<int>().SingleAsync();
            await AssertThrowsAsync<InvalidOperationException>(res.AsTask());
        }

        [Fact]
        public async Task SingleAsync_Empty_IList()
        {
            var res = new int[0].ToAsyncEnumerable().SingleAsync();
            await AssertThrowsAsync<InvalidOperationException>(res.AsTask());
        }

        [Fact]
        public async Task SingleAsync_Single()
        {
            var res = Return42.SingleAsync();
            Assert.Equal(42, await res);
        }

        [Fact]
        public async Task SingleAsync_Simple()
        {
            var res = new[] { 1, 2, 3, 4, 5 }.ToAsyncEnumerable().Where(x => x == 4).SingleAsync();
            Assert.Equal(4, await res);
        }

        [Fact]
        public async Task SingleAsync_Throws_Source()
        {
            var ex = new Exception("Bang!");
            var res = Throw<int>(ex).SingleAsync();
            await AssertThrowsAsync(res, ex);
        }

        [Fact]
        public async Task SingleAsync_Throw_MoreThanOne_IList()
        {
            var res = new[] { 42, 45, 90 }.ToAsyncEnumerable().SingleAsync();
            await AssertThrowsAsync<InvalidOperationException>(res.AsTask());
        }

        [Fact]
        public async Task SingleAsync_Throw_MoreThanOne()
        {
            var res = new[] { 42, 45, 90 }.ToAsyncEnumerable().Select(x => x).SingleAsync();
            await AssertThrowsAsync<InvalidOperationException>(res.AsTask());
        }

        [Fact]
        public async Task SingleAsync_Predicate_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SingleAsync<int>(default, x => true).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SingleAsync(Return42, default(Func<int, bool>)).AsTask());

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SingleAsync<int>(default, x => true, CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SingleAsync(Return42, default, CancellationToken.None).AsTask());
        }

        [Fact]
        public async Task SingleAsync_Predicate_Empty()
        {
            var res = AsyncEnumerable.Empty<int>().SingleAsync(x => true);
            await AssertThrowsAsync<InvalidOperationException>(res.AsTask());
        }

        [Fact]
        public async Task SingleAsync_Predicate_NoMatch()
        {
            var res = Return42.SingleAsync(x => x % 2 != 0);
            await AssertThrowsAsync<InvalidOperationException>(res.AsTask());
        }

        [Fact]
        public async Task SingleAsync_Predicate_Simple()
        {
            var res = Return42.SingleAsync(x => x % 2 == 0);
            Assert.Equal(42, await res);
        }

        [Fact]
        public async Task SingleAsync_Predicate_Throws()
        {
            var ex = new Exception("Bang!");
            var res = Throw<int>(ex).SingleAsync(x => true);
            await AssertThrowsAsync(res, ex);
        }

        [Fact]
        public async Task SingleAsync_Predicate_OneMatch()
        {
            var res = new[] { 42, 45, 90 }.ToAsyncEnumerable().SingleAsync(x => x % 2 != 0);
            Assert.Equal(45, await res);
        }

        [Fact]
        public async Task SingleAsync_Predicate_Throw_MoreThanOne()
        {
            var res = new[] { 42, 23, 45, 90 }.ToAsyncEnumerable().SingleAsync(x => x % 2 != 0);
            await AssertThrowsAsync<InvalidOperationException>(res.AsTask());
        }

        [Fact]
        public async Task SingleAwaitAsync_Predicate_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SingleAwaitAsync<int>(default, x => new ValueTask<bool>(true)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SingleAwaitAsync(Return42, default).AsTask());

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SingleAwaitAsync<int>(default, x => new ValueTask<bool>(true), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SingleAwaitAsync(Return42, default, CancellationToken.None).AsTask());
        }

        [Fact]
        public async Task SingleAwaitAsync_Predicate_Empty()
        {
            var res = AsyncEnumerable.Empty<int>().SingleAwaitAsync(x => new ValueTask<bool>(true));
            await AssertThrowsAsync<InvalidOperationException>(res.AsTask());
        }

        [Fact]
        public async Task SingleAwaitAsync_Predicate_NoMatch()
        {
            var res = Return42.SingleAwaitAsync(x => new ValueTask<bool>(x % 2 != 0));
            await AssertThrowsAsync<InvalidOperationException>(res.AsTask());
        }

        [Fact]
        public async Task SingleAwaitAsync_Predicate_Simple()
        {
            var res = Return42.SingleAwaitAsync(x => new ValueTask<bool>(x % 2 == 0));
            Assert.Equal(42, await res);
        }

        [Fact]
        public async Task SingleAwaitAsync_Predicate_Throws()
        {
            var ex = new Exception("Bang!");
            var res = Throw<int>(ex).SingleAwaitAsync(x => new ValueTask<bool>(true));
            await AssertThrowsAsync(res, ex);
        }

        [Fact]
        public async Task SingleAwaitAsync_Predicate_OneMatch()
        {
            var res = new[] { 42, 45, 90 }.ToAsyncEnumerable().SingleAwaitAsync(x => new ValueTask<bool>(x % 2 != 0));
            Assert.Equal(45, await res);
        }

        [Fact]
        public async Task SingleAwaitAsync_Predicate_Throw_MoreThanOne()
        {
            var res = new[] { 42, 23, 45, 90 }.ToAsyncEnumerable().SingleAwaitAsync(x => new ValueTask<bool>(x % 2 != 0));
            await AssertThrowsAsync<InvalidOperationException>(res.AsTask());
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task SingleAwaitWithCancellationAsync_Predicate_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SingleAwaitWithCancellationAsync<int>(default, (x, ct) => new ValueTask<bool>(true), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SingleAwaitWithCancellationAsync(Return42, default, CancellationToken.None).AsTask());
        }

        [Fact]
        public async Task SingleAwaitWithCancellationAsync_Predicate_Empty()
        {
            var res = AsyncEnumerable.Empty<int>().SingleAwaitWithCancellationAsync((x, ct) => new ValueTask<bool>(true));
            await AssertThrowsAsync<InvalidOperationException>(res.AsTask());
        }

        [Fact]
        public async Task SingleAwaitWithCancellationAsync_Predicate_NoMatch()
        {
            var res = Return42.SingleAwaitWithCancellationAsync((x, ct) => new ValueTask<bool>(x % 2 != 0));
            await AssertThrowsAsync<InvalidOperationException>(res.AsTask());
        }

        [Fact]
        public async Task SingleAwaitWithCancellationAsync_Predicate_Simple()
        {
            var res = Return42.SingleAwaitWithCancellationAsync((x, ct) => new ValueTask<bool>(x % 2 == 0));
            Assert.Equal(42, await res);
        }

        [Fact]
        public async Task SingleAwaitWithCancellationAsync_Predicate_Throws()
        {
            var ex = new Exception("Bang!");
            var res = Throw<int>(ex).SingleAwaitWithCancellationAsync((x, ct) => new ValueTask<bool>(true));
            await AssertThrowsAsync(res, ex);
        }

        [Fact]
        public async Task SingleAwaitWithCancellationAsync_Predicate_OneMatch()
        {
            var res = new[] { 42, 45, 90 }.ToAsyncEnumerable().SingleAwaitWithCancellationAsync((x, ct) => new ValueTask<bool>(x % 2 != 0));
            Assert.Equal(45, await res);
        }

        [Fact]
        public async Task SingleAwaitWithCancellationAsync_Predicate_Throw_MoreThanOne()
        {
            var res = new[] { 42, 23, 45, 90 }.ToAsyncEnumerable().SingleAwaitWithCancellationAsync((x, ct) => new ValueTask<bool>(x % 2 != 0));
            await AssertThrowsAsync<InvalidOperationException>(res.AsTask());
        }
#endif
    }
}

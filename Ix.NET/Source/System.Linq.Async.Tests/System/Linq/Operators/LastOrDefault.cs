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
    public class LastOrDefault : AsyncEnumerableTests
    {
        [Fact]
        public async Task LastOrDefaultAsync_NoParam_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.LastOrDefaultAsync<int>(default).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.LastOrDefaultAsync<int>(default, CancellationToken.None).AsTask());
        }

        [Fact]
        public async Task LastOrDefaultAsync_NoParam_Empty()
        {
            var res = AsyncEnumerable.Empty<int>().LastOrDefaultAsync();
            Assert.Equal(0, await res);
        }

        [Fact]
        public async Task LasyOrDefaultAsync_NoParam_Empty_Enumerable()
        {
            var res = new int[0].Select(x => x).ToAsyncEnumerable().LastOrDefaultAsync();
            Assert.Equal(0, await res);
        }

        [Fact]
        public async Task LastOrDefaultAsync_NoParam_Empty_IList()
        {
            var res = new int[0].ToAsyncEnumerable().LastOrDefaultAsync();
            Assert.Equal(0, await res);
        }

        [Fact]
        public async Task LastOrDefaultAsync_NoParam_Throw()
        {
            var ex = new Exception("Bang!");
            var res = Throw<int>(ex).LastOrDefaultAsync();
            await AssertThrowsAsync(res, ex);
        }

        [Fact]
        public async Task LastOrDefaultAsync_NoParam_Single_IList()
        {
            var res = Return42.LastOrDefaultAsync();
            Assert.Equal(42, await res);
        }

        [Fact]
        public async Task LastOrDefaultAsync_NoParam_Single()
        {
            var res = new[] { 42 }.Select(x => x).ToAsyncEnumerable().LastOrDefaultAsync();
            Assert.Equal(42, await res);
        }

        [Fact]
        public async Task LastOrDefaultAsync_NoParam_Many_IList()
        {
            var res = new[] { 42, 43, 44 }.ToAsyncEnumerable().LastOrDefaultAsync();
            Assert.Equal(44, await res);
        }

        [Fact]
        public async Task LastOrDefaultAsync_NoParam_Many()
        {
            var res = new[] { 42, 43, 44 }.Select(x => x).ToAsyncEnumerable().LastOrDefaultAsync();
            Assert.Equal(44, await res);
        }

        [Fact]
        public async Task LastOrDefaultAsync_Predicate_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.LastOrDefaultAsync<int>(default, x => true).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.LastOrDefaultAsync(Return42, default(Func<int, bool>)).AsTask());

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.LastOrDefaultAsync<int>(default, x => true, CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.LastOrDefaultAsync(Return42, default, CancellationToken.None).AsTask());
        }

        [Fact]
        public async Task LastOrDefaultAsync_Predicate_Empty()
        {
            var res = AsyncEnumerable.Empty<int>().LastOrDefaultAsync(x => true);
            Assert.Equal(0, await res);
        }

        [Fact]
        public async Task LastOrDefaultAsync_Predicate_Throw()
        {
            var ex = new Exception("Bang!");
            var res = Throw<int>(ex).LastOrDefaultAsync(x => true);
            await AssertThrowsAsync(res, ex);
        }

        [Fact]
        public async Task LastOrDefaultAsync_Predicate_Single_None()
        {
            var res = Return42.LastOrDefaultAsync(x => x % 2 != 0);
            Assert.Equal(0, await res);
        }

        [Fact]
        public async Task LastOrDefaultAsync_Predicate_Many_IList_None()
        {
            var res = new[] { 40, 42, 44 }.ToAsyncEnumerable().LastOrDefaultAsync(x => x % 2 != 0);
            Assert.Equal(0, await res);
        }

        [Fact]
        public async Task LastOrDefaultAsync_Predicate_Many_None()
        {
            var res = new[] { 40, 42, 44 }.Select(x => x).ToAsyncEnumerable().LastOrDefaultAsync(x => x % 2 != 0);
            Assert.Equal(0, await res);
        }

        [Fact]
        public async Task LastOrDefaultAsync_Predicate_Single_Pass()
        {
            var res = Return42.LastOrDefaultAsync(x => x % 2 == 0);
            Assert.Equal(42, await res);
        }

        [Fact]
        public async Task LastOrDefaultAsync_Predicate_Many_IList_Pass1()
        {
            var res = new[] { 43, 44, 45 }.ToAsyncEnumerable().LastOrDefaultAsync(x => x % 2 != 0);
            Assert.Equal(45, await res);
        }

        [Fact]
        public async Task LastOrDefaultAsync_Predicate_Many_IList_Pass2()
        {
            var res = new[] { 42, 45, 90 }.ToAsyncEnumerable().LastOrDefaultAsync(x => x % 2 != 0);
            Assert.Equal(45, await res);
        }

        [Fact]
        public async Task LastOrDefaultAsync_Predicate_Many_Pass1()
        {
            var res = new[] { 43, 44, 45 }.Select(x => x).ToAsyncEnumerable().LastOrDefaultAsync(x => x % 2 != 0);
            Assert.Equal(45, await res);
        }

        [Fact]
        public async Task LastOrDefaultAsync_Predicate_Many_Pass2()
        {
            var res = new[] { 42, 45, 90 }.Select(x => x).ToAsyncEnumerable().LastOrDefaultAsync(x => x % 2 != 0);
            Assert.Equal(45, await res);
        }

        [Fact]
        public async Task LastOrDefaultAsync_Predicate_PredicateThrows()
        {
            var res = new[] { 0, 1, 2 }.ToAsyncEnumerable().LastOrDefaultAsync(x => 1 / x > 0);
            await AssertThrowsAsync<DivideByZeroException>(res.AsTask());
        }

        [Fact]
        public async Task LastOrDefaultAwaitAsync_Predicate_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.LastOrDefaultAwaitAsync<int>(default, x => new ValueTask<bool>(true)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.LastOrDefaultAwaitAsync(Return42, default).AsTask());

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.LastOrDefaultAwaitAsync<int>(default, x => new ValueTask<bool>(true), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.LastOrDefaultAwaitAsync(Return42, default, CancellationToken.None).AsTask());
        }

        [Fact]
        public async Task LastOrDefaultAwaitAsync_Predicate_Empty()
        {
            var res = AsyncEnumerable.Empty<int>().LastOrDefaultAwaitAsync(x => new ValueTask<bool>(true));
            Assert.Equal(0, await res);
        }

        [Fact]
        public async Task LastOrDefaultAwaitAsync_Predicate_Throw()
        {
            var ex = new Exception("Bang!");
            var res = Throw<int>(ex).LastOrDefaultAwaitAsync(x => new ValueTask<bool>(true));
            await AssertThrowsAsync(res, ex);
        }

        [Fact]
        public async Task LastOrDefaultAwaitAsync_Predicate_Single_None()
        {
            var res = Return42.LastOrDefaultAwaitAsync(x => new ValueTask<bool>(x % 2 != 0));
            Assert.Equal(0, await res);
        }

        [Fact]
        public async Task LastOrDefaultAwaitAsync_Predicate_Many_IList_None()
        {
            var res = new[] { 40, 42, 44 }.ToAsyncEnumerable().LastOrDefaultAwaitAsync(x => new ValueTask<bool>(x % 2 != 0));
            Assert.Equal(0, await res);
        }

        [Fact]
        public async Task LastOrDefaultAwaitAsync_Predicate_Many_None()
        {
            var res = new[] { 40, 42, 44 }.Select(x => x).ToAsyncEnumerable().LastOrDefaultAwaitAsync(x => new ValueTask<bool>(x % 2 != 0));
            Assert.Equal(0, await res);
        }

        [Fact]
        public async Task LastOrDefaultAwaitAsync_Predicate_Single_Pass()
        {
            var res = Return42.LastOrDefaultAwaitAsync(x => new ValueTask<bool>(x % 2 == 0));
            Assert.Equal(42, await res);
        }

        [Fact]
        public async Task LastOrDefaultAwaitAsync_Predicate_Many_IList_Pass1()
        {
            var res = new[] { 43, 44, 45 }.ToAsyncEnumerable().LastOrDefaultAwaitAsync(x => new ValueTask<bool>(x % 2 != 0));
            Assert.Equal(45, await res);
        }

        [Fact]
        public async Task LastOrDefaultAwaitAsync_Predicate_Many_IList_Pass2()
        {
            var res = new[] { 42, 45, 90 }.ToAsyncEnumerable().LastOrDefaultAwaitAsync(x => new ValueTask<bool>(x % 2 != 0));
            Assert.Equal(45, await res);
        }

        [Fact]
        public async Task LastOrDefaultAwaitAsync_Predicate_Many_Pass1()
        {
            var res = new[] { 43, 44, 45 }.Select(x => x).ToAsyncEnumerable().LastOrDefaultAwaitAsync(x => new ValueTask<bool>(x % 2 != 0));
            Assert.Equal(45, await res);
        }

        [Fact]
        public async Task LastOrDefaultAwaitAsync_Predicate_Many_Pass2()
        {
            var res = new[] { 42, 45, 90 }.Select(x => x).ToAsyncEnumerable().LastOrDefaultAwaitAsync(x => new ValueTask<bool>(x % 2 != 0));
            Assert.Equal(45, await res);
        }

        [Fact]
        public async Task LastOrDefaultAwaitAsync_Predicate_AsyncPredicateThrows()
        {
            var res = new[] { 0, 1, 2 }.ToAsyncEnumerable().LastOrDefaultAwaitAsync(x => new ValueTask<bool>(1 / x > 0));
            await AssertThrowsAsync<DivideByZeroException>(res.AsTask());
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task LastOrDefaultAwaitWithCancellationAsync_Predicate_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.LastOrDefaultAwaitWithCancellationAsync<int>(default, (x, ct) => new ValueTask<bool>(true), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.LastOrDefaultAwaitWithCancellationAsync(Return42, default, CancellationToken.None).AsTask());
        }

        [Fact]
        public async Task LastOrDefaultAwaitWithCancellationAsync_Predicate_Empty()
        {
            var res = AsyncEnumerable.Empty<int>().LastOrDefaultAwaitWithCancellationAsync((x, ct) => new ValueTask<bool>(true), CancellationToken.None);
            Assert.Equal(0, await res);
        }

        [Fact]
        public async Task LastOrDefaultAwaitWithCancellationAsync_Predicate_Throw()
        {
            var ex = new Exception("Bang!");
            var res = Throw<int>(ex).LastOrDefaultAwaitWithCancellationAsync((x, ct) => new ValueTask<bool>(true), CancellationToken.None);
            await AssertThrowsAsync(res, ex);
        }

        [Fact]
        public async Task LastOrDefaultAwaitWithCancellationAsync_Predicate_Single_None()
        {
            var res = Return42.LastOrDefaultAwaitWithCancellationAsync((x, ct) => new ValueTask<bool>(x % 2 != 0), CancellationToken.None);
            Assert.Equal(0, await res);
        }

        [Fact]
        public async Task LastOrDefaultAwaitWithCancellationAsync_Predicate_Many_IList_None()
        {
            var res = new[] { 40, 42, 44 }.ToAsyncEnumerable().LastOrDefaultAwaitWithCancellationAsync((x, ct) => new ValueTask<bool>(x % 2 != 0), CancellationToken.None);
            Assert.Equal(0, await res);
        }

        [Fact]
        public async Task LastOrDefaultAwaitWithCancellationAsync_Predicate_Many_None()
        {
            var res = new[] { 40, 42, 44 }.Select((x, ct) => x).ToAsyncEnumerable().LastOrDefaultAwaitWithCancellationAsync((x, ct) => new ValueTask<bool>(x % 2 != 0), CancellationToken.None);
            Assert.Equal(0, await res);
        }

        [Fact]
        public async Task LastOrDefaultAwaitWithCancellationAsync_Predicate_Single_Pass()
        {
            var res = Return42.LastOrDefaultAwaitWithCancellationAsync((x, ct) => new ValueTask<bool>(x % 2 == 0), CancellationToken.None);
            Assert.Equal(42, await res);
        }

        [Fact]
        public async Task LastOrDefaultAwaitWithCancellationAsync_Predicate_Many_IList_Pass1()
        {
            var res = new[] { 43, 44, 45 }.ToAsyncEnumerable().LastOrDefaultAwaitWithCancellationAsync((x, ct) => new ValueTask<bool>(x % 2 != 0), CancellationToken.None);
            Assert.Equal(45, await res);
        }

        [Fact]
        public async Task LastOrDefaultAwaitWithCancellationAsync_Predicate_Many_IList_Pass2()
        {
            var res = new[] { 42, 45, 90 }.ToAsyncEnumerable().LastOrDefaultAwaitWithCancellationAsync((x, ct) => new ValueTask<bool>(x % 2 != 0), CancellationToken.None);
            Assert.Equal(45, await res);
        }

        [Fact]
        public async Task LastOrDefaultAwaitWithCancellationAsync_Predicate_Many_Pass1()
        {
            var res = new[] { 43, 44, 45 }.Select((x, ct) => x).ToAsyncEnumerable().LastOrDefaultAwaitWithCancellationAsync((x, ct) => new ValueTask<bool>(x % 2 != 0), CancellationToken.None);
            Assert.Equal(45, await res);
        }

        [Fact]
        public async Task LastOrDefaultAwaitWithCancellationAsync_Predicate_Many_Pass2()
        {
            var res = new[] { 42, 45, 90 }.Select((x, ct) => x).ToAsyncEnumerable().LastOrDefaultAwaitWithCancellationAsync((x, ct) => new ValueTask<bool>(x % 2 != 0), CancellationToken.None);
            Assert.Equal(45, await res);
        }

        [Fact]
        public async Task LastOrDefaultAwaitWithCancellationAsync_Predicate_AsyncPredicateWithCancellationThrows()
        {
            var res = new[] { 0, 1, 2 }.ToAsyncEnumerable().LastOrDefaultAwaitWithCancellationAsync((x, ct) => new ValueTask<bool>(1 / x > 0), CancellationToken.None);
            await AssertThrowsAsync<DivideByZeroException>(res.AsTask());
        }
#endif
    }
}

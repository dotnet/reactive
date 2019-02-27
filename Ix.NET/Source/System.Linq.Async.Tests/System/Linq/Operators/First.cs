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
        public async Task FirstAsync_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.FirstAsync<int>(default).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.FirstAsync<int>(default, CancellationToken.None).AsTask());
        }

        [Fact]
        public async Task FirstAsync_NoParam_Empty()
        {
            var res = AsyncEnumerable.Empty<int>().FirstAsync();
            await AssertThrowsAsync<InvalidOperationException>(res.AsTask());
        }

        [Fact]
        public async Task FirstAsync_NoParam_Empty_Enumerable()
        {
            var res = new int[0].Select(x => x).ToAsyncEnumerable().FirstAsync();
            await AssertThrowsAsync<InvalidOperationException>(res.AsTask());
        }

        [Fact]
        public async Task FirstAsync_NoParam_Empty_IList()
        {
            var res = new int[0].ToAsyncEnumerable().FirstAsync();
            await AssertThrowsAsync<InvalidOperationException>(res.AsTask());
        }

        [Fact]
        public async Task FirstAsync_NoParam_Throw()
        {
            var ex = new Exception("Bang!");
            var res = Throw<int>(ex).FirstAsync();
            await AssertThrowsAsync(res, ex);
        }

        [Fact]
        public async Task FirstAsync_NoParam_Single_IList()
        {
            var res = Return42.FirstAsync();
            Assert.Equal(42, await res);
        }

        [Fact]
        public async Task FirstAsync_NoParam_Single()
        {
            var res = new[] { 42 }.Select(x => x).ToAsyncEnumerable().FirstAsync();
            Assert.Equal(42, await res);
        }

        [Fact]
        public async Task FirstAsync_NoParam_Many_IList()
        {
            var res = new[] { 42, 43, 44 }.ToAsyncEnumerable().FirstAsync();
            Assert.Equal(42, await res);
        }

        [Fact]
        public async Task FirstAsync_NoParam_Many()
        {
            var res = new[] { 42, 43, 44 }.Select(x => x).ToAsyncEnumerable().FirstAsync();
            Assert.Equal(42, await res);
        }

        [Fact]
        public async Task FirstAsync_Predicate_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.FirstAsync<int>(default, x => true).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.FirstAsync(Return42, default(Func<int, bool>)).AsTask());

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.FirstAsync<int>(default, x => true, CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.FirstAsync(Return42, default, CancellationToken.None).AsTask());
        }

        [Fact]
        public async Task FirstAsync_Predicate_Empty()
        {
            var res = AsyncEnumerable.Empty<int>().FirstAsync(x => true);
            await AssertThrowsAsync<InvalidOperationException>(res.AsTask());
        }

        [Fact]
        public async Task FirstAsync_Predicate_Throw()
        {
            var ex = new Exception("Bang!");
            var res = Throw<int>(ex).FirstAsync(x => true);
            await AssertThrowsAsync(res, ex);
        }

        [Fact]
        public async Task FirstAsync_Predicate_Single_None()
        {
            var res = Return42.FirstAsync(x => x % 2 != 0);
            await AssertThrowsAsync<InvalidOperationException>(res.AsTask());
        }

        [Fact]
        public async Task FirstAsync_Predicate_Many_IList_None()
        {
            var res = new[] { 40, 42, 44 }.ToAsyncEnumerable().FirstAsync(x => x % 2 != 0);
            await AssertThrowsAsync<InvalidOperationException>(res.AsTask());
        }

        [Fact]
        public async Task FirstAsync_Predicate_Many_None()
        {
            var res = new[] { 40, 42, 44 }.Select(x => x).ToAsyncEnumerable().FirstAsync(x => x % 2 != 0);
            await AssertThrowsAsync<InvalidOperationException>(res.AsTask());
        }

        [Fact]
        public async Task FirstAsync_Predicate_Single_Pass()
        {
            var res = Return42.FirstAsync(x => x % 2 == 0);
            Assert.Equal(42, await res);
        }

        [Fact]
        public async Task FirstAsync_Predicate_Many_IList_Pass1()
        {
            var res = new[] { 42, 43, 44 }.ToAsyncEnumerable().FirstAsync(x => x % 2 != 0);
            Assert.Equal(43, await res);
        }

        [Fact]
        public async Task FirstAsync_Predicate_Many_IList_Pass2()
        {
            var res = new[] { 42, 45, 90 }.ToAsyncEnumerable().FirstAsync(x => x % 2 != 0);
            Assert.Equal(45, await res);
        }

        [Fact]
        public async Task FirstAsync_Predicate_Many_Pass1()
        {
            var res = new[] { 42, 43, 44 }.Select(x => x).ToAsyncEnumerable().FirstAsync(x => x % 2 != 0);
            Assert.Equal(43, await res);
        }

        [Fact]
        public async Task FirstAsync_Predicate_Many_Pass2()
        {
            var res = new[] { 42, 45, 90 }.Select(x => x).ToAsyncEnumerable().FirstAsync(x => x % 2 != 0);
            Assert.Equal(45, await res);
        }

        [Fact]
        public async Task FirstAsync_Predicate_PredicateThrows()
        {
            var res = new[] { 0, 1, 2 }.ToAsyncEnumerable().FirstAsync(x => 1 / x > 0);
            await AssertThrowsAsync<DivideByZeroException>(res.AsTask());
        }

        [Fact]
        public async Task FirstAwaitAsync_Predicate_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.FirstAwaitAsync<int>(default, x => new ValueTask<bool>(true)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.FirstAwaitAsync(Return42, default).AsTask());

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.FirstAwaitAsync<int>(default, x => new ValueTask<bool>(true), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.FirstAwaitAsync(Return42, default, CancellationToken.None).AsTask());
        }

        [Fact]
        public async Task FirstAwaitAsync_Predicate_Empty()
        {
            var res = AsyncEnumerable.Empty<int>().FirstAwaitAsync(x => new ValueTask<bool>(true));
            await AssertThrowsAsync<InvalidOperationException>(res.AsTask());
        }

        [Fact]
        public async Task FirstAwaitAsync_Predicate_Throw()
        {
            var ex = new Exception("Bang!");
            var res = Throw<int>(ex).FirstAwaitAsync(x => new ValueTask<bool>(true));
            await AssertThrowsAsync(res, ex);
        }

        [Fact]
        public async Task FirstAwaitAsync_Predicate_Single_None()
        {
            var res = Return42.FirstAwaitAsync(x => new ValueTask<bool>(x % 2 != 0));
            await AssertThrowsAsync<InvalidOperationException>(res.AsTask());
        }

        [Fact]
        public async Task FirstAwaitAsync_Predicate_Many_IList_None()
        {
            var res = new[] { 40, 42, 44 }.ToAsyncEnumerable().FirstAwaitAsync(x => new ValueTask<bool>(x % 2 != 0));
            await AssertThrowsAsync<InvalidOperationException>(res.AsTask());
        }

        [Fact]
        public async Task FirstAwaitAsync_Predicate_Many_None()
        {
            var res = new[] { 40, 42, 44 }.ToAsyncEnumerable().Select(x => x).FirstAwaitAsync(x => new ValueTask<bool>(x % 2 != 0));
            await AssertThrowsAsync<InvalidOperationException>(res.AsTask());
        }

        [Fact]
        public async Task FirstAwaitAsync_Predicate_Single_Pass()
        {
            var res = Return42.FirstAwaitAsync(x => new ValueTask<bool>(x % 2 == 0));
            Assert.Equal(42, await res);
        }

        [Fact]
        public async Task FirstAwaitAsync_Predicate_Many_IList_Pass1()
        {
            var res = new[] { 42, 43, 44 }.ToAsyncEnumerable().FirstAwaitAsync(x => new ValueTask<bool>(x % 2 != 0));
            Assert.Equal(43, await res);
        }

        [Fact]
        public async Task FirstAwaitAsync_Predicate_Many_IList_Pass2()
        {
            var res = new[] { 42, 45, 90 }.ToAsyncEnumerable().FirstAwaitAsync(x => new ValueTask<bool>(x % 2 != 0));
            Assert.Equal(45, await res);
        }

        [Fact]
        public async Task FirstAwaitAsync_Predicate_Many_Pass1()
        {
            var res = new[] { 42, 43, 44 }.Select(x => x).ToAsyncEnumerable().FirstAwaitAsync(x => new ValueTask<bool>(x % 2 != 0));
            Assert.Equal(43, await res);
        }

        [Fact]
        public async Task FirstAwaitAsync_Predicate_Many_Pass2()
        {
            var res = new[] { 42, 45, 90 }.Select(x => x).ToAsyncEnumerable().FirstAwaitAsync(x => new ValueTask<bool>(x % 2 != 0));
            Assert.Equal(45, await res);
        }

        [Fact]
        public async Task FirstAwaitAsync_Predicate_AsyncPredicateThrows()
        {
            var res = new[] { 0, 1, 2 }.ToAsyncEnumerable().FirstAwaitAsync(x => new ValueTask<bool>(1 / x > 0));
            await AssertThrowsAsync<DivideByZeroException>(res.AsTask());
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task FirstAwaitWithCancellationAsync_Predicate_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.FirstAwaitWithCancellationAsync<int>(default, (x, ct) => new ValueTask<bool>(true), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.FirstAwaitWithCancellationAsync(Return42, default, CancellationToken.None).AsTask());
        }

        [Fact]
        public async Task FirstAwaitWithCancellationAsync_Predicate_Empty()
        {
            var res = AsyncEnumerable.Empty<int>().FirstAwaitWithCancellationAsync((x, ct) => new ValueTask<bool>(true), CancellationToken.None);
            await AssertThrowsAsync<InvalidOperationException>(res.AsTask());
        }

        [Fact]
        public async Task FirstAwaitWithCancellationAsync_Predicate_Throw()
        {
            var ex = new Exception("Bang!");
            var res = Throw<int>(ex).FirstAwaitWithCancellationAsync((x, ct) => new ValueTask<bool>(true), CancellationToken.None);
            await AssertThrowsAsync(res, ex);
        }

        [Fact]
        public async Task FirstAwaitWithCancellationAsync_Predicate_Single_None()
        {
            var res = Return42.FirstAwaitWithCancellationAsync((x, ct) => new ValueTask<bool>(x % 2 != 0), CancellationToken.None);
            await AssertThrowsAsync<InvalidOperationException>(res.AsTask());
        }

        [Fact]
        public async Task FirstAwaitWithCancellationAsync_Predicate_Many_IList_None()
        {
            var res = new[] { 40, 42, 44 }.ToAsyncEnumerable().FirstAwaitWithCancellationAsync((x, ct) => new ValueTask<bool>(x % 2 != 0), CancellationToken.None);
            await AssertThrowsAsync<InvalidOperationException>(res.AsTask());
        }

        [Fact]
        public async Task FirstAwaitWithCancellationAsync_Predicate_Many_None()
        {
            var res = new[] { 40, 42, 44 }.ToAsyncEnumerable().Select((x, ct) => x).FirstAwaitWithCancellationAsync((x, ct) => new ValueTask<bool>(x % 2 != 0), CancellationToken.None);
            await AssertThrowsAsync<InvalidOperationException>(res.AsTask());
        }

        [Fact]
        public async Task FirstAwaitWithCancellationAsync_Predicate_Single_Pass()
        {
            var res = Return42.FirstAwaitWithCancellationAsync((x, ct) => new ValueTask<bool>(x % 2 == 0), CancellationToken.None);
            Assert.Equal(42, await res);
        }

        [Fact]
        public async Task FirstAwaitWithCancellationAsync_Predicate_Many_IList_Pass1()
        {
            var res = new[] { 42, 43, 44 }.ToAsyncEnumerable().FirstAwaitWithCancellationAsync((x, ct) => new ValueTask<bool>(x % 2 != 0), CancellationToken.None);
            Assert.Equal(43, await res);
        }

        [Fact]
        public async Task FirstAwaitWithCancellationAsync_Predicate_Many_IList_Pass2()
        {
            var res = new[] { 42, 45, 90 }.ToAsyncEnumerable().FirstAwaitWithCancellationAsync((x, ct) => new ValueTask<bool>(x % 2 != 0), CancellationToken.None);
            Assert.Equal(45, await res);
        }

        [Fact]
        public async Task FirstAwaitWithCancellationAsync_Predicate_Many_Pass1()
        {
            var res = new[] { 42, 43, 44 }.Select((x, ct) => x).ToAsyncEnumerable().FirstAwaitWithCancellationAsync((x, ct) => new ValueTask<bool>(x % 2 != 0), CancellationToken.None);
            Assert.Equal(43, await res);
        }

        [Fact]
        public async Task FirstAwaitWithCancellationAsync_Predicate_Many_Pass2()
        {
            var res = new[] { 42, 45, 90 }.Select((x, ct) => x).ToAsyncEnumerable().FirstAwaitWithCancellationAsync((x, ct) => new ValueTask<bool>(x % 2 != 0), CancellationToken.None);
            Assert.Equal(45, await res);
        }

        [Fact]
        public async Task FirstAwaitWithCancellationAsync_Predicate_AsyncPredicateWithCancellationThrows()
        {
            var res = new[] { 0, 1, 2 }.ToAsyncEnumerable().FirstAwaitWithCancellationAsync((x, ct) => new ValueTask<bool>(1 / x > 0), CancellationToken.None);
            await AssertThrowsAsync<DivideByZeroException>(res.AsTask());
        }
#endif
    }
}

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
    public class Last : AsyncEnumerableTests
    {
        [Fact]
        public async Task LastAsync_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.LastAsync<int>(default).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.LastAsync<int>(default, CancellationToken.None).AsTask());
        }

        [Fact]
        public async Task LastAsync_NoParam_Empty()
        {
            var res = AsyncEnumerable.Empty<int>().LastAsync();
            await AssertThrowsAsync<InvalidOperationException>(res.AsTask());
        }

        [Fact]
        public async Task LastAsync_NoParam_Empty_Enumerable()
        {
            var res = new int[0].Select(x => x).ToAsyncEnumerable().LastAsync();
            await AssertThrowsAsync<InvalidOperationException>(res.AsTask());
        }

        [Fact]
        public async Task LastAsync_NoParam_Empty_IList()
        {
            var res = new int[0].ToAsyncEnumerable().LastAsync();
            await AssertThrowsAsync<InvalidOperationException>(res.AsTask());
        }

        [Fact]
        public async Task LastAsync_NoParam_Throw()
        {
            var ex = new Exception("Bang!");
            var res = Throw<int>(ex).LastAsync();
            await AssertThrowsAsync(res, ex);
        }

        [Fact]
        public async Task LastAsync_NoParam_Single_IList()
        {
            var res = Return42.LastAsync();
            Assert.Equal(42, await res);
        }

        [Fact]
        public async Task LastAsync_NoParam_Single()
        {
            var res = new[] { 42 }.Select(x => x).ToAsyncEnumerable().LastAsync();
            Assert.Equal(42, await res);
        }

        [Fact]
        public async Task LastAsync_NoParam_Many_IList()
        {
            var res = new[] { 42, 43, 44 }.ToAsyncEnumerable().LastAsync();
            Assert.Equal(44, await res);
        }

        [Fact]
        public async Task LastAsync_NoParam_Many()
        {
            var res = new[] { 42, 43, 44 }.Select(x => x).ToAsyncEnumerable().LastAsync();
            Assert.Equal(44, await res);
        }

        [Fact]
        public async Task LastAsync_Predicate_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.LastAsync<int>(default, x => true).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.LastAsync(Return42, default(Func<int, bool>)).AsTask());

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.LastAsync<int>(default, x => true, CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.LastAsync(Return42, default, CancellationToken.None).AsTask());
        }

        [Fact]
        public async Task LastAsync_Predicate_Empty()
        {
            var res = AsyncEnumerable.Empty<int>().LastAsync(x => true);
            await AssertThrowsAsync<InvalidOperationException>(res.AsTask());
        }

        [Fact]
        public async Task LastAsync_Predicate_Throw()
        {
            var ex = new Exception("Bang!");
            var res = Throw<int>(ex).LastAsync(x => true);
            await AssertThrowsAsync(res, ex);
        }

        [Fact]
        public async Task LastAsync_Predicate_Single_None()
        {
            var res = Return42.LastAsync(x => x % 2 != 0);
            await AssertThrowsAsync<InvalidOperationException>(res.AsTask());
        }

        [Fact]
        public async Task LastAsync_Predicate_Many_IList_None()
        {
            var res = new[] { 40, 42, 44 }.ToAsyncEnumerable().LastAsync(x => x % 2 != 0);
            await AssertThrowsAsync<InvalidOperationException>(res.AsTask());
        }

        [Fact]
        public async Task LastAsync_Predicate_Many_None()
        {
            var res = new[] { 40, 42, 44 }.Select(x => x).ToAsyncEnumerable().LastAsync(x => x % 2 != 0);
            await AssertThrowsAsync<InvalidOperationException>(res.AsTask());
        }

        [Fact]
        public async Task LastAsync_Predicate_Single_Pass()
        {
            var res = Return42.LastAsync(x => x % 2 == 0);
            Assert.Equal(42, await res);
        }

        [Fact]
        public async Task LastAsync_Predicate_Many_IList_Pass1()
        {
            var res = new[] { 43, 44, 45 }.ToAsyncEnumerable().LastAsync(x => x % 2 != 0);
            Assert.Equal(45, await res);
        }

        [Fact]
        public async Task LastAsync_Predicate_Many_IList_Pass2()
        {
            var res = new[] { 42, 45, 90 }.ToAsyncEnumerable().LastAsync(x => x % 2 != 0);
            Assert.Equal(45, await res);
        }

        [Fact]
        public async Task LastAsync_Predicate_Many_Pass1()
        {
            var res = new[] { 43, 44, 45 }.Select(x => x).ToAsyncEnumerable().LastAsync(x => x % 2 != 0);
            Assert.Equal(45, await res);
        }

        [Fact]
        public async Task LastAsync_Predicate_Many_Pass2()
        {
            var res = new[] { 42, 45, 90 }.Select(x => x).ToAsyncEnumerable().LastAsync(x => x % 2 != 0);
            Assert.Equal(45, await res);
        }

        [Fact]
        public async Task LastAsync_Predicate_PredicateThrows()
        {
            var res = new[] { 0, 1, 2 }.ToAsyncEnumerable().LastAsync(x => 1 / x > 0);
            await AssertThrowsAsync<DivideByZeroException>(res.AsTask());
        }

        [Fact]
        public async Task LastAwaitAsync_Predicate_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.LastAwaitAsync<int>(default, x => new ValueTask<bool>(true)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.LastAwaitAsync(Return42, default).AsTask());

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.LastAwaitAsync<int>(default, x => new ValueTask<bool>(true), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.LastAwaitAsync(Return42, default, CancellationToken.None).AsTask());
        }

        [Fact]
        public async Task LastAwaitAsync_Predicate_Empty()
        {
            var res = AsyncEnumerable.Empty<int>().LastAwaitAsync(x => new ValueTask<bool>(true));
            await AssertThrowsAsync<InvalidOperationException>(res.AsTask());
        }

        [Fact]
        public async Task LastAwaitAsync_Predicate_Throw()
        {
            var ex = new Exception("Bang!");
            var res = Throw<int>(ex).LastAwaitAsync(x => new ValueTask<bool>(true));
            await AssertThrowsAsync(res, ex);
        }

        [Fact]
        public async Task LastAwaitAsync_Predicate_Single_None()
        {
            var res = Return42.LastAwaitAsync(x => new ValueTask<bool>(x % 2 != 0));
            await AssertThrowsAsync<InvalidOperationException>(res.AsTask());
        }

        [Fact]
        public async Task LastAwaitAsync_Predicate_Many_IList_None()
        {
            var res = new[] { 40, 42, 44 }.ToAsyncEnumerable().LastAwaitAsync(x => new ValueTask<bool>(x % 2 != 0));
            await AssertThrowsAsync<InvalidOperationException>(res.AsTask());
        }

        [Fact]
        public async Task LastAwaitAsync_Predicate_Many_None()
        {
            var res = new[] { 40, 42, 44 }.Select(x => x).ToAsyncEnumerable().LastAwaitAsync(x => new ValueTask<bool>(x % 2 != 0));
            await AssertThrowsAsync<InvalidOperationException>(res.AsTask());
        }

        [Fact]
        public async Task LastAwaitAsync_Predicate_Single_Pass()
        {
            var res = Return42.LastAwaitAsync(x => new ValueTask<bool>(x % 2 == 0));
            Assert.Equal(42, await res);
        }

        [Fact]
        public async Task LastAwaitAsync_Predicate_Many_IList_Pass1()
        {
            var res = new[] { 43, 44, 45 }.ToAsyncEnumerable().LastAwaitAsync(x => new ValueTask<bool>(x % 2 != 0));
            Assert.Equal(45, await res);
        }

        [Fact]
        public async Task LastAwaitAsync_Predicate_Many_IList_Pass2()
        {
            var res = new[] { 42, 45, 90 }.ToAsyncEnumerable().LastAwaitAsync(x => new ValueTask<bool>(x % 2 != 0));
            Assert.Equal(45, await res);
        }

        [Fact]
        public async Task LastAwaitAsync_Predicate_Many_Pass1()
        {
            var res = new[] { 43, 44, 45 }.Select(x => x).ToAsyncEnumerable().LastAwaitAsync(x => new ValueTask<bool>(x % 2 != 0));
            Assert.Equal(45, await res);
        }

        [Fact]
        public async Task LastAwaitAsync_Predicate_Many_Pass2()
        {
            var res = new[] { 42, 45, 90 }.Select(x => x).ToAsyncEnumerable().LastAwaitAsync(x => new ValueTask<bool>(x % 2 != 0));
            Assert.Equal(45, await res);
        }

        [Fact]
        public async Task LastAwaitAsync_Predicate_AsyncPredicateThrows()
        {
            var res = new[] { 0, 1, 2 }.ToAsyncEnumerable().LastAwaitAsync(x => new ValueTask<bool>(1 / x > 0));
            await AssertThrowsAsync<DivideByZeroException>(res.AsTask());
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task LastAwaitWithCancellationAsync_Predicate_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.LastAwaitWithCancellationAsync<int>(default, (x, ct) => new ValueTask<bool>(true), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.LastAwaitWithCancellationAsync(Return42, default, CancellationToken.None).AsTask());
        }

        [Fact]
        public async Task LastAwaitWithCancellationAsync_Predicate_Empty()
        {
            var res = AsyncEnumerable.Empty<int>().LastAwaitWithCancellationAsync((x, ct) => new ValueTask<bool>(true), CancellationToken.None);
            await AssertThrowsAsync<InvalidOperationException>(res.AsTask());
        }

        [Fact]
        public async Task LastAwaitWithCancellationAsync_Predicate_Throw()
        {
            var ex = new Exception("Bang!");
            var res = Throw<int>(ex).LastAwaitWithCancellationAsync((x, ct) => new ValueTask<bool>(true), CancellationToken.None);
            await AssertThrowsAsync(res, ex);
        }

        [Fact]
        public async Task LastAwaitWithCancellationAsync_Predicate_Single_None()
        {
            var res = Return42.LastAwaitWithCancellationAsync((x, ct) => new ValueTask<bool>(x % 2 != 0), CancellationToken.None);
            await AssertThrowsAsync<InvalidOperationException>(res.AsTask());
        }

        [Fact]
        public async Task LastAwaitWithCancellationAsync_Predicate_Many_IList_None()
        {
            var res = new[] { 40, 42, 44 }.ToAsyncEnumerable().LastAwaitWithCancellationAsync((x, ct) => new ValueTask<bool>(x % 2 != 0), CancellationToken.None);
            await AssertThrowsAsync<InvalidOperationException>(res.AsTask());
        }

        [Fact]
        public async Task LastAwaitWithCancellationAsync_Predicate_Many_None()
        {
            var res = new[] { 40, 42, 44 }.Select((x, ct) => x).ToAsyncEnumerable().LastAwaitWithCancellationAsync((x, ct) => new ValueTask<bool>(x % 2 != 0), CancellationToken.None);
            await AssertThrowsAsync<InvalidOperationException>(res.AsTask());
        }

        [Fact]
        public async Task LastAwaitWithCancellationAsync_Predicate_Single_Pass()
        {
            var res = Return42.LastAwaitWithCancellationAsync((x, ct) => new ValueTask<bool>(x % 2 == 0), CancellationToken.None);
            Assert.Equal(42, await res);
        }

        [Fact]
        public async Task LastAwaitWithCancellationAsync_Predicate_Many_IList_Pass1()
        {
            var res = new[] { 43, 44, 45 }.ToAsyncEnumerable().LastAwaitWithCancellationAsync((x, ct) => new ValueTask<bool>(x % 2 != 0), CancellationToken.None);
            Assert.Equal(45, await res);
        }

        [Fact]
        public async Task LastAwaitWithCancellationAsync_Predicate_Many_IList_Pass2()
        {
            var res = new[] { 42, 45, 90 }.ToAsyncEnumerable().LastAwaitWithCancellationAsync((x, ct) => new ValueTask<bool>(x % 2 != 0), CancellationToken.None);
            Assert.Equal(45, await res);
        }

        [Fact]
        public async Task LastAwaitWithCancellationAsync_Predicate_Many_Pass1()
        {
            var res = new[] { 43, 44, 45 }.Select((x, ct) => x).ToAsyncEnumerable().LastAwaitWithCancellationAsync((x, ct) => new ValueTask<bool>(x % 2 != 0), CancellationToken.None);
            Assert.Equal(45, await res);
        }

        [Fact]
        public async Task LastAwaitWithCancellationAsync_Predicate_Many_Pass2()
        {
            var res = new[] { 42, 45, 90 }.Select((x, ct) => x).ToAsyncEnumerable().LastAwaitWithCancellationAsync((x, ct) => new ValueTask<bool>(x % 2 != 0), CancellationToken.None);
            Assert.Equal(45, await res);
        }

        [Fact]
        public async Task LastAwaitWithCancellationAsync_Predicate_AsyncPredicateWithCancellationThrows()
        {
            var res = new[] { 0, 1, 2 }.ToAsyncEnumerable().LastAwaitWithCancellationAsync((x, ct) => new ValueTask<bool>(1 / x > 0), CancellationToken.None);
            await AssertThrowsAsync<DivideByZeroException>(res.AsTask());
        }
#endif
    }
}

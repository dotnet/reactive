// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class DefaultIfEmpty : AsyncEnumerableTests
    {
        [Fact]
        public void DefaultIfEmpty_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.DefaultIfEmpty<int>(default));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.DefaultIfEmpty(default, 42));
        }

        [Fact]
        public async Task DefaultIfEmpty_Empty()
        {
            var xs = AsyncEnumerable.Empty<int>().DefaultIfEmpty();

            var e = xs.GetAsyncEnumerator();
            await HasNextAsync(e, 0);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task DefaultIfEmpty_Value_Empty()
        {
            var xs = AsyncEnumerable.Empty<int>().DefaultIfEmpty(42);

            var e = xs.GetAsyncEnumerator();
            await HasNextAsync(e, 42);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task DefaultIfEmpty_Single()
        {
            var xs = Return42.DefaultIfEmpty();

            var e = xs.GetAsyncEnumerator();
            await HasNextAsync(e, 42);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task DefaultIfEmpty_Value_Single()
        {
            var xs = Return42.DefaultIfEmpty(24);

            var e = xs.GetAsyncEnumerator();
            await HasNextAsync(e, 42);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task DefaultIfEmpty_Many()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable().Select(x => x).DefaultIfEmpty();

            var e = xs.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 4);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task DefaultIfEmpty_Many_IList()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable().DefaultIfEmpty();

            var e = xs.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 4);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task DefaultIfEmpty_Value_IList()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable().Select(x => x).DefaultIfEmpty(24);

            var e = xs.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 4);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task DefaultIfEmpty_Value_Many_IList()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable().DefaultIfEmpty(24);

            var e = xs.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 4);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task DefaultIfEmpty_Throws_Source()
        {
            var ex = new Exception("Bang!");
            var xs = Throw<int>(ex).DefaultIfEmpty();

            var e = xs.GetAsyncEnumerator();
            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        [Fact]
        public async Task DefaultIfEmpty_Value_Throws_Source()
        {
            var ex = new Exception("Bang!");
            var xs = Throw<int>(ex).DefaultIfEmpty(24);

            var e = xs.GetAsyncEnumerator();
            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        [Fact]
        public async Task DefaultIfEmpty_IAsyncIListProvider_ToArray_Empty()
        {
            var xs = AsyncEnumerable.Empty<int>().DefaultIfEmpty(42);

            var res = new[] { 42 };

            Assert.True(res.SequenceEqual(await xs.ToArrayAsync()));
        }

        [Fact]
        public async Task DefaultIfEmpty_Value_IAsyncIListProvider_ToArray_Empty()
        {
            var xs = AsyncEnumerable.Empty<int>().DefaultIfEmpty(42);

            var res = new List<int> { 42 };

            Assert.True(res.SequenceEqual(await xs.ToListAsync()));
        }

        [Fact]
        public async Task DefaultIfEmpty_IAsyncIListProvider_ToList()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable().DefaultIfEmpty();

            var res = new[] { 1, 2, 3, 4 };

            Assert.True(res.SequenceEqual(await xs.ToArrayAsync()));
        }

        [Fact]
        public async Task DefaultIfEmpty_Value_IAsyncIListProvider_ToList()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable().DefaultIfEmpty(24);

            var res = new List<int> { 1, 2, 3, 4 };

            Assert.True(res.SequenceEqual(await xs.ToListAsync()));
        }

        [Fact]
        public async Task DefaultIfEmpty_IAsyncIListProvider_Count_Empty()
        {
            var xs = AsyncEnumerable.Empty<int>().DefaultIfEmpty(42);

            Assert.Equal(1, await xs.CountAsync());
        }

        [Fact]
        public async Task DefaultIfEmpty_Value_IAsyncIListProvider_Count()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable().Where(x => x > 0).DefaultIfEmpty(24);

            Assert.Equal(4, await xs.CountAsync());
        }

        [Fact]
        public async Task DefaultIfEmpty_Value_IAsyncIListProvider_Count_IAsyncIListProvider()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable().Select(x => x).DefaultIfEmpty(24);

            Assert.Equal(4, await xs.CountAsync());
        }

        [Fact]
        public async Task DefaultIfEmpty_Value_IAsyncIListProvider_Count_IList()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable().DefaultIfEmpty(24);

            Assert.Equal(4, await xs.CountAsync());
        }

        [Fact]
        public async Task DefaultIfEmpty_SequenceIdentity()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable().DefaultIfEmpty(24);

            await SequenceIdentity(xs);
        }
    }
}

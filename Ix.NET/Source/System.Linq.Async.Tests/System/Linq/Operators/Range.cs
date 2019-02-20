// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class Range : AsyncEnumerableTests
    {
        [Fact]
        public void Range_Null()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => AsyncEnumerable.Range(0, -1));
            Assert.Throws<ArgumentOutOfRangeException>(() => AsyncEnumerable.Range(1024, int.MaxValue - 1022));
        }

        [Fact]
        public async Task Range_Simple()
        {
            var xs = AsyncEnumerable.Range(2, 5);

            var e = xs.GetAsyncEnumerator();
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 4);
            await HasNextAsync(e, 5);
            await HasNextAsync(e, 6);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Range_Simple_IAsyncPartition()
        {
            var xs = AsyncEnumerable.Range(2, 5);

            Assert.Equal(5, await xs.CountAsync());

            Assert.Equal(2, await xs.FirstAsync());
            Assert.Equal(6, await xs.LastAsync());

            Assert.Equal(2, await xs.ElementAtAsync(0));
            Assert.Equal(3, await xs.ElementAtAsync(1));
            Assert.Equal(4, await xs.ElementAtAsync(2));
            Assert.Equal(5, await xs.ElementAtAsync(3));
            Assert.Equal(6, await xs.ElementAtAsync(4));
            await AssertThrowsAsync<ArgumentOutOfRangeException>(xs.ElementAtAsync(5).AsTask());

            Assert.Equal(2, await xs.Skip(0).FirstAsync());
            Assert.Equal(6, await xs.Skip(0).LastAsync());
            Assert.Equal(3, await xs.Skip(1).FirstAsync());
            Assert.Equal(6, await xs.Skip(1).LastAsync());
            Assert.Equal(0, await xs.Skip(5).CountAsync());
            Assert.Equal(0, await xs.Skip(1024).CountAsync());

            Assert.Equal(2, await xs.Take(4).FirstAsync());
            Assert.Equal(5, await xs.Take(4).LastAsync());
            Assert.Equal(2, await xs.Take(5).FirstAsync());
            Assert.Equal(6, await xs.Take(5).LastAsync());
            Assert.Equal(2, await xs.Take(1024).FirstAsync());
            Assert.Equal(6, await xs.Take(1024).LastAsync());

            Assert.Equal(new[] { 2, 3, 4, 5, 6 }, await xs.ToArrayAsync());
            Assert.Equal(new[] { 2, 3, 4, 5, 6 }, await xs.ToListAsync());
        }

        [Fact]
        public async Task Range_Empty()
        {
            var xs = AsyncEnumerable.Range(2, 0);

            var e = xs.GetAsyncEnumerator();
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Range_Empty_IAsyncPartition()
        {
            var xs = AsyncEnumerable.Range(2, 0);

            Assert.Equal(0, await xs.CountAsync());

            await AssertThrowsAsync<InvalidOperationException>(xs.FirstAsync().AsTask());
            await AssertThrowsAsync<InvalidOperationException>(xs.LastAsync().AsTask());
            await AssertThrowsAsync<ArgumentOutOfRangeException>(xs.ElementAtAsync(0).AsTask());

            Assert.Empty(await xs.ToArrayAsync());
            Assert.Empty(await xs.ToListAsync());
        }
    }
}

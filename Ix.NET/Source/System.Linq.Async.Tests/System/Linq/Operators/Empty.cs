// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class Empty : AsyncEnumerableTests
    {
        [Fact]
        public async Task Empty_Basics()
        {
            var xs = AsyncEnumerable.Empty<int>();

            await NoNextAsync(xs.GetAsyncEnumerator());

            Assert.Equal(0, xs.GetAsyncEnumerator().Current);
        }

        [Fact]
        public async Task Empty_IAsyncPartition()
        {
            var xs = AsyncEnumerable.Empty<int>();

            Assert.Equal(0, await xs.CountAsync());
            Assert.Equal(0, await xs.Skip(1).CountAsync());
            Assert.Equal(0, await xs.Take(1).CountAsync());

            await AssertThrowsAsync<InvalidOperationException>(xs.FirstAsync().AsTask());
            await AssertThrowsAsync<InvalidOperationException>(xs.LastAsync().AsTask());
            await AssertThrowsAsync<InvalidOperationException>(xs.SingleAsync().AsTask());

            Assert.Empty(await xs.ToArrayAsync());
            Assert.Empty(await xs.ToListAsync());
        }
    }
}

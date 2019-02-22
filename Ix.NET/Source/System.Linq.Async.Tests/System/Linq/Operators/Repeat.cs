// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class Repeat : AsyncEnumerableTests
    {
        [Fact]
        public void Repeat_Null()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => AsyncEnumerable.Repeat(0, -1));
        }

        [Fact]
        public async Task Repeat_Many()
        {
            var xs = AsyncEnumerable.Repeat(2, 5);

            var e = xs.GetAsyncEnumerator();
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 2);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Repeat_Zero()
        {
            var xs = AsyncEnumerable.Repeat(2, 0);

            var e = xs.GetAsyncEnumerator();
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Repeat_Count()
        {
            var xs = AsyncEnumerable.Repeat(2, 5);

            Assert.Equal(5, await xs.CountAsync());
        }

        [Fact]
        public async Task Repeat_ToArray()
        {
            var xs = AsyncEnumerable.Repeat(2, 5);

            Assert.Equal(Enumerable.Repeat(2, 5), await xs.ToArrayAsync());
        }

        [Fact]
        public async Task Repeat_ToList()
        {
            var xs = AsyncEnumerable.Repeat(2, 5);

            Assert.Equal(Enumerable.Repeat(2, 5), await xs.ToListAsync());
        }
    }
}

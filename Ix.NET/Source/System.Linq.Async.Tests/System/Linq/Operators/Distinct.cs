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
    public class Distinct : AsyncEnumerableTests
    {
        [Fact]
        public void Distinct_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.Distinct<int>(default));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.Distinct(default, EqualityComparer<int>.Default));
        }

        [Fact]
        public async Task Distinct_Simple()
        {
            var xs = new[] { 1, 2, 1, 3, 5, 2, 1, 4 }.ToAsyncEnumerable().Distinct();

            var e = xs.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 5);
            await HasNextAsync(e, 4);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Distinct_Comparer()
        {
            var xs = new[] { 1, -2, -1, 3, 5, 2, 1, 4 }.ToAsyncEnumerable().Distinct(new Eq());

            var e = xs.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, -2);
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 5);
            await HasNextAsync(e, 4);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Distinct_ToArray()
        {
            var xs = new[] { 1, 2, 1, 3, 5, 2, 1, 4 }.ToAsyncEnumerable().Distinct();

            var res = new[] { 1, 2, 3, 5, 4 };
            Assert.True(res.SequenceEqual(await xs.ToArrayAsync()));
        }

        [Fact]
        public async Task Distinct_ToList()
        {
            var xs = new[] { 1, 2, 1, 3, 5, 2, 1, 4 }.ToAsyncEnumerable().Distinct();

            var res = new List<int> { 1, 2, 3, 5, 4 };
            Assert.True(res.SequenceEqual(await xs.ToListAsync()));
        }

        [Fact]
        public async Task Distinct_Count()
        {
            var xs = new[] { 1, 2, 1, 3, 5, 2, 1, 4 }.ToAsyncEnumerable().Distinct();

            Assert.Equal(5, await xs.CountAsync());
        }

        [Fact]
        public async Task Distinct_SequenceIdentity()
        {
            var xs = new[] { 1, 2, 1, 3, 5, 2, 1, 4 }.ToAsyncEnumerable().Distinct();

            await SequenceIdentity(xs);
        }

        [Fact]
        public async Task Distinct_Empty()
        {
            var xs = AsyncEnumerable.Empty<int>().Distinct();

            var e = xs.GetAsyncEnumerator();

            await NoNextAsync(e);
        }

        private sealed class Eq : IEqualityComparer<int>
        {
            public bool Equals(int x, int y)
            {
                return EqualityComparer<int>.Default.Equals(Math.Abs(x), Math.Abs(y));
            }

            public int GetHashCode(int obj)
            {
                return EqualityComparer<int>.Default.GetHashCode(Math.Abs(obj));
            }
        }
    }
}

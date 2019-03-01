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
    public class Distinct : AsyncEnumerableExTests
    {
        [Fact]
        public void Distinct_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerableEx.Distinct<int, int>(Return42, default(Func<int, int>)));

            Assert.Throws<ArgumentNullException>(() => AsyncEnumerableEx.Distinct(default(IAsyncEnumerable<int>), x => x));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerableEx.Distinct(Return42, default(Func<int, int>)));

            Assert.Throws<ArgumentNullException>(() => AsyncEnumerableEx.Distinct(default(IAsyncEnumerable<int>), x => x, EqualityComparer<int>.Default));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerableEx.Distinct(Return42, default(Func<int, int>), EqualityComparer<int>.Default));
        }

        [Fact]
        public async Task Distinct1Async()
        {
            var xs = new[] { 1, 2, 3, 4, 5 }.ToAsyncEnumerable().Distinct(x => x / 2);

            var e = xs.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 4);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Distinct6()
        {
            var xs = new[] { 1, 2, 1, 3, 5, 2, 1, 4 }.ToAsyncEnumerable().Distinct(k => k);

            var res = new[] { 1, 2, 3, 5, 4 };
            Assert.True(res.SequenceEqual(await xs.ToArrayAsync()));
        }

        [Fact]
        public async Task Distinct7()
        {
            var xs = new[] { 1, 2, 1, 3, 5, 2, 1, 4 }.ToAsyncEnumerable().Distinct(k => k);

            var res = new List<int> { 1, 2, 3, 5, 4 };
            Assert.True(res.SequenceEqual(await xs.ToListAsync()));
        }

        [Fact]
        public async Task Distinct8()
        {
            var xs = new[] { 1, 2, 1, 3, 5, 2, 1, 4 }.ToAsyncEnumerable().Distinct(k => k);

            Assert.Equal(5, await xs.CountAsync());
        }

        [Fact]
        public async Task Distinct9()
        {
            var xs = new[] { 1, 2, 1, 3, 5, 2, 1, 4 }.ToAsyncEnumerable().Distinct(k => k);

            await SequenceIdentity(xs);
        }

        [Fact]
        public async Task Distinct11Async()
        {
            var xs = AsyncEnumerable.Empty<int>().Distinct(k => k);

            var e = xs.GetAsyncEnumerator();

            await NoNextAsync(e);
        }
    }
}

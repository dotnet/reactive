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
    public class Union : AsyncEnumerableTests
    {
        [Fact]
        public void Union_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.Union(default, Return42));

            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.Union(default, Return42, new Eq()));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.Union(Return42, default, new Eq()));
        }

        [Fact]
        public async Task Union_Simple()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = new[] { 3, 5, 1, 4 }.ToAsyncEnumerable();
            var res = xs.Union(ys);

            var e = res.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 5);
            await HasNextAsync(e, 4);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Union_EqualityComparer()
        {
            var xs = new[] { 1, 2, -3 }.ToAsyncEnumerable();
            var ys = new[] { 3, 5, -1, 4 }.ToAsyncEnumerable();
            var res = xs.Union(ys, new Eq());

            var e = res.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, -3);
            await HasNextAsync(e, 5);
            await HasNextAsync(e, 4);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Union_UnionUnion()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = new[] { 3, 5, 1, 4 }.ToAsyncEnumerable();
            var zs = new[] { 5, 7, 8, 1 }.ToAsyncEnumerable();
            var res = xs.Union(ys).Union(zs);

            var e = res.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 5);
            await HasNextAsync(e, 4);
            await HasNextAsync(e, 7);
            await HasNextAsync(e, 8);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Union_UnionUnionUnion()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = new[] { 3, 5, 1, 4 }.ToAsyncEnumerable();
            var zs = new[] { 5, 7, 8, 1 }.ToAsyncEnumerable();
            var us = new[] { 2, 4, 6, 8 }.ToAsyncEnumerable();
            var res = xs.Union(ys).Union(zs).Union(us);

            var e = res.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 5);
            await HasNextAsync(e, 4);
            await HasNextAsync(e, 7);
            await HasNextAsync(e, 8);
            await HasNextAsync(e, 6);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Union_UnionOfEmpty2()
        {
            var res = AsyncEnumerable.Empty<int>().Union(AsyncEnumerable.Empty<int>());

            var e = res.GetAsyncEnumerator();
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Union_UnionOfEmpty3()
        {
            var res = AsyncEnumerable.Empty<int>().Union(AsyncEnumerable.Empty<int>()).Union(AsyncEnumerable.Empty<int>());

            var e = res.GetAsyncEnumerator();
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Union_ManyUnionsWithEmpty()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = new[] { 3, 5, 1, 4 }.ToAsyncEnumerable();
            var zs = new[] { 5, 7, 8, 1 }.ToAsyncEnumerable();
            var us = new[] { 2, 4, 6, 8 }.ToAsyncEnumerable();
            var res = AsyncEnumerable.Empty<int>().Union(AsyncEnumerable.Empty<int>()).Union(xs).Union(ys).Union(zs).Union(us);

            var e = res.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 5);
            await HasNextAsync(e, 4);
            await HasNextAsync(e, 7);
            await HasNextAsync(e, 8);
            await HasNextAsync(e, 6);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Union_Count()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = new[] { 3, 5, 1, 4 }.ToAsyncEnumerable();
            var res = xs.Union(ys);

            Assert.Equal(5, await res.CountAsync());
        }

        [Fact]
        public async Task Union_ToArray()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = new[] { 3, 5, 1, 4 }.ToAsyncEnumerable();
            var res = xs.Union(ys);

            Assert.Equal(new[] { 1, 2, 3, 4, 5 }, (await res.ToArrayAsync()).OrderBy(x => x));
        }

        [Fact]
        public async Task Union_ToList()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = new[] { 3, 5, 1, 4 }.ToAsyncEnumerable();
            var res = xs.Union(ys);

            Assert.Equal(new[] { 1, 2, 3, 4, 5 }, (await res.ToListAsync()).OrderBy(x => x));
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

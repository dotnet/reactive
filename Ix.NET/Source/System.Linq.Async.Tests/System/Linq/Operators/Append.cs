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
    public class Append : AsyncEnumerableTests
    {
        [Fact]
        public void Append_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.Append(default, 42));
        }

        [Fact]
        public async Task Append_Simple()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();

            var res = xs.Append(4);

            var e = res.GetAsyncEnumerator();

            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 4);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Append_IAsyncIListProvider_ICollection_ToArray()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();

            var res = xs.Append(4);

            var a = new[] { 1, 2, 3, 4 };

            var arr = await res.ToArrayAsync();
            Assert.Equal(a, arr);
        }

        [Fact]
        public async Task Append_IAsyncIListProvider_ICollection_ToList()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();

            var res = xs.Append(4);

            var a = new List<int> { 1, 2, 3, 4 };

            var arr = await res.ToListAsync();
            Assert.Equal(a, arr);
        }

        [Fact]
        public async Task Append_IAsyncIListProvider_ICollection_Count()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();

            var res = xs.Append(4);

            Assert.Equal(4, await res.CountAsync());
        }

        [Fact]
        public async Task Append_IAsyncIListProvider_ToArray()
        {
            var xs = new[] { 1, 2, 3 }.Where(x => true).ToAsyncEnumerable();

            var res = xs.Append(4);

            var a = new[] { 1, 2, 3, 4 };

            var arr = await res.ToArrayAsync();
            Assert.Equal(a, arr);
        }

        [Fact]
        public async Task Append_IAsyncIListProvider_ToList()
        {
            var xs = new[] { 1, 2, 3 }.Where(x => true).ToAsyncEnumerable();

            var res = xs.Append(4);

            var a = new List<int> { 1, 2, 3, 4 };

            var arr = await res.ToListAsync();
            Assert.Equal(a, arr);
        }

        [Fact]
        public async Task Append_IAsyncIListProvider_Count()
        {
            var xs = new[] { 1, 2, 3 }.Where(x => true).ToAsyncEnumerable();

            var res = xs.Append(4);

            Assert.Equal(4, await res.CountAsync());
        }

        [Fact]
        public async Task Append_ToArray()
        {
            var xs = AsyncEnumerable.Range(1, 3).Where(i => true);

            var res = xs.Append(4);

            var a = new[] { 1, 2, 3, 4 };

            var arr = await res.ToArrayAsync();
            Assert.Equal(a, arr);
        }

        [Fact]
        public async Task Append_ToList()
        {
            var xs = AsyncEnumerable.Range(1, 3).Where(i => true);

            var res = xs.Append(4);

            var a = new List<int> { 1, 2, 3, 4 };

            var arr = await res.ToListAsync();
            Assert.Equal(a, arr);
        }

        [Fact]
        public async Task Append_Count()
        {
            var xs = AsyncEnumerable.Range(1, 3).Where(i => true);

            var res = xs.Append(4);

            Assert.Equal(4, await res.CountAsync());
        }

        [Fact]
        public async Task Append_SequenceIdentity()
        {
            var xs = AsyncEnumerable.Range(1, 3).Where(i => true);

            var res = xs.Append(4);

            await SequenceIdentity(res);
        }

        [Fact]
        public async Task Append_Many_Simple()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();

            var res = xs.Append(4)
                        .Append(5)
                        .Append(6);

            var e = res.GetAsyncEnumerator();

            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 4);
            await HasNextAsync(e, 5);
            await HasNextAsync(e, 6);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Append_Many_IAsyncIListProvider_ToArray()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();

            var res = xs.Append(4)
                        .Append(5)
                        .Append(6);

            var a = new[] { 1, 2, 3, 4, 5, 6 };

            var arr = await res.ToArrayAsync();
            Assert.Equal(a, arr);
        }

        [Fact]
        public async Task Append_Many_IAsyncIListProvider_ToList()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();

            var res = xs.Append(4)
                        .Append(5)
                        .Append(6);

            var a = new List<int> { 1, 2, 3, 4, 5, 6 };

            var arr = await res.ToListAsync();
            Assert.Equal(a, arr);
        }

        [Fact]
        public async Task Append_Many_IAsyncIListProvider_Count()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();

            var res = xs.Append(4)
                        .Append(5)
                        .Append(6);

            Assert.Equal(6, await res.CountAsync());
        }

        [Fact]
        public async Task Append_Many_ToArray()
        {
            var xs = AsyncEnumerable.Range(1, 3).Where(i => true);

            var res = xs.Append(4)
                        .Append(5)
                        .Append(6);

            var a = new[] { 1, 2, 3, 4, 5, 6 };

            var arr = await res.ToArrayAsync();
            Assert.Equal(a, arr);
        }

        [Fact]
        public async Task Append_Many_ToList()
        {
            var xs = AsyncEnumerable.Range(1, 3).Where(i => true);

            var res = xs.Append(4)
                        .Append(5)
                        .Append(6);

            var a = new List<int> { 1, 2, 3, 4, 5, 6 };

            var arr = await res.ToListAsync();
            Assert.Equal(a, arr);
        }

        [Fact]
        public async Task Append_Many_Count()
        {
            var xs = AsyncEnumerable.Range(1, 3).Where(i => true);

            var res = xs.Append(4)
                        .Append(5)
                        .Append(6);

            Assert.Equal(6, await res.CountAsync());
        }
    }
}

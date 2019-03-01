// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class Optimizations : AsyncEnumerableTests
    {
        [Fact]
        public async Task SelectWhere2Async()
        {
            var xs = new[] { 0, 1, 2 }.ToAsyncEnumerable();
            var ys = xs.Select(i => i + 2).Where(i => i % 2 == 0);

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 4);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task WhereSelect2Async()
        {
            var xs = new[] { 0, 1, 2 }.ToAsyncEnumerable();
            var ys = xs.Where(i => i % 2 == 0).Select(i => i + 2);

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 4);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task WhereSelect3Async()
        {
            var xs = new[] { 0, 1, 2 }.ToAsyncEnumerable();
            var ys = xs.Where(i => i % 2 == 0).Select(i => i + 2).Select(i => i + 2);

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 4);
            await HasNextAsync(e, 6);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task AppendPrepend1Async()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();

            var res = xs.Prepend(4)
                        .Append(5)
                        .Append(6)
                        .Prepend(7)
                        .Append(8)
                        .Prepend(9)
                        .Prepend(10);

            var e = res.GetAsyncEnumerator();

            await HasNextAsync(e, 10);
            await HasNextAsync(e, 9);
            await HasNextAsync(e, 7);
            await HasNextAsync(e, 4);
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 5);
            await HasNextAsync(e, 6);
            await HasNextAsync(e, 8);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task AppendPrepend2()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();

            var res = xs.Prepend(4)
                        .Append(5)
                        .Append(6)
                        .Prepend(7)
                        .Append(8)
                        .Prepend(9)
                        .Prepend(10);

            var a = new[] { 10, 9, 7, 4, 1, 2, 3, 5, 6, 8 };

            var arr = await res.ToArrayAsync();
            Assert.Equal(a, arr);
        }

        [Fact]
        public async Task AppendPrepend3()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var res = xs.Prepend(4)
                        .Append(5)
                        .Append(6)
                        .Prepend(7)
                        .Append(8)
                        .Prepend(9)
                        .Prepend(10);

            var a = new List<int> { 10, 9, 7, 4, 1, 2, 3, 5, 6, 8 };

            var arr = await res.ToListAsync();
            Assert.Equal(a, arr);
        }

        [Fact]
        public async Task AppendPrepend4()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();

            var res = xs.Prepend(4)
                        .Append(5)
                        .Append(6)
                        .Prepend(7)
                        .Append(8)
                        .Prepend(9)
                        .Prepend(10);

            Assert.Equal(10, await res.CountAsync());
        }

        [Fact]
        public async Task AppendPrepend5()
        {
            var xs = AsyncEnumerable.Range(1, 3).Where(i => true);

            var res = xs.Prepend(4)
                        .Append(5)
                        .Append(6)
                        .Prepend(7)
                        .Append(8)
                        .Prepend(9)
                        .Prepend(10);

            var a = new[] { 10, 9, 7, 4, 1, 2, 3, 5, 6, 8 };

            var arr = await res.ToArrayAsync();
            Assert.Equal(a, arr);
        }

        [Fact]
        public async Task AppendPrepend6()
        {
            var xs = AsyncEnumerable.Range(1, 3).Where(i => true);

            var res = xs.Prepend(4)
                        .Append(5)
                        .Append(6)
                        .Prepend(7)
                        .Append(8)
                        .Prepend(9)
                        .Prepend(10);

            var a = new List<int> { 10, 9, 7, 4, 1, 2, 3, 5, 6, 8 };

            var arr = await res.ToListAsync();
            Assert.Equal(a, arr);
        }

        [Fact]
        public async Task AppendPrepend7()
        {
            var xs = AsyncEnumerable.Range(1, 3).Where(i => true);

            var res = xs.Prepend(4)
                        .Append(5)
                        .Append(6)
                        .Prepend(7)
                        .Append(8)
                        .Prepend(9)
                        .Prepend(10);

            Assert.Equal(10, await res.CountAsync());
        }

        [Fact]
        public async Task AppendPrepend8Async()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();

            var res = xs.Append(4)
                        .Prepend(5);

            var e = res.GetAsyncEnumerator();

            await HasNextAsync(e, 5);
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 4);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task AppendPrepend9()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();

            var res = xs.Append(4)
                        .Prepend(5);

            await SequenceIdentity(res);
        }
    }
}

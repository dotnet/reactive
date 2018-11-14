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
    public class Prepend : AsyncEnumerableTests
    {
        [Fact]
        public void Prepend_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.Prepend(default, 42));
        }

        [Fact]
        public async Task Prepend1Async()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();

            var res = xs.Prepend(4);

            var e = res.GetAsyncEnumerator();

            await HasNextAsync(e, 4);
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Prepend2()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();

            var res = xs.Prepend(4);

            var a = new[] { 4, 1, 2, 3 };

            var arr = await res.ToArrayAsync();
            Assert.Equal(a, arr);
        }

        [Fact]
        public async Task Prepend3()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();

            var res = xs.Prepend(4);

            var a = new List<int> { 4, 1, 2, 3 };

            var arr = await res.ToListAsync();
            Assert.Equal(a, arr);
        }

        [Fact]
        public async Task Prepend4()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();

            var res = xs.Prepend(4);
            Assert.Equal(4, await res.CountAsync());
        }

        [Fact]
        public async Task Prepend5()
        {
            var xs = AsyncEnumerable.Range(1, 3).Where(i => true);

            var res = xs.Prepend(4);

            var a = new[] { 4, 1, 2, 3 };

            var arr = await res.ToArrayAsync();
            Assert.Equal(a, arr);
        }

        [Fact]
        public async Task Prepend6()
        {
            var xs = AsyncEnumerable.Range(1, 3).Where(i => true);

            var res = xs.Prepend(4);

            var a = new List<int> { 4, 1, 2, 3 };

            var arr = await res.ToListAsync();
            Assert.Equal(a, arr);
        }

        [Fact]
        public async Task Prepend7()
        {
            var xs = AsyncEnumerable.Range(1, 3).Where(i => true);

            var res = xs.Prepend(4);
            Assert.Equal(4, await res.CountAsync());
        }

        [Fact]
        public async Task Prepend8()
        {
            var xs = AsyncEnumerable.Range(1, 3).Where(i => true);

            var res = xs.Prepend(4);

            await SequenceIdentity(res);
        }

        [Fact]
        public async Task PrependN1Async()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();

            var res = xs.Prepend(4)
                        .Prepend(5)
                        .Prepend(6);

            var e = res.GetAsyncEnumerator();

            await HasNextAsync(e, 6);
            await HasNextAsync(e, 5);
            await HasNextAsync(e, 4);
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task PrependN2()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();

            var res = xs.Prepend(4)
                        .Prepend(5)
                        .Prepend(6);

            var a = new[] { 6, 5, 4, 1, 2, 3 };

            var arr = await res.ToArrayAsync();
            Assert.Equal(a, arr);
        }

        [Fact]
        public async Task PrependN3()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();

            var res = xs.Prepend(4)
                        .Prepend(5)
                        .Prepend(6);

            var a = new List<int> { 6, 5, 4, 1, 2, 3 };

            var arr = await res.ToListAsync();
            Assert.Equal(a, arr);
        }

        [Fact]
        public async Task PrependN4()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();

            var res = xs.Prepend(4)
                        .Prepend(5)
                        .Prepend(6);

            Assert.Equal(6, await res.CountAsync());
        }

        [Fact]
        public async Task PrependN5()
        {
            var xs = AsyncEnumerable.Range(1, 3).Where(i => true);

            var res = xs.Prepend(4)
                        .Prepend(5)
                        .Prepend(6);

            var a = new[] { 6, 5, 4, 1, 2, 3 };

            var arr = await res.ToArrayAsync();
            Assert.Equal(a, arr);
        }

        [Fact]
        public async Task PrependN6()
        {
            var xs = AsyncEnumerable.Range(1, 3).Where(i => true);

            var res = xs.Prepend(4)
                        .Prepend(5)
                        .Prepend(6);

            var a = new List<int> { 6, 5, 4, 1, 2, 3 };

            var arr = await res.ToListAsync();
            Assert.Equal(a, arr);
        }

        [Fact]
        public async Task PrependN7()
        {
            var xs = AsyncEnumerable.Range(1, 3).Where(i => true);

            var res = xs.Prepend(4)
                        .Prepend(5)
                        .Prepend(6);

            Assert.Equal(6, await res.CountAsync());
        }
    }
}

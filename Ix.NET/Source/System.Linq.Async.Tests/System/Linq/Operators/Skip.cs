// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class Skip : AsyncEnumerableTests
    {
        [Fact]
        public void Skip_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.Skip<int>(default, 5));
        }

        [Fact]
        public async Task Skip_Simple_SkipSome()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable().Where(x => true);
            var ys = xs.Skip(2);

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 4);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Skip_Simple_SkipSome_IList()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.Skip(2);

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 4);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Skip_Simple_SkipAll()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable().Where(x => true);
            var ys = xs.Skip(10);

            var e = ys.GetAsyncEnumerator();
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Skip_Simple_SkipAll_IList()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.Skip(10);

            var e = ys.GetAsyncEnumerator();
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Skip_Simple_SkipNone()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable().Where(x => true);
            var ys = xs.Skip(0);

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 4);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Skip_Simple_SkipNone_IList()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.Skip(0);

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 4);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Skip_Throws_Source()
        {
            var ex = new Exception("Bang");
            var xs = Throw<int>(ex);
            var ys = xs.Skip(2);

            var e = ys.GetAsyncEnumerator();
            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        [Fact]
        public async Task Skip_SequenceIdentity()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.Skip(2);

            await SequenceIdentity(ys);
        }

        [Fact]
        public async Task Skip_IAsyncPartition_NonEmpty_Skip()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable().Where(x => true);
            var ys = xs.Skip(2);

            Assert.Equal(2, await ys.CountAsync());

            Assert.Equal(3, await ys.FirstAsync());
            Assert.Equal(4, await ys.LastAsync());

            Assert.Equal(3, await ys.ElementAtAsync(0));
            Assert.Equal(4, await ys.ElementAtAsync(1));

            Assert.Equal(new[] { 3, 4 }, await ys.ToArrayAsync());
            Assert.Equal(new[] { 3, 4 }, await ys.ToListAsync());
        }

        [Fact]
        public async Task Skip_IAsyncPartition_NonEmpty_SkipSkip()
        {
            var xs = new[] { -2, -1, 0, 1, 2, 3, 4 }.ToAsyncEnumerable().Where(x => true);
            var ys = xs.Skip(2).Skip(3);

            Assert.Equal(2, await ys.CountAsync());

            Assert.Equal(3, await ys.FirstAsync());
            Assert.Equal(4, await ys.LastAsync());

            Assert.Equal(3, await ys.ElementAtAsync(0));
            Assert.Equal(4, await ys.ElementAtAsync(1));

            Assert.Equal(new[] { 3, 4 }, await ys.ToArrayAsync());
            Assert.Equal(new[] { 3, 4 }, await ys.ToListAsync());
        }

        [Fact]
        public async Task Skip_IAsyncPartition_NonEmpty_SkipTake()
        {
            var xs = new[] { 2, 3, 4, 5 }.ToAsyncEnumerable().Where(x => true);
            var ys = xs.Skip(1).Take(2);

            Assert.Equal(2, await ys.CountAsync());

            Assert.Equal(3, await ys.FirstAsync());
            Assert.Equal(4, await ys.LastAsync());

            Assert.Equal(3, await ys.ElementAtAsync(0));
            Assert.Equal(4, await ys.ElementAtAsync(1));

            Assert.Equal(new[] { 3, 4 }, await ys.ToArrayAsync());
            Assert.Equal(new[] { 3, 4 }, await ys.ToListAsync());
        }

        [Fact]
        public async Task Skip_IAsyncPartition_Empty_Skip()
        {
            var xs = new int[0].ToAsyncEnumerable().Where(x => true);
            var ys = xs.Skip(2);

            Assert.Equal(0, await ys.CountAsync());

            await AssertThrowsAsync<InvalidOperationException>(ys.FirstAsync().AsTask());
            await AssertThrowsAsync<InvalidOperationException>(ys.LastAsync().AsTask());

            await AssertThrowsAsync<ArgumentOutOfRangeException>(ys.ElementAtAsync(0).AsTask());
            await AssertThrowsAsync<ArgumentOutOfRangeException>(ys.ElementAtAsync(1).AsTask());

            Assert.Empty(await ys.ToArrayAsync());
            Assert.Empty(await ys.ToListAsync());
        }

        [Fact]
        public async Task Skip_IAsyncPartition_Empty_SkipSkip()
        {
            var xs = new[] { 1, 2 }.ToAsyncEnumerable().Where(x => true);
            var ys = xs.Skip(1).Skip(1);

            Assert.Equal(0, await ys.CountAsync());

            await AssertThrowsAsync<InvalidOperationException>(ys.FirstAsync().AsTask());
            await AssertThrowsAsync<InvalidOperationException>(ys.LastAsync().AsTask());

            await AssertThrowsAsync<ArgumentOutOfRangeException>(ys.ElementAtAsync(0).AsTask());
            await AssertThrowsAsync<ArgumentOutOfRangeException>(ys.ElementAtAsync(1).AsTask());

            Assert.Empty(await ys.ToArrayAsync());
            Assert.Empty(await ys.ToListAsync());
        }

        [Fact]
        public async Task Skip_IAsyncPartition_IList_NonEmpty_Skip()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.Skip(2);

            Assert.Equal(2, await ys.CountAsync());

            Assert.Equal(3, await ys.FirstAsync());
            Assert.Equal(4, await ys.LastAsync());

            Assert.Equal(3, await ys.ElementAtAsync(0));
            Assert.Equal(4, await ys.ElementAtAsync(1));

            Assert.Equal(new[] { 3, 4 }, await ys.ToArrayAsync());
            Assert.Equal(new[] { 3, 4 }, await ys.ToListAsync());
        }

        [Fact]
        public async Task Skip_IAsyncPartition_IList_NonEmpty_SkipSkip()
        {
            var xs = new[] { -2, -1, 0, 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.Skip(2).Skip(3);

            Assert.Equal(2, await ys.CountAsync());

            Assert.Equal(3, await ys.FirstAsync());
            Assert.Equal(4, await ys.LastAsync());

            Assert.Equal(3, await ys.ElementAtAsync(0));
            Assert.Equal(4, await ys.ElementAtAsync(1));

            Assert.Equal(new[] { 3, 4 }, await ys.ToArrayAsync());
            Assert.Equal(new[] { 3, 4 }, await ys.ToListAsync());
        }

        [Fact]
        public async Task Skip_IAsyncPartition_IList_NonEmpty_SkipTake()
        {
            var xs = new[] { 2, 3, 4, 5 }.ToAsyncEnumerable();
            var ys = xs.Skip(1).Take(2);

            Assert.Equal(2, await ys.CountAsync());

            Assert.Equal(3, await ys.FirstAsync());
            Assert.Equal(4, await ys.LastAsync());

            Assert.Equal(3, await ys.ElementAtAsync(0));
            Assert.Equal(4, await ys.ElementAtAsync(1));

            Assert.Equal(new[] { 3, 4 }, await ys.ToArrayAsync());
            Assert.Equal(new[] { 3, 4 }, await ys.ToListAsync());
        }

        [Fact]
        public async Task Skip_IAsyncPartition_IList_Empty_Skip()
        {
            var xs = new int[0].ToAsyncEnumerable();
            var ys = xs.Skip(2);

            Assert.Equal(0, await ys.CountAsync());

            await AssertThrowsAsync<InvalidOperationException>(ys.FirstAsync().AsTask());
            await AssertThrowsAsync<InvalidOperationException>(ys.LastAsync().AsTask());

            await AssertThrowsAsync<ArgumentOutOfRangeException>(ys.ElementAtAsync(0).AsTask());
            await AssertThrowsAsync<ArgumentOutOfRangeException>(ys.ElementAtAsync(1).AsTask());

            Assert.Empty(await ys.ToArrayAsync());
            Assert.Empty(await ys.ToListAsync());
        }

        [Fact]
        public async Task Skip_IAsyncPartition_IList_Empty_SkipSkip()
        {
            var xs = new[] { 1, 2 }.ToAsyncEnumerable();
            var ys = xs.Skip(1).Skip(1);

            Assert.Equal(0, await ys.CountAsync());

            await AssertThrowsAsync<InvalidOperationException>(ys.FirstAsync().AsTask());
            await AssertThrowsAsync<InvalidOperationException>(ys.LastAsync().AsTask());

            await AssertThrowsAsync<ArgumentOutOfRangeException>(ys.ElementAtAsync(0).AsTask());
            await AssertThrowsAsync<ArgumentOutOfRangeException>(ys.ElementAtAsync(1).AsTask());

            Assert.Empty(await ys.ToArrayAsync());
            Assert.Empty(await ys.ToListAsync());
        }
    }
}

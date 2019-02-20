// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class Take : AsyncEnumerableTests
    {
        [Fact]
        public void Take_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.Take<int>(default, 5));
        }

        [Fact]
        public async Task Take_Simple_TakeNegative()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable().Where(x => true);
            var ys = xs.Take(-2);

            var e = ys.GetAsyncEnumerator();
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Take_Simple_TakeNegative_IList()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.Take(-2);

            var e = ys.GetAsyncEnumerator();
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Take_Simple_TakeSome()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable().Where(x => true);
            var ys = xs.Take(2);

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Take_Simple_TakeSome_IList()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.Take(2);

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Take_Simple_TakeAll()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable().Where(x => true);
            var ys = xs.Take(10);

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 4);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Take_Simple_TakeAll_IList()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.Take(10);

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 4);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Take_Simple_TakeZero()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable().Where(x => true);
            var ys = xs.Take(0);

            var e = ys.GetAsyncEnumerator();
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Take_Simple_TakeZero_IList()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.Take(0);

            var e = ys.GetAsyncEnumerator();
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Take_Throws_Source()
        {
            var ex = new Exception("Bang");
            var xs = Throw<int>(ex);
            var ys = xs.Take(2);

            var e = ys.GetAsyncEnumerator();
            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        [Fact]
        public async Task Take_SequenceIdentity()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.Take(2);

            await SequenceIdentity(ys);
        }

        [Fact]
        public async Task Take_IAsyncPartition_NonEmpty_Take()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable().Where(x => true);
            var ys = xs.Take(2);

            Assert.Equal(2, await ys.CountAsync());

            Assert.Equal(1, await ys.FirstAsync());
            Assert.Equal(2, await ys.LastAsync());

            Assert.Equal(1, await ys.ElementAtAsync(0));
            Assert.Equal(2, await ys.ElementAtAsync(1));

            Assert.Equal(new[] { 1, 2 }, await ys.ToArrayAsync());
            Assert.Equal(new[] { 1, 2 }, await ys.ToListAsync());
        }

        [Fact]
        public async Task Take_IAsyncPartition_NonEmpty_TakeTake()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable().Where(x => true);
            var ys = xs.Take(3).Take(2);

            Assert.Equal(2, await ys.CountAsync());

            Assert.Equal(1, await ys.FirstAsync());
            Assert.Equal(2, await ys.LastAsync());

            Assert.Equal(1, await ys.ElementAtAsync(0));
            Assert.Equal(2, await ys.ElementAtAsync(1));

            Assert.Equal(new[] { 1, 2 }, await ys.ToArrayAsync());
            Assert.Equal(new[] { 1, 2 }, await ys.ToListAsync());
        }

        [Fact]
        public async Task Take_IAsyncPartition_NonEmpty_TakeSkip()
        {
            var xs = new[] { 2, 3, 4, 5 }.ToAsyncEnumerable().Where(x => true);
            var ys = xs.Take(3).Skip(1);

            Assert.Equal(2, await ys.CountAsync());

            Assert.Equal(3, await ys.FirstAsync());
            Assert.Equal(4, await ys.LastAsync());

            Assert.Equal(3, await ys.ElementAtAsync(0));
            Assert.Equal(4, await ys.ElementAtAsync(1));

            Assert.Equal(new[] { 3, 4 }, await ys.ToArrayAsync());
            Assert.Equal(new[] { 3, 4 }, await ys.ToListAsync());
        }

        [Fact]
        public async Task Take_IAsyncPartition_Empty_Take()
        {
            var xs = new int[0].ToAsyncEnumerable().Where(x => true);
            var ys = xs.Take(2);

            Assert.Equal(0, await ys.CountAsync());

            await AssertThrowsAsync<InvalidOperationException>(ys.FirstAsync().AsTask());
            await AssertThrowsAsync<InvalidOperationException>(ys.LastAsync().AsTask());

            await AssertThrowsAsync<ArgumentOutOfRangeException>(ys.ElementAtAsync(0).AsTask());
            await AssertThrowsAsync<ArgumentOutOfRangeException>(ys.ElementAtAsync(1).AsTask());

            Assert.Empty(await ys.ToArrayAsync());
            Assert.Empty(await ys.ToListAsync());
        }

        [Fact]
        public async Task Take_IAsyncPartition_Empty_TakeSkip()
        {
            var xs = new[] { 1, 2, 3, 4, 5 }.ToAsyncEnumerable().Where(x => true);
            var ys = xs.Take(7).Skip(5);

            Assert.Equal(0, await ys.CountAsync());

            await AssertThrowsAsync<InvalidOperationException>(ys.FirstAsync().AsTask());
            await AssertThrowsAsync<InvalidOperationException>(ys.LastAsync().AsTask());

            await AssertThrowsAsync<ArgumentOutOfRangeException>(ys.ElementAtAsync(0).AsTask());
            await AssertThrowsAsync<ArgumentOutOfRangeException>(ys.ElementAtAsync(1).AsTask());

            Assert.Empty(await ys.ToArrayAsync());
            Assert.Empty(await ys.ToListAsync());
        }

        [Fact]
        public async Task Take_IAsyncPartition_IList_NonEmpty_Take()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.Take(2);

            Assert.Equal(2, await ys.CountAsync());

            Assert.Equal(1, await ys.FirstAsync());
            Assert.Equal(2, await ys.LastAsync());

            Assert.Equal(1, await ys.ElementAtAsync(0));
            Assert.Equal(2, await ys.ElementAtAsync(1));

            Assert.Equal(new[] { 1, 2 }, await ys.ToArrayAsync());
            Assert.Equal(new[] { 1, 2 }, await ys.ToListAsync());
        }

        [Fact]
        public async Task Take_IAsyncPartition_IList_NonEmpty_TakeTake()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.Take(3).Take(2);

            Assert.Equal(2, await ys.CountAsync());

            Assert.Equal(1, await ys.FirstAsync());
            Assert.Equal(2, await ys.LastAsync());

            Assert.Equal(1, await ys.ElementAtAsync(0));
            Assert.Equal(2, await ys.ElementAtAsync(1));

            Assert.Equal(new[] { 1, 2 }, await ys.ToArrayAsync());
            Assert.Equal(new[] { 1, 2 }, await ys.ToListAsync());
        }

        [Fact]
        public async Task Take_IAsyncPartition_IList_NonEmpty_TakeSkip()
        {
            var xs = new[] { 2, 3, 4, 5 }.ToAsyncEnumerable();
            var ys = xs.Take(3).Skip(1);

            Assert.Equal(2, await ys.CountAsync());

            Assert.Equal(3, await ys.FirstAsync());
            Assert.Equal(4, await ys.LastAsync());

            Assert.Equal(3, await ys.ElementAtAsync(0));
            Assert.Equal(4, await ys.ElementAtAsync(1));

            Assert.Equal(new[] { 3, 4 }, await ys.ToArrayAsync());
            Assert.Equal(new[] { 3, 4 }, await ys.ToListAsync());
        }

        [Fact]
        public async Task Take_IAsyncPartition_IList_Empty_Take()
        {
            var xs = new int[0].ToAsyncEnumerable();
            var ys = xs.Take(2);

            Assert.Equal(0, await ys.CountAsync());

            await AssertThrowsAsync<InvalidOperationException>(ys.FirstAsync().AsTask());
            await AssertThrowsAsync<InvalidOperationException>(ys.LastAsync().AsTask());

            await AssertThrowsAsync<ArgumentOutOfRangeException>(ys.ElementAtAsync(0).AsTask());
            await AssertThrowsAsync<ArgumentOutOfRangeException>(ys.ElementAtAsync(1).AsTask());

            Assert.Empty(await ys.ToArrayAsync());
            Assert.Empty(await ys.ToListAsync());
        }

        [Fact]
        public async Task Take_IAsyncPartition_IList_Empty_TakeSkip()
        {
            var xs = new[] { 1, 2, 3, 4, 5 }.ToAsyncEnumerable();
            var ys = xs.Take(7).Skip(5);

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

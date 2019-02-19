// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class Aggregate : AsyncEnumerableTests
    {
        [Fact]
        public async Task AggregateAsync_Sync_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AggregateAsync<int>(default, (x, y) => x + y).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AggregateAsync(Return42, default(Func<int, int, int>)).AsTask());

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AggregateAsync<int, int>(default, 0, (x, y) => x + y).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AggregateAsync(Return42, 0, default(Func<int, int, int>)).AsTask());

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AggregateAsync<int, int, int>(default, 0, (x, y) => x + y, z => z).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AggregateAsync(Return42, 0, default, z => z).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AggregateAsync<int, int, int>(Return42, 0, (x, y) => x + y, default).AsTask());

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AggregateAsync<int>(default, (x, y) => x + y, CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AggregateAsync(Return42, default(Func<int, int, int>), CancellationToken.None).AsTask());

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AggregateAsync<int, int>(default, 0, (x, y) => x + y, CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AggregateAsync(Return42, 0, default(Func<int, int, int>), CancellationToken.None).AsTask());

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AggregateAsync<int, int, int>(default, 0, (x, y) => x + y, z => z, CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AggregateAsync(Return42, 0, default, z => z, CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AggregateAsync<int, int, int>(Return42, 0, (x, y) => x + y, default, CancellationToken.None).AsTask());
        }

        [Fact]
        public async Task AggregateAsync_Async_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AggregateAsync<int>(default, (x, y) => new ValueTask<int>(x + y)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AggregateAsync(Return42, default(Func<int, int, ValueTask<int>>)).AsTask());

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AggregateAsync<int, int>(default, 0, (x, y) => new ValueTask<int>(x + y)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AggregateAsync(Return42, 0, default(Func<int, int, ValueTask<int>>)).AsTask());

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AggregateAsync<int, int, int>(default, 0, (x, y) => new ValueTask<int>(x + y), z => new ValueTask<int>(z)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AggregateAsync(Return42, 0, default(Func<int, int, ValueTask<int>>), z => new ValueTask<int>(z)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AggregateAsync<int, int, int>(Return42, 0, (x, y) => new ValueTask<int>(x + y), default).AsTask());

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AggregateAsync<int>(default, (x, y) => new ValueTask<int>(x + y), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AggregateAsync(Return42, default(Func<int, int, ValueTask<int>>), CancellationToken.None).AsTask());

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AggregateAsync<int, int>(default, 0, (x, y) => new ValueTask<int>(x + y), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AggregateAsync(Return42, 0, default(Func<int, int, ValueTask<int>>), CancellationToken.None).AsTask());

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AggregateAsync<int, int, int>(default, 0, (x, y) => new ValueTask<int>(x + y), z => new ValueTask<int>(z), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AggregateAsync(Return42, 0, default(Func<int, int, ValueTask<int>>), z => new ValueTask<int>(z), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AggregateAsync<int, int, int>(Return42, 0, (x, y) => new ValueTask<int>(x + y), default, CancellationToken.None).AsTask());
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task AggregateAsync_AsyncCancel_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AggregateAsync<int>(default, (x, y, ct) => new ValueTask<int>(x + y)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AggregateAsync(Return42, default(Func<int, int, CancellationToken, ValueTask<int>>)).AsTask());

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AggregateAsync<int, int>(default, 0, (x, y, ct) => new ValueTask<int>(x + y)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AggregateAsync(Return42, 0, default(Func<int, int, CancellationToken, ValueTask<int>>)).AsTask());

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AggregateAsync<int, int, int>(default, 0, (x, y, ct) => new ValueTask<int>(x + y), (z, ct) => new ValueTask<int>(z)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AggregateAsync(Return42, 0, default(Func<int, int, CancellationToken, ValueTask<int>>), (z, ct) => new ValueTask<int>(z)).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AggregateAsync<int, int, int>(Return42, 0, (x, y, ct) => new ValueTask<int>(x + y), default).AsTask());

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AggregateAsync<int>(default, (x, y, ct) => new ValueTask<int>(x + y), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AggregateAsync(Return42, default(Func<int, int, CancellationToken, ValueTask<int>>), CancellationToken.None).AsTask());

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AggregateAsync<int, int>(default, 0, (x, y, ct) => new ValueTask<int>(x + y), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AggregateAsync(Return42, 0, default(Func<int, int, CancellationToken, ValueTask<int>>), CancellationToken.None).AsTask());

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AggregateAsync<int, int, int>(default, 0, (x, y, ct) => new ValueTask<int>(x + y), (z, ct) => new ValueTask<int>(z), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AggregateAsync(Return42, 0, default(Func<int, int, CancellationToken, ValueTask<int>>), (z, ct) => new ValueTask<int>(z), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.AggregateAsync<int, int, int>(Return42, 0, (x, y, ct) => new ValueTask<int>(x + y), default, CancellationToken.None).AsTask());
#endif
        }

        [Fact]
        public async Task AggregateAsync_Sync_Simple()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.AggregateAsync((x, y) => x * y);
            Assert.Equal(24, await ys);
        }

        [Fact]
        public async Task AggregateAsync_Sync_Empty()
        {
            var xs = new int[0].ToAsyncEnumerable();
            var ys = xs.AggregateAsync((x, y) => x * y);
            await AssertThrowsAsync<InvalidOperationException>(ys.AsTask());
        }

        [Fact]
        public async Task AggregateAsync_Sync_Throw_Source()
        {
            var ex = new Exception("Bang!");
            var xs = Throw<int>(ex);
            var ys = xs.AggregateAsync((x, y) => x * y);
            await AssertThrowsAsync(ys, ex);
        }

        [Fact]
        public async Task AggregateAsync_Sync_Throw_Selector()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.AggregateAsync(new Func<int, int, int>((x, y) => { throw ex; }));
            await AssertThrowsAsync(ys, ex);
        }

        [Fact]
        public async Task AggregateAsync_Sync_Seed_Simple()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.AggregateAsync(1, (x, y) => x * y);
            Assert.Equal(24, await ys);
        }

        [Fact]
        public async Task AggregateAsync_Sync_Seed_Emtpy()
        {
            var xs = new int[0].ToAsyncEnumerable();
            var ys = xs.AggregateAsync(1, (x, y) => x * y);
            Assert.Equal(1, await ys);
        }

        [Fact]
        public async Task AggregateAsync_Sync_Seed_Throw_Source()
        {
            var ex = new Exception("Bang!");
            var xs = Throw<int>(ex);
            var ys = xs.AggregateAsync(1, (x, y) => x * y);
            await AssertThrowsAsync(ys, ex);
        }

        [Fact]
        public async Task AggregateAsync_Sync_Seed_Throw_Selector()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.AggregateAsync(1, new Func<int, int, int>((x, y) => { throw ex; }));
            await AssertThrowsAsync(ys, ex);
        }

        [Fact]
        public async Task AggregateAsync_Sync_Seed_Result_Simple()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.AggregateAsync(1, (x, y) => x * y, x => x + 1);
            Assert.Equal(25, await ys);
        }

        [Fact]
        public async Task AggregateAsync_Sync_Seed_Result_Empty()
        {
            var xs = new int[0].ToAsyncEnumerable();
            var ys = xs.AggregateAsync(1, (x, y) => x * y, x => x + 1);
            Assert.Equal(2, await ys);
        }

        [Fact]
        public async Task AggregateAsync_Sync_Seed_Result_Throw_Source()
        {
            var ex = new Exception("Bang!");
            var xs = Throw<int>(ex);
            var ys = xs.AggregateAsync(1, (x, y) => x * y, x => x + 1);
            await AssertThrowsAsync(ys, ex);
        }

        [Fact]
        public async Task AggregateAsync_Sync_Seed_Result_Throw_Selector()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.AggregateAsync(1, (x, y) => { throw ex; }, x => x + 1);
            await AssertThrowsAsync(ys, ex);
        }

        [Fact]
        public async Task AggregateAsync_Sync_Seed_Result_Throw_ResultSelector()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.AggregateAsync<int, int, int>(1, (x, y) => x * y, x => { throw ex; });
            await AssertThrowsAsync(ys, ex);
        }

        [Fact]
        public async Task AggregateAsync_Async_Simple()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.AggregateAsync((x, y) => new ValueTask<int>(x * y));
            Assert.Equal(24, await ys);
        }

        [Fact]
        public async Task AggregateAsync_Async_Empty()
        {
            var xs = new int[0].ToAsyncEnumerable();
            var ys = xs.AggregateAsync((x, y) => new ValueTask<int>(x * y));
            await AssertThrowsAsync<InvalidOperationException>(ys.AsTask());
        }

        [Fact]
        public async Task AggregateAsync_Async_Throw_Source()
        {
            var ex = new Exception("Bang!");
            var xs = Throw<int>(ex);
            var ys = xs.AggregateAsync((x, y) => new ValueTask<int>(x * y));
            await AssertThrowsAsync(ys, ex);
        }

        [Fact]
        public async Task AggregateAsync_Async_Throw_Selector()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.AggregateAsync(new Func<int, int, ValueTask<int>>((x, y) => { throw ex; }));
            await AssertThrowsAsync(ys, ex);
        }

        [Fact]
        public async Task AggregateAsync_Async_Seed_Simple()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.AggregateAsync(1, (x, y) => new ValueTask<int>(x * y));
            Assert.Equal(24, await ys);
        }

        [Fact]
        public async Task AggregateAsync_Async_Seed_Emtpy()
        {
            var xs = new int[0].ToAsyncEnumerable();
            var ys = xs.AggregateAsync(1, (x, y) => new ValueTask<int>(x * y));
            Assert.Equal(1, await ys);
        }

        [Fact]
        public async Task AggregateAsync_Async_Seed_Throw_Source()
        {
            var ex = new Exception("Bang!");
            var xs = Throw<int>(ex);
            var ys = xs.AggregateAsync(1, (x, y) => new ValueTask<int>(x * y));
            await AssertThrowsAsync(ys, ex);
        }

        [Fact]
        public async Task AggregateAsync_Async_Seed_Throw_Selector()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.AggregateAsync(1, new Func<int, int, ValueTask<int>>((x, y) => { throw ex; }));
            await AssertThrowsAsync(ys, ex);
        }

        [Fact]
        public async Task AggregateAsync_Async_Seed_Result_Simple()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.AggregateAsync(1, (x, y) => new ValueTask<int>(x * y), x => new ValueTask<int>(x + 1));
            Assert.Equal(25, await ys);
        }

        [Fact]
        public async Task AggregateAsync_Async_Seed_Result_Empty()
        {
            var xs = new int[0].ToAsyncEnumerable();
            var ys = xs.AggregateAsync(1, (x, y) => new ValueTask<int>(x * y), x => new ValueTask<int>(x + 1));
            Assert.Equal(2, await ys);
        }

        [Fact]
        public async Task AggregateAsync_Async_Seed_Result_Throw_Source()
        {
            var ex = new Exception("Bang!");
            var xs = Throw<int>(ex);
            var ys = xs.AggregateAsync(1, (x, y) => new ValueTask<int>(x * y), x => new ValueTask<int>(x + 1));
            await AssertThrowsAsync(ys, ex);
        }

        [Fact]
        public async Task AggregateAsync_Async_Seed_Result_Throw_Selector()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.AggregateAsync(1, new Func<int, int, ValueTask<int>>((x, y) => { throw ex; }), x => new ValueTask<int>(x + 1));
            await AssertThrowsAsync(ys, ex);
        }

        [Fact]
        public async Task AggregateAsync_Async_Seed_Result_Throw_ResultSelector()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.AggregateAsync<int, int, int>(1, (x, y) => new ValueTask<int>(x * y), x => { throw ex; });
            await AssertThrowsAsync(ys, ex);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public async Task AggregateAsync_AsyncCancel_Simple()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.AggregateAsync((x, y, ct) => new ValueTask<int>(x * y));
            Assert.Equal(24, await ys);
        }

        [Fact]
        public async Task AggregateAsync_AsyncCancel_Empty()
        {
            var xs = new int[0].ToAsyncEnumerable();
            var ys = xs.AggregateAsync((x, y, ct) => new ValueTask<int>(x * y));
            await AssertThrowsAsync<InvalidOperationException>(ys.AsTask());
        }

        [Fact]
        public async Task AggregateAsync_AsyncCancel_Throw_Source()
        {
            var ex = new Exception("Bang!");
            var xs = Throw<int>(ex);
            var ys = xs.AggregateAsync((x, y, ct) => new ValueTask<int>(x * y));
            await AssertThrowsAsync(ys, ex);
        }

        [Fact]
        public async Task AggregateAsync_AsyncCancel_Throw_Selector()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.AggregateAsync(new Func<int, int, CancellationToken, ValueTask<int>>((x, y, ct) => { throw ex; }));
            await AssertThrowsAsync(ys, ex);
        }

        [Fact]
        public async Task AggregateAsync_AsyncCancel_Seed_Simple()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.AggregateAsync(1, (x, y, ct) => new ValueTask<int>(x * y));
            Assert.Equal(24, await ys);
        }

        [Fact]
        public async Task AggregateAsync_AsyncCancel_Seed_Emtpy()
        {
            var xs = new int[0].ToAsyncEnumerable();
            var ys = xs.AggregateAsync(1, (x, y, ct) => new ValueTask<int>(x * y));
            Assert.Equal(1, await ys);
        }

        [Fact]
        public async Task AggregateAsync_AsyncCancel_Seed_Throw_Source()
        {
            var ex = new Exception("Bang!");
            var xs = Throw<int>(ex);
            var ys = xs.AggregateAsync(1, (x, y, ct) => new ValueTask<int>(x * y));
            await AssertThrowsAsync(ys, ex);
        }

        [Fact]
        public async Task AggregateAsync_AsyncCancel_Seed_Throw_Selector()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.AggregateAsync(1, new Func<int, int, CancellationToken, ValueTask<int>>((x, y, ct) => { throw ex; }));
            await AssertThrowsAsync(ys, ex);
        }

        [Fact]
        public async Task AggregateAsync_AsyncCancel_Seed_Result_Simple()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.AggregateAsync(1, (x, y, ct) => new ValueTask<int>(x * y), (x, ct) => new ValueTask<int>(x + 1));
            Assert.Equal(25, await ys);
        }

        [Fact]
        public async Task AggregateAsync_AsyncCancel_Seed_Result_Empty()
        {
            var xs = new int[0].ToAsyncEnumerable();
            var ys = xs.AggregateAsync(1, (x, y, ct) => new ValueTask<int>(x * y), (x, ct) => new ValueTask<int>(x + 1));
            Assert.Equal(2, await ys);
        }

        [Fact]
        public async Task AggregateAsync_AsyncCancel_Seed_Result_Throw_Source()
        {
            var ex = new Exception("Bang!");
            var xs = Throw<int>(ex);
            var ys = xs.AggregateAsync(1, (x, y, ct) => new ValueTask<int>(x * y), (x, ct) => new ValueTask<int>(x + 1));
            await AssertThrowsAsync(ys, ex);
        }

        [Fact]
        public async Task AggregateAsync_AsyncCancel_Seed_Result_Throw_Selector()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.AggregateAsync(1, new Func<int, int, CancellationToken, ValueTask<int>>((x, y, ct) => { throw ex; }), (x, ct) => new ValueTask<int>(x + 1));
            await AssertThrowsAsync(ys, ex);
        }

        [Fact]
        public async Task AggregateAsync_AsyncCancel_Seed_Result_Throw_ResultSelector()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.AggregateAsync<int, int, int>(1, (x, y, ct) => new ValueTask<int>(x * y), (x, ct) => { throw ex; });
            await AssertThrowsAsync(ys, ex);
        }
#endif
    }
}

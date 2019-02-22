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
    public class Where : AsyncEnumerableTests
    {
        [Fact]
        public void Where_Sync_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.Where<int>(default, x => true));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.Where<int>(default, (x, i) => true));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.Where(Return42, default(Func<int, bool>)));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.Where(Return42, default(Func<int, int, bool>)));
        }

        [Fact]
        public async Task Where_Sync_Simple()
        {
            var xs = new[] { 8, 5, 7, 4, 6, 9, 2, 1, 0 }.ToAsyncEnumerable();
            var ys = xs.Where(x => x % 2 == 0);
            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 8);
            await HasNextAsync(e, 4);
            await HasNextAsync(e, 6);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 0);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Where_Sync_Indexed()
        {
            var xs = new[] { 8, 5, 7, 4, 6, 9, 2, 1, 0 }.ToAsyncEnumerable();
            var ys = xs.Where((x, i) => i % 2 == 0);

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 8);
            await HasNextAsync(e, 7);
            await HasNextAsync(e, 6);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 0);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Where_Sync_Throws_Predicate()
        {
            var xs = new[] { 8, 5, 7, 4, 6, 9, 2, 1, 0 }.ToAsyncEnumerable();
            var ex = new Exception("Bang");
            var ys = xs.Where(x => { if (x == 4) throw ex; return true; });

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 8);
            await HasNextAsync(e, 5);
            await HasNextAsync(e, 7);
            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        [Fact]
        public async Task Where_Sync_Indexed_Throws_Predicate()
        {
            var xs = new[] { 8, 5, 7, 4, 6, 9, 2, 1, 0 }.ToAsyncEnumerable();
            var ex = new Exception("Bang");
            var ys = xs.Where((x, i) => { if (i == 3) throw ex; return true; });

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 8);
            await HasNextAsync(e, 5);
            await HasNextAsync(e, 7);
            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        [Fact]
        public async Task Where_Sync_Throws_Source()
        {
            var ex = new Exception("Bang");
            var xs = Throw<int>(ex);
            var ys = xs.Where(x => true);

            var e = ys.GetAsyncEnumerator();
            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        [Fact]
        public async Task Where_Sync_Indexed_Throws_Source()
        {
            var ex = new Exception("Bang");
            var xs = Throw<int>(ex);

            var ys = xs.Where((x, i) => true);
            var e = ys.GetAsyncEnumerator();
            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        [Fact]
        public async Task Where_Sync_WhereWhere()
        {
            var xs = new[] { 8, 5, 7, 4, 6, 9, 2, 1, 0 }.ToAsyncEnumerable();
            var ys = xs.Where(x => x % 2 == 0).Where(x => x > 5);
            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 8);
            await HasNextAsync(e, 6);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Where_Sync_SequenceIdentity()
        {
            var xs = new[] { 8, 5, 7, 4, 6, 9, 2, 1, 0 }.ToAsyncEnumerable();
            var ys = xs.Where(x => x % 2 == 0);

            await SequenceIdentity(ys);
        }

        [Fact]
        public async Task Where_Sync_Indexed_SequenceIdentity()
        {
            var xs = new[] { 8, 5, 7, 4, 6, 9, 2, 1, 0 }.ToAsyncEnumerable();
            var ys = xs.Where((x, i) => i % 2 == 0);

            await SequenceIdentity(ys);
        }

        [Fact]
        public void Where_Async_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.Where<int>(default, x => new ValueTask<bool>(true)));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.Where<int>(default, (int x, int i) => new ValueTask<bool>(true)));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.Where(Return42, default(Func<int, ValueTask<bool>>)));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.Where(Return42, default(Func<int, int, ValueTask<bool>>)));
        }

        [Fact]
        public async Task Where_Async_Simple()
        {
            var xs = new[] { 8, 5, 7, 4, 6, 9, 2, 1, 0 }.ToAsyncEnumerable();
            var ys = xs.Where(x => new ValueTask<bool>(x % 2 == 0));
            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 8);
            await HasNextAsync(e, 4);
            await HasNextAsync(e, 6);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 0);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Where_Async_Indexed()
        {
            var xs = new[] { 8, 5, 7, 4, 6, 9, 2, 1, 0 }.ToAsyncEnumerable();
            var ys = xs.Where((x, i) => new ValueTask<bool>(i % 2 == 0));

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 8);
            await HasNextAsync(e, 7);
            await HasNextAsync(e, 6);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 0);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Where_Async_Throws_Predicate()
        {
            var xs = new[] { 8, 5, 7, 4, 6, 9, 2, 1, 0 }.ToAsyncEnumerable();
            var ex = new Exception("Bang");
            var ys = xs.Where(x => { if (x == 4) throw ex; return new ValueTask<bool>(true); });

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 8);
            await HasNextAsync(e, 5);
            await HasNextAsync(e, 7);
            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        [Fact]
        public async Task Where_Async_Indexed_Throws_Predicate()
        {
            var xs = new[] { 8, 5, 7, 4, 6, 9, 2, 1, 0 }.ToAsyncEnumerable();
            var ex = new Exception("Bang");
            var ys = xs.Where((x, i) => { if (i == 3) throw ex; return new ValueTask<bool>(true); });

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 8);
            await HasNextAsync(e, 5);
            await HasNextAsync(e, 7);
            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        [Fact]
        public async Task Where_Async_Throws_Source()
        {
            var ex = new Exception("Bang");
            var xs = Throw<int>(ex);
            var ys = xs.Where(x => new ValueTask<bool>(true));

            var e = ys.GetAsyncEnumerator();
            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        [Fact]
        public async Task Where_Async_Indexed_Throws_Source()
        {
            var ex = new Exception("Bang");
            var xs = Throw<int>(ex);

            var ys = xs.Where((int x, int i) => new ValueTask<bool>(true));
            var e = ys.GetAsyncEnumerator();
            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        [Fact]
        public async Task Where_Async_WhereWhere()
        {
            var xs = new[] { 8, 5, 7, 4, 6, 9, 2, 1, 0 }.ToAsyncEnumerable();
            var ys = xs.Where(x => new ValueTask<bool>(x % 2 == 0)).Where(x => new ValueTask<bool>(x > 5));
            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 8);
            await HasNextAsync(e, 6);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Where_Async_SequenceIdentity()
        {
            var xs = new[] { 8, 5, 7, 4, 6, 9, 2, 1, 0 }.ToAsyncEnumerable();
            var ys = xs.Where(x => new ValueTask<bool>(x % 2 == 0));

            await SequenceIdentity(ys);
        }

        [Fact]
        public async Task Where_Async_Indexed_SequenceIdentity()
        {
            var xs = new[] { 8, 5, 7, 4, 6, 9, 2, 1, 0 }.ToAsyncEnumerable();
            var ys = xs.Where((x, i) => new ValueTask<bool>(i % 2 == 0));

            await SequenceIdentity(ys);
        }

#if !NO_DEEP_CANCELLATION

        // REVIEW: These overloads are problematic for type inference. E.g. xs.Where((x, ct) => ...) is ambiguous.

        [Fact]
        public void Where_Async_Cancel_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.Where<int>(default, (int x, CancellationToken ct) => new ValueTask<bool>(true)));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.Where<int>(default, (x, i, ct) => new ValueTask<bool>(true)));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.Where(Return42, default(Func<int, CancellationToken, ValueTask<bool>>)));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.Where(Return42, default(Func<int, int, CancellationToken, ValueTask<bool>>)));
        }

        [Fact]
        public async Task Where_Async_Cancel_Simple()
        {
            var xs = new[] { 8, 5, 7, 4, 6, 9, 2, 1, 0 }.ToAsyncEnumerable();
            var ys = xs.Where((int x, CancellationToken ct) => new ValueTask<bool>(x % 2 == 0));
            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 8);
            await HasNextAsync(e, 4);
            await HasNextAsync(e, 6);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 0);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Where_Async_Cancel_Indexed()
        {
            var xs = new[] { 8, 5, 7, 4, 6, 9, 2, 1, 0 }.ToAsyncEnumerable();
            var ys = xs.Where((x, i, ct) => new ValueTask<bool>(i % 2 == 0));

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 8);
            await HasNextAsync(e, 7);
            await HasNextAsync(e, 6);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 0);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Where_Async_Cancel_Throws_Predicate()
        {
            var xs = new[] { 8, 5, 7, 4, 6, 9, 2, 1, 0 }.ToAsyncEnumerable();
            var ex = new Exception("Bang");
            var ys = xs.Where((int x, CancellationToken ct) => { if (x == 4) throw ex; return new ValueTask<bool>(true); });

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 8);
            await HasNextAsync(e, 5);
            await HasNextAsync(e, 7);
            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        [Fact]
        public async Task Where_Async_Cancel_Indexed_Throws_Predicate()
        {
            var xs = new[] { 8, 5, 7, 4, 6, 9, 2, 1, 0 }.ToAsyncEnumerable();
            var ex = new Exception("Bang");
            var ys = xs.Where((x, i, ct) => { if (i == 3) throw ex; return new ValueTask<bool>(true); });

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 8);
            await HasNextAsync(e, 5);
            await HasNextAsync(e, 7);
            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        [Fact]
        public async Task Where_Async_Cancel_Throws_Source()
        {
            var ex = new Exception("Bang");
            var xs = Throw<int>(ex);
            var ys = xs.Where((int x, CancellationToken ct) => new ValueTask<bool>(true));

            var e = ys.GetAsyncEnumerator();
            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        [Fact]
        public async Task Where_Async_Cancel_Indexed_Throws_Source()
        {
            var ex = new Exception("Bang");
            var xs = Throw<int>(ex);

            var ys = xs.Where((x, i, ct) => new ValueTask<bool>(true));
            var e = ys.GetAsyncEnumerator();
            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        [Fact]
        public async Task Where_Async_Cancel_WhereWhere()
        {
            var xs = new[] { 8, 5, 7, 4, 6, 9, 2, 1, 0 }.ToAsyncEnumerable();
            var ys = xs.Where((int x, CancellationToken ct) => new ValueTask<bool>(x % 2 == 0)).Where((int x, CancellationToken ct) => new ValueTask<bool>(x > 5));
            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 8);
            await HasNextAsync(e, 6);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Where_Async_Cancel_SequenceIdentity()
        {
            var xs = new[] { 8, 5, 7, 4, 6, 9, 2, 1, 0 }.ToAsyncEnumerable();
            var ys = xs.Where((int x, CancellationToken ct) => new ValueTask<bool>(x % 2 == 0));

            await SequenceIdentity(ys);
        }

        [Fact]
        public async Task Where_Async_Cancel_Indexed_SequenceIdentity()
        {
            var xs = new[] { 8, 5, 7, 4, 6, 9, 2, 1, 0 }.ToAsyncEnumerable();
            var ys = xs.Where((x, i, ct) => new ValueTask<bool>(i % 2 == 0));

            await SequenceIdentity(ys);
        }

#endif
    }
}

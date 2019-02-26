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
    public class SkipWhile : AsyncEnumerableTests
    {
        [Fact]
        public void SkipWhile_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.SkipWhile<int>(default, x => true));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.SkipWhile<int>(default, (x, i) => true));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.SkipWhile(Return42, default(Func<int, bool>)));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.SkipWhile(Return42, default(Func<int, int, bool>)));
        }

        [Fact]
        public async Task SkipWhile_Simple1()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.SkipWhile(x => x < 3);

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 4);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task SkipWhile_Simple2()
        {
            var xs = new[] { 1, 2, 3, 4, 3, 2, 1 }.ToAsyncEnumerable();
            var ys = xs.SkipWhile(x => x < 3);

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 4);
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 1);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task SkipWhile_False()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.SkipWhile(x => false);

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 4);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task SkipWhile_True()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.SkipWhile(x => true);

            var e = ys.GetAsyncEnumerator();
            await NoNextAsync(e);
        }

        [Fact]
        public async Task SkipWhile_Throws_Predicate()
        {
            var ex = new Exception("Bang");
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.SkipWhile(new Func<int, bool>(x => { throw ex; }));

            var e = ys.GetAsyncEnumerator();
            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        [Fact]
        public async Task SkipWhile_Indexed()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.SkipWhile((x, i) => i < 2);

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 4);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task SkipWhile_Indexed_False()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.SkipWhile((x, i) => false);

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 4);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task SkipWhile_Indexed_True()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.SkipWhile((x, i) => true);

            var e = ys.GetAsyncEnumerator();
            await NoNextAsync(e);
        }

        [Fact]
        public async Task SkipWhile_Indexed_Throws_Predicate()
        {
            var ex = new Exception("Bang");
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.SkipWhile(new Func<int, int, bool>((x, i) => { throw ex; }));

            var e = ys.GetAsyncEnumerator();
            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        [Fact]
        public async Task SkipWhile_SequenceIdentity()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.SkipWhile(x => x < 3);

            await SequenceIdentity(ys);
        }

        [Fact]
        public async Task SkipWhile_Indexed_SequenceIdentity()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.SkipWhile((x, i) => false);

            await SequenceIdentity(ys);
        }

        [Fact]
        public void SkipWhileAwait_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.SkipWhileAwait<int>(default, x => new ValueTask<bool>(true)));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.SkipWhileAwait(default, (int x, int i) => new ValueTask<bool>(true)));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.SkipWhileAwait(Return42, default(Func<int, ValueTask<bool>>)));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.SkipWhileAwait(Return42, default(Func<int, int, ValueTask<bool>>)));
        }

        [Fact]
        public async Task SkipWhileAwait_Simple1()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.SkipWhileAwait(x => new ValueTask<bool>(x < 3));

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 4);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task SkipWhileAwait_Simple2()
        {
            var xs = new[] { 1, 2, 3, 4, 3, 2, 1 }.ToAsyncEnumerable();
            var ys = xs.SkipWhileAwait(x => new ValueTask<bool>(x < 3));

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 4);
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 1);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task SkipWhileAwait_False()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.SkipWhileAwait(x => new ValueTask<bool>(false));

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 4);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task SkipWhileAwait_True()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.SkipWhileAwait(x => new ValueTask<bool>(true));

            var e = ys.GetAsyncEnumerator();
            await NoNextAsync(e);
        }

        [Fact]
        public async Task SkipWhileAwait_Throws_Predicate()
        {
            var ex = new Exception("Bang");
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.SkipWhileAwait(new Func<int, ValueTask<bool>>(x => { throw ex; }));

            var e = ys.GetAsyncEnumerator();
            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        [Fact]
        public async Task SkipWhileAwait_Indexed()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.SkipWhileAwait((x, i) => new ValueTask<bool>(i < 2));

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 4);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task SkipWhileAwait_Indexed_False()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.SkipWhileAwait((int x, int i) => new ValueTask<bool>(false));

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 4);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task SkipWhileAwait_Indexed_True()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.SkipWhileAwait((int x, int i) => new ValueTask<bool>(true));

            var e = ys.GetAsyncEnumerator();
            await NoNextAsync(e);
        }

        [Fact]
        public async Task SkipWhileAwait_Indexed_Throws_Predicate()
        {
            var ex = new Exception("Bang");
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.SkipWhileAwait(new Func<int, int, ValueTask<bool>>((x, i) => { throw ex; }));

            var e = ys.GetAsyncEnumerator();
            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        [Fact]
        public async Task SkipWhileAwait_SequenceIdentity()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.SkipWhileAwait(x => new ValueTask<bool>(x < 3));

            await SequenceIdentity(ys);
        }

        [Fact]
        public async Task SkipWhileAwait_Indexed_SequenceIdentity()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.SkipWhileAwait((int x, int i) => new ValueTask<bool>(false));

            await SequenceIdentity(ys);
        }

#if !NO_DEEP_CANCELLATION

        [Fact]
        public void SkipWhileAwaitWithCancellation_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.SkipWhileAwaitWithCancellation(default, (int x, CancellationToken ct) => new ValueTask<bool>(true)));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.SkipWhileAwaitWithCancellation<int>(default, (x, i, ct) => new ValueTask<bool>(true)));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.SkipWhileAwaitWithCancellation(Return42, default(Func<int, CancellationToken, ValueTask<bool>>)));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.SkipWhileAwaitWithCancellation(Return42, default(Func<int, int, CancellationToken, ValueTask<bool>>)));
        }

        [Fact]
        public async Task SkipWhileAwaitWithCancellation_Simple1()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.SkipWhileAwaitWithCancellation((int x, CancellationToken ct) => new ValueTask<bool>(x < 3));

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 4);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task SkipWhileAwaitWithCancellation_Simple2()
        {
            var xs = new[] { 1, 2, 3, 4, 3, 2, 1 }.ToAsyncEnumerable();
            var ys = xs.SkipWhileAwaitWithCancellation((int x, CancellationToken ct) => new ValueTask<bool>(x < 3));

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 4);
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 1);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task SkipWhileAwaitWithCancellation_False()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.SkipWhileAwaitWithCancellation((int x, CancellationToken ct) => new ValueTask<bool>(false));

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 4);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task SkipWhileAwaitWithCancellation_True()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.SkipWhileAwaitWithCancellation((int x, CancellationToken ct) => new ValueTask<bool>(true));

            var e = ys.GetAsyncEnumerator();
            await NoNextAsync(e);
        }

        [Fact]
        public async Task SkipWhileAwaitWithCancellation_Throws_Predicate()
        {
            var ex = new Exception("Bang");
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.SkipWhileAwaitWithCancellation(new Func<int, CancellationToken, ValueTask<bool>>((x, ct) => { throw ex; }));

            var e = ys.GetAsyncEnumerator();
            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        [Fact]
        public async Task SkipWhileAwaitWithCancellation_Indexed()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.SkipWhileAwaitWithCancellation((x, i, ct) => new ValueTask<bool>(i < 2));

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 4);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task SkipWhileAwaitWithCancellation_Indexed_False()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.SkipWhileAwaitWithCancellation((x, i, ct) => new ValueTask<bool>(false));

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 4);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task SkipWhileAwaitWithCancellation_Indexed_True()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.SkipWhileAwaitWithCancellation((x, i, ct) => new ValueTask<bool>(true));

            var e = ys.GetAsyncEnumerator();
            await NoNextAsync(e);
        }

        [Fact]
        public async Task SkipWhileAwaitWithCancellation_Indexed_Throws_Predicate()
        {
            var ex = new Exception("Bang");
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.SkipWhileAwaitWithCancellation(new Func<int, int, CancellationToken, ValueTask<bool>>((x, i, ct) => { throw ex; }));

            var e = ys.GetAsyncEnumerator();
            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        [Fact]
        public async Task SkipWhileAwaitWithCancellation_SequenceIdentity()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.SkipWhileAwaitWithCancellation((int x, CancellationToken ct) => new ValueTask<bool>(x < 3));

            await SequenceIdentity(ys);
        }

        [Fact]
        public async Task SkipWhileAwaitWithCancellation_Indexed_SequenceIdentity()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.SkipWhileAwaitWithCancellation((x, i, ct) => new ValueTask<bool>(false));

            await SequenceIdentity(ys);
        }

#endif
    }
}

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
    public class TakeWhile : AsyncEnumerableTests
    {
        [Fact]
        public void TakeWhile_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.TakeWhile<int>(default, x => true));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.TakeWhile<int>(default, (x, i) => true));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.TakeWhile(Return42, default(Func<int, bool>)));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.TakeWhile(Return42, default(Func<int, int, bool>)));
        }

        [Fact]
        public async Task TakeWhile_Simple1()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.TakeWhile(x => x < 3);

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task TakeWhile_Simple2()
        {
            var xs = new[] { 1, 2, 3, 4, 3, 2, 1 }.ToAsyncEnumerable();
            var ys = xs.TakeWhile(x => x < 3);

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task TakeWhile_False()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.TakeWhile(x => false);

            var e = ys.GetAsyncEnumerator();
            await NoNextAsync(e);
        }

        [Fact]
        public async Task TakeWhile_True()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.TakeWhile(x => true);

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 4);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task TakeWhile_Throws_Predicate()
        {
            var ex = new Exception("Bang");
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.TakeWhile(new Func<int, bool>(x => { throw ex; }));

            var e = ys.GetAsyncEnumerator();
            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        [Fact]
        public async Task TakeWhile_Indexed()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.TakeWhile((x, i) => i < 2);

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task TakeWhile_Indexed_False()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.TakeWhile((x, i) => false);

            var e = ys.GetAsyncEnumerator();
            await NoNextAsync(e);
        }

        [Fact]
        public async Task TakeWhile_Indexed_True()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.TakeWhile((x, i) => true);

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 4);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task TakeWhile_Indexed_Throws_Predicate()
        {
            var ex = new Exception("Bang");
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.TakeWhile(new Func<int, int, bool>((x, i) => { throw ex; }));

            var e = ys.GetAsyncEnumerator();
            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        [Fact]
        public async Task TakeWhile_SequenceIdentity()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.TakeWhile(x => x < 3);

            await SequenceIdentity(ys);
        }

        [Fact]
        public async Task TakeWhile_Indexed_SequenceIdentity()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.TakeWhile((x, i) => i < 2);

            await SequenceIdentity(ys);
        }

        [Fact]
        public void TakeWhileAwait_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.TakeWhileAwait<int>(default, x => new ValueTask<bool>(true)));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.TakeWhileAwait(default, (int x, int i) => new ValueTask<bool>(true)));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.TakeWhileAwait(Return42, default(Func<int, ValueTask<bool>>)));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.TakeWhileAwait(Return42, default(Func<int, int, ValueTask<bool>>)));
        }

        [Fact]
        public async Task TakeWhileAwait_Simple1()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.TakeWhileAwait(x => new ValueTask<bool>(x < 3));

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task TakeWhileAwait_Simple2()
        {
            var xs = new[] { 1, 2, 3, 4, 3, 2, 1 }.ToAsyncEnumerable();
            var ys = xs.TakeWhileAwait(x => new ValueTask<bool>(x < 3));

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task TakeWhileAwait_False()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.TakeWhileAwait(x => new ValueTask<bool>(false));

            var e = ys.GetAsyncEnumerator();
            await NoNextAsync(e);
        }

        [Fact]
        public async Task TakeWhileAwait_True()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.TakeWhileAwait(x => new ValueTask<bool>(true));

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 4);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task TakeWhileAwait_Throws_Predicate()
        {
            var ex = new Exception("Bang");
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.TakeWhileAwait(new Func<int, ValueTask<bool>>(x => { throw ex; }));

            var e = ys.GetAsyncEnumerator();
            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        [Fact]
        public async Task TakeWhileAwait_Indexed()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.TakeWhileAwait((x, i) => new ValueTask<bool>(i < 2));

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task TakeWhileAwait_Indexed_False()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.TakeWhileAwait((int x, int i) => new ValueTask<bool>(false));

            var e = ys.GetAsyncEnumerator();
            await NoNextAsync(e);
        }

        [Fact]
        public async Task TakeWhileAwait_Indexed_True()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.TakeWhileAwait((int x, int i) => new ValueTask<bool>(true));

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 4);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task TakeWhileAwait_Indexed_Throws_Predicate()
        {
            var ex = new Exception("Bang");
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.TakeWhileAwait(new Func<int, int, ValueTask<bool>>((x, i) => { throw ex; }));

            var e = ys.GetAsyncEnumerator();
            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        [Fact]
        public async Task TakeWhileAwait_SequenceIdentity()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.TakeWhileAwait(x => new ValueTask<bool>(x < 3));

            await SequenceIdentity(ys);
        }

        [Fact]
        public async Task TakeWhileAwait_Indexed_SequenceIdentity()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.TakeWhileAwait((x, i) => new ValueTask<bool>(i < 2));

            await SequenceIdentity(ys);
        }

#if !NO_DEEP_CANCELLATION

        [Fact]
        public void TakeWhileAwaitWithCancellation_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.TakeWhileAwaitWithCancellation(default, (int x, CancellationToken ct) => new ValueTask<bool>(true)));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.TakeWhileAwaitWithCancellation<int>(default, (x, i, ct) => new ValueTask<bool>(true)));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.TakeWhileAwaitWithCancellation(Return42, default(Func<int, CancellationToken, ValueTask<bool>>)));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.TakeWhileAwaitWithCancellation(Return42, default(Func<int, int, CancellationToken, ValueTask<bool>>)));
        }

        [Fact]
        public async Task TakeWhileAwaitWithCancellation_Simple1()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.TakeWhileAwaitWithCancellation((int x, CancellationToken ct) => new ValueTask<bool>(x < 3));

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task TakeWhileAwaitWithCancellation_Simple2()
        {
            var xs = new[] { 1, 2, 3, 4, 3, 2, 1 }.ToAsyncEnumerable();
            var ys = xs.TakeWhileAwaitWithCancellation((int x, CancellationToken ct) => new ValueTask<bool>(x < 3));

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task TakeWhileAwaitWithCancellation_False()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.TakeWhileAwaitWithCancellation((int x, CancellationToken ct) => new ValueTask<bool>(false));

            var e = ys.GetAsyncEnumerator();
            await NoNextAsync(e);
        }

        [Fact]
        public async Task TakeWhileAwaitWithCancellation_True()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.TakeWhileAwaitWithCancellation((int x, CancellationToken ct) => new ValueTask<bool>(true));

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 4);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task TakeWhileAwaitWithCancellation_Throws_Predicate()
        {
            var ex = new Exception("Bang");
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.TakeWhileAwaitWithCancellation(new Func<int, CancellationToken, ValueTask<bool>>((x, ct) => { throw ex; }));

            var e = ys.GetAsyncEnumerator();
            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        [Fact]
        public async Task TakeWhileAwaitWithCancellation_Indexed()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.TakeWhileAwaitWithCancellation((x, i, ct) => new ValueTask<bool>(i < 2));

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task TakeWhileAwaitWithCancellation_Indexed_False()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.TakeWhileAwaitWithCancellation((x, i, ct) => new ValueTask<bool>(false));

            var e = ys.GetAsyncEnumerator();
            await NoNextAsync(e);
        }

        [Fact]
        public async Task TakeWhileAwaitWithCancellation_Indexed_True()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.TakeWhileAwaitWithCancellation((x, i, ct) => new ValueTask<bool>(true));

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 4);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task TakeWhileAwaitWithCancellation_Indexed_Throws_Predicate()
        {
            var ex = new Exception("Bang");
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.TakeWhileAwaitWithCancellation(new Func<int, int, CancellationToken, ValueTask<bool>>((x, ct, i) => { throw ex; }));

            var e = ys.GetAsyncEnumerator();
            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        [Fact]
        public async Task TakeWhileAwaitWithCancellation_SequenceIdentity()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.TakeWhileAwaitWithCancellation((int x, CancellationToken ct) => new ValueTask<bool>(x < 3));

            await SequenceIdentity(ys);
        }

        [Fact]
        public async Task TakeWhileAwaitWithCancellation_Indexed_SequenceIdentity()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.TakeWhileAwaitWithCancellation((x, i, ct) => new ValueTask<bool>(i < 2));

            await SequenceIdentity(ys);
        }

#endif
    }
}

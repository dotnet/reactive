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
    public class Zip : AsyncEnumerableTests
    {
        [Fact]
        public void Zip_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.Zip<int, int, int>(default, Return42, (x, y) => x + y));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.Zip<int, int, int>(Return42, default, (x, y) => x + y));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.Zip(Return42, Return42, default(Func<int, int, int>)));
        }

        [Fact]
        public async Task Zip_EqualLength()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = new[] { 4, 5, 6 }.ToAsyncEnumerable();
            var res = xs.Zip(ys, (x, y) => x * y);

            var e = res.GetAsyncEnumerator();
            await HasNextAsync(e, 1 * 4);
            await HasNextAsync(e, 2 * 5);
            await HasNextAsync(e, 3 * 6);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Zip_LeftShorter()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = new[] { 4, 5, 6, 7 }.ToAsyncEnumerable();
            var res = xs.Zip(ys, (x, y) => x * y);

            var e = res.GetAsyncEnumerator();
            await HasNextAsync(e, 1 * 4);
            await HasNextAsync(e, 2 * 5);
            await HasNextAsync(e, 3 * 6);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Zip_RightShorter()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = new[] { 4, 5, 6 }.ToAsyncEnumerable();
            var res = xs.Zip(ys, (x, y) => x * y);

            var e = res.GetAsyncEnumerator();
            await HasNextAsync(e, 1 * 4);
            await HasNextAsync(e, 2 * 5);
            await HasNextAsync(e, 3 * 6);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Zip_Throws_Right()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = Throw<int>(ex);
            var res = xs.Zip(ys, (x, y) => x * y);

            var e = res.GetAsyncEnumerator();
            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        [Fact]
        public async Task Zip_Throws_Left()
        {
            var ex = new Exception("Bang!");
            var xs = Throw<int>(ex);
            var ys = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var res = xs.Zip(ys, (x, y) => x * y);

            var e = res.GetAsyncEnumerator();
            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        [Fact]
        public async Task Zip_Throws_Selector()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var res = xs.Zip(ys, (x, y) => { if (x > 0) throw ex; return x * y; });

            var e = res.GetAsyncEnumerator();
            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        [Fact]
        public async Task Zip_SequenceIdentity()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = new[] { 4, 5, 6 }.ToAsyncEnumerable();
            var res = xs.Zip(ys, (x, y) => x * y);

            await SequenceIdentity(res);
        }

        [Fact]
        public void ZipAwait_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.ZipAwait<int, int, int>(default, Return42, (x, y) => new ValueTask<int>(x + y)));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.ZipAwait<int, int, int>(Return42, default, (x, y) => new ValueTask<int>(x + y)));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.ZipAwait(Return42, Return42, default(Func<int, int, ValueTask<int>>)));
        }

        [Fact]
        public async Task ZipAwait_EqualLength()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = new[] { 4, 5, 6 }.ToAsyncEnumerable();
            var res = xs.ZipAwait(ys, (x, y) => new ValueTask<int>(x * y));

            var e = res.GetAsyncEnumerator();
            await HasNextAsync(e, 1 * 4);
            await HasNextAsync(e, 2 * 5);
            await HasNextAsync(e, 3 * 6);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task ZipAwait_LeftShorter()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = new[] { 4, 5, 6, 7 }.ToAsyncEnumerable();
            var res = xs.ZipAwait(ys, (x, y) => new ValueTask<int>(x * y));

            var e = res.GetAsyncEnumerator();
            await HasNextAsync(e, 1 * 4);
            await HasNextAsync(e, 2 * 5);
            await HasNextAsync(e, 3 * 6);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task ZipAwait_RightShorter()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = new[] { 4, 5, 6 }.ToAsyncEnumerable();
            var res = xs.ZipAwait(ys, (x, y) => new ValueTask<int>(x * y));

            var e = res.GetAsyncEnumerator();
            await HasNextAsync(e, 1 * 4);
            await HasNextAsync(e, 2 * 5);
            await HasNextAsync(e, 3 * 6);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task ZipAwait_Throws_Right()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = Throw<int>(ex);
            var res = xs.ZipAwait(ys, (x, y) => new ValueTask<int>(x * y));

            var e = res.GetAsyncEnumerator();
            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        [Fact]
        public async Task ZipAwait_Throws_Left()
        {
            var ex = new Exception("Bang!");
            var xs = Throw<int>(ex);
            var ys = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var res = xs.ZipAwait(ys, (x, y) => new ValueTask<int>(x * y));

            var e = res.GetAsyncEnumerator();
            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        [Fact]
        public async Task ZipAwait_Throws_Selector()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var res = xs.ZipAwait(ys, (x, y) => { if (x > 0) throw ex; return new ValueTask<int>(x * y); });

            var e = res.GetAsyncEnumerator();
            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        [Fact]
        public async Task ZipAwait_SequenceIdentity()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = new[] { 4, 5, 6 }.ToAsyncEnumerable();
            var res = xs.ZipAwait(ys, (x, y) => new ValueTask<int>(x * y));

            await SequenceIdentity(res);
        }

#if !NO_DEEP_CANCELLATION
        [Fact]
        public void ZipAwaitWithCancellation_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.ZipAwaitWithCancellation<int, int, int>(default, Return42, (x, y, ct) => new ValueTask<int>(x + y)));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.ZipAwaitWithCancellation<int, int, int>(Return42, default, (x, y, ct) => new ValueTask<int>(x + y)));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.ZipAwaitWithCancellation(Return42, Return42, default(Func<int, int, CancellationToken, ValueTask<int>>)));
        }

        [Fact]
        public async Task ZipAwaitWithCancellation_EqualLength()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = new[] { 4, 5, 6 }.ToAsyncEnumerable();
            var res = xs.ZipAwaitWithCancellation(ys, (x, y, ct) => new ValueTask<int>(x * y));

            var e = res.GetAsyncEnumerator();
            await HasNextAsync(e, 1 * 4);
            await HasNextAsync(e, 2 * 5);
            await HasNextAsync(e, 3 * 6);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task ZipAwaitWithCancellation_LeftShorter()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = new[] { 4, 5, 6, 7 }.ToAsyncEnumerable();
            var res = xs.ZipAwaitWithCancellation(ys, (x, y, ct) => new ValueTask<int>(x * y));

            var e = res.GetAsyncEnumerator();
            await HasNextAsync(e, 1 * 4);
            await HasNextAsync(e, 2 * 5);
            await HasNextAsync(e, 3 * 6);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task ZipAwaitWithCancellation_RightShorter()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = new[] { 4, 5, 6 }.ToAsyncEnumerable();
            var res = xs.ZipAwaitWithCancellation(ys, (x, y, ct) => new ValueTask<int>(x * y));

            var e = res.GetAsyncEnumerator();
            await HasNextAsync(e, 1 * 4);
            await HasNextAsync(e, 2 * 5);
            await HasNextAsync(e, 3 * 6);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task ZipAwaitWithCancellation_Throws_Right()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = Throw<int>(ex);
            var res = xs.ZipAwaitWithCancellation(ys, (x, y, ct) => new ValueTask<int>(x * y));

            var e = res.GetAsyncEnumerator();
            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        [Fact]
        public async Task ZipAwaitWithCancellation_Throws_Left()
        {
            var ex = new Exception("Bang!");
            var xs = Throw<int>(ex);
            var ys = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var res = xs.ZipAwaitWithCancellation(ys, (x, y, ct) => new ValueTask<int>(x * y));

            var e = res.GetAsyncEnumerator();
            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        [Fact]
        public async Task ZipAwaitWithCancellation_Throws_Selector()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var res = xs.ZipAwaitWithCancellation(ys, (x, y, ct) => { if (x > 0) throw ex; return new ValueTask<int>(x * y); });

            var e = res.GetAsyncEnumerator();
            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        [Fact]
        public async Task ZipAwaitWithCancellation_SequenceIdentity()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = new[] { 4, 5, 6 }.ToAsyncEnumerable();
            var res = xs.ZipAwaitWithCancellation(ys, (x, y, ct) => new ValueTask<int>(x * y));

            await SequenceIdentity(res);
        }
#endif

#if HAS_VALUETUPLE
         [Fact]
        public void Zip_Tuple_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.Zip<int, int>(default, Return42));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.Zip<int, int>(Return42, default));
        }

        [Fact]
        public async Task Zip_Tuple_EqualLength()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = new[] { 4, 5, 6 }.ToAsyncEnumerable();
            var res = xs.Zip(ys);

            var e = res.GetAsyncEnumerator();
            await HasNextAsync(e, (1, 4));
            await HasNextAsync(e, (2, 5));
            await HasNextAsync(e, (3, 6));
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Zip_Tuple_LeftShorter()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = new[] { 4, 5, 6, 7 }.ToAsyncEnumerable();
            var res = xs.Zip(ys);

            var e = res.GetAsyncEnumerator();
            await HasNextAsync(e, (1, 4));
            await HasNextAsync(e, (2, 5));
            await HasNextAsync(e, (3, 6));
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Zip_Tuple_RightShorter()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = new[] { 4, 5, 6 }.ToAsyncEnumerable();
            var res = xs.Zip(ys);

            var e = res.GetAsyncEnumerator();
            await HasNextAsync(e, (1, 4));
            await HasNextAsync(e, (2, 5));
            await HasNextAsync(e, (3, 6));
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Zip_Tuple_Throws_Right()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = Throw<int>(ex);
            var res = xs.Zip(ys);

            var e = res.GetAsyncEnumerator();
            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        [Fact]
        public async Task Zip_Tuple_Throws_Left()
        {
            var ex = new Exception("Bang!");
            var xs = Throw<int>(ex);
            var ys = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var res = xs.Zip(ys);

            var e = res.GetAsyncEnumerator();
            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        [Fact]
        public async Task Zip_Tuple_SequenceIdentity()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = new[] { 4, 5, 6 }.ToAsyncEnumerable();
            var res = xs.Zip(ys);

            await SequenceIdentity(res);
        }
#endif
    }
}

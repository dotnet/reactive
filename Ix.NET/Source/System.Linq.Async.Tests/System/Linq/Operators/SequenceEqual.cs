// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class SequenceEqual : AsyncEnumerableTests
    {
        [Fact]
        public async Task SequenceEqualAsync_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SequenceEqualAsync(default, Return42).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SequenceEqualAsync(Return42, default).AsTask());

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SequenceEqualAsync(default, Return42, new Eq()).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SequenceEqualAsync(Return42, default, new Eq()).AsTask());

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SequenceEqualAsync(default, Return42, CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SequenceEqualAsync(Return42, default, CancellationToken.None).AsTask());

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SequenceEqualAsync(default, Return42, new Eq(), CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SequenceEqualAsync(Return42, default, new Eq(), CancellationToken.None).AsTask());
        }

        [Fact]
        public async Task SequenceEqualAsync_Same()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable().Select(x => x);
            var res = xs.SequenceEqualAsync(xs);
            Assert.True(await res);
        }

        [Fact]
        public async Task SequenceEqualAsync_Same_IList()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var res = xs.SequenceEqualAsync(xs);
            Assert.True(await res);
        }

        [Fact]
        public async Task SequenceEqualAsync_Same_Empty()
        {
            var xs = AsyncEnumerable.Empty<int>();
            var res = xs.SequenceEqualAsync(xs);
            Assert.True(await res);
        }

        [Fact]
        public async Task SequenceEqualAsync_NotSame()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable().Select(x => x);
            var ys = new[] { 1, 3, 2 }.ToAsyncEnumerable().Select(x => x);
            var res = xs.SequenceEqualAsync(ys);
            Assert.False(await res);
        }

        [Fact]
        public async Task SequenceEqualAsync_NotSame_IList()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = new[] { 1, 3, 2 }.ToAsyncEnumerable();
            var res = xs.SequenceEqualAsync(ys);
            Assert.False(await res);
        }

        [Fact]
        public async Task SequenceEqualAsync_NotSame_DifferentLength()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var res = xs.SequenceEqualAsync(ys);
            Assert.False(await res);
        }

        [Fact]
        public async Task SequenceEqualAsync_NotSame_DifferentLength_IList()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable().Select(x => x);
            var ys = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable().Select(x => x);
            var res = xs.SequenceEqualAsync(ys);
            Assert.False(await res);
        }

        [Fact]
        public async Task SequenceEqualAsync_Throws_Second()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = Throw<int>(ex);
            var res = xs.SequenceEqualAsync(ys);

            await AssertThrowsAsync(res, ex);
        }

        [Fact]
        public async Task SequenceEqualAsync_Throws_First()
        {
            var ex = new Exception("Bang!");
            var xs = Throw<int>(ex);
            var ys = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var res = xs.SequenceEqualAsync(ys);

            await AssertThrowsAsync(res, ex);
        }

        [Fact]
        public async Task SequenceEqualAsync_Comparer_Empty()
        {
            var xs = AsyncEnumerable.Empty<int>();
            var res = xs.SequenceEqualAsync(xs, new Eq());
            Assert.True(await res);
        }

        [Fact]
        public async Task SequenceEqualAsync_Comparer_Same1()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var res = xs.SequenceEqualAsync(xs, new Eq());
            Assert.True(await res);
        }

        [Fact]
        public async Task SequenceEqualAsync_Comparer_Same2()
        {
            var xs = new[] { 1, 2, -3, 4 }.ToAsyncEnumerable().Select(x => x);
            var ys = new[] { 1, -2, 3, 4 }.ToAsyncEnumerable().Select(x => x);
            var res = xs.SequenceEqualAsync(ys, new Eq());
            Assert.True(await res);
        }

        [Fact]
        public async Task SequenceEqualAsync_Comparer_NotSame()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable().Select(x => x);
            var ys = new[] { 1, 3, 2 }.ToAsyncEnumerable().Select(x => x);
            var res = xs.SequenceEqualAsync(ys, new Eq());
            Assert.False(await res);
        }

        [Fact]
        public async Task SequenceEqualAsync_Comparer_NotSame_IList()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = new[] { 1, 3, 2 }.ToAsyncEnumerable();
            var res = xs.SequenceEqualAsync(ys, new Eq());
            Assert.False(await res);
        }

        [Fact]
        public async Task SequenceEqualAsync_Comparer_NotSame_DifferentLength()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable().Select(x => x);
            var ys = new[] { 1, 2, 3 }.ToAsyncEnumerable().Select(x => x);
            var res = xs.SequenceEqualAsync(ys, new Eq());
            Assert.False(await res);
        }

        [Fact]
        public async Task SequenceEqualAsync_Comparer_NotSame_DifferentLength_IList()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var res = xs.SequenceEqualAsync(ys, new Eq());
            Assert.False(await res);
        }

        [Fact]
        public async Task SequenceEqualAsync_Comparer_Throws_Second()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = Throw<int>(ex);
            var res = xs.SequenceEqualAsync(ys, new Eq());

            await AssertThrowsAsync(res, ex);
        }

        [Fact]
        public async Task SequenceEqualAsync_Comparer_Throws_First()
        {
            var ex = new Exception("Bang!");
            var xs = Throw<int>(ex);
            var ys = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var res = xs.SequenceEqualAsync(ys, new Eq());

            await AssertThrowsAsync(res, ex);
        }

        [Fact]
        public async Task SequenceEqualAsync_Comparer_Same_IList()
        {
            var xs = new[] { 1, 2, -3, 4 }.ToAsyncEnumerable();
            var ys = new[] { 1, -2, 3, 4 }.ToAsyncEnumerable();
            var res = xs.SequenceEqualAsync(ys, new Eq());
            Assert.True(await res);
        }

        [Fact]
        public async Task SequenceEqualAsync_Comparer_Throws_Equals_IList()
        {
            var xs = new[] { 1, 2, -3, 4 }.ToAsyncEnumerable();
            var ys = new[] { 1, -2, 3, 4 }.ToAsyncEnumerable();
            await Assert.ThrowsAsync<NotImplementedException>(() => xs.SequenceEqualAsync(ys, new EqEx()).AsTask());
        }

        [Fact]
        public async Task SequenceEqualAsync_Comparer_Throws_Equals()
        {
            var xs = new[] { 1, 2, -3, 4 }.Select(x => x * 2).ToAsyncEnumerable();
            var ys = new[] { 1, -2, 3, 4 }.Select(x => x * 2).ToAsyncEnumerable();
            var res = xs.SequenceEqualAsync(ys, new EqEx());
            await AssertThrowsAsync<NotImplementedException>(res.AsTask());
        }

        private sealed class EqEx : IEqualityComparer<int>
        {
            public bool Equals(int x, int y)
            {
                throw new NotImplementedException();
            }

            public int GetHashCode(int obj)
            {
                throw new NotSupportedException();
            }
        }

        private sealed class Eq : IEqualityComparer<int>
        {
            public bool Equals(int x, int y)
            {
                return EqualityComparer<int>.Default.Equals(Math.Abs(x), Math.Abs(y));
            }

            public int GetHashCode(int obj)
            {
                return EqualityComparer<int>.Default.GetHashCode(Math.Abs(obj));
            }
        }
    }
}

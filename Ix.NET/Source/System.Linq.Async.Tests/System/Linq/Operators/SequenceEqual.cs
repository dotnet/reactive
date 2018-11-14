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
        public async Task SequenceEqual_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SequenceEqualAsync(default, Return42));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SequenceEqualAsync(Return42, default));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SequenceEqualAsync(default, Return42, new Eq()));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SequenceEqualAsync(Return42, default, new Eq()));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SequenceEqualAsync(default, Return42, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SequenceEqualAsync(Return42, default, CancellationToken.None));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SequenceEqualAsync(default, Return42, new Eq(), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SequenceEqualAsync(Return42, default, new Eq(), CancellationToken.None));
        }

        [Fact]
        public async Task SequenceEqual1Async()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var res = xs.SequenceEqualAsync(xs);
            Assert.True(await res);
        }

        [Fact]
        public async Task SequenceEqual2Async()
        {
            var xs = AsyncEnumerable.Empty<int>();
            var res = xs.SequenceEqualAsync(xs);
            Assert.True(await res);
        }

        [Fact]
        public async Task SequenceEqual3Async()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = new[] { 1, 3, 2 }.ToAsyncEnumerable();
            var res = xs.SequenceEqualAsync(ys);
            Assert.False(await res);
        }

        [Fact]
        public async Task SequenceEqual4Async()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var res = xs.SequenceEqualAsync(ys);
            Assert.False(await res);
        }

        [Fact]
        public async Task SequenceEqual5Async()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var res = xs.SequenceEqualAsync(ys);
            Assert.False(await res);
        }

        [Fact]
        public async Task SequenceEqual6Async()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = Throw<int>(ex);
            var res = xs.SequenceEqualAsync(ys);

            await AssertThrowsAsync(res, ex);
        }

        [Fact]
        public async Task SequenceEqual7Async()
        {
            var ex = new Exception("Bang!");
            var xs = Throw<int>(ex);
            var ys = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var res = xs.SequenceEqualAsync(ys);

            await AssertThrowsAsync(res, ex);
        }

        [Fact]
        public async Task SequenceEqual8Async()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var res = xs.SequenceEqualAsync(xs, new Eq());
            Assert.True(await res);
        }

        [Fact]
        public async Task SequenceEqual9Async()
        {
            var xs = AsyncEnumerable.Empty<int>();
            var res = xs.SequenceEqualAsync(xs, new Eq());
            Assert.True(await res);
        }

        [Fact]
        public async Task SequenceEqual10Async()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = new[] { 1, 3, 2 }.ToAsyncEnumerable();
            var res = xs.SequenceEqualAsync(ys, new Eq());
            Assert.False(await res);
        }

        [Fact]
        public async Task SequenceEqual11Async()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var res = xs.SequenceEqualAsync(ys, new Eq());
            Assert.False(await res);
        }

        [Fact]
        public async Task SequenceEqual12Async()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var res = xs.SequenceEqualAsync(ys, new Eq());
            Assert.False(await res);
        }

        [Fact]
        public async Task SequenceEqual13Async()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = Throw<int>(ex);
            var res = xs.SequenceEqualAsync(ys, new Eq());

            await AssertThrowsAsync(res, ex);
        }

        [Fact]
        public async Task SequenceEqual14Async()
        {
            var ex = new Exception("Bang!");
            var xs = Throw<int>(ex);
            var ys = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var res = xs.SequenceEqualAsync(ys, new Eq());

            await AssertThrowsAsync(res, ex);
        }

        [Fact]
        public async Task SequenceEqual15Async()
        {
            var xs = new[] { 1, 2, -3, 4 }.ToAsyncEnumerable();
            var ys = new[] { 1, -2, 3, 4 }.ToAsyncEnumerable();
            var res = xs.SequenceEqualAsync(ys, new Eq());
            Assert.True(await res);
        }

        [Fact]
        public async Task SequenceEqual16Async()
        {
            var xs = new[] { 1, 2, -3, 4 }.ToAsyncEnumerable();
            var ys = new[] { 1, -2, 3, 4 }.ToAsyncEnumerable();
            var res = xs.SequenceEqualAsync(ys, new EqEx());
            await AssertThrowsAsync<NotImplementedException>(res);
        }

        private sealed class EqEx : IEqualityComparer<int>
        {
            public bool Equals(int x, int y)
            {
                throw new NotImplementedException();
            }

            public int GetHashCode(int obj)
            {
                throw new NotImplementedException();
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

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
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SequenceEqual(default, Return42));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SequenceEqual(Return42, default));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SequenceEqual(default, Return42, new Eq()));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SequenceEqual(Return42, default, new Eq()));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SequenceEqual(default, Return42, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SequenceEqual(Return42, default, CancellationToken.None));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SequenceEqual(default, Return42, new Eq(), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.SequenceEqual(Return42, default, new Eq(), CancellationToken.None));
        }

        [Fact]
        public async Task SequenceEqual1Async()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var res = xs.SequenceEqual(xs);
            Assert.True(await res);
        }

        [Fact]
        public async Task SequenceEqual2Async()
        {
            var xs = AsyncEnumerable.Empty<int>();
            var res = xs.SequenceEqual(xs);
            Assert.True(await res);
        }

        [Fact]
        public async Task SequenceEqual3Async()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = new[] { 1, 3, 2 }.ToAsyncEnumerable();
            var res = xs.SequenceEqual(ys);
            Assert.False(await res);
        }

        [Fact]
        public async Task SequenceEqual4Async()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var res = xs.SequenceEqual(ys);
            Assert.False(await res);
        }

        [Fact]
        public async Task SequenceEqual5Async()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var res = xs.SequenceEqual(ys);
            Assert.False(await res);
        }

        [Fact]
        public void SequenceEqual6()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = Throw<int>(ex);
            var res = xs.SequenceEqual(ys);

            AssertThrowsAsync(res, ex);
        }

        [Fact]
        public void SequenceEqual7()
        {
            var ex = new Exception("Bang!");
            var xs = Throw<int>(ex);
            var ys = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var res = xs.SequenceEqual(ys);

            AssertThrowsAsync(res, ex);
        }

        [Fact]
        public async Task SequenceEqual8Async()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var res = xs.SequenceEqual(xs, new Eq());
            Assert.True(await res);
        }

        [Fact]
        public async Task SequenceEqual9Async()
        {
            var xs = AsyncEnumerable.Empty<int>();
            var res = xs.SequenceEqual(xs, new Eq());
            Assert.True(await res);
        }

        [Fact]
        public async Task SequenceEqual10Async()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = new[] { 1, 3, 2 }.ToAsyncEnumerable();
            var res = xs.SequenceEqual(ys, new Eq());
            Assert.False(await res);
        }

        [Fact]
        public async Task SequenceEqual11Async()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var res = xs.SequenceEqual(ys, new Eq());
            Assert.False(await res);
        }

        [Fact]
        public async Task SequenceEqual12Async()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var res = xs.SequenceEqual(ys, new Eq());
            Assert.False(await res);
        }

        [Fact]
        public void SequenceEqual13()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = Throw<int>(ex);
            var res = xs.SequenceEqual(ys, new Eq());

            AssertThrowsAsync(res, ex);
        }

        [Fact]
        public void SequenceEqual14()
        {
            var ex = new Exception("Bang!");
            var xs = Throw<int>(ex);
            var ys = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var res = xs.SequenceEqual(ys, new Eq());

            AssertThrowsAsync(res, ex);
        }

        [Fact]
        public async Task SequenceEqual15Async()
        {
            var xs = new[] { 1, 2, -3, 4 }.ToAsyncEnumerable();
            var ys = new[] { 1, -2, 3, 4 }.ToAsyncEnumerable();
            var res = xs.SequenceEqual(ys, new Eq());
            Assert.True(await res);
        }

        [Fact]
        public void SequenceEqual16()
        {
            var xs = new[] { 1, 2, -3, 4 }.ToAsyncEnumerable();
            var ys = new[] { 1, -2, 3, 4 }.ToAsyncEnumerable();
            var res = xs.SequenceEqual(ys, new EqEx());
            AssertThrowsAsync<NotImplementedException>(res);
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

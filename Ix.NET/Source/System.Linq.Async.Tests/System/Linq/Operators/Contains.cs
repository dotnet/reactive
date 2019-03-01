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
    public class Contains : AsyncEnumerableTests
    {
        [Fact]
        public async Task ContainsAsync_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ContainsAsync(default, 42).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ContainsAsync(default, 42, EqualityComparer<int>.Default).AsTask());

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ContainsAsync(default, 42, CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ContainsAsync(default, 42, EqualityComparer<int>.Default, CancellationToken.None).AsTask());
        }

        [Fact]
        public async Task ContainsAsync_Simple_ICollection_True()
        {
            var xs = new[] { 1, 2, 3, 4, 5 }.ToAsyncEnumerable();
            var ys = xs.ContainsAsync(3);
            Assert.True(await ys);
        }

        [Fact]
        public async Task ContainsAsync_Simple_ICollection_False()
        {
            var xs = new[] { 1, 2, 3, 4, 5 }.ToAsyncEnumerable();
            var ys = xs.ContainsAsync(6);
            Assert.False(await ys);
        }

        [Fact]
        public async Task ContainsAsync_Simple_True()
        {
            var xs = new[] { 1, 2, 3, 4, 5 }.Select(x => x).ToAsyncEnumerable();
            var ys = xs.ContainsAsync(3);
            Assert.True(await ys);
        }

        [Fact]
        public async Task ContainsAsync_Simple_False()
        {
            var xs = new[] { 1, 2, 3, 4, 5 }.Select(x => x).ToAsyncEnumerable();
            var ys = xs.ContainsAsync(6);
            Assert.False(await ys);
        }

        [Fact]
        public async Task ContainsAsync_Simple_Comparer_True()
        {
            var xs = new[] { 1, 2, 3, 4, 5 }.ToAsyncEnumerable();
            var ys = xs.ContainsAsync(-3, new Eq());
            Assert.True(await ys);
        }

        [Fact]
        public async Task ContainsAsync_Simple_Comparer_False()
        {
            var xs = new[] { 1, 2, 3, 4, 5 }.ToAsyncEnumerable();
            var ys = xs.ContainsAsync(-6, new Eq());
            Assert.False(await ys);
        }

        private sealed class Eq : IEqualityComparer<int>
        {
            public bool Equals(int x, int y)
            {
                return EqualityComparer<int>.Default.Equals(Math.Abs(x), Math.Abs(y));
            }

            public int GetHashCode(int obj)
            {
                throw new NotImplementedException();
            }
        }
    }
}

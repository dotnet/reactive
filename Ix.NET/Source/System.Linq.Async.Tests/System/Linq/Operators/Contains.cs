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
        public async Task Contains_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Contains<int>(default(IAsyncEnumerable<int>), 42));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Contains<int>(default(IAsyncEnumerable<int>), 42, EqualityComparer<int>.Default));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Contains<int>(AsyncEnumerable.Return(42), 42, null));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Contains<int>(default(IAsyncEnumerable<int>), 42, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Contains<int>(default(IAsyncEnumerable<int>), 42, EqualityComparer<int>.Default, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Contains<int>(AsyncEnumerable.Return(42), 42, null, CancellationToken.None));
        }

        [Fact]
        public void Contains1()
        {
            var xs = new[] { 1, 2, 3, 4, 5 }.ToAsyncEnumerable();
            var ys = xs.Contains(3);
            Assert.True(ys.Result);
        }

        [Fact]
        public void Contains2()
        {
            var xs = new[] { 1, 2, 3, 4, 5 }.ToAsyncEnumerable();
            var ys = xs.Contains(6);
            Assert.False(ys.Result);
        }

        [Fact]
        public void Contains3()
        {
            var xs = new[] { 1, 2, 3, 4, 5 }.ToAsyncEnumerable();
            var ys = xs.Contains(-3, new Eq());
            Assert.True(ys.Result);
        }

        [Fact]
        public void Contains4()
        {
            var xs = new[] { 1, 2, 3, 4, 5 }.ToAsyncEnumerable();
            var ys = xs.Contains(-6, new Eq());
            Assert.False(ys.Result);
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

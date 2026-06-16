// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class MinByWithTies : AsyncEnumerableExTests
    {
        [Fact]
        public async Task MinByWithTies_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerableEx.MinByWithTiesAsync(default(IAsyncEnumerable<int>), x => x).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerableEx.MinByWithTiesAsync(Return42, default(Func<int, int>)).AsTask());

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerableEx.MinByWithTiesAsync(default(IAsyncEnumerable<int>), x => x, Comparer<int>.Default).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerableEx.MinByWithTiesAsync(Return42, default(Func<int, int>), Comparer<int>.Default).AsTask());

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerableEx.MinByWithTiesAsync(default(IAsyncEnumerable<int>), x => x, CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerableEx.MinByWithTiesAsync(Return42, default(Func<int, int>), CancellationToken.None).AsTask());

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerableEx.MinByWithTiesAsync(default(IAsyncEnumerable<int>), x => x, Comparer<int>.Default, CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerableEx.MinByWithTiesAsync(Return42, default(Func<int, int>), Comparer<int>.Default, CancellationToken.None).AsTask());
        }

        [Fact]
        public async Task MinByWithTies1Async()
        {
            var xs = new[] { 3, 5, 7, 6, 4, 2 }.ToAsyncEnumerable().MinByWithTiesAsync(x => x / 2);
            var res = await xs;

            Assert.True(res.SequenceEqual([3, 2]));
        }

        [Fact]
        public async Task MinByWithTies2Async()
        {
            var xs = Array.Empty<int>().ToAsyncEnumerable().MinByWithTiesAsync(x => x / 2);

            await AssertThrowsAsync<InvalidOperationException>(xs.AsTask());
        }

        [Fact]
        public async Task MinByWithTies3Async()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 3, 5, 7, 6, 4, 2 }.ToAsyncEnumerable().MinByWithTiesAsync(x => { if (x == 3) throw ex; return x; });

            await AssertThrowsAsync(xs, ex);
        }

        [Fact]
        public async Task MinByWithTies4Async()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 3, 5, 7, 6, 4, 2 }.ToAsyncEnumerable().MinByWithTiesAsync(x => { if (x == 4) throw ex; return x; });

            await AssertThrowsAsync(xs, ex);
        }

        [Fact]
        public async Task MinByWithTies5Async()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 3, 5, 7, 6, 4, 2 }.ToAsyncEnumerable().Concat(Throw<int>(ex)).MinByWithTiesAsync(x => x, Comparer<int>.Default);

            await AssertThrowsAsync(xs, ex);
        }
    }
}

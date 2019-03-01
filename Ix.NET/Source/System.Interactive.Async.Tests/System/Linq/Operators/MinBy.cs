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
    public class MinBy : AsyncEnumerableExTests
    {
        [Fact]
        public async Task MinBy_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerableEx.MinByAsync(default(IAsyncEnumerable<int>), x => x).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerableEx.MinByAsync(Return42, default(Func<int, int>)).AsTask());

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerableEx.MinByAsync(default(IAsyncEnumerable<int>), x => x, Comparer<int>.Default).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerableEx.MinByAsync(Return42, default(Func<int, int>), Comparer<int>.Default).AsTask());

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerableEx.MinByAsync(default(IAsyncEnumerable<int>), x => x, CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerableEx.MinByAsync(Return42, default(Func<int, int>), CancellationToken.None).AsTask());

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerableEx.MinByAsync(default(IAsyncEnumerable<int>), x => x, Comparer<int>.Default, CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerableEx.MinByAsync(Return42, default(Func<int, int>), Comparer<int>.Default, CancellationToken.None).AsTask());
        }

        [Fact]
        public async Task MinBy1Async()
        {
            var xs = new[] { 3, 5, 7, 6, 4, 2 }.ToAsyncEnumerable().MinByAsync(x => x / 2);
            var res = await xs;

            Assert.True(res.SequenceEqual(new[] { 3, 2 }));
        }

        [Fact]
        public async Task MinBy2Async()
        {
            var xs = new int[0].ToAsyncEnumerable().MinByAsync(x => x / 2);

            await AssertThrowsAsync<InvalidOperationException>(xs.AsTask());
        }

        [Fact]
        public async Task MinBy3Async()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 3, 5, 7, 6, 4, 2 }.ToAsyncEnumerable().MinByAsync(x => { if (x == 3) throw ex; return x; });

            await AssertThrowsAsync(xs, ex);
        }

        [Fact]
        public async Task MinBy4Async()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 3, 5, 7, 6, 4, 2 }.ToAsyncEnumerable().MinByAsync(x => { if (x == 4) throw ex; return x; });

            await AssertThrowsAsync(xs, ex);
        }

        [Fact]
        public async Task MinBy5Async()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 3, 5, 7, 6, 4, 2 }.ToAsyncEnumerable().Concat(Throw<int>(ex)).MinByAsync(x => x, Comparer<int>.Default);

            await AssertThrowsAsync(xs, ex);
        }
    }
}

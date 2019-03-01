// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class DistinctUntilChanged : AsyncEnumerableExTests
    {
        [Fact]
        public void DistinctUntilChanged_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerableEx.DistinctUntilChanged(default(IAsyncEnumerable<int>)));

            Assert.Throws<ArgumentNullException>(() => AsyncEnumerableEx.DistinctUntilChanged(default, EqualityComparer<int>.Default));

            Assert.Throws<ArgumentNullException>(() => AsyncEnumerableEx.DistinctUntilChanged(default(IAsyncEnumerable<int>), x => x));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerableEx.DistinctUntilChanged(Return42, default(Func<int, int>)));

            Assert.Throws<ArgumentNullException>(() => AsyncEnumerableEx.DistinctUntilChanged(default(IAsyncEnumerable<int>), x => x, EqualityComparer<int>.Default));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerableEx.DistinctUntilChanged(Return42, default(Func<int, int>), EqualityComparer<int>.Default));
        }

        [Fact]
        public async Task DistinctUntilChanged1Async()
        {
            var xs = new[] { 1, 2, 2, 3, 4, 4, 4, 4, 5, 6, 6, 7, 3, 2, 2, 1, 1 }.ToAsyncEnumerable().DistinctUntilChanged();

            var e = xs.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 4);
            await HasNextAsync(e, 5);
            await HasNextAsync(e, 6);
            await HasNextAsync(e, 7);
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 1);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task DistinctUntilChanged2Async()
        {
            var xs = new[] { 1, 2, 3, 4, 3, 5, 2 }.ToAsyncEnumerable().DistinctUntilChanged(x => (x + 1) / 2);

            var e = xs.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 5);
            await HasNextAsync(e, 2);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task DistinctUntilChanged3Async()
        {
            var ex = new Exception("Bang!");
            var xs = new[] { 1, 2, 3, 4, 3, 5, 2 }.ToAsyncEnumerable().DistinctUntilChanged(x => { if (x == 4) throw ex; return x; });

            var e = xs.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);
            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        [Fact]
        public async Task DistinctUntilChanged4()
        {
            var xs = new[] { 1, 2, 2, 3, 4, 4, 4, 4, 5, 6, 6, 7, 3, 2, 2, 1, 1 }.ToAsyncEnumerable().DistinctUntilChanged();

            await SequenceIdentity(xs);
        }

        [Fact]
        public async Task DistinctUntilChanged5()
        {
            var xs = new[] { 1, 2, 3, 4, 3, 5, 2 }.ToAsyncEnumerable().DistinctUntilChanged(x => (x + 1) / 2);

            await SequenceIdentity(xs);
        }
    }
}

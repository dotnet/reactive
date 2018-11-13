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
    public class Concat : AsyncEnumerableExTests
    {
        [Fact]
        public void Concat_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerableEx.Concat<int>(default(IAsyncEnumerable<int>[])));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerableEx.Concat<int>(default(IEnumerable<IAsyncEnumerable<int>>)));
        }

        [Fact]
        public async Task Concat4Async()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = new[] { 4, 5 }.ToAsyncEnumerable();
            var zs = new[] { 6, 7, 8 }.ToAsyncEnumerable();

            var res = AsyncEnumerableEx.Concat(xs, ys, zs);

            var e = res.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 4);
            await HasNextAsync(e, 5);
            await HasNextAsync(e, 6);
            await HasNextAsync(e, 7);
            await HasNextAsync(e, 8);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Concat5Async()
        {
            var ex = new Exception("Bang");
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = new[] { 4, 5 }.ToAsyncEnumerable();
            var zs = Throw<int>(ex);

            var res = AsyncEnumerableEx.Concat(xs, ys, zs);

            var e = res.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 4);
            await HasNextAsync(e, 5);
            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        [Fact]
        public async Task Concat6Async()
        {
            var ex = new Exception("Bang");
            var res = AsyncEnumerableEx.Concat(ConcatXss(ex));

            var e = res.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 4);
            await HasNextAsync(e, 5);
            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        [Fact]
        public async Task Concat9()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();
            var ys = new[] { 4, 5 }.ToAsyncEnumerable();
            var zs = new[] { 6, 7, 8 }.ToAsyncEnumerable();

            var res = AsyncEnumerableEx.Concat(xs, ys, zs);

            await SequenceIdentity(res);
        }

        private static IEnumerable<IAsyncEnumerable<int>> ConcatXss(Exception ex)
        {
            yield return new[] { 1, 2, 3 }.ToAsyncEnumerable();
            yield return new[] { 4, 5 }.ToAsyncEnumerable();
            throw ex;
        }
    }
}

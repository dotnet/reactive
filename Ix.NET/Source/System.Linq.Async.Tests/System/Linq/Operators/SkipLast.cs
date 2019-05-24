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
    public class SkipLast : AsyncEnumerableTests
    {
        [Fact]
        public void SkipLast_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.SkipLast(default(IAsyncEnumerable<int>), 5));
        }

        [Fact]
        public async Task SkipLast_Few()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable().SkipLast(2);

            var e = xs.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task SkipLast_All()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable().SkipLast(5);

            var e = xs.GetAsyncEnumerator();
            await NoNextAsync(e);
        }

        [Fact]
        public async Task SkipLast_SequenceIdentity()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable().SkipLast(2);

            await SequenceIdentity(xs);
        }

        [Fact]
        public async Task SkipLast_Zero()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.SkipLast(0);

            Assert.Same(xs, ys);

            var e = ys.GetAsyncEnumerator();

            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 4);
            await NoNextAsync(e);
        }

        [Fact]
        public void SkipLast_Zero_NoAlias()
        {
            var xs = Xs();
            var ys = xs.SkipLast(0);

            Assert.NotSame(xs, ys);
        }

        private async IAsyncEnumerable<int> Xs()
        {
            await Task.Yield();
            yield return 1;
        }
    }
}

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
    public class Retry : AsyncEnumerableExTests
    {
        [Fact]
        public void Retry_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerableEx.Retry<int>(default));

            Assert.Throws<ArgumentNullException>(() => AsyncEnumerableEx.Retry<int>(default, 1));
            Assert.Throws<ArgumentOutOfRangeException>(() => AsyncEnumerableEx.Retry<int>(Return42, -1));
        }

        [Fact]
        public async Task Retry1Async()
        {
            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable();

            var res = xs.Retry();

            var e = res.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Retry2Async()
        {
            var ex = new InvalidOperationException("Bang!");

            var xs = new[] { 1, 2, 3 }.ToAsyncEnumerable().Concat(Throw<int>(ex));

            var res = xs.Retry();

            var e = res.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);
        }
    }
}

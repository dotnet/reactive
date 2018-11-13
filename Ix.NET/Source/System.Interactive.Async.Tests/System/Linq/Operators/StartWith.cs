// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class StartWith : AsyncEnumerableExTests
    {
        [Fact]
        public void StartWith_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerableEx.StartWith(default, new[] { 1 }));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerableEx.StartWith(Return42, default));
        }

        [Fact]
        public async Task StartWith1Async()
        {
            var xs = AsyncEnumerable.Empty<int>().StartWith(1, 2);

            var e = xs.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task StartWith2Async()
        {
            var xs = Return42.StartWith(40, 41);

            var e = xs.GetAsyncEnumerator();
            await HasNextAsync(e, 40);
            await HasNextAsync(e, 41);
            await HasNextAsync(e, 42);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task StartWith3Async()
        {
            var ex = new Exception("Bang!");
            var xs = Throw<int>(ex).StartWith(1, 2);

            var e = xs.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class Range : AsyncEnumerableTests
    {
        [Fact]
        public void Range_Null()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => AsyncEnumerable.Range(0, -1));
        }

        [Fact]
        public async Task Range1Async()
        {
            var xs = AsyncEnumerable.Range(2, 5);

            var e = xs.GetAsyncEnumerator();
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 4);
            await HasNextAsync(e, 5);
            await HasNextAsync(e, 6);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task Range2Async()
        {
            var xs = AsyncEnumerable.Range(2, 0);

            var e = xs.GetAsyncEnumerator();
            await NoNextAsync(e);
        }
    }
}

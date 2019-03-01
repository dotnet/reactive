// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class SelectMany : AsyncEnumerableExTests
    {
        [Fact]
        public void SelectMany_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerableEx.SelectMany<int, int>(default, Return42));
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerableEx.SelectMany<int, int>(Return42, default));
        }

        [Fact]
        public async Task SelectMany1Async()
        {
            var xs = new[] { 0, 1, 2 }.ToAsyncEnumerable();
            var ys = new[] { 3, 4 }.ToAsyncEnumerable();

            var res = xs.SelectMany(ys);

            var e = res.GetAsyncEnumerator();
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 4);
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 4);
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 4);
            await NoNextAsync(e);
        }
    }
}

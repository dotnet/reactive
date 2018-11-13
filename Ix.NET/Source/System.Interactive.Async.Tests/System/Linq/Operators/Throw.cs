// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class Throw : AsyncEnumerableExTests
    {
        [Fact]
        public async Task Throw1()
        {
            var ex = new Exception("Bang!");
            var xs = AsyncEnumerableEx.Throw<int>(ex);
            var e = xs.GetAsyncEnumerator();
            await AssertThrowsAsync(e.MoveNextAsync(), ex);
            Assert.False(await e.MoveNextAsync());
        }
    }
}

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
    public class Defer : AsyncEnumerableExTests
    {
        [Fact]
        public void Defer_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerableEx.Defer<int>(default(Func<IAsyncEnumerable<int>>)));
        }

        [Fact]
        public async Task Defer1Async()
        {
            var x = 0;
            var xs = AsyncEnumerableEx.Defer<int>(() => new[] { x }.ToAsyncEnumerable());

            {
                var e = xs.GetAsyncEnumerator();
                await HasNextAsync(e, 0);
                await NoNextAsync(e);
            }

            {
                x++;
                var e = xs.GetAsyncEnumerator();
                await HasNextAsync(e, 1);
                await NoNextAsync(e);
            }
        }
    }
}

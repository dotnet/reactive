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
    public class Cast : AsyncEnumerableTests
    {
        [Fact]
        public void Cast_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.Cast<int>(default));
        }

        [Fact]
        public async Task Cast1()
        {
            var xs = new object[] { 1, 2, 3, 4 }.ToAsyncEnumerable();
            var ys = xs.Cast<int>();

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 4);
            await NoNextAsync(e);
        }

        [Fact]
        public void Cast2()
        {
            var xs = new[] { new EventArgs(), new EventArgs(), new EventArgs() }.ToAsyncEnumerable();
            var ys = xs.Cast<EventArgs>();

            Assert.Same(xs, ys);
        }
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class OfType : AsyncEnumerableTests
    {
        [Fact]
        public void OfType_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.OfType<int>(default));
        }

        [Fact]
        public async Task OfType_Int32()
        {
            var xs = new object[] { 1, 1.2, true, 4, "" }.ToAsyncEnumerable();
            var ys = xs.OfType<int>();

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 4);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task OfType_String()
        {
            var xs = new object[] { "bar", 42, "foo", 43 }.ToAsyncEnumerable();
            var ys = xs.OfType<string>();

            var e = ys.GetAsyncEnumerator();
            await HasNextAsync(e, "bar");
            await HasNextAsync(e, "foo");
            await NoNextAsync(e);
        }
    }
}

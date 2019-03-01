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
    public class TakeLast : AsyncEnumerableTests
    {
        [Fact]
        public void TakeLast_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerable.TakeLast(default(IAsyncEnumerable<int>), 5));
        }

        [Fact]
        public async Task TakeLast_Negative()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable().TakeLast(-2);

            var e = xs.GetAsyncEnumerator();
            await NoNextAsync(e);
        }

        [Fact]
        public async Task TakeLast_Positive()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable().TakeLast(2);

            var e = xs.GetAsyncEnumerator();
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 4);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task TakeLast_TooMany()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable().TakeLast(5);

            var e = xs.GetAsyncEnumerator();
            await HasNextAsync(e, 1);
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);
            await HasNextAsync(e, 4);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task TakeLast_BreakEarly_Take()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable().TakeLast(3).Take(2);

            var e = xs.GetAsyncEnumerator();
            await HasNextAsync(e, 2);
            await HasNextAsync(e, 3);
            await NoNextAsync(e);
        }

        [Fact]
        public async Task TakeLast_SequenceIdentity()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable().TakeLast(2);

            await SequenceIdentity(xs);
        }

        [Fact]
        public async Task TakeLast_BugFix_TakeLast_Zero_TakesForever()
        {
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable().TakeLast(0);

            var e = xs.GetAsyncEnumerator();

            await NoNextAsync(e);
        }
    }
}

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
    public class IgnoreElements : AsyncEnumerableExTests
    {
        [Fact]
        public void IgnoreElements_Null()
        {
            Assert.Throws<ArgumentNullException>(() => AsyncEnumerableEx.IgnoreElements(default(IAsyncEnumerable<int>)));
        }

        [Fact]
        public async Task IgnoreElements1Async()
        {
            var xs = AsyncEnumerable.Empty<int>().IgnoreElements();

            var e = xs.GetAsyncEnumerator();
            await NoNextAsync(e);
        }

        [Fact]
        public async Task IgnoreElements2Async()
        {
            var xs = Return42.IgnoreElements();

            var e = xs.GetAsyncEnumerator();
            await NoNextAsync(e);
        }

        [Fact]
        public async Task IgnoreElements3Async()
        {
            var xs = AsyncEnumerable.Range(0, 10).IgnoreElements();

            var e = xs.GetAsyncEnumerator();
            await NoNextAsync(e);
        }

        [Fact]
        public async Task IgnoreElements4Async()
        {
            var ex = new Exception("Bang!");
            var xs = Throw<int>(ex).IgnoreElements();

            var e = xs.GetAsyncEnumerator();
            await AssertThrowsAsync(e.MoveNextAsync(), ex);
        }

        [Fact]
        public async Task IgnoreElements5()
        {
            var xs = AsyncEnumerable.Range(0, 10).IgnoreElements();

            await SequenceIdentity(xs);
        }
    }
}

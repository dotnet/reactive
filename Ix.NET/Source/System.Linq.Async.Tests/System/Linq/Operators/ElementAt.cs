// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class ElementAt : AsyncEnumerableTests
    {
        [Fact]
        public async Task ElementAt_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ElementAt<int>(default, 0));
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => AsyncEnumerable.ElementAt(Return42, -1));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ElementAt<int>(default, 0, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => AsyncEnumerable.ElementAt(Return42, -1, CancellationToken.None));
        }

        [Fact]
        public async Task ElementAt1Async()
        {
            var res = AsyncEnumerable.Empty<int>().ElementAt(0);
            await AssertThrowsAsync<ArgumentOutOfRangeException>(res);
        }

        [Fact]
        public async Task ElementAt2Async()
        {
            var res = Return42.ElementAt(0);
            Assert.Equal(42, await res);
        }

        [Fact]
        public async Task ElementAt3Async()
        {
            var res = Return42.ElementAt(1);
            await AssertThrowsAsync<ArgumentOutOfRangeException>(res);
        }

        [Fact]
        public async Task ElementAt4Async()
        {
            var res = new[] { 1, 42, 3 }.ToAsyncEnumerable().ElementAt(1);
            Assert.Equal(42, await res);
        }

        [Fact]
        public async Task ElementAt5Async()
        {
            var res = new[] { 1, 42, 3 }.ToAsyncEnumerable().ElementAt(7);
            await AssertThrowsAsync<ArgumentOutOfRangeException>(res);
        }

        [Fact]
        public async Task ElementAt6Async()
        {
            var ex = new Exception("Bang!");
            var res = Throw<int>(ex).ElementAt(15);
            await AssertThrowsAsync(res, ex);
        }
    }
}

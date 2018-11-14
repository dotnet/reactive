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
    public class ElementAtOrDefault : AsyncEnumerableTests
    {
        [Fact]
        public async Task ElementAtOrDefault_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ElementAtOrDefaultAsync<int>(default, 0));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ElementAtOrDefaultAsync<int>(default, 0, CancellationToken.None));
        }

        [Fact]
        public async Task ElementAtOrDefault1Async()
        {
            var res = AsyncEnumerable.Empty<int>().ElementAtOrDefaultAsync(0);
            Assert.Equal(0, await res);
        }

        [Fact]
        public async Task ElementAtOrDefault2Async()
        {
            var res = Return42.ElementAtOrDefaultAsync(0);
            Assert.Equal(42, await res);
        }

        [Fact]
        public async Task ElementAtOrDefault3Async()
        {
            var res = Return42.ElementAtOrDefaultAsync(1);
            Assert.Equal(0, await res);
        }

        [Fact]
        public async Task ElementAtOrDefault4Async()
        {
            var res = new[] { 1, 42, 3 }.ToAsyncEnumerable().ElementAtOrDefaultAsync(1);
            Assert.Equal(42, await res);
        }

        [Fact]
        public async Task ElementAtOrDefault5Async()
        {
            var res = new[] { 1, 42, 3 }.ToAsyncEnumerable().ElementAtOrDefaultAsync(7);
            Assert.Equal(0, await res);
        }

        [Fact]
        public async Task ElementAtOrDefault6Async()
        {
            var res = Return42.ElementAtOrDefaultAsync(-1);
            Assert.Equal(0, await res);
        }

        [Fact]
        public async Task ElementAtOrDefault7Async()
        {
            var ex = new Exception("Bang!");
            var res = Throw<int>(ex).ElementAtOrDefaultAsync(15);
            await AssertThrowsAsync(res, ex);
        }
    }
}

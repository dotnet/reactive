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
        public async Task ElementAtOrDefaultAsync_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ElementAtOrDefaultAsync<int>(default, 0).AsTask());
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ElementAtOrDefaultAsync<int>(default, 0, CancellationToken.None).AsTask());
        }

        [Fact]
        public async Task ElementAtOrDefaultAsync_Empty_Index0()
        {
            var res = AsyncEnumerable.Empty<int>().ElementAtOrDefaultAsync(0);
            Assert.Equal(0, await res);
        }

        [Fact]
        public async Task ElementAtOrDefaultAsync_Single_Index0()
        {
            var res = Return42.ElementAtOrDefaultAsync(0);
            Assert.Equal(42, await res);
        }

        [Fact]
        public async Task ElementAtOrDefaultAsync_Single_OutOfRange()
        {
            var res = Return42.ElementAtOrDefaultAsync(1);
            Assert.Equal(0, await res);
        }

        [Fact]
        public async Task ElementAtOrDefaultAsync_Single_NegativeIndex()
        {
            var res = Return42.ElementAtOrDefaultAsync(-1);
            Assert.Equal(0, await res);
        }

        [Fact]
        public async Task ElementAtOrDefaultAsync_Many_IList_InRange()
        {
            var res = new[] { 1, 42, 3 }.ToAsyncEnumerable().ElementAtOrDefaultAsync(1);
            Assert.Equal(42, await res);
        }

        [Fact]
        public async Task ElementAtOrDefaultAsync_Many_IList_OutOfRange()
        {
            var res = new[] { 1, 42, 3 }.ToAsyncEnumerable().ElementAtOrDefaultAsync(7);
            Assert.Equal(0, await res);
        }

        [Fact]
        public async Task ElementAtOrDefaultAsync_Many_IPartition_InRange()
        {
            var res = new[] { -1, 1, 42, 3 }.ToAsyncEnumerable().Skip(1).ElementAtOrDefaultAsync(1);
            Assert.Equal(42, await res);
        }

        [Fact]
        public async Task ElementAtOrDefaultAsync_Many_IPartition_OutOfRange()
        {
            var res = new[] { -1, 1, 42, 3 }.ToAsyncEnumerable().Skip(1).ElementAtOrDefaultAsync(7);
            Assert.Equal(0, await res);
        }

        [Fact]
        public async Task ElementAtOrDefaultAsync_Many_InRange()
        {
            var res = new[] { 1, 42, 3 }.ToAsyncEnumerable().Select(x => x).ElementAtOrDefaultAsync(1);
            Assert.Equal(42, await res);
        }

        [Fact]
        public async Task ElementAtOrDefaultAsync_Many_OutOfRange()
        {
            var res = new[] { 1, 42, 3 }.ToAsyncEnumerable().Select(x => x).ElementAtOrDefaultAsync(7);
            Assert.Equal(0, await res);
        }

        [Fact]
        public async Task ElementAtOrDefaultAsync_Throws_Source()
        {
            var ex = new Exception("Bang!");
            var res = Throw<int>(ex).ElementAtOrDefaultAsync(15);
            await AssertThrowsAsync(res, ex);
        }
    }
}

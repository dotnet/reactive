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
        public async Task ElementAtAsync_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ElementAtAsync<int>(default, 0).AsTask());
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => AsyncEnumerable.ElementAtAsync(Return42, -1).AsTask());

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ElementAtAsync<int>(default, 0, CancellationToken.None).AsTask());
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => AsyncEnumerable.ElementAtAsync(Return42, -1, CancellationToken.None).AsTask());
        }

        [Fact]
        public async Task ElementAtAsync_Empty_Index0()
        {
            var res = AsyncEnumerable.Empty<int>().ElementAtAsync(0);
            await AssertThrowsAsync<ArgumentOutOfRangeException>(res.AsTask());
        }

        [Fact]
        public async Task ElementAtAsync_Single_Index0()
        {
            var res = Return42.ElementAtAsync(0);
            Assert.Equal(42, await res);
        }

        [Fact]
        public async Task ElementAtAsync_Single_Index1()
        {
            var res = Return42.ElementAtAsync(1);
            await AssertThrowsAsync<ArgumentOutOfRangeException>(res.AsTask());
        }

        [Fact]
        public async Task ElementAtAsync_Many_IList_InRange()
        {
            var res = new[] { 1, 42, 3 }.ToAsyncEnumerable().ElementAtAsync(1);
            Assert.Equal(42, await res);
        }

        [Fact]
        public async Task ElementAtAsync_Many_IList_OutOfRange()
        {
            var res = new[] { 1, 42, 3 }.ToAsyncEnumerable().ElementAtAsync(7);
            await AssertThrowsAsync<ArgumentOutOfRangeException>(res.AsTask());
        }

        [Fact]
        public async Task ElementAtAsync_Many_IPartition_InRange()
        {
            var res = new[] { -1, 1, 42, 3 }.ToAsyncEnumerable().Skip(1).ElementAtAsync(1);
            Assert.Equal(42, await res);
        }

        [Fact]
        public async Task ElementAtAsync_Many_IPartition_OutOfRange()
        {
            var res = new[] { -1, 1, 42, 3 }.ToAsyncEnumerable().Skip(1).ElementAtAsync(7);
            await AssertThrowsAsync<ArgumentOutOfRangeException>(res.AsTask());
        }

        [Fact]
        public async Task ElementAtAsync_Many_InRange()
        {
            var res = new[] { 1, 42, 3 }.ToAsyncEnumerable().Select(x => x).ElementAtAsync(1);
            Assert.Equal(42, await res);
        }

        [Fact]
        public async Task ElementAtAsync_Many_OutOfRange()
        {
            var res = new[] { 1, 42, 3 }.ToAsyncEnumerable().Select(x => x).ElementAtAsync(7);
            await AssertThrowsAsync<ArgumentOutOfRangeException>(res.AsTask());
        }

        [Fact]
        public async Task ElementAtAsync_Throws_Source()
        {
            var ex = new Exception("Bang!");
            var res = Throw<int>(ex).ElementAtAsync(15);
            await AssertThrowsAsync(res, ex);
        }
    }
}

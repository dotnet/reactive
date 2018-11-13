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
    public class All : AsyncEnumerableTests
    {
        [Fact]
        public async Task All_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.All<int>(default, x => true));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.All(Return42, default(Func<int, bool>)));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.All<int>(default, x => true, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.All(Return42, default(Func<int, bool>), CancellationToken.None));
        }

        [Fact]
        public async Task All1Async()
        {
            var res = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable().All(x => x % 2 == 0);
            Assert.False(await res);
        }

        [Fact]
        public async Task All2Async()
        {
            var res = new[] { 2, 8, 4 }.ToAsyncEnumerable().All(x => x % 2 == 0);
            Assert.True(await res);
        }

        [Fact]
        public void All3()
        {
            var ex = new Exception("Bang!");
            var res = Throw<int>(ex).All(x => x % 2 == 0);
            AssertThrowsAsync(res, ex);
        }

        [Fact]
        public void All4()
        {
            var ex = new Exception("Bang!");
            var res = new[] { 2, 8, 4 }.ToAsyncEnumerable().All(new Func<int, bool>(x => { throw ex; }));
            AssertThrowsAsync(res, ex);
        }
    }
}

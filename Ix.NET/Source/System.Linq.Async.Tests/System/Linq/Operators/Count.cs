// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class Count : AsyncEnumerableTests
    {
        [Fact]
        public async Task Count_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Count<int>(default));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Count<int>(default, x => true));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Count(Return42, default(Func<int, bool>)));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Count<int>(default, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Count<int>(default, x => true, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Count(Return42, default(Func<int, bool>), CancellationToken.None));
        }

        [Fact]
        public async Task Count1()
        {
            Assert.Equal(0, await new int[0].ToAsyncEnumerable().Count());
            Assert.Equal(3, await new[] { 1, 2, 3 }.ToAsyncEnumerable().Count());
        }

        [Fact]
        public async Task Count2()
        {
            Assert.Equal(0, await new int[0].ToAsyncEnumerable().Count(x => x < 3));
            Assert.Equal(2, await new[] { 1, 2, 3 }.ToAsyncEnumerable().Count(x => x < 3));
        }

        [Fact]
        public void Count3()
        {
            var ex = new Exception("Bang!");
            var ys = new[] { 1, 2, 3 }.ToAsyncEnumerable().Count(new Func<int, bool>(x => { throw ex; }));
            AssertThrowsAsync(ys, ex);
        }

        [Fact]
        public void Count4()
        {
            var ex = new Exception("Bang!");
            AssertThrowsAsync(Throw<int>(ex).Count(), ex);
        }

        [Fact]
        public void Count5()
        {
            var ex = new Exception("Bang!");
            AssertThrowsAsync(Throw<int>(ex).Count(x => x < 3), ex);
        }
    }
}

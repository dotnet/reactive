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
    public class Any : AsyncEnumerableTests
    {
        [Fact]
        public async Task Any_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Any<int>(default));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Any<int>(default, x => true));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Any(Return42, default(Func<int, bool>)));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Any<int>(default, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Any<int>(default, x => true, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Any(Return42, default(Func<int, bool>), CancellationToken.None));
        }

        [Fact]
        public void Any1()
        {
            var res = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable().Any(x => x % 2 == 0);
            Assert.True(res.Result);
        }

        [Fact]
        public void Any2()
        {
            var res = new[] { 2, 8, 4 }.ToAsyncEnumerable().Any(x => x % 2 != 0);
            Assert.False(res.Result);
        }

        [Fact]
        public void Any3()
        {
            var ex = new Exception("Bang!");
            var res = Throw<int>(ex).Any(x => x % 2 == 0);
            AssertThrowsAsync(res, ex);
        }

        [Fact]
        public void Any4()
        {
            var ex = new Exception("Bang!");
            var res = new[] { 2, 8, 4 }.ToAsyncEnumerable().Any(new Func<int, bool>(x => { throw ex; }));
            AssertThrowsAsync(res, ex);
        }

        [Fact]
        public void Any5()
        {
            var res = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable().Any();
            Assert.True(res.Result);
        }

        [Fact]
        public void Any6()
        {
            var res = new int[0].ToAsyncEnumerable().Any();
            Assert.False(res.Result);
        }
    }
}

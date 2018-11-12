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
    public class LastOrDefault : AsyncEnumerableTests
    {
        [Fact]
        public async Task LastOrDefault_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.LastOrDefault<int>(default));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.LastOrDefault<int>(default, x => true));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.LastOrDefault(Return42, default(Func<int, bool>)));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.LastOrDefault<int>(default, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.LastOrDefault<int>(default, x => true, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.LastOrDefault(Return42, default(Func<int, bool>), CancellationToken.None));
        }

        [Fact]
        public void LastOrDefault1()
        {
            var res = AsyncEnumerable.Empty<int>().LastOrDefault();
            Assert.Equal(0, res.Result);
        }

        [Fact]
        public void LastOrDefault2()
        {
            var res = AsyncEnumerable.Empty<int>().LastOrDefault(x => true);
            Assert.Equal(0, res.Result);
        }

        [Fact]
        public void LastOrDefault3()
        {
            var res = Return42.LastOrDefault(x => x % 2 != 0);
            Assert.Equal(0, res.Result);
        }

        [Fact]
        public void LastOrDefault4()
        {
            var res = Return42.LastOrDefault();
            Assert.Equal(42, res.Result);
        }

        [Fact]
        public void LastOrDefault5()
        {
            var res = Return42.LastOrDefault(x => x % 2 == 0);
            Assert.Equal(42, res.Result);
        }

        [Fact]
        public void LastOrDefault6()
        {
            var ex = new Exception("Bang!");
            var res = Throw<int>(ex).LastOrDefault();
            AssertThrows(() => res.Wait(WaitTimeoutMs), SingleInnerExceptionMatches(ex));
        }

        [Fact]
        public void LastOrDefault7()
        {
            var ex = new Exception("Bang!");
            var res = Throw<int>(ex).LastOrDefault(x => true);
            AssertThrows(() => res.Wait(WaitTimeoutMs), SingleInnerExceptionMatches(ex));
        }

        [Fact]
        public void LastOrDefault8()
        {
            var res = new[] { 42, 45, 90 }.ToAsyncEnumerable().LastOrDefault();
            Assert.Equal(90, res.Result);
        }

        [Fact]
        public void LastOrDefault9()
        {
            var res = new[] { 42, 23, 45, 90 }.ToAsyncEnumerable().LastOrDefault(x => x % 2 != 0);
            Assert.Equal(45, res.Result);
        }

        [Fact]
        public void LastOrDefault10()
        {
            var res = new[] { 42, 45, 90 }.ToAsyncEnumerable().LastOrDefault(x => x < 10);
            Assert.Equal(0, res.Result);
        }
    }
}

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
    public class First : AsyncEnumerableTests
    {
        [Fact]
        public async Task First_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.First<int>(default));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.First<int>(default, x => true));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.First<int>(Return42, default(Func<int, bool>)));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.First<int>(default, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.First<int>(default, x => true, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.First<int>(Return42, default(Func<int, bool>), CancellationToken.None));
        }

        [Fact]
        public void First1()
        {
            var res = AsyncEnumerable.Empty<int>().First();
            AssertThrows<Exception>(() => res.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is InvalidOperationException);
        }

        [Fact]
        public void First2()
        {
            var res = AsyncEnumerable.Empty<int>().First(x => true);
            AssertThrows<Exception>(() => res.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is InvalidOperationException);
        }

        [Fact]
        public void First3()
        {
            var res = Return42.First(x => x % 2 != 0);
            AssertThrows<Exception>(() => res.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() is InvalidOperationException);
        }

        [Fact]
        public void First4()
        {
            var res = Return42.First();
            Assert.Equal(42, res.Result);
        }

        [Fact]
        public void First5()
        {
            var res = Return42.First(x => x % 2 == 0);
            Assert.Equal(42, res.Result);
        }

        [Fact]
        public void First6()
        {
            var ex = new Exception("Bang!");
            var res = Throw<int>(ex).First();
            AssertThrows<Exception>(() => res.Wait(WaitTimeoutMs), SingleInnerExceptionMatches(ex));
        }

        [Fact]
        public void First7()
        {
            var ex = new Exception("Bang!");
            var res = Throw<int>(ex).First(x => true);
            AssertThrows<Exception>(() => res.Wait(WaitTimeoutMs), SingleInnerExceptionMatches(ex));
        }

        [Fact]
        public void First8()
        {
            var res = new[] { 42, 45, 90 }.ToAsyncEnumerable().First();
            Assert.Equal(42, res.Result);
        }

        [Fact]
        public void First9()
        {
            var res = new[] { 42, 45, 90 }.ToAsyncEnumerable().First(x => x % 2 != 0);
            Assert.Equal(45, res.Result);
        }
    }
}

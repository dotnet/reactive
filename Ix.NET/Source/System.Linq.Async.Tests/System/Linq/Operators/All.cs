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
    public class All : AsyncEnumerableTests
    {
        [Fact]
        public async Task All_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.All<int>(default(IAsyncEnumerable<int>), x => true));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.All<int>(Return42, default(Func<int, bool>)));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.All<int>(default(IAsyncEnumerable<int>), x => true, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.All<int>(Return42, default(Func<int, bool>), CancellationToken.None));
        }

        [Fact]
        public void All1()
        {
            var res = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable().All(x => x % 2 == 0);
            Assert.False(res.Result);
        }

        [Fact]
        public void All2()
        {
            var res = new[] { 2, 8, 4 }.ToAsyncEnumerable().All(x => x % 2 == 0);
            Assert.True(res.Result);
        }

        [Fact]
        public void All3()
        {
            var ex = new Exception("Bang!");
            var res = Throw<int>(ex).All(x => x % 2 == 0);
            AssertThrows<Exception>(() => res.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void All4()
        {
            var ex = new Exception("Bang!");
            var res = new[] { 2, 8, 4 }.ToAsyncEnumerable().All(new Func<int, bool>(x => { throw ex; }));
            AssertThrows<Exception>(() => res.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }
    }
}

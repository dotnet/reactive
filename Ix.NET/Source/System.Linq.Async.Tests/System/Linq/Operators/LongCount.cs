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
    public class LongCount : AsyncEnumerableTests
    {
        [Fact]
        public async Task LongCount_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.LongCount<int>(default(IAsyncEnumerable<int>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.LongCount<int>(default(IAsyncEnumerable<int>), x => true));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.LongCount<int>(Return42, default(Func<int, bool>)));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.LongCount<int>(default(IAsyncEnumerable<int>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.LongCount<int>(default(IAsyncEnumerable<int>), x => true, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.LongCount<int>(Return42, default(Func<int, bool>), CancellationToken.None));
        }

        [Fact]
        public void LongCount1()
        {
            Assert.Equal(0, new int[0].ToAsyncEnumerable().LongCount().Result);
            Assert.Equal(3, new[] { 1, 2, 3 }.ToAsyncEnumerable().LongCount().Result);
            AssertThrows<AggregateException>(() => AsyncEnumerable.Throw<int>(new Exception("Bang!")).LongCount().Wait(WaitTimeoutMs));
        }

        [Fact]
        public void LongCount2()
        {
            Assert.Equal(0, new int[0].ToAsyncEnumerable().LongCount(x => x < 3).Result);
            Assert.Equal(2, new[] { 1, 2, 3 }.ToAsyncEnumerable().LongCount(x => x < 3).Result);
            AssertThrows<AggregateException>(() => AsyncEnumerable.Throw<int>(new Exception("Bang!")).LongCount(x => x < 3).Wait(WaitTimeoutMs));
        }

        [Fact]
        public void LongCount3()
        {
            var ex = new Exception("Bang!");
            var ys = new[] { 1, 2, 3 }.ToAsyncEnumerable().LongCount(new Func<int, bool>(x => { throw ex; }));
            AssertThrows<Exception>(() => ys.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }
    }
}

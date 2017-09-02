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
    public class Count : AsyncEnumerableTests
    {
        [Fact]
        public async Task Count_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Count<int>(null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Count<int>(null, x => true));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Count<int>(AsyncEnumerable.Return(42), default(Func<int, bool>)));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Count<int>(null, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Count<int>(null, x => true, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.Count<int>(AsyncEnumerable.Return(42), default(Func<int, bool>), CancellationToken.None));
        }

        [Fact]
        public void Count1()
        {
            Assert.Equal(0, new int[0].ToAsyncEnumerable().Count().Result);
            Assert.Equal(3, new[] { 1, 2, 3 }.ToAsyncEnumerable().Count().Result);
            AssertThrows<AggregateException>(() => AsyncEnumerable.Throw<int>(new Exception("Bang!")).Count().Wait(WaitTimeoutMs));
        }

        [Fact]
        public void Count2()
        {
            Assert.Equal(0, new int[0].ToAsyncEnumerable().Count(x => x < 3).Result);
            Assert.Equal(2, new[] { 1, 2, 3 }.ToAsyncEnumerable().Count(x => x < 3).Result);
            AssertThrows<AggregateException>(() => AsyncEnumerable.Throw<int>(new Exception("Bang!")).Count(x => x < 3).Wait(WaitTimeoutMs));
        }

        [Fact]
        public void Count3()
        {
            var ex = new Exception("Bang!");
            var ys = new[] { 1, 2, 3 }.ToAsyncEnumerable().Count(new Func<int, bool>(x => { throw ex; }));
            AssertThrows<Exception>(() => ys.Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }
    }
}

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
    public class ForEachAsync : AsyncEnumerableTests
    {
        [Fact]
        public async Task ForEachAsync_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ForEachAsync<int>(default, x => { }));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ForEachAsync<int>(Return42, default(Action<int>)));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ForEachAsync<int>(default, (x, i) => { }));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ForEachAsync<int>(Return42, default(Action<int, int>)));

            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ForEachAsync<int>(default, x => { }, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ForEachAsync<int>(Return42, default(Action<int>), CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ForEachAsync<int>(default, (x, i) => { }, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() => AsyncEnumerable.ForEachAsync<int>(Return42, default(Action<int, int>), CancellationToken.None));
        }

        [Fact]
        public void ForEachAsync1()
        {
            var sum = 0;
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();

            xs.ForEachAsync(x => sum += x).Wait(WaitTimeoutMs);
            Assert.Equal(10, sum);
        }

        [Fact]
        public void ForEachAsync2()
        {
            var sum = 0;
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();

            xs.ForEachAsync((x, i) => sum += x * i).Wait(WaitTimeoutMs);
            Assert.Equal(1 * 0 + 2 * 1 + 3 * 2 + 4 * 3, sum);
        }

        [Fact]
        public void ForEachAsync3()
        {
            var ex = new Exception("Bang");
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();

            AssertThrows<Exception>(() => xs.ForEachAsync(x => { throw ex; }).Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void ForEachAsync4()
        {
            var ex = new Exception("Bang");
            var xs = new[] { 1, 2, 3, 4 }.ToAsyncEnumerable();

            AssertThrows<Exception>(() => xs.ForEachAsync((x, i) => { throw ex; }).Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void ForEachAsync5()
        {
            var ex = new Exception("Bang");
            var xs = Throw<int>(ex);

            AssertThrows<Exception>(() => xs.ForEachAsync(x => { throw ex; }).Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }

        [Fact]
        public void ForEachAsync6()
        {
            var ex = new Exception("Bang");
            var xs = Throw<int>(ex);

            AssertThrows<Exception>(() => xs.ForEachAsync((x, i) => { throw ex; }).Wait(WaitTimeoutMs), ex_ => ((AggregateException)ex_).Flatten().InnerExceptions.Single() == ex);
        }
    }
}
